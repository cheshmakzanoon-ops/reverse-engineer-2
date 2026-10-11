using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.GGGo;

public struct ComponentBuffs : TEcsPoolDelegate<ComponentBuffs>, IEcsPoolDelegate, IEcsAutoReset<ComponentBuffs>, IEcsAutoCopy<ComponentBuffs>, IEcsAutoSnapshot<ComponentBuffs>
{
	public List<BuffRuntime> Buffs;

	public Type DelegateType => typeof(ComponentBuffs);

	public void AutoReset(ref ComponentBuffs c, EcsWorld world, int entity)
	{
		c.Buffs?.Clear();
	}

	public void AutoCopy(ref ComponentBuffs src, ref ComponentBuffs dst)
	{
		throw new NotSupportedException("ComponentBuffs不支持拷贝");
	}

	public object TakeSnapshot(ref ComponentBuffs c, EcsWorld world, int entity, object env)
	{
		ComponentBuffs c2 = default(ComponentBuffs);
		c2.RestoreSnapshot(ref c2, world, entity, c, env);
		return c2;
	}

	public void RestoreSnapshot(ref ComponentBuffs c, EcsWorld world, int entity, object data, object env)
	{
		ComponentBuffs componentBuffs = (ComponentBuffs)data;
		if (c.Buffs == null)
		{
			c.Buffs = new List<BuffRuntime>();
		}
		else
		{
			c.Buffs.Clear();
		}
		for (int i = 0; i < componentBuffs.Buffs.Count; i++)
		{
			BuffRuntime buffRuntime = componentBuffs.Buffs[i];
			c.Buffs.Add(buffRuntime.Clone());
		}
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		ComponentBuffs componentBuffs = (ComponentBuffs)a;
		ComponentBuffs componentBuffs2 = (ComponentBuffs)b;
		int num = componentBuffs.Buffs?.Count ?? 0;
		int num2 = componentBuffs2.Buffs?.Count ?? 0;
		if (num == num2 && num == 0)
		{
			return true;
		}
		if (num != num2)
		{
			return false;
		}
		for (int i = 0; i < componentBuffs.Buffs.Count; i++)
		{
			if (!componentBuffs.Buffs[i].Equals(componentBuffs2.Buffs[i]))
			{
				return false;
			}
		}
		return true;
	}
}
