using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class refreshBuffInfo : IMessage<refreshBuffInfo>, IMessage, IEquatable<refreshBuffInfo>, IDeepCloneable<refreshBuffInfo>
{
	private static readonly MessageParser<refreshBuffInfo> _parser = new MessageParser<refreshBuffInfo>(() => new refreshBuffInfo());

	private UnknownFieldSet _unknownFields;

	public const int RefreshTimeFieldNumber = 1;

	private long refreshTime_;

	public const int BuffIdFieldNumber = 2;

	private int buffId_;

	[DebuggerNonUserCode]
	public static MessageParser<refreshBuffInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[46];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long RefreshTime
	{
		get
		{
			return refreshTime_;
		}
		set
		{
			refreshTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BuffId
	{
		get
		{
			return buffId_;
		}
		set
		{
			buffId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public refreshBuffInfo()
	{
	}

	[DebuggerNonUserCode]
	public refreshBuffInfo(refreshBuffInfo other)
		: this()
	{
		refreshTime_ = other.refreshTime_;
		buffId_ = other.buffId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public refreshBuffInfo Clone()
	{
		return new refreshBuffInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as refreshBuffInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(refreshBuffInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (RefreshTime != other.RefreshTime)
		{
			return false;
		}
		if (BuffId != other.BuffId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (RefreshTime != 0L)
		{
			num ^= RefreshTime.GetHashCode();
		}
		if (BuffId != 0)
		{
			num ^= BuffId.GetHashCode();
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
		if (RefreshTime != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(RefreshTime);
		}
		if (BuffId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(BuffId);
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
		if (RefreshTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(RefreshTime);
		}
		if (BuffId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuffId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(refreshBuffInfo other)
	{
		if (other != null)
		{
			if (other.RefreshTime != 0L)
			{
				RefreshTime = other.RefreshTime;
			}
			if (other.BuffId != 0)
			{
				BuffId = other.BuffId;
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
				RefreshTime = input.ReadInt64();
				break;
			case 16u:
				BuffId = input.ReadInt32();
				break;
			}
		}
	}
}
