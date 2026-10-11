using System;
using System.Net;
using Joker;
using Joker.Client;
using MiniGame.Core.Server;

namespace MiniGame.Core.Client;

public abstract class GameClientPlayer : GameClient
{
	public string LevelPath;

	public int MaxPlayer;

	protected bool IsRoomDisposed;

	public string RoomSessionID { get; set; }

	public string PlayerSessionID { get; set; }

	public string SID { get; set; }

	public string UID { get; set; }

	public string UUID { get; set; }

	public bool IsConnected => Channel >= 0;

	public int PlayerID { get; protected set; }

	public string[] Players { get; protected set; }

	public GameWorld Game { get; set; }

	public bool IsRoomCanReconnect
	{
		get
		{
			if (!IsRoomDisposed && Game != null)
			{
				return Game.State != EGameWorldState.Settlement;
			}
			return false;
		}
	}

	public bool IsObserver { get; set; }

	public string LogicType { get; set; }

	public string LogicVersion { get; set; }

	public string ResourceVersion { get; set; }

	public bool UseHybridNetwork { get; set; }

	public bool IsGameRunning { get; protected set; }

	public Action OnPlayerConnected { get; set; }

	public Action OnPlayerDisconnected { get; set; }

	protected NetworkSystem Network { get; set; }

	protected long Channel { get; set; }

	public virtual void ConnectServer(string address, bool force = true)
	{
		string[] array = address.Split(new char[1] { ':' });
		IPEndPoint address2 = new IPEndPoint(IPAddress.Parse(array[0]), int.Parse(array[1]));
		ConnectServer(address2, force);
	}

	public virtual void ConnectServer(IPEndPoint address, bool force = true)
	{
		IsGameRunning = false;
		if (force || Network == null)
		{
			DestroyNetworkSystem();
			InitNetworkSystem();
		}
		Network.Connect(address);
	}

	public virtual void DisconnectServer()
	{
		IsGameRunning = false;
		long channel = Channel;
		if (Network != null && Channel >= 0)
		{
			DestroyNetworkSystem();
		}
		if (channel >= 0)
		{
			OnDisconnect(channel);
		}
	}

	protected void InitNetworkSystem()
	{
		if (UseHybridNetwork)
		{
			Log.Info(GetType().Name + " init NetworkSystem use NetworkProtocol.Hybrid");
			Network = AddSystem<NetworkSystem, NetworkProtocol>(NetworkProtocol.Hybrid);
		}
		else
		{
			Log.Info(GetType().Name + " init NetworkSystem use NetworkProtocol.TCP");
			Network = AddSystem<NetworkSystem, NetworkProtocol>(NetworkProtocol.TCP);
		}
		NetworkSystem network = Network;
		network.OnConnect = (Action<long>)Delegate.Combine(network.OnConnect, new Action<long>(OnConnect));
		NetworkSystem network2 = Network;
		network2.OnDisconnect = (Action<long>)Delegate.Combine(network2.OnDisconnect, new Action<long>(OnDisconnect));
		NetworkSystem network3 = Network;
		network3.OnMessage = (Action<long, IMessage>)Delegate.Combine(network3.OnMessage, new Action<long, IMessage>(OnMessage));
		NetworkSystem network4 = Network;
		network4.OnError = (Action<long, int>)Delegate.Combine(network4.OnError, new Action<long, int>(OnError));
	}

	protected void DestroyNetworkSystem()
	{
		Channel = -1L;
		if (Network != null)
		{
			NetworkSystem network = Network;
			network.OnConnect = (Action<long>)Delegate.Remove(network.OnConnect, new Action<long>(OnConnect));
			NetworkSystem network2 = Network;
			network2.OnDisconnect = (Action<long>)Delegate.Remove(network2.OnDisconnect, new Action<long>(OnDisconnect));
			NetworkSystem network3 = Network;
			network3.OnMessage = (Action<long, IMessage>)Delegate.Remove(network3.OnMessage, new Action<long, IMessage>(OnMessage));
			NetworkSystem network4 = Network;
			network4.OnError = (Action<long, int>)Delegate.Remove(network4.OnError, new Action<long, int>(OnError));
			DelSystem(Network);
			Network = null;
		}
	}

