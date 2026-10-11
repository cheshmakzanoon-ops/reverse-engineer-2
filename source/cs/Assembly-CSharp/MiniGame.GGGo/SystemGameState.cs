using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemGameState : IEcsInitSystem, IEcsSystem, IEcsRunSystem
{
	private EcsWorldInject _world;

	private EcsSharedInject<GGGoEnv> _shared;

	private EGameWorldState _lastGameState = EGameWorldState.Uninitialized;

	private bool _gameStartEventSent;

	private bool _gameInitEventSent;

	public void Init(IEcsSystems systems)
	{
		GGGoEnv value = _shared.Value;
		if (value != null)
		{
			if (value.GameType == EGameType.PvpClient || value.GameType == EGameType.PvpServer || value.GameType == EGameType.PvpReplay)
			{
				FuncEntity.FillPvpCharacter(_world.Value);
			}
			else
			{
				FuncEntity.FillPveCharacter(_world.Value);
			}
			_gameInitEventSent = false;
			_gameStartEventSent = false;
		}
	}

	public void Run(IEcsSystems systems)
	{
		GGGoEnv value = _shared.Value;
		if (value != null)
		{
			if (value.GameState < _lastGameState)
			{
				OnStateRollback(value.GameState);
			}
			_lastGameState = value.GameState;
			EGameWorldState gameState = value.GameState;
			if (gameState == EGameWorldState.Running)
			{
				HandleInitialized(value);
				HandlePreparing(value);
			}
		}
	}

	private void OnStateRollback(EGameWorldState currentState)
	{
		switch (currentState)
		{
		case EGameWorldState.Initialized:
		case EGameWorldState.Preparing:
			_gameInitEventSent = false;
			_gameStartEventSent = false;
			break;
		case EGameWorldState.Running:
		{
			GGGoEnv value = _shared.Value;
			if (value.LogicTime < value.WaitToStartTime)
			{
				_gameStartEventSent = false;
			}
			break;
		}
		}
	}

	private void HandleInitialized(GGGoEnv env)
	{
		if (!_gameInitEventSent)
		{
			FP waitTime = (FP)env.WaitToStartTime - env.LogicTime;
			FuncEvent.Broadcast(_world.Value, new EventGameInit
			{
				WaitTime = waitTime
			});
			_gameInitEventSent = true;
		}
	}

	private void HandlePreparing(GGGoEnv env)
	{
		if (env.LogicTime > env.WaitToStartTime && !_gameStartEventSent)
		{
			FuncEvent.Broadcast(_world.Value, default(EventGameStart));
			_gameStartEventSent = true;
		}
	}
}
