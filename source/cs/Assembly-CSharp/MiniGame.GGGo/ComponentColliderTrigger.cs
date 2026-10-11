using Box2DSharp.Common;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public struct ComponentColliderTrigger
{
	[LabelText("触发器中心偏移")]
	public FVector2 Offset;

	[LabelText("触发器半尺寸")]
	public FVector2 HalfSize;

	[LabelText("触发器层级")]
	public ColliderLayer Layer;

	public void Set(FVector2 offset, FVector2 halfSize)
	{
		Offset = offset;
		HalfSize = halfSize;
	}
}
