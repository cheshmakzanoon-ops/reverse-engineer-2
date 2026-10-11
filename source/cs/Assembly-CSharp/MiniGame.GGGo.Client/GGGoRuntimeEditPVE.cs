using MiniGame.Core;
using MiniGame.Core.Client;
using UnityEngine;

namespace MiniGame.GGGo.Client;

[ExecuteAlways]
public class GGGoRuntimeEditPVE : GameUnityRuntime
{
	public Transform LevelRootGO;

	protected GGGoReplay _replay;

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

	public IResourceLoader ResourceLoader => Game?.Env.ResourceLoader;

	public bool IsRunning => Game != null;

	public override GameClientPlayer ClientPlayer { get; protected set; }

	public void StartGame(string levelPath)
	{
		GGGoReplay replay = new GGGoReplay
		{
			LevelPath = levelPath,
			Commands = null,
			MD5 = ""
		};
		StartGame(replay);
	}

	public void StartGame(GGGoReplay replay, EGameType gameType = EGameType.PveClient)
	{
		_replay = replay;
		Game = GGGoClient.CreateGame(LevelRootGO, replay.Commands, gameType);
		GGGoShare.InitGameByLevel(Game, replay.LevelPath);
	}

	public void Update()
	{
		UpdateGameWorld();
	}

	public new void OnDestroy()
	{
		ExitGame();
	}

	public override void OnGameOver()
	{
		string.IsNullOrEmpty(_replay.MD5);
	}
}
