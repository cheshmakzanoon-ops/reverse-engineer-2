using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[TitleAndCategory("无敌持续时间", "功能")]
public struct HasBuffCondition : ICondition
{
	public EventTarget Target;

	public BuffId BuffId;

	[LabelText("取反")]
	public bool Invert;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public HasBuffCondition(BuffId buffId, bool invert = false, EventTarget target = EventTarget.Entity)
	{
		Target = target;
		BuffId = buffId;
		Invert = invert;
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (condition is HasBuffCondition hasBuffCondition)
		{
			entity = FuncAction.GetTarget(world, entity, e, hasBuffCondition.Target);
			bool flag = FuncBuff.Has(world, entity, (int)hasBuffCondition.BuffId);
			if (!hasBuffCondition.Invert)
			{
				return flag;
			}
			return !flag;
		}
		return false;
	}
}
