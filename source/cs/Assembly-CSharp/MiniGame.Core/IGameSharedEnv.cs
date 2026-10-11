using Box2DSharp.Common;
using Leopotam.EcsLite;
using Newtonsoft.Json;

namespace MiniGame.Core;

public interface IGameSharedEnv : IGameUniqueIDRegister, ISnapshot
{
	EGameWorldState GameState { get; set; }

	bool IsPaused { get; set; }

	bool GameOver { get; set; }

	bool FrameSyncIsNeeded { get; set; }

	int FrameSyncTickInterval { get; }

	int WaitToStartTime { get; }

	FP FrameSyncLerpFactor { get; }

	FP PhysicsTickDelta { get; set; }

	FP LogicTickDelta { get; set; }

	FP PrepareTime { get; set; }

	FP GameTime { get; set; }

	FP LogicTime { get; }

	FP SettlementTime { get; set; }

	int LogicTickCount { get; set; }

	int LogicTickLockStep { get; set; }

	int PhysicsTickCount { get; set; }

	[JsonIgnore]
	IResourceLoader ResourceLoader { get; }

	EcsPackedEntity EventManager { get; set; }

	IGameLevelConfig GetLevelConfig();
}
