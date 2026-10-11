using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityAltarPointInfo : IMessage<CityAltarPointInfo>, IMessage, IEquatable<CityAltarPointInfo>, IDeepCloneable<CityAltarPointInfo>
{
	private static readonly MessageParser<CityAltarPointInfo> _parser = new MessageParser<CityAltarPointInfo>(() => new CityAltarPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int CfgidFieldNumber = 2;

	private int cfgid_;

	public const int OwnerFieldNumber = 3;

	private CityAltarUserInfo owner_;

	public const int TmpOwnerFieldNumber = 4;

	private CityAltarUserInfo tmpOwner_;

	public const int OccupylistFieldNumber = 5;

	private static readonly FieldCodec<CityAltarOccupyInfo> _repeated_occupylist_codec = FieldCodec.ForMessage(42u, CityAltarOccupyInfo.Parser);

	private readonly RepeatedField<CityAltarOccupyInfo> occupylist_ = new RepeatedField<CityAltarOccupyInfo>();

	public const int GiveupTimeFieldNumber = 6;

	private long giveupTime_;

	public const int BattleStartTimeFieldNumber = 7;

	private long battleStartTime_;

	public const int BattleEndTimeFieldNumber = 8;

	private long battleEndTime_;

	public const int FishStateFieldNumber = 9;

	private int fishState_;

	public const int FirstOwnerFieldNumber = 10;

	private CityAltarUserInfo firstOwner_;

	public const int FirstOccupyTimeFieldNumber = 11;

	private long firstOccupyTime_;

	[DebuggerNonUserCode]
	public static MessageParser<CityAltarPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[34];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Cfgid
	{
		get
		{
			return cfgid_;
		}
		set
		{
			cfgid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityAltarUserInfo Owner
	{
		get
		{
			return owner_;
		}
		set
		{
			owner_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityAltarUserInfo TmpOwner
	{
		get
		{
			return tmpOwner_;
		}
		set
		{
			tmpOwner_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<CityAltarOccupyInfo> Occupylist => occupylist_;

	[DebuggerNonUserCode]
	public long GiveupTime
	{
		get
		{
			return giveupTime_;
		}
		set
		{
			giveupTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long BattleStartTime
	{
		get
		{
			return battleStartTime_;
		}
		set
		{
			battleStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long BattleEndTime
	{
		get
		{
			return battleEndTime_;
		}
		set
		{
			battleEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FishState
	{
		get
		{
			return fishState_;
		}
		set
		{
			fishState_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityAltarUserInfo FirstOwner
	{
		get
		{
			return firstOwner_;
		}
		set
		{
			firstOwner_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FirstOccupyTime
	{
		get
		{
			return firstOccupyTime_;
		}
		set
		{
			firstOccupyTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityAltarPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public CityAltarPointInfo(CityAltarPointInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		cfgid_ = other.cfgid_;
		owner_ = ((other.owner_ != null) ? other.owner_.Clone() : null);
		tmpOwner_ = ((other.tmpOwner_ != null) ? other.tmpOwner_.Clone() : null);
		occupylist_ = other.occupylist_.Clone();
		giveupTime_ = other.giveupTime_;
		battleStartTime_ = other.battleStartTime_;
		battleEndTime_ = other.battleEndTime_;
		fishState_ = other.fishState_;
		firstOwner_ = ((other.firstOwner_ != null) ? other.firstOwner_.Clone() : null);
		firstOccupyTime_ = other.firstOccupyTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CityAltarPointInfo Clone()
	{
		return new CityAltarPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityAltarPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityAltarPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (Cfgid != other.Cfgid)
		{
			return false;
		}
		if (!object.Equals(Owner, other.Owner))
		{
			return false;
		}
		if (!object.Equals(TmpOwner, other.TmpOwner))
		{
			return false;
		}
		if (!occupylist_.Equals(other.occupylist_))
		{
			return false;
		}
		if (GiveupTime != other.GiveupTime)
		{
			return false;
		}
		if (BattleStartTime != other.BattleStartTime)
		{
			return false;
		}
		if (BattleEndTime != other.BattleEndTime)
		{
			return false;
		}
		if (FishState != other.FishState)
		{
			return false;
		}
		if (!object.Equals(FirstOwner, other.FirstOwner))
		{
			return false;
		}
		if (FirstOccupyTime != other.FirstOccupyTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (Cfgid != 0)
		{
			num ^= Cfgid.GetHashCode();
		}
		if (owner_ != null)
		{
			num ^= Owner.GetHashCode();
		}
		if (tmpOwner_ != null)
		{
			num ^= TmpOwner.GetHashCode();
		}
		num ^= occupylist_.GetHashCode();
		if (GiveupTime != 0L)
		{
			num ^= GiveupTime.GetHashCode();
		}
		if (BattleStartTime != 0L)
		{
			num ^= BattleStartTime.GetHashCode();
		}
		if (BattleEndTime != 0L)
		{
			num ^= BattleEndTime.GetHashCode();
		}
		if (FishState != 0)
		{
			num ^= FishState.GetHashCode();
		}
		if (firstOwner_ != null)
		{
			num ^= FirstOwner.GetHashCode();
		}
		if (FirstOccupyTime != 0L)
		{
			num ^= FirstOccupyTime.GetHashCode();
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
		if (Uuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Uuid);
		}
		if (Cfgid != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Cfgid);
		}
		if (owner_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(Owner);
		}
		if (tmpOwner_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(TmpOwner);
		}
		occupylist_.WriteTo(output, _repeated_occupylist_codec);
		if (GiveupTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(GiveupTime);
		}
		if (BattleStartTime != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(BattleStartTime);
		}
		if (BattleEndTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(BattleEndTime);
		}
		if (FishState != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(FishState);
		}
		if (firstOwner_ != null)
		{
			output.WriteRawTag(82);
			output.WriteMessage(FirstOwner);
		}
		if (FirstOccupyTime != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(FirstOccupyTime);
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
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (Cfgid != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Cfgid);
		}
		if (owner_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Owner);
		}
		if (tmpOwner_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(TmpOwner);
		}
		num += occupylist_.CalculateSize(_repeated_occupylist_codec);
		if (GiveupTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(GiveupTime);
		}
		if (BattleStartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(BattleStartTime);
		}
		if (BattleEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(BattleEndTime);
		}
		if (FishState != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FishState);
		}
		if (firstOwner_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(FirstOwner);
		}
		if (FirstOccupyTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FirstOccupyTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CityAltarPointInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
		}
		if (other.Cfgid != 0)
		{
			Cfgid = other.Cfgid;
		}
		if (other.owner_ != null)
		{
			if (owner_ == null)
			{
				Owner = new CityAltarUserInfo();
			}
			Owner.MergeFrom(other.Owner);
		}
		if (other.tmpOwner_ != null)
		{
			if (tmpOwner_ == null)
			{
				TmpOwner = new CityAltarUserInfo();
			}
			TmpOwner.MergeFrom(other.TmpOwner);
		}
		occupylist_.Add(other.occupylist_);
		if (other.GiveupTime != 0L)
		{
			GiveupTime = other.GiveupTime;
		}
		if (other.BattleStartTime != 0L)
		{
			BattleStartTime = other.BattleStartTime;
		}
		if (other.BattleEndTime != 0L)
		{
			BattleEndTime = other.BattleEndTime;
		}
		if (other.FishState != 0)
		{
			FishState = other.FishState;
		}
		if (other.firstOwner_ != null)
		{
			if (firstOwner_ == null)
			{
				FirstOwner = new CityAltarUserInfo();
			}
			FirstOwner.MergeFrom(other.FirstOwner);
		}
		if (other.FirstOccupyTime != 0L)
		{
			FirstOccupyTime = other.FirstOccupyTime;
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				Uuid = input.ReadInt64();
				break;
			case 16u:
				Cfgid = input.ReadInt32();
				break;
			case 26u:
				if (owner_ == null)
				{
					Owner = new CityAltarUserInfo();
				}
				input.ReadMessage(Owner);
				break;
			case 34u:
				if (tmpOwner_ == null)
				{
					TmpOwner = new CityAltarUserInfo();
				}
				input.ReadMessage(TmpOwner);
				break;
			case 42u:
				occupylist_.AddEntriesFrom(input, _repeated_occupylist_codec);
				break;
			case 48u:
				GiveupTime = input.ReadInt64();
				break;
			case 56u:
				BattleStartTime = input.ReadInt64();
				break;
			case 64u:
				BattleEndTime = input.ReadInt64();
				break;
			case 72u:
				FishState = input.ReadInt32();
				break;
			case 82u:
				if (firstOwner_ == null)
				{
					FirstOwner = new CityAltarUserInfo();
				}
				input.ReadMessage(FirstOwner);
				break;
			case 88u:
				FirstOccupyTime = input.ReadInt64();
				break;
			}
		}
	}
}
