using Box2DSharp.Common;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public struct ComponentMoveDirection
{
	[LabelText("移动方向和速度")]
	public FVector2 Value;

	[LabelText("持续时间")]
	public FP EndTime;
}
