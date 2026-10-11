using System.Runtime.CompilerServices;

namespace Joker;

public static class UtilsEncode
{
	public static int Clamp(int value, int min, int max)
	{
		if (value < min)
		{
			return min;
		}
		if (value > max)
		{
			return max;
		}
		return value;
	}

	public static int Encode8U(byte[] p, int offset, byte value)
	{
		p[offset] = value;
		return 1;
	}

	public static int Decode8U(byte[] p, int offset, out byte value)
	{
		value = p[offset];
		return 1;
	}

	public static int Encode16U(byte[] p, int offset, ushort value)
	{
		p[offset] = (byte)value;
		p[1 + offset] = (byte)(value >> 8);
		return 2;
	}

	public static int Decode16U(byte[] p, int offset, out ushort value)
	{
		ushort num = 0;
		num |= p[offset];
		num |= (ushort)(p[1 + offset] << 8);
		value = num;
		return 2;
	}

	public static int Encode32U(byte[] p, int offset, uint value)
	{
		p[offset] = (byte)value;
		p[1 + offset] = (byte)(value >> 8);
		p[2 + offset] = (byte)(value >> 16);
		p[3 + offset] = (byte)(value >> 24);
		return 4;
	}

	public static int Decode32U(byte[] p, int offset, out uint value)
	{
		uint num = 0u;
		num |= p[offset];
		num |= (uint)(p[1 + offset] << 8);
		num |= (uint)(p[2 + offset] << 16);
		num |= (uint)(p[3 + offset] << 24);
		value = num;
		return 4;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int TimeDiff(uint later, uint earlier)
	{
		return (int)(later - earlier);
	}
}
