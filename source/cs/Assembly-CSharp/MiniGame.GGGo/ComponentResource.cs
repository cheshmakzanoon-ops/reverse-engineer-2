using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public struct ComponentResource
{
	[LabelText("资源路径")]
	public string Asset;

	[LabelText("资源类型")]
	public ResourceType Type;
}
