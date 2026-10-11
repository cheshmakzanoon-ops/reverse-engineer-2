using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("掉出地图", "事件")]
public struct EventFallOutCollection : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.TargetOnly;

	public EventFallOutCollection(EcsPackedEntity target)
	{
		Sender = target;
		Target = target;
	}
}
