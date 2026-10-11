using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AccountCredentialManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AccountCredentialManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 16, 2, 0);
		Utils.RegisterFunc(L, -4, "SetUID", _m_SetUID_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGMFlag", _m_SetGMFlag_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetServerNetInfo", _m_SetServerNetInfo_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetAT", _m_SetAT_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetRT", _m_SetRT_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetLoginKey", _m_SetLoginKey_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearAT", _m_ClearAT_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearRT", _m_ClearRT_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearServerInfo", _m_ClearServerInfo_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearServerNetworkInfo", _m_ClearServerNetworkInfo_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearAuthInfo", _m_ClearAuthInfo_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearLoginKey", _m_ClearLoginKey_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearAll", _m_ClearAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "Save", _m_Save_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearUID", _m_ClearUID_xlua_st_);
		Utils.RegisterFunc(L, -2, "ServerInfo", _g_get_ServerInfo);
		Utils.RegisterFunc(L, -2, "AuthTokens", _g_get_AuthTokens);
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
				AccountCredentialManager o = new AccountCredentialManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AccountCredentialManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetUID_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.SetUID(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGMFlag_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.SetGMFlag(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetServerNetInfo_xlua_st_(IntPtr L)
	{
		try
		{
			string ip = Lua.lua_tostring(L, 1);
			int port = Lua.xlua_tointeger(L, 2);
			string zone = Lua.lua_tostring(L, 3);
			int connectionType = Lua.xlua_tointeger(L, 4);
			AccountCredentialManager.SetServerNetInfo(ip, port, zone, connectionType);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAT_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				string token = Lua.lua_tostring(L, 1);
				int time = Lua.xlua_tointeger(L, 2);
				AccountCredentialManager.SetAT(token, time);
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				AccountCredentialManager.SetAT(Lua.lua_tostring(L, 1));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AccountCredentialManager.SetAT!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRT_xlua_st_(IntPtr L)
	{
		try
		{
			string token = Lua.lua_tostring(L, 1);
			int time = Lua.xlua_tointeger(L, 2);
			AccountCredentialManager.SetRT(token, time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLoginKey_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.SetLoginKey(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAT_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ClearAT();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearRT_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ClearRT();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearServerInfo_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ClearServerInfo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearServerNetworkInfo_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ClearServerNetworkInfo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAuthInfo_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ClearAuthInfo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearLoginKey_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ClearLoginKey();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAll_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ClearAll();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Save_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.Save();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearUID_xlua_st_(IntPtr L)
	{
		try
		{
			AccountCredentialManager.ClearUID();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ServerInfo(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, AccountCredentialManager.ServerInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AuthTokens(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, AccountCredentialManager.AuthTokens);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
