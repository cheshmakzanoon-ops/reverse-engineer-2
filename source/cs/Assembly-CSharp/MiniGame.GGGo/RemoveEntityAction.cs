using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("立即移除实体", "功能")]
public struct RemoveEntityAction : IAction
{
	public EventTarget Target;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is RemoveEntityAction removeEntityAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, removeEntityAction.Target);
		}
		FuncEntity.DelEntity(world, entity);
	}
}
