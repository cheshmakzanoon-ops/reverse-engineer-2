using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CrocodileMonster : IMessage<CrocodileMonster>, IMessage, IEquatable<CrocodileMonster>, IDeepCloneable<CrocodileMonster>
{
	private static readonly MessageParser<CrocodileMonster> _parser = new MessageParser<CrocodileMonster>(() => new CrocodileMonster());

	private UnknownFieldSet _unknownFields;

	public const int BornPointIdFieldNumber = 1;

	private int bornPointId_;

	public const int LastAttackTimeFieldNumber = 2;

	private long lastAttackTime_;

	public const int CurHpFieldNumber = 3;

	private long curHp_;

	public const int MaxHpFieldNumber = 4;

	private long maxHp_;

	public const int UidFieldNumber = 5;

	private string uid_ = "";

	public const int AllianceIdFieldNumber = 6;

	private string allianceId_ = "";

	public const int UserNameFieldNumber = 7;

	private string userName_ = "";

	public const int AbbrFieldNumber = 8;

	private string abbr_ = "";

	public const int CreateTimeFieldNumber = 9;

	private long createTime_;

	[DebuggerNonUserCode]
	public static MessageParser<CrocodileMonster> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[46];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BornPointId
	{
		get
		{
			return bornPointId_;
		}
		set
		{
			bornPointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long LastAttackTime
	{
		get
		{
			return lastAttackTime_;
		}
		set
		{
			lastAttackTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurHp
	{
		get
		{
			return curHp_;
		}
		set
		{
			curHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MaxHp
	{
		get
		{
			return maxHp_;
		}
		set
		{
			maxHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Uid
	{
		get
		{
			return uid_;
		}
		set
		{
			uid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AllianceId
	{
		get
		{
			return allianceId_;
		}
		set
		{
			allianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string UserName
	{
		get
		{
			return userName_;
		}
		set
		{
			userName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Abbr
	{
		get
		{
			return abbr_;
		}
		set
		{
			abbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long CreateTime
	{
		get
		{
			return createTime_;
		}
		set
		{
			createTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CrocodileMonster()
	{
	}

	[DebuggerNonUserCode]
	public CrocodileMonster(CrocodileMonster other)
		: this()
	{
		bornPointId_ = other.bornPointId_;
		lastAttackTime_ = other.lastAttackTime_;
		curHp_ = other.curHp_;
		maxHp_ = other.maxHp_;
		uid_ = other.uid_;
		allianceId_ = other.allianceId_;
		userName_ = other.userName_;
		abbr_ = other.abbr_;
		createTime_ = other.createTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CrocodileMonster Clone()
	{
		return new CrocodileMonster(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CrocodileMonster);
	}

	[DebuggerNonUserCode]
	public bool Equals(CrocodileMonster other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BornPointId != other.BornPointId)
		{
			return false;
		}
		if (LastAttackTime != other.LastAttackTime)
		{
			return false;
		}
		if (CurHp != other.CurHp)
		{
			return false;
		}
		if (MaxHp != other.MaxHp)
		{
			return false;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (UserName != other.UserName)
		{
			return false;
		}
		if (Abbr != other.Abbr)
		{
			return false;
		}
		if (CreateTime != other.CreateTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BornPointId != 0)
		{
			num ^= BornPointId.GetHashCode();
		}
		if (LastAttackTime != 0L)
		{
			num ^= LastAttackTime.GetHashCode();
		}
		if (CurHp != 0L)
		{
			num ^= CurHp.GetHashCode();
		}
		if (MaxHp != 0L)
		{
			num ^= MaxHp.GetHashCode();
		}
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (UserName.Length != 0)
		{
			num ^= UserName.GetHashCode();
		}
		if (Abbr.Length != 0)
		{
			num ^= Abbr.GetHashCode();
		}
		if (CreateTime != 0L)
		{
			num ^= CreateTime.GetHashCode();
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
		if (BornPointId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BornPointId);
		}
		if (LastAttackTime != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(LastAttackTime);
		}
		if (CurHp != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CurHp);
		}
		if (MaxHp != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(MaxHp);
		}
		if (Uid.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Uid);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(AllianceId);
		}
		if (UserName.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(UserName);
		}
		if (Abbr.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(Abbr);
		}
		if (CreateTime != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(CreateTime);
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
		if (BornPointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BornPointId);
		}
		if (LastAttackTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(LastAttackTime);
		}
		if (CurHp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurHp);
		}
		if (MaxHp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxHp);
		}
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (UserName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(UserName);
		}
		if (Abbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Abbr);
		}
		if (CreateTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CreateTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CrocodileMonster other)
	{
		if (other != null)
		{
			if (other.BornPointId != 0)
			{
				BornPointId = other.BornPointId;
			}
			if (other.LastAttackTime != 0L)
			{
				LastAttackTime = other.LastAttackTime;
			}
			if (other.CurHp != 0L)
			{
				CurHp = other.CurHp;
			}
			if (other.MaxHp != 0L)
			{
				MaxHp = other.MaxHp;
			}
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.UserName.Length != 0)
			{
				UserName = other.UserName;
			}
			if (other.Abbr.Length != 0)
			{
				Abbr = other.Abbr;
			}
			if (other.CreateTime != 0L)
			{
				CreateTime = other.CreateTime;
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
				BornPointId = input.ReadInt32();
				break;
			case 16u:
				LastAttackTime = input.ReadInt64();
				break;
			case 24u:
				CurHp = input.ReadInt64();
				break;
			case 32u:
				MaxHp = input.ReadInt64();
				break;
			case 42u:
				Uid = input.ReadString();
				break;
			case 50u:
				AllianceId = input.ReadString();
				break;
			case 58u:
				UserName = input.ReadString();
				break;
			case 66u:
				Abbr = input.ReadString();
				break;
			case 72u:
				CreateTime = input.ReadInt64();
				break;
			}
		}
	}
}
