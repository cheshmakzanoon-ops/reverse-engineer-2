using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemItem : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentItem, ComponentActivedUniqueID>, Exc<ComponentTriggers>> _filterInitItem;

	protected readonly EcsPoolInject<ComponentItem> _poolItem;

	protected readonly EcsPoolInject<ComponentTriggers> _poolTriggers;

	public void Run(IEcsSystems systems)
	{
		foreach (int item in _filterInitItem.Value)
		{
			InitBoxItem(item);
		}
	}

	private void InitBoxItem(int entity)
	{
		ComponentItem componentItem = _poolItem.Value.Get(entity);
		if (componentItem.ItemType == ItemType.Box)
		{
			GGGoEnv value = _env.Value;
			ref ComponentTriggers reference = ref _poolTriggers.Value.Add(entity);
			reference.Triggers = new List<Trigger>();
			Trigger item = new Trigger
			{
				Events = new List<Type> { typeof(EventTriggerEnter) },
				Conditions = new List<ICondition>
				{
					new EventWithMeCondition
					{
						EvenLaunchTargetType = EvenLaunchTargetType.Send
					},
					new CollectionLayerCondition
					{
						Layer = (ColliderLayer.Player | ColliderLayer.Enemy)
					}
				},
				Actions = new List<IAction>
				{
					new PickItemAction
					{
						ItemType = componentItem.ItemType,
						Target = EventTarget.Target
					},
					new AddBuffAction(BuffId.Disappear, value.Level.ItemRebornDelay)
				}
			};
			reference.Triggers.Add(item);
		}
	}
}
