using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class SystemClientGame : IEcsRunSystem, IEcsSystem, IEcsInitSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnvClient> _shared;

	private float _waitClientOver;

	private readonly float WaitTime = 8f;

	public void Init(IEcsSystems systems)
	{
		_waitClientOver = WaitTime;
	}

	public void Run(IEcsSystems systems)
	{
		switch (_shared.Value.GameType)
		{
		case EGameType.PvpClient:
			CheckPvpGameOver();
			break;
		case EGameType.PveClient:
			CheckPveGameOver();
			break;
		}
	}

	private void CheckPvpGameOver()
	{
		if (_shared.Value.GameOver)
		{
			if (_shared.Value.Cinematic.Triggered && _shared.Value.Cinematic.Done)
			{
				FuncUI.ShowResult(_world.Value, showUI: true);
				_shared.Value.Cinematic.Done = false;
			}
			return;
		}
		if (_shared.Value.ServerGameOver && !_shared.Value.ClientGameOver && _waitClientOver > 0f)
		{
			_waitClientOver -= Time.deltaTime;
			if (_waitClientOver < 0f)
			{
				_world.Value.LogError("[GGGo] ClientGameOver Fair Please Check");
				FuncUI.ShowNetWork(_world.Value);
				return;
			}
		}
		if (_shared.Value.ClientGameOver && _shared.Value.ServerGameOver)
		{
			_shared.Value.GameOver = true;
			_shared.Value.GameState = EGameWorldState.Settlement;
			SnapshotFocusY(_shared.Value);
			FuncUI.ShowResult(_world.Value);
			_shared.Value.Cinematic.Triggered = true;
		}
	}

	private void CheckPveGameOver()
	{
		if (_shared.Value.ClientGameOver)
		{
			if (_shared.Value.Cinematic.Triggered && _shared.Value.Cinematic.Done)
			{
				FuncUI.ShowResult(_world.Value, showUI: true);
				_shared.Value.Cinematic.Done = false;
			}
		}
		else if (_shared.Value.GameOver)
		{
			_shared.Value.ClientGameOver = true;
			_shared.Value.GameState = EGameWorldState.Settlement;
			bool num = _shared.Value.GameResult != null && _shared.Value.GameResult.IsDestination;
			FuncUI.ShowResult(_world.Value);
			if (num)
			{
				SnapshotFocusY(_shared.Value);
				_shared.Value.Cinematic.Triggered = true;
			}
			else
			{
				_shared.Value.Cinematic.SkipEffects = true;
				_shared.Value.Cinematic.Triggered = true;
			}
		}
	}

	private void SnapshotFocusY(GGGoEnvClient env)
	{
		EcsWorld value = _world.Value;
		GGGoGameResult gameResult = env.GameResult;
		if (gameResult == null)
		{
			return;
		}
		int num = -1;
		if (gameResult.IsDestination)
		{
			num = FindPlayerEntity(value, gameResult.Statistics.WinPlayerId);
		}
		if (num < 0 && gameResult.Statistics.IsDead)
		{
			int num2 = FindDeadPlayerEntity(value);
			bool flag = false;
			if (env.GameType == EGameType.PvpClient && num2 >= 0)
			{
				EcsPool<ComponentPosition> pool = value.GetPool<ComponentPosition>();
				if (pool.Has(num2))
				{
					flag = (float)pool.Get(num2).Position.Y - env.Distance.AsFloat <= (float)env.Level.RangeVertical.X + 0.1f;
				}
			}
			num = (flag ? FindPlayerEntity(value, gameResult.Statistics.WinPlayerId) : num2);
		}
		if (num < 0)
		{
			num = FindPlayerEntity(value, gameResult.Statistics.WinPlayerId);
		}
		if (num < 0)
		{
			return;
		}
		EcsPool<ComponentPosition> pool2 = value.GetPool<ComponentPosition>();
		if (pool2.Has(num))
		{
			ref ComponentPosition reference = ref pool2.Get(num);
			env.Cinematic.FocusLogicY = (float)reference.Position.Y;
			env.Cinematic.FocusLogicX = (float)reference.Position.X;
			env.Cinematic.FocusLogicBottomY = env.Cinematic.FocusLogicY;
			env.Cinematic.FocusLogicTopY = env.Cinematic.FocusLogicY;
			EcsPool<ComponentCollider> pool3 = value.GetPool<ComponentCollider>();
			if (pool3.Has(num))
			{
				ref ComponentCollider reference2 = ref pool3.Get(num);
				env.Cinematic.FocusLogicBottomY = (float)(reference.Position.Y + reference2.Offset.Y - reference2.HalfSize.Y);
				env.Cinematic.FocusLogicTopY = (float)(reference.Position.Y + reference2.Offset.Y + reference2.HalfSize.Y);
			}
			Debug.Log($"[Cinematic] Snapshot focusLogicX={env.Cinematic.FocusLogicX:F2} focusLogicY={env.Cinematic.FocusLogicY:F2} bottomY={env.Cinematic.FocusLogicBottomY:F2} topY={env.Cinematic.FocusLogicTopY:F2} entity={num}");
		}
	}

	private int FindPlayerEntity(EcsWorld ecs, EPlayerID playerID)
	{
		EcsFilter ecsFilter = ecs.Filter<ComponentPlayer>().Inc<ComponentPosition>().End();
		EcsPool<ComponentPlayer> pool = ecs.GetPool<ComponentPlayer>();
		foreach (int item in ecsFilter)
		{
			if (pool.Get(item).PlayerID == playerID)
			{
				return item;
			}
		}
		return -1;
	}

	private int FindDeadPlayerEntity(EcsWorld ecs)
	{
		foreach (int item in ecs.Filter<ComponentPlayer>().Inc<ComponentPosition>().End())
		{
			if (FuncData.GetBoolData(ecs, item, PropertyID.Die))
			{
				return item;
			}
		}
		return -1;
	}
}
