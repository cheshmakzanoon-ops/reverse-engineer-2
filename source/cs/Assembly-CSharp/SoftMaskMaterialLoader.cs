using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using VEngine;

public static class SoftMaskMaterialLoader
{
	private static readonly Dictionary<string, Asset> ms_MaterialAssets = new Dictionary<string, Asset>();

	private static readonly Dictionary<string, List<Action<Material>>> ms_WaitLoadCallBacks = new Dictionary<string, List<Action<Material>>>();

	public static void GetMaterialAsset(string materialPath, Action<Material> callBack)
	{
		if (ms_MaterialAssets.TryGetValue(materialPath, out var value))
		{
			callBack?.Invoke(value.asset as Material);
			return;
		}
		if (!ms_WaitLoadCallBacks.TryGetValue(materialPath, out var value2))
		{
			value2 = new List<Action<Material>>();
			ms_WaitLoadCallBacks.Add(materialPath, value2);
		}
		bool num = value2.Count == 0;
		value2.Add(callBack);
		if (!num)
		{
			return;
		}
		Asset asset = GameEntry.Resource.LoadAssetAsync(materialPath, typeof(Material));
		if (asset == null)
		{
			return;
		}
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset materialAsset)
		{
			Material material = null;
			if (materialAsset.isError)
			{
				Log.Error("SoftMaskMaterialLoader error:" + materialAsset.error);
			}
			else
			{
				material = materialAsset.asset as Material;
				if (material != null)
				{
					ms_MaterialAssets[materialPath] = materialAsset;
				}
			}
			List<Action<Material>> list = ms_WaitLoadCallBacks[materialPath];
			foreach (Action<Material> item in list)
			{
				try
				{
					item?.Invoke(material);
				}
				catch (Exception ex)
				{
					Log.Error("SoftMaskMaterialLoader callBack Error:" + ex.Message);
				}
			}
			list.Clear();
		});
	}

	public static void Release()
	{
		foreach (KeyValuePair<string, List<Action<Material>>> ms_WaitLoadCallBack in ms_WaitLoadCallBacks)
		{
			ms_WaitLoadCallBack.Value.Clear();
		}
		ms_WaitLoadCallBacks.Clear();
		foreach (KeyValuePair<string, Asset> ms_MaterialAsset in ms_MaterialAssets)
		{
			ms_MaterialAsset.Value.Release();
		}
		ms_MaterialAssets.Clear();
	}
}
