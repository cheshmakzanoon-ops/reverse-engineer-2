using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Joker;

public static class UtilsNet
{
	public static uint GenerateConv()
	{
		byte[] array = new byte[4];
		using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
		{
			randomNumberGenerator.GetBytes(array);
		}
		return BitConverter.ToUInt32(array, 0);
	}

	public static bool SendToNonBlocking(this Socket socket, byte[] buffer, int offset, int len, EndPoint remoteEP)
	{
		try
		{
			if (!socket.Poll(0, SelectMode.SelectWrite))
			{
				return false;
			}
			socket.SendTo(buffer, offset, len, SocketFlags.None, remoteEP);
			return true;
		}
		catch (SocketException ex)
		{
			if (ex.SocketErrorCode == SocketError.WouldBlock)
			{
				return false;
			}
			throw;
		}
	}

	public static bool SendNonBlocking(this Socket socket, MemoryBuffer buffer)
	{
		try
		{
			if (!socket.Poll(0, SelectMode.SelectWrite))
			{
				return false;
			}
			socket.Send(buffer.GetBuffer(), (int)buffer.Position, (int)(buffer.Length - buffer.Position), SocketFlags.None);
			return true;
		}
		catch (SocketException ex)
		{
			if (ex.SocketErrorCode == SocketError.WouldBlock)
			{
				return false;
			}
			throw;
		}
	}

	public static bool ReceiveFromNonBlocking(this Socket socket, byte[] buffer, out int len, ref EndPoint remoteEP)
	{
		len = 0;
		try
		{
			if (!socket.Poll(0, SelectMode.SelectRead))
			{
				return false;
			}
			len = socket.ReceiveFrom(buffer, SocketFlags.None, ref remoteEP);
			return true;
		}
		catch (SocketException ex)
		{
			if (ex.SocketErrorCode == SocketError.WouldBlock)
			{
				return false;
			}
			throw;
		}
	}

	public static bool ReceiveNonBlocking(this Socket socket, MemoryBuffer buffer)
	{
		try
		{
			if (!socket.Poll(0, SelectMode.SelectRead))
			{
				return false;
			}
			int num = (int)buffer.Position;
			int size = (int)(buffer.Capacity - buffer.Position);
			int num2 = socket.Receive(buffer.GetBuffer(), num, size, SocketFlags.None);
			buffer.SetLength(num + num2);
			return true;
		}
		catch (SocketException ex)
		{
			if (ex.SocketErrorCode == SocketError.WouldBlock)
			{
				return false;
			}
			throw;
		}
	}
}
