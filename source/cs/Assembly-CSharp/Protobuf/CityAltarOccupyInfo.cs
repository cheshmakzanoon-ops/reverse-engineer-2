using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityAltarOccupyInfo : IMessage<CityAltarOccupyInfo>, IMessage, IEquatable<CityAltarOccupyInfo>, IDeepCloneable<CityAltarOccupyInfo>
{
	private static readonly MessageParser<CityAltarOccupyInfo> _parser = new MessageParser<CityAltarOccupyInfo>(() => new CityAltarOccupyInfo());

	private UnknownFieldSet _unknownFields;

	public const int UserInfoFieldNumber = 1;

	private CityAltarUserInfo userInfo_;

	public const int ScoreFieldNumber = 2;

	private long score_;

	public const int StFieldNumber = 3;

	private long st_;

	public const int EtFieldNumber = 4;

	private long et_;

	public const int SpeedFieldNumber = 5;

	private long speed_;

	[DebuggerNonUserCode]
	public static MessageParser<CityAltarOccupyInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[32];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public CityAltarUserInfo UserInfo
	{
		get
		{
			return userInfo_;
		}
		set
		{
			userInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Score
	{
		get
		{
			return score_;
		}
		set
		{
			score_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long St
	{
		get
		{
			return st_;
		}
		set
		{
			st_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Et
	{
		get
		{
			return et_;
		}
		set
		{
			et_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Speed
	{
		get
		{
			return speed_;
		}
		set
		{
			speed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityAltarOccupyInfo()
	{
	}

	[DebuggerNonUserCode]
	public CityAltarOccupyInfo(CityAltarOccupyInfo other)
		: this()
	{
		userInfo_ = ((other.userInfo_ != null) ? other.userInfo_.Clone() : null);
		score_ = other.score_;
		st_ = other.st_;
		et_ = other.et_;
		speed_ = other.speed_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CityAltarOccupyInfo Clone()
	{
		return new CityAltarOccupyInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityAltarOccupyInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityAltarOccupyInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(UserInfo, other.UserInfo))
		{
			return false;
		}
		if (Score != other.Score)
		{
			return false;
		}
		if (St != other.St)
		{
			return false;
		}
		if (Et != other.Et)
		{
			return false;
		}
		if (Speed != other.Speed)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (userInfo_ != null)
		{
			num ^= UserInfo.GetHashCode();
		}
		if (Score != 0L)
		{
			num ^= Score.GetHashCode();
		}
		if (St != 0L)
		{
			num ^= St.GetHashCode();
		}
		if (Et != 0L)
		{
			num ^= Et.GetHashCode();
		}
		if (Speed != 0L)
		{
			num ^= Speed.GetHashCode();
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
		if (userInfo_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(UserInfo);
		}
		if (Score != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Score);
		}
		if (St != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(St);
		}
		if (Et != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(Et);
		}
		if (Speed != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(Speed);
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
		if (userInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(UserInfo);
		}
		if (Score != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Score);
		}
		if (St != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(St);
		}
		if (Et != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Et);
		}
		if (Speed != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Speed);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CityAltarOccupyInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.userInfo_ != null)
		{
			if (userInfo_ == null)
			{
				UserInfo = new CityAltarUserInfo();
			}
			UserInfo.MergeFrom(other.UserInfo);
		}
		if (other.Score != 0L)
		{
			Score = other.Score;
		}
		if (other.St != 0L)
		{
			St = other.St;
		}
		if (other.Et != 0L)
		{
			Et = other.Et;
		}
		if (other.Speed != 0L)
		{
			Speed = other.Speed;
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				if (userInfo_ == null)
				{
					UserInfo = new CityAltarUserInfo();
				}
				input.ReadMessage(UserInfo);
				break;
			case 16u:
				Score = input.ReadInt64();
				break;
			case 24u:
				St = input.ReadInt64();
				break;
			case 32u:
				Et = input.ReadInt64();
				break;
			case 40u:
				Speed = input.ReadInt64();
				break;
			}
		}
	}
}
