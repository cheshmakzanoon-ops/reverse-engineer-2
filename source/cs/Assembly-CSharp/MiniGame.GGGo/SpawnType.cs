using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public enum SpawnType
{
	[LabelText("被删除")]
	Delete = -999,
	[LabelText("非平台")]
	ItemSpawner = -1,
	[LabelText("普通平台")]
	Normal = 0,
	[LabelText("针刺平台")]
	Spike = 1,
	[LabelText("消失平台")]
	Disappear = 2,
	[LabelText("弹跳平台")]
	Bounce = 3,
	[LabelText("滚动平台<--")]
	RollL = 4,
	[LabelText("滚动平台-->")]
	RollR = 5,
	[LabelText("Pve初始平台")]
	PveInit = 6,
	[LabelText("Pvp初始平台")]
	PvpInit = 7,
	[LabelText("终点平台")]
	Destination = 10
}
