using System;
using Leopotam.EcsLite;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public static class GGGoUnityPrefabExt
{
	public static GameObject GetPrefab(this ref ComponentUnityPrefab comp)
	{
		if (comp.Prefab != null)
		{
			return (GameObject)comp.Prefab;
		}
		if (comp.ResourceHolder == null)
		{
			return null;
		}
		if (!comp.ResourceHolder.IsError && comp.ResourceHolder.IsValid && comp.ResourceHolder.IsDone && comp.ResourceHolder is GGGoAssetHolder gGGoAssetHolder)
		{
			comp.Prefab = gGGoAssetHolder.GetInstance<GameObject>();
			return (GameObject)comp.Prefab;
		}
		return null;
	}

	public static void RemovePrefab(this ref ComponentUnityPrefab comp)
	{
		if (comp.ResourceHolder != null)
		{
			comp.ResourceHolder.Dispose();
			comp.ResourceHolder = null;
		}
		comp.Prefab = null;
	}

	public static void Init(this ref ComponentUnityPrefab comp, EcsWorld world, int entity, string asset)
	{
		GGGoEnvClient obj = world.GetShared() as GGGoEnvClient;
		EcsPackedEntityWithWorld owner = world.PackEntityWithWorld(entity);
		if (!(obj?.ResourceLoader.LoadAssetAsync<GameObject>(owner, asset) is GGGoAssetHolder gGGoAssetHolder))
		{
			return;
		}
		comp.ResourceHolder = gGGoAssetHolder;
		if (gGGoAssetHolder.IsDone)
		{
			InitGameObject(gGGoAssetHolder, world, entity, asset);
			return;
		}
		gGGoAssetHolder.OnDone = (Action<IResourceHolder>)Delegate.Combine(gGGoAssetHolder.OnDone, (Action<IResourceHolder>)delegate(IResourceHolder iholder)
		{
			InitGameObject(iholder as GGGoAssetHolder, world, entity, asset);
		});
	}

	private static void InitGameObject(GGGoAssetHolder holder, EcsWorld world, int entity, string asset)
	{
		GameObject gameObject = holder.AsInstance<GameObject>();
		if (world.GetShared() is GGGoEnvClient gGGoEnvClient)
		{
			if (world.GetPool<ComponentPlayer>().Has(entity))
			{
				gameObject.transform.SetParent(gGGoEnvClient.Scene.PlayerRoot);
			}
			else
			{
				gameObject.transform.SetParent(gGGoEnvClient.Scene.DynamicRoot);
			}
		}
		if (world.GetPool<ComponentPosition>().Has(entity))
		{
			ref ComponentPosition reference = ref world.GetPool<ComponentPosition>().Get(entity);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = new Vector3((float)reference.Position.X, (float)reference.Position.Y, gameObject.transform.localPosition.z);
		}
		ref ComponentResource res = ref world.GetPool<ComponentResource>().Get(entity);
		world.PackEntity(entity);
		TryInitCacheableResource(world, entity, ref res, gameObject);
		TryInitPlayerController(holder, world, entity, gameObject);
	}

	private static void TryInitCacheableResource(EcsWorld world, int entity, ref ComponentResource res, GameObject go)
	{
		if (!(go == null) && go.TryGetComponent<GGGoCacheableResource>(out var component))
		{
			EcsPackedEntity entity2 = world.PackEntity(entity);
			GGGoLoader obj = world.GetShared<GGGoEnv>().ResourceLoader as GGGoLoader;
			component.BindEntity(world.PackEntityWithWorld(entity));
			IGGGoCacheableResourceState state = obj.LoadCachedResourceState(entity2);
			component.LoadState(state);
		}
	}

	private static void TryInitPlayerController(GGGoAssetHolder holder, EcsWorld world, int entity, GameObject prefab)
	{
		EcsPool<ComponentPlayer> pool = world.GetPool<ComponentPlayer>();
		if (pool.Has(entity))
		{
			ref ComponentPlayer reference = ref pool.Get(entity);
			UIGGGoPlayerController componentInChildren = prefab.GetComponentInChildren<UIGGGoPlayerController>();
			if (componentInChildren == null)
			{
				world.LogError($"[GGGo] Player Prefab Not Found Component UIGGGoPlayerController Entity:{entity}");
				return;
			}
			componentInChildren.Init(world.PackEntityWithWorld(entity));
			DataUIRender.UIPlayerBind uIPlayerBind = DataUIRender.UIRender<DataUIRender.UIPlayerBind>.Fetch();
			uIPlayerBind.Controller = componentInChildren;
			FuncUI.FireRender(world, uIPlayerBind);
			DataUIRender.UIHpChange uIHpChange = DataUIRender.UIRender<DataUIRender.UIHpChange>.Fetch();
			uIHpChange.PlayerInfo = new UIPlayerInfo
			{
				PlayerID = reference.PlayerID,
				CurHp = FuncData.GetFloatData(world, entity, PropertyID.CurHp),
				MaxHp = FuncData.GetIntData(world, entity, PropertyID.MaxHp)
			};
			FuncUI.FireRender(world, uIHpChange);
		}
	}
}
