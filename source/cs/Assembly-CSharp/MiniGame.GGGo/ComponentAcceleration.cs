using Box2DSharp.Common;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public struct ComponentAcceleration
{
	[LabelText("输入加速度")]
	public FP MoveAcc;

	[LabelText("当前加速度")]
	public FVector2 Value;
}
