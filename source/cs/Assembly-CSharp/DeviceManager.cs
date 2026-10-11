using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using SFSLitJson;
using UnityEngine;

public class DeviceManager
{
	private const string NativeFuncAppStorageSize = "LW_GetAppStorageSize";

	private const string StorageCollectOkKey = "storage_collect_ok";

	private const string StorageCollectReasonKey = "storage_collect_reason";

	private const string StorageTotalBytesKey = "storage_total_bytes";

	private const string StorageTotalMbKey = "storage_total_mb";

	private const string StorageAppBytesKey = "storage_app_bytes";

	private const string StorageDataBytesKey = "storage_data_bytes";

	private const string StorageCacheBytesKey = "storage_cache_bytes";

	private const string StorageCollectDurationMsKey = "storage_collect_duration_ms";

	private string m_strDeviceUid = "";

	private HashSet<string> _dma_counrtys = new HashSet<string>
	{
		"AT", "BE", "BG", "CY", "CZ", "DE", "DK", "EE", "ES", "FI",
		"FR", "GR", "HR", "HU", "IE", "IT", "LT", "LU", "LV", "MT",
		"NL", "PL", "PT", "RO", "SE", "SI", "SK", "UK", "LI", "NO",
		"IS"
	};

	private static readonly Dictionary<string, string> _convertTrackKey = new Dictionary<string, string>
	{
		{ "storage_collect_ok", "f_para1" },
		{ "storage_collect_duration_ms", "f_para2" },
		{ "storage_collect_reason", "s_para1" },
		{ "storage_total_bytes", "i_para1" },
		{ "storage_total_mb", "i_para2" },
		{ "storage_app_bytes", "i_para3" },
		{ "storage_data_bytes", "i_para4" },
		{ "storage_cache_bytes", "i_para5" }
	};

	public int GetNetworkStatus()
	{
		return GetNetworkType() switch
		{
			NetworkReachability.ReachableViaLocalAreaNetwork => 1, 
			NetworkReachability.ReachableViaCarrierDataNetwork => 2, 
			_ => 0, 
		};
	}

	public NetworkReachability GetNetworkType()
	{
		return Application.internetReachability;
	}

	public string GetNetworkTypeDesc()
	{
		NetworkReachability networkType = GetNetworkType();
		string result = "";
		switch (networkType)
		{
		case NetworkReachability.NotReachable:
			result = "no net";
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			result = "4g";
			break;
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			result = "wifi";
			break;
		}
		return result;
	}

	public string GetSerialID()
	{
		return GameEntry.Sdk.GetSerialID();
	}

	public string GetDeviceInfo()
	{
		return GameEntry.Sdk.GetDeviceInfo();
	}

	public string GetHandSetInfo()
	{
		return GameEntry.Sdk.GetHandSetInfo();
	}

	public string GetNewAndroidDeviceID()
	{
		return GameEntry.Sdk.GenerateHighVersionUUID();
	}

	public string GetOSVersion()
	{
		return SystemInfo.operatingSystem;
	}

	public float GetBatteryLevel()
	{
		return SystemInfo.batteryLevel;
	}

	public BatteryStatus GetBatteryStatus()
	{
		return SystemInfo.batteryStatus;
	}

	public string GetProcessorType()
	{
		return SystemInfo.processorType;
	}

	public int GetProcessorCount()
	{
		return SystemInfo.processorCount;
	}

	public string GetProcessorFrequency()
	{
		return $"{SystemInfo.processorFrequency} MHz";
	}

	public int GetSystemMemorySize()
	{
		return SystemInfo.systemMemorySize;
	}

	public string GetDeviceModel()
	{
		return SystemInfo.deviceModel;
	}

	public DeviceType GetDeviceType()
	{
		return SystemInfo.deviceType;
	}

	private string GenerateDeviceUniqueId()
	{
		return SystemInfo.deviceUniqueIdentifier + UnityEngine.Random.Range(0, 1000000);
	}

	public string GetDeviceUid()
	{
		if (!m_strDeviceUid.IsNullOrEmpty())
		{
			return m_strDeviceUid;
		}
		string text = GameEntry.Setting.GetString("DEVICE_ID", "");
		if (string.IsNullOrEmpty(text))
		{
			text = GameEntry.Sdk.GetDeviceUDID();
			if (string.IsNullOrEmpty(text))
			{
				Debug.LogWarning("原生层获取设备ID为空,请及时排查~");
				text = GenerateDeviceUniqueId();
			}
			text = ((!CommonUtils.IsDebug()) ? (text + "_n3d") : (text + "_3d"));
			GameEntry.Setting.SetString("DEVICE_ID", text);
		}
		if (SDKManager.IS_IPhonePlayer())
		{
			string text2 = text;
			string text3 = "_n3d";
			string text4 = "_3d";
			if (text2.EndsWith(text3))
			{
				text2 = text2.Substring(0, text2.Length - text3.Length);
			}
			else if (text2.EndsWith(text4))
			{
				text2 = text2.Substring(0, text2.Length - text4.Length);
			}
			Log.Info("save to kc : " + text2);
			GameEntry.Sdk.SendDataToNative("SAVE_DEVICE_ID_TO_KEY_CHAIN", text2);
		}
		m_strDeviceUid = text;
		return text;
	}

