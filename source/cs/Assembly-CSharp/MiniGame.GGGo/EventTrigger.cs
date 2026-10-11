using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("自定义触发", "事件")]
public struct EventTrigger : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Broadcast;

	public EventTriggerType Type { get; }

	public int TriggerSource { get; }

	public EventTrigger(EcsPackedEntity sender, EcsPackedEntity target, EventTriggerType type, int triggerSource = 0)
	{
		Sender = sender;
		Target = target;
		Type = type;
		TriggerSource = triggerSource;
	}
}
