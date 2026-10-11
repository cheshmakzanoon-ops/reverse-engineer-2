using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo.Client;

public static class FuncTCClient
{
	private static bool InitTC;

	public static void InitAction(EcsWorld world)
	{
		if (!InitTC)
		{
			FuncEvent.RegisterEvent<EventGameInit>(world, OnGameInit);
			FuncEvent.RegisterEvent<EventGameStart>(world, OnGameStart);
			FuncEvent.RegisterEvent<EventGameOver>(world, OnGameOver);
			FuncEvent.RegisterEvent<EventDataChange>(world, OnDataChange);
			FuncEvent.RegisterEvent<EventTrigger>(world, OnEventTrigger);
			FuncEvent.RegisterEvent<EventMoveState>(world, OnMoveState);
			FuncEvent.RegisterEvent<EventCharacterHurt>(world, OnCharacterHurt);
			FuncEvent.RegisterEvent<EventTryUseItem>(world, TryUseItem);
			FuncEvent.RegisterEvent<EventSkillRelease>(world, OnSkillRelease);
			FuncEvent.RegisterEvent<EventSkillFinished>(world, OnSkillFinished);
			FuncAction.RegisterAction<PickItemAction>(world, PickItemAction);
			FuncAction.RegisterAction<UseItemAction>(world, UseItemAction);
			InitTC = true;
		}
	}

	public static void ClearAction(EcsWorld world)
	{
		FuncEvent.UnregisterEvent<EventGameInit>(world, OnGameInit);
		FuncEvent.UnregisterEvent<EventGameStart>(world, OnGameStart);
		FuncEvent.UnregisterEvent<EventGameOver>(world, OnGameOver);
		FuncEvent.UnregisterEvent<EventDataChange>(world, OnDataChange);
		FuncEvent.UnregisterEvent<EventTrigger>(world, OnEventTrigger);
		FuncEvent.UnregisterEvent<EventMoveState>(world, OnMoveState);
		FuncEvent.UnregisterEvent<EventCharacterHurt>(world, OnCharacterHurt);
		FuncEvent.UnregisterEvent<EventTryUseItem>(world, TryUseItem);
		FuncEvent.UnregisterEvent<EventSkillRelease>(world, OnSkillRelease);
		FuncEvent.UnregisterEvent<EventSkillFinished>(world, OnSkillFinished);
		FuncAction.UnregisterAction<PickItemAction>(world, PickItemAction);
		FuncAction.UnregisterAction<UseItemAction>(world, UseItemAction);
		InitTC = false;
	}

	private static void OnGameInit(EcsWorld world, IEvent e)
	{
		if (e is EventGameInit eventGameInit)
		{
			FuncUI.InitGameUI(world, eventGameInit.WaitTime.ToFloat());
		}
		else
		{
			FuncUI.InitGameUI(world, 0f);
		}
	}

	private static void OnGameStart(EcsWorld world, IEvent e)
	{
		FuncUI.StartGameUI(world);
	}

	private static void OnGameOver(EcsWorld world, IEvent e)
	{
		FuncUI.ShowResult(world);
	}

	private static void OnDataChange(EcsWorld world, IEvent eEvent)
	{
		if (eEvent is EventDataChange { Sender: var packed } eventDataChange && packed.Unpack(world, out var entity) && eventDataChange.PropertyID == 3)
		{
			FuncUI.RefreshHpUI(world, entity);
		}
	}

	private static void OnEventTrigger(EcsWorld world, IEvent e)
	{
		if (!(e is EventTrigger { Sender: var packed } eventTrigger))
		{
			return;
		}
		packed.Unpack(world, out var entity);
		bool flag = false;
		if (eventTrigger.Target.Unpack(world, out var entity2))
		{
			EcsPool<ComponentPlayer> pool = world.GetPool<ComponentPlayer>();
			if (pool.Has(entity2) && pool.Get(entity2).PlayerID == world.GetShared<GGGoEnvClient>().GetPlayerID())
			{
				flag = true;
			}
		}
		switch (eventTrigger.Type)
		{
		case EventTriggerType.PlatformNormal:
			if (flag)
			{
				DataUISound.PlayerSound(6100027);
			}
			break;
		case EventTriggerType.PlatformSpike:
			if (flag)
			{
				DataUISound.PlayerSound(6100029);
			}
			break;
		case EventTriggerType.PlatformBounce:
			FuncClient.PlaySkeletonAnimation(world, entity, "jump");
			if (flag)
			{
				DataUISound.PlayerSound(6100031);
			}
			break;
		case EventTriggerType.PlatformBreak:
			FuncClient.PlaySkeletonAnimation(world, entity, "normal_smash");
			if (flag)
			{
				DataUISound.PlayerSound(6100028);
			}
			break;
		case EventTriggerType.PlatformRoll:
			if (flag)
			{
				DataUISound.PlayerSound(6100030);
			}
			break;
		}
	}

	private static void OnMoveState(EcsWorld world, IEvent e)
	{
		if (e is EventMoveState { Sender: var packed } eventMoveState && packed.Unpack(world, out var entity))
		{
			FuncUI.RefreshMovementUI(world, entity, eventMoveState.MoveState);
		}
	}

	private static void OnCharacterHurt(EcsWorld world, IEvent e)
	{
		if (e is EventCharacterHurt { Target: var packed } eventCharacterHurt && packed.Unpack(world, out var entity))
		{
			EcsPool<ComponentPlayer> pool = world.GetPool<ComponentPlayer>();
			if (pool.Has(entity))
			{
				ref ComponentPlayer reference = ref pool.Get(entity);
				DataUIRender.UIPlayerHurt uIPlayerHurt = DataUIRender.UIRender<DataUIRender.UIPlayerHurt>.Fetch();
				uIPlayerHurt.PlayerID = reference.PlayerID;
				uIPlayerHurt.ProtectTime = FuncBuff.GetRemaining(world, entity, 3);
				uIPlayerHurt.HurtValue = eventCharacterHurt.HurtValue.ToFloat();
				FuncUI.FireRender(world, uIPlayerHurt);
			}
		}
	}

	private static void PickItemAction(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is PickItemAction pickItemAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, pickItemAction.Target);
			FuncUI.RefreshItemUI(world, entity);
		}
	}

	private static void UseItemAction(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is UseItemAction useItemAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, useItemAction.Target);
			FuncUI.RefreshItemUI(world, entity);
		}
	}

	private static void TryUseItem(EcsWorld world, IEvent e)
	{
		if (e is EventTryUseItem { Sender: var packed } eventTryUseItem && packed.Unpack(world, out var entity) && eventTryUseItem.ItemType != ItemType.None)
		{
			FuncUI.RefreshPlayerUseItemUI(world, entity, eventTryUseItem.ItemType);
		}
	}

	private static void OnSkillRelease(EcsWorld world, IEvent e)
	{
	}

	private static void OnSkillFinished(EcsWorld world, IEvent e)
	{
	}
}
