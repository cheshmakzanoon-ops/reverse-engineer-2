using System;
using System.Collections.Generic;
using GameFramework;
using KWSVerification;
using UnityEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public class PrivacyBrazil
{
	private static PrivacyBrazil _instance;

	private Coroutine ageVerificationPollingCoroutine;

	public static PrivacyBrazil Instance => _instance ?? (_instance = new PrivacyBrazil());

	private PrivacyFuncUtil Base => PrivacyFuncUtil.Instance;

	public void HandleCoppaShowPrivacy(int show)
	{
		AccountData accountData = Base.GetAccountData();
		if (accountData == null)
		{
			Base.DebugLog("ShowPrivacy accountData is null, directly HandleUSPrivacy");
			HandleUSPrivacy();
			return;
		}
		bool flag = IsOpenCoppaPrivacyView(accountData);
		bool flag2 = Base.IsPrivacyConfirm();
		Base.DebugLog($"ShowPrivacy isOpenCoppaPrivacyView:{flag} isNewPrivacyConfirmed:{flag2}");
		if (!flag && !flag2)
		{
			Base.ShowUIPrivacy(show);
		}
		else
		{
			HandleUSPrivacy();
		}
	}

	public bool IsClientSwitchOn()
	{
		string key = $"CLIENT_SWITCH_CACHE_ON_{14}";
		if (!PlayerPrefs.HasKey(key))
		{
			Base.DebugLog("ClientSwitch is ON by default as no client switch found.");
			return true;
		}
		bool flag = PlayerPrefs.GetInt(key, 0) == 1;
		Base.DebugLog("ClientSwitch is " + (flag ? "ON" : "OFF") + " .");
		return flag;
	}

	public bool IsGrayDevice()
	{
		bool flag = GrayUtils.IsGrayDevice(100);
		Base.DebugLog("GrayDevice is " + (flag ? "ON" : "OFF"));
		return flag;
	}

	public bool IsFunctionOn()
	{
		if (!IsClientSwitchOn())
		{
			return false;
		}
		if (!IsGrayDevice())
		{
			return false;
		}
		return true;
	}

	public bool IsNonUsRegions()
	{
		return PlayerPrefs.GetString("PRIVACY_COPPA_CURRENT_STATE", "non_us_regions") == "non_us_regions";
	}

	public void HandleUSPrivacy()
	{
		if (!IsFunctionOn())
		{
			Base.DebugLog("TotalFunction is off!");
			SetCoppaState("non_us_regions", showPrivacy: false);
			return;
		}
		Base.LogEvent_CoppaStateTriggerIn();
		AccountData accountData = Base.GetAccountData();
		if (accountData == null || !accountData.need_account)
		{
			HandleUSPrivacy_GetAccountInfo();
		}
		else if (IsAccountVerified(accountData))
		{
			HandleUSPrivacy_UsingCachedAccountData(accountData);
		}
		else
		{
			HandleUSPrivacy_GetAccountInfo();
		}
	}

	private bool IsAccountVerified(AccountData accountData)
	{
		if (accountData.accountInfo == null || string.IsNullOrEmpty(accountData.accountInfo.airKey))
		{
			return false;
		}
		if (!string.IsNullOrEmpty(accountData.age_verified))
		{
			if (accountData.age_verified == "1")
			{
				return true;
			}
			if (accountData.age_verified == "2")
			{
				return false;
			}
		}
		if (HasValidAdultAge(accountData.accountInfo))
		{
			return true;
		}
		return false;
	}

	private bool HasValidAdultAge(AccountInfo accountInfo)
	{
		if (string.IsNullOrEmpty(accountInfo.age) || accountInfo.age == "0")
		{
			return false;
		}
		if (int.TryParse(accountInfo.age, out var result))
		{
			return result > 17;
		}
		return false;
	}

	public void HandleUSPrivacy_UsingCachedAccountData(AccountData accountData)
	{
		string text = PlayerPrefs.GetString("PRIVACY_COPPA_ACCOUNT_INFO", "");
		Base.DebugLog("Using cached accountData " + text);
		HandleCoppaState(accountData);
	}

	public void HandleUSPrivacy_GetAccountInfo()
	{
		KWSVerificationManager.Instance.GetAccountInfo(Base.GetAirKey(), Base.GetCountry(), Base.GetLanguage(), 0, Base.GetUid(considerNewPlayerFlag: true), Base.GetZone(considerNewPlayerFlag: true), delegate(AccountInfoResponse response)
		{
			FirstVerify_ProcessAccountInfoResponse(response);
		}, delegate(string error)
		{
			FirstVerify_HandleAccountInfoError(error);
		});
	}

	public void FirstVerify_ProcessAccountInfoResponse(AccountInfoResponse response)
	{
		Base.DebugLog("Account info retrieved successfully");
		if (response?.data == null)
		{
			Base.DebugError("Response data is null");
			return;
		}
		Base.SaveAccountData(response.data);
		HandleCoppaState(response.data);
	}

	public bool HandleCoppaState(AccountData accountData)
	{
		if (accountData == null || !accountData.need_account)
		{
			Base.DebugLog("function switch off, non US regions");
			return SetCoppaState("non_us_regions", showPrivacy: false);
		}
		if (accountData.accountInfo == null || string.IsNullOrEmpty(accountData.accountInfo.airKey))
		{
			return HandleNoAccount();
		}
		return HandleYesAccount(accountData.accountInfo);
	}

	private bool HandleNoAccount()
	{
		Base.DebugLog("HandleNoAccount, need to create");
		if (PlayerPrefs.HasKey("PRIVACY_COPPA_NEW_PLAYER"))
		{
			Base.DebugLog("New player: UsUncertifiedNewPlayer, Create account ");
			return SetCoppaState("us_uncertified_new_player", showPrivacy: true);
		}
		Base.DebugLog("Old player: UsUncertifiedOldPlayer, Account exists, but not verified");
		KWSVerificationManager.Instance.CreateAccount(Base.GetAirKey(), Base.GetCountry(), Base.GetLanguage(), 0, needParent: false, Base.GetUid(considerNewPlayerFlag: true), Base.GetZone(considerNewPlayerFlag: true), Base.GetConfirmDelayDays(), delegate(CreateAccountResponse response)
		{
			Base.DebugLog("Old player: Account created successfully");
			Base.SaveAccountData(response.data);
			SetCoppaState("us_uncertified_old_player", showPrivacy: false);
		}, delegate(string error)
		{
			Base.DebugError("Old player: Failed to create account: " + error);
			SetCoppaState("non_us_regions", showPrivacy: false);
			Base.LogEvent_CoppaStateException(error);
		});
		return false;
	}

	private bool HandleYesAccount(AccountInfo accountInfo)
	{
		if (string.IsNullOrEmpty(accountInfo.age) || accountInfo.age == "0")
		{
			return HandleNoAge(accountInfo);
		}
		return HandleYesAge(accountInfo);
	}

	private bool HandleNoAge(AccountInfo accountInfo)
	{
		Base.DebugLog("No age information found, checking if user is new or existing");
		if (string.IsNullOrEmpty(accountInfo.finalConfirmTime) || accountInfo.finalConfirmTime == "0")
		{
			Base.DebugLog("No final confirm time , treating as new user");
			return HandleNoDelayDays(accountInfo);
		}
		if (Base.Country == "BR" || Base.IPCountry == "BR")
		{
			Base.DebugLog("yes final confirm time, treating as old user");
			return HandleYesDelayDays(accountInfo);
		}
		Base.DebugLog("old user but not non US regions or ip");
		return SetCoppaState("non_us_regions", showPrivacy: false);
	}

	private bool HandleYesAge(AccountInfo accountInfo)
	{
		Base.DebugLog("HandleYesAge, checking age verification status");
		if (!int.TryParse(accountInfo.age, out var result) || result < 3)
		{
			Base.DebugError("Invalid age: " + accountInfo.age);
			return false;
		}
		if (result > 11)
		{
			return HandleAdultUser();
		}
		return HandleChildUser(accountInfo, result);
	}

	private bool HandleNoDelayDays(AccountInfo accountInfo)
	{
		Base.DebugLog("No delay days, treating as new user");
		return SetCoppaState("us_uncertified_new_player", showPrivacy: true);
	}

	private bool HandleYesDelayDays(AccountInfo accountInfo)
	{
		Base.DebugLog("Has delay days, treating as old user");
		if (!long.TryParse(accountInfo.finalConfirmTime, out var result))
		{
			Base.DebugError(" HandleYesDelayDays Invalid finalConfirmTime");
			return false;
		}
		if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > result)
		{
			Base.DebugLog("Grace period expired, forcing verification");
			return SetCoppaState("us_uncertified_old_time_out", showPrivacy: true);
		}
		Base.DebugLog("Within grace period, allowing gameplay");
		return SetCoppaState("us_uncertified_old_player", showPrivacy: false);
	}

	private bool HandleAdultUser()
	{
		Base.DebugLog("Adult user, no parent verification needed");
		return SetCoppaState("us_certified_adult", showPrivacy: false);
	}

	private bool HandleChildUser(AccountInfo accountInfo, int age)
	{
		if (string.IsNullOrEmpty(accountInfo.email))
		{
			Base.DebugLog("Child user, email not sent yet");
			return SetCoppaState("us_uncertified_need_mail", showPrivacy: true);
		}
		return HandleChildWithEmail(accountInfo);
	}

	private bool HandleChildWithEmail(AccountInfo accountInfo)
	{
		if (accountInfo.parentConfirmed == "1")
		{
			Base.DebugLog("Parent approved privacy collection");
			return SetCoppaState("us_certified_children", showPrivacy: false);
		}
		Base.DebugLog("Parent confirmation pending");
		return SetCoppaState("us_uncertified_mail_pending", showPrivacy: true);
	}

	private bool SetCoppaState(string state, bool showPrivacy)
	{
		PlayerPrefs.SetString("PRIVACY_COPPA_CURRENT_STATE", state);
		if (showPrivacy)
		{
			UIPrivacyBrazilView.Instance.OpenPrivacyView();
		}
		else
		{
			GameEntry.Event.Fire(EventId.UIPrivacy_Confirm);
		}
		return showPrivacy;
	}

	public void FirstVerify_HandleAccountInfoError(string error)
	{
		Base.DebugError("Failed to get account info: " + error);
		SetCoppaState("non_us_regions", showPrivacy: false);
		Base.LogEvent_CoppaStateException(error);
	}

	public void Brazil_DoAgeVerification()
	{
		MyLog("=== Brazil_DoAgeVerification START ===");
		Base.LogEvent("BrazilAgeVerified", new Dictionary<string, object> { { "i_common_num", 0 } });
		KWSVerificationManager.Instance.StartAgeVerification(Base.GetAirKey(), Base.GetCountry(), "", delegate(AgeVerifyResponse response)
		{
			MyLog("=== StartAgeVerification onSuccess TRIGGERED ===");
			if (response == null)
			{
				MyLog("CRITICAL: response is NULL");
				SetCoppaState("non_us_regions", showPrivacy: false);
			}
			else if (string.IsNullOrEmpty(response.url))
			{
				MyLog("CRITICAL:  response.url is empty or null");
				SetCoppaState("non_us_regions", showPrivacy: false);
			}
			else
			{
				MyLog("Open URL: " + response.url);
				try
				{
					SDKManager.OpenURL(response.url);
					MyLog("SDKManager.OpenURL called successfully");
				}
				catch (Exception ex)
				{
					MyLog("SDKManager.OpenURL EXCEPTION: " + ex.Message + "\n" + ex.StackTrace);
					SetCoppaState("non_us_regions", showPrivacy: false);
					return;
				}
				MyLog("Start StartAgeVerificationPolling");
				StopAgeVerificationPolling();
				ageVerificationPollingCoroutine = KWSVerificationManager.Instance.StartAgeVerificationPolling(Base.GetAirKey(), Base.GetCountry(), Base.GetLanguage(), Base.GetUid(considerNewPlayerFlag: true), Base.GetZone(considerNewPlayerFlag: true), delegate(AccountInfoResponse accountInfoResponse)
				{
					MyLog("=== Age verification VERIFIED ===");
					Base.SaveAccountData(accountInfoResponse.data);
					Base.LogEvent("BrazilAgeVerified", new Dictionary<string, object> { 
					{
						"i_common_num",
						accountInfoResponse.data.age_verified
					} });
					GameEntry.Event.Fire(EventId.UIPrivacy_Confirm);
				}, delegate(string error)
				{
					MyLog("=== Age verification FAILED ===");
					Base.DebugError("Polling Error: " + error);
					SetCoppaState("non_us_regions", showPrivacy: false);
					UIUtils.ShowTips("kid_verification_failed", 3f);
				}, 600);
			}
		}, delegate(string error)
		{
			MyLog("=== StartAgeVerification onError TRIGGERED ===");
			SetCoppaState("non_us_regions", showPrivacy: false);
			Base.DebugError("验证失败: " + error);
		});
	}

	private void MyLog(string message)
	{
		Log.Info("[COPPA] " + Base.GetAirKey() + " = " + message);
	}

	public void StopAgeVerificationPolling()
	{
		Base.DebugLog("停止 年龄认证轮询");
		if (ageVerificationPollingCoroutine != null)
		{
			KWSVerificationManager.Instance.StopAgeVerificationPolling(ageVerificationPollingCoroutine);
			ageVerificationPollingCoroutine = null;
		}
	}

	public void Brazil_UpdateAccountInfo()
	{
		KWSVerificationManager.Instance.GetAccountInfo(Base.GetAirKey(), Base.GetCountry(), Base.GetLanguage(), 0, Base.GetUid(considerNewPlayerFlag: true), Base.GetZone(considerNewPlayerFlag: true), delegate(AccountInfoResponse response)
		{
			Base.DebugLog("Brazil_UpdateAccountInfo successful!");
			Base.SaveAccountData(response.data);
			GameEntry.Event.Fire(EventId.UIPrivacy_Confirm);
		}, delegate(string error)
		{
			SetCoppaState("non_us_regions", showPrivacy: false);
			Base.DebugLog("Failed to get account info: " + error);
		});
	}

	public void HandleCoppa_HandleSecondVerify_IpCountry()
	{
		if (!(Base.Country == "BR") && Base.IPCountry == "BR")
		{
			PlayerPrefs.SetString("PRIVACY_COPPA_OPEN_FROM", "Parkour");
			HandleUSPrivacy();
		}
	}

	public void HandleShowUIPrivacyUS(int mode)
	{
		if (!IsFunctionOn())
		{
			Base.DebugLog("TotalFunction is off!");
			return;
		}
		AccountData accountData = Base.GetAccountData();
		if (accountData != null && accountData.accountInfo != null && !HandleCoppaState(accountData))
		{
			UIPrivacyBrazilView.Instance.OpenPrivacyView();
		}
	}

	public bool IsBrazil()
	{
		return Base.GetCountry() == "BR";
	}

	public bool IsLimitBrazil()
	{
		if (!IsFunctionOn())
		{
			return false;
		}
		if (IsNonUsRegions())
		{
			return false;
		}
		switch (PlayerPrefs.GetInt("PRIVACY_COPPA_AGE_VERIFIED", 0))
		{
		case 1:
			return false;
		case 2:
			return true;
		default:
		{
			AccountData accountData = Base.GetAccountData();
			if (accountData != null && accountData.need_account && !string.IsNullOrEmpty(accountData.age_verified) && (accountData.age_verified == "1" || accountData.age_verified == "2"))
			{
				if (accountData.age_verified == "1")
				{
					return false;
				}
				if (accountData.age_verified == "2")
				{
					return true;
				}
			}
			switch (PlayerPrefs.GetInt("PRIVACY_COPPA_ACCOUNT_STATE", 0))
			{
			case 1:
				return false;
			case 2:
				return true;
			default:
			{
				if (accountData != null && accountData.need_account && accountData.accountInfo != null && !string.IsNullOrEmpty(accountData.accountInfo.airKey) && !string.IsNullOrEmpty(accountData.accountInfo.age) && accountData.accountInfo.age != "0" && int.TryParse(accountData.accountInfo.age, out var result) && result <= 17)
				{
					return true;
				}
				return false;
			}
			}
		}
		}
	}

	public bool IsAgeVerified()
	{
		int num = PlayerPrefs.GetInt("PRIVACY_COPPA_AGE_VERIFIED", 0);
		if (num == 1 || num == 2)
		{
			return true;
		}
		AccountData accountData = Base.GetAccountData();
		if (accountData != null && accountData.need_account && accountData.accountInfo != null && !string.IsNullOrEmpty(accountData.age_verified) && (accountData.age_verified == "1" || accountData.age_verified == "2"))
		{
			return true;
		}
		return false;
	}

	public bool HandleAgeVerification()
	{
		if (!IsFunctionOn())
		{
			return true;
		}
		if (IsNonUsRegions())
		{
			return true;
		}
		switch (PlayerPrefs.GetInt("PRIVACY_COPPA_AGE_VERIFIED", 0))
		{
		case 1:
			return true;
		case 2:
			OpenLimitWarningDialog();
			return false;
		default:
		{
			AccountData accountData = Base.GetAccountData();
			if (accountData != null && accountData.need_account && accountData.accountInfo != null && !string.IsNullOrEmpty(accountData.age_verified) && (accountData.age_verified == "1" || accountData.age_verified == "2"))
			{
				if (accountData.age_verified == "1")
				{
					return true;
				}
				if (accountData.age_verified == "2")
				{
					OpenLimitWarningDialog();
					return false;
				}
				return false;
			}
			if (IsAgeVerifiedSwitchOn())
			{
				GameEntry.Lua.UIManager.OpenWindow("UIBrazilAgeVerify");
				return false;
			}
			return true;
		}
		}
	}

	public bool IsAgeVerifiedSwitchOn()
	{
		if (!IsFunctionOn())
		{
			return false;
		}
		if (IsNonUsRegions())
		{
			return false;
		}
		bool flag = GameEntry.Data?.Player?.CheckSwitch("br_ecad_ageverify_open", defaultVal: false) ?? false;
		Base.DebugLog($"switchOn = {flag}");
		return flag;
	}

	public void OpenLimitWarningDialog()
	{
		GameEntry.Lua.UIManager.OpenWindow("UICoppaBrazil");
	}

	private bool IsOpenCoppaPrivacyView(AccountData accountData)
	{
		if (accountData == null || !accountData.need_account)
		{
			return false;
		}
		if (accountData.accountInfo == null || string.IsNullOrEmpty(accountData.accountInfo.airKey))
		{
			if (PlayerPrefs.HasKey("PRIVACY_COPPA_NEW_PLAYER"))
			{
				return true;
			}
			return false;
		}
		AccountInfo accountInfo = accountData.accountInfo;
		if (string.IsNullOrEmpty(accountInfo.age) || accountInfo.age == "0")
		{
			if (string.IsNullOrEmpty(accountInfo.finalConfirmTime) || accountInfo.finalConfirmTime == "0")
			{
				return true;
			}
			if (Base.Country == "BR" || Base.IPCountry == "BR")
			{
				if (!long.TryParse(accountInfo.finalConfirmTime, out var result))
				{
					return false;
				}
				if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > result)
				{
					return true;
				}
				return false;
			}
			return false;
		}
		if (!int.TryParse(accountInfo.age, out var result2) || result2 < 3)
		{
			return false;
		}
		if (result2 > 11)
		{
			return false;
		}
		if (string.IsNullOrEmpty(accountInfo.email))
		{
			return true;
		}
		if (accountInfo.parentConfirmed == "1")
		{
			return false;
		}
		return true;
	}
}
