using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AccountCredentialManagerAuthTokensViewWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AccountCredentialManager.AuthTokensView);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 5, 0);
		Utils.RegisterFunc(L, -2, "at", _g_get_at);
		Utils.RegisterFunc(L, -2, "attime", _g_get_attime);
		Utils.RegisterFunc(L, -2, "rt", _g_get_rt);
		Utils.RegisterFunc(L, -2, "rttime", _g_get_rttime);
		Utils.RegisterFunc(L, -2, "loginKey", _g_get_loginKey);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
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
				AccountCredentialManager.AuthTokensView o = new AccountCredentialManager.AuthTokensView();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AccountCredentialManager.AuthTokensView constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_at(IntPtr L)
	{
		try
		{
			AccountCredentialManager.AuthTokensView authTokensView = (AccountCredentialManager.AuthTokensView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, authTokensView.at);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_attime(IntPtr L)
	{
		try
		{
			AccountCredentialManager.AuthTokensView authTokensView = (AccountCredentialManager.AuthTokensView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, authTokensView.attime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rt(IntPtr L)
	{
		try
		{
			AccountCredentialManager.AuthTokensView authTokensView = (AccountCredentialManager.AuthTokensView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, authTokensView.rt);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rttime(IntPtr L)
	{
		try
		{
			AccountCredentialManager.AuthTokensView authTokensView = (AccountCredentialManager.AuthTokensView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, authTokensView.rttime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loginKey(IntPtr L)
	{
		try
		{
			AccountCredentialManager.AuthTokensView authTokensView = (AccountCredentialManager.AuthTokensView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, authTokensView.loginKey);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
