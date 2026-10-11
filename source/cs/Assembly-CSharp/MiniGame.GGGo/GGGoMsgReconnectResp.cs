using System.Collections.Generic;
using MiniGame.Core.Server;
using Newtonsoft.Json;

namespace MiniGame.GGGo;

public class GGGoMsgReconnectResp : IMessageSync
{
	[JsonProperty("l")]
	public int LogicTickLockStep;

	[JsonProperty("c")]
	public List<GGGoMsgFrameSyncResp.CmdState> Commands;
}
