using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

public static class FuncRegion
{
	public struct PlatformSpawn
	{
		public int Entity;

		public FVector2 Position;

		public SpawnType Type;
	}

	private const string _PlatformBasePath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/";

	private const string _PlatformObstaclePath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/Obstacle.prefab";

	private const string _PlatformItemSpawnerPath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ItemSpawner.prefab";

	private const string _PlatformObstacleDestinationPath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleDestination.prefab";

	private const string _PlatformObstacleDisappearPath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleDisappear.prefab";

	private const string _PlatformObstacleSpikePath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleSpike.prefab";

	private const string _PlatformObstacleBouncePath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleBounce.prefab";

	private const string _PlatformObstacleRollLPath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleRollL.prefab";

	private const string _PlatformObstacleRollRPath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleRollR.prefab";

	private const string _PlatformObstaclePvpPath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstaclePvp.prefab";

	private const string _PlatformObstaclePvePath = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstaclePve.prefab";

	private static List<Type> __ItemSpawnerincludes = new List<Type>
	{
		typeof(ComponentResource),
		typeof(ComponentPosition),
		typeof(ComponentItem),
		typeof(ComponentColliderTrigger),
		typeof(ComponentStatic)
	};

	public static FP RoundByGrid(FP value, FP gridSize)
	{
		if (gridSize <= FP.Zero)
		{
			return value;
		}
		return FP.Round(value / gridSize) * gridSize;
	}

	public static FP SortRegionComponent(GameWorld world)
	{
		EcsFilter ecsFilter = world.World.Filter<ComponentRegion>().End();
		EcsPool<ComponentRegion> pool = world.World.GetPool<ComponentRegion>();
		List<(int, ComponentRegion)> list = new List<(int, ComponentRegion)>();
		foreach (int item2 in ecsFilter)
		{
			list.Add((item2, pool.Get(item2)));
		}
		if (list.Count == 0)
		{
			return FP.Zero;
		}
		list.Sort(((int entity, ComponentRegion region) a, (int entity, ComponentRegion region) b) => b.region.StartPos.CompareTo(a.region.StartPos));
		FP fP = list[0].Item2.StartPos;
		List<PlatformSpawn> spawns = new List<PlatformSpawn>();
		for (int num = 0; num < list.Count; num++)
		{
			int item = list[num].Item1;
			ref ComponentRegion reference = ref pool.Get(item);
			if (num == 0)
			{
				fP = reference.StartPos;
			}
			else
			{
				reference.StartPos = fP;
			}
			reference.Length = GenerateRegionPlatformSpawns(item, world.World, world.Env as GGGoEnv, ref spawns);
			fP = reference.StartPos - reference.Length;
		}
		return fP;
	}

	public static FP GetRegionsLastPos(EcsWorld world, out int lastEntity)
	{
		EcsFilter ecsFilter = world.Filter<ComponentRegion>().End();
		EcsPool<ComponentRegion> pool = world.GetPool<ComponentRegion>();
		FP x = -5;
		lastEntity = -1;
		foreach (int item in ecsFilter)
		{
			ref ComponentRegion reference = ref pool.Get(item);
			FP fP = reference.StartPos - reference.Length;
			if (fP < x)
			{
				x = fP;
				lastEntity = item;
			}
		}
		return x - (FP)3;
	}

