using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VoiceChatEnterRoomResultWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(VoiceChatEnterRoomResult);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 4, 4);
		Utils.RegisterFunc(L, -2, "Result", _g_get_Result);
		Utils.RegisterFunc(L, -2, "ErrorInfo", _g_get_ErrorInfo);
		Utils.RegisterFunc(L, -2, "ChatRoomId", _g_get_ChatRoomId);
		Utils.RegisterFunc(L, -2, "VoiceRoomId", _g_get_VoiceRoomId);
		Utils.RegisterFunc(L, -1, "Result", _s_set_Result);
		Utils.RegisterFunc(L, -1, "ErrorInfo", _s_set_ErrorInfo);
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
				VoiceChatEnterRoomResult o = new VoiceChatEnterRoomResult();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to VoiceChatEnterRoomResult constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Result(IntPtr L)
	{
		try
		{
			VoiceChatEnterRoomResult voiceChatEnterRoomResult = (VoiceChatEnterRoomResult)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, voiceChatEnterRoomResult.Result);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ErrorInfo(IntPtr L)
	{
		try
		{
			VoiceChatEnterRoomResult voiceChatEnterRoomResult = (VoiceChatEnterRoomResult)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatEnterRoomResult.ErrorInfo);
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
			VoiceChatEnterRoomResult voiceChatEnterRoomResult = (VoiceChatEnterRoomResult)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatEnterRoomResult.ChatRoomId);
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
			VoiceChatEnterRoomResult voiceChatEnterRoomResult = (VoiceChatEnterRoomResult)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatEnterRoomResult.VoiceRoomId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Result(IntPtr L)
	{
		try
		{
			((VoiceChatEnterRoomResult)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Result = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ErrorInfo(IntPtr L)
	{
		try
		{
			((VoiceChatEnterRoomResult)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ErrorInfo = Lua.lua_tostring(L, 2);
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
			((VoiceChatEnterRoomResult)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChatRoomId = Lua.lua_tostring(L, 2);
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
			((VoiceChatEnterRoomResult)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).VoiceRoomId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
