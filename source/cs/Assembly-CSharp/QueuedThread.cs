using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using GameFramework;

public class QueuedThread
{
	private Thread thread;

	private AutoResetEvent wakeupEvent = new AutoResetEvent(initialState: false);

	private volatile bool stop;

	private Queue<IQueuedThreadTask> tasks = new Queue<IQueuedThreadTask>(10);

	private Queue<IQueuedThreadTask> urgentTasks = new Queue<IQueuedThreadTask>(10);

	private string name;

	private bool useCompleteTaskQueue;

	private ConcurrentQueue<IQueuedThreadTask> completedTaskQueue;

	private CancellationTokenSource cancellationTokenSource;

	public QueuedThread(string name, bool useCompleteTaskQueue = false)
	{
		this.name = name;
		this.useCompleteTaskQueue = useCompleteTaskQueue;
	}

	public void Start()
	{
		completedTaskQueue = new ConcurrentQueue<IQueuedThreadTask>();
		cancellationTokenSource = new CancellationTokenSource();
		stop = false;
		thread = new Thread(ThreadProc);
		thread.Name = name;
		thread.Start();
	}

	public void Stop()
	{
		stop = true;
		cancellationTokenSource?.Cancel();
		Wakeup();
		if (thread != null)
		{
			thread.Join();
			thread = null;
		}
		urgentTasks.Clear();
		tasks.Clear();
		completedTaskQueue = null;
		cancellationTokenSource?.Dispose();
		cancellationTokenSource = null;
	}

	public void AddTask(IQueuedThreadTask task)
	{
		lock (tasks)
		{
			tasks.Enqueue(task);
		}
		Wakeup();
	}

	public void AddUrgentTask(IQueuedThreadTask task)
	{
		lock (urgentTasks)
		{
			urgentTasks.Enqueue(task);
		}
		Wakeup();
	}

	public bool TryGetCompletedTask(out IQueuedThreadTask task)
	{
		if (completedTaskQueue != null)
		{
			return completedTaskQueue.TryDequeue(out task);
		}
		task = null;
		return false;
	}

	private void Sleep()
	{
		wakeupEvent.WaitOne();
	}

	private void Wakeup()
	{
		wakeupEvent.Set();
	}

	private bool ProcessTask(Queue<IQueuedThreadTask> queue)
	{
		IQueuedThreadTask queuedThreadTask = null;
		lock (queue)
		{
			if (queue.Count > 0)
			{
				queuedThreadTask = queue.Dequeue();
			}
		}
		if (queuedThreadTask != null)
		{
			try
			{
				queuedThreadTask.Process(cancellationTokenSource.Token);
				if (useCompleteTaskQueue)
				{
					completedTaskQueue.Enqueue(queuedThreadTask);
				}
				return true;
			}
			catch (Exception ex)
			{
				Log.Error("exception: {0}, {1}, {2}", string.IsNullOrEmpty(name) ? "[THREAD]" : name, ex.Message, ex.StackTrace);
			}
		}
		return false;
	}

	private void ThreadProc()
	{
		while (!stop)
		{
			if (!ProcessTask(urgentTasks) && !ProcessTask(tasks))
			{
				Sleep();
			}
		}
	}
}
