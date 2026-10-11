using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemGameOver : IEcsRunSystem, IEcsSystem
{
	private EcsWorldInject world;

	public void Run(IEcsSystems systems)
	{
		GGGoEnv shared = world.Value.GetShared<GGGoEnv>();
		if (!shared.CanCheckGameOver)
		{
			return;
		}
		if (shared.GameType == EGameType.PvpClient)
		{
			shared.ClientGameOver = true;
			return;
		}
		GGGoGameResult gameResult = shared.GameResult;
		EPlayerID ePlayerID = EPlayerID.ID_None;
		foreach (int item in world.Value.Filter<ComponentPlayer>().Inc<ComponentData>().End())
		{
			ref ComponentPlayer reference = ref world.Value.GetPool<ComponentPlayer>().Get(item);
			if (FuncData.GetBoolData(world.Value, item, PropertyID.Die))
			{
				ePlayerID = ((reference.PlayerID == EPlayerID.ID_1P) ? EPlayerID.ID_2P : EPlayerID.ID_1P);
				gameResult.Statistics.IsDead = true;
				break;
			}
		}
		if (ePlayerID == EPlayerID.ID_None)
		{
			FP fP = FP.MaxValue;
			foreach (int item2 in world.Value.Filter<ComponentPlayer>().Inc<ComponentPosition>().End())
			{
				EcsPool<ComponentPlayer> pool = world.Value.GetPool<ComponentPlayer>();
				EcsPool<ComponentPosition> pool2 = world.Value.GetPool<ComponentPosition>();
				ref ComponentPlayer reference2 = ref pool.Get(item2);
				ref ComponentPosition reference3 = ref pool2.Get(item2);
				if (reference3.Position.Y < fP)
				{
					fP = reference3.Position.Y;
					ePlayerID = reference2.PlayerID;
				}
			}
		}
		shared.GameOver = true;
		shared.GameState = EGameWorldState.Settlement;
		gameResult.Statistics.WinPlayerId = ePlayerID;
		gameResult.Statistics.GameEndFrame = shared.LogicTickCount;
		GGGoVerify validationResult = GGGoShare.GetValidationResult(world.Value);
		IGameSerializer serializer = shared.ResourceLoader.GetSerializer();
		gameResult.VerifyJson = serializer.ToJson(validationResult);
		string mD = serializer.GetMD5(gameResult.VerifyJson);
		List<SyncCommand> commands = shared.Commands.Commands;
		List<SyncCommand> list = new List<SyncCommand>();
		for (int i = 0; i < commands.Count; i++)
		{
			list.Add(commands[i].Clone());
		}
		gameResult.Replay = new GGGoReplay
		{
			LevelPath = shared.InitData.LevelPath,
			MD5 = mD,
			Commands = list
		};
	}
}
