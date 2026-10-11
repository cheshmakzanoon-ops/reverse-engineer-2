using System;
using System.Collections.Generic;
using GameFramework;
using SFSLitJson;
using Zendesk;

namespace AIHelp;

public class AIHelpProxy
{
	public class AIHelpDataTable
	{
		public string Game_Version;

		public string Device_ID;

		public string AirKey;

		public string Mail_Address;

		public string Distinct_ID;
	}

	public static string CurrentEntranceId = "";

	public static string CurrentEntranceName = "";

	private static bool isAfterInitMessage = false;

	private static int unreadMsgCount = 0;

	public static int MyOutgoingTicket = 0;

	public static AIHelpDataTable aIHelpDataTable;

	private static readonly Dictionary<string, string> FieldMapping = new Dictionary<string, string>
	{
		{ "uid", "ah_uid" },
		{ "username", "ah_username" },
		{ "ServerId", "ah_server_id" },
		{ "Custom_Data", "ah_user_tag" },
		{ "Register_Country", "ah_register_country" },
		{ "Country_Code", "ah_current_country" },
		{ "Game_Language", "ah_current_language" },
		{ "Device_Model", "ah_device_model" },
		{ "Os_Version", "ah_os_version" },
		{ "AppId", "ah_app_name" },
		{ "Platform", "ah_os_platform" },
		{ "Network_Type", "ah_device_network" },
		{ "Main_Level", "ah_main_level" },
		{ "Alliance_ID", "ah_alliance_id" },
		{ "Allliance_Name", "ah_alliance_name" },
		{ "VIP_Level", "ah_vip_level" },
		{ "userVipStage", "ah_vip_stage" },
		{ "contact_status", "ah_contact_status" },
		{ "contact_person", "ah_contact_person" },
		{ "Mail_Address", "ah_bind_email" },
		{ "Register_Date", "ah_register_date" },
		{ "Device_ID", "ah_device_id" },
		{ "AirKey", "ah_airkey" },
		{ "Resource_Version", "ah_resource_version" },
		{ "Distinct_ID", "ah_distinct_id" },
		{ "Game_Version", "ah_app_version" },
		{ "Entrance", "ah_entrance" },
		{ "is_whale", "ah_is_whale" },
		{ "available_ram", "ah_device_memory_free" },
		{ "Cross_Server_Id", "ah_cross_server_id" },
		{ "Alliance_Rank", "ah_alliance_rank" }
	};

	public static int UnreadMsgCount => unreadMsgCount + ZendeskInit.unreadMsgCount;

	public static void SetAiHelpUnreadMsgCount(int msgCount)
	{
		unreadMsgCount = msgCount;
	}

	public static int GetAiHelpUnreadMsgCount()
	{
		return unreadMsgCount;
	}

	public static void OnAfterInitMessage()
	{
		isAfterInitMessage = true;
		if (CheckAiHelpSwitch())
		{
			AIHelpUnreadFetcher.FetchUnreadCount(null, null, ignoreCooldown: true);
		}
	}

	public static bool CheckAiHelpSwitch()
	{
		if (GameEntry.Lua == null)
		{
			return false;
		}
		if (!GameEntry.Lua.HasGameStart)
		{
			return false;
		}
		if (!isAfterInitMessage)
		{
			return false;
		}
		if (GameEntry.Setting.HasSetting("AIHelpGmButton2_ForceMethod"))
		{
			switch (GameEntry.Setting.GetInt("AIHelpGmButton2_ForceMethod"))
			{
			case 1:
				return false;
			case 2:
				return true;
			}
		}
		if (!GameEntry.Lua.CallWithReturn<bool, string>("CSharpCallLuaInterface.CheckSwitch", "operation_backend"))
		{
			return false;
		}
		string text = GameEntry.Lua.CallWithReturn<string, string, string, string>("CSharpCallLuaInterface.GetConfigStr", "operation_backend_switch", "k1", string.Empty);
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		if (!IsServerLanguageMatch(text))
		{
			return false;
		}
		return true;
	}

