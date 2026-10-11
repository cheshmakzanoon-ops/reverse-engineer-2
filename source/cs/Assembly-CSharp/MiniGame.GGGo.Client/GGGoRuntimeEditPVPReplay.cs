using System.IO;
using MiniGame.Core;
using MiniGame.Core.Client;
using Newtonsoft.Json;
using UnityEngine;

namespace MiniGame.GGGo.Client;

[ExecuteAlways]
public class GGGoRuntimeEditPVPReplay : GameUnityRuntime
{
	public Transform LevelRootGO;

	private GGGoDebugReplay _replay;

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

	public override GameClientPlayer ClientPlayer { get; protected set; }

	public override GameWorld Game { get; protected set; }

	public void StartGame(string replay)
	{
		GameUnityRuntime.ResetDumpFolder();
		string value = File.ReadAllText(replay);
		_replay = JsonConvert.DeserializeObject<GGGoDebugReplay>(value);
		Game = GGGoClient.CreateGame(LevelRootGO, _replay.Commands, EGameType.PvpClient);
		GGGoShare.InitGameByLevel(Game, _replay.LevelPath);
		GGGoRuntimePVP.TrySplitLocalView("", 0, base.transform, Game.Env as GGGoEnvClient);
	}

	public override void OnGameOver()
	{
		if (GameUnityRuntime.DumpEnable)
		{
			GGGoShare.SaveCommandsToFile(Game.World, GameUnityRuntime.GetDumpPath("cmd_pvp_syn.json"));
		}
	}

	public void Update()
	{
		UpdateGameWorld();
		if (Game != null && Game.Env != null && _replay.Frames.Contains(Game.Env.LogicTickCount))
		{
			GGGoEnvClient obj = Game.Env as GGGoEnvClient;
			obj.UseRuntimeSnapshot = true;
			SnapshotWorld obj2 = Game.TakeSnapshot();
			obj.UseRuntimeSnapshot = false;
			string contents = Game.Serializer.ToJson(obj2);
			File.WriteAllText($"dump/{Game.Env.LogicTickCount}.json", contents);
		}
	}
}
