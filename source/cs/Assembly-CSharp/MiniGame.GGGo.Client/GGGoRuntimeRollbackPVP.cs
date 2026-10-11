using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using Joker;
using MiniGame.Core;
using MiniGame.Core.Client;
using MiniGame.Core.Server;
using Newtonsoft.Json;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class GGGoRuntimeRollbackPVP : GameUnityRuntime
{
	public float takeSnapshotInterval = 2f;

	private float _takeSnapshotLastTime = float.MinValue;

	[NonSerialized]
	public Action OnServerConnected;

	[NonSerialized]
	public Action OnServerDisconnected;

	[NonSerialized]
	public Action OnServerTryReconnect;

	[NonSerialized]
	public Action OnServerReconnected;

	public int framePredictRangeMax = 2;

	public int framePredictRangeMin = 1;

	public int frameAdjustIntervalMax = 15;

	public int frameAdjustIntervalMin = 2;

	private SnapshotWorld _snapshot;

	private SnapshotWorldRuntime _snapshotRuntime;

	private List<SyncCommand> serverSynCommands;

	public float predictFrameEMAFactor = 0.9f;

	public float predictTimeEMAFactor = 0.9f;

	private int _lastFrameAdjust = -1;

	private int _serverFrameSync = -1;

	public float maxPredictAheadTime = 1.5f;

	private bool _lastPredictAheadLimited;

	private int _playerIndex;

	private GGGoDebugReplay _debugReplay;

	public override bool IsPlaying
	{
		get
		{
			return Game != null;
		}
		protected set
		{
		}
	}

	public override GameWorld Game { get; protected set; }

	public override Type ClientType => typeof(GGGoClientPlayer);

	public override string LogicType => "GGGoLogicRealtime";

	public Transform LevelRoot { get; protected set; }

	private float FramePredictRangeMid => (float)(framePredictRangeMax + framePredictRangeMin) * 0.5f;

	private int FrameAdjustInterval { get; set; }

	private GGGoEnvClient Env => Game?.Env as GGGoEnvClient;

	public override GameClientPlayer ClientPlayer
	{
		get
		{
			return Env?.Player;
		}
		protected set
		{
			Env.Player = value as GGGoClientPlayer;
		}
	}

	private List<SyncCommand> clientPredictCommands => Env?.PredictCommands;

	private float PredictTimeScaleFactor { get; set; } = 1f;

	private float PredictDeltaTime { get; set; }

	private float PredictDeltaSyncTime { get; set; }

	private int MaxPredictAheadFrames
	{
		get
		{
			if (Env == null)
			{
				return 0;
			}
			return Mathf.RoundToInt(maxPredictAheadTime / Env.LogicTickDelta.AsFloat);
		}
	}

	private bool IsPredictAheadLimited
	{
		get
		{
			if (_serverFrameSync < 0 || Env == null)
			{
				return false;
			}
			return Env.LogicTickCount > _serverFrameSync + MaxPredictAheadFrames;
		}
	}

	public void Update()
	{
		UpdateGame();
	}

	public void StartGame(Transform root, string levelPath, string address, string uid, string sid, string uuid, string playerSessionId, string roomSessionId, int index = 0)
	{
		InitPredictState();
		ResetCommand();
		_playerIndex = index;
		address = (string.IsNullOrWhiteSpace(address) ? "127.0.0.1:7758" : address);
		if (!address.Contains(":"))
		{
			address += ":7758";
		}
		LevelRoot = root;
		base.LevelPath = levelPath;
		base.Address = address;
		base.UID = uid;
		base.SID = sid;
		base.UUID = uuid;
		base.PlayerID = playerSessionId;
		base.RoomSessionID = roomSessionId;
		GameUnityApp.Init();
		InitClientPredictGame();
		GGGoRuntimePVP.TrySplitLocalView(roomSessionId, index, base.transform, Env);
		StartCoroutine(StartGameCoroutine());
	}

	protected override IEnumerator StartGameCoroutine()
	{
		return base.StartGameCoroutine();
	}

	public void SetUpVersion(string logicVersion, string resVersion)
	{
		base.LogicVersion = logicVersion;
		base.ResVersion = resVersion;
		if (ClientPlayer != null)
		{
			ClientPlayer.LogicVersion = base.LogicVersion;
			ClientPlayer.ResourceVersion = base.ResVersion;
		}
	}

	public override void ExitGame()
	{
		base.ExitGame();
		OnServerConnected = null;
		OnServerDisconnected = null;
		OnServerTryReconnect = null;
		OnServerReconnected = null;
	}

	private void InitPredictState()
	{
		PredictTimeScaleFactor = 1f;
		PredictDeltaTime = 0f;
		FrameAdjustInterval = frameAdjustIntervalMin;
		_lastFrameAdjust = -1;
		_serverFrameSync = -1;
		_lastPredictAheadLimited = false;
	}

	private void ResetCommand()
	{
		_takeSnapshotLastTime = float.MinValue;
		serverSynCommands = serverSynCommands ?? new List<SyncCommand>();
		serverSynCommands.Clear();
	}

	public override void HandleServerConnected()
	{
		GGGoMsgEnter gGGoMsgEnter = new GGGoMsgEnter();
		gGGoMsgEnter.LevelPath = base.LevelPath;
		gGGoMsgEnter.MaxPlayers = 2;
		ClientPlayer.Send(gGGoMsgEnter);
		OnServerConnected?.Invoke();
	}

	public override void HandleServerDisconnected()
	{
		base.HandleServerDisconnected();
		OnServerDisconnected?.Invoke();
	}

	public override void HandleServerTryReconnect()
	{
		base.HandleServerTryReconnect();
		OnServerTryReconnect?.Invoke();
	}

	public override void HandleServerReconnected()
	{
		base.HandleServerReconnected();
		OnServerReconnected?.Invoke();
	}

	private void UpdateGame()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		(IMessage, object) message;
		while (ClientPlayer != null && ClientPlayer.TryDequeueMessage(out message) && message.Item1 is RoomMessage roomMessage)
		{
			if (roomMessage.Data is GGGoMsgReconnectResp msg)
			{
				num2 = Math.Max(HandleMsgReconnect(msg), num2);
			}
			else if (roomMessage.Data is GGGoMsgFrameSyncResp msg2)
			{
				num = Math.Min(HandleMsgFrameSync(msg2), num);
			}
			else if (roomMessage.Data is GGGoMsgFrameAdjustResp msg3)
			{
				HandleMsgFrameAdjust(msg3);
			}
			else if (!(roomMessage.Data is GGGoMsgEnterResp) && roomMessage.Data is GGGoMsgStartResp msg4)
			{
				HandleMsgGameStart(ClientPlayer, Env, msg4);
			}
		}
		if (num2 > 0)
		{
			RollbackGame(num2, num2 + 4);
		}
		else if (num < Env.LogicTickCount)
		{
			RollbackGame(num, Env.LogicTickCount);
		}
		if (IsPredictAheadLimited)
		{
			if (!_lastPredictAheadLimited)
			{
				GameFramework.Log.Info($"[PredictLimit] 客户端领先超限, 本地tick:{Env.LogicTickCount}, 服务器同步帧:{_serverFrameSync}, 最大领先:{MaxPredictAheadFrames}帧, 等待服务器同步");
			}
			_lastPredictAheadLimited = true;
		}
		else
		{
			if (_lastPredictAheadLimited)
			{
				GameFramework.Log.Info($"[PredictLimit] 恢复运行, 本地tick:{Env.LogicTickCount}, 服务器同步帧:{_serverFrameSync}");
			}
			_lastPredictAheadLimited = false;
			Game?.Update(PickFrameUpdateTime());
		}
		FrameAdjust();
		if (base.IsGameOver && !_triggeredGameOver)
		{
			OnGameOver();
			_triggeredGameOver = true;
		}
	}

	public void HandleMsgFrameAdjust(GGGoMsgFrameAdjustResp msg)
	{
		if (msg.Time > 0f)
		{
			UpdateRtt((Time.unscaledTime - msg.Time) * 1000f);
		}
		if (msg.Pings != null && _playerRtts != null)
		{
			for (int i = 0; i < _playerRtts.Length && i < msg.Pings.Length; i++)
			{
				_playerRtts[i] = msg.Pings[i];
			}
		}
		int frame = msg.Frame;
		if (frame > framePredictRangeMax || frame < framePredictRangeMin)
		{
			PredictDeltaSyncTime = ((float)frame - FramePredictRangeMid) * Env.LogicTickDelta.AsFloat;
			PredictDeltaTime = PredictDeltaTime * predictFrameEMAFactor + PredictDeltaSyncTime * (1f - predictFrameEMAFactor);
		}
		else
		{
			PredictDeltaSyncTime = 0f;
			PredictDeltaTime = 0f;
			PredictTimeScaleFactor = 1f;
		}
		if (PredictDeltaTime == 0f)
		{
			FrameAdjustInterval = frameAdjustIntervalMax;
		}
		else
		{
			FrameAdjustInterval = frameAdjustIntervalMin + (int)((float)(frameAdjustIntervalMax - frameAdjustIntervalMin) * Mathf.Clamp01(0.5f - Mathf.Abs(PredictDeltaSyncTime)));
		}
	}

	public float PickFrameUpdateTime()
	{
		if (PredictDeltaTime == 0f)
		{
			return Time.unscaledDeltaTime;
		}
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		float num = unscaledDeltaTime * PredictTimeScaleFactor;
		if (PredictDeltaTime > 0f)
		{
			PredictDeltaTime -= unscaledDeltaTime - num;
			if (PredictDeltaTime <= 0f)
			{
				PredictDeltaTime = 0f;
				PredictTimeScaleFactor = 1f;
				return num;
			}
		}
		else if (PredictDeltaTime < 0f)
		{
			PredictDeltaTime -= unscaledDeltaTime - num;
			if (PredictDeltaTime >= 0f)
			{
				PredictDeltaTime = 0f;
				PredictTimeScaleFactor = 1f;
				return num;
			}
		}
		PredictTimeScaleFactor = PredictTimeScaleFactor * predictTimeEMAFactor + (1f - PredictDeltaTime) * (1f - predictTimeEMAFactor);
		PredictTimeScaleFactor = Mathf.Clamp(PredictTimeScaleFactor, 0.5f, 2f);
		return num;
	}

	public void FrameAdjust()
	{
		if (ClientPlayer != null && Game.Env.GameState == EGameWorldState.Running && _serverFrameSync >= 0)
		{
			int logicTickCount = Game.Env.LogicTickCount;
			if (_lastFrameAdjust != logicTickCount && logicTickCount % FrameAdjustInterval == 0)
			{
				_lastFrameAdjust = logicTickCount;
				ClientPlayer.Send(new GGGoMsgFrameAdjust
				{
					Frame = logicTickCount,
					Time = Time.unscaledTime,
					Rtt = base.RttMs
				});
			}
		}
	}

	public override void OnGameOver()
	{
		if (GameUnityRuntime.DumpEnable)
		{
			GGGoShare.SaveCommandsToFile(Game.World, GameUnityRuntime.GetDumpPath("cmd_pvp_rollback.json"));
			_debugReplay.LevelPath = Env.InitData.LevelPath;
			_debugReplay.Commands = Env.Commands.Commands;
			File.WriteAllText(GameUnityRuntime.GetDumpPath("cmd_pvp_replay.json"), JsonConvert.SerializeObject(_debugReplay));
		}
	}

	private int HandleMsgReconnect(GGGoMsgReconnectResp msg)
	{
		_serverFrameSync = msg.LogicTickLockStep;
		for (int i = 0; i < msg.Commands.Count; i++)
		{
			GGGoMsgFrameSyncResp.CmdState cmdState = msg.Commands[i];
			if (cmdState.SequenceID >= serverSynCommands.Count)
			{
				SyncCommand item = new SyncCommand
				{
					FrameIndex = cmdState.FrameIndex,
					EntityID = cmdState.EntityID,
					CommandType = cmdState.CommandType,
					SequenceID = cmdState.SequenceID
				};
				serverSynCommands.Add(item);
			}
		}
		clientPredictCommands.Clear();
		return _serverFrameSync;
	}

	private int HandleMsgFrameSync(GGGoMsgFrameSyncResp msg)
	{
		bool flag = true;
		_serverFrameSync = msg.LogicTickLockStep;
		if (msg.Commands != null)
		{
			for (int i = 0; i < msg.Commands.Count; i++)
			{
				GGGoMsgFrameSyncResp.CmdState cmdState = msg.Commands[i];
				if (cmdState.SequenceID != serverSynCommands.Count)
				{
					GameFramework.Log.Error($"Handle message with invalid message sequence id: {cmdState.SequenceID}/{serverSynCommands.Count}");
					continue;
				}
				SyncCommand syncCommand = new SyncCommand
				{
					FrameIndex = cmdState.FrameIndex,
					EntityID = cmdState.EntityID,
					CommandType = cmdState.CommandType,
					SequenceID = cmdState.SequenceID
				};
				serverSynCommands.Add(syncCommand);
				flag &= MergeClientPredictCommand(syncCommand);
			}
		}
		if (flag)
		{
			return int.MaxValue;
		}
		return msg.LogicTickLockStep;
	}

	private bool MergeClientPredictCommand(SyncCommand cmd)
	{
		if (clientPredictCommands.Count == 0)
		{
			return false;
		}
		SyncCommand syncCommand = clientPredictCommands[0];
		if (cmd.Equals(syncCommand))
		{
			syncCommand.SequenceID = cmd.SequenceID;
			clientPredictCommands.RemoveAt(0);
			return true;
		}
		int num = -1;
		for (int i = 0; i < clientPredictCommands.Count; i++)
		{
			SyncCommand syncCommand2 = clientPredictCommands[i];
			if (syncCommand2.FrameIndex <= cmd.FrameIndex)
			{
				syncCommand2.FrameIndex = cmd.FrameIndex;
			}
			if (num < 0 && syncCommand2.EntityID == cmd.EntityID)
			{
				num = i;
			}
		}
		if (num >= 0)
		{
			clientPredictCommands.RemoveAt(num);
		}
		return false;
	}

	private void AssignSyncCommand()
	{
		GGGoCommand.CopyTo(serverSynCommands, Env.Commands.Commands);
	}

	private void AssignPredictCommand()
	{
		GGGoCommand commands = Env.Commands;
		for (int i = 0; i < clientPredictCommands.Count; i++)
		{
			SyncCommand syncCommand = clientPredictCommands[i];
			commands.QueueEvent(syncCommand.FrameIndex, syncCommand.EntityID, syncCommand.CommandType, syncCommand.SequenceID);
		}
	}

	private void RollbackGame(int syncFrame, int targetFrame)
	{
		Env.BeginRollback(syncFrame, targetFrame);
		RestoreSnapshot();
		AssignSyncCommand();
		UpdateGameToFrame(syncFrame);
		TakeSnapshot(force: true);
		AssignPredictCommand();
		UpdateGameToFrame(targetFrame);
		Env.EndRollback();
	}

	private void UpdateGameToFrame(int targetFrame)
	{
		while (Env.LogicTickCount < targetFrame)
		{
			Game.UpdateLogicFrame();
		}
	}

	private void TakeSnapshot(bool force)
	{
		if (force || !(Time.unscaledTime - _takeSnapshotLastTime <= takeSnapshotInterval))
		{
			_takeSnapshotLastTime = Time.unscaledTime;
			Env.UseRuntimeSnapshot = true;
			if (GameUnityRuntime.DumpEnable)
			{
				_snapshot = Game.TakeSnapshot();
			}
			else
			{
				_snapshotRuntime = Game.TakeSnapshotRuntime(_snapshotRuntime);
			}
			Env.UseRuntimeSnapshot = false;
			DumpSnapshotFrame();
		}
	}

	private void DumpSnapshotFrame()
	{
		if (GameUnityRuntime.DumpEnable)
		{
			_debugReplay.Frames.Add(Env.LogicTickCount);
			File.WriteAllText(GameUnityRuntime.GetDumpPath($"{Env.LogicTickCount}.json"), _snapshot.ToJson());
		}
	}

	private void RestoreSnapshot()
	{
		Env.UseRuntimeSnapshot = true;
		if (GameUnityRuntime.DumpEnable)
		{
			Game.RestoreSnapshot(_snapshot);
		}
		else
		{
			Game.RestoreSnapshotRuntime(_snapshotRuntime);
		}
		Env.UseRuntimeSnapshot = false;
	}

	private void HandleMsgGameStart(GameClientPlayer player, GGGoEnv env, GGGoMsgStartResp msg)
	{
		if (Game.Env.LogicTickLockStep < 0)
		{
			player.Send(new GGGoMsgStart());
			StartClientPredictGame();
		}
	}

	private void StartClientPredictGame()
	{
		if (GameUnityRuntime.DumpEnable)
		{
			_debugReplay = new GGGoDebugReplay();
		}
		GameUnityRuntime.ResetDumpFolder();
		if (ClientPlayer.Players != null)
		{
			_playerRtts = new float[ClientPlayer.Players.Length];
		}
		Game.Env.LogicTickLockStep = int.MaxValue;
		TakeSnapshot(force: true);
	}

	private void InitClientPredictGame()
	{
		Game = GGGoClient.CreateGame(LevelRoot, null, EGameType.PvpClient, predict: true);
		GGGoShare.InitGameByLevel(Game, base.LevelPath);
		Game.FreezeDynamicCreateFilter();
		Game.Env.LogicTickLockStep = -1;
	}
}
