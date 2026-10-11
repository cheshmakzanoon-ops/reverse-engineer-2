using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("尝试使用道具", "事件")]
public struct EventTryUseItem : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Local;

	public ItemType ItemType { get; }

	public EventTryUseItem(EcsPackedEntity sender, EcsPackedEntity target, ItemType itemType)
	{
		Sender = sender;
		Target = target;
		ItemType = itemType;
	}
}
