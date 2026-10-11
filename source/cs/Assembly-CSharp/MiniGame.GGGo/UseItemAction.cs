using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("使用道具", "功能")]
public struct UseItemAction : IAction
{
	public EventTarget Target;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is UseItemAction useItemAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, useItemAction.Target);
			EcsPool<ComponentItemHold> pool = world.GetPool<ComponentItemHold>();
			if (pool.Has(entity))
			{
				FuncItem.UseItem(world, entity, pool.Get(entity).ItemType);
				pool.Del(entity);
			}
		}
	}
}
