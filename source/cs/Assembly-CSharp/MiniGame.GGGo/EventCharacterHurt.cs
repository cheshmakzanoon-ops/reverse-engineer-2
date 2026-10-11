using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("受伤事件", "事件")]
public struct EventCharacterHurt : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Local;

	public FP HurtValue { get; }

	public EventCharacterHurt(EcsPackedEntity sender, EcsPackedEntity target, FP hurtValue)
	{
		Sender = sender;
		Target = target;
		HurtValue = hurtValue;
	}
}
