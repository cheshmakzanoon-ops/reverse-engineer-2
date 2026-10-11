using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ExternalCheckoutServiceWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ExternalCheckoutService);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 11, 0, 0);
		Utils.RegisterFunc(L, -4, "Open", _m_Open_xlua_st_);
		Utils.RegisterFunc(L, -4, "PrepareAndroidBillingApproval", _m_PrepareAndroidBillingApproval_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckAvailability", _m_CheckAvailability_xlua_st_);
		Utils.RegisterFunc(L, -4, "Close", _m_Close_xlua_st_);
		Utils.RegisterFunc(L, -4, "OpenPreparedUrl", _m_OpenPreparedUrl_xlua_st_);
		Utils.RegisterFunc(L, -4, "CancelPendingApproval", _m_CancelPendingApproval_xlua_st_);
		Utils.RegisterFunc(L, -4, "HandleNativeLaunchApproved", _m_HandleNativeLaunchApproved_xlua_st_);
		Utils.RegisterFunc(L, -4, "HandleNativeAvailability", _m_HandleNativeAvailability_xlua_st_);
		Utils.RegisterFunc(L, -4, "HandleNativeToken", _m_HandleNativeToken_xlua_st_);
		Utils.RegisterFunc(L, -4, "HandleNativeFailure", _m_HandleNativeFailure_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "ExternalCheckoutService does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Open_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = ExternalCheckoutService.Open((ExternalCheckoutRequest)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(ExternalCheckoutRequest)));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrepareAndroidBillingApproval_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<ExternalCheckoutProgram>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				objectTranslator.Get(L, 1, out ExternalCheckoutProgram v);
				bool preferWebView = Lua.lua_toboolean(L, 2);
				string storefrontCountryCode = Lua.lua_tostring(L, 3);
				bool value = ExternalCheckoutService.PrepareAndroidBillingApproval(v, preferWebView, storefrontCountryCode);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<ExternalCheckoutProgram>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				objectTranslator.Get(L, 1, out ExternalCheckoutProgram v2);
				bool preferWebView2 = Lua.lua_toboolean(L, 2);
				bool value2 = ExternalCheckoutService.PrepareAndroidBillingApproval(v2, preferWebView2);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<ExternalCheckoutProgram>(L, 1))
			{
				objectTranslator.Get(L, 1, out ExternalCheckoutProgram v3);
				bool value3 = ExternalCheckoutService.PrepareAndroidBillingApproval(v3);
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ExternalCheckoutService.PrepareAndroidBillingApproval!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckAvailability_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ExternalCheckoutProgram v);
			bool value = ExternalCheckoutService.CheckAvailability(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Close_xlua_st_(IntPtr L)
	{
		try
		{
			ExternalCheckoutService.Close();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OpenPreparedUrl_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = ExternalCheckoutService.OpenPreparedUrl(Lua.lua_tostring(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CancelPendingApproval_xlua_st_(IntPtr L)
	{
		try
		{
			ExternalCheckoutService.CancelPendingApproval();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeLaunchApproved_xlua_st_(IntPtr L)
	{
		try
		{
			ExternalCheckoutService.HandleNativeLaunchApproved(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeAvailability_xlua_st_(IntPtr L)
	{
		try
		{
			ExternalCheckoutService.HandleNativeAvailability(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeToken_xlua_st_(IntPtr L)
	{
		try
		{
			ExternalCheckoutService.HandleNativeToken(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeFailure_xlua_st_(IntPtr L)
	{
		try
		{
			ExternalCheckoutService.HandleNativeFailure(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
