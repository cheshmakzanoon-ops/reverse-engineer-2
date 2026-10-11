using System;
using GPUDamageText;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GPUDamageTextDamageNumManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DamageNumManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 11, 0, 0);
		Utils.RegisterFunc(L, -3, "GetRenderCount", _m_GetRenderCount);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "AddDamageNum", _m_AddDamageNum);
		Utils.RegisterFunc(L, -3, "GetAliveCount", _m_GetAliveCount);
		Utils.RegisterFunc(L, -3, "GetRenderMesh", _m_GetRenderMesh);
		Utils.RegisterFunc(L, -3, "GetRenderMaterial", _m_GetRenderMaterial);
		Utils.RegisterFunc(L, -3, "GetRenderMaterialPropertyBlock", _m_GetRenderMaterialPropertyBlock);
		Utils.RegisterFunc(L, -3, "GetRenderLocalToWorlds", _m_GetRenderLocalToWorlds);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "Release", _m_Release);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 2, 0);
		Utils.RegisterObject(L, translator, -4, "DEFAULT_RENDER_MAX_NUM", 1023);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
		Utils.RegisterFunc(L, -2, "RenderFeature", _g_get_RenderFeature);
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
				DamageNumManager o = new DamageNumManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GPUDamageText.DamageNumManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderCount(IntPtr L)
	{
		try
		{
			int renderCount = ((DamageNumManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetRenderCount();
			Lua.xlua_pushinteger(L, renderCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((DamageNumManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			((DamageNumManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddDamageNum(IntPtr L)
	{
		try
		{
			DamageNumManager obj = (DamageNumManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int style = Lua.xlua_tointeger(L, 2);
			int damageType = Lua.xlua_tointeger(L, 3);
			ulong damageNum = Lua.lua_touint64(L, 4);
			bool showIcon = Lua.lua_toboolean(L, 5);
			float posX = (float)Lua.lua_tonumber(L, 6);
			float poxY = (float)Lua.lua_tonumber(L, 7);
			float posZ = (float)Lua.lua_tonumber(L, 8);
			int animStyle = Lua.xlua_tointeger(L, 9);
			float scale = (float)Lua.lua_tonumber(L, 10);
			float time = (float)Lua.lua_tonumber(L, 11);
			ulong n = obj.AddDamageNum(style, damageType, damageNum, showIcon, posX, poxY, posZ, animStyle, scale, time);
			Lua.lua_pushuint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAliveCount(IntPtr L)
	{
		try
		{
			int aliveCount = ((DamageNumManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetAliveCount();
			Lua.xlua_pushinteger(L, aliveCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Mesh renderMesh = ((DamageNumManager)objectTranslator.FastGetCSObj(L, 1)).GetRenderMesh();
			objectTranslator.Push(L, renderMesh);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Material renderMaterial = ((DamageNumManager)objectTranslator.FastGetCSObj(L, 1)).GetRenderMaterial();
			objectTranslator.Push(L, renderMaterial);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderMaterialPropertyBlock(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			MaterialPropertyBlock renderMaterialPropertyBlock = ((DamageNumManager)objectTranslator.FastGetCSObj(L, 1)).GetRenderMaterialPropertyBlock();
			objectTranslator.Push(L, renderMaterialPropertyBlock);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderLocalToWorlds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Matrix4x4[] renderLocalToWorlds = ((DamageNumManager)objectTranslator.FastGetCSObj(L, 1)).GetRenderLocalToWorlds();
			objectTranslator.Push(L, renderLocalToWorlds);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((DamageNumManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Release(IntPtr L)
	{
		try
		{
			((DamageNumManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Release();
			return 0;
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
			ObjectTranslatorPool.Instance.Find(L).Push(L, DamageNumManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RenderFeature(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DamageNumManager.RenderFeature);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
