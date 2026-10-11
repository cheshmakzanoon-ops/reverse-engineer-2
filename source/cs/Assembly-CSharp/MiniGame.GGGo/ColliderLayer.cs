using System;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[Flags]
[LabelText("碰撞层")]
public enum ColliderLayer
{
	None = 0,
	[LabelText("玩家")]
	Player = 1,
	[LabelText("敌人")]
	Enemy = 2,
	[LabelText("平台")]
	Platform = 4,
	[LabelText("终点")]
	Destination = 8,
	[LabelText("道具")]
	Item = 0x10
}
