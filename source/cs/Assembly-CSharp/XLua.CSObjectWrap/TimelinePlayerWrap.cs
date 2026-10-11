using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TimelinePlayerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TimelinePlayer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 2, 2);
		Utils.RegisterFunc(L, -3, "PlayTimeline", _m_PlayTimeline);
		Utils.RegisterFunc(L, -3, "PlayIdleQueued", _m_PlayIdleQueued);
		Utils.RegisterFunc(L, -3, "PlayTimelineAt", _m_PlayTimelineAt);
		Utils.RegisterFunc(L, -3, "StopTimeline", _m_StopTimeline);
		Utils.RegisterFunc(L, -2, "aniData", _g_get_aniData);
		Utils.RegisterFunc(L, -2, "playableDirector", _g_get_playableDirector);
		Utils.RegisterFunc(L, -1, "aniData", _s_set_aniData);
		Utils.RegisterFunc(L, -1, "playableDirector", _s_set_playableDirector);
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
				TimelinePlayer o = new TimelinePlayer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimelinePlayer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayTimeline(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimelinePlayer timelinePlayer = (TimelinePlayer)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<DirectorWrapMode>(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				string name = Lua.lua_tostring(L, 2);
				bool toIdle = Lua.lua_toboolean(L, 3);
				objectTranslator.Get(L, 4, out DirectorWrapMode v);
				bool rewind = Lua.lua_toboolean(L, 5);
				double number = timelinePlayer.PlayTimeline(name, toIdle, v, rewind);
				Lua.lua_pushnumber(L, number);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && objectTranslator.Assignable<DirectorWrapMode>(L, 4))
			{
				string name2 = Lua.lua_tostring(L, 2);
				bool toIdle2 = Lua.lua_toboolean(L, 3);
				objectTranslator.Get(L, 4, out DirectorWrapMode v2);
				double number2 = timelinePlayer.PlayTimeline(name2, toIdle2, v2);
				Lua.lua_pushnumber(L, number2);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string name3 = Lua.lua_tostring(L, 2);
				bool toIdle3 = Lua.lua_toboolean(L, 3);
				double number3 = timelinePlayer.PlayTimeline(name3, toIdle3);
				Lua.lua_pushnumber(L, number3);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name4 = Lua.lua_tostring(L, 2);
				double number4 = timelinePlayer.PlayTimeline(name4);
				Lua.lua_pushnumber(L, number4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimelinePlayer.PlayTimeline!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayIdleQueued(IntPtr L)
	{
		try
		{
			((TimelinePlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PlayIdleQueued();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayTimelineAt(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimelinePlayer timelinePlayer = (TimelinePlayer)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DirectorWrapMode>(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string name = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out DirectorWrapMode v);
				bool rewind = Lua.lua_toboolean(L, 4);
				float startTime = (float)Lua.lua_tonumber(L, 5);
				double number = timelinePlayer.PlayTimelineAt(name, v, rewind, startTime);
				Lua.lua_pushnumber(L, number);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DirectorWrapMode>(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				string name2 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out DirectorWrapMode v2);
				bool rewind2 = Lua.lua_toboolean(L, 4);
				double number2 = timelinePlayer.PlayTimelineAt(name2, v2, rewind2);
				Lua.lua_pushnumber(L, number2);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<DirectorWrapMode>(L, 3))
			{
				string name3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out DirectorWrapMode v3);
				double number3 = timelinePlayer.PlayTimelineAt(name3, v3);
				Lua.lua_pushnumber(L, number3);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name4 = Lua.lua_tostring(L, 2);
				double number4 = timelinePlayer.PlayTimelineAt(name4);
				Lua.lua_pushnumber(L, number4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TimelinePlayer.PlayTimelineAt!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopTimeline(IntPtr L)
	{
		try
		{
			((TimelinePlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopTimeline();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_aniData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimelinePlayer timelinePlayer = (TimelinePlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, timelinePlayer.aniData);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playableDirector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TimelinePlayer timelinePlayer = (TimelinePlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, timelinePlayer.playableDirector);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_aniData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TimelinePlayer)objectTranslator.FastGetCSObj(L, 1)).aniData = (List<TimelinePlayer.Data>)objectTranslator.GetObject(L, 2, typeof(List<TimelinePlayer.Data>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playableDirector(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TimelinePlayer)objectTranslator.FastGetCSObj(L, 1)).playableDirector = (PlayableDirector)objectTranslator.GetObject(L, 2, typeof(PlayableDirector));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
