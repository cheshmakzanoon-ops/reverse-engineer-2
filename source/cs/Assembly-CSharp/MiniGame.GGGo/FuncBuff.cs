using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

public static class FuncBuff
{
	public static float GetRemaining(EcsWorld world, int targetEntity, int buffId)
	{
		EcsPool<ComponentBuffs> pool = world.GetPool<ComponentBuffs>();
		if (!pool.Has(targetEntity))
		{
			return 0f;
		}
		ref ComponentBuffs reference = ref pool.Get(targetEntity);
		int num = FindIndex(in reference, buffId);
		if (num < 0)
		{
			return 0f;
		}
		BuffRuntime buffRuntime = reference.Buffs[num];
		FP fP = buffRuntime.Duration - buffRuntime.ElapsedTime;
		if (!(fP > FP.Zero))
		{
			return 0f;
		}
		return fP.AsFloat;
	}

	public static bool Has(EcsWorld world, int targetEntity, int buffId)
	{
		EcsPool<ComponentBuffs> pool = world.GetPool<ComponentBuffs>();
		if (!pool.Has(targetEntity))
		{
			return false;
		}
		ref ComponentBuffs reference = ref pool.Get(targetEntity);
		if (reference.Buffs == null || reference.Buffs.Count == 0)
		{
			return false;
		}
		return FindIndex(in reference, buffId) >= 0;
	}

	public static bool GetBuffDataShared(EcsWorld world, int buffId, out BuffData buffData)
	{
		GGGoEnv shared = world.GetShared<GGGoEnv>();
		if (shared.BuffDatas[buffId] == null)
		{
			buffData = new BuffData(buffId);
			shared.BuffDatas[buffId] = buffData;
			return false;
		}
		buffData = shared.BuffDatas[buffId];
		return true;
	}

	public static BuffRuntime Add(EcsWorld world, int targetEntity, BuffData buff, FP durationOverride)
	{
		if (targetEntity < 0)
		{
			return null;
		}
		ref ComponentBuffs orAdd = ref world.GetPool<ComponentBuffs>().GetOrAdd(targetEntity);
		if (orAdd.Buffs == null)
		{
			orAdd.Buffs = new List<BuffRuntime>();
		}
		int num = FindIndex(in orAdd, buff.BuffID);
		if (num >= 0)
		{
			BuffRuntime buffRuntime = orAdd.Buffs[num];
			buffRuntime.ElapsedTime = FP.Zero;
			buffRuntime.ElapsedIntervalTime = FP.Zero;
			buffRuntime.DurationOverride = durationOverride;
			orAdd.Buffs[num] = buffRuntime;
			return buffRuntime;
		}
		BuffRuntime buffRuntime2 = new BuffRuntime(buff);
		buffRuntime2.DurationOverride = durationOverride;
		orAdd.Buffs.Add(buffRuntime2);
		ExecuteActions(world, targetEntity, buffRuntime2.BuffData.OnApplyActions);
		ExecuteActions(world, targetEntity, buffRuntime2.OnApplyActions);
		return buffRuntime2;
	}

	public static bool Remove(EcsWorld world, int targetEntity, int buffId, bool executeExpireActions = true)
	{
		EcsPool<ComponentBuffs> pool = world.GetPool<ComponentBuffs>();
		if (!pool.Has(targetEntity))
		{
			return false;
		}
		ref ComponentBuffs reference = ref pool.Get(targetEntity);
		if (reference.Buffs == null || reference.Buffs.Count == 0)
		{
			return false;
		}
		int num = FindIndex(in reference, buffId);
		if (num < 0)
		{
			return false;
		}
		BuffRuntime buffRuntime = reference.Buffs[num];
		if (executeExpireActions)
		{
			ExecuteActions(world, targetEntity, buffRuntime.BuffData.OnExpireActions);
			ExecuteActions(world, targetEntity, buffRuntime.OnExpireActions);
		}
		reference.Buffs.RemoveAt(num);
		if (reference.Buffs.Count == 0)
		{
			pool.Del(targetEntity);
		}
		return true;
	}

