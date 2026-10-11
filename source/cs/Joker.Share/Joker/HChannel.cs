using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Joker;

internal sealed class HChannel : NetworkChannel
{
	private readonly HService _parentService;

	private readonly Dictionary<AService, HService.ConnectionStatus> _connectionStatus = new Dictionary<AService, HService.ConnectionStatus>();

	private readonly Queue<(MemoryBuffer data, bool reliable)> _pendingSends = new Queue<(MemoryBuffer, bool)>();

	private readonly Queue<MemoryBuffer> _pendingReceives = new Queue<MemoryBuffer>();

	private AService _activeService;

	private long _activeChannelId = -1L;

	private bool _decisionMade;

	private long _waitingStartTimeMs = -1L;

	private bool _connectionConfirmed;

	public Guid Guid { get; }

	public long ActiveChannelId => _activeChannelId;

	public long WaitingTimeMs
	{
		get
		{
			if (_waitingStartTimeMs >= 0)
			{
				return Timer.ElapsedMilliseconds - _waitingStartTimeMs;
			}
			return -1L;
		}
	}

	public bool IsConfirmed => _connectionConfirmed;

	public Dictionary<AService, HService.ConnectionStatus> Connections => _connectionStatus;

	private Stopwatch Timer => _parentService._stopwatch;

	private long TimeoutMs => _parentService._maxDelayMs;

	public HChannel(long id, IPEndPoint remoteAddress, HService parentService, Guid guid, ChannelType type)
	{
		base.ChannelType = type;
		base.Id = id;
		Guid = guid;
		base.RemoteAddress = remoteAddress;
		_parentService = parentService;
	}

	public void SendHandShakeOnClientConnected(AService service, long channelId)
	{
		if (service != null && _connectionStatus.TryGetValue(service, out var _))
		{
			MemoryBuffer memoryBuffer = _parentService.Fetch();
			byte[] data = Guid.ToByteArray();
			memoryBuffer.Write(HService.kGUIDMagicNumber);
			memoryBuffer.Write(data);
			memoryBuffer.Seek(0L, SeekOrigin.Begin);
			service.Send(channelId, memoryBuffer);
			_parentService.Recycle(memoryBuffer);
			ConnectedSubChannel(service, channelId);
		}
	}

	internal long CreateSubChannel(HService.ServiceEntry entry)
	{
		if (!_connectionStatus.ContainsKey(entry.Service))
		{
			long num = ((base.ChannelType == ChannelType.Connect) ? entry.Service.Create(base.RemoteAddress) : (-1));
			_connectionStatus[entry.Service] = new HService.ConnectionStatus
			{
				Entry = entry,
				ChannelID = num,
				Connected = false,
				ArrivalTimeMs = -1L
			};
			return num;
		}
		return -1L;
	}

	internal void ConnectedSubChannel(AService service, long subChannelId)
	{
		foreach (HService.ServiceEntry service2 in _parentService._services)
		{
			if (service2.Service == service)
			{
				ConnectedSubChannel(service2, subChannelId);
				break;
			}
		}
	}

	internal void ConnectedSubChannel(HService.ServiceEntry entry, long subChannelId)
	{
		StartWaitting();
		if (!_connectionStatus.TryGetValue(entry.Service, out var value))
		{
			value = new HService.ConnectionStatus
			{
				Entry = entry
			};
			_connectionStatus[entry.Service] = value;
		}
		value.ChannelID = subChannelId;
		value.Connected = true;
		value.ArrivalTimeMs = Timer.ElapsedMilliseconds;
		if (!IsWaitingForAllConnections())
		{
			ExecuteFinalDecision();
		}
	}

	internal void RemoveSubChannel(long subChannelId, AService service)
	{
		if (_connectionStatus.TryGetValue(service, out var value) && value.ChannelID == subChannelId)
		{
			value.Connected = false;
		}
	}

	private void ExecuteFinalDecision()
	{
		if (_decisionMade)
		{
			return;
		}
		long num = long.MaxValue;
		long num2 = -1L;
		AService aService = null;
		foreach (KeyValuePair<AService, HService.ConnectionStatus> item in _connectionStatus)
		{
			if (item.Value.Connected)
			{
				long arrivalTimeMs = item.Value.ArrivalTimeMs;
				int delayMs = item.Value.Entry.DelayMs;
				long num3 = arrivalTimeMs + delayMs;
				if (num3 < num)
				{
					num = num3;
					num2 = item.Value.ChannelID;
					aService = item.Key;
				}
			}
		}
		if (num2 < 0)
		{
			HandleNoValidConnection();
			return;
		}
		_decisionMade = true;
		_activeChannelId = num2;
		_activeService = aService;
		ConfirmedConnection(num2, aService);
	}

	public void ConfirmedConnection(long pickChannelId, AService pickService)
	{
		if (!_connectionConfirmed)
		{
			_decisionMade = true;
			_activeChannelId = pickChannelId;
			_activeService = pickService;
			Log.Info($"HService connection confirmed {Guid}: {_activeChannelId} type {_activeService.GetType()}");
			_parentService._events.Enqueue(new HService.DelayEvent
			{
				Op = HService.DelayOp.Connect,
				Channel = this
			});
			_parentService._events.Enqueue(new HService.DelayEvent
			{
				Op = HService.DelayOp.Recv,
				Channel = this
			});
			_parentService._events.Enqueue(new HService.DelayEvent
			{
				Op = HService.DelayOp.Send,
				Channel = this
			});
		}
	}

