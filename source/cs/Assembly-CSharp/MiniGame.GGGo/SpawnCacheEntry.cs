using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.GGGo;

public struct SpawnCacheEntry
{
	public FVector2 Position;

	public SpawnType Type;

	public EcsPackedEntity RegionEntity;

	public int IndexInRegion;

	public CacheEntryState State;
}
