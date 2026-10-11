using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public static class FuncUdpLatencySafe
{
	private static WaitForSeconds PingTimeOut = new WaitForSeconds(3f);

	private static Coroutine WaitCo;

	private static Action<string> Callback;

	public static void PingAll(int[] serverIds, Action<string> complete)
	{
		if (WaitCo != null)
		{
			YieldUtils.StopCoroutine(WaitCo);
			WaitCo = null;
		}
		Callback = complete;
		complete = (Action<string>)Delegate.Combine(complete, new Action<string>(DoPingFinish));
		WaitCo = YieldUtils.StartCoroutine(WaitToPingResult(FuncUdpLatency.PingAll(serverIds, complete)));
	}

	public static void GetGameLiftServerPingValues(int[] serverIds, Action<string> complete)
	{
		if (WaitCo != null)
		{
			YieldUtils.StopCoroutine(WaitCo);
			WaitCo = null;
		}
		Callback = complete;
		complete = (Action<string>)Delegate.Combine(complete, new Action<string>(DoPingFinish));
		ConcurrentDictionary<string, double> concurrentDictionary = new ConcurrentDictionary<string, double>();
		WaitCo = YieldUtils.StartCoroutine(WaitToPingResult(concurrentDictionary));
		FuncUdpLatency.GetGameLiftServerPingValues(serverIds, complete, concurrentDictionary);
	}

	private static void DoPingFinish(string pingInfo)
	{
		if (WaitCo != null)
		{
			YieldUtils.StopCoroutine(WaitCo);
			WaitCo = null;
		}
		Callback = null;
	}

	private static IEnumerator WaitToPingResult(List<UtilsPing.PingRequest> requests)
	{
		yield return PingTimeOut;
		string pingResults = FuncUdpLatency.GetPingResults(requests);
		Callback?.Invoke(pingResults);
		Callback = null;
	}

	private static IEnumerator WaitToPingResult(ConcurrentDictionary<string, double> pingResults)
	{
		yield return PingTimeOut;
		if (pingResults != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, double> pingResult in pingResults)
			{
				if (num != 0)
				{
					stringBuilder.Append(";");
				}
				stringBuilder.Append($"{pingResult.Key},{pingResult.Value:F2}");
				num++;
			}
			Callback?.Invoke(stringBuilder.ToString());
			Callback = null;
		}
		else
		{
			Callback?.Invoke("");
			Callback = null;
		}
	}
}
