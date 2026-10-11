using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo.Client;

public class SystemPrefabClient : IEcsRunSystem, IEcsSystem
{
	protected readonly EcsWorldInject _world;

	protected readonly EcsFilterInject<Inc<ComponentResource>, Exc<ComponentUnityPrefab>> _filterAdd;

	protected readonly EcsFilterInject<Inc<ComponentUnityPrefab>, Exc<ComponentResource>> _filterDel;

	protected readonly EcsPoolInject<ComponentUnityPrefab> _poolPrefab;

	protected readonly EcsPoolInject<ComponentResource> _poolRes;

	public void Run(IEcsSystems systems)
	{
		RunDel(systems);
		RunAdd(systems);
	}

	protected void RunDel(IEcsSystems systems)
	{
		foreach (int item in _filterDel.Value)
		{
			_poolPrefab.Value.Del(item);
		}
	}

	protected void RunAdd(IEcsSystems systems)
	{
		foreach (int item in _filterAdd.Value)
		{
			ref ComponentResource reference = ref _poolRes.Value.Get(item);
			_poolPrefab.Value.Add(item).Init(_world.Value, item, reference.Asset);
		}
	}
}