	public static bool GetCurRegion(EcsWorld world, FP distance, out ComponentRegion region, out int entity)
	{
		EcsFilter ecsFilter = world.Filter<ComponentRegion>().End();
		if (ecsFilter.GetEntitiesCount() <= 0)
		{
			region = default(ComponentRegion);
			entity = -1;
			return false;
		}
		EcsPool<ComponentRegion> pool = world.GetPool<ComponentRegion>();
		foreach (int item in ecsFilter)
		{
			region = pool.Get(item);
			if (distance <= region.StartPos && distance > region.StartPos - region.Length)
			{
				entity = item;
				return true;
			}
		}
		ComponentRegion componentRegion = default(ComponentRegion);
		ComponentRegion componentRegion2 = default(ComponentRegion);
		FP fP = FP.MinValue;
		FP fP2 = FP.MaxValue;
		int num = -1;
		int num2 = -1;
		foreach (int item2 in ecsFilter)
		{
			region = pool.Get(item2);
			if (region.StartPos > fP)
			{
				fP = region.StartPos;
				componentRegion = region;
				num = item2;
			}
			if (region.StartPos - region.Length <= fP2)
			{
				fP2 = region.StartPos - region.Length;
				componentRegion2 = region;
				num2 = item2;
			}
		}
		region = ((distance > componentRegion.StartPos) ? componentRegion : componentRegion2);
		entity = ((distance > componentRegion.StartPos) ? num : num2);
		return true;
	}

	public static bool GetLastRegion(EcsWorld world, ComponentRegion curRegion, out ComponentRegion lastRegion, out int entity)
	{
		EcsFilter ecsFilter = world.Filter<ComponentRegion>().End();
		EcsPool<ComponentRegion> pool = world.GetPool<ComponentRegion>();
		FP fP = FP.MaxValue;
		lastRegion = default(ComponentRegion);
		entity = -1;
		foreach (int item in ecsFilter)
		{
			ComponentRegion componentRegion = pool.Get(item);
			if (componentRegion.StartPos > curRegion.StartPos && componentRegion.StartPos < fP)
			{
				fP = componentRegion.StartPos;
				lastRegion = componentRegion;
				entity = item;
			}
		}
		return fP != FP.MaxValue;
	}

	public static FP GetCurSpeed(EcsWorld world, FP distance, out int entity)
	{
		if (!GetCurRegion(world, distance, out var region, out entity))
		{
			return FP.Zero;
		}
		if (!GetLastRegion(world, region, out var lastRegion, out var _) || lastRegion.Speed == region.Speed)
		{
			return region.Speed;
		}
		GGGoEnv shared = world.GetShared<GGGoEnv>();
		if (shared == null || shared.Level.SpeedTransTime <= FP.Zero)
		{
			return region.Speed;
		}
		FP y = shared.Level.SpeedTransTime;
		FP x = region.StartPos;
		FP y2 = x + region.Speed * y;
		if (distance < y2 || distance > x)
		{
			return region.Speed;
		}
		FP t = (x - distance) / (x - y2);
		return FPFunc.Lerp(lastRegion.Speed, region.Speed, t);
	}

	public static FVector2 GetClosestPlatformBelow(EcsWorld world, FP x, FP y)
	{
		if (TryGetClosestPlatformBelow(world, x, y, out var platformEntity, out var platformPos))
		{
			EcsPool<ComponentCollider> pool = world.GetPool<ComponentCollider>();
			if (pool.Has(platformEntity))
			{
				ref ComponentCollider reference = ref pool.Get(platformEntity);
				ref FP x2 = ref platformPos.X;
				x2 += reference.Offset.X;
				ref FP y2 = ref platformPos.Y;
				y2 += reference.Offset.Y + reference.HalfSize.Y;
			}
			return platformPos;
		}
		return new FVector2(x, y);
	}

	public static bool TryGetClosestPlatformBelow(EcsWorld world, FP x, FP y, out int platformEntity, out FVector2 platformPos)
	{
		platformEntity = -1;
		platformPos = new FVector2(x, y);
		EcsPool<ComponentPosition> pool = world.GetPool<ComponentPosition>();
		EcsFilter ecsFilter = world.Filter<ComponentRegionSpawned>().End();
		FP fP = FP.MaxValue;
		foreach (int item in ecsFilter)
		{
			if (!world.IsEntityAliveInternal(item) || !pool.Has(item))
			{
				continue;
			}
			FVector2 position = pool.Get(item).Position;
			if (!(position.Y > y))
			{
				FP fP2 = y - position.Y;
				if (fP2 < fP)
				{
					fP = fP2;
					platformEntity = item;
					platformPos = position;
				}
			}
		}
		return platformEntity >= 0;
	}

