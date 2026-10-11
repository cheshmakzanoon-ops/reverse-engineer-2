using System.Threading;

public interface IQueuedThreadTask
{
	void Process(CancellationToken cancellationToken);
}
