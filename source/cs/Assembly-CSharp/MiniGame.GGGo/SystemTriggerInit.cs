using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemTriggerInit : IEcsInitSystem, IEcsSystem, IEcsDestroySystem
{
	private EcsWorldInject world;

	private EcsSharedInject<IGameSharedEnv> _shared;

	public void Init(IEcsSystems systems)
	{
		Register();
	}

	public void Destroy(IEcsSystems systems)
	{
		FuncCondition.Clear(world.Value);
		FuncAction.Clear(world.Value);
	}

	public void Register()
	{
		EcsWorld value = world.Value;
		FuncCondition.RegisterCondition<EventWithMeCondition>(value, EventWithMeCondition.Check);
		FuncCondition.RegisterCondition<DataChangeCondition>(value, DataChangeCondition.Check);
		FuncCondition.RegisterCondition<DataCheckCondition>(value, DataCheckCondition.Check);
		FuncCondition.RegisterCondition<TargetDataCheckCondition>(value, TargetDataCheckCondition.Check);
		FuncCondition.RegisterCondition<TargetDataCompareCondition>(value, TargetDataCompareCondition.Check);
		FuncCondition.RegisterCondition<EventTriggerCondition>(value, EventTriggerCondition.Check);
		FuncCondition.RegisterCondition<CollectionLayerCondition>(value, CollectionLayerCondition.Check);
		FuncCondition.RegisterCondition<HasBuffCondition>(value, HasBuffCondition.Check);
		FuncAction.RegisterAction<SetDataAction>(value, SetDataAction.Execute);
		FuncAction.RegisterAction<ChangeActivityAction>(value, ChangeActivityAction.Execute);
		FuncAction.RegisterAction<ToggleActivityAction>(value, ToggleActivityAction.Execute);
		FuncAction.RegisterAction<AddTriggerAction>(value, AddTriggerAction.Execute);
		FuncAction.RegisterAction<RemoveTriggerAction>(value, RemoveTriggerAction.Execute);
		FuncAction.RegisterAction<EventTriggerBroadAction>(value, EventTriggerBroadAction.Execute);
		FuncAction.RegisterAction<PickItemAction>(value, PickItemAction.Execute);
		FuncAction.RegisterAction<UseItemAction>(value, UseItemAction.Execute);
		FuncAction.RegisterAction<AddBuffAction>(value, AddBuffAction.Execute);
		FuncAction.RegisterAction<RemoveBuffAction>(value, RemoveBuffAction.Execute);
		FuncAction.RegisterAction<RigidBodyDisableAction>(value, RigidBodyDisableAction.Execute);
		FuncAction.RegisterAction<StaticAction>(value, StaticAction.Execute);
		FuncAction.RegisterAction<ControlDisableAction>(value, ControlDisableAction.Execute);
		FuncAction.RegisterAction<CheckGameOverAction>(value, CheckGameOverAction.Execute);
		FuncAction.RegisterAction<RemoveEntityAction>(value, RemoveEntityAction.Execute);
		FuncAction.RegisterAction<CharacterHurtAction>(value, CharacterHurtAction.Execute);
		FuncAction.RegisterAction<SetVelocityActionY>(value, SetVelocityActionY.Execute);
		FuncAction.RegisterAction<SetMoveDirectionAction>(value, SetMoveDirectionAction.Execute);
		FuncAction.RegisterAction<MoveToTopAction>(value, MoveToTopAction.Execute);
	}
}
