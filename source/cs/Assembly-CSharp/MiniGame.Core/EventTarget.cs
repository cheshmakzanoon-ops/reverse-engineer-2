using MiniGame.OdinInspector;

namespace MiniGame.Core;

[LabelText("执行对象")]
public enum EventTarget
{
	[LabelText("实体自己")]
	Entity,
	[LabelText("消息发送者")]
	Sender,
	[LabelText("消息发送目标")]
	Target
}
