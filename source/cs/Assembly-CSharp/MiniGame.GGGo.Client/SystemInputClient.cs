using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class SystemInputClient : IEcsRunSystem, IEcsSystem
{
	protected readonly EcsWorldInject _world;

	protected readonly EcsSharedInject<GGGoEnvClient> _env;

	protected readonly EcsFilterInject<Inc<ComponentPlayer>, Exc<ComponentStatic>> _filter;

	protected readonly EcsPoolInject<ComponentPlayer> _poolPlayer;

	public void Run(IEcsSystems systems)
	{
		if (!_env.Value.IgnoreInput && FuncGame.IsPlaying(_world.Value))
		{
			RunInput();
		}
	}

	private void RunInput()
	{
		if (_env.Value.GameType == EGameType.Replay || _env.Value.GameType == EGameType.PvpReplay)
		{
			return;
		}
		EPlayerID playerID = _env.Value.GetPlayerID();
		foreach (int item in _filter.Value)
		{
			if (_poolPlayer.Value.Get(item).PlayerID == playerID)
			{
				HandleUseItem(playerID, item);
				HandleMove(playerID, item);
			}
		}
		_env.Value.input.Clear();
	}

	protected void HandleUseItem(EPlayerID mainPlayerID, int entity)
	{
		if (InputUseItem(mainPlayerID) && FuncEntity.GetCurrentItemType(_world.Value, entity) != ItemType.None)
		{
			DoCommand(_env.Value.LogicTickCount, entity, 3);
		}
	}

	protected void HandleMove(EPlayerID mainPlayerID, int entity)
	{
		int num = SampleInputStateMove(mainPlayerID);
		_env.Value.InputState.TryGetValue(entity, out var value);
		if (value != num)
		{
			_env.Value.InputState[entity] = num;
			DoCommand(_env.Value.LogicTickCount, entity, num switch
			{
				1 => 1, 
				0 => 0, 
				_ => 2, 
			});
			ref ComponentPlayer reference = ref _poolPlayer.Value.Get(entity);
			DataUIRender.UIMovementChange uIMovementChange = DataUIRender.UIRender<DataUIRender.UIMovementChange>.Fetch();
			uIMovementChange.PlayerID = reference.PlayerID;
			uIMovementChange.MoveState = num;
			FuncUI.FireRender(_world.Value, uIMovementChange);
		}
	}

	protected virtual void DoCommand(int frame, int entity, int command)
	{
		if (_env.Value.Player == null)
		{
			_env.Value.Commands.QueueEvent(frame, entity, command, -1);
			return;
		}
		GGGoMsgFrameSyncResp.CmdState message = new GGGoMsgFrameSyncResp.CmdState
		{
			EntityID = entity,
			FrameIndex = -1,
			CommandType = command
		};
		_env.Value.Player.Send(message);
	}

	private int SampleInputStateMove(EPlayerID mainPlayerID)
	{
		int externalJoystickState = GetExternalJoystickState();
		if (externalJoystickState != -1)
		{
			return externalJoystickState;
		}
		return GetRuntimeKeyboardState(mainPlayerID);
	}

	private int GetRuntimeKeyboardState(EPlayerID mainPlayerID)
	{
		return 0;
	}

	private int GetExternalJoystickState()
	{
		_env.Value.input.TryGetValue(GGGoEnvClient.InputKey.Left, out var value);
		_env.Value.input.TryGetValue(GGGoEnvClient.InputKey.Right, out var value2);
		if (value && !value2)
		{
			return 2;
		}
		if (value2 && !value)
		{
			return 1;
		}
		return -1;
	}

	private bool ShouldUseWasd(EPlayerID playerID)
	{
		if (_env.Value.IsRealClient())
		{
			return playerID == _env.Value.GetPlayerID();
		}
		return playerID == EPlayerID.ID_1P;
	}

	private bool InputUseItem(EPlayerID playerID)
	{
		if (!Application.isPlaying)
		{
			return false;
		}
		if (GetExternalUseItem())
		{
			return true;
		}
		return false;
	}

	private bool GetExternalUseItem()
	{
		_env.Value.input.TryGetValue(GGGoEnvClient.InputKey.Skill, out var value);
		if (value)
		{
			return true;
		}
		return false;
	}
}
