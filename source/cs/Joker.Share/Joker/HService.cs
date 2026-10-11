using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Joker;

public class HService : AService
{
	internal enum DelayOp
	{
		Connect = 1,
		Disconnect,
		Recv,
		Send
	}

	internal struct DelayEvent
	{
		public DelayOp Op;

		public HChannel Channel;
	}

	internal class ServiceEntry
	{
		public AService Service { get; }

		public int DelayMs { get; }

		public ServiceEntry(AService service, int delayMs)
		{
			Service = service;
			DelayMs = delayMs;
		}
	}

	internal class ConnectionStatus
	{
		public ServiceEntry Entry;

		public long ChannelID;

		public bool Connected;

		public long ArrivalTimeMs;
	}

	public const int kDefaultTcpDelayMS = 500;

	public const int kDefaultUdpDelayMS = 0;

	public static readonly byte[] kGUIDMagicNumber = Encoding.ASCII.GetBytes("DCN-GUID");

	internal readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	internal readonly List<ServiceEntry> _services = new List<ServiceEntry>();

	internal readonly Dictionary<long, HChannel> _subChannels = new Dictionary<long, HChannel>();

	internal readonly Dictionary<long, HChannel> _channels = new Dictionary<long, HChannel>();

	internal readonly Dictionary<Guid, HChannel> _guidChannels = new Dictionary<Guid, HChannel>();

	internal readonly Dictionary<long, AService> _waittingHandleShakeChannels = new Dictionary<long, AService>();

	internal readonly Queue<DelayEvent> _events = new Queue<DelayEvent>();

	internal byte[] _guidReadBuffer = new byte[kGUIDMagicNumber.Length + 16];

	internal long _maxDelayMs;

	public HService()
	{
		AddServiceAndSetupCallbacks(new TService(), 500);
		AddServiceAndSetupCallbacks(new UService(), 0);
	}

	public HService(IPEndPoint address)
	{
		AddServiceAndSetupCallbacks(new TService(address), 500);
		AddServiceAndSetupCallbacks(new UService(address), 0);
	}

	public override long Create(IPEndPoint address)
	{
		long num = AService.CreateChannelId();
		Create(num, address, Guid.NewGuid(), ChannelType.Connect);
		return num;
	}

	private HChannel Create(long id, IPEndPoint address, Guid guid, ChannelType type)
	{
		HChannel hChannel = new HChannel(id, address, this, guid, type);
		_channels.Add(id, hChannel);
		_guidChannels.Add(guid, hChannel);
		foreach (ServiceEntry service in _services)
		{
			long num = hChannel.CreateSubChannel(service);
			if (num >= 0 && type == ChannelType.Connect)
			{
				_subChannels.Add(num, hChannel);
			}
		}
		return hChannel;
	}

	public override void Send(long channelId, MemoryBuffer memoryBuffer)
	{
		if (_channels.TryGetValue(channelId, out var value))
		{
			value.SendReliable(memoryBuffer);
		}
	}

	public override void SendUnreliable(long channelId, MemoryBuffer memoryBuffer)
	{
		if (_channels.TryGetValue(channelId, out var value))
		{
			value.SendUnreliable(memoryBuffer);
		}
	}

	public override long GetChannelId(IPEndPoint address)
	{
		foreach (KeyValuePair<long, HChannel> channel in _channels)
		{
			if (channel.Value.RemoteAddress.Equals(address))
			{
				return channel.Key;
			}
		}
		return -1L;
	}

	public override bool IsValidChannel(long channelId)
	{
		if (!_channels.TryGetValue(channelId, out var value))
		{
			return false;
		}
		return !value.IsDisposed;
	}

	public override IPEndPoint GetChannelAddress(long channelId)
	{
		if (_channels.TryGetValue(channelId, out var value))
		{
			return value.RemoteAddress;
		}
		return null;
	}

	public override void Remove(long id, int error = 0)
	{
		if (_channels.TryGetValue(id, out var value))
		{
			value.Close(error);
			_events.Enqueue(new DelayEvent
			{
				Op = DelayOp.Disconnect,
				Channel = value
			});
		}
	}

	private void UpdateDelayChannelEvents()
	{
		while (_events.Count > 0)
		{
			DelayEvent delayEvent = _events.Dequeue();
			HChannel channel = delayEvent.Channel;
			if (channel == null || !_channels.ContainsKey(channel.Id))
			{
				continue;
			}
			if (delayEvent.Op == DelayOp.Connect)
			{
				if (!_channels.ContainsKey(channel.Id))
				{
					_channels.Add(channel.Id, channel);
				}
				if (!_guidChannels.ContainsKey(channel.Guid))
				{
					_guidChannels.Add(channel.Guid, channel);
				}
				foreach (KeyValuePair<AService, ConnectionStatus> connection in channel.Connections)
				{
					if (connection.Value.ChannelID >= 0 && connection.Value.ChannelID != channel.ActiveChannelId && _subChannels.ContainsKey(connection.Value.ChannelID))
					{
						_subChannels.Remove(connection.Value.ChannelID);
					}
				}
				channel.OnConnectionCallback();
			}
			else if (delayEvent.Op == DelayOp.Disconnect)
			{
				if (_channels.ContainsKey(channel.Id))
				{
					_channels.Remove(channel.Id);
				}
				if (_guidChannels.ContainsKey(channel.Guid))
				{
					_guidChannels.Remove(channel.Guid);
				}
				foreach (KeyValuePair<AService, ConnectionStatus> connection2 in channel.Connections)
				{
					if (connection2.Value.ChannelID >= 0 && _subChannels.ContainsKey(connection2.Value.ChannelID))
					{
						_subChannels.Remove(connection2.Value.ChannelID);
					}
				}
				channel.Dispose();
			}
			else if (delayEvent.Op == DelayOp.Recv)
			{
				channel.FlushPendingReceives();
			}
			else if (delayEvent.Op == DelayOp.Send)
			{
				channel.FlushPendingSends();
			}
		}
	}

