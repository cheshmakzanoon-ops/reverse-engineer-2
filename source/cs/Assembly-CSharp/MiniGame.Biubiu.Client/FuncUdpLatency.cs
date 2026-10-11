using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public static class FuncUdpLatency
{
	private class GameLiftServerInfo
	{
		public string Domain;

		public int Port;
	}

	public static List<UtilsPing.PingRequest> PingAll(int[] serverIds, Action<string> complete)
	{
		UtilsPing.SetPostMainThreadDelegate(MainThreadDispatcher.Instance.Enqueue);
		List<UtilsPing.PingRequest> requests = new List<UtilsPing.PingRequest>();
		foreach (int id in serverIds)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("season_bullet_server", id, "Domain");
			string templateData2 = GameEntry.ConfigCache.GetTemplateData("season_bullet_server", id, "Port");
			string templateData3 = GameEntry.ConfigCache.GetTemplateData("season_bullet_server", id, "HttpUrl");
			string templateData4 = GameEntry.ConfigCache.GetTemplateData("season_bullet_server", id, "LocationName");
			int.TryParse(templateData2, out var result);
			requests.Add(new UtilsPing.PingRequest
			{
				Name = templateData4,
				UdpUrl = $"{templateData}:{result}",
				HttpUrl = templateData3
			});
		}
		UtilsPing.PingAll(requests, delegate
		{
			string pingResults = GetPingResults(requests);
			complete?.Invoke(pingResults.ToString());
		}, 2000);
		return requests;
	}

	public static string GetPingResults(List<UtilsPing.PingRequest> requests)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < requests.Count; i++)
		{
			UtilsPing.PingResult result = requests[i].Result;
			if (i == 0)
			{
				stringBuilder.Append($"{result.Request.Name},{result.RttMs:F2}");
			}
			else
			{
				stringBuilder.Append($";{result.Request.Name},{result.RttMs:F2}");
			}
			UnityEngine.Debug.Log($"{result.RttMs} - http:{result.Request.HttpUrl} udp:{result.Request.UdpUrl} {result.Url} {result.Error}");
		}
		return stringBuilder.ToString();
	}

	public static void GetGameLiftServerPingValues(int[] serverIds, Action<string> complete, ConcurrentDictionary<string, double> pingResult = null)
	{
		Dictionary<string, GameLiftServerInfo> dictionary = new Dictionary<string, GameLiftServerInfo>();
		foreach (int id in serverIds)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("season_bullet_server", id, "Domain");
			string templateData2 = GameEntry.ConfigCache.GetTemplateData("season_bullet_server", id, "Port");
			string templateData3 = GameEntry.ConfigCache.GetTemplateData("season_bullet_server", id, "LocationName");
			int.TryParse(templateData2, out var result);
			if (!dictionary.ContainsKey(templateData3))
			{
				dictionary.Add(templateData3, new GameLiftServerInfo
				{
					Domain = templateData,
					Port = result
				});
			}
		}
		GameLiftServerPingValuesImplAsync(dictionary, complete, pingResult);
	}

	private static async void GameLiftServerPingValuesImplAsync(Dictionary<string, GameLiftServerInfo> serverInfos, Action<string> complete, ConcurrentDictionary<string, double> pingResult = null)
	{
		(string, double, int)[] obj = await System.Threading.Tasks.Task.WhenAll(serverInfos.Select(async delegate(KeyValuePair<string, GameLiftServerInfo> serverEach, int index)
		{
			double item = await MeasureLatencyAsync(serverEach.Value.Domain, serverEach.Value.Port, "Ping", 1, 1000, pingResult, serverEach.Key);
			return (Key: serverEach.Key, Ping: item, Index: index);
		}).ToList()).ConfigureAwait(continueOnCapturedContext: false);
		StringBuilder pingValues = new StringBuilder();
		(string, double, int)[] array = obj;
		for (int num = 0; num < array.Length; num++)
		{
			(string, double, int) tuple = array[num];
			if (tuple.Item2 > 0.0)
			{
				if (tuple.Item3 == 0)
				{
					pingValues.Append($"{tuple.Item1},{tuple.Item2:F2}");
				}
				else
				{
					pingValues.Append($";{tuple.Item1},{tuple.Item2:F2}");
				}
			}
		}
		MainThreadDispatcher.Instance.Enqueue(delegate
		{
			complete?.Invoke(pingValues.ToString());
		});
	}

	public static async Task<double> MeasureLatencyAsync(string domain, int port, string message, int numPings, int timeoutMs, ConcurrentDictionary<string, double> pingResult = null, string region = null)
	{
		double ping = 9999.0;
		using (UdpClient udpClient = new UdpClient())
		{
			_ = 2;
			try
			{
				IPAddress[] array = await Dns.GetHostAddressesAsync(domain);
				if (array.Length == 0)
				{
					return ping;
				}
				IPEndPoint endPoint = new IPEndPoint(array[0], port);
				byte[] messageBytes = Encoding.UTF8.GetBytes(message);
				udpClient.Client.ReceiveTimeout = timeoutMs;
				double totalLatency = 0.0;
				int successfulPings = 0;
				Stopwatch stopwatch = new Stopwatch();
				for (int i = 0; i < numPings; i++)
				{
					try
					{
						stopwatch.Restart();
						await udpClient.SendAsync(messageBytes, messageBytes.Length, endPoint);
						await ReceiveWithTimeoutAsync(udpClient, timeoutMs);
						stopwatch.Stop();
						double totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
						totalLatency += totalMilliseconds;
						successfulPings++;
					}
					catch (SocketException)
					{
					}
					catch (TimeoutException)
					{
					}
				}
				if (successfulPings > 0)
				{
					ping = totalLatency / (double)successfulPings;
				}
			}
			catch (Exception)
			{
			}
		}
		pingResult?.TryAdd(region, ping);
		return ping;
	}

	private static async Task<UdpReceiveResult> ReceiveWithTimeoutAsync(UdpClient client, int timeoutMs)
	{
		using CancellationTokenSource cts = new CancellationTokenSource(timeoutMs);
		try
		{
			return await client.ReceiveAsync().WaitAsync(TimeSpan.FromMilliseconds(timeoutMs), cts.Token);
		}
		catch (OperationCanceledException)
		{
			throw new TimeoutException("Receive operation timed out");
		}
	}

	private static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
	{
		using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		cts.CancelAfter(timeout);
		System.Threading.Tasks.Task delayTask = System.Threading.Tasks.Task.Delay(-1, cts.Token);
		if (await System.Threading.Tasks.Task.WhenAny(task, delayTask).ConfigureAwait(continueOnCapturedContext: false) == delayTask)
		{
			throw new TimeoutException("The operation has timed out.");
		}
		cts.Cancel();
		return await task.ConfigureAwait(continueOnCapturedContext: false);
	}
}
