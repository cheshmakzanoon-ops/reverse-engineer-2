using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityAltarUserInfo : IMessage<CityAltarUserInfo>, IMessage, IEquatable<CityAltarUserInfo>, IDeepCloneable<CityAltarUserInfo>
{
	private static readonly MessageParser<CityAltarUserInfo> _parser = new MessageParser<CityAltarUserInfo>(() => new CityAltarUserInfo());

	private UnknownFieldSet _unknownFields;

	public const int UidFieldNumber = 1;

	private string uid_ = "";

	public const int UidNameFieldNumber = 2;

	private string uidName_ = "";

	public const int PicFieldNumber = 3;

	private string pic_ = "";

	public const int PicVerFieldNumber = 4;

	private int picVer_;

	public const int HeadSkinIdFieldNumber = 5;

	private int headSkinId_;

	public const int HeadSkinETFieldNumber = 6;

	private long headSkinET_;

	public const int AlAbbrFieldNumber = 7;

	private string alAbbr_ = "";

	public const int AllianceIdFieldNumber = 8;

	private string allianceId_ = "";

	public const int AlNameFieldNumber = 9;

	private string alName_ = "";

	public const int AlIconFieldNumber = 10;

	private string alIcon_ = "";

	public const int SidFieldNumber = 11;

	private int sid_;

	[DebuggerNonUserCode]
	public static MessageParser<CityAltarUserInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[33];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public string UidName
	{
		get
		{
			return uidName_;
		}
		set
		{
			uidName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Pic
	{
		get
		{
			return pic_;
		}
		set
		{
			pic_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int PicVer
	{
		get
		{
			return picVer_;
		}
		set
		{
			picVer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadSkinId
	{
		get
		{
			return headSkinId_;
		}
		set
		{
			headSkinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long HeadSkinET
	{
		get
		{
			return headSkinET_;
		}
		set
		{
			headSkinET_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string AlAbbr
	{
		get
		{
			return alAbbr_;
		}
		set
		{
			alAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public string AlName
	{
		get
		{
			return alName_;
		}
		set
		{
			alName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AlIcon
	{
		get
		{
			return alIcon_;
		}
		set
		{
			alIcon_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Sid
	{
		get
		{
			return sid_;
		}
		set
		{
			sid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityAltarUserInfo()
	{
	}

	[DebuggerNonUserCode]
	public CityAltarUserInfo(CityAltarUserInfo other)
		: this()
	{
		uid_ = other.uid_;
		uidName_ = other.uidName_;
		pic_ = other.pic_;
		picVer_ = other.picVer_;
		headSkinId_ = other.headSkinId_;
		headSkinET_ = other.headSkinET_;
		alAbbr_ = other.alAbbr_;
		allianceId_ = other.allianceId_;
		alName_ = other.alName_;
		alIcon_ = other.alIcon_;
		sid_ = other.sid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CityAltarUserInfo Clone()
	{
		return new CityAltarUserInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityAltarUserInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityAltarUserInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (UidName != other.UidName)
		{
			return false;
		}
		if (Pic != other.Pic)
		{
			return false;
		}
		if (PicVer != other.PicVer)
		{
			return false;
		}
		if (HeadSkinId != other.HeadSkinId)
		{
			return false;
		}
		if (HeadSkinET != other.HeadSkinET)
		{
			return false;
		}
		if (AlAbbr != other.AlAbbr)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (AlName != other.AlName)
		{
			return false;
		}
		if (AlIcon != other.AlIcon)
		{
			return false;
		}
		if (Sid != other.Sid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (UidName.Length != 0)
		{
			num ^= UidName.GetHashCode();
		}
		if (Pic.Length != 0)
		{
			num ^= Pic.GetHashCode();
		}
		if (PicVer != 0)
		{
			num ^= PicVer.GetHashCode();
		}
		if (HeadSkinId != 0)
		{
			num ^= HeadSkinId.GetHashCode();
		}
		if (HeadSkinET != 0L)
		{
			num ^= HeadSkinET.GetHashCode();
		}
		if (AlAbbr.Length != 0)
		{
			num ^= AlAbbr.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (AlName.Length != 0)
		{
			num ^= AlName.GetHashCode();
		}
		if (AlIcon.Length != 0)
		{
			num ^= AlIcon.GetHashCode();
		}
		if (Sid != 0)
		{
			num ^= Sid.GetHashCode();
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
		if (Uid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Uid);
		}
		if (UidName.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(UidName);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Pic);
		}
		if (PicVer != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(PicVer);
		}
		if (HeadSkinId != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(HeadSkinId);
		}
		if (HeadSkinET != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(HeadSkinET);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(AllianceId);
		}
		if (AlName.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(AlName);
		}
		if (AlIcon.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(AlIcon);
		}
		if (Sid != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(Sid);
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
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (UidName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(UidName);
		}
		if (Pic.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Pic);
		}
		if (PicVer != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PicVer);
		}
		if (HeadSkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadSkinId);
		}
		if (HeadSkinET != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(HeadSkinET);
		}
		if (AlAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (AlName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlName);
		}
		if (AlIcon.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlIcon);
		}
		if (Sid != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Sid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CityAltarUserInfo other)
	{
		if (other != null)
		{
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.UidName.Length != 0)
			{
				UidName = other.UidName;
			}
			if (other.Pic.Length != 0)
			{
				Pic = other.Pic;
			}
			if (other.PicVer != 0)
			{
				PicVer = other.PicVer;
			}
			if (other.HeadSkinId != 0)
			{
				HeadSkinId = other.HeadSkinId;
			}
			if (other.HeadSkinET != 0L)
			{
				HeadSkinET = other.HeadSkinET;
			}
			if (other.AlAbbr.Length != 0)
			{
				AlAbbr = other.AlAbbr;
			}
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.AlName.Length != 0)
			{
				AlName = other.AlName;
			}
			if (other.AlIcon.Length != 0)
			{
				AlIcon = other.AlIcon;
			}
			if (other.Sid != 0)
			{
				Sid = other.Sid;
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
			case 10u:
				Uid = input.ReadString();
				break;
			case 18u:
				UidName = input.ReadString();
				break;
			case 26u:
				Pic = input.ReadString();
				break;
			case 32u:
				PicVer = input.ReadInt32();
				break;
			case 40u:
				HeadSkinId = input.ReadInt32();
				break;
			case 48u:
				HeadSkinET = input.ReadInt64();
				break;
			case 58u:
				AlAbbr = input.ReadString();
				break;
			case 66u:
				AllianceId = input.ReadString();
				break;
			case 74u:
				AlName = input.ReadString();
				break;
			case 82u:
				AlIcon = input.ReadString();
				break;
			case 88u:
				Sid = input.ReadInt32();
				break;
			}
		}
	}
}
