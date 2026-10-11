using System;
using System.Collections.Generic;
using GameFramework;
using VEngine;

namespace GPUDamageText;

public static class DamageNumResLoader
{
	private static Asset ms_DamageNumResAsset;

	private static readonly List<Action<DamageNumRes>> ms_WaitLoadCallBacks = new List<Action<DamageNumRes>>();

	private static readonly string ms_DamageNumResAssetPath = "Assets/Main/Prefabs/GPUDamageNum/GPUDamageNumRes.asset";

	public static void GetResAsset(Action<DamageNumRes> callBack)
	{
		if (ms_DamageNumResAsset != null)
		{
			DamageNumRes damageNumRes = ms_DamageNumResAsset.asset as DamageNumRes;
			if (damageNumRes != null)
			{
				callBack?.Invoke(damageNumRes);
				return;
			}
		}
		bool num = ms_WaitLoadCallBacks.Count == 0;
		ms_WaitLoadCallBacks.Add(callBack);
		if (!num)
		{
			return;
		}
		Asset asset = GameEntry.Resource.LoadAssetAsync(ms_DamageNumResAssetPath, typeof(DamageNumRes));
		if (asset == null)
		{
			return;
		}
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset resAsset)
		{
			DamageNumRes damageNumRes2 = null;
			if (resAsset.isError)
			{
				Log.Error("DamageNum DamageNumResLoader Load Res Asset Error! :" + resAsset.error);
			}
			else
			{
				damageNumRes2 = resAsset.asset as DamageNumRes;
				if (damageNumRes2 != null)
				{
					ms_DamageNumResAsset = resAsset;
				}
			}
			foreach (Action<DamageNumRes> ms_WaitLoadCallBack in ms_WaitLoadCallBacks)
			{
				try
				{
					ms_WaitLoadCallBack?.Invoke(damageNumRes2);
				}
				catch (Exception ex)
				{
					Log.Error("DamageNum DamageNumResLoader GetResAsset callBack Error:" + ex.Message);
				}
			}
			ms_WaitLoadCallBacks.Clear();
		});
	}

	public static void Release()
	{
		ms_WaitLoadCallBacks.Clear();
		if (ms_DamageNumResAsset != null)
		{
			ms_DamageNumResAsset.Release();
		}
		ms_DamageNumResAsset = null;
	}
}
