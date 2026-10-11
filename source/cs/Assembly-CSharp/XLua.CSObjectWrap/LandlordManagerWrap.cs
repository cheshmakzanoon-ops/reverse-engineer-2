using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LandlordManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LandlordManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 6, 3);
		Utils.RegisterFunc(L, -3, "RefreshTableNameFromWorld", _m_RefreshTableNameFromWorld);
		Utils.RegisterFunc(L, -3, "SetIsNewCenterMapFlag", _m_SetIsNewCenterMapFlag);
		Utils.RegisterFunc(L, -3, "CalculateOccupyCurProgress", _m_CalculateOccupyCurProgress);
		Utils.RegisterFunc(L, -3, "EnsureCenterMapRandomFxSystem", _m_EnsureCenterMapRandomFxSystem);
		Utils.RegisterFunc(L, -3, "DisposeCenterMapRandomFxSystem", _m_DisposeCenterMapRandomFxSystem);
		Utils.RegisterFunc(L, -2, "cityTableName", _g_get_cityTableName);
		Utils.RegisterFunc(L, -2, "isLandlordActOpenAndNewMap", _g_get_isLandlordActOpenAndNewMap);
		Utils.RegisterFunc(L, -2, "isNewCenterMap", _g_get_isNewCenterMap);
		Utils.RegisterFunc(L, -2, "myCampId", _g_get_myCampId);
		Utils.RegisterFunc(L, -2, "previewBoomTime", _g_get_previewBoomTime);
		Utils.RegisterFunc(L, -2, "centerServerId", _g_get_centerServerId);
		Utils.RegisterFunc(L, -1, "myCampId", _s_set_myCampId);
		Utils.RegisterFunc(L, -1, "previewBoomTime", _s_set_previewBoomTime);
		Utils.RegisterFunc(L, -1, "centerServerId", _s_set_centerServerId);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 1, 0);
		Utils.RegisterFunc(L, -4, "Purge", _m_Purge_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearLandlordCache", _m_ClearLandlordCache_xlua_st_);
		Utils.RegisterFunc(L, -4, "TriggerCityExplosion", _m_TriggerCityExplosion_xlua_st_);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
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
				LandlordManager o = new LandlordManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LandlordManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Purge_xlua_st_(IntPtr L)
	{
		try
		{
			LandlordManager.Purge();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearLandlordCache_xlua_st_(IntPtr L)
	{
		try
		{
			LandlordManager.ClearLandlordCache();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshTableNameFromWorld(IntPtr L)
	{
		try
		{
			((LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshTableNameFromWorld();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetIsNewCenterMapFlag(IntPtr L)
	{
		try
		{
			LandlordManager obj = (LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isNewCenterMapValue = Lua.lua_toboolean(L, 2);
			bool isFromSrcServerPush = Lua.lua_toboolean(L, 3);
			obj.SetIsNewCenterMapFlag(isNewCenterMapValue, isFromSrcServerPush);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateOccupyCurProgress(IntPtr L)
	{
		try
		{
			LandlordManager obj = (LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long startOccupyTime = Lua.lua_toint64(L, 2);
			float startOccupyProgress = (float)Lua.lua_tonumber(L, 3);
			float maxProgress = (float)Lua.lua_tonumber(L, 4);
			int campId = Lua.xlua_tointeger(L, 5);
			float extraEffectValue = (float)Lua.lua_tonumber(L, 6);
			obj.CalculateOccupyCurProgress(startOccupyTime, startOccupyProgress, maxProgress, campId, extraEffectValue, out var curProgress, out var timeToComplete);
			Lua.xlua_pushinteger(L, curProgress);
			Lua.xlua_pushinteger(L, timeToComplete);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnsureCenterMapRandomFxSystem(IntPtr L)
	{
		try
		{
			((LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnsureCenterMapRandomFxSystem();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisposeCenterMapRandomFxSystem(IntPtr L)
	{
		try
		{
			((LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DisposeCenterMapRandomFxSystem();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TriggerCityExplosion_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = LandlordManager.TriggerCityExplosion(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, LandlordManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cityTableName(IntPtr L)
	{
		try
		{
			LandlordManager landlordManager = (LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, landlordManager.cityTableName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isLandlordActOpenAndNewMap(IntPtr L)
	{
		try
		{
			LandlordManager landlordManager = (LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, landlordManager.isLandlordActOpenAndNewMap);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isNewCenterMap(IntPtr L)
	{
		try
		{
			LandlordManager landlordManager = (LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, landlordManager.isNewCenterMap);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_myCampId(IntPtr L)
	{
		try
		{
			LandlordManager landlordManager = (LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, landlordManager.myCampId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_previewBoomTime(IntPtr L)
	{
		try
		{
			LandlordManager landlordManager = (LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, landlordManager.previewBoomTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_centerServerId(IntPtr L)
	{
		try
		{
			LandlordManager landlordManager = (LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, landlordManager.centerServerId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_myCampId(IntPtr L)
	{
		try
		{
			((LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).myCampId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_previewBoomTime(IntPtr L)
	{
		try
		{
			((LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).previewBoomTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_centerServerId(IntPtr L)
	{
		try
		{
			((LandlordManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).centerServerId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
