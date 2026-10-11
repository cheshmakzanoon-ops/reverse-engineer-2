using Leopotam.EcsLite;
using MiniGame.Core;
using Spine.Unity;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public static class FuncClient
{
	public static void PlaySkeletonAnimation(EcsWorld world, int entity, string animationName, bool loop = false)
	{
		EcsPool<ComponentUnityPrefab> pool = world.GetPool<ComponentUnityPrefab>();
		if (!pool.Has(entity))
		{
			return;
		}
		ComponentUnityPrefab comp = pool.Get(entity);
		GameObject prefab = comp.GetPrefab();
		if (prefab == null)
		{
			return;
		}
		if (prefab.TryGetComponent<GGGoCacheableSpine>(out var component))
		{
			component.PlayAnimation(animationName, loop);
			return;
		}
		SkeletonAnimation componentInChildren = prefab.GetComponentInChildren<SkeletonAnimation>();
		if (!(componentInChildren == null))
		{
			SkeletonBundlePlayer component2 = componentInChildren.gameObject.GetComponent<SkeletonBundlePlayer>();
			if (component2 != null)
			{
				component2.Play(animationName);
			}
			else
			{
				componentInChildren.AnimationState.SetAnimation(0, animationName, loop);
			}
		}
	}
}
