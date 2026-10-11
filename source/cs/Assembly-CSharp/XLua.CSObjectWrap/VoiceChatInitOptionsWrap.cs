using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VoiceChatInitOptionsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(VoiceChatInitOptions);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 5, 5);
		Utils.RegisterFunc(L, -2, "SdkAppId", _g_get_SdkAppId);
		Utils.RegisterFunc(L, -2, "OpenId", _g_get_OpenId);
		Utils.RegisterFunc(L, -2, "AppScene", _g_get_AppScene);
		Utils.RegisterFunc(L, -2, "EnableSpeakerOnEnter", _g_get_EnableSpeakerOnEnter);
		Utils.RegisterFunc(L, -2, "EnableMicOnEnter", _g_get_EnableMicOnEnter);
		Utils.RegisterFunc(L, -1, "SdkAppId", _s_set_SdkAppId);
		Utils.RegisterFunc(L, -1, "OpenId", _s_set_OpenId);
		Utils.RegisterFunc(L, -1, "AppScene", _s_set_AppScene);
		Utils.RegisterFunc(L, -1, "EnableSpeakerOnEnter", _s_set_EnableSpeakerOnEnter);
		Utils.RegisterFunc(L, -1, "EnableMicOnEnter", _s_set_EnableMicOnEnter);
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
				VoiceChatInitOptions o = new VoiceChatInitOptions();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to VoiceChatInitOptions constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SdkAppId(IntPtr L)
	{
		try
		{
			VoiceChatInitOptions voiceChatInitOptions = (VoiceChatInitOptions)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatInitOptions.SdkAppId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OpenId(IntPtr L)
	{
		try
		{
			VoiceChatInitOptions voiceChatInitOptions = (VoiceChatInitOptions)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, voiceChatInitOptions.OpenId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AppScene(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatInitOptions voiceChatInitOptions = (VoiceChatInitOptions)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushVoiceChatAppScene(L, voiceChatInitOptions.AppScene);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EnableSpeakerOnEnter(IntPtr L)
	{
		try
		{
			VoiceChatInitOptions voiceChatInitOptions = (VoiceChatInitOptions)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, voiceChatInitOptions.EnableSpeakerOnEnter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EnableMicOnEnter(IntPtr L)
	{
		try
		{
			VoiceChatInitOptions voiceChatInitOptions = (VoiceChatInitOptions)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, voiceChatInitOptions.EnableMicOnEnter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SdkAppId(IntPtr L)
	{
		try
		{
			((VoiceChatInitOptions)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SdkAppId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OpenId(IntPtr L)
	{
		try
		{
			((VoiceChatInitOptions)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OpenId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_AppScene(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatInitOptions voiceChatInitOptions = (VoiceChatInitOptions)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VoiceChatAppScene val);
			voiceChatInitOptions.AppScene = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_EnableSpeakerOnEnter(IntPtr L)
	{
		try
		{
			((VoiceChatInitOptions)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnableSpeakerOnEnter = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_EnableMicOnEnter(IntPtr L)
	{
		try
		{
			((VoiceChatInitOptions)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnableMicOnEnter = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