	internal void OnConnectionCallback()
	{
		if (_connectionConfirmed)
		{
			return;
		}
		_connectionConfirmed = true;
		foreach (KeyValuePair<AService, HService.ConnectionStatus> item in _connectionStatus)
		{
			if (item.Value.ChannelID != ActiveChannelId)
			{
				item.Value.Connected = false;
				item.Key.Remove(item.Value.ChannelID);
			}
		}
		if (base.ChannelType == ChannelType.Connect)
		{
			_parentService.ConnectCallback?.Invoke(base.Id);
		}
		else
		{
			_parentService.AcceptCallback?.Invoke(base.Id, base.RemoteAddress);
		}
	}

	private void HandleNoValidConnection()
	{
		base.Error = 200002;
		_parentService.Remove(base.Id);
	}

	private void StartWaitting()
	{
		if (_waitingStartTimeMs < 0)
		{
			_waitingStartTimeMs = Timer.ElapsedMilliseconds;
		}
	}

	private bool IsWaitingForAllConnections()
	{
		if (_decisionMade)
		{
			return false;
		}
		if (WaitingTimeMs > TimeoutMs)
		{
			return false;
		}
		int num = 0;
		foreach (KeyValuePair<AService, HService.ConnectionStatus> item in _connectionStatus)
		{
			if (item.Value.Connected)
			{
				num++;
			}
		}
		return num != _parentService._services.Count;
	}

	private bool IsInvalidBeforeConfirm()
	{
		if (_connectionConfirmed)
		{
			return false;
		}
		int num = 0;
		foreach (KeyValuePair<AService, HService.ConnectionStatus> item in _connectionStatus)
		{
			if (item.Value.ChannelID > 0 && (item.Key.IsDisposed() || !item.Key.IsValidChannel(item.Value.ChannelID)))
			{
				num++;
			}
		}
		return num == _connectionStatus.Count;
	}

	public void Update()
	{
		if (!base.IsDisposed && !_decisionMade)
		{
			if (!IsWaitingForAllConnections())
			{
				ExecuteFinalDecision();
			}
			if (IsInvalidBeforeConfirm())
			{
				HandleNoValidConnection();
			}
		}
	}

	private MemoryBuffer CloneBuffer(MemoryBuffer data)
	{
		MemoryBuffer memoryBuffer = _parentService.Fetch(data.DataSize);
		memoryBuffer.Write(data.GetBuffer(), (int)data.Position, data.DataSize);
		memoryBuffer.Seek(0L, SeekOrigin.Begin);
		return memoryBuffer;
	}

	public void ReadData(MemoryBuffer data)
	{
		if (_decisionMade && _connectionConfirmed)
		{
			_parentService.ReadCallback?.Invoke(base.Id, data);
		}
		else
		{
			_pendingReceives.Enqueue(CloneBuffer(data));
		}
	}

	public void SendReliable(MemoryBuffer data)
	{
		if (_decisionMade && _connectionConfirmed)
		{
			_activeService.Send(_activeChannelId, data);
		}
		else
		{
			_pendingSends.Enqueue((CloneBuffer(data), true));
		}
	}

	public void SendUnreliable(MemoryBuffer data)
	{
		if (_decisionMade && _connectionConfirmed)
		{
			_activeService.SendUnreliable(_activeChannelId, data);
		}
		else
		{
			_pendingSends.Enqueue((CloneBuffer(data), false));
		}
	}

	internal void FlushPendingReceives()
	{
		while (_pendingReceives.Count > 0)
		{
			MemoryBuffer memoryBuffer = _pendingReceives.Dequeue();
			_parentService.ReadCallback?.Invoke(base.Id, memoryBuffer);
			_parentService.Recycle(memoryBuffer);
		}
	}

	internal void FlushPendingSends()
	{
		while (_pendingSends.Count > 0)
		{
			(MemoryBuffer data, bool reliable) tuple = _pendingSends.Dequeue();
			var (memoryBuffer, _) = tuple;
			if (tuple.reliable)
			{
				SendReliable(memoryBuffer);
			}
			else
			{
				SendUnreliable(memoryBuffer);
			}
			_parentService.Recycle(memoryBuffer);
		}
	}

	public void Close(int error = 0)
	{
		foreach (KeyValuePair<AService, HService.ConnectionStatus> item2 in _connectionStatus)
		{
			if (item2.Value.ChannelID != -1)
			{
				item2.Key.Remove(item2.Value.ChannelID, error);
			}
		}
		while (_pendingReceives.Count > 0)
		{
			MemoryBuffer memoryBuffer = _pendingReceives.Dequeue();
			_parentService.Recycle(memoryBuffer);
		}
		_pendingReceives.Clear();
		while (_pendingSends.Count > 0)
		{
			MemoryBuffer item = _pendingSends.Dequeue().data;
			_parentService.Recycle(item);
		}
		_pendingSends.Clear();
		_decisionMade = true;
	}

	public override void Dispose()
	{
		if (base.IsDisposed)
		{
			return;
		}
		_ = base.Id;
		if (IsConfirmed || base.ChannelType == ChannelType.Connect)
		{
			if (base.Error != 0)
			{
				_parentService.ErrorCallback?.Invoke(base.Id, base.Error);
			}
			_parentService.DisconnectCallback?.Invoke(base.Id);
		}
		foreach (KeyValuePair<AService, HService.ConnectionStatus> item in _connectionStatus)
		{
			HService.ConnectionStatus value = item.Value;
			if (value.ChannelID > 0 && !value.Entry.Service.IsDisposed())
			{
				value.Entry.Service.Remove(value.ChannelID);
				value.Connected = false;
				value.ChannelID = -1L;
			}
		}
		base.Id = 0L;
		_activeChannelId = -1L;
		_activeService = null;
	}
}
