using System;
using System.Collections.Generic;
using Joker;
using Joker.Client;
using MiniGame.Core.Server;
using Newtonsoft.Json;
using UnityEngine;

namespace MiniGame.Core.Client;

[ExecuteAlways]
public sealed class GameUnityApp : MonoBehaviour
{
	public static GameUnityApp Instance { get; private set; }

	public static void Init()
	{
		if (Instance == null)
		{
			Instance = UnityEngine.Object.FindObjectOfType<GameUnityApp>();
		}
		if (Instance == null)
		{
			Instance = new GameObject("MiniGameApp").AddComponent<GameUnityApp>();
		}
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(Instance.gameObject);
		}
		Instance.enabled = true;
		Instance.gameObject.SetActive(value: true);
	}

	private void Awake()
	{
		App.AddService<TimeService>();
		App.AddService<ThreadService>();
		App.AddService<ISchedulerService, SchedulerServiceThread>();
		App.AddService<WorldService>();
		App.AddService<ILogService, UnityLogService>();
		App.AddService<IMessageService, MessageServiceJson, List<Type>, JsonSerializerSettings>(RoomMessage.MessageTypes, null);
		App.Startup();
	}

	private void Update()
	{
		App.Update(Time.deltaTime);
	}

	private void LateUpdate()
	{
		App.LateUpdate(Time.deltaTime);
	}

	private void OnDestroy()
	{
		App.Shutdown();
	}
}
