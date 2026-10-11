using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Joker;

public abstract class UChannel : NetworkChannel
{
	public enum EState
	{
		Connected,
		Authenticated,
		Disconnected
	}

	protected readonly byte[] _udpSendBuffer;

	protected readonly byte[] _udpRecvBuffer;

	protected readonly MemoryBuffer _ppBuffer = new MemoryBuffer(8);

	internal readonly List<UService.ResendRequest> _kcpFECResendList = new List<UService.ResendRequest>();

	protected MemoryBuffer _splitDataBuffer;

	protected UService _service;

	protected Socket _socket;

	protected Kcp _kcp;

	protected EState _state;

	protected long _lastReceiveTime;

	protected long _nextSendConnectTime;

	protected long _retrySendConnectCount;

	protected long _disposeTime;

	protected long _lastPingTime;

	protected readonly Stopwatch _watch = new Stopwatch();

	protected Queue<MemoryBuffer> _waitConnectionDataQueue = new Queue<MemoryBuffer>();

	protected UFECData _fec = new UFECData(4096);

	private int times;

	public uint MaxSendRate => _kcp.SendWindowSize * _kcp.MaximumTransmissionUnit * 1000 / _kcp.Interval;

	public uint MaxReceiveRate => _kcp.ReceiveWindowSize * _kcp.MaximumTransmissionUnit * 1000 / _kcp.Interval;

	public int RttInMilliseconds => _kcp?.RxSrtt ?? 460;

	public int Timeout => _service.Config.Timeout;

	public long Time => _watch.ElapsedMilliseconds;

	public bool IsConnected => _state == EState.Authenticated;

	public bool CanSafeDispose
	{
		get
		{
			if (_state == EState.Disconnected)
			{
				return Time >= _disposeTime;
			}
			return false;
		}
	}

	internal int UnreliableMax => _service.Config.Mtu - 5;

	public UChannel(long id, IPEndPoint endpoint, UService service)
	{
		base.Id = id;
		_service = service;
		base.RemoteAddress = endpoint;
		_state = EState.Connected;
		_udpSendBuffer = new byte[_service.Config.Mtu];
		_udpRecvBuffer = new byte[_service.Config.Mtu];
	}

	public override void Dispose()
	{
		if (!base.IsDisposed)
		{
			long id = base.Id;
			Disconnect();
			while (_waitConnectionDataQueue.Count > 0)
			{
				MemoryBuffer memoryBuffer = _waitConnectionDataQueue.Dequeue();
				_service?.Recycle(memoryBuffer);
			}
			_service?.Remove(id);
			_kcp?.Dispose();
			if (base.ChannelType == ChannelType.Connect)
			{
				_socket?.Close();
				_socket?.Dispose();
			}
			_socket = null;
			_kcp = null;
			base.Id = 0L;
		}
	}

	protected void InitKCP(uint conv)
	{
		UService.UConfig config = _service.Config;
		_lastReceiveTime = 0L;
		_lastPingTime = 0L;
		_watch.Restart();
		_kcp?.Dispose();
		_kcp = new Kcp(conv, KcpOutput);
		_kcp.SetNoDelay(config.NoDelay ? 1 : 0, config.Interval, config.FastResend, (!config.CongestionWindow) ? 1 : 0);
		_kcp.SetWindowSize(config.SendWindowSize, config.ReceiveWindowSize);
		_kcp.SetMtu(config.Mtu - 5 - 6);
		_kcp.SetDeadLink(config.MaxRetransmits);
	}

	protected abstract bool OnAuthenticated(MemoryBuffer buffer);

	protected virtual void OnData(MemoryBuffer buffer, UService.UChannelType channelType)
	{
		_service?.OnMessageComplete(this, buffer);
	}

	protected abstract void OnDisconnected();

	protected virtual void OnError(NetworkError error, string message)
	{
		Log.Warning(message);
		_service?.OnErrorComplete(this, (int)error);
	}

