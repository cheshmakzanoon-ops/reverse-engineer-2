using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("广播自定义触发", "广播")]
public struct EventTriggerBroadAction : IAction
{
	public EventTriggerType Type;

	public EventTarget Target;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is EventTriggerBroadAction eventTriggerBroadAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, eventTriggerBroadAction.Target);
			EcsPackedEntity sender = world.PackEntity(entity);
			FuncEvent.Broadcast(world, new EventTrigger(sender, e.Target, eventTriggerBroadAction.Type));
		}
	}
}
