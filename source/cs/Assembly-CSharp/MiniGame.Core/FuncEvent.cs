using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public static class FuncEvent
{
	public static ref ComponentEventManager GetComponentEventManager(EcsWorld world)
	{
		world.GetShared<IGameSharedEnv>().EventManager.Unpack(world, out var entity);
		return ref world.GetPool<ComponentEventManager>().Get(entity);
	}

	public static bool HasEventManager(EcsWorld world)
	{
		int entity;
		return world.GetShared<IGameSharedEnv>().EventManager.Unpack(world, out entity);
	}

	public static void Broadcast(EcsWorld world, IEvent e)
	{
		if (HasEventManager(world))
		{
			Broadcast(ref GetComponentEventManager(world), e);
		}
	}

	public static void Broadcast(ref ComponentEventManager evtMgr, IEvent e)
	{
		evtMgr.EventQueue.Enqueue(e);
	}

	public static void Subscribe(ref ComponentEventManager evtMgr, int entity, int index, Dictionary<Type, List<int>> regs, Type eventType)
	{
		if (!regs.TryGetValue(eventType, out var value))
		{
			value = (regs[eventType] = new List<int>());
		}
		if (!value.Contains(index))
		{
			value.Add(index);
		}
		if (!evtMgr.TargetListeners.TryGetValue(eventType, out var value2))
		{
			value2 = new List<int>();
			evtMgr.TargetListeners.Add(eventType, value2);
		}
		if (!value2.Contains(entity))
		{
			value2.Add(entity);
		}
	}

	public static void Subscribe(ref ComponentEventManager evtMgr, int entity, int index, Dictionary<Type, List<int>> regs, Trigger trigger)
	{
		for (int i = 0; i < trigger.Events.Count; i++)
		{
			Subscribe(ref evtMgr, entity, index, regs, trigger.Events[i]);
		}
	}

	public static void UnSubscribe(ref ComponentEventManager evtMgr, int entity, Dictionary<Type, List<int>> triggers)
	{
		foreach (KeyValuePair<Type, List<int>> trigger in triggers)
		{
			if (evtMgr.TargetListeners.TryGetValue(trigger.Key, out var value) && value.Contains(entity))
			{
				value.Remove(entity);
			}
		}
	}

	public static void RegisterEvent<T>(EcsWorld world, Action<EcsWorld, IEvent> callback) where T : IEvent
	{
		Dictionary<Type, Action<EcsWorld, IEvent>> eventDict = world.GetShared<GameSharedEnv>().EventDict;
		Type typeFromHandle = typeof(T);
		if (eventDict.ContainsKey(typeFromHandle))
		{
			Dictionary<Type, Action<EcsWorld, IEvent>> dictionary = eventDict;
			Type key = typeFromHandle;
			dictionary[key] = (Action<EcsWorld, IEvent>)Delegate.Combine(dictionary[key], callback);
		}
		else
		{
			eventDict[typeFromHandle] = callback;
		}
	}

	public static void UnregisterEvent<T>(EcsWorld world, Action<EcsWorld, IEvent> callback) where T : IEvent
	{
		Dictionary<Type, Action<EcsWorld, IEvent>> eventDict = world.GetShared<GameSharedEnv>().EventDict;
		Type typeFromHandle = typeof(T);
		if (eventDict.ContainsKey(typeFromHandle))
		{
			Dictionary<Type, Action<EcsWorld, IEvent>> dictionary = eventDict;
			Type key = typeFromHandle;
			dictionary[key] = (Action<EcsWorld, IEvent>)Delegate.Remove(dictionary[key], callback);
		}
	}
}
