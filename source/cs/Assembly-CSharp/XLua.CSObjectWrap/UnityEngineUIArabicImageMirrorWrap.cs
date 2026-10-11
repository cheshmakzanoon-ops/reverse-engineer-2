using System;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIArabicImageMirrorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ArabicImageMirror);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 2, 1);
		Utils.RegisterFunc(L, -3, "ModifyMesh", _m_ModifyMesh);
		Utils.RegisterFunc(L, -2, "mirrorType", _g_get_mirrorType);
		Utils.RegisterFunc(L, -2, "rectTransform", _g_get_rectTransform);
		Utils.RegisterFunc(L, -1, "mirrorType", _s_set_mirrorType);
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
				ArabicImageMirror o = new ArabicImageMirror();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.ArabicImageMirror constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ModifyMesh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArabicImageMirror arabicImageMirror = (ArabicImageMirror)objectTranslator.FastGetCSObj(L, 1);
			VertexHelper vh = (VertexHelper)objectTranslator.GetObject(L, 2, typeof(VertexHelper));
			arabicImageMirror.ModifyMesh(vh);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mirrorType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArabicImageMirror arabicImageMirror = (ArabicImageMirror)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, arabicImageMirror.mirrorType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rectTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArabicImageMirror arabicImageMirror = (ArabicImageMirror)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, arabicImageMirror.rectTransform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mirrorType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ArabicImageMirror arabicImageMirror = (ArabicImageMirror)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ArabicImageMirror.MirrorType v);
			arabicImageMirror.mirrorType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
