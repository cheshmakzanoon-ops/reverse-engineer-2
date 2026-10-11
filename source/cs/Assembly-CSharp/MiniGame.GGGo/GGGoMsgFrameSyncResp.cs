using System.Collections.Generic;
using MiniGame.Core.Server;
using Newtonsoft.Json;

namespace MiniGame.GGGo;

public class GGGoMsgFrameSyncResp : IMessageSync
{
	public class CmdState : IMessageSync
	{
		[JsonProperty("e")]
		public int EntityID;

		[JsonProperty("f")]
		public int FrameIndex { get; set; }

		[JsonProperty("t")]
		public int CommandType { get; set; }

		[JsonProperty("s")]
		public int SequenceID { get; set; }
	}

	[JsonProperty("l")]
	public int LogicTickLockStep;

	[JsonProperty("c")]
	public List<CmdState> Commands;
}
