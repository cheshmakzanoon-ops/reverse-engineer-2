using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("移动状态变化", "事件")]
public struct EventMoveState : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target => Sender;

	public TriggerEventType EventType => TriggerEventType.SenderOnly;

	public int MoveState { get; }

	public EventMoveState(EcsPackedEntity sender, int moveState)
	{
		Sender = sender;
		MoveState = moveState;
	}
}
