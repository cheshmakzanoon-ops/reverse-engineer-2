using System;
using Joker;

namespace MiniGame.Biubiu.Client;

public static class DataUISound
{
	public static Action<int, string> PlaySound;

	public static void PlayerSound(int soundID, string soundGroupName = "Effect")
	{
		PlaySound?.Invoke(soundID, soundGroupName);
		Log.Debug($"[BiuBiu] PlaySound {soundID}");
		if (GameEntry.Sound != null)
		{
			GameEntry.Sound.PlaySoundById(soundID, soundGroupName);
		}
	}
}
