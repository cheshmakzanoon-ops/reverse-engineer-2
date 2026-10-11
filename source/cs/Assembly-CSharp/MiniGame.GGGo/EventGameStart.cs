using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("游戏开始触发", "事件")]
public struct EventGameStart : IEvent
{
	public EcsPackedEntity Sender => EcsPackedEntity.Invalid;

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Broadcast;
}
