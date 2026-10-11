using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceCityShield : IMessage<AllianceCityShield>, IMessage, IEquatable<AllianceCityShield>, IDeepCloneable<AllianceCityShield>
{
	private static readonly MessageParser<AllianceCityShield> _parser = new MessageParser<AllianceCityShield>(() => new AllianceCityShield());

	private UnknownFieldSet _unknownFields;

	public const int ChargeEndTimeFieldNumber = 1;

	private long chargeEndTime_;

	public const int ParticipantAllianceIdsFieldNumber = 2;

	private static readonly FieldCodec<string> _repeated_participantAllianceIds_codec = FieldCodec.ForString(18u);

	private readonly RepeatedField<string> participantAllianceIds_ = new RepeatedField<string>();

	public const int SkillIdFieldNumber = 3;

	private int skillId_;

	public const int StartTimeFieldNumber = 4;

	private long startTime_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceCityShield> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ShieldInfoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long ChargeEndTime
	{
		get
		{
			return chargeEndTime_;
		}
		set
		{
			chargeEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<string> ParticipantAllianceIds => participantAllianceIds_;

	[DebuggerNonUserCode]
	public int SkillId
	{
		get
		{
			return skillId_;
		}
		set
		{
			skillId_ = value;
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
	public AllianceCityShield()
	{
	}

	[DebuggerNonUserCode]
	public AllianceCityShield(AllianceCityShield other)
		: this()
	{
		chargeEndTime_ = other.chargeEndTime_;
		participantAllianceIds_ = other.participantAllianceIds_.Clone();
		skillId_ = other.skillId_;
		startTime_ = other.startTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceCityShield Clone()
	{
		return new AllianceCityShield(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceCityShield);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceCityShield other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ChargeEndTime != other.ChargeEndTime)
		{
			return false;
		}
		if (!participantAllianceIds_.Equals(other.participantAllianceIds_))
		{
			return false;
		}
		if (SkillId != other.SkillId)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ChargeEndTime != 0L)
		{
			num ^= ChargeEndTime.GetHashCode();
		}
		num ^= participantAllianceIds_.GetHashCode();
		if (SkillId != 0)
		{
			num ^= SkillId.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
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
		if (ChargeEndTime != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(ChargeEndTime);
		}
		participantAllianceIds_.WriteTo(output, _repeated_participantAllianceIds_codec);
		if (SkillId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(SkillId);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(StartTime);
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
		if (ChargeEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ChargeEndTime);
		}
		num += participantAllianceIds_.CalculateSize(_repeated_participantAllianceIds_codec);
		if (SkillId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillId);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceCityShield other)
	{
		if (other != null)
		{
			if (other.ChargeEndTime != 0L)
			{
				ChargeEndTime = other.ChargeEndTime;
			}
			participantAllianceIds_.Add(other.participantAllianceIds_);
			if (other.SkillId != 0)
			{
				SkillId = other.SkillId;
			}
			if (other.StartTime != 0L)
			{
				StartTime = other.StartTime;
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
				ChargeEndTime = input.ReadInt64();
				break;
			case 18u:
				participantAllianceIds_.AddEntriesFrom(input, _repeated_participantAllianceIds_codec);
				break;
			case 24u:
				SkillId = input.ReadInt32();
				break;
			case 32u:
				StartTime = input.ReadInt64();
				break;
			}
		}
	}
}
