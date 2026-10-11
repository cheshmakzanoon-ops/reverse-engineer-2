using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("碰撞开始结束", "事件")]
public struct EventTriggerExit : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Local;

	public EventTriggerExit(EcsPackedEntity sender, EcsPackedEntity target)
	{
		Sender = sender;
		Target = target;
	}
}
