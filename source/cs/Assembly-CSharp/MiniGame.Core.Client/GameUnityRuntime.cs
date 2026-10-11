using System;
using System.Collections;
using System.IO;
using Joker;
using MiniGame.GGGo.Client;
using UnityEngine;

namespace MiniGame.Core.Client;

public abstract class GameUnityRuntime : MonoBehaviour
{
	public static string DumpFolder = "dump";

	protected float[] _playerRtts;

	public int lostConnectionLimit = 5;

	public int reconnectAttemptsMax = 3;

	public float reconnectTimeout = 5f;

	public float reconnectDelay;

	public bool useHybridNetwork;

	protected Coroutine ReconnectCoroutine;

	protected bool _triggeredGameOver;

	public static bool DumpEnable => false;

	public abstract bool IsPlaying { get; protected set; }

	public abstract GameWorld Game { get; protected set; }

	public abstract GameClientPlayer ClientPlayer { get; protected set; }

	public virtual Type ClientType => null;

	public virtual string LogicType => null;

	public string LevelPath { get; protected set; }

	public string Address { get; protected set; }

	public string UID { get; protected set; }

	public string SID { get; protected set; }

	public string UUID { get; protected set; }

	public string PlayerID { get; protected set; }

	public string RoomSessionID { get; protected set; }

	public string LogicVersion { get; protected set; }

	public string ResVersion { get; protected set; }

	public bool IsAppPaused { get; protected set; }

	public float RttMs { get; protected set; }

	public int LostConnectionTimes { get; protected set; }

	public float connectTimeout { get; protected set; } = -1f;

	public int ReconnectAttemptsLeft { get; protected set; } = -1;

	public bool Reconnecting { get; protected set; }

	public bool IsGameOver
	{
		get
		{
			if (Game == null)
			{
				return false;
			}
			if (Game.Env.GameOver)
			{
				return Game.State == EGameWorldState.Settlement;
			}
			return false;
		}
	}

	protected void UpdateRtt(float rttMs)
	{
		if (RttMs == 0f)
		{
			RttMs = rttMs;
		}
		else
		{
			RttMs = RttMs * 0.7f + rttMs * 0.3f;
		}
	}

	public float GetPlayerRttMs(int playerIndex)
	{
		if (_playerRtts == null || playerIndex < 0 || playerIndex >= _playerRtts.Length)
		{
			return 0f;
		}
		return _playerRtts[playerIndex];
	}

	protected virtual void Awake()
	{
		GlobalMonobehaviourDispatcher.onApplicationPause += HanleAppPauseState;
	}

	protected virtual void OnDestroy()
	{
		ExitGame();
		StopAllCoroutines();
		GlobalMonobehaviourDispatcher.onApplicationPause -= HanleAppPauseState;
	}

	protected virtual IEnumerator StartGameCoroutine()
	{
		yield return InitClientPlayerCoroutine();
		yield return ConnectToServerCoroutine();
	}

	protected virtual IEnumerator InitClientPlayerCoroutine()
	{
		ResetClientPlayerState();
		PlayerID = (string.IsNullOrEmpty(PlayerID) ? $"P-{Guid.NewGuid()}" : PlayerID);
		Singleton<WorldService>.Instance.Create(ClientType, ESchedulerType.Main, -1, PlayerID);
		while (ClientPlayer == null)
		{
			yield return null;
			ClientPlayer = Singleton<WorldService>.Instance.GetWorld(PlayerID) as GGGoClientPlayer;
		}
		ClientPlayer.UseHybridNetwork = useHybridNetwork;
		GameClientPlayer clientPlayer = ClientPlayer;
		clientPlayer.OnPlayerConnected = (Action)Delegate.Combine(clientPlayer.OnPlayerConnected, new Action(OnClientPlayerConnected));
		GameClientPlayer clientPlayer2 = ClientPlayer;
		clientPlayer2.OnPlayerDisconnected = (Action)Delegate.Combine(clientPlayer2.OnPlayerDisconnected, new Action(OnClientPlayerDisconnected));
		ClientPlayer.RoomSessionID = (string.IsNullOrEmpty(RoomSessionID) ? "local_pvp_room" : RoomSessionID);
		ClientPlayer.PlayerSessionID = PlayerID;
		ClientPlayer.UID = (string.IsNullOrEmpty(UID) ? Guid.NewGuid().ToString() : UID);
		ClientPlayer.SID = (string.IsNullOrEmpty(SID) ? Guid.NewGuid().ToString() : SID);
		ClientPlayer.UUID = (string.IsNullOrEmpty(UUID) ? Guid.NewGuid().ToString() : UUID);
		ClientPlayer.LogicType = LogicType;
		ClientPlayer.LogicVersion = LogicVersion;
		ClientPlayer.ResourceVersion = ResVersion;
		ClientPlayer.Game = Game;
	}

	protected virtual IEnumerator ConnectToServerCoroutine()
	{
		ClientPlayer.ConnectServer(Address);
		float startTime = Time.unscaledTime;
		while (!ClientPlayer.IsConnected)
		{
			if (connectTimeout > 0f && Time.unscaledTime - startTime > connectTimeout)
			{
				HandleServerDisconnected();
				Singleton<WorldService>.Instance.DestroyWorld(ClientPlayer);
				ClientPlayer = null;
				yield break;
			}
			yield return null;
		}
		HandleServerConnected();
	}

