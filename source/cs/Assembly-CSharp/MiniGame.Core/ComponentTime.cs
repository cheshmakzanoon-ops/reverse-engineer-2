using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public struct ComponentTime
{
	public int ID;

	public int TriggerMax;

	public FP Start;

	public FP Interval;

	public FP TriggerNext;

	public int TriggerCount;

	public EcsPackedEntity Owner;
}
