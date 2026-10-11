using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Joker;

public sealed class UService : AService
{
	public enum UChannelType : byte
	{
		Reliable = 1,
		Unreliable,
		ReliableFEC
	}

	internal class ResendRequest
	{
		public long Time;

		public long TimeInterval;

		public int LeftResendNum;

		public int Length;

		public readonly byte[] Data = new byte[1400];

		public void Clear()
		{
			Time = 0L;
			TimeInterval = 0L;
			Length = 0;
			LeftResendNum = 0;
		}
	}

	public class UConfig
	{
		public int RecvBufferSize;

		public int SendBufferSize;

		public int Mtu;

		public bool NoDelay;

		public int Interval;

		public int FastResend;

		public bool CongestionWindow;

		public int SendWindowSize;

		public int ReceiveWindowSize;

		public int Timeout;

		public int MaxRetransmits;

		public int HandshakeInterval;

		public int FECResendTimes;

		public int FECResendInterval;

		public int FecResendSizeLimit;

		public int ResendRequestPoolLimit;

		public int WaitDisposeTime;

		public bool ManualSplitLongData;

		public UConfig(int recvBufferSize = 7340032, int sendBufferSize = 7340032, int mtu = 470, bool noDelay = true, int interval = 6, int fastResend = 3, bool congestionWindow = true, int sendWindowSize = 32, int receiveWindowSize = 128, int timeout = 5000, int maxRetransmits = 20, int handshakeInterval = 50, int waitDisposeTime = 50, int fecResendTimes = 2, int fecResendInterval = 6, int fecResendSizeLimit = 150, int resendRequestPoolLimit = 100, bool manualSplitLongData = false)
		{
			RecvBufferSize = recvBufferSize;
			SendBufferSize = sendBufferSize;
			Mtu = mtu;
			NoDelay = noDelay;
			Interval = interval;
			FastResend = fastResend;
			CongestionWindow = congestionWindow;
			SendWindowSize = sendWindowSize;
			ReceiveWindowSize = receiveWindowSize;
			Timeout = timeout;
			MaxRetransmits = maxRetransmits;
			HandshakeInterval = handshakeInterval;
			WaitDisposeTime = waitDisposeTime;
			FECResendTimes = fecResendTimes;
			FECResendInterval = fecResendInterval;
			FecResendSizeLimit = fecResendSizeLimit;
			ResendRequestPoolLimit = resendRequestPoolLimit;
			ManualSplitLongData = manualSplitLongData;
		}
	}

	public const int kDefaultSendBufferSize = 8388608;

	public const int kDefaultReceiveBufferSize = 8388608;

	public const int kDefaultTimeout = 5000;

	public const int kDefaultPingInterval = 2000;

	public const int kDefaultQueueDisconnectThreshold = 10000;

	private Socket _acceptor;

	private EndPoint _receiveEndPoint;

	private bool _isDisposed;

	private readonly byte[] _udpRecvBuffer;

	private readonly Dictionary<IPEndPoint, UChannel> _ep2channels = new Dictionary<IPEndPoint, UChannel>();

	private readonly Dictionary<long, UChannel> _id2channels = new Dictionary<long, UChannel>();

	private readonly List<UChannel> _channels = new List<UChannel>();

	private readonly List<UChannel> _waitDisposeList = new List<UChannel>();

	private readonly Stack<ResendRequest> _resendRequestPool = new Stack<ResendRequest>();

	internal UConfig Config { get; private set; }

	public UService()
	{
		Config = new UConfig();
		_udpRecvBuffer = new byte[Config.Mtu];
	}

	public UService(IPEndPoint endpoint, UConfig config = null)
	{
		Config = config ?? new UConfig();
		_udpRecvBuffer = new byte[Config.Mtu];
		_acceptor = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
		_acceptor.Blocking = false;
		_receiveEndPoint = new IPEndPoint(IPAddress.Any, endpoint.Port);
		ConfigureSioConnReset(_acceptor);
		ConfigureSocketBuffers(_acceptor);
		_acceptor.Bind(_receiveEndPoint);
	}

	public override void Remove(long id, int error = 0)
	{
		if (_id2channels.TryGetValue(id, out var value))
		{
			value.Error = error;
			_id2channels.Remove(id);
			_channels.RemoveSwapBack(value);
			if (_ep2channels.ContainsKey(value.RemoteAddress))
			{
				_ep2channels.Remove(value.RemoteAddress);
			}
			value.Disconnect();
			_waitDisposeList.Add(value);
		}
	}

	public override bool IsDisposed()
	{
		return _isDisposed;
	}

