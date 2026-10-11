using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using BestHTTP;
using BestHTTP.Forms;
using GameFramework;
using GameKit.Base;
using Main.Scripts.Network;
using ProtoBufNet;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Util;
using UnityEngine.Networking;
using ZstdNet;

public class NetworkManager : IGameController, INetManager
{
	public class NetworkConfig
	{
		public string[] ServerUrlList { get; internal set; }

		public string FinalServerUrl { get; internal set; }

		public int ServerPort { get; internal set; }

		public string ZoneName { get; internal set; }

		public int ConnectionType { get; internal set; }

		public bool Validate()
		{
			if (ServerUrlList == null || ServerUrlList.Length == 0)
			{
				Log.Error("[NetworkManager]ServerUrlList cannot be null or empty");
				return false;
			}
			if (ServerPort <= 0)
			{
				Log.Error("[NetworkManager]ServerPort must be greater than 0");
				return false;
			}
			if (string.IsNullOrEmpty(ZoneName))
			{
				Log.Error("[NetworkManager]ZoneName cannot be null or empty");
			}
			if (ConnectionType != 0 && ConnectionType != 1)
			{
				Log.Error($"[NetworkManager]ConnectionType error:{ConnectionType}");
				return false;
			}
			bool flag = IsWsUrl(ServerUrlList);
			if (flag != IsWsPort(ServerPort) || flag != IsWsType(ConnectionType))
			{
				Log.Error(string.Format("[NetworkManager]Maybe type error? p:{0} ,t:{1} url:{2} ", ServerPort, ConnectionType, string.Join(",", ServerUrlList)));
			}
			return true;
		}

		public bool IsYace()
		{
			string finalServerUrl = FinalServerUrl;
			if (string.IsNullOrEmpty(finalServerUrl))
			{
				return false;
			}
			string[] array = finalServerUrl.Split(new char[1] { '-' });
			if (array.Length > 1)
			{
				return array[0].Equals("yace");
			}
			return false;
		}

		private bool IsWsUrl(string[] urlList)
		{
			if (urlList == null)
			{
				return false;
			}
			for (int i = 0; i < urlList.Length; i++)
			{
				if (!Regex.IsMatch(urlList[i], "/s\\d+$"))
				{
					return false;
				}
			}
			return true;
		}

		private bool IsWsPort(int port)
		{
			return port == 80;
		}

		private bool IsWsType(int connectType)
		{
			return connectType == 1;
		}
	}

	private NetworkConfig _config;

	public const bool ENCRYPT_SERVERLIST = true;

	private bool _isIfDownloadBattleReportDisableCache;

	private bool _isIfDownloadBattleReportDisAbleCacheInit;

	private FutureManager _futureManager;

	private MailBattleReportDownloadManager _battleReportDownloader;

	private INetProxy m_proxy;

	private List<INetProxy> m_allSelectProxy = new List<INetProxy>();

	public Action<string, string> OnConnectionEvent;

	public Action<string> OnConnectLostEvent;

	private const string k_ChooseLineCountKey = "CHOOSE_LINE_COUNT";

	private Stopwatch _flushProfiler = Stopwatch.StartNew();

	private const int FLUSH_TIME = 30000;

	private static string _finalGateServer;

	private const int DAY_SECONDS = 86400;

	private static string BattleReportFile = ".bin";

	private static bool _ForceUseOnlineCDN = false;

	private int _battleReportCancelIndex = -1;

	public NetworkConfig config => _config;

	public bool Logined { get; private set; }

	public bool IsConnected
	{
		get
		{
			if (m_proxy != null)
			{
				return m_proxy.IsConnected;
			}
			return false;
		}
	}

	public bool IsConnecting
	{
		get
		{
			if (m_proxy != null)
			{
				return m_proxy.IsConnecting;
			}
			return false;
		}
	}

	public bool IsPingPongTimeOut
	{
		get
		{
			if (m_proxy != null)
			{
				return m_proxy.IsPingPongTimeOut;
			}
			return false;
		}
	}

	public bool isNetworkValid
	{
		get
		{
			if (m_proxy != null)
			{
				return m_proxy.IsConnected;
			}
			return false;
		}
	}

	public LoginServerInfo[] ServerList { get; set; }

	private string ServerListHost
	{
		get
		{
			if (GameEntry.Sdk.GetPackageName().EndsWith("cn"))
			{
				return ConstURLConfig.onlineGateServer_CN;
			}
			if (string.IsNullOrEmpty(_finalGateServer))
			{
				Log.Error("NetworkManager::ServerListHost _finalGateServer is null !");
			}
			return _finalGateServer;
		}
	}

	private string ServerNoticeHost
	{
		get
		{
			if (GameEntry.Sdk.GetPackageName().EndsWith("cn"))
			{
				return ConstURLConfig.onlineGateServer_CN;
			}
			if (string.IsNullOrEmpty(_finalGateServer))
			{
				Log.Error("NetworkManager::ServerNoticeHost _finalGateServer is null !");
			}
			return _finalGateServer;
		}
	}

