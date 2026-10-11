using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[TitleAndCategory("检测自己的数据", "数据")]
public struct DataCheckCondition : ICondition
{
	[LabelText("属性ID")]
	public short PropertyID;

	[LabelText("比较值")]
	public FP ComparableValue;

	public ConditionOp ConditionOp;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (condition is DataCheckCondition dataCheckCondition)
		{
			FP fpData = MiniGame.Core.FuncData.GetFpData(world, entity, dataCheckCondition.PropertyID);
			return FuncCondition.Comparable((dataCheckCondition.ConditionOp == ConditionOp.None) ? ConditionOp.Equal : dataCheckCondition.ConditionOp, fpData, dataCheckCondition.ComparableValue);
		}
		return false;
	}
}
