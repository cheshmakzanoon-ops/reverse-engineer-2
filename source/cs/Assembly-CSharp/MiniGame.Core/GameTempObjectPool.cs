using System;
using System.Collections.Generic;

namespace MiniGame.Core;

public static class GameTempObjectPool<T> where T : class, IDisposable, new()
{
	[ThreadStatic]
	private static Stack<T> _pool;

	private static int _maxCapacity = 1000;

	public static void SetMaxCapacity(int capacity)
	{
		_maxCapacity = capacity;
	}

	public static T Fetch()
	{
		if (_pool == null)
		{
			_pool = new Stack<T>();
		}
		if (_pool.Count == 0)
		{
			return new T();
		}
		return _pool.Pop();
	}

	public static void Recycle(T obj)
	{
		if (_pool == null)
		{
			_pool = new Stack<T>();
		}
		if (obj != null)
		{
			obj.Dispose();
			if (_pool.Count < _maxCapacity)
			{
				_pool.Push(obj);
			}
		}
	}
}
