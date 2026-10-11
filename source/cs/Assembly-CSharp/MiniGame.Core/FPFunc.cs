using Box2DSharp.Common;

namespace MiniGame.Core;

public static class FPFunc
{
	public static FP Lerp(FP a, FP b, FP t)
	{
		return a + (b - a) * t;
	}
}
