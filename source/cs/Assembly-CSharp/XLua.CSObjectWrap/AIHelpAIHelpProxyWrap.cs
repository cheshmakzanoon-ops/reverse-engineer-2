using System;
using AIHelp;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AIHelpAIHelpProxyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AIHelpProxy);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 12, 5, 4);
		Utils.RegisterFunc(L, -4, "SetAiHelpUnreadMsgCount", _m_SetAiHelpUnreadMsgCount_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetAiHelpUnreadMsgCount", _m_GetAiHelpUnreadMsgCount_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnAfterInitMessage", _m_OnAfterInitMessage_xlua_st_);
		Utils.RegisterFunc(L, -4, "CheckAiHelpSwitch", _m_CheckAiHelpSwitch_xlua_st_);
		Utils.RegisterFunc(L, -4, "Init", _m_Init_xlua_st_);
		Utils.RegisterFunc(L, -4, "Show", _m_Show_xlua_st_);
		Utils.RegisterFunc(L, -4, "CloseAiHelpUnreadCount", _m_CloseAiHelpUnreadCount_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetAIHelpDataList", _m_SetAIHelpDataList_xlua_st_);
		Utils.RegisterFunc(L, -4, "UpdateUserInfo", _m_UpdateUserInfo_xlua_st_);
		Utils.RegisterFunc(L, -4, "Logout", _m_Logout_xlua_st_);
		Utils.RegisterFunc(L, -4, "MapZendeskFieldsToAIHelpJson", _m_MapZendeskFieldsToAIHelpJson_xlua_st_);
		Utils.RegisterFunc(L, -2, "UnreadMsgCount", _g_get_UnreadMsgCount);
		Utils.RegisterFunc(L, -2, "CurrentEntranceId", _g_get_CurrentEntranceId);
		Utils.RegisterFunc(L, -2, "CurrentEntranceName", _g_get_CurrentEntranceName);
		Utils.RegisterFunc(L, -2, "MyOutgoingTicket", _g_get_MyOutgoingTicket);
		Utils.RegisterFunc(L, -2, "aIHelpDataTable", _g_get_aIHelpDataTable);
		Utils.RegisterFunc(L, -1, "CurrentEntranceId", _s_set_CurrentEntranceId);
		Utils.RegisterFunc(L, -1, "CurrentEntranceName", _s_set_CurrentEntranceName);
		Utils.RegisterFunc(L, -1, "MyOutgoingTicket", _s_set_MyOutgoingTicket);
		Utils.RegisterFunc(L, -1, "aIHelpDataTable", _s_set_aIHelpDataTable);
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
				AIHelpProxy o = new AIHelpProxy();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AIHelp.AIHelpProxy constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAiHelpUnreadMsgCount_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.SetAiHelpUnreadMsgCount(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAiHelpUnreadMsgCount_xlua_st_(IntPtr L)
	{
		try
		{
			int aiHelpUnreadMsgCount = AIHelpProxy.GetAiHelpUnreadMsgCount();
			Lua.xlua_pushinteger(L, aiHelpUnreadMsgCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAfterInitMessage_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.OnAfterInitMessage();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckAiHelpSwitch_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = AIHelpProxy.CheckAiHelpSwitch();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Show_xlua_st_(IntPtr L)
	{
		try
		{
			string entranceId = Lua.lua_tostring(L, 1);
			string welcomeMessage = Lua.lua_tostring(L, 2);
			AIHelpProxy.Show(entranceId, welcomeMessage);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CloseAiHelpUnreadCount_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.CloseAiHelpUnreadCount();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAIHelpDataList_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.SetAIHelpDataList();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateUserInfo_xlua_st_(IntPtr L)
	{
		try
		{
			string uid = Lua.lua_tostring(L, 1);
			string uname = Lua.lua_tostring(L, 2);
			string tag = Lua.lua_tostring(L, 3);
			string serverId = Lua.lua_tostring(L, 4);
			string json = Lua.lua_tostring(L, 5);
			AIHelpProxy.UpdateUserInfo(uid, uname, tag, serverId, json);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Logout_xlua_st_(IntPtr L)
	{
		try
		{
			AIHelpProxy.Logout();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MapZendeskFieldsToAIHelpJson_xlua_st_(IntPtr L)
	{
		try
		{
			string str = AIHelpProxy.MapZendeskFieldsToAIHelpJson();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UnreadMsgCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, AIHelpProxy.UnreadMsgCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentEntranceId(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, AIHelpProxy.CurrentEntranceId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurrentEntranceName(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, AIHelpProxy.CurrentEntranceName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MyOutgoingTicket(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, AIHelpProxy.MyOutgoingTicket);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_aIHelpDataTable(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, AIHelpProxy.aIHelpDataTable);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CurrentEntranceId(IntPtr L)
	{
		try
		{
			AIHelpProxy.CurrentEntranceId = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CurrentEntranceName(IntPtr L)
	{
		try
		{
			AIHelpProxy.CurrentEntranceName = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MyOutgoingTicket(IntPtr L)
	{
		try
		{
			AIHelpProxy.MyOutgoingTicket = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_aIHelpDataTable(IntPtr L)
	{
		try
		{
			AIHelpProxy.aIHelpDataTable = (AIHelpProxy.AIHelpDataTable)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(AIHelpProxy.AIHelpDataTable));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
