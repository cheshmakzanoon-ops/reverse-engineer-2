using System;
using Box2DSharp.Common;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

[Serializable]
public struct PlatformInfo
{
	[LabelText("平台类型")]
	public SpawnType Type;

	[LabelText("平台权重")]
	public FP Weight;
}
