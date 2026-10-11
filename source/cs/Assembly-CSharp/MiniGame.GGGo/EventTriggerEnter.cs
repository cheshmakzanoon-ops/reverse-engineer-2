using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("碰撞开始", "事件")]
public struct EventTriggerEnter : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Local;

	public ColliderLayer SenderLayer { get; set; }

	public ColliderLayer TargetLayer { get; set; }

	public CollisionDirection Direction { get; }

	public EventTriggerEnter(EcsPackedEntity sender, EcsPackedEntity target, CollisionDirection direction, ColliderLayer senderLayer, ColliderLayer targetLayer)
	{
		Sender = sender;
		Target = target;
		Direction = direction;
		SenderLayer = senderLayer;
		TargetLayer = targetLayer;
	}
}
