using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[TitleAndCategory("角色扣血", "功能")]
public struct CharacterHurtAction : IAction
{
	public EventTarget Target;

	[LabelText("扣血数值")]
	public FP HurtValue;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (!(action is CharacterHurtAction characterHurtAction))
		{
			return;
		}
		int target = FuncAction.GetTarget(world, entity, e, characterHurtAction.Target);
		EcsPackedEntity ecsPackedEntity = world.PackEntity(target);
		if (!world.GetPool<ComponentData>().Has(target))
		{
			return;
		}
		if (characterHurtAction.HurtValue > FP.Zero)
		{
			if (FuncBuff.Has(world, target, 7) && FuncBuff.Remove(world, target, 7))
			{
				return;
			}
			FuncEvent.Broadcast(world, new EventCharacterHurt(ecsPackedEntity, ecsPackedEntity, characterHurtAction.HurtValue));
		}
		else if (characterHurtAction.HurtValue < FP.Zero)
		{
			FuncEvent.Broadcast(world, new EventCharacterHeal(ecsPackedEntity, ecsPackedEntity, -characterHurtAction.HurtValue));
		}
		e.Target.Unpack(world, out var _);
		FuncData.ChangeData_HP(world, target, -characterHurtAction.HurtValue);
	}
}
