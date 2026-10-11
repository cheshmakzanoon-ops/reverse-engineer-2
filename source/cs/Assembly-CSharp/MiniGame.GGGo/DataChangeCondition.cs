using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[TitleAndCategory("数据改变", "数据")]
public struct DataChangeCondition : ICondition
{
	[LabelText("属性ID")]
	public short PropertyID;

	public ConditionOp ConditionOp;

	[LabelText("比较值")]
	public FP ComparableValue;

	[LabelText("仅自己变化时触发")]
	public bool IsMe;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		EventDataChange eventDataChange = ((e is EventDataChange) ? ((EventDataChange)(object)e) : default(EventDataChange));
		DataChangeCondition dataChangeCondition = ((condition is DataChangeCondition) ? ((DataChangeCondition)(object)condition) : default(DataChangeCondition));
		EcsPackedEntity ecsPackedEntity = world.PackEntity(entity);
		if (dataChangeCondition.IsMe && ecsPackedEntity != e.Sender)
		{
			return false;
		}
		e.Sender.Unpack(world, out var entity2);
		if (eventDataChange.PropertyID == dataChangeCondition.PropertyID || eventDataChange.PropertyID == 0)
		{
			EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
			if (pool.Has(entity2))
			{
				if (dataChangeCondition.ConditionOp == ConditionOp.None)
				{
					return true;
				}
				FP propertyValue = pool.Get(entity2).GetPropertyValue(dataChangeCondition.PropertyID);
				return FuncCondition.Comparable(dataChangeCondition.ConditionOp, propertyValue, dataChangeCondition.ComparableValue);
			}
		}
		return false;
	}
}
