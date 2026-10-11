using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class OtherEffectInfo : IMessage<OtherEffectInfo>, IMessage, IEquatable<OtherEffectInfo>, IDeepCloneable<OtherEffectInfo>
{
	private static readonly MessageParser<OtherEffectInfo> _parser = new MessageParser<OtherEffectInfo>(() => new OtherEffectInfo());

	private UnknownFieldSet _unknownFields;

	public const int TabTypeFieldNumber = 1;

	private int tabType_;

	public const int EffectsFieldNumber = 2;

	private static readonly FieldCodec<Effect> _repeated_effects_codec = FieldCodec.ForMessage(18u, Effect.Parser);

	private readonly RepeatedField<Effect> effects_ = new RepeatedField<Effect>();

	[DebuggerNonUserCode]
	public static MessageParser<OtherEffectInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[29];

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
	public RepeatedField<Effect> Effects => effects_;

	[DebuggerNonUserCode]
	public OtherEffectInfo()
	{
	}

	[DebuggerNonUserCode]
	public OtherEffectInfo(OtherEffectInfo other)
		: this()
	{
		tabType_ = other.tabType_;
		effects_ = other.effects_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OtherEffectInfo Clone()
	{
		return new OtherEffectInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OtherEffectInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(OtherEffectInfo other)
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
		if (!effects_.Equals(other.effects_))
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
		num ^= effects_.GetHashCode();
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
		effects_.WriteTo(output, _repeated_effects_codec);
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
		num += effects_.CalculateSize(_repeated_effects_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OtherEffectInfo other)
	{
		if (other != null)
		{
			if (other.TabType != 0)
			{
				TabType = other.TabType;
			}
			effects_.Add(other.effects_);
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
			case 18u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			}
		}
	}
}