	public static bool ForceUseOnlineCDN
	{
		get
		{
			return _ForceUseOnlineCDN;
		}
		set
		{
			MailBattleReportDownloadManager.ForceUseOnlineCDN = value;
			_ForceUseOnlineCDN = value;
		}
	}

	public NetworkManager()
	{
		MessageFactory.Instance.InitMessageHandlers();
		_futureManager = new FutureManager();
		_battleReportDownloader = new MailBattleReportDownloadManager();
	}

	public void OnUpdate(float elapseSeconds)
	{
		UpdateSmartFoxClient();
		if (_battleReportDownloader.IsOpen())
		{
			_battleReportDownloader.OnUpdate();
		}
	}

	public void Shutdown()
	{
		Disconnect();
		ClearAllLine();
		_battleReportCancelIndex = -1;
		if (_battleReportDownloader.IsOpen())
		{
			_battleReportDownloader.Shutdown();
		}
		Log.Info("net work shut down");
	}

	public void ClearNetworkConfig()
	{
		_config = null;
	}

	public FutureManager getFutureManager()
	{
		return _futureManager;
	}

	public string getCurLine()
	{
		if (m_proxy == null)
		{
			return "direct";
		}
		return m_proxy.proxyHost;
	}

	public void Connect(string[] serverUrlList, int serverPort, string zoneName, int connectionType)
	{
		NetworkConfig networkConfig = new NetworkConfig
		{
			ServerUrlList = serverUrlList,
			ServerPort = serverPort,
			ZoneName = zoneName,
			ConnectionType = connectionType
		};
		if (networkConfig.Validate())
		{
			_config = networkConfig;
			Logined = false;
			ClearAllLine();
			ChooseLine();
			_futureManager.reset();
		}
	}

	public void Reconnect()
	{
		if (IsConnected)
		{
			Disconnect();
		}
		if (config == null)
		{
			Log.Error("[NetworkManager] config == null");
		}
		else
		{
			Connect(config.ServerUrlList, config.ServerPort, config.ZoneName, config.ConnectionType);
		}
	}

	private void ClearAllLine()
	{
		foreach (INetProxy item in m_allSelectProxy)
		{
			item.Disconnect();
		}
		m_allSelectProxy.Clear();
	}

	private void InitLine()
	{
		HashSet<string> hashSet = new HashSet<string>();
		int connectionType = _config.ConnectionType;
		int serverPort = _config.ServerPort;
		string[] serverUrlList = _config.ServerUrlList;
		HashSet<string> gameLineBlackList = GameEntry.GlobalData.gameLineBlackList;
		string zoneName = _config.ZoneName;
		ushort result = 0;
		if (!string.IsNullOrEmpty(zoneName) && zoneName.StartsWith("APS"))
		{
			ushort.TryParse(zoneName.Substring(3), out result);
		}
		if (gameLineBlackList.Count > 0)
		{
			HashSet<string> hashSet2 = new HashSet<string>();
			for (int i = 0; i < serverUrlList?.Length; i++)
			{
				hashSet2.Add(serverUrlList[i]);
			}
			if (hashSet2.Count == gameLineBlackList.Count)
			{
				gameLineBlackList.Clear();
			}
		}
		if (serverUrlList == null || serverUrlList.Length == 0)
		{
			Log.Error("NetworkManager::initLine urlList is null !");
			return;
		}
		string[] array = serverUrlList;
		foreach (string text in array)
		{
			if (!gameLineBlackList.Contains(text) && hashSet.Add(text))
			{
				INetProxy item = new NetRawProxy(text, text, serverPort, connectionType, this, result);
				Log.Info($"[Net] [NetworkManager] InitLine: {text}:{serverPort},s:{result}");
				PostEventLog.TrackMap("InitNetProxy", new Dictionary<string, object>
				{
					{
						"netName",
						text ?? ""
					},
					{
						"netUrl",
						text ?? ""
					},
					{
						"serverPort",
						$"{serverPort}"
					}
				});
				m_allSelectProxy.Add(item);
			}
		}
		m_allSelectProxy.Reverse();
	}

	private void ChooseLine()
	{
		int num = GameEntry.Setting.GetInt("CHOOSE_LINE_COUNT", 0);
		num++;
		GameEntry.Setting.SetInt("CHOOSE_LINE_COUNT", num);
		InitLine();
		foreach (INetProxy item in m_allSelectProxy)
		{
			item.Connect();
		}
	}

	public void Send(IRequest request)
	{
		if (m_proxy != null)
		{
			m_proxy.Send(request);
		}
		else
		{
			Log.Error("send message when m_proxy == null");
		}
	}

