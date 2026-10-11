using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WorldMarchDeltaMeta : IMessage<WorldMarchDeltaMeta>, IMessage, IEquatable<WorldMarchDeltaMeta>, IDeepCloneable<WorldMarchDeltaMeta>
{
	private static readonly MessageParser<WorldMarchDeltaMeta> _parser = new MessageParser<WorldMarchDeltaMeta>(() => new WorldMarchDeltaMeta());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private int type_;

	public const int OneServerMarchFieldNumber = 2;

	private bool oneServerMarch_;

	[DebuggerNonUserCode]
	public static MessageParser<WorldMarchDeltaMeta> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool OneServerMarch
	{
		get
		{
			return oneServerMarch_;
		}
		set
		{
			oneServerMarch_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WorldMarchDeltaMeta()
	{
	}

	[DebuggerNonUserCode]
	public WorldMarchDeltaMeta(WorldMarchDeltaMeta other)
		: this()
	{
		type_ = other.type_;
		oneServerMarch_ = other.oneServerMarch_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WorldMarchDeltaMeta Clone()
	{
		return new WorldMarchDeltaMeta(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WorldMarchDeltaMeta);
	}

	[DebuggerNonUserCode]
	public bool Equals(WorldMarchDeltaMeta other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (OneServerMarch != other.OneServerMarch)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (OneServerMarch)
		{
			num ^= OneServerMarch.GetHashCode();
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
		if (Type != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Type);
		}
		if (OneServerMarch)
		{
			output.WriteRawTag(16);
			output.WriteBool(OneServerMarch);
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
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (OneServerMarch)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WorldMarchDeltaMeta other)
	{
		if (other != null)
		{
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.OneServerMarch)
			{
				OneServerMarch = other.OneServerMarch;
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
				Type = input.ReadInt32();
				break;
			case 16u:
				OneServerMarch = input.ReadBool();
				break;
			}
		}
	}
}
