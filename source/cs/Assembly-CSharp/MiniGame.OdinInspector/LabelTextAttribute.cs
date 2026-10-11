using System;

namespace MiniGame.OdinInspector;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class LabelTextAttribute : Attribute
{
	public LabelTextAttribute(string text)
	{
	}
}
