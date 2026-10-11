using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("移除触发器", "功能")]
public struct RemoveTriggerAction : IAction
{
	public Trigger[] Triggers;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (!(action is RemoveTriggerAction removeTriggerAction) || !world.GetPool<ComponentTriggers>().Has(entity))
		{
			return;
		}
		ref ComponentTriggers reference = ref world.GetPool<ComponentTriggers>().Get(entity);
		if (reference.Triggers == null)
		{
			return;
		}
		bool flag = false;
		Trigger[] triggers = removeTriggerAction.Triggers;
		foreach (Trigger item in triggers)
		{
			if (reference.Triggers.Contains(item))
			{
				reference.Triggers.Remove(item);
				flag = true;
			}
		}
		if (reference.Triggers.Count == 0)
		{
			world.GetPool<ComponentTriggers>().Del(entity);
		}
		else
		{
			if (!flag)
			{
				return;
			}
			EcsPool<ComponentActivedTriggers> pool = world.GetPool<ComponentActivedTriggers>();
			if (pool.Has(entity))
			{
				ComponentActivedTriggers componentActivedTriggers = pool.Get(entity);
				if (componentActivedTriggers.Triggers != null)
				{
					FuncEvent.UnSubscribe(ref FuncEvent.GetComponentEventManager(world), entity, componentActivedTriggers.Triggers);
				}
				pool.Del(entity);
			}
		}
	}
}
