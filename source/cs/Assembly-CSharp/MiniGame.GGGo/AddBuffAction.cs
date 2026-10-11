using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("添加buff", "效果")]
public struct AddBuffAction : IAction
{
	public EventTarget Target;

	public BuffId BuffId;

	public FP Duration;

	public FP Param;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public AddBuffAction(BuffId buffId, FP duration, EventTarget target = EventTarget.Entity)
	{
		Target = target;
		BuffId = buffId;
		Duration = duration;
		Param = FP.Zero;
	}

	public AddBuffAction(BuffId buffId, FP duration, FP param, EventTarget target = EventTarget.Entity)
	{
		Target = target;
		BuffId = buffId;
		Duration = duration;
		Param = param;
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is AddBuffAction addBuffAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, addBuffAction.Target);
			FuncBuff.AddBuff(world, entity, addBuffAction.BuffId, addBuffAction.Duration, addBuffAction.Param);
		}
	}
}