	public static bool CreateRegion(int target, EcsWorld world, GGGoEnv env, List<PlatformSpawn> spawns = null)
	{
		ClearRegion(target, world);
		EcsPool<ComponentRegionSpawned> pool = world.GetPool<ComponentRegionSpawned>();
		ref ComponentRegion reference = ref world.GetPool<ComponentRegion>().Get(target);
		reference.Length = GenerateRegionPlatformSpawns(target, world, env, ref spawns);
		if (spawns == null || spawns.Count == 0)
		{
			return false;
		}
		ModifyRegionSpawns(reference, ref spawns);
		for (int i = 0; i < spawns.Count; i++)
		{
			PlatformSpawn value = spawns[i];
			if (value.Type != SpawnType.Delete)
			{
				int entity = 0;
				if (value.Type >= SpawnType.Normal)
				{
					entity = CreatePlatform(world, value.Position, value.Type);
				}
				else if (value.Type == SpawnType.ItemSpawner)
				{
					entity = CreateItemSpawner(value.Position, world);
				}
				ref ComponentRegionSpawned reference2 = ref pool.Add(entity);
				reference2.RegionEntity = target;
				reference2.IndexInRegion = i;
				value.Entity = entity;
				spawns[i] = value;
			}
		}
		return true;
	}

	public static void ClearRegionModify(int target, EcsWorld world)
	{
		if (world.GetPool<ComponentRegion>().Has(target))
		{
			world.GetPool<ComponentRegion>().Get(target).Modifies = null;
		}
	}

	public static FP GenerateRegionPlatformSpawns(int target, EcsWorld world, GGGoEnv env, ref List<PlatformSpawn> spawns)
	{
		if (env == null || !world.GetPool<ComponentRegion>().Has(target))
		{
			return FP.Zero;
		}
		return GenerateRegionPlatformSpawns(world.GetPool<ComponentRegion>().Get(target), world, env, ref spawns);
	}

	public static void ModifyRegionSpawns(ComponentRegion region, ref List<PlatformSpawn> spawns)
	{
		if (spawns == null || spawns.Count == 0 || region.Modifies == null || region.Modifies.Length == 0)
		{
			return;
		}
		Dictionary<int, PlatformModify> dictionary = new Dictionary<int, PlatformModify>();
		for (int i = 0; i < region.Modifies.Length; i++)
		{
			PlatformModify value = region.Modifies[i];
			dictionary[value.Index] = value;
		}
		for (int j = 0; j < spawns.Count; j++)
		{
			PlatformSpawn value2 = spawns[j];
			if (dictionary.TryGetValue(j, out var value3))
			{
				if (value3.Deleted)
				{
					value2.Type = SpawnType.Delete;
					spawns[j] = value2;
				}
				else
				{
					value2.Position = value3.Position;
					spawns[j] = value2;
				}
			}
		}
	}