	public void SendLuaMessage(string msgId, byte[] sfsObjBinary)
	{
		SFSObject sFSObject = ((sfsObjBinary == null) ? SFSObject.NewInstance() : SFSObject.NewFromBinaryData(new ByteArray(sfsObjBinary)));
		int futureId = _futureManager.getFutureId();
		sFSObject.PutInt("_id", futureId);
		_futureManager.onSendRequest(futureId, msgId);
		if (GMSwitch.IsGM && GMSwitch.GetBool("DebugLogProtocolMsg"))
		{
			Log.Warning("[Msg][Send][Lua]<color=green>send msg <" + msgId + "> |</color> " + (sFSObject?.ToJson() ?? "NULL"));
		}
		if (m_proxy == null)
		{
			Log.Error("send message when m_proxy == null : " + msgId);
		}
		else
		{
			Send(new ExtensionRequest(msgId, sFSObject));
		}
	}

	public void Disconnect()
	{
		Logined = false;
		if (m_proxy != null)
		{
			m_proxy.Disconnect();
			m_proxy = null;
		}
	}

	public void KillConnection()
	{
	}

	public void SyncPingPong(int time = -1)
	{
		if (m_proxy != null)
		{
			m_proxy.SyncPingPong(time);
		}
	}

	public bool OnConnection(INetProxy proxy, BaseEvent e)
	{
		if ((bool)e.Params["success"])
		{
			if (m_proxy == null)
			{
				foreach (INetProxy item in m_allSelectProxy)
				{
					if (!item.proxyName.Equals(proxy.proxyName))
					{
						item.Disconnect();
					}
				}
				m_proxy = proxy;
				Log.Info("NetworkManager::On connection: {0}", proxy.proxyName);
				PostEventLog.TrackMap("ConnectNetSucceed", new Dictionary<string, object>
				{
					{
						"netName",
						proxy.proxyName ?? ""
					},
					{
						"netUrl",
						proxy.proxyHost ?? ""
					},
					{
						"serverPort",
						$"{proxy.proxyPort}"
					},
					{
						"resolveDns",
						$"{proxy.resolveDnsTime}"
					},
					{
						"connectTime",
						$"{proxy.connectTime}"
					},
					{
						"chooseLineCount",
						string.Format("{0}", GameEntry.Setting.GetInt("CHOOSE_LINE_COUNT", 0))
					}
				});
				if (!string.IsNullOrEmpty(GameEntry.Setting.GetString("CATCH_PUSH_TYPE", "")))
				{
					GameEntry.Setting.SetString("CATCH_PUSH_TYPE", "");
				}
				_config.FinalServerUrl = proxy.proxyHost;
				_config.ConnectionType = proxy.proxyConnectionType;
				AccountCredentialManager.SetServerNetInfo(proxy.proxyHost, proxy.proxyPort, _config.ZoneName, proxy.proxyConnectionType);
				OnConnectionEvent?.Invoke("E000", null);
				return true;
			}
			return false;
		}
		bool flag = true;
		Log.Info("NetworkManager On connection Error");
		PostEventLog.TrackMap("ConnectNetFailed", new Dictionary<string, object>
		{
			{
				"netName",
				proxy.proxyName ?? ""
			},
			{
				"netUrl",
				proxy.proxyHost ?? ""
			},
			{
				"serverPort",
				$"{proxy.proxyPort}"
			}
		});
		foreach (INetProxy item2 in m_allSelectProxy)
		{
			if (item2.Status != ProxyStatus.connectError)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			_ = e.Params["errorCode"];
			string text = (string)e.Params["errorMessage"];
			try
			{
				text = text.Split(new char[1] { '\n' })[0];
				int num = text.LastIndexOf(':');
				if (num != -1)
				{
					text = text.Substring(num);
				}
				Log.Error("Connect to server failed:{0}", text);
				PostEventLog.TrackMap("ConnectNetAllFailed", new Dictionary<string, object> { 
				{
					"detail",
					text ?? ""
				} });
				if (text.Contains("Network is unreachable"))
				{
					OnConnectionEvent?.Invoke("E105", text);
				}
				else
				{
					OnConnectionEvent?.Invoke("E101", text);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
			}
		}
		return false;
	}

	public void OnConnectionLost(string reason, INetProxy proxy)
	{
		Log.Error("Connection {0} was lost, Reason: {1} cur line {2}", proxy.proxyName, reason, getCurLine());
		if (m_proxy == null)
		{
			return;
		}
		if (m_proxy != null && !m_proxy.proxyName.Equals(proxy.proxyName))
		{
			Log.Info("line {0} lost not cur {1} line do nothing", proxy.proxyName, getCurLine());
		}
		else if (ApplicationLaunch.Instance.isReloading)
		{
			Log.Info("Stop reconnect because reloading");
		}
		else if (IsConnecting)
		{
			OnConnectionError("error connect lost");
		}
		else if (!GameEntry.GlobalData.isInBackGround && !GameEntry.GlobalData.pushOffWithQuitGame)
		{
			PostEventLog.Record("DISCONNECT_RETRY", "connection lost, reason: " + reason);
			Disconnect();
			OnConnectLostEvent?.Invoke(reason);
			if (ApplicationLaunch.Instance.Loading.currState != LoadingState.ConnectGame && ApplicationLaunch.Instance.Loading.currState != LoadingState.AccountSelect)
			{
				Log.Info("NetworkManager::Connection lost, reconnect");
				Log.Error("NetworkManager:Connection lost in state:{0}", ApplicationLaunch.Instance.Loading.currState);
				ApplicationLaunch.Instance.DisconnectRetry();
			}
		}
	}

	private void OnConnectionError(string error)
	{
		bool flag = true;
		if (m_proxy != null)
		{
			flag = true;
		}
		else
		{
			foreach (INetProxy item in m_allSelectProxy)
			{
				if (item.Status != ProxyStatus.connectError)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			Disconnect();
			int.TryParse(error, out var result);
			OnConnectionEvent?.Invoke("E101", "OnConnectionError");
			if (error == "error no net" || result < -200)
			{
				GameEntry.Event.Fire(EventId.Net_Connect_Error, error);
			}
			else
			{
				GameEntry.Event.Fire(EventId.Net_Connect_Error, "E124");
			}
		}
	}

	public void OnLogin(BaseEvent e)
	{
		Logined = MessageFactory.Instance.OnLogin(e);
	}

	private void OnPingPong(BaseEvent evt)
	{
		SyncPingPong();
	}

	public void OnLoginError(BaseEvent e)
	{
		Logined = false;
		MessageFactory.Instance.OnLogin(e);
	}

	public void OnExtensionResponse(string cmd, SFSObject so)
	{
	}

	public bool IsMainLine()
	{
		return true;
	}

	public void OnLogout(BaseEvent e)
	{
		Logined = false;
	}

	private void OnExtensionResponse(BaseEvent e)
	{
		SyncPingPong();
		MessageFactory.Instance.DispatchResponse(e);
	}

	private void OnPublicMessage(BaseEvent e)
	{
	}

	private void OnLogError(BaseEvent e)
	{
		string text = (string)e.Params["message"];
		Log.Error(text);
		PostEventLog.Record("SOCKET_ERROR", "socket error: " + text);
	}

	private void UpdateSmartFoxClient()
	{
		bool flag = _flushProfiler.ElapsedMilliseconds > 30000;
		if (flag)
		{
			_flushProfiler.Restart();
		}
		if (m_proxy != null)
		{
			m_proxy.UpdateSmartFoxClient();
			if (flag && m_proxy != null)
			{
				m_proxy.FlushProfiler(FlushNetProfilerToShuShu);
			}
		}
		else
		{
			for (int num = m_allSelectProxy.Count - 1; num >= 0; num--)
			{
				m_allSelectProxy[num]?.UpdateSmartFoxClient();
			}
		}
	}

	public static void FlushNetProfilerToShuShu(Dictionary<string, string> obj)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (KeyValuePair<string, string> item in TrackKey.keyToTrackStr)
		{
			if (obj.TryGetValue(item.Key, out var value))
			{
				dictionary.Add(item.Value, value);
			}
		}
		foreach (KeyValuePair<string, string> item2 in TrackKey.keyToTrackNum)
		{
			if (obj.TryGetValue(item2.Key, out var value2))
			{
				int result;
				int num = (int.TryParse(value2, out result) ? result : (-1));
				dictionary.Add(item2.Value, num);
			}
		}
		PostEventLog.TrackMap("CurNetProxy", dictionary);
	}

	public LoginServerInfo GetServerInfo(int id)
	{
		if (ServerList == null)
		{
			return new LoginServerInfo
			{
				id = id,
				ip = AccountCredentialManager.ServerInfo.ip,
				port = AccountCredentialManager.ServerInfo.port,
				zone = AccountCredentialManager.ServerInfo.zone
			};
		}
		for (int i = 0; i < ServerList.Length; i++)
		{
			if (ServerList[i].id == id)
			{
				return ServerList[i];
			}
		}
		return null;
	}

	public void SelectFinalGateServer(string host)
	{
		_finalGateServer = host;
		Log.Info("NetworkManager::SelectFinalGateServer::host:" + host);
	}

	public string GetCurFinalGateServer()
	{
		return _finalGateServer;
	}

	public void ClearFinalGateServer()
	{
		_finalGateServer = null;
		Log.Info("NetworkManager::ClearFinalGateServer");
	}

	public HTTPRequest GetCrossServerListRequest()
	{
		string uid = AccountCredentialManager.ServerInfo.uid;
		string finalGateServer = _finalGateServer;
		HTTPRequest hTTPRequest = new HTTPRequest(new Uri(finalGateServer + "/gameservice/getserverlist.php"), HTTPMethods.Post);
		string deviceUid = GameEntry.Device.GetDeviceUid();
		string deviceUid_Transcoding = GameEntry.Device.GetDeviceUid_Transcoding();
		hTTPRequest.AddField("uuid", deviceUid);
		hTTPRequest.AddField("airKey", deviceUid_Transcoding);
		hTTPRequest.AddField("loginFlag", "1");
		hTTPRequest.AddField("country", GameEntry.GlobalData.fromCountry);
		hTTPRequest.AddField("is3D", "1");
		hTTPRequest.AddField("lang", GameEntry.Localization.GetLanguageName());
		hTTPRequest.AddField("simOp", GameEntry.Sdk.GetSimOperator());
		hTTPRequest.AddField("platform", GameUtility.GetPlatformName());
		hTTPRequest.AddField("newServer", "1");
		hTTPRequest.AddField("isSimulator", GameEntry.Sdk.IsSimulator() ? "1" : "0");
		hTTPRequest.AddField("gameuid", uid);
		List<HTTPFieldData> formFields = hTTPRequest.GetFormFields();
		string arg = string.Empty;
		if (formFields != null)
		{
			arg = string.Join("&", formFields.Select((HTTPFieldData i) => i.Name + "=" + i.Text));
		}
		Log.Info("getcrossserverlist : {0}_{1}", finalGateServer + "/gameservice/getserverlist.php", arg);
		return hTTPRequest;
	}

	public HTTPRequest GetServerListRequest()
	{
		string zone = AccountCredentialManager.ServerInfo.zone;
		string uid = AccountCredentialManager.ServerInfo.uid;
		HTTPRequest hTTPRequest = new HTTPRequest(new Uri(ServerListHost + "/gameservice/getserverlist.php"), HTTPMethods.Post);
		string deviceUid = GameEntry.Device.GetDeviceUid();
		string deviceUid_Transcoding = GameEntry.Device.GetDeviceUid_Transcoding();
		if (CommonUtils.IsDebug())
		{
			if (!ApplicationLaunch.Instance.Loading.IsShowServerList)
			{
				hTTPRequest.AddField("loginFlag", "1");
				hTTPRequest.AddField("zone", zone);
				hTTPRequest.AddField("gameuid", uid);
				hTTPRequest.AddField("uuid", deviceUid);
				hTTPRequest.AddField("airKey", deviceUid_Transcoding);
			}
		}
		else
		{
			hTTPRequest.AddField("uuid", deviceUid);
			hTTPRequest.AddField("airKey", deviceUid_Transcoding);
			hTTPRequest.AddField("loginFlag", "1");
			hTTPRequest.AddField("country", GameEntry.GlobalData.fromCountry);
			hTTPRequest.AddField("is3D", "1");
			hTTPRequest.AddField("lang", GameEntry.Localization.GetLanguageName());
			hTTPRequest.AddField("simOp", GameEntry.Sdk.GetSimOperator());
			hTTPRequest.AddField("platform", GameUtility.GetPlatformName());
			hTTPRequest.AddField("isSimulator", GameEntry.Sdk.IsSimulator() ? "1" : "0");
			hTTPRequest.AddField("zone", zone);
			hTTPRequest.AddField("gameuid", uid);
			hTTPRequest.AddField("newServer", "1");
			hTTPRequest.AddField("openCountry", GameEntry.PayOrderData.GetStorefrontCode());
			Log.Info("[GSL] openCountry besthttp " + GameEntry.PayOrderData.GetStorefrontCode());
			string at = AccountCredentialManager.AuthTokens.at;
			long num = AccountCredentialManager.AuthTokens.attime;
			string rt = AccountCredentialManager.AuthTokens.rt;
			_ = AccountCredentialManager.AuthTokens.rttime;
			string loginKey = AccountCredentialManager.AuthTokens.loginKey;
			string text = "";
			if (!string.IsNullOrEmpty(loginKey))
			{
				text = "login";
				hTTPRequest.AddField("opt", "login");
				hTTPRequest.AddField("loginKey", loginKey);
			}
			else if (string.IsNullOrEmpty(uid))
			{
				if (!string.IsNullOrEmpty(loginKey))
				{
					text = "login";
					hTTPRequest.AddField("opt", "login");
					hTTPRequest.AddField("loginKey", loginKey);
				}
				else
				{
					text = "new";
					hTTPRequest.AddField("opt", "new");
					if (!string.IsNullOrEmpty(at) || !string.IsNullOrEmpty(rt))
					{
						Log.Error("[GetServerList] LocalDataException have at:" + at + " or rt:" + rt + " when gameuid is null");
					}
				}
			}
			else
			{
				long num2 = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
				bool flag = num > 0 && num2 - num >= 2592000;
				if (string.IsNullOrEmpty(rt))
				{
					text = "fix";
					hTTPRequest.AddField("opt", "fix");
				}
				else if (string.IsNullOrEmpty(at) || flag)
				{
					text = "refresh";
					hTTPRequest.AddField("opt", "refresh");
					hTTPRequest.AddField("rt", rt);
				}
			}
			Log.Info("[GetServerList] uid:" + uid + " zone:" + zone + " opt:" + text);
		}
		List<HTTPFieldData> formFields = hTTPRequest.GetFormFields();
		string requestData = string.Empty;
		if (formFields != null)
		{
			requestData = string.Join("&", formFields.Select((HTTPFieldData i) => i.Name + "=" + i.Text));
		}
		Tuple<string, string> gSLRequestParams = AESHelper.GetGSLRequestParams(GameEntry.Device.GetDeviceUid(), requestData);
		hTTPRequest.ClearForm();
		hTTPRequest.AddField("uuid", gSLRequestParams.Item1);
		hTTPRequest.AddField("data", gSLRequestParams.Item2);
		return hTTPRequest;
	}

	public UnityWebRequest GetServerList()
	{
		string zone = AccountCredentialManager.ServerInfo.zone;
		string uid = AccountCredentialManager.ServerInfo.uid;
		string deviceUid = GameEntry.Device.GetDeviceUid();
		string deviceUid_Transcoding = GameEntry.Device.GetDeviceUid_Transcoding();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (CommonUtils.IsDebug())
		{
			if (!ApplicationLaunch.Instance.Loading.IsShowServerList)
			{
				dictionary.Add("loginFlag", "1");
				dictionary.Add("zone", zone);
				dictionary.Add("gameuid", uid);
				dictionary.Add("uuid", deviceUid);
				dictionary.Add("airKey", deviceUid_Transcoding);
			}
		}
		else
		{
			dictionary.Add("uuid", deviceUid);
			dictionary.Add("airKey", deviceUid_Transcoding);
			dictionary.Add("loginFlag", "1");
			dictionary.Add("country", GameEntry.GlobalData.fromCountry);
			dictionary.Add("is3D", "1");
			dictionary.Add("lang", GameEntry.Localization.GetLanguageName());
			dictionary.Add("simOp", GameEntry.Sdk.GetSimOperator());
			dictionary.Add("platform", GameUtility.GetPlatformName());
			dictionary.Add("newServer", "1");
			dictionary.Add("isSimulator", GameEntry.Sdk.IsSimulator() ? "1" : "0");
			dictionary.Add("zone", zone);
			dictionary.Add("gameuid", uid);
			dictionary.Add("openCountry", GameEntry.PayOrderData.GetStorefrontCode());
			Log.Info("[GSL] openCountry UnityWebRequest " + GameEntry.PayOrderData.GetStorefrontCode());
			string at = AccountCredentialManager.AuthTokens.at;
			long num = AccountCredentialManager.AuthTokens.attime;
			string rt = AccountCredentialManager.AuthTokens.rt;
			string loginKey = AccountCredentialManager.AuthTokens.loginKey;
			if (!string.IsNullOrEmpty(loginKey))
			{
				dictionary.Add("opt", "login");
				dictionary.Add("loginKey", loginKey);
			}
			else if (string.IsNullOrEmpty(uid))
			{
				dictionary.Add("opt", "new");
				if (!string.IsNullOrEmpty(at) || !string.IsNullOrEmpty(rt))
				{
					Log.Error("[GetServerList] LocalDataException have at:" + at + " or rt:" + rt + " when gameuid is null");
				}
			}
			else
			{
				long num2 = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
				bool flag = num > 0 && num2 - num >= 2592000;
				if (string.IsNullOrEmpty(rt))
				{
					dictionary.Add("opt", "fix");
				}
				else if (string.IsNullOrEmpty(at) || flag)
				{
					dictionary.Add("opt", "refresh");
					dictionary.Add("rt", rt);
				}
			}
		}
		dictionary.TryGetValue("opt", out var value);
		Log.Info("[GetServerList] uid:" + uid + " zone:" + zone + " opt:" + value);
		string requestData = string.Join("&", dictionary.Select((KeyValuePair<string, string> i) => i.Key + "=" + i.Value));
		Tuple<string, string> gSLRequestParams = AESHelper.GetGSLRequestParams(GameEntry.Device.GetDeviceUid(), requestData);
		dictionary = new Dictionary<string, string>();
		dictionary.Add("uuid", gSLRequestParams.Item1);
		dictionary.Add("data", gSLRequestParams.Item2);
		UnityWebRequest unityWebRequest = UnityWebRequest.Post(ServerListHost + "/gameservice/getserverlist.php", dictionary);
		if (!WebRequestManager.DisableCertificateHandler() && ServerListHost.StartsWith("https://"))
		{
			unityWebRequest.certificateHandler = new WebRequestManager.WebRequestCertificateHandler();
		}
		unityWebRequest.SendWebRequest();
		return unityWebRequest;
	}

	public HTTPRequest GetCrossServerList(int targetServerId)
	{
		HTTPRequest hTTPRequest = new HTTPRequest(new Uri(ServerListHost + "/gameservice/getserverlist.php"), HTTPMethods.Post);
		hTTPRequest.AddField("zone", targetServerId.ToString());
		Log.Info(string.Format("getCrossServerList : {0}?zone={1}", ServerListHost + "/gameservice/getserverlist.php", targetServerId));
		return hTTPRequest;
	}

	public UnityWebRequest GetCrossServerListWebRequest(int targetServerId)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("zone", targetServerId.ToString());
		UnityWebRequest unityWebRequest = UnityWebRequest.Post(ServerListHost + "/gameservice/getserverlist.php", dictionary);
		if (!WebRequestManager.DisableCertificateHandler() && ServerListHost.StartsWith("https://"))
		{
			unityWebRequest.certificateHandler = new WebRequestManager.WebRequestCertificateHandler();
		}
		unityWebRequest.SendWebRequest();
		return unityWebRequest;
	}

