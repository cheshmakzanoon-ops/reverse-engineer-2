using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ShieldInfo : IMessage<ShieldInfo>, IMessage, IEquatable<ShieldInfo>, IDeepCloneable<ShieldInfo>
{
	public enum ExtraOneofCase
	{
		None = 0,
		AllianceCity = 10,
		Base = 11
	}

	private static readonly MessageParser<ShieldInfo> _parser = new MessageParser<ShieldInfo>(() => new ShieldInfo());

	private UnknownFieldSet _unknownFields;

	public const int ShieldTypeFieldNumber = 1;

	private int shieldType_;

	public const int ShieldValueFieldNumber = 2;

	private long shieldValue_;

	public const int ShieldMaxValueFieldNumber = 3;

	private long shieldMaxValue_;

	public const int TotalContributionFieldNumber = 4;

	private long totalContribution_;

	public const int ExpireTimeFieldNumber = 5;

	private long expireTime_;

	public const int AllianceCityFieldNumber = 10;

	public const int BaseFieldNumber = 11;

	private object extra_;

	private ExtraOneofCase extraCase_;

	[DebuggerNonUserCode]
	public static MessageParser<ShieldInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ShieldInfoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ShieldType
	{
		get
		{
			return shieldType_;
		}
		set
		{
			shieldType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ShieldValue
	{
		get
		{
			return shieldValue_;
		}
		set
		{
			shieldValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ShieldMaxValue
	{
		get
		{
			return shieldMaxValue_;
		}
		set
		{
			shieldMaxValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TotalContribution
	{
		get
		{
			return totalContribution_;
		}
		set
		{
			totalContribution_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ExpireTime
	{
		get
		{
			return expireTime_;
		}
		set
		{
			expireTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceCityShield AllianceCity
	{
		get
		{
			if (extraCase_ != ExtraOneofCase.AllianceCity)
			{
				return null;
			}
			return (AllianceCityShield)extra_;
		}
		set
		{
			extra_ = value;
			extraCase_ = ((value != null) ? ExtraOneofCase.AllianceCity : ExtraOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public BaseShield Base
	{
		get
		{
			if (extraCase_ != ExtraOneofCase.Base)
			{
				return null;
			}
			return (BaseShield)extra_;
		}
		set
		{
			extra_ = value;
			extraCase_ = ((value != null) ? ExtraOneofCase.Base : ExtraOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public ExtraOneofCase ExtraCase => extraCase_;

	[DebuggerNonUserCode]
	public ShieldInfo()
	{
	}

	[DebuggerNonUserCode]
	public ShieldInfo(ShieldInfo other)
		: this()
	{
		shieldType_ = other.shieldType_;
		shieldValue_ = other.shieldValue_;
		shieldMaxValue_ = other.shieldMaxValue_;
		totalContribution_ = other.totalContribution_;
		expireTime_ = other.expireTime_;
		switch (other.ExtraCase)
		{
		case ExtraOneofCase.AllianceCity:
			AllianceCity = other.AllianceCity.Clone();
			break;
		case ExtraOneofCase.Base:
			Base = other.Base.Clone();
			break;
		}
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ShieldInfo Clone()
	{
		return new ShieldInfo(this);
	}

	[DebuggerNonUserCode]
	public void ClearExtra()
	{
		extraCase_ = ExtraOneofCase.None;
		extra_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ShieldInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ShieldInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ShieldType != other.ShieldType)
		{
			return false;
		}
		if (ShieldValue != other.ShieldValue)
		{
			return false;
		}
		if (ShieldMaxValue != other.ShieldMaxValue)
		{
			return false;
		}
		if (TotalContribution != other.TotalContribution)
		{
			return false;
		}
		if (ExpireTime != other.ExpireTime)
		{
			return false;
		}
		if (!object.Equals(AllianceCity, other.AllianceCity))
		{
			return false;
		}
		if (!object.Equals(Base, other.Base))
		{
			return false;
		}
		if (ExtraCase != other.ExtraCase)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ShieldType != 0)
		{
			num ^= ShieldType.GetHashCode();
		}
		if (ShieldValue != 0L)
		{
			num ^= ShieldValue.GetHashCode();
		}
		if (ShieldMaxValue != 0L)
		{
			num ^= ShieldMaxValue.GetHashCode();
		}
		if (TotalContribution != 0L)
		{
			num ^= TotalContribution.GetHashCode();
		}
		if (ExpireTime != 0L)
		{
			num ^= ExpireTime.GetHashCode();
		}
		if (extraCase_ == ExtraOneofCase.AllianceCity)
		{
			num ^= AllianceCity.GetHashCode();
		}
		if (extraCase_ == ExtraOneofCase.Base)
		{
			num ^= Base.GetHashCode();
		}
		num ^= (int)extraCase_;
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
		if (ShieldType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ShieldType);
		}
		if (ShieldValue != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(ShieldValue);
		}
		if (ShieldMaxValue != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ShieldMaxValue);
		}
		if (TotalContribution != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(TotalContribution);
		}
		if (ExpireTime != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(ExpireTime);
		}
		if (extraCase_ == ExtraOneofCase.AllianceCity)
		{
			output.WriteRawTag(82);
			output.WriteMessage(AllianceCity);
		}
		if (extraCase_ == ExtraOneofCase.Base)
		{
			output.WriteRawTag(90);
			output.WriteMessage(Base);
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
		if (ShieldType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ShieldType);
		}
		if (ShieldValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShieldValue);
		}
		if (ShieldMaxValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShieldMaxValue);
		}
		if (TotalContribution != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TotalContribution);
		}
		if (ExpireTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ExpireTime);
		}
		if (extraCase_ == ExtraOneofCase.AllianceCity)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceCity);
		}
		if (extraCase_ == ExtraOneofCase.Base)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Base);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ShieldInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.ShieldType != 0)
		{
			ShieldType = other.ShieldType;
		}
		if (other.ShieldValue != 0L)
		{
			ShieldValue = other.ShieldValue;
		}
		if (other.ShieldMaxValue != 0L)
		{
			ShieldMaxValue = other.ShieldMaxValue;
		}
		if (other.TotalContribution != 0L)
		{
			TotalContribution = other.TotalContribution;
		}
		if (other.ExpireTime != 0L)
		{
			ExpireTime = other.ExpireTime;
		}
		switch (other.ExtraCase)
		{
		case ExtraOneofCase.AllianceCity:
			if (AllianceCity == null)
			{
				AllianceCity = new AllianceCityShield();
			}
			AllianceCity.MergeFrom(other.AllianceCity);
			break;
		case ExtraOneofCase.Base:
			if (Base == null)
			{
				Base = new BaseShield();
			}
			Base.MergeFrom(other.Base);
			break;
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
			case 8u:
				ShieldType = input.ReadInt32();
				break;
			case 16u:
				ShieldValue = input.ReadInt64();
				break;
			case 24u:
				ShieldMaxValue = input.ReadInt64();
				break;
			case 32u:
				TotalContribution = input.ReadInt64();
				break;
			case 40u:
				ExpireTime = input.ReadInt64();
				break;
			case 82u:
			{
				AllianceCityShield allianceCityShield = new AllianceCityShield();
				if (extraCase_ == ExtraOneofCase.AllianceCity)
				{
					allianceCityShield.MergeFrom(AllianceCity);
				}
				input.ReadMessage(allianceCityShield);
				AllianceCity = allianceCityShield;
				break;
			}
			case 90u:
			{
				BaseShield baseShield = new BaseShield();
				if (extraCase_ == ExtraOneofCase.Base)
				{
					baseShield.MergeFrom(Base);
				}
				input.ReadMessage(baseShield);
				Base = baseShield;
				break;
			}
			}
		}
	}
}
