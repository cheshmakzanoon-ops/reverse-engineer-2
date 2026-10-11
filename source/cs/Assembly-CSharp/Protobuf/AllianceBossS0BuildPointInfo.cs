using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceBossS0BuildPointInfo : IMessage<AllianceBossS0BuildPointInfo>, IMessage, IEquatable<AllianceBossS0BuildPointInfo>, IDeepCloneable<AllianceBossS0BuildPointInfo>
{
	private static readonly MessageParser<AllianceBossS0BuildPointInfo> _parser = new MessageParser<AllianceBossS0BuildPointInfo>(() => new AllianceBossS0BuildPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int AllianceIdFieldNumber = 1;

	private string allianceId_ = "";

	public const int CfgIdFieldNumber = 2;

	private int cfgId_;

	public const int AbbrFieldNumber = 3;

	private string abbr_ = "";

	public const int StartTimeFieldNumber = 4;

	private long startTime_;

	public const int LastReserveTimeFieldNumber = 5;

	private long lastReserveTime_;

	public const int ActEndTimeFieldNumber = 6;

	private long actEndTime_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceBossS0BuildPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[55];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string AllianceId
	{
		get
		{
			return allianceId_;
		}
		set
		{
			allianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int CfgId
	{
		get
		{
			return cfgId_;
		}
		set
		{
			cfgId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Abbr
	{
		get
		{
			return abbr_;
		}
		set
		{
			abbr_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public long LastReserveTime
	{
		get
		{
			return lastReserveTime_;
		}
		set
		{
			lastReserveTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ActEndTime
	{
		get
		{
			return actEndTime_;
		}
		set
		{
			actEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceBossS0BuildPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public AllianceBossS0BuildPointInfo(AllianceBossS0BuildPointInfo other)
		: this()
	{
		allianceId_ = other.allianceId_;
		cfgId_ = other.cfgId_;
		abbr_ = other.abbr_;
		startTime_ = other.startTime_;
		lastReserveTime_ = other.lastReserveTime_;
		actEndTime_ = other.actEndTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceBossS0BuildPointInfo Clone()
	{
		return new AllianceBossS0BuildPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceBossS0BuildPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceBossS0BuildPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (CfgId != other.CfgId)
		{
			return false;
		}
		if (Abbr != other.Abbr)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (LastReserveTime != other.LastReserveTime)
		{
			return false;
		}
		if (ActEndTime != other.ActEndTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (CfgId != 0)
		{
			num ^= CfgId.GetHashCode();
		}
		if (Abbr.Length != 0)
		{
			num ^= Abbr.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (LastReserveTime != 0L)
		{
			num ^= LastReserveTime.GetHashCode();
		}
		if (ActEndTime != 0L)
		{
			num ^= ActEndTime.GetHashCode();
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
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(AllianceId);
		}
		if (CfgId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(CfgId);
		}
		if (Abbr.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Abbr);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(StartTime);
		}
		if (LastReserveTime != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(LastReserveTime);
		}
		if (ActEndTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(ActEndTime);
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
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (CfgId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CfgId);
		}
		if (Abbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Abbr);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (LastReserveTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(LastReserveTime);
		}
		if (ActEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ActEndTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceBossS0BuildPointInfo other)
	{
		if (other != null)
		{
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.CfgId != 0)
			{
				CfgId = other.CfgId;
			}
			if (other.Abbr.Length != 0)
			{
				Abbr = other.Abbr;
			}
			if (other.StartTime != 0L)
			{
				StartTime = other.StartTime;
			}
			if (other.LastReserveTime != 0L)
			{
				LastReserveTime = other.LastReserveTime;
			}
			if (other.ActEndTime != 0L)
			{
				ActEndTime = other.ActEndTime;
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
			case 10u:
				AllianceId = input.ReadString();
				break;
			case 16u:
				CfgId = input.ReadInt32();
				break;
			case 26u:
				Abbr = input.ReadString();
				break;
			case 32u:
				StartTime = input.ReadInt64();
				break;
			case 40u:
				LastReserveTime = input.ReadInt64();
				break;
			case 48u:
				ActEndTime = input.ReadInt64();
				break;
			}
		}
	}
}
