using System;
using Box2DSharp.Common;

namespace MiniGame.Core;

public static class ComponentDataUtil
{
	public static void SetPropertyValue(ref FP[] list, short propertyID, FP value)
	{
		if (list == null)
		{
			list = new FP[(propertyID + 3) * 4 / 3];
		}
		if (propertyID >= list.Length)
		{
			Array.Resize(ref list, (propertyID + 3) * 4 / 3);
		}
		list[propertyID] = value;
	}

	public static void SetPropertyValue(this ref ComponentData componentData, short propertyID, FP value)
	{
		SetPropertyValue(ref componentData.Values, propertyID, value);
	}

	public static FP GetPropertyValue(this ref ComponentData componentData, short propertyID)
	{
		if (componentData.Values.Length <= propertyID)
		{
			return FP.Zero;
		}
		return componentData.Values[propertyID];
	}

	public static FP GetPropertyValue(this ref ComponentData componentData, short propertyID, FP dv)
	{
		if (componentData.Values.Length > propertyID)
		{
			return componentData.Values[propertyID];
		}
		return dv;
	}

	public static bool TryGetPropertyValue(this ref ComponentData componentData, short propertyID, out FP value)
	{
		if (componentData.Values.Length > propertyID)
		{
			value = componentData.Values[propertyID];
			return true;
		}
		value = default(FP);
		return false;
	}
}
