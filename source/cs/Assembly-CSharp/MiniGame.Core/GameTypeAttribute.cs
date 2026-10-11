using System;

namespace MiniGame.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class GameTypeAttribute : GameNamedAttribute
{
	public string Name;

	public GameTypeAttribute(string displayName, string name)
		: base(displayName)
	{
		Name = name;
	}
}