	public override void Dispose()
	{
		if (IsDisposed())
		{
			return;
		}
		_isDisposed = true;
		_acceptor?.Close();
		_acceptor = null;
		UChannel[] array = _id2channels.Values.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Dispose();
		}
		foreach (UChannel waitDispose in _waitDisposeList)
		{
			waitDispose.Dispose();
		}
		_channels.Clear();
		_id2channels.Clear();
		_ep2channels.Clear();
	}

	public void Create(long id, IPEndPoint ipEndPoint)
	{
		if (!_id2channels.ContainsKey(id))
		{
			UConnectChannel uConnectChannel = new UConnectChannel(id, ipEndPoint, this);
			_channels.Add(uConnectChannel);
			_id2channels.Add(id, uConnectChannel);
			_ep2channels.Add(uConnectChannel.RemoteAddress, uConnectChannel);
			uConnectChannel.SendConnect();
		}
	}

	public override long Create(IPEndPoint ipEndPoint)
	{
		long num = AService.CreateChannelId();
		Create(num, ipEndPoint);
		return num;
	}

	public override void Send(long channelId, MemoryBuffer memoryBuffer)
	{
		if (_id2channels.TryGetValue(channelId, out var value))
		{
			value.SendReliable(UMessageReliable.Data, memoryBuffer);
		}
	}

	public override void SendUnreliable(long channelId, MemoryBuffer memoryBuffer)
	{
		if (_id2channels.TryGetValue(channelId, out var value))
		{
			value.SendUnreliable(UMessageUnreliable.Data, memoryBuffer);
		}
	}

	public override long GetChannelId(IPEndPoint address)
	{
		if (_ep2channels.TryGetValue(address, out var value))
		{
			return value.Id;
		}
		return -1L;
	}

	public override IPEndPoint GetChannelAddress(long channelId)
	{
		if (_id2channels.TryGetValue(channelId, out var value))
		{
			return value.RemoteAddress;
		}
		return null;
	}

	public override bool IsValidChannel(long channelId)
	{
		if (!_id2channels.TryGetValue(channelId, out var value))
		{
			return false;
		}
		return !value.IsDisposed;
	}

	public override void Update()
	{
		UpdateIncoming();
		UpdateOutgoing();
	}

	private void UpdateIncoming()
	{
		while (ReceiveChannelMessage())
		{
		}
		for (int num = _channels.Count - 1; num >= 0; num--)
		{
			_channels[num].TickIncoming();
		}
		for (int num2 = _waitDisposeList.Count - 1; num2 >= 0; num2--)
		{
			if (_waitDisposeList[num2].CanSafeDispose)
			{
				_waitDisposeList[num2].Dispose();
				_waitDisposeList.RemoveAt(num2);
			}
		}
	}

	private void UpdateOutgoing()
	{
		for (int num = _channels.Count - 1; num >= 0; num--)
		{
			_channels[num].TickOutgoing();
		}
	}

	internal void OnAcceptComplete(UChannel channel)
	{
		AcceptCallback?.Invoke(channel.Id, channel.RemoteAddress);
	}

	internal void OnConnectComplete(UChannel channel)
	{
		ConnectCallback?.Invoke(channel.Id);
	}

	internal void OnMessageComplete(UChannel channel, MemoryBuffer buffer)
	{
		ReadCallback?.Invoke(channel.Id, buffer);
	}

	internal void OnDisconnectComplete(UChannel channel)
	{
		Remove(channel.Id);
		DisconnectCallback?.Invoke(channel.Id);
	}

	internal void OnErrorComplete(UChannel channel, int error)
	{
		ErrorCallback?.Invoke(channel.Id, error);
	}

	private bool ReceiveChannelMessage()
	{
		if (!ReceiveRawData(out var buffer, out var len, ref _receiveEndPoint))
		{
			return false;
		}
		if (!(_receiveEndPoint is IPEndPoint iPEndPoint))
		{
			Log.Error($"ReceiveChannelMessage failed: {_receiveEndPoint} is not IPEndPoint.");
			return true;
		}
		if (!_ep2channels.TryGetValue(iPEndPoint, out var value))
		{
			if (IsWaitForDispose(iPEndPoint))
			{
				return true;
			}
			long num = AService.CreateChannelId();
			value = new UAcceptChannel(num, _acceptor, iPEndPoint, this);
			value.RawInput(buffer, len);
			if (value.IsConnected)
			{
				_channels.Add(value);
				_ep2channels.Add(iPEndPoint, value);
				_id2channels.Add(num, value);
			}
			else
			{
				value.Dispose();
			}
		}
		else
		{
			value.RawInput(buffer, len);
		}
		return true;
	}

	private bool ReceiveRawData(out byte[] buffer, out int len, ref EndPoint ep)
	{
		buffer = null;
		len = 0;
		if (_acceptor == null)
		{
			return false;
		}
		try
		{
			if (_acceptor.ReceiveFromNonBlocking(_udpRecvBuffer, out len, ref ep))
			{
				buffer = _udpRecvBuffer;
				return true;
			}
		}
		catch (SocketException arg)
		{
			Log.Error($"[UDP] Server: ReceiveFrom failed: {arg}");
		}
		return false;
	}

	private bool IsWaitForDispose(IPEndPoint ep)
	{
		for (int i = 0; i < _waitDisposeList.Count; i++)
		{
			if (_waitDisposeList[i].RemoteAddress.Equals(ep))
			{
				return true;
			}
		}
		return false;
	}

	internal ResendRequest FetchResendRequest()
	{
		if (_resendRequestPool.Count != 0)
		{
			return _resendRequestPool.Pop();
		}
		return new ResendRequest();
	}

	internal void RecycleResendRequest(ResendRequest resendRequest)
	{
		if (_resendRequestPool.Count <= Config.ResendRequestPoolLimit)
		{
			resendRequest.Clear();
			_resendRequestPool.Push(resendRequest);
		}
	}

	public static void ConfigureSocketBuffers(Socket socket)
	{
		try
		{
			socket.SendBufferSize = 8388608;
			socket.ReceiveBufferSize = 8388608;
		}
		catch (SocketException)
		{
			Log.Warning($"[UCP] failed to set Socket RecvBufSize = {8388608} SendBufSize = {8388608}");
		}
	}

	public static void ConfigureSioConnReset(Socket socket)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			socket.IOControl(-1744830452, new byte[1] { Convert.ToByte(value: false) }, null);
		}
	}
}
