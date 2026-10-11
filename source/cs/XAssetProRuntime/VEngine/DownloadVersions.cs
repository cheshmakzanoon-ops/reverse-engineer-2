using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace VEngine;

public sealed class DownloadVersions : Operation
{
	private readonly List<Download> downloads = new List<Download>();

	public DownloadInfo[] groups;

	public int queueID;

	public ulong downloadByteWithFakeStream;

	public ulong totalSize { get; private set; }

	public ulong downloadedBytes { get; private set; }

	public float downloadTime { get; private set; }

	public ulong realDownloadedBytes { get; private set; }

	public ulong warmupBytes { get; private set; }

	public float[] cdnUsePct { get; private set; } = new float[4];

	public override void Start()
	{
		base.Start();
		DownloadInfo[] array = groups;
		foreach (DownloadInfo downloadInfo in array)
		{
			downloads.Add(Download.DownloadAsync(downloadInfo, queueID));
			totalSize += downloadInfo.size;
		}
		downloadedBytes = 0uL;
		downloadByteWithFakeStream = 0uL;
	}

	public void ChangeDownloadsPrepareQueue(int targetQueueID)
	{
		foreach (Download download in downloads)
		{
			Download.ChangeDownloadsPrepareQueue(download, targetQueueID);
		}
	}

	protected override void Update()
	{
		OperationStatus operationStatus = base.status;
		if (operationStatus != OperationStatus.Processing)
		{
			return;
		}
		downloadedBytes = 0uL;
		bool flag = true;
		foreach (Download download in downloads)
		{
			downloadedBytes += download.downloadedBytes;
			if (!download.isDone)
			{
				flag = false;
			}
		}
		if (downloadByteWithFakeStream < downloadedBytes)
		{
			downloadByteWithFakeStream = downloadedBytes;
		}
		base.progress = (float)downloadedBytes / (float)totalSize;
		if (!flag)
		{
			return;
		}
		downloadTime = 0f;
		warmupBytes = 0uL;
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Download download2 in downloads)
		{
			if (!string.IsNullOrEmpty(download2.error))
			{
				stringBuilder.AppendLine(download2.error);
			}
			downloadTime += download2.downloadTime;
			if (download2.useWarmup)
			{
				warmupBytes += download2.downloadedBytes;
			}
			else
			{
				realDownloadedBytes += download2.downloadedBytes;
			}
			if (download2._usedCdnIndex >= 0 && download2._usedCdnIndex < 4)
			{
				cdnUsePct[download2._usedCdnIndex] += 1f;
			}
		}
		int count = downloads.Count;
		for (int i = 0; i < 4; i++)
		{
			cdnUsePct[i] /= count;
		}
		Finish(stringBuilder.ToString());
	}

	public void Stop()
	{
		if (base.status != OperationStatus.Processing)
		{
			return;
		}
		foreach (Download download in downloads)
		{
			download.Cancel();
			Download.RemoveDownload(download);
		}
		Finish();
	}

	public void TryStopSingleDownloadInSpecialQueue(Dictionary<string, HashSet<int>> downloadInfoWithPackIdMap, int packageId)
	{
		if (base.status != OperationStatus.Processing)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (Download download in downloads)
		{
			if ((download.waitPrepareQueue != 2 && download.waitPrepareQueue != 4) || download.info == null || !downloadInfoWithPackIdMap.ContainsKey(download.info.url))
			{
				continue;
			}
			HashSet<int> hashSet = downloadInfoWithPackIdMap[download.info.url];
			if (hashSet != null)
			{
				if (hashSet.Contains(packageId))
				{
					hashSet.Remove(packageId);
				}
				if (hashSet.Count == 0)
				{
					download.Cancel();
					Download.RemoveDownload(download);
					list.Add(download.info.url);
				}
			}
			else
			{
				Log.Error("TryStopSingleDownloadInSpecialQueue  暂停下载时，download 在 downloadInfoWithPackIdMap 中对应的packIdSet为空 检查" + download.info.url);
			}
		}
		foreach (string item in list)
		{
			if (downloadInfoWithPackIdMap.ContainsKey(item))
			{
				downloadInfoWithPackIdMap.Remove(item);
			}
		}
		Finish();
	}
}
