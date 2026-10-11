using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class FishPondInfo : IMessage<FishPondInfo>, IMessage, IEquatable<FishPondInfo>, IDeepCloneable<FishPondInfo>
{
	private static readonly MessageParser<FishPondInfo> _parser = new MessageParser<FishPondInfo>(() => new FishPondInfo());

	private UnknownFieldSet _unknownFields;

	public const int FishPlayerListFieldNumber = 1;

	private static readonly FieldCodec<FishPlayerInfo> _repeated_fishPlayerList_codec = FieldCodec.ForMessage(10u, FishPlayerInfo.Parser);

	private readonly RepeatedField<FishPlayerInfo> fishPlayerList_ = new RepeatedField<FishPlayerInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<FishPondInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[31];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<FishPlayerInfo> FishPlayerList => fishPlayerList_;

	[DebuggerNonUserCode]
	public FishPondInfo()
	{
	}

	[DebuggerNonUserCode]
	public FishPondInfo(FishPondInfo other)
		: this()
	{
		fishPlayerList_ = other.fishPlayerList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public FishPondInfo Clone()
	{
		return new FishPondInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as FishPondInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(FishPondInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!fishPlayerList_.Equals(other.fishPlayerList_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= fishPlayerList_.GetHashCode();
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
		fishPlayerList_.WriteTo(output, _repeated_fishPlayerList_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += fishPlayerList_.CalculateSize(_repeated_fishPlayerList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(FishPondInfo other)
	{
		if (other != null)
		{
			fishPlayerList_.Add(other.fishPlayerList_);
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				fishPlayerList_.AddEntriesFrom(input, _repeated_fishPlayerList_codec);
			}
		}
	}
}
