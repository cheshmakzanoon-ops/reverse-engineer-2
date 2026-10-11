using System.Collections.Generic;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.Core.Client;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public static class GGGoClient
{
	public static GameSerializer Serializer = new GameSerializer(new GameGGGoJsonTypeBinder(), null, new List<IEcsPoolDelegate>
	{
		new GGGoUnityPrefabDelegate()
	});

	public static GameWorld CreateGame(Transform levelRoot, List<SyncCommand> commands = null, EGameType gameType = EGameType.PveClient, bool predict = false)
	{
		GameWorld gameWorld = new GameWorld(new GGGoEnvClient(), Serializer, new EcsUnityDebugger());
		GGGoEnvClient gGGoEnvClient = gameWorld.Env as GGGoEnvClient;
		gGGoEnvClient.GameType = gameType;
		IResourceLoader resource = CreateResourceLoader();
		GGGoShare.InitEnv(commands, resource, gGGoEnvClient);
		IResourceHolder resourceHolder = gameWorld.Env.ResourceLoader.LoadAsset<GameObject>("Assets/Main/MiniGameRes/GGGo/Res/GGGoScene.prefab");
		gGGoEnvClient.Scene = Object.Instantiate(resourceHolder.As<GameObject>(), levelRoot).GetComponent<GGGoScene>();
		gGGoEnvClient.Scene.AdjustScale();
		if (predict)
		{
			gameWorld.LogicSystems.Add(new SystemInputClientPredict());
		}
		else
		{
			gameWorld.LogicSystems.Add(new SystemInputClient());
		}
		GGGoShare.InitComponents(gameWorld);
		GGGoShare.InitSystems(gameWorld);
		gameWorld.PrepareSystems.Add(new SystemUIClient()).Add(new SystemWaitPlayer());
		gameWorld.ViewSystems.Add(new SystemUIClient()).Add(new SystemClientGame()).Add(new SystemPrefabClient())
			.Add(new SystemAutoRollClient())
			.Add(new SystemGameOverCinematicClient())
			.Add(new SystemPositionClient())
			.Add(new SystemFireRender());
		if (GGGoEnv.Dump)
		{
			gameWorld.LogicSystems.Add(GGGoShare.CreateRuntimeDumpSystem("DumpClientRuntime.json"));
		}
		return gameWorld;
	}

	public static IResourceLoader CreateResourceLoader(bool cache = true)
	{
		return new GGGoLoader(Serializer, "Assets/Main/MiniGameRes/GGGo/Template/{0}.txt", "Assets/Main/MiniGameRes/GGGo/Map/{0}.txt", cache);
	}
}
