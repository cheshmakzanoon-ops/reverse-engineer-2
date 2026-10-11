using System;
using Newtonsoft.Json;

namespace MiniGame.GGGo;

public class SyncCommand : IDisposable
{
	[JsonProperty("e")]
	public int EntityID;

	[JsonProperty("f")]
	public int FrameIndex { get; set; }

	[JsonIgnore]
	public bool IsValid { get; set; }

	[JsonProperty("c")]
	public int CommandType { get; set; }

	[JsonIgnore]
	public int SequenceID { get; set; } = -1;

	public SyncCommand Clone()
	{
		return new SyncCommand
		{
			FrameIndex = FrameIndex,
			EntityID = EntityID,
			IsValid = IsValid,
			CommandType = CommandType
		};
	}

	public bool Equals(SyncCommand other)
	{
		if (FrameIndex == other.FrameIndex && EntityID == other.EntityID)
		{
			return CommandType == other.CommandType;
		}
		return false;
	}

	public void Dispose()
	{
	}
}
