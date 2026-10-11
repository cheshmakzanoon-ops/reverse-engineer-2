using MiniGame.OdinInspector;

namespace MiniGame.Core;

[LabelText("数据操作")]
public enum DataOp
{
	[LabelText("增加")]
	Add = 1,
	[LabelText("减少")]
	Reduce,
	[LabelText("取反")]
	Negate
}
