using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Zendesk;

namespace AIHelp;

public static class AIHelpEnvConfig
{
	public struct Config
	{
		public string AppKey;

		public string Domain;

		public string AppId;
	}

	private const string PREFS_KEY = "ZendeskEnv";

	public const int ENV_DEV = 0;

	public const int ENV_TEST = 1;

	public const int ENV_ONLINE = 2;

	public static readonly Config Dev = new Config
	{
		AppKey = "LASTWARDEV_app_3a8bd35be72e471abfaaa5f30971c54d",
		Domain = "aihelp-dev.lastwar.com",
		AppId = "lastwardev_platform_f9ace0a21f24b65b2633ca9c807c4e07"
	};

	public static readonly Config Test = new Config
	{
		AppKey = "AIHELPTEST_app_456d5be3a59944458d221cb9835910ce",
		Domain = "aihelp-test.lastwar.com",
		AppId = "aihelptest_platform_015db24cb587631d21342c11889cc597"
	};

	public static readonly Config Online = new Config
	{
		AppKey = "AlHELPPROD_app_58825bed20d14f63ba9863e8fdf75f12",
		Domain = "aihelp-prod.lastwar.com",
		AppId = "aihelpprod_platform_3a67ba54dcefee83957124c067e1dfb3"
	};

	public static Config Current => PlayerPrefs.GetInt("ZendeskEnv", 2) switch
	{
		0 => Dev, 
		1 => Test, 
		_ => Online, 
	};

	public static string GetUrl()
	{
		string value = AIHelpProxy.MapZendeskFieldsToAIHelpJson();
		Dictionary<string, string> queryParams = new Dictionary<string, string>
		{
			{
				"entranceId",
				AIHelpProxy.CurrentEntranceId
			},
			{
				"appName",
				GetZendeskValue("AppId")
			},
			{
				"userId",
				GetZendeskValue("uid")
			},
			{
				"userName",
				GetZendeskValue("username")
			},
			{
				"userTags",
				GetZendeskValue("Custom_Data")
			},
			{
				"language",
				GetZendeskValue("Game_Language")
			},
			{ "customData", value }
		};
		Config current = Current;
		string baseUrl = "https://" + current.Domain + "/webchatv4/#/";
		Dictionary<string, string> pathParams = new Dictionary<string, string>
		{
			{ "appKey", current.AppKey },
			{ "domain", current.Domain },
			{ "appId", current.AppId }
		};
		return BuildZendeskUrl(baseUrl, pathParams, queryParams);
	}

	private static string GetZendeskValue(string name)
	{
		Dictionary<string, ZendeskDefine.UserConfig.FieldInfo> dict = ZendeskDefine.UserConfig.GetDict();
		if (dict.ContainsKey(name))
		{
			return dict[name].value;
		}
		return null;
	}

	private static string BuildZendeskUrl(string baseUrl, Dictionary<string, string> pathParams, Dictionary<string, string> queryParams)
	{
		StringBuilder stringBuilder = new StringBuilder(baseUrl);
		foreach (KeyValuePair<string, string> pathParam in pathParams)
		{
			stringBuilder.Append(pathParam.Key + "/" + Uri.EscapeDataString(pathParam.Value) + "/");
		}
		StringBuilder stringBuilder2 = new StringBuilder("?");
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> queryParam in queryParams)
		{
			list.Add(queryParam.Key + "=" + queryParam.Value);
		}
		stringBuilder2.Append(string.Join("&", list));
		return stringBuilder.ToString() + stringBuilder2.ToString();
	}
}
