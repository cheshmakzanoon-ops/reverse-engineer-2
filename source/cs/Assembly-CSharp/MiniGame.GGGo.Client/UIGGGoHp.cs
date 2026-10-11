using System.Collections.Generic;
using UnityEngine;

namespace MiniGame.GGGo.Client;

[ExecuteInEditMode]
public class UIGGGoHp : MonoBehaviour
{
	public UIGGGoHpItem HpItemPrefab;

	public Transform ItemsParent;

	public List<UIGGGoHpItem> _items = new List<UIGGGoHpItem>();

	private int _currentHp;

	private void Reset()
	{
		if (ItemsParent == null && HpItemPrefab != null)
		{
			ItemsParent = HpItemPrefab.transform.parent;
		}
	}

	private void Awake()
	{
		_items.RemoveAll((UIGGGoHpItem item) => item == null);
		if (HpItemPrefab != null)
		{
			HpItemPrefab.gameObject.SetActive(value: false);
			HpItemPrefab.SetFilledImmediate(filled: false);
		}
	}

	public void Init(int maxHp, int startHp)
	{
		if (maxHp < 0)
		{
			maxHp = 0;
		}
		EnsureItems(maxHp);
		startHp = Mathf.Clamp(startHp, 0, maxHp);
		SetHpInstant(startHp);
		_currentHp = startHp;
	}

	public void SetHpInstant(int hp)
	{
		if (_items == null)
		{
			return;
		}
		hp = Mathf.Clamp(hp, 0, _items.Count);
		for (int i = 0; i < _items.Count; i++)
		{
			UIGGGoHpItem uIGGGoHpItem = _items[i];
			if (!(uIGGGoHpItem == null))
			{
				uIGGGoHpItem.gameObject.SetActive(value: true);
				uIGGGoHpItem.transform.SetSiblingIndex(i);
				uIGGGoHpItem.SetFilledImmediate(i < hp);
			}
		}
		_currentHp = hp;
	}

	public void SetHp(int newHp)
	{
		SetHpInstant(newHp);
	}

	private void EnsureItems(int count)
	{
		if (HpItemPrefab == null)
		{
			return;
		}
		if (ItemsParent == null)
		{
			ItemsParent = HpItemPrefab.transform.parent;
		}
		for (int i = _items.Count; i < count; i++)
		{
			GameObject gameObject = Object.Instantiate(HpItemPrefab.gameObject, ItemsParent);
			gameObject.SetActive(value: true);
			UIGGGoHpItem component = gameObject.GetComponent<UIGGGoHpItem>();
			if (component != null)
			{
				component.SetFilledImmediate(filled: false);
				_items.Add(component);
			}
			else
			{
				Object.Destroy(gameObject);
			}
		}
		for (int j = 0; j < _items.Count; j++)
		{
			UIGGGoHpItem uIGGGoHpItem = _items[j];
			if (!(uIGGGoHpItem == null))
			{
				uIGGGoHpItem.transform.SetSiblingIndex(j);
				if (j >= count)
				{
					uIGGGoHpItem.SetFilledImmediate(filled: false);
					uIGGGoHpItem.gameObject.SetActive(value: true);
				}
			}
		}
	}
}
