using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ZWLBuildingPoint : IMessage<ZWLBuildingPoint>, IMessage, IEquatable<ZWLBuildingPoint>, IDeepCloneable<ZWLBuildingPoint>
{
	private static readonly MessageParser<ZWLBuildingPoint> _parser = new MessageParser<ZWLBuildingPoint>(() => new ZWLBuildingPoint());

	private UnknownFieldSet _unknownFields;

	public const int CityIdFieldNumber = 1;

	private int cityId_;

	public const int TypeFieldNumber = 2;

	private int type_;

	public const int StateFieldNumber = 3;

	private int state_;

	public const int OwnerCampIdFieldNumber = 4;

	private int ownerCampId_;

	public const int TmpOwnerCampIdFieldNumber = 5;

	private int tmpOwnerCampId_;

	public const int FixStartTimeFieldNumber = 6;

	private long fixStartTime_;

	public const int FixEndTimeFieldNumber = 7;

	private long fixEndTime_;

	public const int ProgressFieldNumber = 8;

	private int progress_;

	public const int ProgressMaxFieldNumber = 9;

	private int progressMax_;

	public const int OccupyStartTimeFieldNumber = 10;

	private long occupyStartTime_;

	public const int OccupyStartProgressFieldNumber = 11;

	private int occupyStartProgress_;

	public const int FireTimeFieldNumber = 12;

	private long fireTime_;

	public const int BuffInfoFieldNumber = 13;

	private refreshBuffInfo buffInfo_;

	public const int OverTimeFieldNumber = 14;

	private long overTime_;

	public const int EffectFieldNumber = 15;

	private static readonly MapField<int, float>.Codec _map_effect_codec = new MapField<int, float>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForFloat(21u, 0f), 122u);

	private readonly MapField<int, float> effect_ = new MapField<int, float>();

	public const int AlAbbrFieldNumber = 16;

	private string alAbbr_ = "";

	public const int AlServerIdFieldNumber = 17;

	private int alServerId_;

	public const int UnlockTimeFieldNumber = 18;

	private long unlockTime_;

	public const int BattleStartTimeFieldNumber = 19;

	private long battleStartTime_;

	public const int BattleEndTimeFieldNumber = 20;

	private long battleEndTime_;

	[DebuggerNonUserCode]
	public static MessageParser<ZWLBuildingPoint> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[45];

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
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int State
	{
		get
		{
			return state_;
		}
		set
		{
			state_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OwnerCampId
	{
		get
		{
			return ownerCampId_;
		}
		set
		{
			ownerCampId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TmpOwnerCampId
	{
		get
		{
			return tmpOwnerCampId_;
		}
		set
		{
			tmpOwnerCampId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FixStartTime
	{
		get
		{
			return fixStartTime_;
		}
		set
		{
			fixStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FixEndTime
	{
		get
		{
			return fixEndTime_;
		}
		set
		{
			fixEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Progress
	{
		get
		{
			return progress_;
		}
		set
		{
			progress_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ProgressMax
	{
		get
		{
			return progressMax_;
		}
		set
		{
			progressMax_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long OccupyStartTime
	{
		get
		{
			return occupyStartTime_;
		}
		set
		{
			occupyStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OccupyStartProgress
	{
		get
		{
			return occupyStartProgress_;
		}
		set
		{
			occupyStartProgress_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FireTime
	{
		get
		{
			return fireTime_;
		}
		set
		{
			fireTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public refreshBuffInfo BuffInfo
	{
		get
		{
			return buffInfo_;
		}
		set
		{
			buffInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long OverTime
	{
		get
		{
			return overTime_;
		}
		set
		{
			overTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MapField<int, float> Effect => effect_;

	[DebuggerNonUserCode]
	public string AlAbbr
	{
		get
		{
			return alAbbr_;
		}
		set
		{
			alAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int AlServerId
	{
		get
		{
			return alServerId_;
		}
		set
		{
			alServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UnlockTime
	{
		get
		{
			return unlockTime_;
		}
		set
		{
			unlockTime_ = value;
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
	public ZWLBuildingPoint()
	{
	}

	[DebuggerNonUserCode]
	public ZWLBuildingPoint(ZWLBuildingPoint other)
		: this()
	{
		cityId_ = other.cityId_;
		type_ = other.type_;
		state_ = other.state_;
		ownerCampId_ = other.ownerCampId_;
		tmpOwnerCampId_ = other.tmpOwnerCampId_;
		fixStartTime_ = other.fixStartTime_;
		fixEndTime_ = other.fixEndTime_;
		progress_ = other.progress_;
		progressMax_ = other.progressMax_;
		occupyStartTime_ = other.occupyStartTime_;
		occupyStartProgress_ = other.occupyStartProgress_;
		fireTime_ = other.fireTime_;
		buffInfo_ = ((other.buffInfo_ != null) ? other.buffInfo_.Clone() : null);
		overTime_ = other.overTime_;
		effect_ = other.effect_.Clone();
		alAbbr_ = other.alAbbr_;
		alServerId_ = other.alServerId_;
		unlockTime_ = other.unlockTime_;
		battleStartTime_ = other.battleStartTime_;
		battleEndTime_ = other.battleEndTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ZWLBuildingPoint Clone()
	{
		return new ZWLBuildingPoint(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ZWLBuildingPoint);
	}

	[DebuggerNonUserCode]
	public bool Equals(ZWLBuildingPoint other)
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
		if (Type != other.Type)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (OwnerCampId != other.OwnerCampId)
		{
			return false;
		}
		if (TmpOwnerCampId != other.TmpOwnerCampId)
		{
			return false;
		}
		if (FixStartTime != other.FixStartTime)
		{
			return false;
		}
		if (FixEndTime != other.FixEndTime)
		{
			return false;
		}
		if (Progress != other.Progress)
		{
			return false;
		}
		if (ProgressMax != other.ProgressMax)
		{
			return false;
		}
		if (OccupyStartTime != other.OccupyStartTime)
		{
			return false;
		}
		if (OccupyStartProgress != other.OccupyStartProgress)
		{
			return false;
		}
		if (FireTime != other.FireTime)
		{
			return false;
		}
		if (!object.Equals(BuffInfo, other.BuffInfo))
		{
			return false;
		}
		if (OverTime != other.OverTime)
		{
			return false;
		}
		if (!Effect.Equals(other.Effect))
		{
			return false;
		}
		if (AlAbbr != other.AlAbbr)
		{
			return false;
		}
		if (AlServerId != other.AlServerId)
		{
			return false;
		}
		if (UnlockTime != other.UnlockTime)
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
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (OwnerCampId != 0)
		{
			num ^= OwnerCampId.GetHashCode();
		}
		if (TmpOwnerCampId != 0)
		{
			num ^= TmpOwnerCampId.GetHashCode();
		}
		if (FixStartTime != 0L)
		{
			num ^= FixStartTime.GetHashCode();
		}
		if (FixEndTime != 0L)
		{
			num ^= FixEndTime.GetHashCode();
		}
		if (Progress != 0)
		{
			num ^= Progress.GetHashCode();
		}
		if (ProgressMax != 0)
		{
			num ^= ProgressMax.GetHashCode();
		}
		if (OccupyStartTime != 0L)
		{
			num ^= OccupyStartTime.GetHashCode();
		}
		if (OccupyStartProgress != 0)
		{
			num ^= OccupyStartProgress.GetHashCode();
		}
		if (FireTime != 0L)
		{
			num ^= FireTime.GetHashCode();
		}
		if (buffInfo_ != null)
		{
			num ^= BuffInfo.GetHashCode();
		}
		if (OverTime != 0L)
		{
			num ^= OverTime.GetHashCode();
		}
		num ^= Effect.GetHashCode();
		if (AlAbbr.Length != 0)
		{
			num ^= AlAbbr.GetHashCode();
		}
		if (AlServerId != 0)
		{
			num ^= AlServerId.GetHashCode();
		}
		if (UnlockTime != 0L)
		{
			num ^= UnlockTime.GetHashCode();
		}
		if (BattleStartTime != 0L)
		{
			num ^= BattleStartTime.GetHashCode();
		}
		if (BattleEndTime != 0L)
		{
			num ^= BattleEndTime.GetHashCode();
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
		if (Type != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Type);
		}
		if (State != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(State);
		}
		if (OwnerCampId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(OwnerCampId);
		}
		if (TmpOwnerCampId != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(TmpOwnerCampId);
		}
		if (FixStartTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(FixStartTime);
		}
		if (FixEndTime != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(FixEndTime);
		}
		if (Progress != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(Progress);
		}
		if (ProgressMax != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(ProgressMax);
		}
		if (OccupyStartTime != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(OccupyStartTime);
		}
		if (OccupyStartProgress != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(OccupyStartProgress);
		}
		if (FireTime != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(FireTime);
		}
		if (buffInfo_ != null)
		{
			output.WriteRawTag(106);
			output.WriteMessage(BuffInfo);
		}
		if (OverTime != 0L)
		{
			output.WriteRawTag(112);
			output.WriteInt64(OverTime);
		}
		effect_.WriteTo(output, _map_effect_codec);
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(130, 1);
			output.WriteString(AlAbbr);
		}
		if (AlServerId != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(AlServerId);
		}
		if (UnlockTime != 0L)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt64(UnlockTime);
		}
		if (BattleStartTime != 0L)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt64(BattleStartTime);
		}
		if (BattleEndTime != 0L)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt64(BattleEndTime);
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
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (OwnerCampId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OwnerCampId);
		}
		if (TmpOwnerCampId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TmpOwnerCampId);
		}
		if (FixStartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FixStartTime);
		}
		if (FixEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FixEndTime);
		}
		if (Progress != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Progress);
		}
		if (ProgressMax != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ProgressMax);
		}
		if (OccupyStartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(OccupyStartTime);
		}
		if (OccupyStartProgress != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OccupyStartProgress);
		}
		if (FireTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FireTime);
		}
		if (buffInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(BuffInfo);
		}
		if (OverTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(OverTime);
		}
		num += effect_.CalculateSize(_map_effect_codec);
		if (AlAbbr.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(AlAbbr);
		}
		if (AlServerId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(AlServerId);
		}
		if (UnlockTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(UnlockTime);
		}
		if (BattleStartTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(BattleStartTime);
		}
		if (BattleEndTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(BattleEndTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ZWLBuildingPoint other)
	{
		if (other == null)
		{
			return;
		}
		if (other.CityId != 0)
		{
			CityId = other.CityId;
		}
		if (other.Type != 0)
		{
			Type = other.Type;
		}
		if (other.State != 0)
		{
			State = other.State;
		}
		if (other.OwnerCampId != 0)
		{
			OwnerCampId = other.OwnerCampId;
		}
		if (other.TmpOwnerCampId != 0)
		{
			TmpOwnerCampId = other.TmpOwnerCampId;
		}
		if (other.FixStartTime != 0L)
		{
			FixStartTime = other.FixStartTime;
		}
		if (other.FixEndTime != 0L)
		{
			FixEndTime = other.FixEndTime;
		}
		if (other.Progress != 0)
		{
			Progress = other.Progress;
		}
		if (other.ProgressMax != 0)
		{
			ProgressMax = other.ProgressMax;
		}
		if (other.OccupyStartTime != 0L)
		{
			OccupyStartTime = other.OccupyStartTime;
		}
		if (other.OccupyStartProgress != 0)
		{
			OccupyStartProgress = other.OccupyStartProgress;
		}
		if (other.FireTime != 0L)
		{
			FireTime = other.FireTime;
		}
		if (other.buffInfo_ != null)
		{
			if (buffInfo_ == null)
			{
				BuffInfo = new refreshBuffInfo();
			}
			BuffInfo.MergeFrom(other.BuffInfo);
		}
		if (other.OverTime != 0L)
		{
			OverTime = other.OverTime;
		}
		effect_.Add(other.effect_);
		if (other.AlAbbr.Length != 0)
		{
			AlAbbr = other.AlAbbr;
		}
		if (other.AlServerId != 0)
		{
			AlServerId = other.AlServerId;
		}
		if (other.UnlockTime != 0L)
		{
			UnlockTime = other.UnlockTime;
		}
		if (other.BattleStartTime != 0L)
		{
			BattleStartTime = other.BattleStartTime;
		}
		if (other.BattleEndTime != 0L)
		{
			BattleEndTime = other.BattleEndTime;
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
				CityId = input.ReadInt32();
				break;
			case 16u:
				Type = input.ReadInt32();
				break;
			case 24u:
				State = input.ReadInt32();
				break;
			case 32u:
				OwnerCampId = input.ReadInt32();
				break;
			case 40u:
				TmpOwnerCampId = input.ReadInt32();
				break;
			case 48u:
				FixStartTime = input.ReadInt64();
				break;
			case 56u:
				FixEndTime = input.ReadInt64();
				break;
			case 64u:
				Progress = input.ReadInt32();
				break;
			case 72u:
				ProgressMax = input.ReadInt32();
				break;
			case 80u:
				OccupyStartTime = input.ReadInt64();
				break;
			case 88u:
				OccupyStartProgress = input.ReadInt32();
				break;
			case 96u:
				FireTime = input.ReadInt64();
				break;
			case 106u:
				if (buffInfo_ == null)
				{
					BuffInfo = new refreshBuffInfo();
				}
				input.ReadMessage(BuffInfo);
				break;
			case 112u:
				OverTime = input.ReadInt64();
				break;
			case 122u:
				effect_.AddEntriesFrom(input, _map_effect_codec);
				break;
			case 130u:
				AlAbbr = input.ReadString();
				break;
			case 136u:
				AlServerId = input.ReadInt32();
				break;
			case 144u:
				UnlockTime = input.ReadInt64();
				break;
			case 152u:
				BattleStartTime = input.ReadInt64();
				break;
			case 160u:
				BattleEndTime = input.ReadInt64();
				break;
			}
		}
	}
}
