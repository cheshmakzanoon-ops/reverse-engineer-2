using System;
using UnityEngine.Video;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class RawImageVideoPlayerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RawImageVideoPlayer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 9, 9);
		Utils.RegisterFunc(L, -3, "LoadVideo", _m_LoadVideo);
		Utils.RegisterFunc(L, -3, "PauseVideo", _m_PauseVideo);
		Utils.RegisterFunc(L, -3, "StopVideo", _m_StopVideo);
		Utils.RegisterFunc(L, -2, "videoPath", _g_get_videoPath);
		Utils.RegisterFunc(L, -2, "width", _g_get_width);
		Utils.RegisterFunc(L, -2, "height", _g_get_height);
		Utils.RegisterFunc(L, -2, "playOnAwake", _g_get_playOnAwake);
		Utils.RegisterFunc(L, -2, "waitForFirstFrame", _g_get_waitForFirstFrame);
		Utils.RegisterFunc(L, -2, "loop", _g_get_loop);
		Utils.RegisterFunc(L, -2, "skipOnDrop", _g_get_skipOnDrop);
		Utils.RegisterFunc(L, -2, "audioOutputMode", _g_get_audioOutputMode);
		Utils.RegisterFunc(L, -2, "aspectRatio", _g_get_aspectRatio);
		Utils.RegisterFunc(L, -1, "videoPath", _s_set_videoPath);
		Utils.RegisterFunc(L, -1, "width", _s_set_width);
		Utils.RegisterFunc(L, -1, "height", _s_set_height);
		Utils.RegisterFunc(L, -1, "playOnAwake", _s_set_playOnAwake);
		Utils.RegisterFunc(L, -1, "waitForFirstFrame", _s_set_waitForFirstFrame);
		Utils.RegisterFunc(L, -1, "loop", _s_set_loop);
		Utils.RegisterFunc(L, -1, "skipOnDrop", _s_set_skipOnDrop);
		Utils.RegisterFunc(L, -1, "audioOutputMode", _s_set_audioOutputMode);
		Utils.RegisterFunc(L, -1, "aspectRatio", _s_set_aspectRatio);
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
				RawImageVideoPlayer o = new RawImageVideoPlayer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to RawImageVideoPlayer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadVideo(IntPtr L)
	{
		try
		{
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string newVideoPath = Lua.lua_tostring(L, 2);
				rawImageVideoPlayer.LoadVideo(newVideoPath);
				return 0;
			}
			if (num == 1)
			{
				rawImageVideoPlayer.LoadVideo();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to RawImageVideoPlayer.LoadVideo!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PauseVideo(IntPtr L)
	{
		try
		{
			((RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PauseVideo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopVideo(IntPtr L)
	{
		try
		{
			((RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopVideo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_videoPath(IntPtr L)
	{
		try
		{
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, rawImageVideoPlayer.videoPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_width(IntPtr L)
	{
		try
		{
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rawImageVideoPlayer.width);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_height(IntPtr L)
	{
		try
		{
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, rawImageVideoPlayer.height);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playOnAwake(IntPtr L)
	{
		try
		{
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, rawImageVideoPlayer.playOnAwake);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_waitForFirstFrame(IntPtr L)
	{
		try
		{
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, rawImageVideoPlayer.waitForFirstFrame);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loop(IntPtr L)
	{
		try
		{
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, rawImageVideoPlayer.loop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skipOnDrop(IntPtr L)
	{
		try
		{
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, rawImageVideoPlayer.skipOnDrop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_audioOutputMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rawImageVideoPlayer.audioOutputMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_aspectRatio(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, rawImageVideoPlayer.aspectRatio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_videoPath(IntPtr L)
	{
		try
		{
			((RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).videoPath = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_width(IntPtr L)
	{
		try
		{
			((RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).width = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_height(IntPtr L)
	{
		try
		{
			((RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).height = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playOnAwake(IntPtr L)
	{
		try
		{
			((RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playOnAwake = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_waitForFirstFrame(IntPtr L)
	{
		try
		{
			((RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).waitForFirstFrame = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loop(IntPtr L)
	{
		try
		{
			((RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loop = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skipOnDrop(IntPtr L)
	{
		try
		{
			((RawImageVideoPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).skipOnDrop = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_audioOutputMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoAudioOutputMode v);
			rawImageVideoPlayer.audioOutputMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_aspectRatio(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RawImageVideoPlayer rawImageVideoPlayer = (RawImageVideoPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoAspectRatio v);
			rawImageVideoPlayer.aspectRatio = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
