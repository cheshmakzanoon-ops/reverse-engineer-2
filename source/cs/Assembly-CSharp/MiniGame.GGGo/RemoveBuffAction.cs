using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("移除buff", "效果")]
public struct RemoveBuffAction : IAction
{
	public EventTarget Target;

	public BuffId BuffId;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public RemoveBuffAction(BuffId buffId, EventTarget target = EventTarget.Entity)
	{
		Target = target;
		BuffId = buffId;
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is RemoveBuffAction removeBuffAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, removeBuffAction.Target);
			FuncBuff.Remove(world, entity, (int)removeBuffAction.BuffId);
		}
	}
}
