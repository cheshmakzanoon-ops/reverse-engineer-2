using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Core;

[TitleAndCategory("数据变化", "事件")]
public struct EventDataChange : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Local;

	public int PropertyID { get; }

	public FP Change { get; }

	public EventDataChange(EcsPackedEntity sender, EcsPackedEntity target, int propertyID)
	{
		Sender = sender;
		Target = target;
		PropertyID = propertyID;
		Change = 0;
	}

	public EventDataChange(EcsPackedEntity sender, EcsPackedEntity target, int propertyID, FP change)
	{
		Sender = sender;
		Target = target;
		PropertyID = propertyID;
		Change = change;
	}
}
