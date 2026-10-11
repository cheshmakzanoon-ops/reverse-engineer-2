using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using GameFramework;
using SFSLitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace AIHelp;

public static class AIHelpUnreadFetcher
{
	private const string PathUnreadCount = "/sdk/api/v4.0/ticket/getunreadcount";

	private const string PathZendeskTicketStatus = "/sdk/api/v4.0/ticket/queryzendeskticketstatus";

	private const int TimeoutSeconds = 10;

	private static float _lastFetchUnreadTime = -999f;

	private static float _lastFetchTicketTime = -999f;

	private const float CooldownSeconds = 30f;

	private static string _userId = "";

	private static readonly System.Random _rng = new System.Random();

	public static void SetUserId(string uid)
	{
		if (!string.IsNullOrEmpty(uid))
		{
			_userId = uid;
		}
	}

	public static void FetchUnreadCount(Action<int> onSuccess = null, Action<string> onError = null, bool ignoreCooldown = false)
	{
		if (CheckPreconditions("FetchUnreadCount", ref _lastFetchUnreadTime, ignoreCooldown, onError))
		{
			_lastFetchUnreadTime = Time.realtimeSinceStartup;
			ApplicationLaunch.Instance.StartCoroutine(GetCoroutine("/sdk/api/v4.0/ticket/getunreadcount", ParseUnreadCountResponse, onSuccess, onError));
		}
	}

	public static void FetchZendeskTicketStatus(Action<int> onSuccess = null, Action<string> onError = null, bool ignoreCooldown = false)
	{
		if (CheckPreconditions("FetchZendeskTicketStatus", ref _lastFetchTicketTime, ignoreCooldown, onError))
		{
			_lastFetchTicketTime = Time.realtimeSinceStartup;
			ApplicationLaunch.Instance.StartCoroutine(GetCoroutine("/sdk/api/v4.0/ticket/queryzendeskticketstatus", ParseZendeskTicketStatusResponse, onSuccess, onError));
		}
	}

	private static bool CheckPreconditions(string tag, ref float lastTime, bool ignoreCooldown, Action<string> onError)
	{
		if (string.IsNullOrEmpty(_userId))
		{
			Log.Warning("[AiHelp] " + tag + " skipped: userId not set yet");
			onError?.Invoke("UserId not set");
			return false;
		}
		if (!ignoreCooldown && Time.realtimeSinceStartup - lastTime < 30f)
		{
			Log.Info("[AiHelp] " + tag + " skipped: cooldown not elapsed");
			onError?.Invoke("Cooldown");
			return false;
		}
		if (ApplicationLaunch.Instance == null)
		{
			Log.Error("[AiHelp] " + tag + " failed: ApplicationLaunch.Instance is null");
			onError?.Invoke("ApplicationLaunch unavailable");
			return false;
		}
		return true;
	}

	private static IEnumerator GetCoroutine(string path, Action<string, Action<int>, Action<string>> parser, Action<int> onSuccess, Action<string> onError)
	{
		AIHelpEnvConfig.Config current = AIHelpEnvConfig.Current;
		string text = CurrentTimestampMs();
		string text2 = GenerateNonce(64);
		string value = CalculateSign(current.AppKey, text2, text);
		string uri = "https://" + current.Domain + path;
		UnityWebRequest request = null;
		string responseText = null;
		bool isNetError = false;
		string netErrMsg = null;
		try
		{
			request = UnityWebRequest.Get(uri);
			request.timeout = 10;
			request.SetRequestHeader("appkey", current.AppKey);
			request.SetRequestHeader("appid", current.AppId);
			request.SetRequestHeader("nonce", text2);
			request.SetRequestHeader("timestamp", text);
			request.SetRequestHeader("sign", value);
			request.SetRequestHeader("userid", _userId);
		}
		catch (Exception ex)
		{
			Log.Error("[AiHelp] GET " + path + " build request error: " + ex.Message);
			onError?.Invoke("Build request error: " + ex.Message);
			request?.Dispose();
			yield break;
		}
		yield return request.SendWebRequest();
		try
		{
			if (request.isNetworkError || request.isHttpError)
			{
				isNetError = true;
				netErrMsg = $"HTTP {request.responseCode}: {request.error}";
			}
			else
			{
				responseText = request.downloadHandler?.text;
			}
		}
		finally
		{
			request.Dispose();
		}
		if (isNetError)
		{
			Log.Error("[AiHelp] GET " + path + " network error: " + netErrMsg);
			onError?.Invoke(netErrMsg);
		}
		else if (string.IsNullOrEmpty(responseText))
		{
			Log.Error("[AiHelp] GET " + path + ": empty response");
			onError?.Invoke("Empty response");
		}
		else
		{
			parser(responseText, onSuccess, onError);
		}
	}

