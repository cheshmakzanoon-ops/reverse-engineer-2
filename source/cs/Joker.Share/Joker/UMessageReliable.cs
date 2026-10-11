namespace Joker;

public enum UMessageReliable : byte
{
	Invalid,
	Ping,
	Pong,
	Data,
	DataSegment
}
