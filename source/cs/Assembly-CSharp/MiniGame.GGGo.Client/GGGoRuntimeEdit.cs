using MiniGame.Core;
using MiniGame.Core.Client;
using UnityEngine;

namespace MiniGame.GGGo.Client;

[ExecuteAlways]
public class GGGoRuntimeEdit : GameUnityRuntime
{
	public new string LevelPath;

	public Transform LevelRootGO;

	public override bool IsPlaying
	{
		get
		{
			return false;
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
		LevelPath = levelPath;
		Game = GGGoClientEdit.CreateGame(LevelRootGO);
		GGGoShare.InitGameByLevel(Game, levelPath);
	}

	public void Update()
	{
		UpdateGameWorld();
	}

	public new void OnDestroy()
	{
		ExitGame();
	}
}
