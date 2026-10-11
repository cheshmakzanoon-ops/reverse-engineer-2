using System;
using System.Collections.Generic;
using FibMatrix;
using GameFramework;
using RiverBISDK;
using Sfs2X.Entities.Data;
using VEngine;
using Zendesk;

public class InitMessage : BaseMessage
{
	private enum NewUserWorld
	{
		Skip,
		Ing,
		Pass
	}

	private static InitMessage _instance;

	public static InitMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<InitMessage>());

	public override string GetMsgId()
	{
		return "init";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		PostEventLog.TrackMap("PUSH_INIT_RECV", SDKManager.AddBILaunchTimeProperty());
		Log.Info("[PushInit] handle PushInit");
		ApplicationLaunch.Instance.Loading.IsPushInitReceived = true;
		if (message.ContainsKey("identification"))
		{
			ISFSObject iSFSObject = message.TryGetObj("identification");
			if (iSFSObject != null)
			{
				bool flag = iSFSObject.TryGetBool("authenticate");
				bool flag2 = iSFSObject.TryGetBool("isCN");
				bool flag3 = iSFSObject.TryGetBool("isChild");
				GameEntry.GlobalData.SetCnFlagFromServer(flag2);
				if (!flag && flag2)
				{
					ApplicationLaunch.Instance.Loading.IsNeedIdentification = true;
					string text = iSFSObject.TryGetString("uid");
					AccountCredentialManager.SetUID(text);
					Log.Info("[AT]SetGUID_InitMsg1:" + text);
					return;
				}
				if (flag2 && flag3)
				{
					ApplicationLaunch.Instance.Loading.IsNeedIdentification = true;
					ApplicationLaunch.Instance.Loading.IsChild = true;
					string text2 = iSFSObject.TryGetString("uid");
					AccountCredentialManager.SetUID(text2);
					Log.Info("[AT]SetGUID_InitMsg2:" + text2);
					return;
				}
			}
		}
		ApplicationLaunch.Instance.Loading.IsNeedIdentification = false;
		PrivacyFuncUtil.Instance.PushInit(message);
		InitData(message);
	}

	public void InitData(ISFSObject message)
	{
		bool flag = false;
		try
		{
			if (message.ContainsKey("user"))
			{
				AccountCredentialManager.SetUID(message.GetSFSObject("user").GetUtfString("uid"));
			}
			if (!CheckSeasonResourceData(message))
			{
				ApplicationLaunch.Instance.ReStartGame();
				return;
			}
			if (message.ContainsKey("isShipShow"))
			{
				message.GetSFSObject("dataConfig")?.PutInt("isShipShow", message.TryGetInt("isShipShow"));
			}
			if (message.ContainsKey("user"))
			{
				ISFSObject sFSObject = message.GetSFSObject("user");
				string utfString = sFSObject.GetUtfString("uid");
				int num = sFSObject.GetInt("serverId");
				BIManager.UpdateGameInfo(new Dictionary<string, object>(5)
				{
					{ "uid", utfString },
					{
						"device_id",
						GameEntry.Device.GetDeviceUid()
					},
					{
						"airKey",
						GameEntry.Device.GetDeviceUid_Transcoding()
					},
					{ "sid", num },
					{
						"platform",
						GameEntry.Sdk.GetPlatformNameForBI()
					},
					{
						"country",
						GameEntry.GlobalData.fromCountry
					}
				});
				if (sFSObject.ContainsKey("crossWatchServerNew") && sFSObject.GetBool("crossWatchServerNew"))
				{
					Log.Info("Set crossWatchServerNew in C#");
					CrossServerComponent.OnlyMainLine = true;
				}
				else
				{
					Log.Info("close crossWatchServerNew in C#");
					CrossServerComponent.OnlyMainLine = false;
				}
				if (sFSObject.ContainsKey("regCountry"))
				{
					string utfString2 = sFSObject.GetUtfString("regCountry");
					Log.Info("regCountry: " + utfString2);
					GameEntry.Sdk.RegCountry = utfString2;
				}
			}
			string uid = AccountCredentialManager.ServerInfo.uid;
			if (!string.IsNullOrEmpty(uid))
			{
				if (!GameEntry.Sdk.alreadyStartAF)
				{
					GameEntry.Sdk.initAppsFlyer(uid);
					GameEntry.Sdk.alreadyStartAF = true;
				}
				GameEntry.Sdk.SetUserId(uid);
				string zone = AccountCredentialManager.ServerInfo.zone;
				FibMatrix.Logger.CurrentRemoteLoggerTarget?.UpdateUserInfo(uid, zone);
			}
			if (message.ContainsKey("realLoginCountry"))
			{
				string utfString3 = message.GetUtfString("realLoginCountry");
				Log.Info("realLoginCountry: " + utfString3);
				ZendeskDefine.UserConfig.Update("Country_Code", utfString3);
				GameEntry.Sdk.IPCountry = utfString3;
			}
			else
			{
				Log.Info("realLoginCountry: not found by server");
			}
			GameEntry.GlobalData.Init(message);
			GameEntry.Data.Fog.Init(message);
			GameEntry.Data.Player.Init(message);
			if (SceneManager.IsSceneNone())
			{
				if (GameEntry.Data.Player.GetWorldId() > 0)
				{
					SceneManager.CreateWorld();
				}
				else if (!(message.GetUtfString("lwGuideRecord") == "0"))
				{
					SceneManager.CreateCity();
				}
			}
			else
			{
				SceneManager.OnInitMessage();
				flag = SceneManager.IsInWorld();
			}
			PushManager.Instance.onLoginComplete();
			GameEntry.Timer.Tomorrow = (long)message.GetInt("tomorrow") * 1000L;
			if (message.ContainsKey("real_timezone_offset"))
			{
				GameEntry.Timer.SetServerOffset(message.GetInt("real_timezone_offset") * 1000);
			}
			CheckDeviceChangeMessage.Instance.Send();
			GameEntry.Sdk.SendDataToNative("GetInstallRefererInfo", "");
			LuaInitData(message);
			if (flag && SceneManager.World != null)
			{
				SceneManager.World.OnChangeServerRemove();
				SceneManager.World.RefreshView();
			}
			LoginExtMessage.Instance.ClearData();
			if (message.ContainsKey("add"))
			{
				ISFSObject sFSObject2 = message.GetSFSObject("add");
				if (sFSObject2 != null)
				{
					string utfString4 = sFSObject2.GetUtfString("data");
					string utfString5 = sFSObject2.GetUtfString("a");
					string utfString6 = sFSObject2.GetUtfString("b");
					LoginExtMessage.Instance.SetData(utfString4, utfString5, utfString6);
				}
			}
			LoginExtMessage.Instance.Send();
			AnoProxy.AnoUserLogin(uid);
		}
		catch (Exception ex)
		{
			PostEventLog.Record("LOGIN_PARSE_ERROR", ex.StackTrace);
		}
	}

	private void LuaInitData(ISFSObject message)
	{
		GameEntry.Lua.DispatchResponse(GetMsgId(), ((SFSObject)message).ToLuaTable(GameEntry.Lua.Env));
	}

	private bool CheckSeasonResourceData(ISFSObject message)
	{
		if (Versions.SkipUpdate)
		{
			Log.Info("[InitMessage] SkipUpdate, Skip CheckSeasonResourceData");
			return true;
		}
		List<int> list = new List<int>();
		if (message != null && message.ContainsKey("curServerSeasonInfo"))
		{
			ISFSObject sFSObject = message.GetSFSObject("curServerSeasonInfo");
			if (sFSObject != null && sFSObject.ContainsKey("seasonStartTime"))
			{
				int num = message.TryGetInt("seasonId");
				if (num > 0 && num < 7)
				{
					list.Add(1001 + num);
				}
				long num2 = sFSObject.TryGetLong("seasonStartTime");
				long serverTime = GameEntry.Timer.GetServerTime();
				Log.Info($"CheckSeasonResourceData : startTime: {num2}, now: {serverTime}");
				if (num2 < serverTime && sFSObject.ContainsKey("seasonConfigId"))
				{
					int num3 = sFSObject.TryGetInt("seasonConfigId");
					string templateData = GameEntry.ConfigCache.GetTemplateData("lw_season", num3, "package");
					if (int.TryParse(templateData, out var result))
					{
						Log.Info($"CheckSeasonResourceData configId: {num3}, packageId: {templateData}");
						int item = GameEntry.ConfigCache.GetTemplateData("download_packs", result, "pack_id").ToInt();
						list.Add(item);
					}
				}
			}
		}
		try
		{
			if (message != null && message.ContainsKey("downloadPackageStr"))
			{
				string utfString = message.GetUtfString("downloadPackageStr");
				string[] array = utfString.Split(new char[1] { '|' });
				for (int i = 0; i < array.Length; i++)
				{
					if (int.TryParse(array[i], out var result2))
					{
						list.Add(result2);
					}
				}
				Log.Info("[ResourcePackageManager] CheckSeasonResourceData downloadPackageStr: " + utfString);
			}
		}
		catch (Exception message2)
		{
			Log.Info("[ResourcePackageManager] CheckSeasonResourceData Exception");
			Log.Error(message2);
		}
		Log.Info("[ResourcePackageManager] CheckSeasonResourceData requiredPackages: " + string.Join(", ", list));
		bool flag = true;
		for (int num4 = list.Count - 1; num4 >= 0; num4--)
		{
			if (list[num4] == 0 || list[num4] == 1 || list[num4] == 1000)
			{
				list.RemoveAt(num4);
			}
			else if (!ResourcePackageManager.IsPackageHasBundle(list[num4]))
			{
				Log.Info($"[ResourcePackageManager] CheckSeasonResourceData skip empty package {list[num4]}");
			}
			else if (list[num4] >= 1001 && list[num4] < 1010)
			{
				if (!ResourcePackageManager.IsSeasonResDownloadedByPackageId(list[num4]))
				{
					Log.Info($"[ResourcePackageManager] CheckSeasonResourceData package {list[num4]} needs to be downloaded");
					flag = false;
				}
			}
			else if (!ResourcePackageManager.IsPackageDownloaded(list[num4]))
			{
				Log.Info($"[ResourcePackageManager] CheckSeasonResourceData package {list[num4]} needs to be downloaded");
				flag = false;
			}
		}
		ResourcePackageManager.SetRequiredPackagesByInitMessage(list);
		if (!flag)
		{
			if (GameEntry.Setting.CheckFirstLaunchSkipUpdate())
			{
				flag = true;
			}
			else
			{
				Log.Info("[ResourcePackageManager] CheckSeasonResourceData restartGame download packages: " + string.Join(", ", list));
				GameEntry.Setting.DisableFirstLaunchSkipUpdate();
			}
		}
		return flag;
	}
}
