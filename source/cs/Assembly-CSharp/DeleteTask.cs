using System;
using System.IO;
using System.Threading;

internal class DeleteTask : FileActionTask
{
	private Action callback;

	public DeleteTask(string id, Action callback)
		: base(id)
	{
		this.callback = callback;
	}

	public override void Process(CancellationToken token)
	{
		if (File.Exists(FilePath))
		{
			File.Delete(FilePath);
		}
		base.Process(token);
	}

	protected internal override void CallBack()
	{
		callback?.Invoke();
	}
}
