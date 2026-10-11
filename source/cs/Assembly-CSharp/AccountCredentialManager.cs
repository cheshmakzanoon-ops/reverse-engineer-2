using GameFramework;
using UnityEngine;

public class AccountCredentialManager
{
	public sealed class ServerInfoView
	{
		public string uid => PlayerPrefs.GetString("Setting.GAME_UID", string.Empty);

		public int gm => PlayerPrefs.GetInt("Setting.GM_FLAG", 0);

		public string ip => PlayerPrefs.GetString("SERVER_IP", string.Empty);

		public int port => PlayerPrefs.GetInt("SERVER_PORT", 0);

		public string zone => PlayerPrefs.GetString("SERVER_ZONE", string.Empty);

		public int connectionType => PlayerPrefs.GetInt("SERVER_CONNECTION_TYPE", 0);
	}

	public sealed class AuthTokensView
	{
		public string at => PlayerPrefs.GetString("Login.access_token", string.Empty);

		public int attime => PlayerPrefs.GetInt("Login.access_token_time", 0);

		public string rt => PlayerPrefs.GetString("Login.refresh_token", string.Empty);

		public int rttime => PlayerPrefs.GetInt("Login.refresh_token_time", 0);

		public string loginKey => PlayerPrefs.GetString("Login.login_key", string.Empty);
	}

	private static readonly ServerInfoView _serverInfo = new ServerInfoView();

	private static readonly AuthTokensView _authTokens = new AuthTokensView();

	public static ServerInfoView ServerInfo => _serverInfo;

	public static AuthTokensView AuthTokens => _authTokens;

	private static void SetStringIfChanged(string key, string value, string logName, bool hideValueInLog = false)
	{
		string text = PlayerPrefs.GetString(key, string.Empty);
		if (!(text == value))
		{
			if (hideValueInLog)
			{
				Log.Info("[AccountManager] " + logName + " updated");
			}
			else
			{
				Log.Info("[AccountManager] " + logName + " " + text + " -> " + value);
			}
			PlayerPrefs.SetString(key, value);
		}
	}

	private static void SetIntIfChanged(string key, int value, int defaultValue, string logName)
	{
		int num = PlayerPrefs.GetInt(key, defaultValue);
		if (num != value)
		{
			Log.Info($"[AccountManager] {logName} {num} -> {value}");
			PlayerPrefs.SetInt(key, value);
		}
	}

	private static void DeleteKeyWithLog(string key, string logName, bool hideValueInLog = false)
	{
		if (PlayerPrefs.HasKey(key))
		{
			if (hideValueInLog)
			{
				Log.Info("[AccountManager] " + logName + " cleared");
			}
			else
			{
				Log.Info("[AccountManager] " + logName + " cleared (" + key + ")");
			}
			PlayerPrefs.DeleteKey(key);
		}
	}

	public static void SetUID(string uid)
	{
		SetStringIfChanged("Setting.GAME_UID", uid, "UID");
	}

	public static void SetGMFlag(int gm)
	{
		SetIntIfChanged("Setting.GM_FLAG", gm, 0, "GM");
	}

	public static void SetServerNetInfo(string ip, int port, string zone, int connectionType)
	{
		SetStringIfChanged("SERVER_IP", ip, "IP");
		SetIntIfChanged("SERVER_PORT", port, 0, "Port");
		SetStringIfChanged("SERVER_ZONE", zone, "Zone");
		SetIntIfChanged("SERVER_CONNECTION_TYPE", connectionType, 0, "ConnectionType");
	}

	public static void SetAT(string token, int time = int.MaxValue)
	{
		SetStringIfChanged("Login.access_token", token, "AT", hideValueInLog: true);
		SetIntIfChanged("Login.access_token_time", time, 0, "ATTime");
	}

	public static void SetRT(string token, int time)
	{
		SetStringIfChanged("Login.refresh_token", token, "RT", hideValueInLog: true);
		SetIntIfChanged("Login.refresh_token_time", time, 0, "RTTime");
	}

	public static void SetLoginKey(string token)
	{
		SetStringIfChanged("Login.login_key", token, "LoginKey", hideValueInLog: true);
	}

	public static void ClearAT()
	{
		SetAT(string.Empty, 0);
	}

	public static void ClearRT()
	{
		SetRT(string.Empty, 0);
	}

	public static void ClearServerInfo()
	{
		DeleteKeyWithLog("Setting.GM_FLAG", "GM");
		DeleteKeyWithLog("Setting.GAME_UID", "UID");
		DeleteKeyWithLog("SERVER_IP", "IP");
		DeleteKeyWithLog("SERVER_PORT", "Port");
		DeleteKeyWithLog("SERVER_ZONE", "Zone");
		DeleteKeyWithLog("SERVER_CONNECTION_TYPE", "ConnectionType");
	}

	public static void ClearServerNetworkInfo()
	{
		DeleteKeyWithLog("SERVER_IP", "IP");
		DeleteKeyWithLog("SERVER_PORT", "Port");
		DeleteKeyWithLog("SERVER_CONNECTION_TYPE", "ConnectionType");
	}

	public static void ClearAuthInfo()
	{
		DeleteKeyWithLog("Login.access_token", "AT", hideValueInLog: true);
		DeleteKeyWithLog("Login.access_token_time", "ATTime");
		DeleteKeyWithLog("Login.refresh_token", "RT", hideValueInLog: true);
		DeleteKeyWithLog("Login.refresh_token_time", "RTTime");
		DeleteKeyWithLog("Login.login_key", "LoginKey", hideValueInLog: true);
	}

	public static void ClearLoginKey()
	{
		SetLoginKey(string.Empty);
	}

	public static void ClearAll()
	{
		ClearServerInfo();
		ClearAuthInfo();
		Save();
	}

	public static void Save()
	{
		PlayerPrefs.Save();
	}

	public static void ClearUID()
	{
		SetUID(string.Empty);
	}
}