	protected virtual void HandleTimeout(long time)
	{
		if (time >= _lastReceiveTime + Timeout)
		{
			OnError(NetworkError.Timeout, $"{GetType()}: Connection timeout after not receiving any message for {Timeout} ms. Disconnecting conv={_kcp?.ConversationId} {base.RemoteAddress?.ToString()}.");
			Disconnect();
		}
	}

	protected virtual void HandleDeadLink()
	{
		if (_kcp != null && _kcp.State == -1)
		{
			OnError(NetworkError.Timeout, $"{GetType()}: dead_link detected: a message was retransmitted {_kcp.DeadLink} times without ack. Disconnecting.");
			Disconnect();
		}
	}

	protected virtual void HandlePing(long time)
	{
		if (_kcp != null && time >= _lastPingTime + 2000)
		{
			SendPing();
			_lastPingTime = time;
		}
	}

	protected virtual void HandleChoked()
	{
		if (_kcp != null)
		{
			int num = (int)(_kcp.ReceiveQueueCount + _kcp.SendQueueCount + _kcp.ReceiveBufferCount + _kcp.SendBufferCount);
			if (num > 10000)
			{
				OnError(NetworkError.KcpWaitSendSizeTooLarge, $"{GetType()}: disconnecting connection because it can't process data fast enough.\n" + $"Queue total {num}>{10000}. rcv_queue={_kcp.ReceiveQueueCount} snd_queue={_kcp.SendQueueCount} rcv_buf={_kcp.ReceiveBufferCount} snd_buf={_kcp.SendBufferCount}\n" + "* Try to Enable NoDelay, decrease INTERVAL, disable Congestion Window (= enable NOCWND!), increase SEND/RECV WINDOW or compress data.\n* Or perhaps the network is simply too slow on our end, or on the other end.");
				Disconnect();
			}
		}
	}

	protected virtual void HandleRetryConnect()
	{
		if (_state == EState.Connected && Time >= _nextSendConnectTime)
		{
			SendConnect();
		}
	}

	protected bool ReceiveNextReliable(out UMessageReliable message, out MemoryBuffer buffer)
	{
		buffer = null;
		message = UMessageReliable.Invalid;
		if (_kcp == null)
		{
			return false;
		}
		int num = _kcp.PeekSize();
		if (num <= 0)
		{
			return false;
		}
		buffer = _service.Fetch(num);
		buffer.SetLength(num);
		buffer.Seek(0L, SeekOrigin.Begin);
		int num2 = _kcp.Receive(buffer.GetBuffer(), num);
		if (num2 < 0)
		{
			message = UMessageReliable.Invalid;
			OnError(NetworkError.TChannelRecvError, $"{GetType()}: Receive failed with error={num2}. closing connection.");
			Disconnect();
			return false;
		}
		buffer = ParseKcpReceiveData(out message, buffer);
		if (buffer == null)
		{
			return false;
		}
		_lastReceiveTime = Time;
		return true;
	}

	protected void TickIncoming_Connected(long time)
	{
		HandleRetryConnect();
		HandleTimeout(time);
		HandleDeadLink();
		HandlePing(time);
		HandleChoked();
		UMessageReliable message;
		MemoryBuffer buffer;
		while (ReceiveNextReliable(out message, out buffer))
		{
			try
			{
				int num = (int)(buffer.Length - buffer.Position);
				if (message == UMessageReliable.Data && num > 0)
				{
					_service.Fetch(num).Write(buffer.GetBuffer(), (int)buffer.Position, (int)(buffer.Length - buffer.Position));
					_waitConnectionDataQueue.Enqueue(buffer);
				}
			}
			catch (Exception arg)
			{
				Log.Error($"[UDP] ReceiveNextReliable with exception: {arg}");
			}
			finally
			{
				if (buffer != null)
				{
					_service.Recycle(buffer);
				}
			}
		}
	}

