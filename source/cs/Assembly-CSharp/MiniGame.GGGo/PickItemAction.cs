using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("拾取道具", "功能")]
public struct PickItemAction : IAction
{
	public EventTarget Target;

	public ItemType ItemType;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (!(action is PickItemAction pickItemAction))
		{
			return;
		}
		entity = FuncAction.GetTarget(world, entity, e, pickItemAction.Target);
		if (!world.GetPool<ComponentItemHold>().Has(entity))
		{
			ref ComponentItemHold reference = ref world.GetPool<ComponentItemHold>().Add(entity);
			if (pickItemAction.ItemType == ItemType.Box)
			{
				FuncItem.UseBox(world, entity);
			}
			else
			{
				reference.ItemType = pickItemAction.ItemType;
			}
		}
	}
}
