using System;

namespace MiniGame.Core;

public class GameNamedAttribute : Attribute
{
	public string DisplayName;

	public GameNamedAttribute(string name)
	{
		DisplayName = name;
	}
}