	public UnityWebRequest GetServerNotice()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string serverNoticeHost = ServerNoticeHost;
		string zone = AccountCredentialManager.ServerInfo.zone;
		dictionary.Add("serverId", zone.StartsWith("APS") ? zone.Substring(3) : "");
		dictionary.Add("lang", GameEntry.Localization.GetLanguageName());
		UnityWebRequest unityWebRequest = UnityWebRequest.Post(serverNoticeHost + "/gameservice/getnotice.php", dictionary);
		if (!WebRequestManager.DisableCertificateHandler() && ServerNoticeHost.StartsWith("https://"))
		{
			unityWebRequest.certificateHandler = new WebRequestManager.WebRequestCertificateHandler();
		}
		unityWebRequest.SendWebRequest();
		string arg = string.Join("&", dictionary.Select((KeyValuePair<string, string> i) => i.Key + "=" + i.Value));
		Log.Info("getnotice : {0}_{1}", ServerNoticeHost, arg);
		return unityWebRequest;
	}

	public UnityWebRequest GetServerStatus()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("lang", GameEntry.Localization.GetLanguageName());
		dictionary.Add("sid", AccountCredentialManager.ServerInfo.zone.StartsWith("APS") ? AccountCredentialManager.ServerInfo.zone.Substring(3) : "");
		UnityWebRequest unityWebRequest = UnityWebRequest.Post(ServerListHost + "/gameservice/probe.php", dictionary);
		if (!WebRequestManager.DisableCertificateHandler() && ServerListHost.StartsWith("https://"))
		{
			unityWebRequest.certificateHandler = new WebRequestManager.WebRequestCertificateHandler();
		}
		unityWebRequest.SendWebRequest();
		return unityWebRequest;
	}

	public int GetPing()
	{
		if (m_proxy != null)
		{
			return m_proxy.GetPing();
		}
		return 0;
	}

	public int GetLastPingPongTime()
	{
		if (m_proxy != null)
		{
			return m_proxy.GetLastPingPongTime();
		}
		return 0;
	}

	public bool IsPressureTestServer()
	{
		if (_config == null)
		{
			return false;
		}
		return _config.IsYace();
	}

	public bool IsDebugConnectOnlineServer()
	{
		if (CommonUtils.IsDebug())
		{
			NetworkConfig networkConfig = _config;
			if (networkConfig == null)
			{
				return false;
			}
			return networkConfig.ServerPort > 17000;
		}
		return false;
	}

	public void CancelBattleReport(int cancelIndex)
	{
		if (_battleReportDownloader.IsOpen())
		{
			_battleReportDownloader.CancelBattleReport(cancelIndex);
		}
		else if (cancelIndex > _battleReportCancelIndex)
		{
			_battleReportCancelIndex = cancelIndex;
		}
	}

	public void GetBattleReport(string uuid, int cancelIndex, bool isFull, bool isAddressMode, string address)
	{
		if (_battleReportDownloader.IsOpen())
		{
			_battleReportDownloader.GetBattleReport(uuid, cancelIndex, isFull, isAddressMode, address);
			return;
		}
		if (string.IsNullOrWhiteSpace(uuid))
		{
			Log.Error("GetBattleReport reportId is empty");
			if (_battleReportCancelIndex < cancelIndex)
			{
				GameEntry.Lua.Call<byte[], int, string>("BattleReportUtil.Handle", null, cancelIndex, null);
			}
			return;
		}
		string battleReportHostByCurGroupType = NetworkURLConfig.GetBattleReportHostByCurGroupType(ForceUseOnlineCDN, isFull, isAddressMode, address);
		if (isAddressMode)
		{
			address = NetworkURLConfig.ModifyAddressStr(address);
		}
		string text = (isAddressMode ? (battleReportHostByCurGroupType + address) : (battleReportHostByCurGroupType + uuid + BattleReportFile));
		if (string.IsNullOrEmpty(text))
		{
			Log.Error("url is null ");
			return;
		}
		HTTPRequest httpRequest = new HTTPRequest(new Uri(text), HTTPMethods.Get);
		if (GameEntry.Network.IfDownloadBattleReportDisableCache())
		{
			httpRequest.DisableCache = true;
		}
		httpRequest.Callback = delegate(HTTPRequest req, HTTPResponse resp)
		{
			byte[] array = null;
			if (resp == null)
			{
				Log.Error("GetBattleReport null response, reportId : {0}", uuid);
			}
			else if (resp.IsSuccess)
			{
				Log.Info("GetBattleReport success, reportId : {0}", uuid);
				array = resp.Data;
			}
			else
			{
				Log.Error("GetBattleReport failed : {0} , reportId : {1}", resp.Message, uuid);
			}
			if (_battleReportCancelIndex < cancelIndex)
			{
				if (IfUseZstdCompress(resp, uuid))
				{
					array = ExecuteZstdDecompressor(array, uuid);
				}
				GameEntry.Lua.Call("BattleReportUtil.Handle", array, cancelIndex, uuid);
			}
			httpRequest.Dispose();
		};
		httpRequest.Send();
	}

	public void UpdateSrcServerId(ushort sid)
	{
		if (m_proxy != null)
		{
			m_proxy.UpdateSrcServerId(sid);
		}
	}

	public bool IfDownloadBattleReportDisableCache()
	{
		if (!_isIfDownloadBattleReportDisAbleCacheInit && GameEntry.Lua != null && GameEntry.Lua.HasGameStart)
		{
			_isIfDownloadBattleReportDisableCache = GameEntry.Lua.CallWithReturn<bool, string>("CSharpCallLuaInterface.CheckSwitch", "report_oss_client_disable_cache");
			_isIfDownloadBattleReportDisAbleCacheInit = true;
		}
		return _isIfDownloadBattleReportDisableCache;
	}

	public bool IfUseZstdCompress(HTTPResponse resp, string uuid = "")
	{
		bool flag = false;
		bool flag2 = false;
		string text = "";
		if (resp != null && resp.Headers != null && resp.Headers.ContainsKey("content-encoding"))
		{
			List<string> headerValues = resp.GetHeaderValues("content-encoding");
			flag2 = true;
			foreach (string item in headerValues)
			{
				text = text + item + ";";
				if (item.Equals("zstd", StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					break;
				}
			}
		}
		try
		{
			if (SDKManager.IS_UNITY_EDITOR() || GrayUtils.InGrayServer(3, 68))
			{
				string text2 = "";
				string text3 = "";
				if (resp != null && resp.Headers != null)
				{
					foreach (KeyValuePair<string, List<string>> header in resp.Headers)
					{
						text2 = text2 + header.Key + "=" + string.Join(",", header.Value) + ";";
					}
					text3 = resp.StatusCode.ToString();
				}
				Log.Warning("contentEncodingStr=={0} , useZstd={1}, uuid={2}, hasContentEncoding= {3}, respHeadersStr = {4}, statusCode = {5}", text, flag, uuid, flag2, text2, text3);
			}
		}
		catch (Exception ex)
		{
			Log.Error("ZSTD log fail , error : {0}", ex.Message);
		}
		return flag;
	}

	public byte[] ExecuteZstdDecompressor(byte[] data, string uuid)
	{
		if (data == null || data.Length == 0)
		{
			Log.Error("ExcuteZstdDecompressor data is null or empty, reportId : {0}", uuid);
			return null;
		}
		Decompressor decompressor = new Decompressor();
		byte[] result;
		try
		{
			result = decompressor.Unwrap(data);
		}
		catch (Exception ex)
		{
			Log.Error("GetBattleReport Decompressor failed, reportId : {0}, error: {1}", uuid, ex.Message);
			return null;
		}
		decompressor.Dispose();
		return result;
	}

	public void DownloadBattleReport(string uuid, string extra, bool isAddressMode, string address, bool immediate)
	{
		if (_battleReportDownloader.IsOpen())
		{
			_battleReportDownloader.DownloadBattleReport(uuid, extra, isAddressMode, address, immediate);
		}
		else
		{
			if (string.IsNullOrWhiteSpace(uuid))
			{
				return;
			}
			string battleReportDownloadHostByCurGroupType = NetworkURLConfig.GetBattleReportDownloadHostByCurGroupType(isAddressMode, address);
			if (isAddressMode)
			{
				address = NetworkURLConfig.ModifyAddressStr(address);
			}
			string uriString = (isAddressMode ? (battleReportDownloadHostByCurGroupType + address) : (battleReportDownloadHostByCurGroupType + uuid + BattleReportFile));
			HTTPRequest httpRequest = new HTTPRequest(new Uri(uriString), HTTPMethods.Get);
			if (GameEntry.Network.IfDownloadBattleReportDisableCache())
			{
				httpRequest.DisableCache = true;
			}
			httpRequest.Callback = delegate(HTTPRequest req, HTTPResponse resp)
			{
				byte[] array = null;
				int param = -1;
				if (resp != null)
				{
					if (resp.IsSuccess)
					{
						array = resp.Data;
					}
					else
					{
						param = resp.StatusCode;
					}
				}
				if (GameEntry.Lua != null && GameEntry.Lua.HasGameStart)
				{
					if (IfUseZstdCompress(resp, uuid))
					{
						array = ExecuteZstdDecompressor(array, uuid);
					}
					GameEntry.Lua.Call("BattleReportUtil.OnBattleReportDownload", param, array, uuid, extra);
				}
				httpRequest.Dispose();
			};
			httpRequest.Send();
		}
	}
}
