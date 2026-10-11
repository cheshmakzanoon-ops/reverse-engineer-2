using System;
using System.Threading.Tasks;
using MiniGame.Biubiu.Client;
using MiniGame.Core.Client;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class ImageUIPing : MonoBehaviour
{
	private string Domain;

	private int Port;

	private GameUnityRuntime Runtime;

	private int _playerIndex = -1;

	private GGGoEnvClient _envClient;

	private bool _isSelf;

	private float PingTime;

	private float PingInterval = 2f;

	private bool _isPinging;

	private float _rttUpdateTimer;

	private const float RttUpdateInterval = 0.5f;

	[SerializeField]
	private GameObject[] SignalObjects;

	[SerializeField]
	private int MaxPingMs = 400;

	private void Start()
	{
		SetSignalIndex(-1);
	}

	private void Update()
	{
		if (Runtime != null)
		{
			_rttUpdateTimer -= Time.deltaTime;
			if (_rttUpdateTimer <= 0f)
			{
				_rttUpdateTimer = 0.5f;
				int num = ResolvePlayerIndex();
				float num2 = ((num >= 0) ? Runtime.GetPlayerRttMs(num) : Runtime.RttMs);
				if (num2 <= 0f)
				{
					SetSignalIndex(-1);
				}
				else
				{
					UpdateSignal(Mathf.CeilToInt(num2));
				}
			}
		}
		else if (Domain != null)
		{
			if (PingTime < 0f && !_isPinging)
			{
				DoPing();
			}
			PingTime -= Time.deltaTime;
		}
	}

	private void DoPing()
	{
		_isPinging = true;
		ToGetPingAsync().ContinueWith(delegate(Task<double> ping)
		{
			MainThreadDispatcher.Instance.Enqueue(delegate
			{
				_isPinging = false;
				int num = Mathf.CeilToInt((float)ping.Result);
				int pingMs = Math.Min(MaxPingMs, num + (int)Math.Min((float)num * 0.5f, 100f));
				UpdateSignal(pingMs);
			});
		});
	}

	private void UpdateSignal(int pingMs)
	{
		if (SignalObjects != null && SignalObjects.Length != 0)
		{
			int num = SignalObjects.Length;
			int signalIndex = Mathf.Clamp(Mathf.FloorToInt((float)pingMs / (float)MaxPingMs * (float)num), 0, num - 1);
			SetSignalIndex(signalIndex);
		}
	}

	private void SetSignalIndex(int activeIndex)
	{
		if (SignalObjects == null)
		{
			return;
		}
		for (int i = 0; i < SignalObjects.Length; i++)
		{
			if (SignalObjects[i] != null && i == activeIndex != SignalObjects[i].activeSelf)
			{
				SignalObjects[i].SetActive(i == activeIndex);
			}
		}
	}

	private async Task<double> ToGetPingAsync()
	{
		PingTime = PingInterval;
		return await FuncUdpLatency.MeasureLatencyAsync(Domain, Port, "Ping", 1, 10000);
	}

	public void SetPingInfo(int serverId)
	{
		Runtime = null;
		Domain = GameEntry.Lua.GetTemplateData("season_bullet_server", serverId, "Domain");
		int.TryParse(GameEntry.ConfigCache.GetTemplateData("season_bullet_server", serverId, "Port"), out Port);
	}

	public void SetRuntimeRtt(GameUnityRuntime runtime, int index = 0)
	{
		Runtime = runtime;
		_playerIndex = index;
		_envClient = null;
		_rttUpdateTimer = 0f;
		Domain = null;
		Port = 0;
	}

	public void SetRuntimeRtt(GameUnityRuntime runtime, GGGoEnvClient envClient, bool isSelf)
	{
		Runtime = runtime;
		_playerIndex = -1;
		_envClient = envClient;
		_isSelf = isSelf;
		_rttUpdateTimer = 0f;
		Domain = null;
		Port = 0;
	}

	private int ResolvePlayerIndex()
	{
		if (_envClient != null)
		{
			int playerID = (int)_envClient.GetPlayerID();
			if (!_isSelf)
			{
				return 1 - playerID;
			}
			return playerID;
		}
		return _playerIndex;
	}

	private void OnDestroy()
	{
		Runtime = null;
		_playerIndex = -1;
		_envClient = null;
		Domain = null;
		Port = 0;
	}
}