	public static FP GenerateRegionPlatformSpawns(ComponentRegion region, EcsWorld world, GGGoEnv env, ref List<PlatformSpawn> spawns)
	{
		if (spawns == null)
		{
			spawns = new List<PlatformSpawn>();
		}
		else
		{
			spawns.Clear();
		}
		PlatformInfo[] platforms = region.Platforms;
		FP[] cumWeights;
		FP totalWeight = GenWeights(platforms, out cumWeights);
		FP x = env.Level.GridSize.X;
		FP y = env.Level.GridSize.Y;
		FP y2 = env.Level.RangeHorizon.X;
		FP x2 = env.Level.RangeHorizon.Y;
		if (x2 < y2)
		{
			FP fP = x2;
			FP fP2 = y2;
			y2 = fP;
			x2 = fP2;
		}
		FP x3 = region.StartPos;
		FP value = ((region.StepVertical > FP.Zero) ? region.StepVertical : FP.One);
		value = RoundByGrid(value, y);
		if (value <= FP.Zero)
		{
			value = ((y > FP.Zero) ? y : FP.One);
		}
		FP value2 = ((region.GapHorizontal > FP.Zero) ? region.GapHorizontal : FP.One);
		value2 = RoundByGrid(value2, x);
		if (value2 <= FP.Zero)
		{
			value2 = ((x > FP.Zero) ? x : FP.One);
		}
		FP fP3 = x2 - y2;
		if (fP3 <= FP.Zero)
		{
			fP3 = FP.One;
		}
		int num = 1;
		if (value2 > FP.Zero)
		{
			int num2 = FP.Floor(fP3 / value2).AsInt + 1;
			if (num2 > 1)
			{
				num = num2;
			}
			if (num > 64)
			{
				num = 64;
			}
		}
		int countVertical = region.CountVertical;
		if (countVertical <= 0)
		{
			return FP.Zero;
		}
		PesudoRandom pesudoRandom = new PesudoRandom(region.RandomSeed);
		List<FP> list = new List<FP>(num);
		FP x4 = x3 + value;
		FP y3 = x3;
		for (int i = 0; i < countVertical; i++)
		{
			int num3 = 1;
			if (num > 1)
			{
				num3 = (int)pesudoRandom.Next(1u, (uint)(num + 1));
			}
			list.Clear();
			PlatformInfo randomPlatformType = GetRandomPlatformType(platforms, cumWeights, totalWeight, pesudoRandom);
			FVector2 platFormHalfSize = GetPlatFormHalfSize(randomPlatformType.Type);
			FP value3;
			FP fP4 = (value3 = RoundByGrid(x4 - value, y));
			value3 = RoundByGrid(value3, y);
			FP x5 = PickNonOverlappingX(pesudoRandom, list, y2, x2, value2, platFormHalfSize.X, x);
			PlatformSpawn item = new PlatformSpawn
			{
				Position = new FVector2(x5, value3),
				Type = randomPlatformType.Type
			};
			spawns.Add(item);
			FP fP5 = fP4;
			FP fP6 = value3 - platFormHalfSize.Y * (FP)2;
			for (int j = 1; j < num3; j++)
			{
				PlatformInfo randomPlatformType2 = GetRandomPlatformType(platforms, cumWeights, totalWeight, pesudoRandom);
				FVector2 platFormHalfSize2 = GetPlatFormHalfSize(randomPlatformType2.Type);
				FP value4 = fP5;
				value4 = RoundByGrid(value4, y);
				FP fP7 = PickNonOverlappingX(pesudoRandom, list, y2, x2, value2, platFormHalfSize2.X, x);
				if (!(fP7 == FP.MaxValue))
				{
					spawns.Add(new PlatformSpawn
					{
						Position = new FVector2(fP7, value4),
						Type = randomPlatformType2.Type
					});
					FP fP8 = value4 - platFormHalfSize2.Y * (FP)2;
					if (fP8 < fP6)
					{
						fP6 = fP8;
					}
				}
			}
			x4 = fP6;
			if (fP6 < y3)
			{
				y3 = fP6;
			}
		}
		FP fP9 = x3 - y3 + value;
		if (fP9 < FP.Zero)
		{
			fP9 = FP.Zero;
		}
		return fP9;
	}

	private static FP PickNonOverlappingX(PesudoRandom rnd, List<FP> occupiedCenters, FP minX, FP maxX, FP gapX, FP halfWidth, FP gridSizeX)
	{
		FP x = minX + halfWidth;
		FP x2 = maxX - halfWidth;
		if (x2 < x)
		{
			FP fP = x2;
			FP fP2 = x;
			x = fP;
			x2 = fP2;
		}
		for (int i = 0; i < 32; i++)
		{
			FP y = (FP)rnd.Next(0u, 1000u) * FP.EN3;
			FP value = x + (x2 - x) * y;
			value = RoundByGrid(value, gridSizeX);
			bool flag = true;
			for (int j = 0; j < occupiedCenters.Count; j++)
			{
				if (FP.Abs(occupiedCenters[j] - value) < gapX)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				occupiedCenters.Add(value);
				return value;
			}
		}
		FP fP3 = RoundByGrid(x, gridSizeX);
		FP fP4 = x2;
		FP y2 = ((gapX > FP.Zero) ? gapX : ((gridSizeX > FP.Zero) ? gridSizeX : FP.One));
		FP y3 = fP3;
		while (y3 <= fP4)
		{
			bool flag2 = true;
			for (int k = 0; k < occupiedCenters.Count; k++)
			{
				if (FP.Abs(occupiedCenters[k] - y3) < gapX)
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				FP fP5 = RoundByGrid(y3, gridSizeX);
				occupiedCenters.Add(fP5);
				return fP5;
			}
			y3 += y2;
		}
		return FP.MaxValue;
	}

