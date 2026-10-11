using System;

namespace MiniGame.Core;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class TitleAndCategoryAttribute : Attribute
{
	public string Title { get; }

	public string Category { get; }

	public TitleAndCategoryAttribute(string title, string category)
	{
		Title = title;
		Category = category;
	}
}
