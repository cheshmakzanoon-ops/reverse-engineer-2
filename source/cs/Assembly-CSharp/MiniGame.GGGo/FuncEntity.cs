using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

public static class FuncEntity
{
	public static bool HasEntity(EcsWorld world, int entity)
	{
		return world.IsEntityAliveInternal(entity);
	}

	public static void DelEntity(EcsWorld world, int entity)
	{
		bool flag = world.GetPool<ComponentPlayer>().Has(entity);
		if (!flag || !FuncData.GetBoolData(world, entity, PropertyID.Die))
		{
			FuncUniqueID.TryGetUniqueIDByEntity(world, entity, out var id);
			FuncEvent.Broadcast(world, new EventEntityRemove(EcsPackedEntity.Invalid, world.PackEntity(entity), id));
			if (flag)
			{
				FuncData.SetData_Die(world, entity, entity, isDie: true);
			}
			else
			{
				world.DelEntity(entity);
			}
		}
	}

	public static void AddActiveComponent(EcsWorld ecsWorld, int entity, List<Type> includes = null)
	{
		AddComponent(ecsWorld, entity, typeof(ComponentActivitySnapshot));
		ref ComponentActivitySnapshot reference = ref ecsWorld.GetPool<ComponentActivitySnapshot>().Get(entity);
		reference.IsActive = true;
		if (includes != null)
		{
			reference.Includes = includes;
			return;
		}
		reference.Includes = new List<Type>
		{
			typeof(ComponentResource),
			typeof(ComponentPosition)
		};
	}

	public static void AddComponent(EcsWorld world, int entity, Type selectType)
	{
		IEcsPool poolByType = world.GetPoolByType(selectType);
		if (!poolByType.Has(entity))
		{
			poolByType.AddRaw(entity, Activator.CreateInstance(selectType));
		}
	}

	public static ItemType GetCurrentItemType(EcsWorld world, int entity = -1)
	{
		EcsPool<ComponentItemHold> pool = world.GetPool<ComponentItemHold>();
		if (pool.Has(entity))
		{
			return pool.Get(entity).ItemType;
		}
		return ItemType.None;
	}