	public virtual void Send(object message)
	{
		RoomMessage roomMessage = null;
		bool reliable = true;
		if (message is IMessageSync)
		{
			if (!IsGameRunning)
			{
				return;
			}
			roomMessage = new C2SGameRoomSync();
		}
		else if (message is IMessageUnreliableSync)
		{
			if (!IsGameRunning)
			{
				return;
			}
			reliable = false;
			roomMessage = new C2SGameRoomSync();
		}
		else if (message is IMessageEnter messageEnter)
		{
			messageEnter.LevelPath = (string.IsNullOrEmpty(messageEnter.LevelPath) ? LevelPath : messageEnter.LevelPath);
			messageEnter.MaxPlayers = ((messageEnter.MaxPlayers <= 0) ? MaxPlayer : messageEnter.MaxPlayers);
			roomMessage = new C2SGameRoomEnter
			{
				LevelPath = messageEnter.LevelPath,
				MaxPlayers = messageEnter.MaxPlayers,
				IsObserver = IsObserver,
				LogicType = LogicType,
				LogicVersion = LogicVersion,
				ResourceVersion = ResourceVersion,
				Data = messageEnter
			};
		}
		else
		{
			roomMessage = ((message is IMessageLeave) ? new C2SGameRoomLeave() : ((message is IMessageStart) ? new C2SGameRoomStart() : ((!(message is IMessageVerify data)) ? (message as RoomMessage) : new C2SGameRoomVerify
			{
				LogicType = LogicType,
				LogicVersion = LogicVersion,
				ResourceVersion = ResourceVersion,
				Data = data
			})));
		}
		SendRoomMessage(message, roomMessage, reliable);
	}

	protected virtual void SendRoomMessage(object message, RoomMessage room, bool reliable)
	{
		if (!(room is C2SGameRoomSync))
		{
			room.RoomSessionID = RoomSessionID;
			room.PlayerSessionID = PlayerSessionID;
			room.SID = SID;
			room.UID = UID;
			room.UUID = UUID;
		}
		else
		{
			room.RoomSessionID = null;
			room.PlayerSessionID = null;
			room.SID = null;
			room.UID = null;
			room.UUID = null;
		}
		PackMessage(message, out room.OpCode, out room.Data);
		if (reliable)
		{
			Network?.Send(Channel, room);
		}
		else
		{
			Network?.SendUnreliable(Channel, room);
		}
	}

	protected virtual void OnConnect(long channel)
	{
		Channel = channel;
		Log.Info($"{GetType().Name} OnConnect {Channel}");
		OnPlayerConnected?.Invoke();
	}

	protected virtual void OnDisconnect(long channel)
	{
		Log.Info($"{GetType().Name} OnDisconnect {GetType().Name}:{Channel}");
		Channel = -1L;
		IsGameRunning = false;
		OnPlayerDisconnected?.Invoke();
	}

	protected virtual void OnMessage(long channel, IMessage message)
	{
		if (message is RoomMessage roomMessage)
		{
			UnpackMessage(roomMessage.OpCode, roomMessage.Data, out roomMessage.Data);
		}
		if (message is S2CGameRoomStart s2CGameRoomStart)
		{
			Players = s2CGameRoomStart.PlayerSessionIDs.Clone() as string[];
			PlayerID = -1;
			for (int i = 0; i < Players.Length; i++)
			{
				if (Players[i] == PlayerSessionID)
				{
					PlayerID = i;
					break;
				}
			}
			IsGameRunning = true;
			OnGameStart(s2CGameRoomStart);
		}
		else if (message is S2CGameRoomEnd msg)
		{
			OnGameEnd(msg);
			IsRoomDisposed = true;
		}
		else if (message is S2CGameRoomEnter s2CGameRoomEnter)
		{
			Log.Info(GetType().Name + " FleetID:" + s2CGameRoomEnter.FleetID + " InstanceID:" + s2CGameRoomEnter.InstanceID + " ProcessID:" + s2CGameRoomEnter.ProcessID);
			OnRoomEnter(s2CGameRoomEnter);
		}
		else if (message is S2CGameRoomLeave msg2)
		{
			OnRoomLeave(msg2);
		}
		else if (message is S2CGameRoomVerify)
		{
			IsRoomDisposed = true;
		}
		EnqueueMessage(message, channel);
	}

	protected virtual void OnError(long channel, int error)
	{
		string errorDesc = NetworkSystem.GetErrorDesc(error);
		Log.Info($"{GetType().Name} Network lost {channel} with resion:{errorDesc}({error})");
	}

	protected abstract void PackMessage(object message, out int opCode, out object data);

	protected abstract bool UnpackMessage(int opCode, object data, out object result);

	protected virtual void OnGameStart(S2CGameRoomStart msg)
	{
	}

	protected virtual void OnGameEnd(S2CGameRoomEnd msg)
	{
	}

	protected virtual void OnRoomEnter(S2CGameRoomEnter msg)
	{
	}

	protected virtual void OnRoomLeave(S2CGameRoomLeave msg)
	{
	}
}
