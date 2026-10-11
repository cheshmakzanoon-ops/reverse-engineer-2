using System;
using System.Collections.Generic;
using KWSVerification;
using Sfs2X.Entities.Data;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PrivacyFuncUtilWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PrivacyFuncUtil);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 42, 3, 3);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "ShowUIPrivacy", _m_ShowUIPrivacy);
		Utils.RegisterFunc(L, -3, "ShowUIPrivacyKR", _m_ShowUIPrivacyKR);
		Utils.RegisterFunc(L, -3, "CanShowDMA", _m_CanShowDMA);
		Utils.RegisterFunc(L, -3, "IsPrivacyConfirm", _m_IsPrivacyConfirm);
		Utils.RegisterFunc(L, -3, "SavePrivacyKey", _m_SavePrivacyKey);
		Utils.RegisterFunc(L, -3, "ShowPrivacy", _m_ShowPrivacy);
		Utils.RegisterFunc(L, -3, "HandleCoppaShowPrivacy", _m_HandleCoppaShowPrivacy);
		Utils.RegisterFunc(L, -3, "IsOldPrivacyConfirmed", _m_IsOldPrivacyConfirmed);
		Utils.RegisterFunc(L, -3, "GetAirKey", _m_GetAirKey);
		Utils.RegisterFunc(L, -3, "GetCountry", _m_GetCountry);
		Utils.RegisterFunc(L, -3, "GetLanguage", _m_GetLanguage);
		Utils.RegisterFunc(L, -3, "GetUid", _m_GetUid);
		Utils.RegisterFunc(L, -3, "GetZone", _m_GetZone);
		Utils.RegisterFunc(L, -3, "GetConfirmDelayDays", _m_GetConfirmDelayDays);
		Utils.RegisterFunc(L, -3, "SaveAccountData", _m_SaveAccountData);
		Utils.RegisterFunc(L, -3, "GetAccountData", _m_GetAccountData);
		Utils.RegisterFunc(L, -3, "SignCoppaNewPlayer", _m_SignCoppaNewPlayer);
		Utils.RegisterFunc(L, -3, "SetFromCountry", _m_SetFromCountry);
		Utils.RegisterFunc(L, -3, "UpdateGmCountry", _m_UpdateGmCountry);
		Utils.RegisterFunc(L, -3, "GetFinalConfirmTime", _m_GetFinalConfirmTime);
		Utils.RegisterFunc(L, -3, "GetCoppaAge", _m_GetCoppaAge);
		Utils.RegisterFunc(L, -3, "GetCoppaEmail", _m_GetCoppaEmail);
		Utils.RegisterFunc(L, -3, "IsClientSwitchOn", _m_IsClientSwitchOn);
		Utils.RegisterFunc(L, -3, "IsGrayDevice", _m_IsGrayDevice);
		Utils.RegisterFunc(L, -3, "IsFunctionOn", _m_IsFunctionOn);
		Utils.RegisterFunc(L, -3, "HandleUSPrivacy", _m_HandleUSPrivacy);
		Utils.RegisterFunc(L, -3, "FirstVerify_ProcessAccountInfoResponse", _m_FirstVerify_ProcessAccountInfoResponse);
		Utils.RegisterFunc(L, -3, "HandleCoppaState", _m_HandleCoppaState);
		Utils.RegisterFunc(L, -3, "FirstVerify_HandleAccountInfoError", _m_FirstVerify_HandleAccountInfoError);
		Utils.RegisterFunc(L, -3, "PushInit", _m_PushInit);
		Utils.RegisterFunc(L, -3, "Coppa_HandleSecondVerify_IpCountry", _m_Coppa_HandleSecondVerify_IpCountry);
		Utils.RegisterFunc(L, -3, "HandleCoppa_HandleSecondVerify_IpCountry", _m_HandleCoppa_HandleSecondVerify_IpCountry);
		Utils.RegisterFunc(L, -3, "ShowUIPrivacyUS", _m_ShowUIPrivacyUS);
		Utils.RegisterFunc(L, -3, "HandleShowUIPrivacyUS", _m_HandleShowUIPrivacyUS);
		Utils.RegisterFunc(L, -3, "IsUs", _m_IsUs);
		Utils.RegisterFunc(L, -3, "IsLimitUs", _m_IsLimitUs);
		Utils.RegisterFunc(L, -3, "LogEvent_CoppaStateTriggerIn", _m_LogEvent_CoppaStateTriggerIn);
		Utils.RegisterFunc(L, -3, "LogEvent_CoppaStateException", _m_LogEvent_CoppaStateException);
		Utils.RegisterFunc(L, -3, "LogEvent", _m_LogEvent);
		Utils.RegisterFunc(L, -3, "DebugLog", _m_DebugLog);
		Utils.RegisterFunc(L, -3, "DebugError", _m_DebugError);
		Utils.RegisterFunc(L, -2, "Country", _g_get_Country);
		Utils.RegisterFunc(L, -2, "IPCountry", _g_get_IPCountry);
		Utils.RegisterFunc(L, -2, "PrivacyKey", _g_get_PrivacyKey);
		Utils.RegisterFunc(L, -1, "Country", _s_set_Country);
		Utils.RegisterFunc(L, -1, "IPCountry", _s_set_IPCountry);
		Utils.RegisterFunc(L, -1, "PrivacyKey", _s_set_PrivacyKey);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				PrivacyFuncUtil o = new PrivacyFuncUtil();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PrivacyFuncUtil constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowUIPrivacy(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int mode = Lua.xlua_tointeger(L, 2);
			obj.ShowUIPrivacy(mode);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowUIPrivacyKR(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int mode = Lua.xlua_tointeger(L, 2);
			obj.ShowUIPrivacyKR(mode);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanShowDMA(IntPtr L)
	{
		try
		{
			int value = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CanShowDMA();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPrivacyConfirm(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsPrivacyConfirm();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SavePrivacyKey(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SavePrivacyKey();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowPrivacy(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int show = Lua.xlua_tointeger(L, 2);
			obj.ShowPrivacy(show);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleCoppaShowPrivacy(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int show = Lua.xlua_tointeger(L, 2);
			obj.HandleCoppaShowPrivacy(show);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOldPrivacyConfirmed(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsOldPrivacyConfirmed();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAirKey(IntPtr L)
	{
		try
		{
			string airKey = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetAirKey();
			Lua.lua_pushstring(L, airKey);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCountry(IntPtr L)
	{
		try
		{
			string country = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCountry();
			Lua.lua_pushstring(L, country);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLanguage(IntPtr L)
	{
		try
		{
			string language = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLanguage();
			Lua.lua_pushstring(L, language);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUid(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool considerNewPlayerFlag = Lua.lua_toboolean(L, 2);
				string uid = privacyFuncUtil.GetUid(considerNewPlayerFlag);
				Lua.lua_pushstring(L, uid);
				return 1;
			}
			if (num == 1)
			{
				string uid2 = privacyFuncUtil.GetUid();
				Lua.lua_pushstring(L, uid2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PrivacyFuncUtil.GetUid!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZone(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool considerNewPlayerFlag = Lua.lua_toboolean(L, 2);
				string zone = privacyFuncUtil.GetZone(considerNewPlayerFlag);
				Lua.lua_pushstring(L, zone);
				return 1;
			}
			if (num == 1)
			{
				string zone2 = privacyFuncUtil.GetZone();
				Lua.lua_pushstring(L, zone2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PrivacyFuncUtil.GetZone!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetConfirmDelayDays(IntPtr L)
	{
		try
		{
			float confirmDelayDays = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetConfirmDelayDays();
			Lua.lua_pushnumber(L, confirmDelayDays);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveAccountData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)objectTranslator.FastGetCSObj(L, 1);
			AccountData accountData = (AccountData)objectTranslator.GetObject(L, 2, typeof(AccountData));
			privacyFuncUtil.SaveAccountData(accountData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAccountData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AccountData accountData = ((PrivacyFuncUtil)objectTranslator.FastGetCSObj(L, 1)).GetAccountData();
			objectTranslator.Push(L, accountData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SignCoppaNewPlayer(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SignCoppaNewPlayer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFromCountry(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string fromCountry = Lua.lua_tostring(L, 2);
			obj.SetFromCountry(fromCountry);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGmCountry(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateGmCountry();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFinalConfirmTime(IntPtr L)
	{
		try
		{
			long finalConfirmTime = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetFinalConfirmTime();
			Lua.lua_pushint64(L, finalConfirmTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCoppaAge(IntPtr L)
	{
		try
		{
			int coppaAge = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCoppaAge();
			Lua.xlua_pushinteger(L, coppaAge);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCoppaEmail(IntPtr L)
	{
		try
		{
			string coppaEmail = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCoppaEmail();
			Lua.lua_pushstring(L, coppaEmail);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsClientSwitchOn(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsClientSwitchOn();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsGrayDevice(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsGrayDevice();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsFunctionOn(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsFunctionOn();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleUSPrivacy(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HandleUSPrivacy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FirstVerify_ProcessAccountInfoResponse(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)objectTranslator.FastGetCSObj(L, 1);
			AccountInfoResponse response = (AccountInfoResponse)objectTranslator.GetObject(L, 2, typeof(AccountInfoResponse));
			privacyFuncUtil.FirstVerify_ProcessAccountInfoResponse(response);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleCoppaState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)objectTranslator.FastGetCSObj(L, 1);
			AccountData accountData = (AccountData)objectTranslator.GetObject(L, 2, typeof(AccountData));
			bool value = privacyFuncUtil.HandleCoppaState(accountData);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FirstVerify_HandleAccountInfoError(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string error = Lua.lua_tostring(L, 2);
			obj.FirstVerify_HandleAccountInfoError(error);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PushInit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			privacyFuncUtil.PushInit(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Coppa_HandleSecondVerify_IpCountry(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Coppa_HandleSecondVerify_IpCountry();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleCoppa_HandleSecondVerify_IpCountry(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HandleCoppa_HandleSecondVerify_IpCountry();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowUIPrivacyUS(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int mode = Lua.xlua_tointeger(L, 2);
			obj.ShowUIPrivacyUS(mode);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleShowUIPrivacyUS(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int mode = Lua.xlua_tointeger(L, 2);
			obj.HandleShowUIPrivacyUS(mode);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUs(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsUs();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsLimitUs(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsLimitUs();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogEvent_CoppaStateTriggerIn(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LogEvent_CoppaStateTriggerIn();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogEvent_CoppaStateException(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string msg = Lua.lua_tostring(L, 2);
			obj.LogEvent_CoppaStateException(msg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogEvent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)objectTranslator.FastGetCSObj(L, 1);
			string eventName = Lua.lua_tostring(L, 2);
			Dictionary<string, object> additionalData = (Dictionary<string, object>)objectTranslator.GetObject(L, 3, typeof(Dictionary<string, object>));
			privacyFuncUtil.LogEvent(eventName, additionalData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugLog(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string message = Lua.lua_tostring(L, 2);
			obj.DebugLog(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugError(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil obj = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string message = Lua.lua_tostring(L, 2);
			obj.DebugError(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, PrivacyFuncUtil.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Country(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, privacyFuncUtil.Country);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IPCountry(IntPtr L)
	{
		try
		{
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, privacyFuncUtil.IPCountry);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PrivacyKey(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PrivacyFuncUtil privacyFuncUtil = (PrivacyFuncUtil)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, privacyFuncUtil.PrivacyKey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Country(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Country = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IPCountry(IntPtr L)
	{
		try
		{
			((PrivacyFuncUtil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IPCountry = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_PrivacyKey(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PrivacyFuncUtil)objectTranslator.FastGetCSObj(L, 1)).PrivacyKey = (Dictionary<string, string>)objectTranslator.GetObject(L, 2, typeof(Dictionary<string, string>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
