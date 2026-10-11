using System;
using LS.UnityEngine.UI;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LSUnityEngineUIWorldMapZoneImageWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldMapZoneImage);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 4, 5);
		Utils.RegisterFunc(L, -3, "SetZoneCampIds", _m_SetZoneCampIds);
		Utils.RegisterFunc(L, -3, "ClearZoneColors", _m_ClearZoneColors);
		Utils.RegisterFunc(L, -2, "rows", _g_get_rows);
		Utils.RegisterFunc(L, -2, "columns", _g_get_columns);
		Utils.RegisterFunc(L, -2, "topScale", _g_get_topScale);
		Utils.RegisterFunc(L, -2, "space", _g_get_space);
		Utils.RegisterFunc(L, -1, "rows", _s_set_rows);
		Utils.RegisterFunc(L, -1, "columns", _s_set_columns);
		Utils.RegisterFunc(L, -1, "topScale", _s_set_topScale);
		Utils.RegisterFunc(L, -1, "space", _s_set_space);
		Utils.RegisterFunc(L, -1, "CampColors", _s_set_CampColors);
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
				WorldMapZoneImage o = new WorldMapZoneImage();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LS.UnityEngine.UI.WorldMapZoneImage constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetZoneCampIds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMapZoneImage worldMapZoneImage = (WorldMapZoneImage)objectTranslator.FastGetCSObj(L, 1);
			int[] zoneCampIds = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
			worldMapZoneImage.SetZoneCampIds(zoneCampIds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearZoneColors(IntPtr L)
	{
		try
		{
			((WorldMapZoneImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearZoneColors();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rows(IntPtr L)
	{
		try
		{
			WorldMapZoneImage worldMapZoneImage = (WorldMapZoneImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMapZoneImage.rows);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_columns(IntPtr L)
	{
		try
		{
			WorldMapZoneImage worldMapZoneImage = (WorldMapZoneImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMapZoneImage.columns);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_topScale(IntPtr L)
	{
		try
		{
			WorldMapZoneImage worldMapZoneImage = (WorldMapZoneImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldMapZoneImage.topScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_space(IntPtr L)
	{
		try
		{
			WorldMapZoneImage worldMapZoneImage = (WorldMapZoneImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, worldMapZoneImage.space);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rows(IntPtr L)
	{
		try
		{
			((WorldMapZoneImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).rows = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_columns(IntPtr L)
	{
		try
		{
			((WorldMapZoneImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).columns = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_topScale(IntPtr L)
	{
		try
		{
			((WorldMapZoneImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).topScale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_space(IntPtr L)
	{
		try
		{
			((WorldMapZoneImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).space = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CampColors(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldMapZoneImage)objectTranslator.FastGetCSObj(L, 1)).CampColors = (Color[])objectTranslator.GetObject(L, 2, typeof(Color[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
