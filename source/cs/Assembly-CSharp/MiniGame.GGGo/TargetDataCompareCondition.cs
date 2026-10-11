using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[TitleAndCategory("对比目标数据", "数据")]
public struct TargetDataCompareCondition : ICondition
{
	[LabelText("左唯一ID")]
	public int LUniqueID;

	[LabelText("右唯一ID")]
	public int RUniqueID;

	[LabelText("左属性ID")]
	public short LPropertyID;

	[LabelText("右属性ID")]
	public short RPropertyID;

	public ConditionOp ConditionOp;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (condition is TargetDataCompareCondition targetDataCompareCondition)
		{
			int entity2 = entity;
			int entity3 = entity;
			if (targetDataCompareCondition.LUniqueID >= 0 && !FuncUniqueID.TryGetEntityByUniqueID(world, targetDataCompareCondition.LUniqueID, out entity2))
			{
				return false;
			}
			if (targetDataCompareCondition.RUniqueID >= 0 && !FuncUniqueID.TryGetEntityByUniqueID(world, targetDataCompareCondition.RUniqueID, out entity3))
			{
				return false;
			}
			FP fpData = MiniGame.Core.FuncData.GetFpData(world, entity2, targetDataCompareCondition.LPropertyID, 0);
			FP fpData2 = MiniGame.Core.FuncData.GetFpData(world, entity3, targetDataCompareCondition.RPropertyID, 0);
			return FuncCondition.Comparable(targetDataCompareCondition.ConditionOp, fpData, fpData2);
		}
		return false;
	}
}
