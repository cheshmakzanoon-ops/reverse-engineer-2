using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[LabelText("触发类型")]
public enum EventTriggerType
{
	None,
	[LabelText("触发普通平台")]
	PlatformNormal,
	[LabelText("触发针刺平台")]
	PlatformSpike,
	[LabelText("触发弹簧")]
	PlatformBounce,
	[LabelText("平台碎裂")]
	PlatformBreak,
	[LabelText("触发滚动平台")]
	PlatformRoll
}
