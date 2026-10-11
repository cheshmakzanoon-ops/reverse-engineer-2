using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.GGGo;

public class SystemRegionAOI : IEcsInitSystem, IEcsSystem, IEcsRunSystem, IEcsDestroySystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	private readonly EcsFilterInject<Inc<ComponentRegion>> _filterRegion;

	private readonly EcsFilterInject<Inc<ComponentPlayer, ComponentPosition>> _filterPlayer;

	private readonly EcsFilterInject<Inc<ComponentRegionSpawned, ComponentPosition>> _filterSpawned;

	private readonly EcsPoolInject<ComponentPosition> _poolPos;

	private readonly EcsPoolInject<ComponentRegionSpawned> _poolSpawned;

	private readonly List<int> _destroyBuffer = new List<int>();

	private bool _initialized;

	public void Init(IEcsSystems systems)
	{
		if (!_initialized && _filterRegion.Value.GetEntitiesCount() > 0)
		{
			BuildCache();
		}
	}

	public void Destroy(IEcsSystems systems)
	{
		_destroyBuffer.Clear();
		_env.Value.SpawnCache = null;
		_initialized = false;
	}

	public void Run(IEcsSystems systems)
	{
		if (!_initialized)
		{
			if (_filterRegion.Value.GetEntitiesCount() == 0)
			{
				return;
			}
			BuildCache();
		}
		GGGoLevelConfig level = _env.Value.Level;
		FP x = _env.Value.Distance;
		FP createMinY = x + level.RangeVertical.X - level.AoiLoadBuffer;
		FP createMaxY = x + level.RangeVertical.Y + level.AoiLoadBuffer;
		FP destroyMinY = x + level.RangeVertical.X - level.AoiUnloadBuffer;
		FP destroyMaxY = x + level.RangeVertical.Y + level.AoiUnloadBuffer;
		DestroyOutOfRange(destroyMinY, destroyMaxY);
		CreateInRange(createMinY, createMaxY);
	}

	private void BuildCache()
	{
		EcsWorld value = _world.Value;
		GGGoEnv value2 = _env.Value;
		List<SpawnCacheEntry> list = new List<SpawnCacheEntry>(256);
		List<FuncRegion.PlatformSpawn> spawns = new List<FuncRegion.PlatformSpawn>(64);
		foreach (int item in _filterRegion.Value)
		{
			spawns.Clear();
			ComponentRegion region = value.GetPool<ComponentRegion>().Get(item);
			FuncRegion.GenerateRegionPlatformSpawns(region, value, value2, ref spawns);
			FuncRegion.ModifyRegionSpawns(region, ref spawns);
			EcsPackedEntity regionEntity = value.PackEntity(item);
			for (int i = 0; i < spawns.Count; i++)
			{
				FuncRegion.PlatformSpawn platformSpawn = spawns[i];
				if (platformSpawn.Type != SpawnType.Delete)
				{
					list.Add(new SpawnCacheEntry
					{
						Position = platformSpawn.Position,
						Type = platformSpawn.Type,
						RegionEntity = regionEntity,
						IndexInRegion = i
					});
				}
			}
		}
		int entitiesCount = _filterPlayer.Value.GetEntitiesCount();
		if (entitiesCount > 1)
		{
			using EcsFilter.Enumerator enumerator = _filterPlayer.Value.GetEnumerator();
			if (enumerator.MoveNext())
			{
				int current2 = enumerator.Current;
				FVector2 position = _poolPos.Value.Get(current2).Position;
				list.Add(new SpawnCacheEntry
				{
					Position = new FVector2(0, position.Y - FP._0_5),
					Type = SpawnType.PvpInit,
					RegionEntity = default(EcsPackedEntity),
					IndexInRegion = -1
				});
			}
		}
		else if (entitiesCount == 1)
		{
			using EcsFilter.Enumerator enumerator = _filterPlayer.Value.GetEnumerator();
			if (enumerator.MoveNext())
			{
				int current3 = enumerator.Current;
				FVector2 position2 = _poolPos.Value.Get(current3).Position;
				list.Add(new SpawnCacheEntry
				{
					Position = new FVector2(position2.X - (FP)1.25f, position2.Y - FP._0_5),
					Type = SpawnType.PveInit,
					RegionEntity = default(EcsPackedEntity),
					IndexInRegion = -1
				});
			}
		}
		int lastEntity;
		FP regionsLastPos = FuncRegion.GetRegionsLastPos(value, out lastEntity);
		list.Add(new SpawnCacheEntry
		{
			Position = new FVector2(0, regionsLastPos),
			Type = SpawnType.Destination,
			RegionEntity = default(EcsPackedEntity),
			IndexInRegion = -2
		});
		list.Sort((SpawnCacheEntry a, SpawnCacheEntry b) => b.Position.Y.CompareTo(a.Position.Y));
		_env.Value.SpawnCache = list.ToArray();
		_initialized = true;
	}

	private void DestroyOutOfRange(FP destroyMinY, FP destroyMaxY)
	{
		_destroyBuffer.Clear();
		foreach (int item in _filterSpawned.Value)
		{
			FP y = _poolPos.Value.Get(item).Position.Y;
			if (y < destroyMinY || y > destroyMaxY)
			{
				_destroyBuffer.Add(item);
			}
		}
		foreach (int item2 in _destroyBuffer)
		{
			ref ComponentRegionSpawned reference = ref _poolSpawned.Value.Get(item2);
			int num = FindCacheIndex(reference.RegionEntity, reference.IndexInRegion);
			if (num >= 0)
			{
				_env.Value.SpawnCache[num].State = CacheEntryState.Removed;
			}
			_world.Value.DelEntity(item2);
		}
	}

	private void CreateInRange(FP createMinY, FP createMaxY)
	{
		SpawnCacheEntry[] spawnCache = _env.Value.SpawnCache;
		if (spawnCache == null || spawnCache.Length == 0)
		{
			return;
		}
		int num = FindFirstAtOrBelow(createMaxY);
		int num2 = FindFirstBelow(createMinY);
		EcsWorld value = _world.Value;
		for (int i = num; i < num2; i++)
		{
			ref SpawnCacheEntry reference = ref spawnCache[i];
			if (reference.State != CacheEntryState.Idle)
			{
				continue;
			}
			int entity;
			if (reference.Type >= SpawnType.Normal)
			{
				entity = FuncRegion.CreatePlatform(value, reference.Position, reference.Type);
			}
			else
			{
				if (reference.Type != SpawnType.ItemSpawner)
				{
					continue;
				}
				entity = FuncRegion.CreateItemSpawner(reference.Position, value);
			}
			reference.RegionEntity.Unpack(value, out var entity2);
			ref ComponentRegionSpawned reference2 = ref _poolSpawned.Value.Add(entity);
			reference2.RegionEntity = entity2;
			reference2.IndexInRegion = reference.IndexInRegion;
			reference.State = CacheEntryState.Active;
		}
	}

	private int FindCacheIndex(int regionRawId, int indexInRegion)
	{
		SpawnCacheEntry[] spawnCache = _env.Value.SpawnCache;
		if (spawnCache == null)
		{
			return -1;
		}
		for (int i = 0; i < spawnCache.Length; i++)
		{
			ref SpawnCacheEntry reference = ref spawnCache[i];
			if (reference.IndexInRegion == indexInRegion)
			{
				if (indexInRegion < 0)
				{
					return i;
				}
				if (reference.RegionEntity.Unpack(_world.Value, out var entity) && entity == regionRawId)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private int FindFirstAtOrBelow(FP threshold)
	{
		SpawnCacheEntry[] spawnCache = _env.Value.SpawnCache;
		int num = 0;
		int num2 = spawnCache.Length;
		while (num < num2)
		{
			int num3 = num + (num2 - num) / 2;
			if (spawnCache[num3].Position.Y > threshold)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		return num;
	}

	private int FindFirstBelow(FP threshold)
	{
		SpawnCacheEntry[] spawnCache = _env.Value.SpawnCache;
		int num = 0;
		int num2 = spawnCache.Length;
		while (num < num2)
		{
			int num3 = num + (num2 - num) / 2;
			if (spawnCache[num3].Position.Y >= threshold)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		return num;
	}
}
