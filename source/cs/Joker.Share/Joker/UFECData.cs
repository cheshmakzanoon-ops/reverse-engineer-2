using System;

namespace Joker;

public class UFECData
{
	public const int kUFECDataSize = 6;

	private ushort _id;

	private uint _accumulator;

	private uint[] _receives;

	public ushort MaxID { get; private set; }

	public UFECData(ushort maxID = 4096)
	{
		MaxID = maxID;
		_receives = new uint[maxID];
	}

	public bool Pack(byte[] buffer, int offset)
	{
		if (buffer.Length - offset < 6)
		{
			return false;
		}
		ushort value = _id++;
		if (_id >= MaxID)
		{
			_id = 0;
		}
		if (_accumulator >= uint.MaxValue)
		{
			_accumulator = 0u;
		}
		_accumulator++;
		UtilsEncode.Encode16U(buffer, offset, value);
		UtilsEncode.Encode32U(buffer, offset + 2, _accumulator);
		return true;
	}

	public bool Unpack(byte[] buffer, int offset)
	{
		if (buffer.Length - offset < 6)
		{
			return false;
		}
		UtilsEncode.Decode16U(buffer, offset, out var value);
		if (value >= _receives.Length)
		{
			int newSize = Math.Min(value + 100, 65535);
			Array.Resize(ref _receives, newSize);
		}
		uint num = _receives[value];
		UtilsEncode.Decode32U(buffer, offset + 2, out var value2);
		if (num != 0 && num == value2)
		{
			return false;
		}
		_receives[value] = value2;
		return true;
	}
}
