using System;

namespace MiniGame.GGGo.Client;

public interface IRender : IDisposable
{
	UIRenderType Type { get; }

	void Recycle();
}
