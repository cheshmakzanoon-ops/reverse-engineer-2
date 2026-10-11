using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VoiceChatRoomTypeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Utils.BeginObjectRegister(typeof(VoiceChatRoomType), L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeof(VoiceChatRoomType), L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeof(VoiceChatRoomType), L, null, 4, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Fluency", VoiceChatRoomType.Fluency);
		Utils.RegisterObject(L, translator, -4, "Standard", VoiceChatRoomType.Standard);
		Utils.RegisterObject(L, translator, -4, "HighQuality", VoiceChatRoomType.HighQuality);
		Utils.RegisterFunc(L, -4, "__CastFrom", __CastFrom);
		Utils.EndClassRegister(typeof(VoiceChatRoomType), L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CastFrom(IntPtr L)
	{
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
		LuaTypes luaTypes = Lua.lua_type(L, 1);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TNUMBER:
			objectTranslator.PushVoiceChatRoomType(L, (VoiceChatRoomType)Lua.xlua_tointeger(L, 1));
			break;
		case LuaTypes.LUA_TSTRING:
			if (Lua.xlua_is_eq_str(L, 1, "Fluency"))
			{
				objectTranslator.PushVoiceChatRoomType(L, VoiceChatRoomType.Fluency);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "Standard"))
			{
				objectTranslator.PushVoiceChatRoomType(L, VoiceChatRoomType.Standard);
				break;
			}
			if (Lua.xlua_is_eq_str(L, 1, "HighQuality"))
			{
				objectTranslator.PushVoiceChatRoomType(L, VoiceChatRoomType.HighQuality);
				break;
			}
			return Lua.luaL_error(L, "invalid string for VoiceChatRoomType!");
		default:
			return Lua.luaL_error(L, "invalid lua type for VoiceChatRoomType! Expect number or string, got + " + luaTypes);
		}
		return 1;
	}
}
