using Leopotam.EcsLite;

namespace MiniGame.Core;

public interface IEvent
{
	EcsPackedEntity Sender { get; }

	EcsPackedEntity Target { get; }

	TriggerEventType EventType { get; }
}
