using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

public static class FuncItem
{
	private static string ItemPath = "Assets/Main/MiniGameRes/GGGo/Prefab/Item/{0}.prefab";

	public static void UseItem(EcsWorld world, int entity, ItemType itemType)
	{
		int num = entity;
		switch (itemType)
		{
		default:
			return;
		case ItemType.Box:
			UseBox(world, entity);
			break;
		case ItemType.Bubble:
		{
			num = GetTarget(world, entity);
			if (num < 0)
			{
				return;
			}
			ref ComponentSkillRelease reference4 = ref NewSKillEntity(world, entity, itemType, "BubbleShot");
			reference4.TargetEntity = num;
			reference4.Duration = FP.One;
			break;
		}
		case ItemType.RocketFish:
		{
			num = GetTarget(world, entity);
			if (num < 0)
			{
				return;
			}
			ref ComponentSkillRelease reference3 = ref NewSKillEntity(world, entity, itemType, "RocketShot");
			reference3.TargetEntity = num;
			reference3.Duration = 3;
			break;
		}
		case ItemType.IceCloud:
			NewSKillEntity(world, entity, itemType, "Cloud").Duration = 3;
			break;
		case ItemType.IceBlock:
			SetIcePlatform(world, entity);
			break;
		case ItemType.Angel:
		{
			ref ComponentSkillRelease reference2 = ref NewSKillEntity(world, entity, itemType, "Buff_Shield");
			reference2.Duration = 3;
			FuncBuff.AddBuff(world, entity, BuffId.Shield, reference2.Duration);
			break;
		}
		case ItemType.RedHeart:
			NewSKillEntity(world, entity, itemType, "Buff_Heal").Duration = 1;
			FuncData.ChangeData_HP(world, entity, FP.One);
			break;
		case ItemType.SpeedBoost:
		{
			ref ComponentSkillRelease reference = ref NewSKillEntity(world, entity, itemType, "Buff_Speed");
			reference.Duration = 3;
			FuncBuff.AddBuff(world, entity, BuffId.GroundSpeedUp, reference.Duration);
			break;
		}
		}
		FuncEvent.Broadcast(world, new EventSkillRelease(world.PackEntity(entity), world.PackEntity(num), itemType));
	}

	public static int GetTarget(EcsWorld world, int entity)
	{
		foreach (int item in world.Filter<ComponentPlayer>().End())
		{
			if (item != entity)
			{
				return item;
			}
		}
		return -1;
	}

	public static ref ComponentSkillRelease NewSKillEntity(EcsWorld world, int entity, ItemType itemType, string resource = null)
	{
		int entity2 = world.NewEntity();
		ref ComponentSkillRelease reference = ref world.GetPool<ComponentSkillRelease>().Add(entity2);
		reference.TargetEntity = entity;
		reference.SkillType = itemType;
		if (resource != null)
		{
			ref ComponentResource reference2 = ref world.GetPool<ComponentResource>().Add(entity2);
			reference2.Asset = string.Format(ItemPath, resource);
			reference2.Type = ResourceType.Skill;
		}
		return ref reference;
	}

	public static void UseBox(EcsWorld world, int entity)
	{
		ref ComponentItemHold orAdd = ref world.GetPool<ComponentItemHold>().GetOrAdd(entity);
		uint itemType = new PesudoRandom((uint)(long)(world.GetShared<GameSharedEnv>().LogicTime + (FP)entity)).Next(2u, 9u);
		orAdd.ItemType = (ItemType)itemType;
	}

	public static void SetIcePlatform(EcsWorld world, int entity)
	{
	}
}
