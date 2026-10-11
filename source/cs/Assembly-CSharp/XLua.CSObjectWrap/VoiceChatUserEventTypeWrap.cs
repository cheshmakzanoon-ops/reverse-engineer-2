using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VoiceChatUserEventTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(VoiceChatUserEventType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(VoiceChatUserEventType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(VoiceChatUserEventType), L, null, 8, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Unknown", VoiceChatUserEventType.Unknown);
		Utils.RegisterObject(L, translator, -4, "UserEnter", VoiceChatUserEventType.UserEnter);
		Utils.RegisterObject(L, translator, -4, "UserExit", VoiceChatUserEventType.UserExit);
		Utils.RegisterObject(L, translator, -4, "UserMicOpened", VoiceChatUserEventType.UserMicOpened);
		Utils.RegisterObject(L, translator, -4, "UserMicClosed", VoiceChatUserEventType.UserMicClosed);
		Utils.RegisterObject(L, translator, -4, "UserHasAudio", VoiceChatUserEventType.UserHasAudio);
		Utils.RegisterObject(L, translator, -4, "UserNoAudio", VoiceChatUserEventType.UserNoAudio);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(VoiceChatUserEventType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushVoiceChatUserEventType(L, (VoiceChatUserEventType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Unknown"))
			{
				objectTranslator.PushVoiceChatUserEventType(L, VoiceChatUserEventType.Unknown);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UserEnter"))
			{
				objectTranslator.PushVoiceChatUserEventType(L, VoiceChatUserEventType.UserEnter);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UserExit"))
			{
				objectTranslator.PushVoiceChatUserEventType(L, VoiceChatUserEventType.UserExit);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UserMicOpened"))
			{
				objectTranslator.PushVoiceChatUserEventType(L, VoiceChatUserEventType.UserMicOpened);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UserMicClosed"))
			{
				objectTranslator.PushVoiceChatUserEventType(L, VoiceChatUserEventType.UserMicClosed);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UserHasAudio"))
			{
				objectTranslator.PushVoiceChatUserEventType(L, VoiceChatUserEventType.UserHasAudio);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "UserNoAudio"))
			{
				objectTranslator.PushVoiceChatUserEventType(L, VoiceChatUserEventType.UserNoAudio);
				break;
			}
			return Lua.luaL_error(L, "invalid string for VoiceChatUserEventType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for VoiceChatUserEventType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
