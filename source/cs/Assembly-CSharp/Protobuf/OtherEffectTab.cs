using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class OtherEffectTab : IMessage<OtherEffectTab>, IMessage, IEquatable<OtherEffectTab>, IDeepCloneable<OtherEffectTab>
{
	private static readonly MessageParser<OtherEffectTab> _parser = new MessageParser<OtherEffectTab>(() => new OtherEffectTab());

	private UnknownFieldSet _unknownFields;

	public const int TabIdFieldNumber = 1;

	private int tabId_;

	public const int EffectsFieldNumber = 2;

	private static readonly FieldCodec<Effect> _repeated_effects_codec = FieldCodec.ForMessage(18u, Effect.Parser);

	private readonly RepeatedField<Effect> effects_ = new RepeatedField<Effect>();

	[DebuggerNonUserCode]
	public static MessageParser<OtherEffectTab> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[20];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int TabId
	{
		get
		{
			return tabId_;
		}
		set
		{
			tabId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Effect> Effects => effects_;

	[DebuggerNonUserCode]
	public OtherEffectTab()
	{
	}

	[DebuggerNonUserCode]
	public OtherEffectTab(OtherEffectTab other)
		: this()
	{
		tabId_ = other.tabId_;
		effects_ = other.effects_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OtherEffectTab Clone()
	{
		return new OtherEffectTab(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OtherEffectTab);
	}

	[DebuggerNonUserCode]
	public bool Equals(OtherEffectTab other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TabId != other.TabId)
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
		if (TabId != 0)
		{
			num ^= TabId.GetHashCode();
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
		if (TabId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(TabId);
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
		if (TabId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TabId);
		}
		num += effects_.CalculateSize(_repeated_effects_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OtherEffectTab other)
	{
		if (other != null)
		{
			if (other.TabId != 0)
			{
				TabId = other.TabId;
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
				TabId = input.ReadInt32();
				break;
			case 18u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			}
		}
	}
}