	private static JsonData ValidateAndGetData(string body, string tag, Action<string> onError, out bool ok)
	{
		ok = false;
		try
		{
			JsonData jsonData = JsonMapper.ToObject(body);
			if (jsonData == null)
			{
				Log.Error("[AiHelp] " + tag + ": JSON is null");
				onError?.Invoke("JSON null");
				return null;
			}
			bool flag = jsonData.Keys.Contains("flag") && jsonData["flag"] != null && (bool)jsonData["flag"];
			int num = ((jsonData.Keys.Contains("code") && jsonData["code"] != null) ? ((int)jsonData["code"]) : (-1));
			if (!flag || num != 200)
			{
				string arg = ((jsonData.Keys.Contains("desc") && jsonData["desc"] != null) ? ((string)jsonData["desc"]) : "unknown");
				string text = $"flag={flag}, code={num}, desc={arg}";
				Log.Error("[AiHelp] " + tag + " server error: " + text);
				onError?.Invoke(text);
				return null;
			}
			if (!jsonData.Keys.Contains("data") || jsonData["data"] == null)
			{
				Log.Error("[AiHelp] " + tag + ": missing data field");
				onError?.Invoke("Missing data field");
				return null;
			}
			ok = true;
			return jsonData["data"];
		}
		catch (Exception ex)
		{
			Log.Error("[AiHelp] " + tag + " parse error: " + ex.Message + "\nBody: " + body);
			onError?.Invoke("Parse error: " + ex.Message);
			return null;
		}
	}

	private static void ParseUnreadCountResponse(string body, Action<int> onSuccess, Action<string> onError)
	{
		bool ok;
		JsonData jsonData = ValidateAndGetData(body, "FetchUnreadCount", onError, out ok);
		if (ok)
		{
			int num = ((jsonData.Keys.Contains("unReadMsgCount") && jsonData["unReadMsgCount"] != null) ? ((int)jsonData["unReadMsgCount"]) : 0);
			AIHelpProxy.SetAiHelpUnreadMsgCount(num);
			onSuccess?.Invoke(num);
		}
	}

	private static void ParseZendeskTicketStatusResponse(string body, Action<int> onSuccess, Action<string> onError)
	{
		bool ok;
		JsonData jsonData = ValidateAndGetData(body, "FetchZendeskTicketStatus", onError, out ok);
		if (ok)
		{
			int obj = ((jsonData.Keys.Contains("outgoingTicket") && jsonData["outgoingTicket"] != null) ? ((int)jsonData["outgoingTicket"]) : 0);
			onSuccess?.Invoke(obj);
		}
	}

	private static string CalculateSign(string appKey, string nonce, string timestamp)
	{
		SortedDictionary<string, string> obj = new SortedDictionary<string, string>(StringComparer.Ordinal)
		{
			{ "AppKey", appKey },
			{ "Nonce", nonce },
			{ "TimeSpan", timestamp }
		};
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (KeyValuePair<string, string> item in obj)
		{
			if (!flag)
			{
				stringBuilder.Append('&');
			}
			stringBuilder.Append(item.Key).Append('=').Append(item.Value);
			flag = false;
		}
		string text = Md5Hex(appKey + timestamp);
		return Md5Hex(stringBuilder.ToString() + text);
	}

	private static string Md5Hex(string input)
	{
		using MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder(32);
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.Append(b.ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	private static string GenerateNonce(int length)
	{
		StringBuilder stringBuilder = new StringBuilder(length);
		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[_rng.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)]);
		}
		return stringBuilder.ToString();
	}

	private static string CurrentTimestampMs()
	{
		return ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString();
	}
}