	private static bool IsServerLanguageMatch(string switchCfg)
	{
		int selfServerId = GameEntry.Data.Player.GetSelfServerId();
		string value = GameEntry.Localization.GetLanguageName() ?? "en";
		string[] array = switchCfg.Split(new char[1] { '|' });
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Trim().Split(new char[1] { ';' });
			if (array2.Length != 2)
			{
				continue;
			}
			string text = array2[0].Trim();
			string text2 = array2[1].Trim();
			bool flag;
			if (text.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				flag = true;
			}
			else
			{
				string[] array3 = text.Split(new char[1] { '-' });
				if (array3.Length == 1 && int.TryParse(array3[0], out var result))
				{
					flag = selfServerId == result;
				}
				else
				{
					if (array3.Length != 2 || !int.TryParse(array3[0], out var result2) || !int.TryParse(array3[1], out var result3))
					{
						continue;
					}
					flag = selfServerId >= result2 && selfServerId <= result3;
				}
			}
			if (!flag)
			{
				continue;
			}
			if (text2.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			string[] array4 = text2.Split(new char[1] { ',' });
			for (int j = 0; j < array4.Length; j++)
			{
				if (array4[j].Trim().Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void Init()
	{
		AIHelpSupport.SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback);
		AIHelpSupport.Init("LASTWARDEV_app_3a8bd35be72e471abfaaa5f30971c54d", "lastwardev.aihelp.net", "lastwardev_platform_342905211d470af49c7bd03dbb6bb026");
	}

	private static void OnAIHelpInitializedCallback(bool isSuccess, string message)
	{
		if (isSuccess)
		{
			AIHelpSupport.StartUnreadMessageCountPolling(OnUnreadMessageCountPolling);
		}
		else
		{
			Log.Error("[AiHelp] AIHelp init failed: " + message);
		}
	}

	private static void OnUnreadMessageCountPolling(int msgCount)
	{
	}

	public static void Show(string entranceId, string welcomeMessage)
	{
		CurrentEntranceId = entranceId;
		CurrentEntranceName = entranceId;
		if (ZendeskDefine.EntranceName.ContainsKey(entranceId))
		{
			CurrentEntranceName = ZendeskDefine.EntranceName[entranceId].name;
		}
		if (GameEntry.Setting.HasSetting("AIHelpGmButton2_ForceMethod"))
		{
			switch (GameEntry.Setting.GetInt("AIHelpGmButton2_ForceMethod"))
			{
			case 1:
				ShowZendesk();
				return;
			case 2:
				ShowAiHelp();
				return;
			}
		}
		if (!CheckAiHelpSwitch())
		{
			ShowZendesk();
			return;
		}
		if (ZendeskInit.unreadMsgCount > 0)
		{
			ShowZendesk();
			return;
		}
		AIHelpUnreadFetcher.FetchZendeskTicketStatus(delegate(int outgoingTicket)
		{
			MyOutgoingTicket = outgoingTicket;
			if (outgoingTicket == 0)
			{
				ShowAiHelp();
			}
			else
			{
				ShowZendesk();
			}
		}, delegate
		{
			MyOutgoingTicket = 0;
			ShowZendesk();
		}, ignoreCooldown: true);
	}

	private static void ShowAiHelp()
	{
		CloseAiHelpUnreadCount();
		AIHelpH5Dialog.Instance.OpenPrivacyView();
	}

	private static void ShowZendesk()
	{
		unreadMsgCount = 0;
		ZendeskCore.ZendeskSupport(CurrentEntranceId);
	}

	public static void CloseAiHelpUnreadCount()
	{
		unreadMsgCount = 0;
		if (GameEntry.Lua != null && GameEntry.Lua.HasGameStart)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.DoCloseCustomerServiceRedPointData");
		}
	}

	public static void SetAIHelpDataList()
	{
		if (aIHelpDataTable == null)
		{
			aIHelpDataTable = new AIHelpDataTable();
		}
		aIHelpDataTable.Game_Version = GameEntry.Sdk.Version;
		aIHelpDataTable.Device_ID = GameEntry.Device.GetDeviceUid();
		aIHelpDataTable.AirKey = GameEntry.Device.GetDeviceUid_Transcoding();
		aIHelpDataTable.Mail_Address = GameEntry.Setting.GetString("Setting.CUSTOM_UID", "");
		aIHelpDataTable.Distinct_ID = GameEntry.Sdk.DistinctId;
	}

	public static void UpdateUserInfo(string uid, string uname, string tag, string serverId, string json)
	{
		AIHelpUnreadFetcher.SetUserId(uid);
		ZendeskDefine.UserConfig.Set(uid, uname, serverId, tag, json);
		ZendeskCore.SetFields(ZendeskDefine.UserConfig.ToFields());
		AIHelpSupport.UpdateUserInfo(new UserConfig.Builder().SetUserId(uid).SetUserName(uname).SetUserTags(tag)
			.SetCustomData(json)
			.SetServerId(serverId)
			.build());
	}

	public static void Logout()
	{
		isAfterInitMessage = false;
		unreadMsgCount = 0;
		ZendeskCore.Logout();
		ZendeskCore.ClearFields();
	}

	public static string MapZendeskFieldsToAIHelpJson()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Dictionary<string, ZendeskDefine.UserConfig.FieldInfo> dict = ZendeskDefine.UserConfig.GetDict();
		foreach (KeyValuePair<string, string> item in FieldMapping)
		{
			string key = item.Key;
			string value = item.Value;
			if (dict.TryGetValue(key, out var value2))
			{
				string value3 = value2.value;
				if (!string.IsNullOrEmpty(value3))
				{
					dictionary[value] = value3;
				}
			}
		}
		dictionary["ah_entrance"] = CurrentEntranceId;
		dictionary["zd_entrance"] = CurrentEntranceName;
		dictionary["ah_channel"] = "Game-App";
		if (aIHelpDataTable != null && !string.IsNullOrEmpty(aIHelpDataTable.Mail_Address))
		{
			dictionary["ah_bind_email"] = aIHelpDataTable.Mail_Address;
		}
		JsonData jsonData = new JsonData();
		foreach (KeyValuePair<string, string> item2 in dictionary)
		{
			jsonData[item2.Key] = item2.Value;
		}
		return jsonData.ToJson();
	}
}
