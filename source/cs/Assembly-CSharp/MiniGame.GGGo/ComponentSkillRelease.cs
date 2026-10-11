using Box2DSharp.Common;

namespace MiniGame.GGGo;

public struct ComponentSkillRelease
{
	public ItemType SkillType;

	public int TargetEntity;

	public FP Duration;

	public FP ElapsedTime;
}
