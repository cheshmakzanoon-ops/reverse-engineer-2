using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemSkill : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentSkillRelease>> _filterSkill;

	protected readonly EcsPoolInject<ComponentSkillRelease> _poolSkill;

	public void Run(IEcsSystems systems)
	{
		FP y = _env.Value.LogicTickDelta;
		EcsWorld value = _world.Value;
		foreach (int item in _filterSkill.Value)
		{
			ref ComponentSkillRelease reference = ref _poolSkill.Value.Get(item);
			if (reference.ElapsedTime < reference.Duration)
			{
				ref FP elapsedTime = ref reference.ElapsedTime;
				elapsedTime += y;
			}
			else
			{
				OnSkillFinished(value, reference.TargetEntity, reference.SkillType);
				FuncEvent.Broadcast(value, new EventSkillFinished(value.PackEntity(item), value.PackEntity(reference.TargetEntity), reference.SkillType));
				_world.Value.DelEntity(item);
			}
		}
	}

	private void OnSkillFinished(EcsWorld world, int targetEntity, ItemType skillType)
	{
		switch (skillType)
		{
		case ItemType.Bubble:
			FuncBuff.AddBuff(world, targetEntity, BuffId.Bubble, 1.5);
			break;
		case ItemType.RocketFish:
			FuncBuff.AddBuff(world, targetEntity, BuffId.Stun, FP.One);
			break;
		}
	}
}