	public static void ClearRegion(int entityId, EcsWorld world)
	{
		EcsPool<ComponentRegionSpawned> pool = world.GetPool<ComponentRegionSpawned>();
		foreach (int item in world.Filter<ComponentRegionSpawned>().End())
		{
			if (pool.Get(item).RegionEntity == entityId)
			{
				world.DelEntity(item);
			}
		}
	}

	public static void GenerateRegionItems(int entityId, EcsWorld world, GGGoEnv env, ref List<PlatformSpawn> spawns)
	{
		if (env == null || spawns == null || spawns.Count == 0 || !world.GetPool<ComponentRegion>().Has(entityId))
		{
			return;
		}
		int num = env.Level.ItemGapLimit;
		int num2 = env.Level.ItemGapLeast;
		if (num <= 0 && num2 <= 0)
		{
			return;
		}
		if (num <= 0)
		{
			num = int.MaxValue;
		}
		if (num2 <= 0)
		{
			num2 = int.MaxValue;
		}
		PesudoRandom pesudoRandom = new PesudoRandom(world.GetPool<ComponentRegion>().Get(entityId).RandomSeed ^ 0x9E3779B9u);
		int num3 = 0;
		for (int num4 = spawns.Count - 1; num4 >= 0; num4--)
		{
			PlatformSpawn platformSpawn = spawns[num4];
			if (platformSpawn.Type >= SpawnType.Normal)
			{
				num3++;
				bool flag = num3 >= num2;
				if (num3 <= num)
				{
					bool flag2 = false;
					if (!flag)
					{
						flag2 = pesudoRandom.Next(0u, 1000u) < 200;
					}
					if (flag || flag2)
					{
						FVector2 platFormHalfSize = GetPlatFormHalfSize(platformSpawn.Type);
						FP y = platformSpawn.Position.Y + platFormHalfSize.Y + FP._0_2;
						FVector2 position = new FVector2(platformSpawn.Position.X, y);
						spawns.Add(new PlatformSpawn
						{
							Position = position,
							Type = SpawnType.ItemSpawner
						});
						num3 = 0;
					}
				}
			}
		}
	}

	public static void GetRegionEntities(int entityId, EcsWorld world, Dictionary<int, int> entities)
	{
		entities.Clear();
		EcsPool<ComponentRegionSpawned> pool = world.GetPool<ComponentRegionSpawned>();
		foreach (int item in world.Filter<ComponentRegionSpawned>().End())
		{
			ComponentRegionSpawned componentRegionSpawned = pool.Get(item);
			if (componentRegionSpawned.RegionEntity == entityId)
			{
				entities[componentRegionSpawned.IndexInRegion] = item;
			}
		}
	}

	public static void SaveRegionModify(int target, EcsWorld world, GGGoEnv env)
	{
		ref ComponentRegion reference = ref world.GetPool<ComponentRegion>().Get(target);
		List<PlatformSpawn> spawns = new List<PlatformSpawn>();
		reference.Length = GenerateRegionPlatformSpawns(target, world, env, ref spawns);
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		GetRegionEntities(target, world, dictionary);
		List<PlatformModify> list = null;
		for (int i = 0; i < spawns.Count; i++)
		{
			if (dictionary.TryGetValue(i, out var value))
			{
				ref ComponentPosition reference2 = ref world.GetPool<ComponentPosition>().Get(value);
				FVector2 position = spawns[i].Position;
				if (reference2.Position != position)
				{
					if (list == null)
					{
						list = new List<PlatformModify>();
					}
					list.Add(new PlatformModify
					{
						Index = i,
						Deleted = false,
						Position = reference2.Position
					});
				}
			}
			else
			{
				if (list == null)
				{
					list = new List<PlatformModify>();
				}
				list.Add(new PlatformModify
				{
					Index = i,
					Deleted = true,
					Position = spawns[i].Position
				});
			}
		}
		if (list != null && list.Count > 0)
		{
			reference.Modifies = list.ToArray();
		}
		else
		{
			reference.Modifies = null;
		}
	}

