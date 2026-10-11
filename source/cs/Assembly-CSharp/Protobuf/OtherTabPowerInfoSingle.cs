using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class OtherTabPowerInfoSingle : IMessage<OtherTabPowerInfoSingle>, IMessage, IEquatable<OtherTabPowerInfoSingle>, IDeepCloneable<OtherTabPowerInfoSingle>
{
	private static readonly MessageParser<OtherTabPowerInfoSingle> _parser = new MessageParser<OtherTabPowerInfoSingle>(() => new OtherTabPowerInfoSingle());

	private UnknownFieldSet _unknownFields;

	public const int TabTypeFieldNumber = 1;

	private int tabType_;

	public const int ValueFieldNumber = 2;

	private float value_;

	[DebuggerNonUserCode]
	public static MessageParser<OtherTabPowerInfoSingle> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[12];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int TabType
	{
		get
		{
			return tabType_;
		}
		set
		{
			tabType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float Value
	{
		get
		{
			return value_;
		}
		set
		{
			value_ = value;
		}
	}

	[DebuggerNonUserCode]
	public OtherTabPowerInfoSingle()
	{
	}

	[DebuggerNonUserCode]
	public OtherTabPowerInfoSingle(OtherTabPowerInfoSingle other)
		: this()
	{
		tabType_ = other.tabType_;
		value_ = other.value_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OtherTabPowerInfoSingle Clone()
	{
		return new OtherTabPowerInfoSingle(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OtherTabPowerInfoSingle);
	}

	[DebuggerNonUserCode]
	public bool Equals(OtherTabPowerInfoSingle other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TabType != other.TabType)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Value, other.Value))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TabType != 0)
		{
			num ^= TabType.GetHashCode();
		}
		if (Value != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Value);
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
		if (TabType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(TabType);
		}
		if (Value != 0f)
		{
			output.WriteRawTag(21);
			output.WriteFloat(Value);
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
		if (TabType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TabType);
		}
		if (Value != 0f)
		{
			num += 5;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OtherTabPowerInfoSingle other)
	{
		if (other != null)
		{
			if (other.TabType != 0)
			{
				TabType = other.TabType;
			}
			if (other.Value != 0f)
			{
				Value = other.Value;
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
				TabType = input.ReadInt32();
				break;
			case 21u:
				Value = input.ReadFloat();
				break;
			}
		}
	}
}