	public static void FillPveCharacter(EcsWorld world)
	{
		EcsFilter ecsFilter = world.Filter<ComponentPlayer>().End();
		EcsPool<ComponentPlayer> pool = world.GetPool<ComponentPlayer>();
		using (EcsFilter.Enumerator enumerator = ecsFilter.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				pool.Get(current).PlayerID = EPlayerID.ID_1P;
				AddCharacterTriggers(world, current);
				return;
			}
		}
		int entity = CreateCharacter(world, EPlayerID.ID_1P, new FVector2(-2.4, FP._0_8));
		AddCharacterTriggers(world, entity);
	}

	public static void FillPvpCharacter(EcsWorld world)
	{
		EcsFilter ecsFilter = world.Filter<ComponentPlayer>().End();
		List<int> list = new List<int>();
		foreach (int item in ecsFilter)
		{
			list.Add(item);
		}
		EcsPool<ComponentPlayer> pool = world.GetPool<ComponentPlayer>();
		for (int i = 0; i < list.Count && i < 2; i++)
		{
			int entity = list[i];
			pool.Get(entity).PlayerID = (EPlayerID)i;
			AddCharacterTriggers(world, entity, isPvp: true);
		}
		int num = 2 - Math.Min(2, list.Count);
		for (int j = 0; j < num; j++)
		{
			int entity2 = CreateCharacter(world, (EPlayerID)(list.Count + j), new FVector2(((list.Count + j) % 2 == 0) ? (-2.4) : 2.4, FP._0_8));
			AddCharacterTriggers(world, entity2, isPvp: true);
		}
	}

	public static int CreateCharacter(EcsWorld world, EPlayerID playerId, FVector2 startPos)
	{
		int num = world.NewEntity();
		world.GetPool<ComponentUniqueID>().Add(num);
		world.GetPool<ComponentResource>().Add(num).Asset = ((playerId == EPlayerID.ID_1P) ? "Assets/Main/MiniGameRes/GGGo/Prefab/Character/SelfPlayer.prefab" : "Assets/Main/MiniGameRes/GGGo/Prefab/Character/OtherPlayer.prefab");
		world.GetPool<ComponentPosition>().Add(num).Position = startPos;
		world.GetPool<ComponentPlayer>().Add(num).PlayerID = playerId;
		ref ComponentVelocity reference = ref world.GetPool<ComponentVelocity>().Add(num);
		reference.Velocity = FVector2.Zero;
		reference.MaxSpeed = new FVector2(4, 50);
		ref ComponentAcceleration reference2 = ref world.GetPool<ComponentAcceleration>().Add(num);
		reference2.Value = new FVector2(FP.Zero, FP.Zero);
		reference2.MoveAcc = 180;
		world.GetPool<ComponentGravity>().Add(num).Value = -15;
		ref ComponentCollider reference3 = ref world.GetPool<ComponentCollider>().Add(num);
		reference3.Set(new FVector2(FP.Zero, -0.14), new FVector2(FP._0_3, FP._0_2));
		reference3.Layer = ColliderLayer.Player;
		world.GetPool<ComponentRigidBody>().Add(num).ContinuousDetection = true;
		ref ComponentData componentData = ref world.GetPool<ComponentData>().Add(num);
		componentData.SetPropertyValue(PropertyID.MaxHp, 8);
		componentData.SetPropertyValue(PropertyID.CurHp, 8);
		componentData.SetPropertyValue(PropertyID.HpRegen, FP._0_2);
		return num;
	}

	public static void AddCharacterTriggers(EcsWorld world, int entity, bool isPvp = false)
	{
		ref ComponentTriggers orAdd = ref world.GetPool<ComponentTriggers>().GetOrAdd(entity);
		orAdd.Triggers = new List<Trigger>();
		Trigger item = new Trigger
		{
			Events = new List<Type> { typeof(EventEntityRemove) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Target
				}
			},
			Actions = new List<IAction> { default(CheckGameOverAction) }
		};
		orAdd.Triggers.Add(item);
		item = ((!isPvp) ? new Trigger
		{
			Events = new List<Type> { typeof(EventFallOutCollection) },
			Actions = new List<IAction> { default(RemoveEntityAction) }
		} : new Trigger
		{
			Events = new List<Type> { typeof(EventFallOutCollection) },
			Actions = new List<IAction>
			{
				new CharacterHurtAction
				{
					HurtValue = FP.One / FP._0_5,
					Target = EventTarget.Entity
				},
				default(MoveToTopAction)
			}
		});
		orAdd.Triggers.Add(item);
		item = new Trigger
		{
			Events = new List<Type> { typeof(EventHitTopCollection) },
			Conditions = new List<ICondition>
			{
				new HasBuffCondition(BuffId.Invincible, invert: true, EventTarget.Target)
			},
			Actions = new List<IAction>
			{
				new CharacterHurtAction
				{
					HurtValue = FP.One,
					Target = EventTarget.Entity
				},
				new EventTriggerBroadAction
				{
					Type = EventTriggerType.PlatformSpike
				}
			}
		};
		orAdd.Triggers.Add(item);
		item = new Trigger
		{
			Events = new List<Type> { typeof(EventCharacterHurt) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Target
				}
			},
			Actions = new List<IAction>
			{
				new AddBuffAction(BuffId.Invincible, FP.One)
			}
		};
		orAdd.Triggers.Add(item);
		item = new Trigger
		{
			Events = new List<Type> { typeof(EventTryUseItem) },
			Conditions = new List<ICondition>
			{
				new EventWithMeCondition
				{
					EvenLaunchTargetType = EvenLaunchTargetType.Send
				}
			},
			Actions = new List<IAction> { default(UseItemAction) }
		};
		orAdd.Triggers.Add(item);
	}
}
