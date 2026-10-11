using System;

namespace MiniGame.GGGo.Client;

public static class DataUISound
{
	public static Action<int, string> PlaySound;

	public static void PlayerSound(int soundID, string soundGroupName = "Effect")
	{
		PlaySound?.Invoke(soundID, soundGroupName);
		if (GameEntry.Sound != null)
		{
			GameEntry.Sound.PlaySoundById(soundID, soundGroupName);
		}
	}
}
