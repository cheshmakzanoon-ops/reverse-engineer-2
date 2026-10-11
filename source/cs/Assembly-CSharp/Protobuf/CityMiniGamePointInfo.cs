using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityMiniGamePointInfo : IMessage<CityMiniGamePointInfo>, IMessage, IEquatable<CityMiniGamePointInfo>, IDeepCloneable<CityMiniGamePointInfo>
{
	private static readonly MessageParser<CityMiniGamePointInfo> _parser = new MessageParser<CityMiniGamePointInfo>(() => new CityMiniGamePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int CityIdFieldNumber = 1;

	private int cityId_;

	public const int GameCityStateFieldNumber = 2;

	private int gameCityState_;

	public const int UidFieldNumber = 3;

	private string uid_ = "";

	public const int UserNameFieldNumber = 4;

	private string userName_ = "";

	public const int PicFieldNumber = 5;

	private string pic_ = "";

	public const int PicVerFieldNumber = 6;

	private int picVer_;

	public const int HeadSkinIdFieldNumber = 7;

	private int headSkinId_;

	public const int HeadSkinETFieldNumber = 8;

	private long headSkinET_;

	public const int ServerIdFieldNumber = 9;

	private int serverId_;

	public const int StartTimeFieldNumber = 10;

	private long startTime_;

	public const int EndTimeFieldNumber = 11;

	private long endTime_;

	public const int OpenTimeFieldNumber = 12;

	private long openTime_;

	public const int RankTotalNumFieldNumber = 13;

	private int rankTotalNum_;

	[DebuggerNonUserCode]
	public static MessageParser<CityMiniGamePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[38];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int CityId
	{
		get
		{
			return cityId_;
		}
		set
		{
			cityId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int GameCityState
	{
		get
		{
			return gameCityState_;
		}
		set
		{
			gameCityState_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Uid
	{
		get
		{
			return uid_;
		}
		set
		{
			uid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string UserName
	{
		get
		{
			return userName_;
		}
		set
		{
			userName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Pic
	{
		get
		{
			return pic_;
		}
		set
		{
			pic_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int PicVer
	{
		get
		{
			return picVer_;
		}
		set
		{
			picVer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadSkinId
	{
		get
		{
			return headSkinId_;
		}
		set
		{
			headSkinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long HeadSkinET
	{
		get
		{
			return headSkinET_;
		}
		set
		{
			headSkinET_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ServerId
	{
		get
		{
			return serverId_;
		}
		set
		{
			serverId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StartTime
	{
		get
		{
			return startTime_;
		}
		set
		{
			startTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long EndTime
	{
		get
		{
			return endTime_;
		}
		set
		{
			endTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long OpenTime
	{
		get
		{
			return openTime_;
		}
		set
		{
			openTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RankTotalNum
	{
		get
		{
			return rankTotalNum_;
		}
		set
		{
			rankTotalNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityMiniGamePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public CityMiniGamePointInfo(CityMiniGamePointInfo other)
		: this()
	{
		cityId_ = other.cityId_;
		gameCityState_ = other.gameCityState_;
		uid_ = other.uid_;
		userName_ = other.userName_;
		pic_ = other.pic_;
		picVer_ = other.picVer_;
		headSkinId_ = other.headSkinId_;
		headSkinET_ = other.headSkinET_;
		serverId_ = other.serverId_;
		startTime_ = other.startTime_;
		endTime_ = other.endTime_;
		openTime_ = other.openTime_;
		rankTotalNum_ = other.rankTotalNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CityMiniGamePointInfo Clone()
	{
		return new CityMiniGamePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityMiniGamePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityMiniGamePointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CityId != other.CityId)
		{
			return false;
		}
		if (GameCityState != other.GameCityState)
		{
			return false;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (UserName != other.UserName)
		{
			return false;
		}
		if (Pic != other.Pic)
		{
			return false;
		}
		if (PicVer != other.PicVer)
		{
			return false;
		}
		if (HeadSkinId != other.HeadSkinId)
		{
			return false;
		}
		if (HeadSkinET != other.HeadSkinET)
		{
			return false;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (EndTime != other.EndTime)
		{
			return false;
		}
		if (OpenTime != other.OpenTime)
		{
			return false;
		}
		if (RankTotalNum != other.RankTotalNum)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CityId != 0)
		{
			num ^= CityId.GetHashCode();
		}
		if (GameCityState != 0)
		{
			num ^= GameCityState.GetHashCode();
		}
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (UserName.Length != 0)
		{
			num ^= UserName.GetHashCode();
		}
		if (Pic.Length != 0)
		{
			num ^= Pic.GetHashCode();
		}
		if (PicVer != 0)
		{
			num ^= PicVer.GetHashCode();
		}
		if (HeadSkinId != 0)
		{
			num ^= HeadSkinId.GetHashCode();
		}
		if (HeadSkinET != 0L)
		{
			num ^= HeadSkinET.GetHashCode();
		}
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (EndTime != 0L)
		{
			num ^= EndTime.GetHashCode();
		}
		if (OpenTime != 0L)
		{
			num ^= OpenTime.GetHashCode();
		}
		if (RankTotalNum != 0)
		{
			num ^= RankTotalNum.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (CityId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CityId);
		}
		if (GameCityState != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(GameCityState);
		}
		if (Uid.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Uid);
		}
		if (UserName.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(UserName);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Pic);
		}
		if (PicVer != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(PicVer);
		}
		if (HeadSkinId != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(HeadSkinId);
		}
		if (HeadSkinET != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(HeadSkinET);
		}
		if (ServerId != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(ServerId);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(StartTime);
		}
		if (EndTime != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(EndTime);
		}
		if (OpenTime != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(OpenTime);
		}
		if (RankTotalNum != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(RankTotalNum);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (CityId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CityId);
		}
		if (GameCityState != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GameCityState);
		}
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (UserName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(UserName);
		}
		if (Pic.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Pic);
		}
		if (PicVer != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PicVer);
		}
		if (HeadSkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadSkinId);
		}
		if (HeadSkinET != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(HeadSkinET);
		}
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (EndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(EndTime);
		}
		if (OpenTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(OpenTime);
		}
		if (RankTotalNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RankTotalNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CityMiniGamePointInfo other)
	{
		if (other != null)
		{
			if (other.CityId != 0)
			{
				CityId = other.CityId;
			}
			if (other.GameCityState != 0)
			{
				GameCityState = other.GameCityState;
			}
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.UserName.Length != 0)
			{
				UserName = other.UserName;
			}
			if (other.Pic.Length != 0)
			{
				Pic = other.Pic;
			}
			if (other.PicVer != 0)
			{
				PicVer = other.PicVer;
			}
			if (other.HeadSkinId != 0)
			{
				HeadSkinId = other.HeadSkinId;
			}
			if (other.HeadSkinET != 0L)
			{
				HeadSkinET = other.HeadSkinET;
			}
			if (other.ServerId != 0)
			{
				ServerId = other.ServerId;
			}
			if (other.StartTime != 0L)
			{
				StartTime = other.StartTime;
			}
			if (other.EndTime != 0L)
			{
				EndTime = other.EndTime;
			}
			if (other.OpenTime != 0L)
			{
				OpenTime = other.OpenTime;
			}
			if (other.RankTotalNum != 0)
			{
				RankTotalNum = other.RankTotalNum;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				CityId = input.ReadInt32();
				break;
			case 16u:
				GameCityState = input.ReadInt32();
				break;
			case 26u:
				Uid = input.ReadString();
				break;
			case 34u:
				UserName = input.ReadString();
				break;
			case 42u:
				Pic = input.ReadString();
				break;
			case 48u:
				PicVer = input.ReadInt32();
				break;
			case 56u:
				HeadSkinId = input.ReadInt32();
				break;
			case 64u:
				HeadSkinET = input.ReadInt64();
				break;
			case 72u:
				ServerId = input.ReadInt32();
				break;
			case 80u:
				StartTime = input.ReadInt64();
				break;
			case 88u:
				EndTime = input.ReadInt64();
				break;
			case 96u:
				OpenTime = input.ReadInt64();
				break;
			case 104u:
				RankTotalNum = input.ReadInt32();
				break;
			}
		}
	}
}
