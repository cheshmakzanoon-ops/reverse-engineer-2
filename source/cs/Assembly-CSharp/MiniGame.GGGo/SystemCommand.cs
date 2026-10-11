using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemCommand : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsPoolInject<ComponentAcceleration> _poolAcc;

	public void Run(IEcsSystems systems)
	{
		GGGoCommand commands = _env.Value.Commands;
		int logicTickCount = _env.Value.LogicTickCount;
		SyncCommand command;
		while (commands.TryPickEvent(logicTickCount, out command))
		{
			_env.Value.FrameSyncIsNeeded = true;
			command.IsValid = DoCommand(command);
		}
	}

	private bool DoCommand(SyncCommand command)
	{
		EcsWorld value = _world.Value;
		int entityID = command.EntityID;
		if (entityID < 0 || value == null || !value.IsEntityAliveInternal(entityID))
		{
			return false;
		}
		switch (command.CommandType)
		{
		case 0:
			SetAcceleration(value, entityID, new FVector2(FP.Zero, FP.Zero));
			FuncEvent.Broadcast(value, new EventMoveState(value.PackEntity(entityID), 0));
			break;
		case 1:
			SetAcceleration(value, entityID, new FVector2(-FP.One, FP.Zero));
			FuncEvent.Broadcast(value, new EventMoveState(value.PackEntity(entityID), 1));
			break;
		case 2:
			SetAcceleration(value, entityID, new FVector2(FP.One, FP.Zero));
			FuncEvent.Broadcast(value, new EventMoveState(value.PackEntity(entityID), 2));
			break;
		case 3:
		{
			ItemType currentItemType = FuncEntity.GetCurrentItemType(_world.Value, entityID);
			if (currentItemType != ItemType.None)
			{
				FuncEvent.Broadcast(value, new EventTryUseItem(value.PackEntity(entityID), EcsPackedEntity.Invalid, currentItemType));
			}
			break;
		}
		default:
			return false;
		}
		return true;
	}

	private void SetAcceleration(EcsWorld world, int entity, FVector2 rate)
	{
		ref ComponentAcceleration orAdd = ref _poolAcc.Value.GetOrAdd(entity);
		orAdd.Value = rate * orAdd.MoveAcc;
	}
}
