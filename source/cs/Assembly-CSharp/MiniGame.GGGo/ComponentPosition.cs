using Box2DSharp.Common;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public struct ComponentPosition
{
	[LabelText("位置")]
	public FVector2 Position;

	public FVector2 PrevPosition;
}
