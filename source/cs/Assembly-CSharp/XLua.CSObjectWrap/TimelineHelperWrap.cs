using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TimelineHelperWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TimelineHelper);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "GetGenericBindingByTrackName", _m_GetGenericBindingByTrackName_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGenericBindingListByTrackName", _m_GetGenericBindingListByTrackName_xlua_st_);
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
				TimelineHelper o = new TimelineHelper();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimelineHelper constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGenericBindingByTrackName_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector director = (PlayableDirector)objectTranslator.GetObject(L, 1, typeof(PlayableDirector));
			string trackName = Lua.lua_tostring(L, 2);
			GameObject genericBindingByTrackName = TimelineHelper.GetGenericBindingByTrackName(director, trackName);
			objectTranslator.Push(L, genericBindingByTrackName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGenericBindingListByTrackName_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayableDirector director = (PlayableDirector)objectTranslator.GetObject(L, 1, typeof(PlayableDirector));
			string trackName = Lua.lua_tostring(L, 2);
			List<GameObject> genericBindingListByTrackName = TimelineHelper.GetGenericBindingListByTrackName(director, trackName);
			objectTranslator.Push(L, genericBindingListByTrackName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
