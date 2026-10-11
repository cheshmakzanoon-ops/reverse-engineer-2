using System;
using System.Diagnostics;
using System.Threading;

namespace Joker;

public class ConsoleLogger : ILogger
{
	private Stopwatch _sw = Stopwatch.StartNew();

	public void Debug(string message)
	{
		Console.WriteLine($"Debug[{Thread.CurrentThread.ManagedThreadId}] {_sw.ElapsedMilliseconds}:{message}");
	}

	public void Info(string message)
	{
		Console.WriteLine($"Info[{Thread.CurrentThread.ManagedThreadId}] {_sw.ElapsedMilliseconds}:{message}");
	}

	public void Warning(string message)
	{
		Console.WriteLine($"Warning[{Thread.CurrentThread.ManagedThreadId}] {_sw.ElapsedMilliseconds}:{message}");
	}

	public void Error(string message)
	{
		Console.WriteLine($"Error[{Thread.CurrentThread.ManagedThreadId}] {_sw.ElapsedMilliseconds}:{message}");
	}

	public void Exception(Exception e)
	{
		Console.WriteLine($"Error[{Thread.CurrentThread.ManagedThreadId}] {_sw.ElapsedMilliseconds}:{e}");
	}

	public void Assert(bool condition, object message = null)
	{
		if (!condition)
		{
			string arg = ((message == null) ? "failed" : message.ToString());
			Console.WriteLine($"Assert[{Thread.CurrentThread.ManagedThreadId}] {_sw.ElapsedMilliseconds}:{arg}");
		}
	}
}