	private static FP GenWeights(PlatformInfo[] platformInfos, out FP[] cumWeights)
	{
		FP x = FP.Zero;
		if (platformInfos != null && platformInfos.Length != 0)
		{
			cumWeights = new FP[platformInfos.Length];
			for (int i = 0; i < platformInfos.Length; i++)
			{
				x += platformInfos[i].Weight;
				cumWeights[i] = x;
			}
			if (x <= FP.Zero)
			{
				x = FP.One;
				for (int j = 0; j < cumWeights.Length; j++)
				{
					cumWeights[j] = j + 1;
				}
			}
		}
		else
		{
			cumWeights = Array.Empty<FP>();
		}
		return x;
	}

	private static PlatformInfo GetRandomPlatformType(PlatformInfo[] platformInfos, FP[] weights, FP totalWeight, PesudoRandom rnd)
	{
		if (platformInfos == null || platformInfos.Length == 0)
		{
			return new PlatformInfo
			{
				Type = SpawnType.Normal,
				Weight = FP.One
			};
		}
		FP fP = (FP)rnd.Next(0u, 1000u) * FP.EN3 * totalWeight;
		for (int i = 0; i < weights.Length; i++)
		{
			if (fP < weights[i])
			{
				return platformInfos[i];
			}
		}
		return platformInfos[^1];
	}

	public static int CreateItemSpawner(FVector2 pos, EcsWorld world)
	{
		int num = world.NewEntity();
		world.GetPool<ComponentUniqueID>().Add(num);
		world.GetPool<ComponentStatic>().Add(num);
		world.GetPool<ComponentPosition>().Add(num).Position = pos;
		world.GetPool<ComponentItem>().Add(num).ItemType = ItemType.Box;
		world.GetPool<ComponentResource>().Add(num).Asset = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ItemSpawner.prefab";
		ref ComponentColliderTrigger reference = ref world.GetPool<ComponentColliderTrigger>().Add(num);
		reference.Offset = FVector2.Zero;
		reference.HalfSize = new FVector2(FP._0_4, FP._0_4);
		reference.Layer = ColliderLayer.Item;
		FuncEntity.AddActiveComponent(world, num, __ItemSpawnerincludes);
		return num;
	}

	public static int CreatePlatform(EcsWorld world, FVector2 position, SpawnType type = SpawnType.Normal)
	{
		int num = world.NewEntity();
		ref ComponentResource reference = ref world.GetPool<ComponentResource>().Add(num);
		reference.Asset = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/Obstacle.prefab";
		world.GetPool<ComponentPosition>().Add(num).Position = position;
		ref ComponentCollider reference2 = ref world.GetPool<ComponentCollider>().Add(num);
		FVector2 platFormHalfSize = GetPlatFormHalfSize(type);
		reference2.Set(new FVector2(FP.Zero, -platFormHalfSize.Y), platFormHalfSize);
		reference2.Layer = ColliderLayer.Platform;
		world.GetPool<ComponentStatic>().Add(num);
		world.GetPool<ComponentColliderTrigger>().Add(num).Set(new FVector2(reference2.Offset.X, reference2.Offset.Y + platFormHalfSize.Y), new FVector2(platFormHalfSize.X, FP.EN1));
		world.GetPool<ComponentTriggers>().Add(num).Triggers = new List<Trigger>();
		switch (type)
		{
		case SpawnType.Normal:
			ConfigureNormal(world, num);
			break;
		case SpawnType.Disappear:
			ConfigureDisappear(world, num);
			break;
		case SpawnType.Spike:
			ConfigureSpike(world, num);
			break;
		case SpawnType.Bounce:
			ConfigureBounce(world, num);
			break;
		case SpawnType.RollL:
		case SpawnType.RollR:
			ConfigureRoll(world, num, type);
			break;
		case SpawnType.PveInit:
			reference.Asset = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstaclePve.prefab";
			break;
		case SpawnType.PvpInit:
			reference.Asset = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstaclePvp.prefab";
			break;
		case SpawnType.Destination:
			ConfigureDestination(world, num);
			break;
		}
		return num;
	}