	protected void TickIncoming_Authenticated(long time)
	{
		HandleTimeout(time);
		HandleDeadLink();
		HandlePing(time);
		HandleChoked();
		while (_waitConnectionDataQueue.Count > 0)
		{
			MemoryBuffer memoryBuffer = _waitConnectionDataQueue.Dequeue();
			OnData(memoryBuffer, UService.UChannelType.Reliable);
			_service?.Recycle(memoryBuffer);
		}
		UMessageReliable message;
		MemoryBuffer buffer;
		while (ReceiveNextReliable(out message, out buffer))
		{
			try
			{
				int num = (int)(buffer.Length - buffer.Position);
				switch (message)
				{
				case UMessageReliable.Data:
					if (num > 0)
					{
						OnData(buffer, UService.UChannelType.Reliable);
					}
					else
					{
						Log.Error($"{GetType()}: conv={_kcp?.ConversationId} received empty Data message in Authenticated. ");
					}
					break;
				case UMessageReliable.Ping:
					SendPong();
					break;
				case UMessageReliable.Pong:
					break;
				}
			}
			catch (Exception arg)
			{
				Log.Error($"[UDP] ReceiveNextReliable with exception: {arg}");
			}
			finally
			{
				if (buffer != null)
				{
					_service.Recycle(buffer);
				}
			}
		}
	}

	public virtual void TickIncoming()
	{
		try
		{
			switch (_state)
			{
			case EState.Connected:
				TickIncoming_Connected(Time);
				break;
			case EState.Authenticated:
				TickIncoming_Authenticated(Time);
				break;
			case EState.Disconnected:
				break;
			}
		}
		catch (SocketException arg)
		{
			OnError(NetworkError.SocketError, $"{GetType()}: Disconnecting because {arg}. This is fine.");
			Disconnect();
		}
		catch (ObjectDisposedException arg2)
		{
			OnError(NetworkError.PeerDisconnect, $"{GetType()}: Disconnecting because {arg2}. This is fine.");
			Disconnect();
		}
		catch (Exception arg3)
		{
			OnError(NetworkError.Exception, $"{GetType()}: unexpected Exception: {arg3}");
			Disconnect();
		}
	}

	public virtual void TickOutgoing()
	{
		if (_kcp == null)
		{
			return;
		}
		try
		{
			switch (_state)
			{
			case EState.Connected:
			case EState.Authenticated:
				TickOutgoing_Authenticated(Time);
				break;
			}
		}
		catch (SocketException arg)
		{
			OnError(NetworkError.SocketError, $"{GetType()}: Disconnecting because {arg}. This is fine.");
			Disconnect();
		}
		catch (ObjectDisposedException arg2)
		{
			OnError(NetworkError.PeerDisconnect, $"{GetType()}: Disconnecting because {arg2}. This is fine.");
			Disconnect();
		}
		catch (Exception arg3)
		{
			OnError(NetworkError.SocketError, $"{GetType()}: unexpected exception: {arg3}");
			Disconnect();
		}
	}

	internal virtual void TickOutgoing_Authenticated(long time)
	{
		_kcp.Update((uint)Time);
		for (int num = _kcpFECResendList.Count - 1; num >= 0; num--)
		{
			UService.ResendRequest resendRequest = _kcpFECResendList[num];
			if (resendRequest.Time < Time)
			{
				resendRequest.Time = Time + resendRequest.TimeInterval;
				resendRequest.LeftResendNum--;
				try
				{
					_socket.SendToNonBlocking(resendRequest.Data, 0, resendRequest.Length, base.RemoteAddress);
				}
				catch (Exception arg)
				{
					Log.Error($"[UDP] SendToNonBlocking with exception: {arg}");
				}
				finally
				{
					if (resendRequest.LeftResendNum <= 0)
					{
						_service.RecycleResendRequest(resendRequest);
						_kcpFECResendList.RemoveSwapBackAt(num);
					}
				}
			}
		}
	}

	internal virtual void RawInput(byte[] buffer, int len)
	{
		if (len < 5 || _state == EState.Disconnected)
		{
			return;
		}
		UtilsEncode.Decode32U(buffer, 0, out var value);
		if (value != UHeader.kMagicNumber)
		{
			Log.Warning($"[UDP] received message with invalid magic number {value}!={UHeader.kMagicNumber}. ignore...");
			return;
		}
		UtilsEncode.Decode8U(buffer, 4, out var value2);
		switch (value2)
		{
		case 1:
			OnRawInputReliable(buffer, 5, len - 5);
			break;
		case 2:
			OnRawInputUnreliable(buffer, 5, len - 5);
			break;
		case 3:
			OnRawInputReliableFEC(buffer, 5, len - 5);
			break;
		default:
			Log.Warning($"[UDP] Client: invalid channel header: {value2}, likely internet noise");
			break;
		}
	}

