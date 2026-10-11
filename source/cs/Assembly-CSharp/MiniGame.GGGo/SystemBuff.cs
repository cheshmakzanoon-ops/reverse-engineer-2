using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemBuff : IEcsRunSystem, IEcsSystem
{
	private EcsSharedInject<IGameSharedEnv> _shared;

	private EcsFilterInject<Inc<ComponentBuffs>> _filter;

	private EcsPoolInject<ComponentBuffs> _poolBuffs;

	public void Run(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		FP y = _shared.Value.LogicTickDelta;
		if (y <= FP.Zero)
		{
			return;
		}
		EcsPool<ComponentBuffs> value = _poolBuffs.Value;
		foreach (int item in _filter.Value)
		{
			ref ComponentBuffs reference = ref value.Get(item);
			if (reference.Buffs == null || reference.Buffs.Count == 0)
			{
				continue;
			}
			bool flag = true;
			for (int num = reference.Buffs.Count - 1; num >= 0; num--)
			{
				BuffRuntime buffRuntime = reference.Buffs[num];
				FP duration = buffRuntime.Duration;
				if (duration > FP.Zero)
				{
					buffRuntime.ElapsedTime += y;
				}
				bool flag2 = false;
				if (buffRuntime.BuffData.Interval > FP.Zero)
				{
					buffRuntime.ElapsedIntervalTime += y;
					while (buffRuntime.ElapsedIntervalTime >= buffRuntime.BuffData.Interval)
					{
						buffRuntime.ElapsedIntervalTime -= buffRuntime.BuffData.Interval;
						flag2 |= FuncBuff.ExecuteActions(world, item, buffRuntime.BuffData.OnIntervalActions, buffRuntime.BuffData.Conditions);
						flag2 |= FuncBuff.ExecuteActions(world, item, buffRuntime.OnIntervalActions, buffRuntime.BuffData.Conditions);
					}
				}
				else
				{
					flag2 |= FuncBuff.ExecuteActions(world, item, buffRuntime.BuffData.OnIntervalActions, buffRuntime.BuffData.Conditions);
					flag2 |= FuncBuff.ExecuteActions(world, item, buffRuntime.OnIntervalActions, buffRuntime.BuffData.Conditions);
				}
				if (duration > FP.Zero && buffRuntime.ElapsedTime >= duration)
				{
					flag2 |= FuncBuff.ExecuteActions(world, item, buffRuntime.BuffData.OnExpireActions);
					if ((flag2 | FuncBuff.ExecuteActions(world, item, buffRuntime.OnExpireActions)) && !world.IsEntityAliveInternal(item))
					{
						flag = false;
						break;
					}
					reference.Buffs.RemoveAt(num);
				}
			}
			if (flag && reference.Buffs.Count == 0)
			{
				value.Del(item);
			}
		}
	}
}
