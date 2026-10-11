using MiniGame.OdinInspector;

namespace MiniGame.Core;

[LabelText("条件比较符")]
public enum ConditionOp
{
	None,
	[LabelText("==")]
	Equal,
	[LabelText("!=")]
	NotEqual,
	[LabelText(">")]
	GreaterThan,
	[LabelText(">=")]
	GreaterThanOrEqual,
	[LabelText("<")]
	LessThan,
	[LabelText("<=")]
	LessThanOrEqual
}