	protected void OnRawInputReliable(byte[] buffer, int offset, int len)
	{
		if (_kcp == null)
		{
			Log.Warning("[UDP] RawInputReliable called with invalid state: kcp==null");
			return;
		}
		int num = _kcp.Input(buffer, offset, len);
		if (num != 0)
		{
			Log.Warning($"[UDP] {GetType()}: Input failed with error={num} for buffer with length={len}");
		}
	}

	protected void OnRawInputReliableFEC(byte[] buffer, int offset, int len)
	{
		if (_fec.Unpack(buffer, offset))
		{
			OnRawInputReliable(buffer, offset + 6, len - 6);
		}
	}

	protected void OnRawInputUnreliable(byte[] buffer, int offset, int len)
	{
		if (len < 1)
		{
			return;
		}
		UtilsEncode.Decode8U(buffer, offset, out var value);
		if (!UHeader.TryParseUnreliable(value, out var message))
		{
			OnError(NetworkError.MessageSocketParserError, $"{GetType()}: Receive failed to parse header: {value} is not defined in {typeof(UMessageUnreliable)}.");
			Disconnect();
			return;
		}
		len--;
		MemoryBuffer memoryBuffer = _service.Fetch(len);
		memoryBuffer.Write(buffer, offset + 1, len);
		memoryBuffer.Seek(0L, SeekOrigin.Begin);
		try
		{
			switch (message)
			{
			case UMessageUnreliable.Connect:
				if (_state == EState.Connected && OnAuthenticated(memoryBuffer))
				{
					_lastReceiveTime = Time;
					Log.Info($"[UDP] {GetType()}: received connect with conv={_kcp?.ConversationId}");
				}
				break;
			case UMessageUnreliable.Disconnect:
				if (_state == EState.Authenticated)
				{
					OnDisconnect(memoryBuffer);
				}
				break;
			case UMessageUnreliable.Data:
				if (_state == EState.Authenticated)
				{
					OnData(memoryBuffer, UService.UChannelType.Unreliable);
					_lastReceiveTime = Time;
				}
				break;
			}
		}
		catch (Exception arg)
		{
			Log.Error($"[UDP] unexpected exception: {arg}");
		}
		finally
		{
			_service.Recycle(memoryBuffer);
		}
	}

	protected void OnDisconnect(MemoryBuffer buffer)
	{
		if (buffer.Length < 4)
		{
			Log.Info("[UDP] OnDisconnect received with invalid size.igonre!");
		}
		if (_kcp == null)
		{
			Log.Info("[UDP] OnDisconnect when kcp is null.ignore!");
			return;
		}
		uint num = buffer.Read<uint>();
		if (_kcp.ConversationId != num)
		{
			Log.Info($"[UDP] OnDisconnect when kcp.ConversationId {_kcp?.ConversationId} != conv {num}");
			return;
		}
		Log.Info($"[UDP] {GetType()} conv={_kcp?.ConversationId}: received disconnect message");
		Disconnect();
	}

	protected void KcpOutput(byte[] data, int length)
	{
		if (_socket == null)
		{
			return;
		}
		try
		{
			if (!TrySendFEC(data, length))
			{
				UtilsEncode.Encode32U(_udpSendBuffer, 0, UHeader.kMagicNumber);
				UtilsEncode.Encode8U(_udpSendBuffer, 4, 1);
				_udpSendBuffer.WriteTo(5, data, 0, length);
				_socket.SendToNonBlocking(_udpSendBuffer, 0, 5 + length, base.RemoteAddress);
			}
		}
		catch (Exception arg)
		{
			Log.Error($"[UDP] KcpOutput with exception: {arg}");
		}
	}

