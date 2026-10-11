using Box2DSharp.Common;

namespace MiniGame.GGGo;

public static class ComponentCollisionExtensions
{
	public static bool RectOverlaps(FVector2 centerA, FVector2 halfA, FVector2 centerB, FVector2 halfB)
	{
		if (FP.Abs(centerA.X - centerB.X) > halfA.X + halfB.X)
		{
			return false;
		}
		if (FP.Abs(centerA.Y - centerB.Y) > halfA.Y + halfB.Y)
		{
			return false;
		}
		return true;
	}

	public static bool RectOverlaps(this ComponentCollider a, ComponentCollider b, ComponentPosition posA, ComponentPosition posB)
	{
		return RectOverlaps(posA.Position + a.Offset, a.HalfSize, posB.Position + b.Offset, b.HalfSize);
	}
}
