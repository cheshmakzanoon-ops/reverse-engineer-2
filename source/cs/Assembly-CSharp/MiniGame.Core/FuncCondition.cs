using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public static class FuncCondition
{
	public static void RegisterCondition<T>(EcsWorld world, Func<EcsWorld, int, Trigger, ICondition, IEvent, bool> conditionFunc) where T : ICondition
	{
		GetDict(world)[typeof(T)] = conditionFunc;
	}

	public static void Clear(EcsWorld world)
	{
		GetDict(world).Clear();
	}

	private static Dictionary<Type, Func<EcsWorld, int, Trigger, ICondition, IEvent, bool>> GetDict(EcsWorld world)
	{
		return world.GetShared<GameSharedEnv>().ConditionDict;
	}

	public static bool CheckConditions(EcsWorld world, int entity, Trigger trigger, IEvent e)
	{
		return CheckConditions(world, entity, trigger.Conditions, trigger, e);
	}

	public static bool CheckConditions(EcsWorld world, int entity, List<ICondition> Conditions, Trigger trigger = null, IEvent e = null)
	{
		if (Conditions == null || Conditions.Count == 0)
		{
			return true;
		}
		foreach (ICondition Condition in Conditions)
		{
			if (!CheckCondition(world, entity, trigger, Condition, e))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CheckCondition(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (GetDict(world).TryGetValue(condition.GetType(), out var value))
		{
			return value(world, entity, trigger, condition, e);
		}
		world.LogError($"未注册的Condition类型: {condition.GetType()}");
		return false;
	}

	public static bool Comparable<T>(ConditionOp conditionOp, T intParam1, T intParam2) where T : IComparable<T>, IEquatable<T>
	{
		int num = intParam1.CompareTo(intParam2);
		return conditionOp switch
		{
			ConditionOp.Equal => intParam1.Equals(intParam2), 
			ConditionOp.NotEqual => !intParam1.Equals(intParam2), 
			ConditionOp.GreaterThan => num > 0, 
			ConditionOp.GreaterThanOrEqual => num >= 0, 
			ConditionOp.LessThan => num < 0, 
			ConditionOp.LessThanOrEqual => num <= 0, 
			_ => throw new Exception($"not support {conditionOp}"), 
		};
	}

	public static bool Compare(ConditionOp op, FP lhs, FP rhs)
	{
		return op switch
		{
			ConditionOp.Equal => lhs == rhs, 
			ConditionOp.NotEqual => lhs != rhs, 
			ConditionOp.GreaterThan => lhs > rhs, 
			ConditionOp.GreaterThanOrEqual => lhs >= rhs, 
			ConditionOp.LessThan => lhs < rhs, 
			ConditionOp.LessThanOrEqual => lhs <= rhs, 
			_ => throw new Exception($"not support {op}"), 
		};
	}
}
