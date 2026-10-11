using System;
using UnityEngine;

public class LoopTimerSound : IDisposable
{
	private ITimer timer;

	private IDisposable _disposableImplementation;

	private int[] assetIdArray;

	private bool isBGM;

	private int playingSoundSerialId = -1;

	public int soundId { get; set; }

	public LoopTimerSound(int soundId, string assetName, int soundLength, float volumeScale = 1f, float soundVolumeSet = -1f)
	{
		this.soundId = soundId;
		timer = GameEntry.Timer.RegisterTimerRepeat(0.01f, soundLength, delegate
		{
			CallbackAction();
		});
		timer.isPause = true;
	}

	public LoopTimerSound(int soundId, int soundLength, float volumeScale = 1f, float soundVolumeSet = -1f, bool isBGM = false)
	{
		this.soundId = soundId;
		timer = GameEntry.Timer.RegisterTimerRepeat(0.01f, soundLength, delegate
		{
			CallbackAction();
		});
		timer.isPause = true;
	}

	public void Pause()
	{
		if (timer != null)
		{
			timer.isPause = true;
		}
	}

	public void Resume()
	{
		if (timer != null)
		{
			timer.isPause = false;
		}
	}

	public void Dispose()
	{
		soundId = 0;
		if (timer != null)
		{
			GameEntry.Timer.CancelTimer(timer);
			timer = null;
		}
		GameEntry.Sound.StopSound(playingSoundSerialId);
		soundId = 0;
		assetIdArray = null;
	}

	private void CallbackAction()
	{
		if (soundId > 0)
		{
			playingSoundSerialId = GameEntry.Sound.PlayEffectById(soundId);
		}
		else if (assetIdArray != null && assetIdArray.Length != 0)
		{
			int id = assetIdArray[UnityEngine.Random.Range(0, assetIdArray.Length - 1)];
			if (!isBGM)
			{
				playingSoundSerialId = GameEntry.Sound.PlayEffectById(id);
			}
		}
	}
}
