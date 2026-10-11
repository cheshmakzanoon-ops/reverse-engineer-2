using MiniGame.Core;
using MiniGame.Core.Client;
using UnityEngine;

namespace MiniGame.GGGo.Client;

[ExecuteAlways]
public class GGGoRuntime : GameUnityRuntime
{
	public Transform LevelRootGO;

	public override bool IsPlaying
	{
		get
		{
			return IsRunning;
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
		Game = GGGoClient.CreateGame(LevelRootGO);
		GGGoShare.InitGameByLevel(Game, levelPath);
	}

	public void Update()
	{
		Game?.Update(Time.deltaTime);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
