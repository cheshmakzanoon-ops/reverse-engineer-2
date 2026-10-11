using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class SystemPositionClient : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentPosition, ComponentUnityPrefab>, Exc<ComponentStatic>> _filterPos;

	protected readonly EcsPoolInject<ComponentPosition> _poolPos;

	protected readonly EcsPoolInject<ComponentUnityPrefab> _poolPrefab;

	public void Run(IEcsSystems systems)
	{
		float asFloat = _env.Value.LogicTickDelta.AsFloat;
		foreach (int item in _filterPos.Value)
		{
			ref ComponentPosition reference = ref _poolPos.Value.Get(item);
			GameObject prefab = _poolPrefab.Value.Get(item).GetPrefab();
			if (prefab == null)
			{
				continue;
			}
			Vector3 vector = new Vector3((float)reference.Position.X, (float)reference.Position.Y, prefab.transform.localPosition.z);
			if (prefab != null)
			{
				if (prefab.TryGetComponent<GGGoSmoothMovable>(out var component))
				{
					component.UpdatePosition(vector, asFloat);
				}
				else
				{
					prefab.transform.localPosition = vector;
				}
			}
		}
	}
}
