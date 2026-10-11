using System;
using Box2DSharp.Common;

namespace MiniGame.GGGo;

[Serializable]
public struct PlatformModify
{
	public int Index;

	public bool Deleted;

	public FVector2 Position;
}
