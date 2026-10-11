using Box2DSharp.Common;
using MiniGame.Core;

namespace MiniGame.GGGo;

public static class ComponentDataUtil
{
	public static FP GetPropertyValue(this ref ComponentData componentData, PropertyID propertyID)
	{
		return componentData.GetPropertyValue((short)propertyID);
	}

	public static FP GetPropertyValue(this ref ComponentData componentData, PropertyID propertyID, FP dv)
	{
		return componentData.GetPropertyValue((short)propertyID, dv);
	}

	public static void SetPropertyValue(this ref ComponentData componentData, PropertyID propertyID, FP value)
	{
		componentData.SetPropertyValue((short)propertyID, value);
	}
}
