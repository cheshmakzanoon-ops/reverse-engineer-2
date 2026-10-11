using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView;

public class ItemPool
{
	private GameObject mPrefabObj;

	private string mPrefabName;

	private int mInitCreateCount = 1;

	private float mPadding;

	private float mStartPosOffset;

	private bool mAnimationCancle;

	private string mAnimatorPath = string.Empty;

	private Queue<LoopListViewItem2> mTmpPooledItemList = new Queue<LoopListViewItem2>();

	private Queue<LoopListViewItem2> mPooledItemList = new Queue<LoopListViewItem2>();

	private static int mCurItemIdCount;

	private RectTransform mItemParent;

	public int CacheCount => mTmpPooledItemList.Count + mPooledItemList.Count;

	public bool IsPrefab => mPrefabObj?.scene.name == null;

	public bool mUseCanvas { get; set; }

	public void Init(string prefabName, ItemPrefabConfData data, RectTransform parent, Action<LoopListViewItem2> arabicCallback = null)
	{
		mPrefabObj = data.mItemPrefab;
		mPrefabName = prefabName;
		mInitCreateCount = data.mInitCreateCount;
		mPadding = data.mPadding;
		mStartPosOffset = data.mStartPosOffset;
		mItemParent = parent;
		mAnimationCancle = data.mAnimationCancle;
		mAnimatorPath = data.mAnimatorPath;
		mPrefabObj.SetActive(value: false);
		for (int i = 0; i < mInitCreateCount; i++)
		{
			LoopListViewItem2 loopListViewItem = CreateItem();
			if (IsPrefab)
			{
				arabicCallback?.Invoke(loopListViewItem);
			}
			RecycleItemReal(loopListViewItem);
		}
	}

	public LoopListViewItem2 GetItem()
	{
		mCurItemIdCount++;
		LoopListViewItem2 loopListViewItem = null;
		if (mTmpPooledItemList.Count > 0)
		{
			loopListViewItem = mTmpPooledItemList.Dequeue();
			loopListViewItem.SetVisible(visible: true, !mUseCanvas);
		}
		else if (mPooledItemList.Count == 0)
		{
			loopListViewItem = CreateItem();
		}
		else
		{
			loopListViewItem = mPooledItemList.Dequeue();
			loopListViewItem.SetVisible(visible: true, !mUseCanvas);
		}
		loopListViewItem.Padding = mPadding;
		loopListViewItem.ItemId = mCurItemIdCount;
		return loopListViewItem;
	}

	public void DestroyAllItem()
	{
		ClearTmpRecycledItem();
		int count = mPooledItemList.Count;
		for (int i = 0; i < count; i++)
		{
			UnityEngine.Object.Destroy(mPooledItemList.Dequeue().gameObject);
		}
		mPooledItemList.Clear();
	}

	public LoopListViewItem2 CreateItem()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(mPrefabObj, Vector3.zero, Quaternion.identity, mItemParent);
		gameObject.SetActive(value: true);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.localScale = Vector3.one;
		component.localPosition = Vector3.zero;
		component.localEulerAngles = Vector3.zero;
		LoopListViewItem2 component2 = gameObject.GetComponent<LoopListViewItem2>();
		component2.ItemPrefabName = mPrefabName;
		component2.StartPosOffset = mStartPosOffset;
		component2.ItemAnimatorPath = mAnimatorPath;
		component2.ItemCancleAnimation = mAnimationCancle;
		if (mUseCanvas)
		{
			gameObject.AddComponent<Canvas>().additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
			gameObject.AddComponent<GraphicRaycaster>();
		}
		return component2;
	}

	private void RecycleItemReal(LoopListViewItem2 item)
	{
		item.SetVisible(visible: false, !mUseCanvas);
		mPooledItemList.Enqueue(item);
	}

	public void RecycleItem(LoopListViewItem2 item)
	{
		mTmpPooledItemList.Enqueue(item);
	}

	public void ClearTmpRecycledItem()
	{
		int count = mTmpPooledItemList.Count;
		if (count != 0)
		{
			for (int i = 0; i < count; i++)
			{
				LoopListViewItem2 item = mTmpPooledItemList.Dequeue();
				RecycleItemReal(item);
			}
			mTmpPooledItemList.Clear();
		}
	}
}
