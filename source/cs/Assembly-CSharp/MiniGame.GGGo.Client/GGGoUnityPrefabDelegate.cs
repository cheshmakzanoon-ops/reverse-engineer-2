using System;
using Leopotam.EcsLite;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class GGGoUnityPrefabDelegate : TEcsPoolDelegate<ComponentUnityPrefab>, IEcsPoolDelegate, IEcsAutoReset<ComponentUnityPrefab>, IEcsAutoCopy<ComponentUnityPrefab>, IEcsAutoSnapshot<ComponentUnityPrefab>
{
	public struct Snapshot
	{
		public string Asset;

		public IGGGoCacheableResourceState State;
	}

	public Type DelegateType => typeof(ComponentUnityPrefab);

	public void AutoReset(ref ComponentUnityPrefab c, EcsWorld world, int entity)
	{
		GameObject prefab = c.GetPrefab();
		if (prefab != null && prefab.TryGetComponent<GGGoCacheableResource>(out var component))
		{
			(world.GetShared<GGGoEnvClient>().ResourceLoader as GGGoLoader).SaveCachedResourceState(world.PackEntity(entity), component);
		}
		c.Prefab = null;
		if (c.ResourceHolder != null)
		{
			c.ResourceHolder.Dispose();
			c.ResourceHolder = null;
		}
	}

	public void AutoCopy(ref ComponentUnityPrefab src, ref ComponentUnityPrefab dst)
	{
		throw new NotImplementedException();
	}

	public object TakeSnapshot(ref ComponentUnityPrefab c, EcsWorld world, int entity, object env)
	{
		return world.GetPool<ComponentResource>().Get(entity).Asset;
	}

	public void RestoreSnapshot(ref ComponentUnityPrefab c, EcsWorld world, int entity, object data, object env)
	{
		if (c.ResourceHolder != null)
		{
			c.ResourceHolder.Dispose();
			c.ResourceHolder = null;
		}
		c.Init(world, entity, data as string);
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}
}
