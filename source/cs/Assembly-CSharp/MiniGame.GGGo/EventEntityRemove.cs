using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("实体死亡", "事件")]
public struct EventEntityRemove : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Broadcast;

	public int UniqueId { get; }

	public EventEntityRemove(EcsPackedEntity sender, EcsPackedEntity target, int uniqueId)
	{
		Sender = sender;
		Target = target;
		UniqueId = uniqueId;
	}
}
