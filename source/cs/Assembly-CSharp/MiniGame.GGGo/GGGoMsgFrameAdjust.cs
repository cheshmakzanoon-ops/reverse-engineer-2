using MiniGame.Core.Server;
using Newtonsoft.Json;

namespace MiniGame.GGGo;

public class GGGoMsgFrameAdjust : IMessageUnreliableSync
{
	[JsonProperty("d")]
	public int Frame;

	[JsonProperty("t")]
	public float Time;

	[JsonProperty("r")]
	public float Rtt;
}
