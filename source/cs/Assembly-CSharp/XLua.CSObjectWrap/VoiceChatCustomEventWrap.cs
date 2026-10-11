using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VoiceChatCustomEventWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(VoiceChatCustomEvent);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 5, 5);
		Utils.RegisterFunc(L, -2, "Type", _g_get_Type);
		Utils.RegisterFunc(L, -2, "SubType", _g_get_SubType);
		Utils.RegisterFunc(L, -2, "Data", _g_get_Data);
		Utils.RegisterFunc(L, -2, "ChatRoomId", _g_get_ChatRoomId);
		Utils.RegisterFunc(L, -2, "VoiceRoomId", _g_get_VoiceRoomId);
		Utils.RegisterFunc(L, -1, "Type", _s_set_Type);
		Utils.RegisterFunc(L, -1, "SubType", _s_set_SubType);
		Utils.RegisterFunc(L, -1, "Data", _s_set_Data);
		Utils.RegisterFunc(L, -1, "ChatRoomId", _s_set_ChatRoomId);
		Utils.RegisterFunc(L, -1, "VoiceRoomId", _s_set_VoiceRoomId);
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
				VoiceChatCustomEvent o = new VoiceChatCustomEvent();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to VoiceChatCustomEvent constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Type(IntPtr L)
	{
		try
		{
			VoiceChatCustomEvent voiceChatCustomEvent = (VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, voiceChatCustomEvent.Type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SubType(IntPtr L)
	{
		try
		{
			VoiceChatCustomEvent voiceChatCustomEvent = (VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, voiceChatCustomEvent.SubType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Data(IntPtr L)
	{
		try
		{
			VoiceChatCustomEvent voiceChatCustomEvent = (VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatCustomEvent.Data);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ChatRoomId(IntPtr L)
	{
		try
		{
			VoiceChatCustomEvent voiceChatCustomEvent = (VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatCustomEvent.ChatRoomId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_VoiceRoomId(IntPtr L)
	{
		try
		{
			VoiceChatCustomEvent voiceChatCustomEvent = (VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatCustomEvent.VoiceRoomId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Type(IntPtr L)
	{
		try
		{
			((VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Type = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SubType(IntPtr L)
	{
		try
		{
			((VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SubType = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Data(IntPtr L)
	{
		try
		{
			((VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Data = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ChatRoomId(IntPtr L)
	{
		try
		{
			((VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChatRoomId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_VoiceRoomId(IntPtr L)
	{
		try
		{
			((VoiceChatCustomEvent)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).VoiceRoomId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
