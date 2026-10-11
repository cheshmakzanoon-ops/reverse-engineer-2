using MiniGame.OdinInspector;

namespace MiniGame.Core;

[LabelText("事件触发目标类型")]
public enum EvenLaunchTargetType
{
	[LabelText("消息发送者")]
	Send = 1,
	[LabelText("消息接收者")]
	Target,
	[LabelText("发送或接收者")]
	Other
}
