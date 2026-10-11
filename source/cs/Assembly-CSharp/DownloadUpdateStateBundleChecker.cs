using System.Collections.Generic;
using System.IO;
using VEngine;

public class DownloadUpdateStateBundleChecker : DownloadChecker
{
	public Queue<string> originBundleNames = new Queue<string>();

	protected override void OnCollect()
	{
		int i = 0;
		for (int num = _downloads.Length; i < num; i++)
		{
			foreach (KeyValuePair<string, long> item in _downloads[i])
			{
				if (Path.GetExtension(item.Key) == ".bundle" && (item.Key.StartsWith("gameres") || item.Key.StartsWith("dllres")))
				{
					originBundleNames.Enqueue(item.Key);
				}
				else if (item.Key.StartsWith("gameres_raw"))
				{
					originBundleNames.Enqueue(item.Key);
				}
			}
		}
	}
}
