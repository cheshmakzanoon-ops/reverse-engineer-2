using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class GGGoEnvClient : GGGoEnv, ISnapshotRuntime, IDisposable
{
	public class RuntimeSnapshot
	{
		public int LogicTickCount;

		public int PhysicsTickCount;

		public FP RollSpeed;

		public FP Distance;

		public GGGoCommand Commands;

		public SpawnCacheEntry[] SpawnCache;
	}

	public enum InputKey
	{
		Skill,
		Left,
		Right
	}

	public class CinematicState
	{
		public bool Triggered { get; set; }

		public bool Done { get; set; }

		public bool OverrideRoll { get; set; }

		public float FocusLogicY { get; set; } = float.MinValue;

		public float FocusLogicX { get; set; } = float.MinValue;

		public float FocusLogicBottomY { get; set; } = float.MinValue;

		public float FocusLogicTopY { get; set; } = float.MinValue;

		public bool SkipEffects { get; set; }
	}

	public GGGoScene Scene;

	public GGGoClientPlayer Player;

	public List<SyncCommand> PredictCommands = new List<SyncCommand>();

	public Dictionary<InputKey, bool> input = new Dictionary<InputKey, bool>();

	public Dictionary<int, int> InputState = new Dictionary<int, int>();

	public int RollbackSyncTick { get; private set; } = -1;

	public int RollbackTargetTick { get; private set; } = -1;

	public bool IgnoreInput { get; set; }

	public bool IsRollbacking => RollbackTargetTick > RollbackSyncTick;

	public Action<IRender> UIAction { get; set; }

	public Queue<IRender> RenderQueue { get; } = new Queue<IRender>();

	public bool UseRuntimeSnapshot { get; set; }

	public CinematicState Cinematic { get; } = new CinematicState();

	public void BeginRollback(int syncFrame, int targetFrame)
	{
		IgnoreInput = true;
		RollbackSyncTick = syncFrame;
		RollbackTargetTick = targetFrame;
	}

	public void EndRollback()
	{
		IgnoreInput = false;
		RollbackSyncTick = -1;
		RollbackTargetTick = -1;
	}

	public EPlayerID GetPlayerID()
	{
		if (Player == null)
		{
			return EPlayerID.ID_1P;
		}
		return (EPlayerID)Player.PlayerID;
	}

	public bool IsRealClient()
	{
		if (GameType != EGameType.PveClient && GameType != EGameType.PvpClient)
		{
			return false;
		}
		if (GameEntry.Resource == null)
		{
			return false;
		}
		return true;
	}

	public object TakeSnapshotRuntime(object data)
	{
		RuntimeSnapshot runtimeSnapshot = data as RuntimeSnapshot;
		if (runtimeSnapshot == null)
		{
			runtimeSnapshot = new RuntimeSnapshot();
		}
		runtimeSnapshot.LogicTickCount = base.LogicTickCount;
		runtimeSnapshot.PhysicsTickCount = base.PhysicsTickCount;
		runtimeSnapshot.RollSpeed = RollSpeed;
		runtimeSnapshot.Distance = Distance;
		runtimeSnapshot.Commands = base.Commands.CopyTo(runtimeSnapshot.Commands);
		if (runtimeSnapshot.SpawnCache == null || runtimeSnapshot.SpawnCache.Length != SpawnCache.Length)
		{
			runtimeSnapshot.SpawnCache = new SpawnCacheEntry[SpawnCache.Length];
		}
		Array.Copy(SpawnCache, runtimeSnapshot.SpawnCache, SpawnCache.Length);
		return runtimeSnapshot;
	}

	public void RestoreSnapshotRuntime(object data)
	{
		RuntimeSnapshot runtimeSnapshot = data as RuntimeSnapshot;
		base.LogicTickCount = runtimeSnapshot.LogicTickCount;
		base.PhysicsTickCount = runtimeSnapshot.PhysicsTickCount;
		RollSpeed = runtimeSnapshot.RollSpeed;
		Distance = runtimeSnapshot.Distance;
		base.Commands = runtimeSnapshot.Commands.CopyTo(base.Commands);
		if (SpawnCache == null || SpawnCache.Length != runtimeSnapshot.SpawnCache.Length)
		{
			SpawnCache = new SpawnCacheEntry[runtimeSnapshot.SpawnCache.Length];
		}
		Array.Copy(runtimeSnapshot.SpawnCache, SpawnCache, runtimeSnapshot.SpawnCache.Length);
	}

	public override object TakeSnapshot()
	{
		if (!UseRuntimeSnapshot)
		{
			return base.TakeSnapshot();
		}
		return TakeSnapshotRuntime(null);
	}

	public override void RestoreSnapshot(object snapshot)
	{
		if (!UseRuntimeSnapshot)
		{
			base.RestoreSnapshot(snapshot);
		}
		else
		{
			RestoreSnapshotRuntime(snapshot);
		}
	}

	public void Dispose()
	{
		ResourceLoader?.Dispose();
		ResourceLoader = null;
		if (Scene != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(Scene.gameObject);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(Scene.gameObject);
			}
			Scene = null;
		}
	}
}
