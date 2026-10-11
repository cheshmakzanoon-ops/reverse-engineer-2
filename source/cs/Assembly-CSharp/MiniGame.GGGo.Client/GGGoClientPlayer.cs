using MiniGame.Core.Client;
using MiniGame.Core.Server;

namespace MiniGame.GGGo.Client;

public class GGGoClientPlayer : GameClientPlayer
{
	protected override void OnGameEnd(S2CGameRoomEnd end)
	{
		GGGoMsgEndResp gGGoMsgEndResp = end.Data as GGGoMsgEndResp;
		GGGoEnv gGGoEnv = base.Game?.Env as GGGoEnv;
		if (gGGoMsgEndResp != null && gGGoEnv != null)
		{
			gGGoEnv.ServerGameOver = true;
			gGGoEnv.GameResult.Statistics = gGGoMsgEndResp.Statistics;
		}
	}

	protected override void PackMessage(object message, out int opCode, out object data)
	{
		GGGoMessage.PackMessage(message, out opCode, out data);
	}

	protected override bool UnpackMessage(int opCode, object data, out object result)
	{
		return GGGoMessage.UnpackMessage(opCode, data, out result);
	}
}
