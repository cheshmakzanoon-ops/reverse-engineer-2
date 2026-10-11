using System;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo.Client;

public static class FuncUI
{
	public static void FireRender(EcsWorld world, IRender render)
	{
		world.GetShared<GGGoEnvClient>().RenderQueue.Enqueue(render);
	}

	public static void InitGameUI(EcsWorld world, float waitTime)
	{
		RefreshUI(world);
		DataUIRender.UIWait uIWait = DataUIRender.UIRender<DataUIRender.UIWait>.Fetch();
		uIWait.WaitTime = waitTime;
		FireRender(world, uIWait);
	}

	public static void StartGameUI(EcsWorld world)
	{
		RefreshUI(world, isStart: true);
	}

	public static void RefreshUI(EcsWorld world, bool isStart = false)
	{
		DataUIRender.UIRefresh uIRefresh = DataUIRender.UIRender<DataUIRender.UIRefresh>.Fetch();
		uIRefresh.IsStart = isStart;
		uIRefresh.PlayerInfos = GetHpInfo(world, ref uIRefresh.PlayerInfos);
		uIRefresh.ItemType = FuncEntity.GetCurrentItemType(world, GetMainPlayer(world));
		FireRender(world, uIRefresh);
	}

	public static void RefreshHpUI(EcsWorld world, int entity, bool isHurt = true)
	{
		ref ComponentPlayer reference = ref world.GetPool<ComponentPlayer>().Get(entity);
		float floatData = FuncData.GetFloatData(world, entity, PropertyID.CurHp);
		FP fpData = FuncData.GetFpData(world, entity, PropertyID.MaxHp);
		DataUIRender.UIHpChange uIHpChange = DataUIRender.UIRender<DataUIRender.UIHpChange>.Fetch();
		uIHpChange.PlayerInfo = new UIPlayerInfo
		{
			PlayerID = reference.PlayerID,
			CurHp = floatData,
			MaxHp = (int)(long)FP.Ceiling(fpData)
		};
		FireRender(world, uIHpChange);
	}

	public static void RefreshMovementUI(EcsWorld world, int entity, int moveState)
	{
		ref ComponentPlayer reference = ref world.GetPool<ComponentPlayer>().Get(entity);
		DataUIRender.UIMovementChange uIMovementChange = DataUIRender.UIRender<DataUIRender.UIMovementChange>.Fetch();
		uIMovementChange.PlayerID = reference.PlayerID;
		uIMovementChange.MoveState = moveState;
		FireRender(world, uIMovementChange);
	}

	public static void RefreshItemUI(EcsWorld world, int entity)
	{
		GGGoEnvClient shared = world.GetShared<GGGoEnvClient>();
		if (world.GetPool<ComponentPlayer>().Get(entity).PlayerID == shared.GetPlayerID())
		{
			ItemType currentItemType = FuncEntity.GetCurrentItemType(world, entity);
			DataUIRender.UIItemType uIItemType = DataUIRender.UIRender<DataUIRender.UIItemType>.Fetch();
			uIItemType.ItemType = currentItemType;
			FireRender(world, uIItemType);
		}
	}

	public static void RefreshPlayerUseItemUI(EcsWorld world, int entity, ItemType itemType)
	{
		ref ComponentPlayer reference = ref world.GetPool<ComponentPlayer>().Get(entity);
		DataUIRender.UIPlayerUseItem uIPlayerUseItem = DataUIRender.UIRender<DataUIRender.UIPlayerUseItem>.Fetch();
		uIPlayerUseItem.PlayerID = reference.PlayerID;
		uIPlayerUseItem.ItemType = itemType;
		FireRender(world, uIPlayerUseItem);
	}

	public static void ShowResult(EcsWorld world, bool showUI = false)
	{
		if (TryGetResultInfo(world, out var result))
		{
			result.ShowUI = showUI;
			FireRender(world, result);
		}
	}

	public static void ShowNetWork(EcsWorld world, bool succOrFair = false)
	{
		DataUIRender.UINetWork uINetWork = DataUIRender.UIRender<DataUIRender.UINetWork>.Fetch();
		uINetWork.SuccOrFair = succOrFair;
		FireRender(world, uINetWork);
	}

	public static int GetMainPlayer(EcsWorld world)
	{
		EPlayerID playerID = world.GetShared<GGGoEnvClient>().GetPlayerID();
		foreach (int item in world.Filter<ComponentPlayer>().Inc<ComponentData>().End())
		{
			if (world.GetPool<ComponentPlayer>().Get(item).PlayerID == playerID)
			{
				return item;
			}
		}
		return -1;
	}

	public static UIPlayerInfo[] GetHpInfo(EcsWorld world, ref UIPlayerInfo[] players)
	{
		EcsFilter ecsFilter = world.Filter<ComponentPlayer>().Inc<ComponentData>().End();
		Array.Resize(ref players, ecsFilter.GetEntitiesCount());
		int num = 0;
		foreach (int item in ecsFilter)
		{
			ref ComponentPlayer reference = ref world.GetPool<ComponentPlayer>().Get(item);
			float floatData = FuncData.GetFloatData(world, item, PropertyID.CurHp);
			int intData = FuncData.GetIntData(world, item, PropertyID.MaxHp);
			players[num++] = new UIPlayerInfo
			{
				PlayerID = reference.PlayerID,
				CurHp = floatData,
				MaxHp = intData
			};
		}
		return players;
	}

	public static bool TryGetResultInfo(EcsWorld world, out DataUIRender.UIResult result)
	{
		result = DataUIRender.UIRender<DataUIRender.UIResult>.Fetch();
		GGGoEnvClient shared = world.GetShared<GGGoEnvClient>();
		if (!shared.GameOver)
		{
			return false;
		}
		result.SelfPlayerID = (int)shared.GetPlayerID();
		result.WinPlayerID = shared.GameResult.Statistics.WinPlayerId;
		result.IsWin = shared.GameResult.Statistics.WinPlayerId == shared.GetPlayerID();
		result.battleTimeMills = (long)(((FP)shared.GameResult.Statistics.GameEndFrame * shared.LogicTickDelta - (FP)shared.WaitToStartTime) * (FP)1000);
		result.PlayerInfos = GetHpInfo(world, ref result.PlayerInfos);
		if (shared.GameResult.Replay != null)
		{
			IGameSerializer serializer = shared.ResourceLoader.GetSerializer();
			result.ValidationStr = serializer.ToJson(shared.GameResult.Replay);
		}
		return true;
	}
}
