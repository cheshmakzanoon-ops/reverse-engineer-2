using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class FuncData
{
	public static void SetData(EcsWorld world, int entity, PropertyID propertyID, FP value, bool notify = true)
	{
		MiniGame.Core.FuncData.SetData(world, entity, (short)propertyID, value, notify);
	}

	public static FP GetFpData(EcsWorld world, int entity, PropertyID propertyID)
	{
		return MiniGame.Core.FuncData.GetFpData(world, entity, (short)propertyID);
	}

	public static int GetIntData(EcsWorld world, int entity, PropertyID propertyID, int defaultValue = 0)
	{
		return MiniGame.Core.FuncData.GetIntData(world, entity, (short)propertyID, defaultValue);
	}

	public static float GetFloatData(EcsWorld world, int entity, PropertyID propertyID)
	{
		return MiniGame.Core.FuncData.GetFpData(world, entity, (short)propertyID).AsFloat;
	}

	public static bool GetBoolData(EcsWorld world, int entity, PropertyID propertyID)
	{
		return MiniGame.Core.FuncData.GetFpData(world, entity, (short)propertyID) > FP.Zero;
	}

	public static void ChangeData_HP(EcsWorld world, int entity, FP value, bool notify = true)
	{
		FP fP = GetFpData(world, entity, PropertyID.CurHp) + value;
		if (fP < FP.Zero)
		{
			fP = FP.Zero;
		}
		SetData(world, entity, PropertyID.CurHp, fP, notify);
		if (fP <= FP.Zero)
		{
			FuncEntity.DelEntity(world, entity);
		}
	}

	public static void SetData_Die(EcsWorld world, int entity, int sender, bool isDie, bool notify = true)
	{
		SetData(world, entity, PropertyID.Die, isDie ? FP.One : FP.Zero, notify);
	}
}
