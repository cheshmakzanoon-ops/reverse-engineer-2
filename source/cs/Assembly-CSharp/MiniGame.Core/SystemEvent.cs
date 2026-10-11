using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Core;

public class SystemEvent : IEcsRunSystem, IEcsSystem
{
	private EcsSharedInject<GameSharedEnv> _shared;

	private EcsPoolInject<ComponentEventManager> _eventPool;

	private EcsPoolInject<ComponentTriggers> _poolTriggers;

	private EcsPoolInject<ComponentActivedTriggers> _poolActiveTriggers;

	public void Run(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		if (!_shared.Value.EventManager.Unpack(world, out var entity))
		{
			throw new Exception("EventManager not init");
		}
		ref ComponentEventManager reference = ref _eventPool.Value.Get(entity);
		int num = 0;
		bool flag = false;
		while (reference.EventQueue.Count > 0)
		{
			num++;
			if (num > 1000)
			{
				throw new Exception("Too many events, may be looped!");
			}
			IEvent obj = reference.EventQueue.Dequeue();
			Type type = obj.GetType();
			if (reference.TargetListeners.TryGetValue(type, out var value))
			{
				switch (obj.EventType)
				{
				case TriggerEventType.Local:
					flag |= BroadCastToEntity(world, obj.Sender, obj, value);
					flag |= BroadCastToEntity(world, obj.Target, obj, value);
					break;
				case TriggerEventType.TargetOnly:
					flag |= BroadCastToEntity(world, obj.Target, obj, value);
					break;
				case TriggerEventType.SenderOnly:
					flag |= BroadCastToEntity(world, obj.Sender, obj, value);
					break;
				case TriggerEventType.Broadcast:
				{
					for (int num2 = value.Count - 1; num2 >= 0; num2--)
					{
						flag |= BroadCastToEntity(world, value[num2], obj);
					}
					break;
				}
				}
			}
			BroadCastToExternal(world, obj);
		}
		if (flag)
		{
			_shared.Value.FrameSyncIsNeeded = true;
		}
	}

	private bool BroadCastToEntity(EcsWorld world, EcsPackedEntity entity, IEvent e, List<int> listeners)
	{
		if (entity.Unpack(world, out var entity2))
		{
			int num = FindEntityInListeners(listeners, entity2);
			if (num >= 0)
			{
				return BroadCastToEntity(world, num, e);
			}
		}
		return false;
	}

	private bool BroadCastToEntity(EcsWorld world, int entity, IEvent e)
	{
		bool result = false;
		EcsPool<ComponentActivedTriggers> value = _poolActiveTriggers.Value;
		EcsPool<ComponentTriggers> value2 = _poolTriggers.Value;
		ref ComponentActivedTriggers reference = ref value.Get(entity);
		ref ComponentTriggers reference2 = ref value2.Get(entity);
		List<int> list = reference.Triggers[e.GetType()];
		ref List<Trigger> triggers = ref reference2.Triggers;
		for (int i = 0; i < list.Count; i++)
		{
			if (triggers != null && triggers.Count > 0)
			{
				Trigger trigger = triggers[list[i]];
				if (!trigger.IsExhausted && FuncCondition.CheckConditions(world, entity, trigger, e))
				{
					result = true;
					trigger.TriggerCount++;
					FuncAction.DoActions(world, entity, trigger, e);
				}
			}
		}
		return result;
	}

	private void BroadCastToExternal(EcsWorld world, IEvent e)
	{
		Dictionary<Type, Action<EcsWorld, IEvent>> eventDict = _shared.Value.EventDict;
		if (eventDict != null && eventDict.TryGetValue(e.GetType(), out var value) && value != null)
		{
			try
			{
				value(world, e);
			}
			catch (Exception arg)
			{
				world.LogError($"Event callback exception: {arg}");
			}
		}
	}

	private int FindEntityInListeners(List<int> listeners, int targetEntity)
	{
		if (listeners == null || targetEntity < 0)
		{
			return -1;
		}
		for (int i = 0; i < listeners.Count; i++)
		{
			if (listeners[i] == targetEntity)
			{
				return listeners[i];
			}
		}
		return -1;
	}
}
