using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutZWLBuilding : IMessage<ScoutZWLBuilding>, IMessage, IEquatable<ScoutZWLBuilding>, IDeepCloneable<ScoutZWLBuilding>
{
	private static readonly MessageParser<ScoutZWLBuilding> _parser = new MessageParser<ScoutZWLBuilding>(() => new ScoutZWLBuilding());

	private UnknownFieldSet _unknownFields;

	public const int BuildIdFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_buildId_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? buildId_;

	public const int PointFieldNumber = 2;

	private PointInfo point_;

	public const int TemplateIdFieldNumber = 4;

	private int templateId_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutZWLBuilding> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[29];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? BuildId
	{
		get
		{
			return buildId_;
		}
		set
		{
			buildId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public PointInfo Point
	{
		get
		{
			return point_;
		}
		set
		{
			point_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TemplateId
	{
		get
		{
			return templateId_;
		}
		set
		{
			templateId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutZWLBuilding()
	{
	}

	[DebuggerNonUserCode]
	public ScoutZWLBuilding(ScoutZWLBuilding other)
		: this()
	{
		BuildId = other.BuildId;
		point_ = ((other.point_ != null) ? other.point_.Clone() : null);
		templateId_ = other.templateId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutZWLBuilding Clone()
	{
		return new ScoutZWLBuilding(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutZWLBuilding);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutZWLBuilding other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BuildId != other.BuildId)
		{
			return false;
		}
		if (!object.Equals(Point, other.Point))
		{
			return false;
		}
		if (TemplateId != other.TemplateId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (buildId_.HasValue)
		{
			num ^= BuildId.GetHashCode();
		}
		if (point_ != null)
		{
			num ^= Point.GetHashCode();
		}
		if (TemplateId != 0)
		{
			num ^= TemplateId.GetHashCode();
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
		if (buildId_.HasValue)
		{
			_single_buildId_codec.WriteTagAndValue(output, BuildId);
		}
		if (point_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(Point);
		}
		if (TemplateId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(TemplateId);
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
		if (buildId_.HasValue)
		{
			num += _single_buildId_codec.CalculateSizeWithTag(BuildId);
		}
		if (point_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Point);
		}
		if (TemplateId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TemplateId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutZWLBuilding other)
	{
		if (other == null)
		{
			return;
		}
		if (other.buildId_.HasValue && (!buildId_.HasValue || other.BuildId != 0))
		{
			BuildId = other.BuildId;
		}
		if (other.point_ != null)
		{
			if (point_ == null)
			{
				Point = new PointInfo();
			}
			Point.MergeFrom(other.Point);
		}
		if (other.TemplateId != 0)
		{
			TemplateId = other.TemplateId;
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
			{
				int? num2 = _single_buildId_codec.Read(input);
				if (!buildId_.HasValue || num2 != 0)
				{
					BuildId = num2;
				}
				break;
			}
			case 18u:
				if (point_ == null)
				{
					Point = new PointInfo();
				}
				input.ReadMessage(Point);
				break;
			case 32u:
				TemplateId = input.ReadInt32();
				break;
			}
		}
	}
}
