using Box2DSharp.Common;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public struct ComponentCollider
{
	[LabelText("碰撞体中心偏移")]
	public FVector2 Offset;

	[LabelText("碰撞体半尺寸")]
	public FVector2 HalfSize;

	[LabelText("碰撞体层级")]
	public ColliderLayer Layer;

	public void Set(FVector2 offset, FVector2 halfSize)
	{
		Offset = offset;
		HalfSize = halfSize;
	}
}
