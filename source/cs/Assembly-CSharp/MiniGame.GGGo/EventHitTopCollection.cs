using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("撞到顶部", "事件")]
public struct EventHitTopCollection : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.TargetOnly;

	public EventHitTopCollection(EcsPackedEntity target)
	{
		Sender = target;
		Target = target;
	}
}
