using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class ShieldInfoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static ShieldInfoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChBTaGllbGRJbmZvLnByb3RvEghwcm90b2J1ZiLhAQoKU2hpZWxkSW5mbxIS" + "CgpzaGllbGRUeXBlGAEgASgFEhMKC3NoaWVsZFZhbHVlGAIgASgDEhYKDnNo" + "aWVsZE1heFZhbHVlGAMgASgDEhkKEXRvdGFsQ29udHJpYnV0aW9uGAQgASgD" + "EhIKCmV4cGlyZVRpbWUYBSABKAMSNAoMYWxsaWFuY2VDaXR5GAogASgLMhwu" + "cHJvdG9idWYuQWxsaWFuY2VDaXR5U2hpZWxkSAASJAoEYmFzZRgLIAEoCzIU" + "LnByb3RvYnVmLkJhc2VTaGllbGRIAEIHCgVleHRyYSJvChJBbGxpYW5jZUNp" + "dHlTaGllbGQSFQoNY2hhcmdlRW5kVGltZRgBIAEoAxIeChZwYXJ0aWNpcGFu" + "dEFsbGlhbmNlSWRzGAIgAygJEg8KB3NraWxsSWQYAyABKAUSEQoJc3RhcnRU" + "aW1lGAQgASgDIgwKCkJhc2VTaGllbGRCHQobbmV0LmltMzAuYXBzLm1vZGVs" + "LnByb3RvYnVmYgZwcm90bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[3]
		{
			new GeneratedClrTypeInfo(typeof(ShieldInfo), ShieldInfo.Parser, new string[7] { "ShieldType", "ShieldValue", "ShieldMaxValue", "TotalContribution", "ExpireTime", "AllianceCity", "Base" }, new string[1] { "Extra" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(AllianceCityShield), AllianceCityShield.Parser, new string[4] { "ChargeEndTime", "ParticipantAllianceIds", "SkillId", "StartTime" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(BaseShield), BaseShield.Parser, null, null, null, null, null)
		}));
	}
}
