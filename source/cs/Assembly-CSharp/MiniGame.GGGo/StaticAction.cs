using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("位置冻结", "效果")]
public struct StaticAction : IAction
{
	public EventTarget Target;

	public bool Value;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public StaticAction(bool value, EventTarget target = EventTarget.Entity)
	{
		Target = target;
		Value = value;
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is StaticAction staticAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, staticAction.Target);
			FuncComp.ChangeLayer<ComponentStatic>(world, entity, staticAction.Value);
		}
	}
}
