using Leopotam.EcsLite;
using MiniGame.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.GGGo.Client;

public class UIGGGoProgress : MonoBehaviour
{
	[Tooltip("从上到下，0-1显示当前关卡进度")]
	[SerializeField]
	private Slider _Slider;

	[Tooltip("显示在关卡进度上的自己头像，高度位置随着关卡进度变化")]
	[SerializeField]
	private RectTransform _HeadSelf;

	[Tooltip("显示在关卡进度上的对手头像，高度位置随着关卡进度变化")]
	[SerializeField]
	private RectTransform _HeadOther;

	[SerializeField]
	private Transform _SubRoot;

	[Tooltip("分割线预制体，实例化后放在_SubRoot下，按照关卡数量等分布在进度条上")]
	[SerializeField]
	private GameObject _SubTemp;

	private GameWorld world;

	private GGGoEnvClient envClient;

	private float _totalDistance = 1f;

	private float _progress01;

	private int _selfEntity = -1;

	private int _otherEntity = -1;

	public void Init(GameWorld world)
	{
		this.world = world;
		envClient = world?.Env as GGGoEnvClient;
		InitHeads();
		CachePlayerEntities();
		EcsWorld ecsWorld = this.world?.World;
		if (ecsWorld != null && envClient != null)
		{
			int lastEntity;
			float num = (float)(FuncRegion.GetRegionsLastPos(ecsWorld, out lastEntity) + envClient.Level.RangeVertical.Y);
			_totalDistance = Mathf.Max(1f, 0f - num);
		}
		UpdateSub(_totalDistance, 10f);
		UpdateInfo();
	}

	private void Update()
	{
		UpdateInfo();
	}

	private void InitHeads()
	{
		if (world?.World != null)
		{
			int entitiesCount = world.World.Filter<ComponentPlayer>().End().GetEntitiesCount();
			if (_HeadOther != null)
			{
				_HeadOther.gameObject.SetActive(entitiesCount > 1);
			}
			if (_HeadSelf != null)
			{
				_HeadSelf.gameObject.SetActive(entitiesCount > 0);
			}
		}
	}

	private void UpdateInfo()
	{
		if (envClient != null && world?.World != null)
		{
			float num = Mathf.Max(0f, 0f - (float)envClient.Distance);
			_progress01 = Mathf.Clamp01(num / Mathf.Max(1f, _totalDistance));
			UpdateSlider();
			UpdateHeads();
		}
	}

	private void UpdateSlider()
	{
		if (!(_Slider == null))
		{
			_Slider.value = _progress01;
		}
	}

	private void CachePlayerEntities()
	{
		_selfEntity = -1;
		_otherEntity = -1;
		if (world?.World == null || envClient == null)
		{
			return;
		}
		EcsWorld ecsWorld = world.World;
		EcsFilter ecsFilter = ecsWorld.Filter<ComponentPlayer>().Inc<ComponentPosition>().End();
		EcsPool<ComponentPlayer> pool = ecsWorld.GetPool<ComponentPlayer>();
		EPlayerID playerID = envClient.GetPlayerID();
		foreach (int item in ecsFilter)
		{
			if (world.World.IsEntityAliveInternal(item))
			{
				if (pool.Get(item).PlayerID == playerID)
				{
					_selfEntity = item;
				}
				else if (_otherEntity < 0)
				{
					_otherEntity = item;
				}
			}
		}
	}

	private void UpdateHeads()
	{
		if (_Slider == null || world?.World == null)
		{
			return;
		}
		RectTransform rectTransform = _Slider.transform as RectTransform;
		if (!(rectTransform == null))
		{
			EcsWorld ecsWorld = world.World;
			EcsPool<ComponentPosition> pool = ecsWorld.GetPool<ComponentPosition>();
			int lastEntity;
			float num = (float)FuncRegion.GetRegionsLastPos(ecsWorld, out lastEntity);
			if (num >= 0f)
			{
				num = -1f;
			}
			float yMin = rectTransform.rect.yMin;
			float yMax = rectTransform.rect.yMax;
			if (_HeadSelf != null && _selfEntity >= 0 && pool.Has(_selfEntity))
			{
				float t = Mathf.Clamp01((0f - (float)pool.Get(_selfEntity).Position.Y) / (0f - num));
				float y = Mathf.Lerp(yMax, yMin, t);
				Vector2 anchoredPosition = _HeadSelf.anchoredPosition;
				_HeadSelf.anchoredPosition = new Vector2(anchoredPosition.x, y);
			}
			if (_HeadOther != null && _HeadOther.gameObject.activeSelf && _otherEntity >= 0 && pool.Has(_otherEntity))
			{
				float t2 = Mathf.Clamp01((0f - (float)pool.Get(_otherEntity).Position.Y) / (0f - num));
				float y2 = Mathf.Lerp(yMax, yMin, t2);
				Vector2 anchoredPosition2 = _HeadOther.anchoredPosition;
				_HeadOther.anchoredPosition = new Vector2(anchoredPosition2.x, y2);
			}
		}
	}

	private void UpdateSub(float totalDistance, float gap)
	{
		if (!(_SubRoot == null) && !(_SubTemp == null))
		{
			for (int num = _SubRoot.childCount - 1; num >= 0; num--)
			{
				Object.Destroy(_SubRoot.GetChild(num).gameObject);
			}
			int num2 = Mathf.FloorToInt(Mathf.Max(0f, totalDistance) / Mathf.Max(0.01f, gap));
			for (int i = 0; i < num2; i++)
			{
				Object.Instantiate(_SubTemp, _SubRoot);
			}
		}
	}
}
