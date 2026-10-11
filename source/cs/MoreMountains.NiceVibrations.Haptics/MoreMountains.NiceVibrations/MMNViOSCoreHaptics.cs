using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.NiceVibrations;

public static class MMNViOSCoreHaptics
{
	private static float _initialContinuousIntensity;

	private static float _initialContinuousSharpness;

	public static event Action OnHapticPatternStopped;

	public static event Action OnHapticPatternError;

	public static event Action OnHapticPatternReset;

	private static bool MMNViOS_CoreHapticsSupported()
	{
		return false;
	}

	private static void MMNViOS_CoreHapticsSetDebugMode(bool status)
	{
	}

	private static void MMNViOS_CreateEngine()
	{
	}

	private static void MMNViOS_StopEngine()
	{
	}

	private static void MMNViOS_PlayTransientHapticPattern(float intensity, float sharpness, bool threaded)
	{
	}

	private static void MMNViOS_PlayContinuousHapticPattern(float intensity, float sharpness, float duration, bool threaded, bool fullIntensity)
	{
	}

	private static void MMNViOS_PlayContinuousHapticPattern_V2(float intensity, float sharpness, float duration, bool threaded, bool fullIntensity)
	{
	}

	private static void MMNViOS_UpdateContinuousHapticPattern(float intensity, float sharpness, bool threaded)
	{
	}

	private static void MMNViOS_StopContinuousHaptic()
	{
	}

	private static void MMNViOS_PlayCoreHapticsFromJSON(string jsonString, bool threaded)
	{
	}

	private static void MMNViOS_CoreHapticsRegisterHapticEngineFinishedCallback(Action callback)
	{
	}

	private static void MMNViOS_CoreHapticsRegisterHapticEngineErrorCallback(Action callback)
	{
	}

	private static void MMNViOS_CoreHapticsRegisterHapticEngineResetCallback(Action callback)
	{
	}

	static MMNViOSCoreHaptics()
	{
		MMNViOS_CoreHapticsRegisterHapticEngineFinishedCallback(HapticStoppedCallback);
		MMNViOS_CoreHapticsRegisterHapticEngineErrorCallback(HapticsErrorCallback);
		MMNViOS_CoreHapticsRegisterHapticEngineResetCallback(HapticsResetCallback);
	}

	public static void PlayCoreHapticsFromJSON(string jsonString, bool threaded = false)
	{
		if (jsonString != null && !(jsonString == ""))
		{
			MMNViOS_PlayCoreHapticsFromJSON(jsonString, threaded);
		}
	}

	public static void PlayTransientHapticPattern(float intensity, float sharpness, bool threaded = false)
	{
		MMNViOS_PlayTransientHapticPattern(intensity, sharpness, threaded);
	}

	public static void PlayContinuousHapticPattern(float intensity, float sharpness, float duration, MonoBehaviour coroutineMonobehaviour = null, bool threaded = false, bool fullIntensity = true)
	{
		if (intensity < 0.01f)
		{
			intensity = 0.01f;
		}
		_initialContinuousIntensity = intensity;
		_initialContinuousSharpness = sharpness;
		if (BaseVersionCompare(Application.version, "1.0.337") < 0)
		{
			MMNViOS_PlayContinuousHapticPattern(intensity, sharpness, duration, threaded, fullIntensity);
		}
		else
		{
			MMNViOS_PlayContinuousHapticPattern_V2(intensity, sharpness, duration, threaded, fullIntensity);
		}
	}

	public static IEnumerator ContinuousHapticPatternCoroutine(float intensity, float sharpness, bool threaded = false)
	{
		yield return null;
		MMNViOS_UpdateContinuousHapticPattern(intensity, sharpness, threaded);
	}

	public static void UpdateContinuousHapticPattern(float intensity, float sharpness, bool threaded = false)
	{
		MMNViOS_UpdateContinuousHapticPattern(intensity, sharpness, threaded);
	}

	public static void UpdateContinuousHapticPatternRational(float intensity, float sharpness, bool threaded = false)
	{
		if (_initialContinuousIntensity < 0.01f)
		{
			_initialContinuousIntensity = 0.01f;
		}
		float intensity2 = intensity / _initialContinuousIntensity;
		float sharpness2 = sharpness - _initialContinuousSharpness;
		MMNViOS_UpdateContinuousHapticPattern(intensity2, sharpness2, threaded);
	}

	public static void StopHapticPatterns()
	{
		MMNViOS_StopContinuousHaptic();
	}

	public static void CreateEngine()
	{
		MMNViOS_CreateEngine();
	}

	public static void StopEngine()
	{
		MMNViOS_StopEngine();
	}

	public static void SetDebugMode(bool newStatus)
	{
		MMNViOS_CoreHapticsSetDebugMode(newStatus);
	}

	public static bool CoreHapticsSupported()
	{
		return MMNViOS_CoreHapticsSupported();
	}

	private static void HapticStoppedCallback()
	{
		MMNViOSCoreHaptics.OnHapticPatternStopped?.Invoke();
	}

	private static void HapticsErrorCallback()
	{
		MMNViOSCoreHaptics.OnHapticPatternError?.Invoke();
	}

	private static void HapticsResetCallback()
	{
		MMNViOSCoreHaptics.OnHapticPatternReset?.Invoke();
	}

	public static int BaseVersionCompare(string a, string b)
	{
		if (string.IsNullOrEmpty(a))
		{
			if (string.IsNullOrEmpty(b))
			{
				return 0;
			}
			return -1;
		}
		if (string.IsNullOrEmpty(b))
		{
			return 1;
		}
		string[] array = a.Split(new char[1] { '.' });
		string[] array2 = b.Split(new char[1] { '.' });
		int num = array.Length;
		int num2 = array2.Length;
		int num3 = Math.Max(num, num2);
		for (int i = 0; i < num3; i++)
		{
			string text = ((i < num) ? array[i] : null);
			string text2 = ((i < num2) ? array2[i] : null);
			int num4 = 0;
			int num5 = 0;
			if (!string.IsNullOrEmpty(text))
			{
				num4 = int.Parse(text);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				num5 = int.Parse(text2);
			}
			if (num4 > num5)
			{
				return 1;
			}
			if (num4 < num5)
			{
				return -1;
			}
		}
		return 0;
	}
}
