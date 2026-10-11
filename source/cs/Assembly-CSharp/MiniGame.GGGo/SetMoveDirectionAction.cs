using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[TitleAndCategory("增加移除定向移动", "功能")]
public struct SetMoveDirectionAction : IAction
{
	public EventTarget Target;

	[LabelText("移动方向和大小")]
	public FVector2 Value;

	[LabelText("持续时间")]
	public FP LifeTime;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (!(action is SetMoveDirectionAction setMoveDirectionAction))
		{
			return;
		}
		entity = FuncAction.GetTarget(world, entity, e, setMoveDirectionAction.Target);
		EcsPool<ComponentMoveDirection> pool = world.GetPool<ComponentMoveDirection>();
		if (setMoveDirectionAction.Value.Equals(FVector2.Zero))
		{
			if (pool.Has(entity))
			{
				pool.Del(entity);
			}
			return;
		}
		FP x = world.GetShared<GameSharedEnv>().LogicTime;
		if (pool.Has(entity))
		{
			ref ComponentMoveDirection reference = ref pool.Get(entity);
			reference.Value = setMoveDirectionAction.Value;
			if (setMoveDirectionAction.LifeTime <= FP.Zero)
			{
				reference.EndTime = FP.Zero;
				return;
			}
			FP righ = x + setMoveDirectionAction.LifeTime;
			reference.EndTime = FP.Max(reference.EndTime, righ);
		}
		else
		{
			ref ComponentMoveDirection reference2 = ref pool.Add(entity);
			reference2.Value = setMoveDirectionAction.Value;
			if (setMoveDirectionAction.LifeTime <= FP.Zero)
			{
				reference2.EndTime = FP.Zero;
			}
			else
			{
				reference2.EndTime = x + setMoveDirectionAction.LifeTime;
			}
		}
	}
}
