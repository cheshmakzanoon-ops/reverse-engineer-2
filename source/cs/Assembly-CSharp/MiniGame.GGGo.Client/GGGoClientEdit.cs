using System.Collections.Generic;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.Core.Client;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public static class GGGoClientEdit
{
	public static GameSerializer Serializer = new GameSerializer(new GameGGGoJsonTypeBinder(), null, new List<IEcsPoolDelegate>
	{
		new GGGoUnityPrefabDelegate()
	});

	public static GameWorld CreateGame(Transform levelRoot)
	{
		GameWorld gameWorld = new GameWorld(new GGGoEnvClient(), Serializer, new EcsUnityDebugger());
		GGGoEnvClient gGGoEnvClient = gameWorld.Env as GGGoEnvClient;
		gGGoEnvClient.GameType = EGameType.PveClient;
		IResourceLoader resource = GGGoClient.CreateResourceLoader(cache: false);
		GGGoShare.InitEnv(null, resource, gGGoEnvClient);
		IResourceHolder resourceHolder = gameWorld.Env.ResourceLoader.LoadAsset<GameObject>("Assets/Main/MiniGameRes/GGGo/Res/GGGoScene.prefab");
		gGGoEnvClient.Scene = Object.Instantiate(resourceHolder.As<GameObject>(), levelRoot).GetComponent<GGGoScene>();
		gGGoEnvClient.Scene.AdjustScale();
		UIGGGoMain componentInChildren = levelRoot.GetComponentInChildren<UIGGGoMain>(includeInactive: true);
		if ((bool)componentInChildren)
		{
			componentInChildren.gameObject.SetActive(value: true);
			componentInChildren.BindUI(gameWorld, isEdit: true);
		}
		GGGoShare.InitComponents(gameWorld);
		gameWorld.LogicSystems.Add(new SystemClientPhysicsSimulateDebug()).Add(new SystemTemplateInstantiate()).Add(new SystemActivity())
			.Add(new SystemUniqueIDWithAutoID());
		gameWorld.ViewSystems.Add(new SystemPrefabClient());
		return gameWorld;
	}
}
