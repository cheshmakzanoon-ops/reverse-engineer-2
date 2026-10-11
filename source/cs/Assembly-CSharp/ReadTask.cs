using System;
using System.IO;
using System.Text;
using System.Threading;

internal class ReadTask : FileActionTask
{
	private Action<string> callback;

	private string content;

	public ReadTask(string id, Action<string> callback)
		: base(id)
	{
		this.callback = callback;
	}

	public override void Process(CancellationToken token)
	{
		if (File.Exists(FilePath))
		{
			using FileStream fileStream = new FileStream(FilePath, FileMode.Open);
			if (FileContentHelper.useBinary)
			{
				using BinaryReader binaryReader = new BinaryReader(fileStream);
				content = binaryReader.ReadString();
			}
			else
			{
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				content = Encoding.UTF8.GetString(array);
			}
		}
		base.Process(token);
	}

	protected internal override void CallBack()
	{
		callback?.Invoke(content);
	}
}
