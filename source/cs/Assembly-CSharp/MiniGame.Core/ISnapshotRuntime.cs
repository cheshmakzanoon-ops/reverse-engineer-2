namespace MiniGame.Core;

public interface ISnapshotRuntime
{
	object TakeSnapshotRuntime(object snapshot);

	void RestoreSnapshotRuntime(object snapshot);
}
