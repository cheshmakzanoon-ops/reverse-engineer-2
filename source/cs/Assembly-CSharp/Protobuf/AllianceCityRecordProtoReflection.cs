using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class AllianceCityRecordProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static AllianceCityRecordProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("Ch1BbGxpYW5jZUNpdHlSZWNvcmRQcm90by5wcm90bxIIcHJvdG9idWYiNAoW" + "QWxsaWFuY2VDaXR5VXNlclJlY29yZBILCgN1aWQYASABKAkSDQoFcG9pbnQY" + "AiABKAMikgEKF0FsbGlhbmNlQ2l0eVJlY29yZFByb3RvEjkKD2tpbGxVc2Vy" + "UmVjb3JkcxgBIAMoCzIgLnByb3RvYnVmLkFsbGlhbmNlQ2l0eVVzZXJSZWNv" + "cmQSPAoSZGVzdHJveVVzZXJSZWNvcmRzGAIgAygLMiAucHJvdG9idWYuQWxs" + "aWFuY2VDaXR5VXNlclJlY29yZCKyAQoWQWxsaWFuY2VDaXR5T2NjdXB5SW5m" + "bxIOCgZjaXR5SWQYASABKAUSDAoEYWJichgCIAEoCRISCgphbGxhaW5jZUlk" + "GAMgASgJEg0KBWNvbG9yGAQgASgFEhAKCGNpdHlOYW1lGAUgASgJEhQKDGFs" + "bGlhbmNlTmFtZRgGIAEoCRIWCg5vY2N1cHlTZXJ2ZXJJZBgHIAEoBRIXCg9m" + "aXJzdE9jY3VweVRpbWUYCCABKAMiTAoYV29ybGRBbGxBbGxpYW5jZUNpdHlJ" + "bmZvEjAKBmluZm9lcxgBIAMoCzIgLnByb3RvYnVmLkFsbGlhbmNlQ2l0eU9j" + "Y3VweUluZm8iiwEKF1dvcmxkQWxsaWFuY2VPY2N1cHlJbmZvEjgKEkFsbGlh" + "bmNlT2NjdXB5SW5mbxgBIAMoCzIcLnByb3RvYnVmLkFsbGlhbmNlT2NjdXB5" + "SW5mbxI2ChFzZXJ2ZXJEZXN0cm95SW5mbxgCIAMoCzIbLnByb3RvYnVmLlNl" + "cnZlckRlc3Ryb3lJbmZvItQBChJBbGxpYW5jZU9jY3VweUluZm8SEgoKYWxs" + "aWFuY2VJZBgBIAEoCRIMCgRhYmJyGAIgASgJEg0KBWNvbG9yGAMgASgFEhAK" + "CGNpdHlOYW1lGAQgASgJEhQKDGFsbGlhbmNlTmFtZRgFIAEoCRIWCg5vY2N1" + "cHlTZXJ2ZXJJZBgGIAEoBRIkCghjaXR5SW5mbxgHIAMoCzISLnByb3RvYnVm" + "LkNpdHlJbmZvEicKC2Rlc3Ryb3lJbmZvGAggAygLMhIucHJvdG9idWYuQ2l0" + "eUluZm8iGgoIQ2l0eUluZm8SDgoGY2l0eUlkGAEgASgFIj0KEVNlcnZlckRl" + "c3Ryb3lJbmZvEhAKCHNlcnZlcklkGAEgASgFEhYKDmRlc3Ryb3lDaXR5SWRz" + "GAIgAygFQh0KG25ldC5pbTMwLmFwcy5tb2RlbC5wcm90b2J1ZmIGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[8]
		{
			new GeneratedClrTypeInfo(typeof(AllianceCityUserRecord), AllianceCityUserRecord.Parser, new string[2] { "Uid", "Point" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(AllianceCityRecordProto), AllianceCityRecordProto.Parser, new string[2] { "KillUserRecords", "DestroyUserRecords" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(AllianceCityOccupyInfo), AllianceCityOccupyInfo.Parser, new string[8] { "CityId", "Abbr", "AllainceId", "Color", "CityName", "AllianceName", "OccupyServerId", "FirstOccupyTime" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(WorldAllAllianceCityInfo), WorldAllAllianceCityInfo.Parser, new string[1] { "Infoes" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(WorldAllianceOccupyInfo), WorldAllianceOccupyInfo.Parser, new string[2] { "AllianceOccupyInfo", "ServerDestroyInfo" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(AllianceOccupyInfo), AllianceOccupyInfo.Parser, new string[8] { "AllianceId", "Abbr", "Color", "CityName", "AllianceName", "OccupyServerId", "CityInfo", "DestroyInfo" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(CityInfo), CityInfo.Parser, new string[1] { "CityId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ServerDestroyInfo), ServerDestroyInfo.Parser, new string[2] { "ServerId", "DestroyCityIds" }, null, null, null, null)
		}));
	}
}