	public string GetDeviceUid_Transcoding()
	{
		string deviceUid = GetDeviceUid();
		return SetDeviceUidToTranscoding(deviceUid);
	}

	public string SetDeviceUidToTranscoding(string deviceId)
	{
		string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(deviceId));
		return "lwDid_" + text;
	}

	public string GetDeviceString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Model={0}|", SystemInfo.deviceModel);
		stringBuilder.AppendFormat("Memory={0}|", SystemInfo.systemMemorySize);
		stringBuilder.AppendFormat("Vendor={0}|", SystemInfo.graphicsDeviceVendor);
		stringBuilder.AppendFormat("Processor={0}|", SystemInfo.processorFrequency);
		stringBuilder.AppendFormat("Graphics={0}|", SystemInfo.graphicsDeviceName);
		stringBuilder.AppendFormat("DeviceType={0}|", getGraphicsDeviceType());
		return stringBuilder.ToString();
	}

	public int GetOpenGL()
	{
		int result = 0;
		try
		{
			using AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			using AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			using AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getApplication", Array.Empty<object>());
			using AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getSystemService", new object[1] { "activity" });
			using AndroidJavaObject androidJavaObject4 = androidJavaObject3.Call<AndroidJavaObject>("getDeviceConfigurationInfo", Array.Empty<object>());
			return androidJavaObject4.Get<int>("reqGlEsVersion");
		}
		catch (Exception)
		{
			return result;
		}
	}

	public string getGraphicsDeviceType()
	{
		try
		{
			int openGL = GetOpenGL();
			int num = openGL >> 16;
			int num2 = openGL & 0xFFFF;
			string value = $"OpenGL{num}.{num2}";
			bool num3 = isSupportDxt();
			bool num4 = IsSupportASTC();
			bool flag = IsSupportEtc2();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			if (num4)
			{
				stringBuilder.Append("-ASTC");
			}
			if (flag)
			{
				stringBuilder.Append("-ETC2");
			}
			if (num3)
			{
				stringBuilder.Append("-DXT");
			}
			if (SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_4x4))
			{
				stringBuilder.Append("-ASTC_4x4");
			}
			if (SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_6x6))
			{
				stringBuilder.Append("-ASTC_6x6");
			}
			return stringBuilder.ToString();
		}
		catch (Exception value2)
		{
			Console.WriteLine(value2);
		}
		return "";
	}

	public bool isSupportDxt()
	{
		if (!SystemInfo.SupportsTextureFormat(TextureFormat.DXT1) && !SystemInfo.SupportsTextureFormat(TextureFormat.DXT5) && !SystemInfo.SupportsTextureFormat(TextureFormat.DXT1Crunched))
		{
			return SystemInfo.SupportsTextureFormat(TextureFormat.DXT5Crunched);
		}
		return true;
	}

	public bool IsSupportASTC()
	{
		bool flag = false;
		for (TextureFormat textureFormat = TextureFormat.ASTC_4x4; textureFormat <= TextureFormat.ASTC_RGBA_12x12; textureFormat++)
		{
			flag = SystemInfo.SupportsTextureFormat(textureFormat);
			if (!flag)
			{
				return flag;
			}
		}
		return flag;
	}

	public bool IsSupportEtc2()
	{
		bool flag = false;
		for (TextureFormat textureFormat = TextureFormat.ETC2_RGB; textureFormat <= TextureFormat.ETC2_RGBA8; textureFormat++)
		{
			flag = SystemInfo.SupportsTextureFormat(textureFormat);
			if (!flag)
			{
				return flag;
			}
		}
		return flag;
	}

	public bool IsEUCountry()
	{
		if (_dma_counrtys.Contains(GameEntry.GlobalData.fromCountry))
		{
			return true;
		}
		return false;
	}

	public bool IsKR()
	{
		if (GameEntry.GlobalData.fromCountry == "KR")
		{
			return true;
		}
		return false;
	}

	public void RequestAndroidAppStorageSizeCollect(bool forceRefresh = false)
	{
		if (SDKManager.IS_UNITY_ANDROID() && !SDKManager.IS_UNITY_EDITOR() && GameEntry.Sdk != null)
		{
			JsonData jsonData = new JsonData();
			jsonData["forceRefresh"] = forceRefresh;
			GameEntry.Sdk.SendDataToNative("LW_GetAppStorageSize", jsonData.ToJson());
		}
	}

	public void TrackAndroidAppStorageSizeTrackData()
	{
		if (SDKManager.IS_UNITY_ANDROID() && !SDKManager.IS_UNITY_EDITOR())
		{
			Dictionary<string, object> androidAppStorageSizeTrackData = GetAndroidAppStorageSizeTrackData();
			PostEventLog.TrackMap("ANDROID_APP_INFO", ConvertTrackMap(androidAppStorageSizeTrackData));
		}
	}

	private Dictionary<string, object> GetAndroidAppStorageSizeTrackData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>(8);
		if (SDKManager.IS_UNITY_ANDROID() && !SDKManager.IS_UNITY_EDITOR())
		{
			if (GameEntry.Sdk == null)
			{
				dictionary["storage_collect_ok"] = 0;
				dictionary["storage_collect_reason"] = "sdk_not_ready";
				return dictionary;
			}
			string dataFromNative = GameEntry.Sdk.GetDataFromNative("LW_GetAppStorageSize", "");
			FillAppStorageTrackMap(dataFromNative, dictionary);
		}
		else
		{
			dictionary["storage_collect_ok"] = 0;
			dictionary["storage_collect_reason"] = "not_android";
		}
		return dictionary;
	}

	private static Dictionary<string, object> ConvertTrackMap(Dictionary<string, object> source)
	{
		if (source == null || source.Count == 0)
		{
			return source;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>(source.Count);
		foreach (KeyValuePair<string, object> item in source)
		{
			if (_convertTrackKey.TryGetValue(item.Key, out var value))
			{
				dictionary[value] = item.Value;
			}
			else
			{
				dictionary[item.Key] = item.Value;
			}
		}
		return dictionary;
	}

	private void FillAppStorageTrackMap(string rawJson, Dictionary<string, object> map)
	{
		map["storage_collect_ok"] = 0;
		map["storage_collect_reason"] = "empty_result";
		if (string.IsNullOrEmpty(rawJson))
		{
			return;
		}
		try
		{
			JsonData jsonData = JsonMapper.ToObject(rawJson);
			if (jsonData == null || !jsonData.IsObject)
			{
				map["storage_collect_reason"] = "invalid_json";
				return;
			}
			int jsonInt = GetJsonInt(jsonData, "ok", 0);
			string jsonString = GetJsonString(jsonData, "reason", (jsonInt == 1) ? "success" : "unknown");
			long jsonLong = GetJsonLong(jsonData, "totalBytes", 0L);
			long jsonLong2 = GetJsonLong(jsonData, "appBytes", 0L);
			long jsonLong3 = GetJsonLong(jsonData, "dataBytes", 0L);
			long jsonLong4 = GetJsonLong(jsonData, "cacheBytes", 0L);
			int jsonInt2 = GetJsonInt(jsonData, "durationMs", 0);
			map["storage_collect_ok"] = jsonInt;
			map["storage_collect_reason"] = jsonString;
			map["storage_total_bytes"] = jsonLong;
			map["storage_total_mb"] = BytesToMb(jsonLong);
			map["storage_app_bytes"] = jsonLong2;
			map["storage_data_bytes"] = jsonLong3;
			map["storage_cache_bytes"] = jsonLong4;
			if (jsonInt2 > 0)
			{
				map["storage_collect_duration_ms"] = jsonInt2;
			}
		}
		catch (Exception)
		{
			map["storage_collect_ok"] = 0;
			map["storage_collect_reason"] = "json_parse_error";
		}
	}

	private static int GetJsonInt(JsonData json, string key, int defaultValue)
	{
		try
		{
			return Convert.ToInt32(json[key].ToString());
		}
		catch
		{
			return defaultValue;
		}
	}

	private static long GetJsonLong(JsonData json, string key, long defaultValue)
	{
		try
		{
			return Convert.ToInt64(json[key].ToString());
		}
		catch
		{
			return defaultValue;
		}
	}

	private static string GetJsonString(JsonData json, string key, string defaultValue)
	{
		try
		{
			string text = json[key].ToString();
			return string.IsNullOrEmpty(text) ? defaultValue : text;
		}
		catch
		{
			return defaultValue;
		}
	}

	private static double BytesToMb(long bytes)
	{
		if (bytes <= 0)
		{
			return 0.0;
		}
		return Math.Round((double)bytes / 1024.0 / 1024.0, 2);
	}
}
