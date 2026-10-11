using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("治疗事件", "事件")]
public struct EventCharacterHeal : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Local;

	public FP Value { get; }

	public EventCharacterHeal(EcsPackedEntity sender, EcsPackedEntity target, FP value)
	{
		Sender = sender;
		Target = target;
		Value = value;
	}
}