	public static FVector2 GetPlatFormHalfSize(SpawnType type)
	{
		switch (type)
		{
		case SpawnType.Normal:
		case SpawnType.Spike:
		case SpawnType.Disappear:
		case SpawnType.Bounce:
			return new FVector2(0.9f, 0.3f);
		case SpawnType.RollL:
		case SpawnType.RollR:
			return new FVector2(0.9f, 0.4f);
		case SpawnType.PveInit:
			return new FVector2(2f, 0.3f);
		case SpawnType.PvpInit:
			return new FVector2(1.15f, 0.3f);
		case SpawnType.Destination:
			return new FVector2(5f, 0.8f);
		default:
			return new FVector2(0.9f, 0.3f);
		}
	}

	private static void ConfigureDestination(EcsWorld world, int entity)
	{
		world.GetPool<ComponentCollider>().Get(entity).Layer = ColliderLayer.Destination;
		world.GetPool<ComponentResource>().Get(entity).Asset = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleDestination.prefab";
		world.GetPool<ComponentTriggers>().Get(entity).Triggers.Add(new Trigger
		{
			Events = new List<Type> { typeof(EventTriggerEnter) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Send
				},
				new CollectionLayerCondition
				{
					Layer = (ColliderLayer.Player | ColliderLayer.Enemy)
				}
			},
			Actions = new List<IAction>
			{
				new CheckGameOverAction
				{
					IsDestination = true
				},
				new EventTriggerBroadAction
				{
					Type = EventTriggerType.PlatformNormal
				}
			}
		});
	}

	private static void ConfigureNormal(EcsWorld world, int entity)
	{
		world.GetPool<ComponentTriggers>().Get(entity).Triggers.Add(new Trigger
		{
			Events = new List<Type> { typeof(EventTriggerEnter) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Send
				},
				new CollectionLayerCondition
				{
					Layer = (ColliderLayer.Player | ColliderLayer.Enemy)
				}
			},
			Actions = new List<IAction>
			{
				new EventTriggerBroadAction
				{
					Type = EventTriggerType.PlatformNormal
				}
			}
		});
	}

	private static void ConfigureDisappear(EcsWorld world, int entity)
	{
		world.GetPool<ComponentResource>().Get(entity).Asset = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleDisappear.prefab";
		ref ComponentTriggers reference = ref world.GetPool<ComponentTriggers>().Get(entity);
		Trigger trigger = new Trigger
		{
			Events = new List<Type> { typeof(EventTriggerEnter) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Send
				},
				new CollectionLayerCondition
				{
					Layer = (ColliderLayer.Player | ColliderLayer.Enemy)
				}
			},
			Actions = new List<IAction>
			{
				new AddBuffAction(BuffId.Death, 1.5),
				new AddBuffAction(BuffId.DelayThrough, FP._0_5),
				new EventTriggerBroadAction
				{
					Type = EventTriggerType.PlatformBreak
				}
			}
		};
		trigger.TriggerLimit = 1;
		reference.Triggers.Add(trigger);
	}

	private static void ConfigureSpike(EcsWorld world, int entity)
	{
		world.GetPool<ComponentResource>().Get(entity).Asset = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleSpike.prefab";
		world.GetPool<ComponentTriggers>().Get(entity).Triggers.Add(new Trigger
		{
			Events = new List<Type> { typeof(EventTriggerEnter) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Send
				},
				new CollectionLayerCondition
				{
					Layer = (ColliderLayer.Player | ColliderLayer.Enemy)
				},
				new HasBuffCondition(BuffId.Invincible, invert: true, EventTarget.Target)
			},
			Actions = new List<IAction>
			{
				new CharacterHurtAction
				{
					HurtValue = FP.One,
					Target = EventTarget.Target
				},
				new EventTriggerBroadAction
				{
					Type = EventTriggerType.PlatformSpike
				}
			}
		});
	}

	private static void ConfigureBounce(EcsWorld world, int entity)
	{
		world.GetPool<ComponentResource>().Get(entity).Asset = "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleBounce.prefab";
		world.GetPool<ComponentTriggers>().Get(entity).Triggers.Add(new Trigger
		{
			Events = new List<Type> { typeof(EventTriggerEnter) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Send
				},
				new CollectionLayerCondition
				{
					Layer = (ColliderLayer.Player | ColliderLayer.Enemy)
				}
			},
			Actions = new List<IAction>
			{
				new AddBuffAction(BuffId.Jump, 0.02, 6, EventTarget.Target),
				new EventTriggerBroadAction
				{
					Type = EventTriggerType.PlatformBounce
				}
			}
		});
	}

	private static void ConfigureRoll(EcsWorld world, int entity, SpawnType type)
	{
		world.GetPool<ComponentResource>().Get(entity).Asset = ((type == SpawnType.RollL) ? "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleRollL.prefab" : "Assets/Main/MiniGameRes/GGGo/Prefab/Obstacle/ObstacleRollR.prefab");
		ref ComponentColliderTrigger reference = ref world.GetPool<ComponentColliderTrigger>().Get(entity);
		FP x = ((type == SpawnType.RollL) ? (-FP.EN1) : FP.EN1);
		reference.Offset = new FVector2(x, reference.Offset.Y);
		ref ComponentTriggers reference2 = ref world.GetPool<ComponentTriggers>().Get(entity);
		if (type == SpawnType.RollL)
		{
			reference2.Triggers.Add(new Trigger
			{
				Events = new List<Type> { typeof(EventTriggerEnter) },
				Conditions = new List<ICondition>
				{
					new EventWithMeCondition
					{
						EvenLaunchTargetType = EvenLaunchTargetType.Send
					},
					new CollectionLayerCondition
					{
						Layer = (ColliderLayer.Player | ColliderLayer.Enemy)
					}
				},
				Actions = new List<IAction>
				{
					new SetMoveDirectionAction
					{
						Value = new FVector2(-4, FP.Zero),
						Target = EventTarget.Target
					},
					new EventTriggerBroadAction
					{
						Type = EventTriggerType.PlatformRoll
					}
				}
			});
		}
		else
		{
			reference2.Triggers.Add(new Trigger
			{
				Events = new List<Type> { typeof(EventTriggerEnter) },
				Conditions = new List<ICondition>
				{
					new EventWithMeCondition
					{
						EvenLaunchTargetType = EvenLaunchTargetType.Send
					},
					new CollectionLayerCondition
					{
						Layer = (ColliderLayer.Player | ColliderLayer.Enemy)
					}
				},
				Actions = new List<IAction>
				{
					new SetMoveDirectionAction
					{
						Value = new FVector2(4, FP.Zero),
						Target = EventTarget.Target
					},
					new EventTriggerBroadAction
					{
						Type = EventTriggerType.PlatformRoll
					}
				}
			});
		}
		reference2.Triggers.Add(new Trigger
		{
			Events = new List<Type> { typeof(EventTriggerExit) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Send
				},
				new CollectionLayerCondition
				{
					Layer = (ColliderLayer.Player | ColliderLayer.Enemy)
				}
			},
			Actions = new List<IAction>
			{
				new SetMoveDirectionAction
				{
					Value = FVector2.Zero,
					Target = EventTarget.Target
				}
			}
		});
	}
}
