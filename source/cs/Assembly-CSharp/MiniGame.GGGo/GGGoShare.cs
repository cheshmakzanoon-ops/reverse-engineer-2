using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using Newtonsoft.Json;

namespace MiniGame.GGGo;

public static class GGGoShare
{
	public static GameSerializer Serializer = new GameSerializer(new GameGGGoJsonTypeBinder(), null, new List<IEcsPoolDelegate>
	{
		new ComponentUnityPrefabIgnoreDelegate(),
		new ComponentControllerClientIgnoreDelegate()
	});

	public static List<Type> Incs = new List<Type>
	{
		typeof(ComponentResource),
		typeof(ComponentPosition),
		typeof(ComponentData)
	};

	public static List<Type> IncsFrame0 = new List<Type>
	{
		typeof(ComponentResource),
		typeof(ComponentPosition),
		typeof(ComponentData),
		typeof(ComponentVelocity),
		typeof(ComponentAcceleration)
	};

	public const int kRuntimeDumpStep = 10;

	public const int kRuntimeDumpMaxSize = 8388608;

	public static GGGoEnv InitEnv(List<SyncCommand> commands, IResourceLoader resource, GGGoEnv env = null)
	{
		if (env == null)
		{
			env = new GGGoEnv();
		}
		env.ResourceLoader = resource;
		env.Commands = new GGGoCommand(commands);
		env.GameResult = new GGGoGameResult();
		env.InitData = default(GGGoInitData);
		return env;
	}

	public static void InitGameByLevel(GameWorld world, string level)
	{
		(world.Env as GGGoEnv).InitData.LevelPath = level;
		world.InitWithLevel(level);
	}

	public static void InitComponents(GameWorld world)
	{
		world.World.InitPool<ComponentTemplate>().InitPool<ComponentUniqueID>().InitPool<ComponentActivedUniqueID>()
			.InitPool<ComponentUniqueIDManager>()
			.InitPool<ComponentActivitySnapshot>()
			.InitPool<ComponentActivityChanged>()
			.InitPool<ComponentActivedTriggers>()
			.InitPool<ComponentTime>()
			.InitPool<ComponentEventManager>()
			.InitPool<ComponentTriggers>()
			.InitPool<ComponentResource>()
			.InitPool<ComponentUnityPrefab>()
			.InitPool<ComponentData>()
			.InitPool<ComponentPosition>()
			.InitPool<ComponentVelocity>()
			.InitPool<ComponentAcceleration>()
			.InitPool<ComponentMoveDirection>()
			.InitPool<ComponentStatic>()
			.InitPool<ComponentGravity>()
			.InitPool<ComponentCollider>()
			.InitPool<ComponentColliderTrigger>()
			.InitPool<ComponentColliderDisable>()
			.InitPool<ComponentRigidBody>()
			.InitPool<ComponentPlayer>()
			.InitPool<ComponentBuffs>()
			.InitPool<ComponentControlDisable>()
			.InitPool<ComponentRegion>()
			.InitPool<ComponentRegionEntities>()
			.InitPool<ComponentRegionSpawned>()
			.InitPool<ComponentControllerClient>()
			.InitPool<ComponentItem>()
			.InitPool<ComponentItemHold>()
			.InitPool<ComponentSkillRelease>();
	}

	public static void InitSystems(GameWorld world)
	{
		world.LogicSystems.Add(new SystemFilterInit()).Add(new SystemEventInit()).Add(new SystemGameState())
			.Add(new SystemTime())
			.Add(new SystemTemplateInstantiate())
			.Add(new SystemActivity())
			.Add(new SystemUniqueIDWithAutoID())
			.Add(new SystemBuff())
			.Add(new SystemCommand())
			.Add(new SystemRegion())
			.Add(new SystemRegionAOI())
			.Add(new SystemItem())
			.Add(new SystemSkill())
			.Add(new SystemHpRegen())
			.Add(new SystemTriggerInit())
			.Add(new SystemGameOver())
			.Add(new SystemTrigger())
			.Add(new SystemEvent());
		world.PhysicsSystems.Add(new SystemAcceleration()).Add(new SystemMoveByVelocity()).Add(new SystemCollision())
			.Add(new SystemCollisionTriggers())
			.Add(new SystemMoveRestriction())
			.Add(new SystemGravity());
	}

	public static SystemRuntimeDump CreateRuntimeDumpSystem(string file = null)
	{
		if (string.IsNullOrEmpty(file))
		{
			return new SystemRuntimeDump(Incs, null, (default(Inc<ComponentResource>), null), IncsFrame0, null, (default(Inc<ComponentResource>), null), 10, 8388608);
		}
		return new SystemRuntimeDumpFile(file, Incs, null, (default(Inc<ComponentResource>), null), IncsFrame0, null, (default(Inc<ComponentResource>), null), 10, 8388608);
	}

	public static GGGoVerify GetValidationResult(EcsWorld world)
	{
		return new GGGoVerify
		{
			AliveEntities = world.TakeEntitiesStates<Inc<ComponentResource>, Exc<ComponentUniqueIDManager, ComponentActivedUniqueID>>(Incs),
			LogicTickCount = world.GetShared<IGameSharedEnv>().LogicTickCount,
			PhysicsTickCount = world.GetShared<IGameSharedEnv>().PhysicsTickCount
		};
	}

	public static string GetValidationResultJson(EcsWorld world)
	{
		return world.GetShared<IGameSharedEnv>().ResourceLoader.GetSerializer().ToJson(GetValidationResult(world));
	}

	public static string GetValidationResultMD5(EcsWorld world)
	{
		IGameSerializer serializer = world.GetShared<IGameSharedEnv>().ResourceLoader.GetSerializer();
		string validationResultJson = GetValidationResultJson(world);
		return serializer.GetMD5(validationResultJson);
	}

	public static GGGoReplay GetReplay(EcsWorld world)
	{
		GGGoEnv shared = world.GetShared<GGGoEnv>();
		if (string.IsNullOrEmpty(shared.GameResult.VerifyJson))
		{
			throw new Exception("GameResult.VerifyJson is empty, can't create replay");
		}
		string mD = shared.ResourceLoader.GetSerializer().GetMD5(shared.GameResult.VerifyJson);
		string levelPath = shared.InitData.LevelPath;
		List<SyncCommand> commands = shared.Commands.Commands;
		return new GGGoReplay
		{
			LevelPath = levelPath,
			MD5 = mD,
			Commands = commands
		};
	}

	public static string GetReplayJson(EcsWorld world)
	{
		return JsonConvert.SerializeObject(GetReplay(world), Formatting.None);
	}

	public static void SaveReplayToFile(EcsWorld world, string filename)
	{
		string replayJson = GetReplayJson(world);
		File.WriteAllText(filename, replayJson, Encoding.UTF8);
	}

	public static GGGoReplay LoadReplayFromFile(string filename)
	{
		return JsonConvert.DeserializeObject<GGGoReplay>(File.ReadAllText(filename, Encoding.UTF8));
	}

	public static void SaveCommandsToFile(EcsWorld world, string filename)
	{
		string contents = JsonConvert.SerializeObject(world.GetShared<GGGoEnv>().Commands, Formatting.Indented);
		File.WriteAllText(filename, contents, Encoding.UTF8);
	}

	public static int CreateEntityXXX(GameWorld world)
	{
		return 0;
	}
}
