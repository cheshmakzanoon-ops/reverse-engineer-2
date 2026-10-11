using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Core;

public class SystemEventInit : IEcsInitSystem, IEcsSystem
{
	private EcsSharedInject<GameSharedEnv> _shared;

	private EcsPoolInject<ComponentEventManager> _eventPool;

	private EcsPoolInject<ComponentTriggers> _poolTriggers;

	private EcsPoolInject<ComponentActivedTriggers> _poolActiveTriggers;

	public void Init(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		if (!_shared.Value.EventManager.Unpack(world, out var _))
		{
			int entity2 = world.NewEntity();
			ref ComponentEventManager reference = ref _eventPool.Value.Add(entity2);
			reference.TargetListeners = new Dictionary<Type, List<int>>();
			reference.EventQueue = new Queue<IEvent>();
			_shared.Value.EventManager = world.PackEntity(entity2);
		}
	}
}