	private static int FindIndex(in ComponentBuffs comp, int buffId)
	{
		if (comp.Buffs == null)
		{
			return -1;
		}
		for (int i = 0; i < comp.Buffs.Count; i++)
		{
			if (comp.Buffs[i].BuffData.BuffID == buffId)
			{
				return i;
			}
		}
		return -1;
	}

	internal static bool ExecuteActions(EcsWorld world, int targetEntity, List<IAction> actions, List<ICondition> conditions = null)
	{
		if (actions == null || actions.Count == 0)
		{
			return false;
		}
		if (FuncCondition.CheckConditions(world, targetEntity, conditions))
		{
			FuncAction.DoActions(world, targetEntity, actions);
			return true;
		}
		return false;
	}

	public static void AddBuff(EcsWorld world, int entity, BuffId buffId, FP duration)
	{
		AddBuff(world, entity, buffId, duration, FP.Zero);
	}

	public static void AddBuff(EcsWorld world, int entity, BuffId buffId, FP duration, FP param)
	{
		switch (buffId)
		{
		case BuffId.Death:
			AddDeath(world, entity, duration);
			break;
		case BuffId.Stun:
			AddStun(world, entity, duration);
			break;
		case BuffId.Invincible:
			AddInvincible(world, entity, duration);
			break;
		case BuffId.Ghost:
			AddGhost(world, entity, duration);
			break;
		case BuffId.DelayThrough:
			AddDelayThrough(world, entity, duration, param);
			break;
		case BuffId.Disappear:
			AddDisappear(world, entity, duration);
			break;
		case BuffId.Shield:
			AddShield(world, entity, duration);
			break;
		case BuffId.Jump:
			AddJump(world, entity, duration, param);
			break;
		case BuffId.Bubble:
			AddBubble(world, entity, duration);
			break;
		case BuffId.SpeedUp:
			AddSpeedUp(world, entity, duration, param);
			break;
		case BuffId.GroundSpeedUp:
			AddGroundSpeedUp(world, entity, duration);
			break;
		case BuffId.AttachSpeedUp:
			AddAttachSpeedUp(world, entity, duration);
			break;
		}
	}

	public static void AddDeath(EcsWorld world, int entity, FP duration)
	{
		if (!GetBuffDataShared(world, 1, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1, null, new List<IAction> { default(RemoveEntityAction) });
		}
		Add(world, entity, buffData, duration);
	}

	public static void AddStun(EcsWorld world, int entity, FP duration)
	{
		if (!GetBuffDataShared(world, 2, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1, new List<IAction>
			{
				new ControlDisableAction(value: true)
			}, new List<IAction>
			{
				new ControlDisableAction(value: false)
			});
		}
		Add(world, entity, buffData, duration);
	}

	public static void AddInvincible(EcsWorld world, int entity, FP duration)
	{
		if (!GetBuffDataShared(world, 3, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1);
		}
		Add(world, entity, buffData, duration);
	}

	public static void AddDisappear(EcsWorld world, int entity, FP duration)
	{
		if (!FuncUniqueID.TryGetUniqueIDByEntity(world, entity, out var id))
		{
			world.Debugger.LogError($"Entity {entity} has no UniqueID component, cannot apply Disappear buff.");
			return;
		}
		if (!GetBuffDataShared(world, 6, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1);
		}
		Add(world, entity, buffData, duration)?.Init(new List<IAction>
		{
			new ChangeActivityAction
			{
				UniqueID = id,
				IsActive = false
			}
		}, new List<IAction>
		{
			new ChangeActivityAction
			{
				UniqueID = id,
				IsActive = true
			}
		});
	}

	public static void AddShield(EcsWorld world, int entity, FP duration)
	{
		if (!GetBuffDataShared(world, 7, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1);
		}
		Add(world, entity, buffData, duration);
	}

