using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("关闭碰撞", "效果")]
public struct RigidBodyDisableAction : IAction
{
	public EventTarget Target;

	public bool Value;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public RigidBodyDisableAction(bool value, EventTarget target = EventTarget.Entity)
	{
		Target = target;
		Value = value;
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is RigidBodyDisableAction rigidBodyDisableAction)
		{
			entity = FuncAction.GetTarget(world, entity, e, rigidBodyDisableAction.Target);
			FuncComp.ChangeLayer<ComponentColliderDisable>(world, entity, rigidBodyDisableAction.Value);
		}
	}
}
