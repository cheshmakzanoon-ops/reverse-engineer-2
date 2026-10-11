namespace MiniGame.GGGo.Client;

public class SystemInputClientPredict : SystemInputClient
{
	protected override void DoCommand(int frame, int entity, int command)
	{
		if (_env.Value.Player == null)
		{
			_env.Value.Commands.QueueEvent(frame, entity, command, -1);
			return;
		}
		GGGoMsgFrameSyncResp.CmdState message = new GGGoMsgFrameSyncResp.CmdState
		{
			EntityID = entity,
			FrameIndex = frame,
			CommandType = command
		};
		_env.Value.Player.Send(message);
		_env.Value.Commands.QueueEvent(frame, entity, command, -1);
		_env.Value.PredictCommands.Add(new SyncCommand
		{
			FrameIndex = frame,
			EntityID = entity,
			CommandType = command
		});
	}
}
