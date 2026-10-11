using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VoiceChatMicPermissionWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(VoiceChatMicPermission), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(VoiceChatMicPermission), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(VoiceChatMicPermission), L, null, 5, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Granted", VoiceChatMicPermission.Granted);
		Utils.RegisterObject(L, translator, -4, "Denied", VoiceChatMicPermission.Denied);
		Utils.RegisterObject(L, translator, -4, "NotDetermined", VoiceChatMicPermission.NotDetermined);
		Utils.RegisterObject(L, translator, -4, "Unknown", VoiceChatMicPermission.Unknown);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(VoiceChatMicPermission), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushVoiceChatMicPermission(L, (VoiceChatMicPermission)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Granted"))
			{
				objectTranslator.PushVoiceChatMicPermission(L, VoiceChatMicPermission.Granted);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Denied"))
			{
				objectTranslator.PushVoiceChatMicPermission(L, VoiceChatMicPermission.Denied);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "NotDetermined"))
			{
				objectTranslator.PushVoiceChatMicPermission(L, VoiceChatMicPermission.NotDetermined);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Unknown"))
			{
				objectTranslator.PushVoiceChatMicPermission(L, VoiceChatMicPermission.Unknown);
				break;
			}
			return Lua.luaL_error(L, "invalid string for VoiceChatMicPermission!");
		default:
			return Lua.luaL_error(L, "invalid lua type for VoiceChatMicPermission! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
