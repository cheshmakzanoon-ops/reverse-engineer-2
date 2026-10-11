using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("移动到顶部", "功能")]
public struct MoveToTopAction : IAction
{
	public EventTarget Target;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is MoveToTopAction moveToTopAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, moveToTopAction.Target);
		}
		GGGoEnv shared = world.GetShared<GGGoEnv>();
		if (shared != null && world.GetPool<ComponentPosition>().Has(entity) && (!world.GetPool<ComponentPlayer>().Has(entity) || !FuncData.GetBoolData(world, entity, PropertyID.Die)))
		{
			FP y = shared.Distance + shared.Level.RangeVertical.Y - FP.One;
			ref ComponentPosition reference = ref world.GetPool<ComponentPosition>().Get(entity);
			FVector2 closestPlatformBelow = FuncRegion.GetClosestPlatformBelow(world, reference.Position.X, y);
			reference.Position = closestPlatformBelow;
			EcsPool<ComponentVelocity> pool = world.GetPool<ComponentVelocity>();
			if (pool.Has(entity))
			{
				pool.Get(entity).Velocity.Y = FP.Zero;
			}
		}
	}
}
