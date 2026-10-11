using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public enum PropertyID
{
	None = 0,
	[LabelText("配置ID")]
	ConfigID = 1,
	[LabelText("是否死亡")]
	Die = 2,
	[LabelText("当前血量")]
	CurHp = 3,
	[LabelText("最大血量")]
	MaxHp = 4,
	[LabelText("回血速度")]
	HpRegen = 5,
	[LabelText("额外移速倍率")]
	MoveSpeedRate = 6,
	MAX_COMMON = 30
}