	public static void AddJump(EcsWorld world, int entity, FP duration, FP velocity)
	{
		if (!GetBuffDataShared(world, 8, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1);
		}
		Add(world, entity, buffData, duration)?.Init(null, new List<IAction>
		{
			new SetVelocityActionY
			{
				Velocity = velocity
			}
		});
	}

	public static void AddGhost(EcsWorld world, int entity, FP duration)
	{
		if (!GetBuffDataShared(world, 4, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1, new List<IAction>
			{
				new RigidBodyDisableAction(value: true)
			}, new List<IAction>
			{
				new RigidBodyDisableAction(value: false)
			});
		}
		Add(world, entity, buffData, duration);
	}

	public static void AddDelayThrough(EcsWorld world, int entity, FP duration, FP param)
	{
		if (!GetBuffDataShared(world, 5, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1, null, new List<IAction>
			{
				new RigidBodyDisableAction(value: true)
			});
		}
		Add(world, entity, buffData, duration);
	}

	public static void AddBubble(EcsWorld world, int entity, FP duration)
	{
		if (!GetBuffDataShared(world, 9, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1, new List<IAction>
			{
				new ControlDisableAction(value: true),
				new StaticAction(value: true)
			}, new List<IAction>
			{
				new ControlDisableAction(value: false),
				new StaticAction(value: false)
			});
		}
		Add(world, entity, buffData, duration);
	}

	public static void AddSpeedUp(EcsWorld world, int entity, FP duration, FP speedRate)
	{
		if (!GetBuffDataShared(world, 10, out var buffData))
		{
			buffData.Init(FP.One, FP.EN1);
		}
		Add(world, entity, buffData, duration)?.Init(new List<IAction>
		{
			new SetDataAction
			{
				PropertyID = 6,
				DataOp = DataOp.Add,
				DeltaValue = speedRate
			}
		}, new List<IAction>
		{
			new SetDataAction
			{
				PropertyID = 6,
				DataOp = DataOp.Reduce,
				DeltaValue = speedRate
			}
		});
	}

	public static void AddGroundSpeedUp(EcsWorld world, int entity, FP duration)
	{
		if (!GetBuffDataShared(world, 11, out var buffData))
		{
			Trigger trigger = new Trigger
			{
				Events = new List<Type> { typeof(EventTriggerEnter) },
				Conditions = new List<ICondition>
				{
					new CollectionLayerCondition
					{
						Layer = ColliderLayer.Platform
					}
				},
				Actions = new List<IAction>
				{
					new AddBuffAction
					{
						BuffId = BuffId.SpeedUp,
						Duration = FP.Zero,
						Target = EventTarget.Target,
						Param = FP._0_5
					}
				}
			};
			Trigger trigger2 = new Trigger
			{
				Events = new List<Type> { typeof(EventTriggerExit) },
				Conditions = new List<ICondition>
				{
					new CollectionLayerCondition
					{
						Layer = ColliderLayer.Platform
					}
				},
				Actions = new List<IAction>
				{
					new RemoveBuffAction
					{
						BuffId = BuffId.SpeedUp,
						Target = EventTarget.Target
					}
				}
			};
			Trigger[] triggers = new Trigger[2] { trigger, trigger2 };
			buffData.Init(FP.One, FP.EN1, new List<IAction>
			{
				new AddTriggerAction
				{
					Triggers = triggers
				}
			}, new List<IAction>
			{
				new RemoveTriggerAction
				{
					Triggers = triggers
				},
				new RemoveBuffAction
				{
					BuffId = BuffId.SpeedUp,
					Target = EventTarget.Entity
				}
			});
		}
		Add(world, entity, buffData, duration);
	}

	public static void AddAttachSpeedUp(EcsWorld world, int entity, FP duration)
	{
		GetBuffDataShared(world, 12, out var buffData);
		Add(world, entity, buffData, duration);
	}
}
