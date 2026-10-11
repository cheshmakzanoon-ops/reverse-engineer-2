public class LandlordConst
{
	public enum LandlordStage
	{
		NONE,
		PREVIEW,
		GROUP,
		PREPARE,
		BATTLE,
		REST,
		ENDED
	}

	public enum ZWLBuildingState
	{
		RUIN = 1,
		FIX,
		NORMAL,
		OVER
	}

	public enum LLBuildingState
	{
		None,
		NotOpen,
		OpenButShield,
		Fighting,
		WillExplode,
		Exploding,
		Ruins,
		Rebuilding
	}

	public enum LandLordGroup
	{
		NONE,
		LORD,
		FARMER
	}

	public const int LORD_EFFECT_ID = 90204;

	public const int FARMER_EFFECT_ID = 90201;

	public const int boomingTime = 10;

	public const int DEFAULT_PREPARE_TIME = 10;
}