	protected bool TrySendFEC(byte[] data, int len)
	{
		if (_state != EState.Authenticated)
		{
			return false;
		}
		if (len > _service.Config.FecResendSizeLimit)
		{
			return false;
		}
		if (_service.Config.FECResendTimes < 1)
		{
			return false;
		}
		UtilsEncode.Encode32U(_udpSendBuffer, 0, UHeader.kMagicNumber);
		UtilsEncode.Encode8U(_udpSendBuffer, 4, 3);
		_fec.Pack(_udpSendBuffer, 5);
		_udpSendBuffer.WriteTo(11, data, 0, len);
		int num = 11 + len;
		_socket.SendToNonBlocking(_udpSendBuffer, 0, 11 + len, base.RemoteAddress);
		UService.ResendRequest resendRequest = _service.FetchResendRequest();
		resendRequest.Length = num;
		resendRequest.LeftResendNum = _service.Config.FECResendTimes;
		resendRequest.Time = Time + _service.Config.FECResendInterval;
		resendRequest.TimeInterval = _service.Config.FECResendInterval;
		Buffer.BlockCopy(_udpSendBuffer, 0, resendRequest.Data, 0, num);
		_kcpFECResendList.Add(resendRequest);
		return true;
	}

	internal bool TrySplitKcpSendData(int len, out int segment, out int mss)
	{
		if (!_service.Config.ManualSplitLongData || _kcp.MaximumSegmentSize >= len + 1)
		{
			segment = 1;
			mss = (int)(_kcp.MaximumSegmentSize - 1);
			return false;
		}
		int num = (int)(_kcp.MaximumSegmentSize - 5);
		segment = (len + num - 1) / num;
		mss = num;
		return true;
	}

	internal MemoryBuffer ParseKcpReceiveData(out UMessageReliable message, MemoryBuffer buffer)
	{
		int count2;
		int index;
		int count = UHeader.UnpackMessageReliableHeader(buffer.GetBuffer(), (int)buffer.Position, out message, out count2, out index);
		buffer.Advance(count);
		if (message == UMessageReliable.DataSegment)
		{
			if (index == 0)
			{
				if (_splitDataBuffer != null)
				{
					_service.Recycle(_splitDataBuffer);
					_splitDataBuffer = null;
				}
				_splitDataBuffer = _service.Fetch();
			}
			_splitDataBuffer.Write(buffer.GetBuffer(), (int)buffer.Position, buffer.DataSize);
			_service.Recycle(buffer);
			if (count2 - 1 == index)
			{
				message = UMessageReliable.Data;
				buffer = _splitDataBuffer;
				buffer.Seek(0L, SeekOrigin.Begin);
				_splitDataBuffer = null;
				return buffer;
			}
			return null;
		}
		return buffer;
	}

	internal void SendReliable(UMessageReliable message, MemoryBuffer buffer)
	{
		if (_state != EState.Authenticated)
		{
			Log.Warning($"[UDP] SendReliable called with invalid state: {_state}");
			return;
		}
		try
		{
			int offset = (int)buffer.Position;
			int num = (int)(buffer.Length - buffer.Position);
			if (TrySplitKcpSendData(num, out var segment, out var mss))
			{
				SendReliableSplit(message, buffer, offset, num, segment, mss);
			}
			else
			{
				SendReliableSimple(message, buffer, offset, num);
			}
		}
		catch (Exception arg)
		{
			Log.Warning($"[UDP] SendReliable  unexpected exception: {arg}");
		}
	}

	internal void SendReliableSimple(UMessageReliable message, MemoryBuffer buffer, int offset, int length)
	{
		byte[] array = UHeader.PackMessageReliableHeader(message);
		int num = _kcp.SendWithUserHeader(buffer.GetBuffer(), offset, length, array, array.Length);
		if (num < 0)
		{
			OnError(NetworkError.KcpSendFailed, $"{GetType()}: Send failed with error={num} for content with length={length}");
		}
	}

