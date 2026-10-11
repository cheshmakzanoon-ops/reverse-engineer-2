using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WorldAllianceOccupyInfo : IMessage<WorldAllianceOccupyInfo>, IMessage, IEquatable<WorldAllianceOccupyInfo>, IDeepCloneable<WorldAllianceOccupyInfo>
{
	private static readonly MessageParser<WorldAllianceOccupyInfo> _parser = new MessageParser<WorldAllianceOccupyInfo>(() => new WorldAllianceOccupyInfo());

	private UnknownFieldSet _unknownFields;

	public const int AllianceOccupyInfoFieldNumber = 1;

	private static readonly FieldCodec<AllianceOccupyInfo> _repeated_allianceOccupyInfo_codec = FieldCodec.ForMessage(10u, Protobuf.AllianceOccupyInfo.Parser);

	private readonly RepeatedField<AllianceOccupyInfo> allianceOccupyInfo_ = new RepeatedField<AllianceOccupyInfo>();

	public const int ServerDestroyInfoFieldNumber = 2;

	private static readonly FieldCodec<ServerDestroyInfo> _repeated_serverDestroyInfo_codec = FieldCodec.ForMessage(18u, Protobuf.ServerDestroyInfo.Parser);

	private readonly RepeatedField<ServerDestroyInfo> serverDestroyInfo_ = new RepeatedField<ServerDestroyInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<WorldAllianceOccupyInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AllianceCityRecordProtoReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<AllianceOccupyInfo> AllianceOccupyInfo => allianceOccupyInfo_;

	[DebuggerNonUserCode]
	public RepeatedField<ServerDestroyInfo> ServerDestroyInfo => serverDestroyInfo_;

	[DebuggerNonUserCode]
	public WorldAllianceOccupyInfo()
	{
	}

	[DebuggerNonUserCode]
	public WorldAllianceOccupyInfo(WorldAllianceOccupyInfo other)
		: this()
	{
		allianceOccupyInfo_ = other.allianceOccupyInfo_.Clone();
		serverDestroyInfo_ = other.serverDestroyInfo_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WorldAllianceOccupyInfo Clone()
	{
		return new WorldAllianceOccupyInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WorldAllianceOccupyInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(WorldAllianceOccupyInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!allianceOccupyInfo_.Equals(other.allianceOccupyInfo_))
		{
			return false;
		}
		if (!serverDestroyInfo_.Equals(other.serverDestroyInfo_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= allianceOccupyInfo_.GetHashCode();
		num ^= serverDestroyInfo_.GetHashCode();
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
		allianceOccupyInfo_.WriteTo(output, _repeated_allianceOccupyInfo_codec);
		serverDestroyInfo_.WriteTo(output, _repeated_serverDestroyInfo_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += allianceOccupyInfo_.CalculateSize(_repeated_allianceOccupyInfo_codec);
		num += serverDestroyInfo_.CalculateSize(_repeated_serverDestroyInfo_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WorldAllianceOccupyInfo other)
	{
		if (other != null)
		{
			allianceOccupyInfo_.Add(other.allianceOccupyInfo_);
			serverDestroyInfo_.Add(other.serverDestroyInfo_);
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
				allianceOccupyInfo_.AddEntriesFrom(input, _repeated_allianceOccupyInfo_codec);
				break;
			case 18u:
				serverDestroyInfo_.AddEntriesFrom(input, _repeated_serverDestroyInfo_codec);
				break;
			}
		}
	}
}
