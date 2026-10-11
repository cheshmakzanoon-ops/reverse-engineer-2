using System;
using UnityEngine;
using UnityEngine.Video;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FullScreenVideoManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FullScreenVideoManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 1, 1);
		Utils.RegisterFunc(L, -3, "LoadVideo", _m_LoadVideo);
		Utils.RegisterFunc(L, -3, "OnVideoLoaded", _m_OnVideoLoaded);
		Utils.RegisterFunc(L, -3, "OnVideoPrepared", _m_OnVideoPrepared);
		Utils.RegisterFunc(L, -3, "OnVideoStopped", _m_OnVideoStopped);
		Utils.RegisterFunc(L, -3, "PauseVideo", _m_PauseVideo);
		Utils.RegisterFunc(L, -3, "StopAndReleaseVideo", _m_StopAndReleaseVideo);
		Utils.RegisterFunc(L, -3, "CreateVideoPlayer", _m_CreateVideoPlayer);
		Utils.RegisterFunc(L, -3, "SetVideoAlpha", _m_SetVideoAlpha);
		Utils.RegisterFunc(L, -2, "mVideoPlayer", _g_get_mVideoPlayer);
		Utils.RegisterFunc(L, -1, "mVideoPlayer", _s_set_mVideoPlayer);
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
				FullScreenVideoManager o = new FullScreenVideoManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FullScreenVideoManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadVideo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FullScreenVideoManager fullScreenVideoManager = (FullScreenVideoManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<Action<Transform, VideoPlayer>>(L, 4) && objectTranslator.Assignable<Action>(L, 5))
			{
				string vPath = Lua.lua_tostring(L, 2);
				bool isLoop = Lua.lua_toboolean(L, 3);
				Action<Transform, VideoPlayer> firstFrameReadyCallback = objectTranslator.GetDelegate<Action<Transform, VideoPlayer>>(L, 4);
				Action videoStoppedCallback = objectTranslator.GetDelegate<Action>(L, 5);
				fullScreenVideoManager.LoadVideo(vPath, isLoop, firstFrameReadyCallback, videoStoppedCallback);
				return 0;
			}
			if (num == 7 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<Action<Transform, VideoPlayer>>(L, 4) && objectTranslator.Assignable<Action>(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && objectTranslator.Assignable<Action>(L, 7))
			{
				string vPath2 = Lua.lua_tostring(L, 2);
				bool isLoop2 = Lua.lua_toboolean(L, 3);
				Action<Transform, VideoPlayer> firstFrameReadyCallback2 = objectTranslator.GetDelegate<Action<Transform, VideoPlayer>>(L, 4);
				Action videoStoppedCallback2 = objectTranslator.GetDelegate<Action>(L, 5);
				bool useExternalAudio = Lua.lua_toboolean(L, 6);
				Action videoStartedCallback = objectTranslator.GetDelegate<Action>(L, 7);
				fullScreenVideoManager.LoadVideo(vPath2, isLoop2, firstFrameReadyCallback2, videoStoppedCallback2, useExternalAudio, videoStartedCallback);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FullScreenVideoManager.LoadVideo!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnVideoLoaded(IntPtr L)
	{
		try
		{
			FullScreenVideoManager obj = (FullScreenVideoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string url = Lua.lua_tostring(L, 2);
			obj.OnVideoLoaded(url);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnVideoPrepared(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FullScreenVideoManager fullScreenVideoManager = (FullScreenVideoManager)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer vp = (VideoPlayer)objectTranslator.GetObject(L, 2, typeof(VideoPlayer));
			fullScreenVideoManager.OnVideoPrepared(vp);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnVideoStopped(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FullScreenVideoManager fullScreenVideoManager = (FullScreenVideoManager)objectTranslator.FastGetCSObj(L, 1);
			VideoPlayer vp = (VideoPlayer)objectTranslator.GetObject(L, 2, typeof(VideoPlayer));
			fullScreenVideoManager.OnVideoStopped(vp);
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
			((FullScreenVideoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PauseVideo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAndReleaseVideo(IntPtr L)
	{
		try
		{
			((FullScreenVideoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAndReleaseVideo();
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
			((FullScreenVideoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateVideoPlayer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVideoAlpha(IntPtr L)
	{
		try
		{
			FullScreenVideoManager obj = (FullScreenVideoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float videoAlpha = (float)Lua.lua_tonumber(L, 2);
			obj.SetVideoAlpha(videoAlpha);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mVideoPlayer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FullScreenVideoManager fullScreenVideoManager = (FullScreenVideoManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, fullScreenVideoManager.mVideoPlayer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mVideoPlayer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((FullScreenVideoManager)objectTranslator.FastGetCSObj(L, 1)).mVideoPlayer = (VideoPlayer)objectTranslator.GetObject(L, 2, typeof(VideoPlayer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
