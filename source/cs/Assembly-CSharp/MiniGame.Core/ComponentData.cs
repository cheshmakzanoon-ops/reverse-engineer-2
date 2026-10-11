using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public struct ComponentData : TEcsPoolDelegate<ComponentData>, IEcsPoolDelegate, IEcsAutoReset<ComponentData>, IEcsAutoCopy<ComponentData>, IEcsAutoSnapshot<ComponentData>
{
	public FP[] Values;

	public Type DelegateType => typeof(ComponentData);

	public void AutoReset(ref ComponentData c, EcsWorld world, int entity)
	{
		c.Values = null;
	}

	public void AutoCopy(ref ComponentData src, ref ComponentData dst)
	{
		throw new NotSupportedException("ComponentData不支持拷贝");
	}

	public object TakeSnapshot(ref ComponentData c, EcsWorld world, int entity, object env)
	{
		ComponentDataSnapshotData componentDataSnapshotData = new ComponentDataSnapshotData();
		List<Property> list = new List<Property>();
		int num = 0;
		if (c.Values != null)
		{
			for (int i = 0; i < c.Values.Length; i++)
			{
				if (c.Values[i] != FP.Zero)
				{
					list.Add(new Property
					{
						PropertyID = (short)i,
						Value = c.Values[i]
					});
					num = Math.Max(num, i);
				}
			}
		}
		componentDataSnapshotData.MaxPropertyID = num;
		componentDataSnapshotData.KeyValues = list.ToArray();
		return componentDataSnapshotData;
	}

	public void RestoreSnapshot(ref ComponentData c, EcsWorld world, int entity, object data, object env)
	{
		ComponentDataSnapshotData componentDataSnapshotData = (ComponentDataSnapshotData)data;
		c.Values = new FP[componentDataSnapshotData.MaxPropertyID + 1];
		Property[] keyValues = componentDataSnapshotData.KeyValues;
		for (int i = 0; i < keyValues.Length; i++)
		{
			Property property = keyValues[i];
			c.Values[property.PropertyID] = property.Value;
		}
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		ComponentDataSnapshotData componentDataSnapshotData = (ComponentDataSnapshotData)a;
		ComponentDataSnapshotData componentDataSnapshotData2 = (ComponentDataSnapshotData)b;
		if (componentDataSnapshotData.KeyValues.Length != componentDataSnapshotData2.KeyValues.Length)
		{
			return false;
		}
		for (int i = 0; i < componentDataSnapshotData.KeyValues.Length; i++)
		{
			if (componentDataSnapshotData.KeyValues[i].PropertyID != componentDataSnapshotData2.KeyValues[i].PropertyID)
			{
				return false;
			}
			if (componentDataSnapshotData.KeyValues[i].Value != componentDataSnapshotData2.KeyValues[i].Value)
			{
				return false;
			}
		}
		return true;
	}
}
