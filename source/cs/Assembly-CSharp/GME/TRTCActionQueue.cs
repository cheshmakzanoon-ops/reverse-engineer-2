using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GME;

public sealed class TRTCActionQueue
{
	private readonly Queue<Action> _actions = new Queue<Action>();

	private bool _paused;

	private bool _destroyed;

	internal void Clear()
	{
		lock (_actions)
		{
			_actions.Clear();
		}
	}

	internal void Enqueue(Action action, bool canDrop = false)
	{
		if (action == null || (canDrop && _paused))
		{
			return;
		}
		lock (_actions)
		{
			if (!_destroyed)
			{
				_actions.Enqueue(action);
			}
		}
	}

	private Action Dequeue()
	{
		Action result = null;
		lock (_actions)
		{
			if (_actions.Count > 0)
			{
				result = _actions.Dequeue();
			}
		}
		return result;
	}

	internal void Update()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		while (stopwatch.ElapsedMilliseconds < 20)
		{
			Action action = Dequeue();
			if (action != null)
			{
				try
				{
					action();
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.Log($"TRTCActionQueue Invoke {arg}");
				}
				continue;
			}
			break;
		}
	}

	internal void Destroy()
	{
		lock (_actions)
		{
			_destroyed = true;
			_actions.Clear();
		}
	}

	internal void Pause()
	{
		_paused = true;
	}

	internal void Resume()
	{
		_paused = false;
	}
}
