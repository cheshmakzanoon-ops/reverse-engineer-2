using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyUnitOtherEffectInfo : IMessage<ArmyUnitOtherEffectInfo>, IMessage, IEquatable<ArmyUnitOtherEffectInfo>, IDeepCloneable<ArmyUnitOtherEffectInfo>
{
	private static readonly MessageParser<ArmyUnitOtherEffectInfo> _parser = new MessageParser<ArmyUnitOtherEffectInfo>(() => new ArmyUnitOtherEffectInfo());

	private UnknownFieldSet _unknownFields;

	public const int IsAddFieldNumber = 1;

	private bool isAdd_;

	public const int OtherEffectMapFieldNumber = 2;

	private static readonly MapField<int, OtherEffectTab>.Codec _map_otherEffectMap_codec = new MapField<int, OtherEffectTab>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForMessage(18u, OtherEffectTab.Parser), 18u);

	private readonly MapField<int, OtherEffectTab> otherEffectMap_ = new MapField<int, OtherEffectTab>();

	[DebuggerNonUserCode]
	public static MessageParser<ArmyUnitOtherEffectInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[19];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public bool IsAdd
	{
		get
		{
			return isAdd_;
		}
		set
		{
			isAdd_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MapField<int, OtherEffectTab> OtherEffectMap => otherEffectMap_;

	[DebuggerNonUserCode]
	public ArmyUnitOtherEffectInfo()
	{
	}

	[DebuggerNonUserCode]
	public ArmyUnitOtherEffectInfo(ArmyUnitOtherEffectInfo other)
		: this()
	{
		isAdd_ = other.isAdd_;
		otherEffectMap_ = other.otherEffectMap_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyUnitOtherEffectInfo Clone()
	{
		return new ArmyUnitOtherEffectInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyUnitOtherEffectInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyUnitOtherEffectInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (IsAdd != other.IsAdd)
		{
			return false;
		}
		if (!OtherEffectMap.Equals(other.OtherEffectMap))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (IsAdd)
		{
			num ^= IsAdd.GetHashCode();
		}
		num ^= OtherEffectMap.GetHashCode();
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
		if (IsAdd)
		{
			output.WriteRawTag(8);
			output.WriteBool(IsAdd);
		}
		otherEffectMap_.WriteTo(output, _map_otherEffectMap_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (IsAdd)
		{
			num += 2;
		}
		num += otherEffectMap_.CalculateSize(_map_otherEffectMap_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyUnitOtherEffectInfo other)
	{
		if (other != null)
		{
			if (other.IsAdd)
			{
				IsAdd = other.IsAdd;
			}
			otherEffectMap_.Add(other.otherEffectMap_);
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
				IsAdd = input.ReadBool();
				break;
			case 18u:
				otherEffectMap_.AddEntriesFrom(input, _map_otherEffectMap_codec);
				break;
			}
		}
	}
}
