using System;

namespace GPUDamageText;

[Serializable]
public class AnimationData
{
	public string Name;

	public AnimatedPropertyData[] Properties;

	public float Length;

	public float Evaluate(int propertyIndex, float time)
	{
		if (Properties == null || Properties.Length <= propertyIndex)
		{
			return 0f;
		}
		if (Properties[propertyIndex].Curve == null)
		{
			return 0f;
		}
		return Properties[propertyIndex].Curve.Evaluate(time);
	}
}