	protected virtual void UpdateGameWorld()
	{
		Game?.Update(Time.deltaTime);
		if (IsGameOver && !_triggeredGameOver)
		{
			OnGameOver();
			_triggeredGameOver = true;
		}
	}

	public virtual void UpdateTestConnection(KeyCode reconnectKey = KeyCode.P, KeyCode disconnectKey = KeyCode.Q, int playerId = 0)
	{
		if (playerId == 0 || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			if (Input.GetKeyDown(reconnectKey))
			{
				ClientPlayer?.DisconnectServer();
			}
			if (Input.GetKeyDown(disconnectKey))
			{
				ReconnectAttemptsLeft = 0;
				ClientPlayer?.DisconnectServer();
			}
		}
	}

	public virtual void OnGameOver()
	{
	}

	public virtual void ExitGame()
	{
		Game?.Dispose();
		Game = null;
		ResetClientPlayerState();
	}

	public virtual void HanleAppPauseState(bool pause)
	{
		if (!pause)
		{
			OnAppResume();
		}
		else
		{
			OnAppPause();
		}
	}

	public virtual void OnAppResume()
	{
		IsAppPaused = false;
		HandleServerTryReconnect();
	}

	public virtual void OnAppPause()
	{
		IsAppPaused = true;
	}

	public virtual void HandleServerConnected()
	{
		Log.Info(GetType().Name + " HandleServerConnected");
	}

	public virtual void HandleServerReconnected()
	{
		Log.Info(GetType().Name + " HandleServerReconnected");
		HandleServerConnected();
	}

	public virtual void HandleServerDisconnected()
	{
		Log.Info(GetType().Name + " HandleServerDisconnected");
	}

	public virtual void HandleServerTryReconnect()
	{
		Log.Info(GetType().Name + " HandleServerTryReconnect");
		if (reconnectAttemptsMax <= 0 || ReconnectAttemptsLeft == 0)
		{
			ReconnectAttemptsLeft = -1;
			HandleServerDisconnected();
		}
		else if (!IsAppPaused)
		{
			if (LostConnectionTimes >= lostConnectionLimit)
			{
				HandleServerDisconnected();
			}
			else if (ClientPlayer != null && ClientPlayer.IsRoomCanReconnect)
			{
				StartCoroutine(TryReconnectWrapperCoroutine());
			}
		}
	}

	public virtual void CancelReconnect()
	{
		if (ReconnectCoroutine != null)
		{
			StopCoroutine(ReconnectCoroutine);
			ReconnectCoroutine = null;
			Reconnecting = false;
		}
	}

	public virtual void ResetClientPlayerState()
	{
		CancelReconnect();
		LostConnectionTimes = 0;
		ReconnectAttemptsLeft = -1;
		connectTimeout = -1f;
		RttMs = 0f;
		_playerRtts = null;
		if (ClientPlayer != null)
		{
			Singleton<WorldService>.Instance?.DestroyWorld(ClientPlayer);
			ClientPlayer = null;
		}
	}

	protected IEnumerator TryReconnectWrapperCoroutine()
	{
		CancelReconnect();
		Coroutine myCoroutine = (ReconnectCoroutine = StartCoroutine(TryReconnectCoroutine()));
		yield return myCoroutine;
		if (ReconnectCoroutine == myCoroutine)
		{
			ReconnectCoroutine = null;
		}
	}

	protected virtual IEnumerator TryReconnectCoroutine()
	{
		float timeout = reconnectTimeout;
		ReconnectAttemptsLeft = reconnectAttemptsMax;
		while (ReconnectAttemptsLeft > 0)
		{
			if (!Reconnecting)
			{
				if (reconnectDelay > 0f)
				{
					yield return new WaitForSecondsRealtime(reconnectDelay);
				}
				if (ClientPlayer == null)
				{
					yield break;
				}
				ClientPlayer.ConnectServer(Address);
				Reconnecting = true;
				ReconnectAttemptsLeft--;
				timeout = reconnectTimeout;
			}
			yield return null;
			if (ClientPlayer == null)
			{
				yield break;
			}
			if (ClientPlayer.IsConnected)
			{
				Reconnecting = false;
				ReconnectAttemptsLeft = -1;
				HandleServerReconnected();
				yield break;
			}
			timeout -= Time.unscaledDeltaTime;
			if (timeout < 0f)
			{
				Reconnecting = false;
			}
		}
		Reconnecting = false;
		ReconnectAttemptsLeft = -1;
		HandleServerDisconnected();
	}

	protected virtual void OnClientPlayerConnected()
	{
	}

	protected virtual void OnClientPlayerDisconnected()
	{
		LostConnectionTimes++;
		HandleServerTryReconnect();
	}

	public static void ResetDumpFolder()
	{
		if (DumpEnable)
		{
			if (Directory.Exists("dump"))
			{
				Directory.Delete("dump", recursive: true);
			}
			Directory.CreateDirectory("dump");
		}
	}

	public static string GetDumpPath(string path)
	{
		return Path.Combine(DumpFolder, path);
	}
}
