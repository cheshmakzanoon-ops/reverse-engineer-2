using System;
using UnityEngine.Video;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class QuadVideoPlayerCreatorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(QuadVideoPlayerCreator);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 10, 10);
		Utils.RegisterFunc(L, -3, "LoadVideo", _m_LoadVideo);
		Utils.RegisterFunc(L, -3, "PauseVideo", _m_PauseVideo);
		Utils.RegisterFunc(L, -3, "StopVideo", _m_StopVideo);
		Utils.RegisterFunc(L, -3, "CreateVideoPlayer", _m_CreateVideoPlayer);
		Utils.RegisterFunc(L, -3, "TryCreateRT", _m_TryCreateRT);
		Utils.RegisterFunc(L, -3, "ClearRT", _m_ClearRT);
		Utils.RegisterFunc(L, -3, "OnVideoLoaded", _m_OnVideoLoaded);
		Utils.RegisterFunc(L, -3, "OnDestroy", _m_OnDestroy);
		Utils.RegisterFunc(L, -2, "videoPath", _g_get_videoPath);
		Utils.RegisterFunc(L, -2, "width", _g_get_width);
		Utils.RegisterFunc(L, -2, "height", _g_get_height);
		Utils.RegisterFunc(L, -2, "playOnAwake", _g_get_playOnAwake);
		Utils.RegisterFunc(L, -2, "waitForFirstFrame", _g_get_waitForFirstFrame);
		Utils.RegisterFunc(L, -2, "loop", _g_get_loop);
		Utils.RegisterFunc(L, -2, "skipOnDrop", _g_get_skipOnDrop);
		Utils.RegisterFunc(L, -2, "isMirror", _g_get_isMirror);
		Utils.RegisterFunc(L, -2, "audioOutputMode", _g_get_audioOutputMode);
		Utils.RegisterFunc(L, -2, "aspectRatio", _g_get_aspectRatio);
		Utils.RegisterFunc(L, -1, "videoPath", _s_set_videoPath);
		Utils.RegisterFunc(L, -1, "width", _s_set_width);
		Utils.RegisterFunc(L, -1, "height", _s_set_height);
		Utils.RegisterFunc(L, -1, "playOnAwake", _s_set_playOnAwake);
		Utils.RegisterFunc(L, -1, "waitForFirstFrame", _s_set_waitForFirstFrame);
		Utils.RegisterFunc(L, -1, "loop", _s_set_loop);
		Utils.RegisterFunc(L, -1, "skipOnDrop", _s_set_skipOnDrop);
		Utils.RegisterFunc(L, -1, "isMirror", _s_set_isMirror);
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
				QuadVideoPlayerCreator o = new QuadVideoPlayerCreator();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to QuadVideoPlayerCreator constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadVideo(IntPtr L)
	{
		try
		{
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LoadVideo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PauseVideo(IntPtr L)
	{
		try
		{
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PauseVideo();
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
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopVideo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateVideoPlayer(IntPtr L)
	{
		try
		{
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateVideoPlayer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryCreateRT(IntPtr L)
	{
		try
		{
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TryCreateRT();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearRT(IntPtr L)
	{
		try
		{
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearRT();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnVideoLoaded(IntPtr L)
	{
		try
		{
			QuadVideoPlayerCreator obj = (QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			obj.OnVideoLoaded(path);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDestroy(IntPtr L)
	{
		try
		{
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDestroy();
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, quadVideoPlayerCreator.videoPath);
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, quadVideoPlayerCreator.width);
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, quadVideoPlayerCreator.height);
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, quadVideoPlayerCreator.playOnAwake);
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, quadVideoPlayerCreator.waitForFirstFrame);
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, quadVideoPlayerCreator.loop);
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, quadVideoPlayerCreator.skipOnDrop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isMirror(IntPtr L)
	{
		try
		{
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, quadVideoPlayerCreator.isMirror);
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, quadVideoPlayerCreator.audioOutputMode);
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, quadVideoPlayerCreator.aspectRatio);
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
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).videoPath = Lua.lua_tostring(L, 2);
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
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).width = Lua.xlua_tointeger(L, 2);
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
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).height = Lua.xlua_tointeger(L, 2);
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
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playOnAwake = Lua.lua_toboolean(L, 2);
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
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).waitForFirstFrame = Lua.lua_toboolean(L, 2);
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
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loop = Lua.lua_toboolean(L, 2);
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
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).skipOnDrop = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isMirror(IntPtr L)
	{
		try
		{
			((QuadVideoPlayerCreator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isMirror = Lua.lua_toboolean(L, 2);
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoAudioOutputMode v);
			quadVideoPlayerCreator.audioOutputMode = v;
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
			QuadVideoPlayerCreator quadVideoPlayerCreator = (QuadVideoPlayerCreator)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out VideoAspectRatio v);
			quadVideoPlayerCreator.aspectRatio = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
