using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceOccupyInfo : IMessage<AllianceOccupyInfo>, IMessage, IEquatable<AllianceOccupyInfo>, IDeepCloneable<AllianceOccupyInfo>
{
	private static readonly MessageParser<AllianceOccupyInfo> _parser = new MessageParser<AllianceOccupyInfo>(() => new AllianceOccupyInfo());

	private UnknownFieldSet _unknownFields;

	public const int AllianceIdFieldNumber = 1;

	private string allianceId_ = "";

	public const int AbbrFieldNumber = 2;

	private string abbr_ = "";

	public const int ColorFieldNumber = 3;

	private int color_;

	public const int CityNameFieldNumber = 4;

	private string cityName_ = "";

	public const int AllianceNameFieldNumber = 5;

	private string allianceName_ = "";

	public const int OccupyServerIdFieldNumber = 6;

	private int occupyServerId_;

	public const int CityInfoFieldNumber = 7;

	private static readonly FieldCodec<CityInfo> _repeated_cityInfo_codec = FieldCodec.ForMessage(58u, Protobuf.CityInfo.Parser);

	private readonly RepeatedField<CityInfo> cityInfo_ = new RepeatedField<CityInfo>();

	public const int DestroyInfoFieldNumber = 8;

	private static readonly FieldCodec<CityInfo> _repeated_destroyInfo_codec = FieldCodec.ForMessage(66u, Protobuf.CityInfo.Parser);

	private readonly RepeatedField<CityInfo> destroyInfo_ = new RepeatedField<CityInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<AllianceOccupyInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AllianceCityRecordProtoReflection.Descriptor.MessageTypes[5];

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
	public int Color
	{
		get
		{
			return color_;
		}
		set
		{
			color_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string CityName
	{
		get
		{
			return cityName_;
		}
		set
		{
			cityName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AllianceName
	{
		get
		{
			return allianceName_;
		}
		set
		{
			allianceName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int OccupyServerId
	{
		get
		{
			return occupyServerId_;
		}
		set
		{
			occupyServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<CityInfo> CityInfo => cityInfo_;

	[DebuggerNonUserCode]
	public RepeatedField<CityInfo> DestroyInfo => destroyInfo_;

	[DebuggerNonUserCode]
	public AllianceOccupyInfo()
	{
	}

	[DebuggerNonUserCode]
	public AllianceOccupyInfo(AllianceOccupyInfo other)
		: this()
	{
		allianceId_ = other.allianceId_;
		abbr_ = other.abbr_;
		color_ = other.color_;
		cityName_ = other.cityName_;
		allianceName_ = other.allianceName_;
		occupyServerId_ = other.occupyServerId_;
		cityInfo_ = other.cityInfo_.Clone();
		destroyInfo_ = other.destroyInfo_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceOccupyInfo Clone()
	{
		return new AllianceOccupyInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceOccupyInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceOccupyInfo other)
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
		if (Abbr != other.Abbr)
		{
			return false;
		}
		if (Color != other.Color)
		{
			return false;
		}
		if (CityName != other.CityName)
		{
			return false;
		}
		if (AllianceName != other.AllianceName)
		{
			return false;
		}
		if (OccupyServerId != other.OccupyServerId)
		{
			return false;
		}
		if (!cityInfo_.Equals(other.cityInfo_))
		{
			return false;
		}
		if (!destroyInfo_.Equals(other.destroyInfo_))
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
		if (Abbr.Length != 0)
		{
			num ^= Abbr.GetHashCode();
		}
		if (Color != 0)
		{
			num ^= Color.GetHashCode();
		}
		if (CityName.Length != 0)
		{
			num ^= CityName.GetHashCode();
		}
		if (AllianceName.Length != 0)
		{
			num ^= AllianceName.GetHashCode();
		}
		if (OccupyServerId != 0)
		{
			num ^= OccupyServerId.GetHashCode();
		}
		num ^= cityInfo_.GetHashCode();
		num ^= destroyInfo_.GetHashCode();
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
		if (Abbr.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Abbr);
		}
		if (Color != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Color);
		}
		if (CityName.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(CityName);
		}
		if (AllianceName.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(AllianceName);
		}
		if (OccupyServerId != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(OccupyServerId);
		}
		cityInfo_.WriteTo(output, _repeated_cityInfo_codec);
		destroyInfo_.WriteTo(output, _repeated_destroyInfo_codec);
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
		if (Abbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Abbr);
		}
		if (Color != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Color);
		}
		if (CityName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CityName);
		}
		if (AllianceName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceName);
		}
		if (OccupyServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OccupyServerId);
		}
		num += cityInfo_.CalculateSize(_repeated_cityInfo_codec);
		num += destroyInfo_.CalculateSize(_repeated_destroyInfo_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceOccupyInfo other)
	{
		if (other != null)
		{
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.Abbr.Length != 0)
			{
				Abbr = other.Abbr;
			}
			if (other.Color != 0)
			{
				Color = other.Color;
			}
			if (other.CityName.Length != 0)
			{
				CityName = other.CityName;
			}
			if (other.AllianceName.Length != 0)
			{
				AllianceName = other.AllianceName;
			}
			if (other.OccupyServerId != 0)
			{
				OccupyServerId = other.OccupyServerId;
			}
			cityInfo_.Add(other.cityInfo_);
			destroyInfo_.Add(other.destroyInfo_);
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
			case 18u:
				Abbr = input.ReadString();
				break;
			case 24u:
				Color = input.ReadInt32();
				break;
			case 34u:
				CityName = input.ReadString();
				break;
			case 42u:
				AllianceName = input.ReadString();
				break;
			case 48u:
				OccupyServerId = input.ReadInt32();
				break;
			case 58u:
				cityInfo_.AddEntriesFrom(input, _repeated_cityInfo_codec);
				break;
			case 66u:
				destroyInfo_.AddEntriesFrom(input, _repeated_destroyInfo_codec);
				break;
			}
		}
	}
}
