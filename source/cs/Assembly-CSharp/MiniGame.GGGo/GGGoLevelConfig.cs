using Box2DSharp.Common;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class GGGoLevelConfig : IGameLevelConfig
{
	public FVector2 GridSize = new FVector2(FP._0_2, FP._0_2);

	public FVector2 RangeHorizon = new FVector2(-3.1, 3.1);

	public FVector2 RangeVertical = new FVector2(-9, 4.5);

	public FP AoiLoadBuffer = 2;

	public FP AoiUnloadBuffer = 4;

	public FP SpeedTransTime = 5;

	public FP LowerBound = FP._0_3;

	public FP LowerBoundSpeedRate = 1.3;

	public int ItemGapLimit = 5;

	public int ItemGapLeast = 10;

	public FP ItemRebornDelay = 2;
}
