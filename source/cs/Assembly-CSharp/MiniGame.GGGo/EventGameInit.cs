using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("游戏重新初始化", "事件")]
public struct EventGameInit : IEvent
{
	public FP WaitTime;

	public EcsPackedEntity Sender => EcsPackedEntity.Invalid;

	public EcsPackedEntity Target { get; }

	public TriggerEventType EventType => TriggerEventType.Broadcast;
}
