using System;
using KWSVerification;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PrivacyBrazilWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PrivacyBrazil);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 22, 0, 0);
		Utils.RegisterFunc(L, -3, "HandleCoppaShowPrivacy", _m_HandleCoppaShowPrivacy);
		Utils.RegisterFunc(L, -3, "IsClientSwitchOn", _m_IsClientSwitchOn);
		Utils.RegisterFunc(L, -3, "IsGrayDevice", _m_IsGrayDevice);
		Utils.RegisterFunc(L, -3, "IsFunctionOn", _m_IsFunctionOn);
		Utils.RegisterFunc(L, -3, "IsNonUsRegions", _m_IsNonUsRegions);
		Utils.RegisterFunc(L, -3, "HandleUSPrivacy", _m_HandleUSPrivacy);
		Utils.RegisterFunc(L, -3, "HandleUSPrivacy_UsingCachedAccountData", _m_HandleUSPrivacy_UsingCachedAccountData);
		Utils.RegisterFunc(L, -3, "HandleUSPrivacy_GetAccountInfo", _m_HandleUSPrivacy_GetAccountInfo);
		Utils.RegisterFunc(L, -3, "FirstVerify_ProcessAccountInfoResponse", _m_FirstVerify_ProcessAccountInfoResponse);
		Utils.RegisterFunc(L, -3, "HandleCoppaState", _m_HandleCoppaState);
		Utils.RegisterFunc(L, -3, "FirstVerify_HandleAccountInfoError", _m_FirstVerify_HandleAccountInfoError);
		Utils.RegisterFunc(L, -3, "Brazil_DoAgeVerification", _m_Brazil_DoAgeVerification);
		Utils.RegisterFunc(L, -3, "StopAgeVerificationPolling", _m_StopAgeVerificationPolling);
		Utils.RegisterFunc(L, -3, "Brazil_UpdateAccountInfo", _m_Brazil_UpdateAccountInfo);
		Utils.RegisterFunc(L, -3, "HandleCoppa_HandleSecondVerify_IpCountry", _m_HandleCoppa_HandleSecondVerify_IpCountry);
		Utils.RegisterFunc(L, -3, "HandleShowUIPrivacyUS", _m_HandleShowUIPrivacyUS);
		Utils.RegisterFunc(L, -3, "IsBrazil", _m_IsBrazil);
		Utils.RegisterFunc(L, -3, "IsLimitBrazil", _m_IsLimitBrazil);
		Utils.RegisterFunc(L, -3, "IsAgeVerified", _m_IsAgeVerified);
		Utils.RegisterFunc(L, -3, "HandleAgeVerification", _m_HandleAgeVerification);
		Utils.RegisterFunc(L, -3, "IsAgeVerifiedSwitchOn", _m_IsAgeVerifiedSwitchOn);
		Utils.RegisterFunc(L, -3, "OpenLimitWarningDialog", _m_OpenLimitWarningDialog);
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
				PrivacyBrazil o = new PrivacyBrazil();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PrivacyBrazil constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleCoppaShowPrivacy(IntPtr L)
	{
		try
		{
			PrivacyBrazil obj = (PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_IsClientSwitchOn(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsClientSwitchOn();
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
			bool value = ((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsGrayDevice();
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
			bool value = ((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsFunctionOn();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNonUsRegions(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsNonUsRegions();
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
			((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HandleUSPrivacy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleUSPrivacy_UsingCachedAccountData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PrivacyBrazil privacyBrazil = (PrivacyBrazil)objectTranslator.FastGetCSObj(L, 1);
			AccountData accountData = (AccountData)objectTranslator.GetObject(L, 2, typeof(AccountData));
			privacyBrazil.HandleUSPrivacy_UsingCachedAccountData(accountData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleUSPrivacy_GetAccountInfo(IntPtr L)
	{
		try
		{
			((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HandleUSPrivacy_GetAccountInfo();
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
			PrivacyBrazil privacyBrazil = (PrivacyBrazil)objectTranslator.FastGetCSObj(L, 1);
			AccountInfoResponse response = (AccountInfoResponse)objectTranslator.GetObject(L, 2, typeof(AccountInfoResponse));
			privacyBrazil.FirstVerify_ProcessAccountInfoResponse(response);
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
			PrivacyBrazil privacyBrazil = (PrivacyBrazil)objectTranslator.FastGetCSObj(L, 1);
			AccountData accountData = (AccountData)objectTranslator.GetObject(L, 2, typeof(AccountData));
			bool value = privacyBrazil.HandleCoppaState(accountData);
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
			PrivacyBrazil obj = (PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_Brazil_DoAgeVerification(IntPtr L)
	{
		try
		{
			((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Brazil_DoAgeVerification();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAgeVerificationPolling(IntPtr L)
	{
		try
		{
			((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAgeVerificationPolling();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Brazil_UpdateAccountInfo(IntPtr L)
	{
		try
		{
			((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Brazil_UpdateAccountInfo();
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
			((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HandleCoppa_HandleSecondVerify_IpCountry();
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
			PrivacyBrazil obj = (PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_IsBrazil(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBrazil();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsLimitBrazil(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsLimitBrazil();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAgeVerified(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAgeVerified();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleAgeVerification(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HandleAgeVerification();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAgeVerifiedSwitchOn(IntPtr L)
	{
		try
		{
			bool value = ((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsAgeVerifiedSwitchOn();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OpenLimitWarningDialog(IntPtr L)
	{
		try
		{
			((PrivacyBrazil)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OpenLimitWarningDialog();
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, PrivacyBrazil.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
