using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AccountCredentialManagerServerInfoViewWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AccountCredentialManager.ServerInfoView);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 6, 0);
		Utils.RegisterFunc(L, -2, "uid", _g_get_uid);
		Utils.RegisterFunc(L, -2, "gm", _g_get_gm);
		Utils.RegisterFunc(L, -2, "ip", _g_get_ip);
		Utils.RegisterFunc(L, -2, "port", _g_get_port);
		Utils.RegisterFunc(L, -2, "zone", _g_get_zone);
		Utils.RegisterFunc(L, -2, "connectionType", _g_get_connectionType);
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
				AccountCredentialManager.ServerInfoView o = new AccountCredentialManager.ServerInfoView();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AccountCredentialManager.ServerInfoView constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uid(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ServerInfoView serverInfoView = (AccountCredentialManager.ServerInfoView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, serverInfoView.uid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gm(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ServerInfoView serverInfoView = (AccountCredentialManager.ServerInfoView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, serverInfoView.gm);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ip(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ServerInfoView serverInfoView = (AccountCredentialManager.ServerInfoView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, serverInfoView.ip);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_port(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ServerInfoView serverInfoView = (AccountCredentialManager.ServerInfoView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, serverInfoView.port);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_zone(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ServerInfoView serverInfoView = (AccountCredentialManager.ServerInfoView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, serverInfoView.zone);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_connectionType(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ServerInfoView serverInfoView = (AccountCredentialManager.ServerInfoView)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, serverInfoView.connectionType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
