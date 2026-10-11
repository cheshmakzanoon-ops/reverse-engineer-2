using System;
using System.Text;

namespace Joker;

public static class UHeader
{
	public static readonly uint kMagicNumber = MakeMagicNumber("KCPH");

	public const int kHeaderMagicSize = 4;

	public const int kHeaderChannelSize = 1;

	public const int kHeaderSize = 5;

	public const int kMessageTypeSize = 1;

	public const int kMessageSegmentTypeSize = 5;

	[ThreadStatic]
	private static byte[] kMessageTypeHeader;

	[ThreadStatic]
	private static byte[] kMessageTypeSegmentHeader;

	public static byte[] PackMessageReliableHeader(UMessageReliable type, int count = 0, int index = 0)
	{
		if (type != UMessageReliable.DataSegment)
		{
			if (kMessageTypeHeader == null)
			{
				kMessageTypeHeader = new byte[1];
			}
			kMessageTypeHeader[0] = (byte)type;
			return kMessageTypeHeader;
		}
		if (kMessageTypeSegmentHeader == null)
		{
			kMessageTypeSegmentHeader = new byte[5];
		}
		kMessageTypeSegmentHeader[0] = (byte)type;
		UtilsEncode.Encode16U(kMessageTypeSegmentHeader, 1, (ushort)count);
		UtilsEncode.Encode16U(kMessageTypeSegmentHeader, 3, (ushort)index);
		return kMessageTypeSegmentHeader;
	}

	public static int UnpackMessageReliableHeader(byte[] data, int offset, out UMessageReliable type, out int count, out int index)
	{
		count = 0;
		index = 0;
		if (!TryParseReliable(data[offset], out type))
		{
			return 0;
		}
		if (type != UMessageReliable.DataSegment)
		{
			return 1;
		}
		UtilsEncode.Decode16U(data, offset + 1, out var value);
		UtilsEncode.Decode16U(data, offset + 1 + 2, out var value2);
		count = value;
		index = value2;
		return 5;
	}

	public static uint MakeMagicNumber(string magicString)
	{
		if (string.IsNullOrEmpty(magicString) || magicString.Length != 4)
		{
			throw new ArgumentException("Magic string must be exactly 4 characters long.", "magicString");
		}
		byte[] bytes = Encoding.ASCII.GetBytes(magicString);
		return (uint)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
	}

	public static bool TryParseUnreliable(byte value, out UMessageUnreliable message)
	{
		if (Enum.IsDefined(typeof(UMessageUnreliable), value))
		{
			message = (UMessageUnreliable)value;
			return true;
		}
		message = UMessageUnreliable.Invalid;
		return false;
	}

	public static bool TryParseReliable(byte value, out UMessageReliable message)
	{
		if (Enum.IsDefined(typeof(UMessageReliable), value))
		{
			message = (UMessageReliable)value;
			return true;
		}
		message = UMessageReliable.Invalid;
		return false;
	}
}
