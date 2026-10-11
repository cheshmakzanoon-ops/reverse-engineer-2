using Box2DSharp.Common;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public struct ComponentVelocity
{
	[LabelText("速度")]
	public FVector2 Velocity;

	[LabelText("最大速度")]
	public FVector2 MaxSpeed;
}