	public override void Update()
	{
		foreach (ServiceEntry service in _services)
		{
			service.Service.Update();
		}
		foreach (HChannel value in _channels.Values)
		{
			value.Update();
		}
		UpdateDelayChannelEvents();
	}

	public override void Dispose()
	{
		foreach (ServiceEntry service in _services)
		{
			service.Service?.Dispose();
		}
		_services.Clear();
		_channels.Clear();
		_guidChannels.Clear();
		_subChannels.Clear();
		_events.Clear();
		_waittingHandleShakeChannels.Clear();
	}

	public override bool IsDisposed()
	{
		return _services.All((ServiceEntry s) => s.Service.IsDisposed());
	}

	private void AddServiceAndSetupCallbacks(AService service, int delayMs)
	{
		ServiceEntry entry = new ServiceEntry(service, delayMs);
		_services.Add(entry);
		if (delayMs > _maxDelayMs)
		{
			_maxDelayMs = delayMs;
		}
		service.AcceptCallback = delegate(long subChannelId, IPEndPoint addr)
		{
			_waittingHandleShakeChannels.Add(subChannelId, service);
		};
		service.ConnectCallback = delegate(long subChannelId)
		{
			FindHybridChannelBySubId(subChannelId, service).SendHandShakeOnClientConnected(service, subChannelId);
		};
		service.ReadCallback = delegate(long subChannelId, MemoryBuffer data)
		{
			HChannel hChannel = FindHybridChannelBySubId(subChannelId, service);
			if (hChannel == null)
			{
				if (_waittingHandleShakeChannels.ContainsKey(subChannelId))
				{
					_waittingHandleShakeChannels.Remove(subChannelId);
				}
				long position = data.Position;
				bool flag = false;
				if (data.Length - data.Position == _guidReadBuffer.Length)
				{
					data.Read(_guidReadBuffer, 0, _guidReadBuffer.Length);
					flag = MemoryExtensions.AsSpan(_guidReadBuffer, 0, kGUIDMagicNumber.Length).SequenceEqual(kGUIDMagicNumber);
					if (!flag)
					{
						data.Seek(position, SeekOrigin.Begin);
					}
				}
				if (flag)
				{
					byte[] b = MemoryExtensions.AsSpan(_guidReadBuffer, kGUIDMagicNumber.Length).ToArray();
					Guid guid = new Guid(b);
					hChannel = FindHybridChannelByGuid(guid);
					if (hChannel == null)
					{
						IPEndPoint channelAddress = service.GetChannelAddress(subChannelId);
						hChannel = Create(AService.CreateChannelId(), channelAddress, guid, ChannelType.Accept);
					}
					hChannel.ConnectedSubChannel(entry, subChannelId);
					_subChannels.Add(subChannelId, hChannel);
				}
				else
				{
					Guid guid = Guid.NewGuid();
					IPEndPoint channelAddress2 = service.GetChannelAddress(subChannelId);
					hChannel = Create(AService.CreateChannelId(), channelAddress2, guid, ChannelType.Accept);
					hChannel.ConnectedSubChannel(entry, subChannelId);
					_subChannels.Add(subChannelId, hChannel);
					hChannel.ConfirmedConnection(subChannelId, service);
					hChannel.ReadData(data);
				}
			}
			else
			{
				hChannel.ReadData(data);
			}
		};
		service.ErrorCallback = delegate(long subChannelId, int error)
		{
			if (_waittingHandleShakeChannels.ContainsKey(subChannelId))
			{
				_waittingHandleShakeChannels.Remove(subChannelId);
			}
			HChannel hChannel = FindHybridChannelBySubId(subChannelId, service);
			if (hChannel != null)
			{
				hChannel.RemoveSubChannel(subChannelId, service);
				if (hChannel.ActiveChannelId == subChannelId)
				{
					hChannel.Error = error;
					Remove(hChannel.Id, error);
				}
			}
		};
		service.DisconnectCallback = delegate(long subChannelId)
		{
			if (_waittingHandleShakeChannels.ContainsKey(subChannelId))
			{
				_waittingHandleShakeChannels.Remove(subChannelId);
			}
			HChannel hChannel = FindHybridChannelBySubId(subChannelId, service);
			if (hChannel != null)
			{
				hChannel.RemoveSubChannel(subChannelId, service);
				if (hChannel.ActiveChannelId == subChannelId)
				{
					Remove(hChannel.Id);
				}
			}
		};
	}

	private HChannel FindHybridChannelBySubId(long subId, AService service)
	{
		if (_subChannels.TryGetValue(subId, out var value))
		{
			return value;
		}
		return null;
	}

	private HChannel FindHybridChannelByGuid(Guid guid)
	{
		if (_guidChannels.TryGetValue(guid, out var value))
		{
			return value;
		}
		return null;
	}
}
