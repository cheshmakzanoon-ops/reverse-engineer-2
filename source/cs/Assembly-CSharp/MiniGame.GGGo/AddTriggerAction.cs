using System.Collections.Generic;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("添加触发器", "功能")]
public struct AddTriggerAction : IAction
{
	public Trigger[] Triggers;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (!(action is AddTriggerAction addTriggerAction))
		{
			return;
		}
		ref ComponentTriggers orAdd = ref world.GetPool<ComponentTriggers>().GetOrAdd(entity);
		if (orAdd.Triggers == null)
		{
			orAdd.Triggers = new List<Trigger>();
		}
		Trigger[] triggers = addTriggerAction.Triggers;
		foreach (Trigger item in triggers)
		{
			if (!orAdd.Triggers.Contains(item))
			{
				orAdd.Triggers.Add(item);
			}
		}
		EcsPool<ComponentActivedTriggers> pool = world.GetPool<ComponentActivedTriggers>();
		if (pool.Has(entity))
		{
			pool.Del(entity);
		}
	}
}
