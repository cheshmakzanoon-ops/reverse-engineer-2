using System.IO;
using System.Threading;

internal abstract class FileActionTask : IQueuedThreadTask
{
	public volatile bool Processed;

	protected readonly string FilePath;

	protected readonly string DirectoryPath;

	protected internal FileActionTask(string id)
	{
		string rootDirectory = FileContentHelper.GetRootDirectory();
		FilePath = Path.Combine(rootDirectory, id + ".bin");
		DirectoryPath = Path.GetDirectoryName(FilePath);
	}

	public virtual void Process(CancellationToken token)
	{
		Processed = true;
	}

	protected internal abstract void CallBack();
}
