using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("修改移动速度", "功能")]
public struct SetVelocityActionY : IAction
{
	public EventTarget Target;

	public FP Velocity;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is SetVelocityActionY setVelocityActionY)
		{
			entity = FuncAction.GetTarget(world, entity, e, setVelocityActionY.Target);
			EcsPool<ComponentVelocity> pool = world.GetPool<ComponentVelocity>();
			if (pool.Has(entity))
			{
				pool.Get(entity).Velocity.Y = setVelocityActionY.Velocity;
			}
		}
	}
}
