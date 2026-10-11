using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class OtherTabPowerInfo : IMessage<OtherTabPowerInfo>, IMessage, IEquatable<OtherTabPowerInfo>, IDeepCloneable<OtherTabPowerInfo>
{
	private static readonly MessageParser<OtherTabPowerInfo> _parser = new MessageParser<OtherTabPowerInfo>(() => new OtherTabPowerInfo());

	private UnknownFieldSet _unknownFields;

	public const int IsOpenFieldNumber = 1;

	private bool isOpen_;

	public const int PowerInfoFieldNumber = 2;

	private static readonly FieldCodec<OtherTabPowerInfoSingle> _repeated_powerInfo_codec = FieldCodec.ForMessage(18u, OtherTabPowerInfoSingle.Parser);

	private readonly RepeatedField<OtherTabPowerInfoSingle> powerInfo_ = new RepeatedField<OtherTabPowerInfoSingle>();

	[DebuggerNonUserCode]
	public static MessageParser<OtherTabPowerInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[11];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public bool IsOpen
	{
		get
		{
			return isOpen_;
		}
		set
		{
			isOpen_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<OtherTabPowerInfoSingle> PowerInfo => powerInfo_;

	[DebuggerNonUserCode]
	public OtherTabPowerInfo()
	{
	}

	[DebuggerNonUserCode]
	public OtherTabPowerInfo(OtherTabPowerInfo other)
		: this()
	{
		isOpen_ = other.isOpen_;
		powerInfo_ = other.powerInfo_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OtherTabPowerInfo Clone()
	{
		return new OtherTabPowerInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OtherTabPowerInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(OtherTabPowerInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (IsOpen != other.IsOpen)
		{
			return false;
		}
		if (!powerInfo_.Equals(other.powerInfo_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (IsOpen)
		{
			num ^= IsOpen.GetHashCode();
		}
		num ^= powerInfo_.GetHashCode();
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
		if (IsOpen)
		{
			output.WriteRawTag(8);
			output.WriteBool(IsOpen);
		}
		powerInfo_.WriteTo(output, _repeated_powerInfo_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (IsOpen)
		{
			num += 2;
		}
		num += powerInfo_.CalculateSize(_repeated_powerInfo_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OtherTabPowerInfo other)
	{
		if (other != null)
		{
			if (other.IsOpen)
			{
				IsOpen = other.IsOpen;
			}
			powerInfo_.Add(other.powerInfo_);
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
				IsOpen = input.ReadBool();
				break;
			case 18u:
				powerInfo_.AddEntriesFrom(input, _repeated_powerInfo_codec);
				break;
			}
		}
	}
}
