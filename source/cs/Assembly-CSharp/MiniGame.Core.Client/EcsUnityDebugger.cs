using Leopotam.EcsLite;
using UnityEngine;

namespace MiniGame.Core.Client;

public class EcsUnityDebugger : IEcsDebugger
{
	public void LogDebug(string log)
	{
		Debug.Log(log);
	}

	public void LogInfo(string log)
	{
		Debug.Log(log);
	}

	public void LogError(string log)
	{
		Debug.LogError(log);
	}

	public void LogWarning(string log)
	{
		Debug.LogWarning(log);
	}
}
