using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("禁止行动", "效果")]
public struct ControlDisableAction : IAction
{
	public EventTarget Target;

	public bool Value;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public ControlDisableAction(bool value, EventTarget target = EventTarget.Entity)
	{
		Target = target;
		Value = value;
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is ControlDisableAction controlDisableAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, controlDisableAction.Target);
			FuncComp.ChangeLayer<ComponentControlDisable>(world, entity, controlDisableAction.Value);
		}
	}
}
