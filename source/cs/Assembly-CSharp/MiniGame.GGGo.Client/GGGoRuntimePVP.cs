using System;
using System.Collections;
using Joker;
using MiniGame.Core;
using MiniGame.Core.Client;
using MiniGame.Core.Server;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class GGGoRuntimePVP : GameUnityRuntime
{
	public Action<string> OnError;

	public Action OnFinish;

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

	public void Update()
	{
		GGGoEnvClient gGGoEnvClient = Game?.Env as GGGoEnvClient;
		GGGoClientPlayer gGGoClientPlayer = gGGoEnvClient?.Player;
		(IMessage, object) message;
		while (gGGoClientPlayer != null && gGGoClientPlayer.TryDequeueMessage(out message) && message.Item1 is RoomMessage roomMessage)
		{
			if (roomMessage.Data is GGGoMsgFrameSyncResp gGGoMsgFrameSyncResp)
			{
				GGGoCommand commands = gGGoEnvClient.Commands;
				Game.Env.LogicTickLockStep = gGGoMsgFrameSyncResp.LogicTickLockStep;
				if (gGGoMsgFrameSyncResp.Commands != null)
				{
					for (int i = 0; i < gGGoMsgFrameSyncResp.Commands.Count; i++)
					{
						GGGoMsgFrameSyncResp.CmdState cmdState = gGGoMsgFrameSyncResp.Commands[i];
						commands.TryQueueEvent(cmdState.FrameIndex, cmdState.EntityID, cmdState.CommandType, cmdState.SequenceID);
					}
				}
			}
			else if (roomMessage.Data is GGGoMsgStartResp)
			{
				gGGoClientPlayer.Send(new GGGoMsgStart());
			}
			else if (!(roomMessage.Data is GGGoMsgEnterResp))
			{
			}
		}
		UpdateGameWorld();
	}

	public void StartGame(Transform root, string levelPath, string address, string uid, string sid, string uuid, string playerSessionId, string roomSessionId, int index = 0)
	{
		address = (string.IsNullOrEmpty(address) ? "127.0.0.1:7758" : address);
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
		InitClientSyncGame();
		TrySplitLocalView(roomSessionId, index, base.transform, Env);
		StartCoroutine(StartGameCoroutine());
	}

	public void SetUpVersion(string logicVersion, string resVersion)
	{
		base.LogicVersion = logicVersion;
		base.ResVersion = resVersion;
		UpdatePlayerVersionImpl();
	}

	private void UpdatePlayerVersionImpl()
	{
		if (ClientPlayer != null)
		{
			ClientPlayer.LogicVersion = base.LogicVersion;
			ClientPlayer.ResourceVersion = base.ResVersion;
		}
	}

	public static void TrySplitLocalView(string roomSessionId, int index, Transform trans, GGGoEnvClient env)
	{
		if (string.IsNullOrWhiteSpace(roomSessionId) || roomSessionId == "local_pvp_room")
		{
			trans.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			Vector2 sizeDelta = trans.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta;
			Vector3 localPosition = trans.localPosition;
			localPosition.x = sizeDelta.x * (float)(-1 + 2 * index) / 4f;
			trans.localPosition = localPosition;
		}
		else
		{
			trans.localScale = Vector3.one;
			trans.localPosition = Vector3.zero;
		}
	}

	public override void OnGameOver()
	{
		if (GameUnityRuntime.DumpEnable)
		{
			GGGoShare.SaveCommandsToFile(Game.World, GameUnityRuntime.GetDumpPath("cmd_pvp_syn.json"));
		}
	}

	public override void ExitGame()
	{
		base.ExitGame();
		OnFinish = null;
		OnError = null;
	}

	protected override IEnumerator StartGameCoroutine()
	{
		return base.StartGameCoroutine();
	}

	public override void HandleServerConnected()
	{
		GGGoMsgEnter gGGoMsgEnter = new GGGoMsgEnter();
		gGGoMsgEnter.LevelPath = base.LevelPath;
		gGGoMsgEnter.MaxPlayers = 2;
		ClientPlayer.Send(gGGoMsgEnter);
		OnFinish?.Invoke();
	}

	private void InitClientSyncGame()
	{
		Game = GGGoClient.CreateGame(LevelRoot, null, EGameType.PvpClient);
		GGGoShare.InitGameByLevel(Game, base.LevelPath);
		Game.FreezeDynamicCreateFilter();
		Game.Env.LogicTickLockStep = -1;
	}
}
