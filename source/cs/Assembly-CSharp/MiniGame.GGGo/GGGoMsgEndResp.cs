using MiniGame.Core.Server;

namespace MiniGame.GGGo;

public class GGGoMsgEndResp : IMessageEnd
{
	public int Code;

	public SharedGameStatistics Statistics;
}
