using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ServerDestroyInfo : IMessage<ServerDestroyInfo>, IMessage, IEquatable<ServerDestroyInfo>, IDeepCloneable<ServerDestroyInfo>
{
	private static readonly MessageParser<ServerDestroyInfo> _parser = new MessageParser<ServerDestroyInfo>(() => new ServerDestroyInfo());

	private UnknownFieldSet _unknownFields;

	public const int ServerIdFieldNumber = 1;

	private int serverId_;

	public const int DestroyCityIdsFieldNumber = 2;

	private static readonly FieldCodec<int> _repeated_destroyCityIds_codec = FieldCodec.ForInt32(18u);

	private readonly RepeatedField<int> destroyCityIds_ = new RepeatedField<int>();

	[DebuggerNonUserCode]
	public static MessageParser<ServerDestroyInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AllianceCityRecordProtoReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ServerId
	{
		get
		{
			return serverId_;
		}
		set
		{
			serverId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<int> DestroyCityIds => destroyCityIds_;

	[DebuggerNonUserCode]
	public ServerDestroyInfo()
	{
	}

	[DebuggerNonUserCode]
	public ServerDestroyInfo(ServerDestroyInfo other)
		: this()
	{
		serverId_ = other.serverId_;
		destroyCityIds_ = other.destroyCityIds_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ServerDestroyInfo Clone()
	{
		return new ServerDestroyInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ServerDestroyInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ServerDestroyInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (!destroyCityIds_.Equals(other.destroyCityIds_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		num ^= destroyCityIds_.GetHashCode();
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
		if (ServerId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ServerId);
		}
		destroyCityIds_.WriteTo(output, _repeated_destroyCityIds_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		num += destroyCityIds_.CalculateSize(_repeated_destroyCityIds_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ServerDestroyInfo other)
	{
		if (other != null)
		{
			if (other.ServerId != 0)
			{
				ServerId = other.ServerId;
			}
			destroyCityIds_.Add(other.destroyCityIds_);
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
				ServerId = input.ReadInt32();
				break;
			case 16u:
			case 18u:
				destroyCityIds_.AddEntriesFrom(input, _repeated_destroyCityIds_codec);
				break;
			}
		}
	}
}
