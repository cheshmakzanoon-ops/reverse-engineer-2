using Box2DSharp.Common;

namespace MiniGame.GGGo;

public struct ComponentRegion
{
	public FP StartPos;

	public int CountVertical;

	public FP StepVertical;

	public FP GapHorizontal;

	public FP Length;

	public FP Speed;

	public uint RandomSeed;

	public PlatformInfo[] Platforms;

	public PlatformModify[] Modifies;
}
