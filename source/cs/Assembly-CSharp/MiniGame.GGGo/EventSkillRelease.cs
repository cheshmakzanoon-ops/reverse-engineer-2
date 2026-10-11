using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("技能开始释放", "事件")]
public struct EventSkillRelease : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Local;

	public ItemType ItemType { get; }

	public EventSkillRelease(EcsPackedEntity sender, EcsPackedEntity target, ItemType itemType)
	{
		Sender = sender;
		Target = target;
		ItemType = itemType;
	}
}
