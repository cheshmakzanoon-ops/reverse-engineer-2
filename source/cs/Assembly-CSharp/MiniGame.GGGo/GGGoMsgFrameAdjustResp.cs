using MiniGame.Core.Server;
using Newtonsoft.Json;

namespace MiniGame.GGGo;

public class GGGoMsgFrameAdjustResp : IMessageUnreliableSync
{
	[JsonProperty("d")]
	public int Frame;

	[JsonProperty("t")]
	public float Time;

	[JsonProperty("p")]
	public float[] Pings;
}