	internal void SendReliableSplit(UMessageReliable message, MemoryBuffer buffer, int offset, int length, int count, int mss)
	{
		if (message != UMessageReliable.Data)
		{
			OnError(NetworkError.KcpSendFailed, $"{GetType()}: Send failed with invalid message type");
			return;
		}
		for (int i = 0; i < count; i++)
		{
			byte[] array = UHeader.PackMessageReliableHeader(UMessageReliable.DataSegment, count, i);
			int num = _kcp.SendWithUserHeader(buffer.GetBuffer(), offset, Math.Min(mss, length), array, array.Length);
			if (num < 0)
			{
				OnError(NetworkError.KcpSendFailed, $"{GetType()}: Send failed with invalid split message.");
				break;
			}
			num -= array.Length;
			offset += num;
			length -= num;
		}
	}

	internal void SendUnreliable(UMessageUnreliable message, MemoryBuffer buffer)
	{
		try
		{
			int num = (int)(buffer.Length - buffer.Position);
			if (num > UnreliableMax)
			{
				Log.Error($"[UDP] {GetType()}: Failed to send unreliable message of size {num} because it's larger than UnreliableMaxMessageSize={UnreliableMax}");
				return;
			}
			UtilsEncode.Encode32U(_udpSendBuffer, 0, UHeader.kMagicNumber);
			UtilsEncode.Encode8U(_udpSendBuffer, 4, 2);
			UtilsEncode.Encode8U(_udpSendBuffer, 5, (byte)message);
			_udpSendBuffer.WriteTo(6, buffer);
			_socket.SendToNonBlocking(_udpSendBuffer, 0, 6 + num, base.RemoteAddress);
		}
		catch (Exception arg)
		{
			if (!App.IsExit)
			{
				Log.Warning($"[UDP] SendUnreliable unexpected exception: {arg}");
			}
		}
	}

	public void SendConnect()
	{
		Log.Info($"[UDP] {GetType()}: sending handshake to other end with conv={_kcp?.ConversationId}");
		MemoryBuffer memoryBuffer = _service.Fetch(4);
		memoryBuffer.Write(_kcp.ConversationId);
		memoryBuffer.Seek(0L, SeekOrigin.Begin);
		SendUnreliable(UMessageUnreliable.Connect, memoryBuffer);
		_service.Recycle(memoryBuffer);
		_nextSendConnectTime = Time + Math.Max(1L, _retrySendConnectCount - 2) * _service.Config.HandshakeInterval;
		_retrySendConnectCount++;
	}

	public void SendPing()
	{
		if (_state == EState.Authenticated)
		{
			_ppBuffer.Clear();
			_ppBuffer.Seek(0L, SeekOrigin.Begin);
			SendReliable(UMessageReliable.Ping, _ppBuffer);
		}
	}

	public void SendPong()
	{
		if (_state == EState.Authenticated)
		{
			_ppBuffer.Clear();
			_ppBuffer.Seek(0L, SeekOrigin.Begin);
			SendReliable(UMessageReliable.Pong, _ppBuffer);
		}
	}

	public void SendDisconnect()
	{
		if (_socket != null && _kcp != null && _service != null)
		{
			MemoryBuffer memoryBuffer = _service.Fetch(4);
			memoryBuffer.Write(_kcp.ConversationId);
			memoryBuffer.Seek(0L, SeekOrigin.Begin);
			for (int i = 0; i < 5; i++)
			{
				SendUnreliable(UMessageUnreliable.Disconnect, memoryBuffer);
			}
			_service.Recycle(memoryBuffer);
		}
	}

	public virtual void Disconnect()
	{
		if (_service.IsDisposed())
		{
			_socket = null;
		}
		if (_state == EState.Disconnected)
		{
			return;
		}
		_disposeTime = Time + _service.Config.WaitDisposeTime;
		try
		{
			if (_state == EState.Authenticated)
			{
				SendDisconnect();
			}
		}
		catch (SocketException)
		{
		}
		catch (ObjectDisposedException)
		{
		}
		Log.Info($"[UDP] {GetType()}: Disconnected. conv={_kcp?.ConversationId}");
		_state = EState.Disconnected;
		OnDisconnected();
	}
}
