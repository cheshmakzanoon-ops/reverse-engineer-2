using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VoiceChatUserUpdateWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(VoiceChatUserUpdate);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 4, 4);
		Utils.RegisterFunc(L, -2, "EventType", _g_get_EventType);
		Utils.RegisterFunc(L, -2, "UserIds", _g_get_UserIds);
		Utils.RegisterFunc(L, -2, "ChatRoomId", _g_get_ChatRoomId);
		Utils.RegisterFunc(L, -2, "VoiceRoomId", _g_get_VoiceRoomId);
		Utils.RegisterFunc(L, -1, "EventType", _s_set_EventType);
		Utils.RegisterFunc(L, -1, "UserIds", _s_set_UserIds);
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
				VoiceChatUserUpdate o = new VoiceChatUserUpdate();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to VoiceChatUserUpdate constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EventType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatUserUpdate voiceChatUserUpdate = (VoiceChatUserUpdate)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushVoiceChatUserEventType(L, voiceChatUserUpdate.EventType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UserIds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatUserUpdate voiceChatUserUpdate = (VoiceChatUserUpdate)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, voiceChatUserUpdate.UserIds);
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
			VoiceChatUserUpdate voiceChatUserUpdate = (VoiceChatUserUpdate)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatUserUpdate.ChatRoomId);
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
			VoiceChatUserUpdate voiceChatUserUpdate = (VoiceChatUserUpdate)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatUserUpdate.VoiceRoomId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_EventType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatUserUpdate voiceChatUserUpdate = (VoiceChatUserUpdate)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VoiceChatUserEventType val);
			voiceChatUserUpdate.EventType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_UserIds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((VoiceChatUserUpdate)objectTranslator.FastGetCSObj(L, 1)).UserIds = (string[])objectTranslator.GetObject(L, 2, typeof(string[]));
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
			((VoiceChatUserUpdate)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChatRoomId = Lua.lua_tostring(L, 2);
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
			((VoiceChatUserUpdate)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).VoiceRoomId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
