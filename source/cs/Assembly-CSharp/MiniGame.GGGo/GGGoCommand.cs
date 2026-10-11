using System;
using System.Collections.Generic;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class GGGoCommand : ICloneable
{
	public List<SyncCommand> Commands;

	public int ExecutedIndex = -1;

	public int LastPickedFrame
	{
		get
		{
			if (ExecutedIndex < 0 || ExecutedIndex >= Commands.Count)
			{
				return -1;
			}
			return Commands[ExecutedIndex].FrameIndex;
		}
	}

	public GGGoCommand()
	{
		Commands = new List<SyncCommand>();
	}

	public GGGoCommand(List<SyncCommand> commands = null)
	{
		Commands = commands ?? new List<SyncCommand>();
	}

	public void QueueEvent(int frame, int entity, int type, int sequence)
	{
		if (Commands.Count > 0 && Commands[Commands.Count - 1].FrameIndex > frame)
		{
			throw new Exception("FrameIndex is not ascending");
		}
		SyncCommand syncCommand = GameTempObjectPool<SyncCommand>.Fetch();
		syncCommand.FrameIndex = frame;
		syncCommand.EntityID = entity;
		syncCommand.CommandType = type;
		syncCommand.SequenceID = sequence;
		syncCommand.IsValid = true;
		Commands.Add(syncCommand);
	}

	public bool InsertEvent(int frame, int entity, int type)
	{
		SyncCommand syncCommand = GameTempObjectPool<SyncCommand>.Fetch();
		syncCommand.FrameIndex = frame;
		syncCommand.EntityID = entity;
		syncCommand.CommandType = type;
		syncCommand.SequenceID = -1;
		syncCommand.IsValid = true;
		int num;
		for (num = Commands.Count; num > 0; num--)
		{
			if (syncCommand.FrameIndex >= Commands[num - 1].FrameIndex)
			{
				return InsertEvent(syncCommand, num);
			}
		}
		return InsertEvent(syncCommand, num);
	}

	private bool InsertEvent(SyncCommand evt, int index)
	{
		Commands.Insert(index, evt);
		if (LastPickedFrame >= evt.FrameIndex)
		{
			ExecutedIndex++;
			return false;
		}
		return true;
	}

	public bool TryQueueEvent(int frame, int entity, int type, int sequence)
	{
		SyncCommand syncCommand = GameTempObjectPool<SyncCommand>.Fetch();
		syncCommand.FrameIndex = frame;
		syncCommand.EntityID = entity;
		syncCommand.CommandType = type;
		syncCommand.SequenceID = sequence;
		syncCommand.IsValid = true;
		if (Commands.Count > 0)
		{
			if (Commands[Commands.Count - 1].FrameIndex > syncCommand.FrameIndex)
			{
				return false;
			}
			if (Commands[Commands.Count - 1].FrameIndex == syncCommand.FrameIndex)
			{
				for (int num = Commands.Count - 1; num >= 0; num--)
				{
					SyncCommand syncCommand2 = Commands[num];
					if (syncCommand2.FrameIndex != syncCommand.FrameIndex)
					{
						break;
					}
					if (syncCommand2.Equals(syncCommand))
					{
						return false;
					}
				}
			}
		}
		Commands.Add(syncCommand);
		return true;
	}

	public SyncCommand PickEvent(int frameIndex)
	{
		if (TryPickEvent(frameIndex, out var command))
		{
			return command;
		}
		return null;
	}

	public bool TryPickEvent(int frameIndex, out SyncCommand command)
	{
		int num = ExecutedIndex + 1;
		if (num >= Commands.Count)
		{
			command = null;
			return false;
		}
		command = Commands[num];
		if (command.FrameIndex != frameIndex)
		{
			command = null;
			return false;
		}
		ExecutedIndex = num;
		return true;
	}

	public object Clone()
	{
		GGGoCommand gGGoCommand = new GGGoCommand();
		for (int i = 0; i < Commands.Count; i++)
		{
			SyncCommand syncCommand = Commands[i];
			gGGoCommand.Commands.Add(syncCommand.Clone());
		}
		gGGoCommand.ExecutedIndex = ExecutedIndex;
		return gGGoCommand;
	}

	public GGGoCommand CopyTo(GGGoCommand command)
	{
		if (command == null)
		{
			command = new GGGoCommand();
		}
		command.ExecutedIndex = ExecutedIndex;
		CopyTo(Commands, command.Commands);
		return command;
	}

	public static void CopyTo(List<SyncCommand> from, List<SyncCommand> to)
	{
		int i;
		for (i = 0; i < from.Count; i++)
		{
			SyncCommand syncCommand = from[i];
			SyncCommand syncCommand2;
			if (i < to.Count)
			{
				syncCommand2 = to[i];
			}
			else
			{
				syncCommand2 = GameTempObjectPool<SyncCommand>.Fetch();
				to.Add(syncCommand2);
			}
			syncCommand2.FrameIndex = syncCommand.FrameIndex;
			syncCommand2.EntityID = syncCommand.EntityID;
			syncCommand2.IsValid = syncCommand.IsValid;
			syncCommand2.CommandType = syncCommand.CommandType;
			syncCommand2.SequenceID = syncCommand.SequenceID;
		}
		for (int num = to.Count - 1; num >= i; num--)
		{
			GameTempObjectPool<SyncCommand>.Recycle(to[num]);
			to.RemoveAt(num);
		}
	}
}
