using Box2DSharp.Common;
using MiniGame.OdinInspector;

namespace MiniGame.Core;

public struct Property
{
	[LabelText("属性ID")]
	public int PropertyID;

	[LabelText("属性值")]
	public FP Value;
}
