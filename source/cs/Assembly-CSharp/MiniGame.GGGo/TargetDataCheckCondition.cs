using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[TitleAndCategory("检测目标数据", "数据")]
public struct TargetDataCheckCondition : ICondition
{
	public int UniqueID;

	[LabelText("属性ID")]
	public short PropertyID;

	public ConditionOp ConditionOp;

	[LabelText("比较值")]
	public FP ComparableValue;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (condition is TargetDataCheckCondition targetDataCheckCondition)
		{
			if (targetDataCheckCondition.UniqueID >= 0 && !FuncUniqueID.TryGetEntityByUniqueID(world, targetDataCheckCondition.UniqueID, out entity))
			{
				return false;
			}
			FP fpData = MiniGame.Core.FuncData.GetFpData(world, entity, targetDataCheckCondition.PropertyID, 0);
			return FuncCondition.Comparable(targetDataCheckCondition.ConditionOp, fpData, targetDataCheckCondition.ComparableValue);
		}
		return false;
	}
}
