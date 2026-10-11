using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public static class FuncAction
{
	public static void RegisterAction<T>(EcsWorld world, Action<EcsWorld, int, IAction, IEvent> actionFunc) where T : IAction
	{
		Dictionary<Type, Action<EcsWorld, int, IAction, IEvent>> dict = GetDict(world);
		Type typeFromHandle = typeof(T);
		if (dict.ContainsKey(typeFromHandle))
		{
			Dictionary<Type, Action<EcsWorld, int, IAction, IEvent>> dictionary = dict;
			Type key = typeFromHandle;
			dictionary[key] = (Action<EcsWorld, int, IAction, IEvent>)Delegate.Combine(dictionary[key], actionFunc);
		}
		else
		{
			dict[typeFromHandle] = actionFunc;
		}
	}

	public static void UnregisterAction<T>(EcsWorld world, Action<EcsWorld, int, IAction, IEvent> actionFunc) where T : IAction
	{
		Dictionary<Type, Action<EcsWorld, int, IAction, IEvent>> dict = GetDict(world);
		Type typeFromHandle = typeof(T);
		if (dict.ContainsKey(typeFromHandle))
		{
			Dictionary<Type, Action<EcsWorld, int, IAction, IEvent>> dictionary = dict;
			Type key = typeFromHandle;
			dictionary[key] = (Action<EcsWorld, int, IAction, IEvent>)Delegate.Remove(dictionary[key], actionFunc);
		}
	}

	public static void Clear(EcsWorld world)
	{
		GetDict(world).Clear();
	}

	private static Dictionary<Type, Action<EcsWorld, int, IAction, IEvent>> GetDict(EcsWorld world)
	{
		return world.GetShared<GameSharedEnv>().ActionDict;
	}

	public static void DoActions(EcsWorld world, int entity, Trigger trigger, IEvent e)
	{
		for (int i = 0; i < trigger.Actions.Count; i++)
		{
			IAction action = trigger.Actions[i];
			DoAction(world, entity, action, e);
		}
	}

	public static void DoActions(EcsWorld world, int entity, List<IAction> actions, IEvent e = null)
	{
		for (int i = 0; i < actions.Count; i++)
		{
			IAction action = actions[i];
			DoAction(world, entity, action, e);
		}
	}

	public static void DoAction(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (GetDict(world).TryGetValue(action.GetType(), out var value))
		{
			value(world, entity, action, e);
		}
		else
		{
			world.LogError($"未注册的Action类型: {action.GetType()}");
		}
	}

	public static int GetTarget(EcsWorld world, int entity, IEvent e, EventTarget target = EventTarget.Entity)
	{
		int entity2 = entity;
		switch (target)
		{
		case EventTarget.Entity:
			entity2 = entity;
			break;
		case EventTarget.Sender:
			e.Sender.Unpack(world, out entity2);
			break;
		case EventTarget.Target:
			e.Target.Unpack(world, out entity2);
			break;
		}
		return entity2;
	}
}
