using System;

namespace Leopotam.EcsLite;

public class EcsWorldSnapshotRuntime : IDisposable
{
	private const int kCapacityReserved = 16;

	public short[] Entities;

	public int EntitiesItemSize;

	public int EntitiesCount;

	public int[] RecycledEntities;

	public int RecycledEntitiesCount;

	public EcsFilterSnapshotRuntime[] Filters;

	public int FilterCount;

	public IEcsPoolSnapshotRuntime[] Pools;

	public int PoolCount;

	public void EnsureCapacity(int entitiesCount, int entitiesItemSize, int recycledEntitiesCount, int poolCount, int filterCount)
	{
		if (Entities == null || Entities.Length < entitiesCount * entitiesItemSize)
		{
			Entities = new short[entitiesCount * entitiesItemSize + 16];
		}
		EntitiesCount = entitiesCount;
		EntitiesItemSize = entitiesItemSize;
		if (RecycledEntities == null || RecycledEntities.Length < recycledEntitiesCount)
		{
			RecycledEntities = new int[recycledEntitiesCount + 16];
		}
		RecycledEntitiesCount = recycledEntitiesCount;
		if (Filters == null || Filters.Length < filterCount)
		{
			Array.Resize(ref Filters, filterCount + 16);
		}
		FilterCount = filterCount;
		if (Pools == null || Pools.Length < poolCount)
		{
			Array.Resize(ref Pools, poolCount + 16);
		}
		PoolCount = poolCount;
	}

	public void Clear()
	{
		EntitiesCount = 0;
		EntitiesItemSize = 0;
		RecycledEntitiesCount = 0;
		PoolCount = 0;
		FilterCount = 0;
	}

	public void Dispose()
	{
		Clear();
		Entities = null;
		RecycledEntities = null;
		Filters = null;
		Pools = null;
	}
}
