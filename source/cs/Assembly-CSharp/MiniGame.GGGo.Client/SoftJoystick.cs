using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGame.GGGo.Client;

[RequireComponent(typeof(RectTransform))]
public class SoftJoystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
{
	[Header("摇杆配置")]
	[Tooltip("摇杆可视外圈（背景），按下时跟随手指位置")]
	[SerializeField]
	private RectTransform _joystickBg;

	[Tooltip("摇杆可视内圈（手柄），跟随偏移方向移动")]
	[SerializeField]
	private RectTransform _joystickHandle;

	[Tooltip("手柄距中心的最大像素距离")]
	[SerializeField]
	private float _maxRadius = 80f;

	[Tooltip("死区阈值（0~1）：归一化偏移量低于此值视为无输入")]
	[SerializeField]
	[Range(0f, 0.5f)]
	private float _deadZone = 0.1f;

	[Tooltip("松手后摇杆背景回到此 anchoredPosition（相对其父节点）")]
	[SerializeField]
	private Vector2 _initialCenter = Vector2.zero;

	[Header("输出事件（可直接在 Inspector 中绑定方法）")]
	[Tooltip("摇杆按下时触发，参数为 (0,0)")]
	public Vector2Event onJoystickStart = new Vector2Event();

	[Tooltip("摇杆拖拽时每帧触发，参数为归一化二维方向")]
	public Vector2Event onJoystickMove = new Vector2Event();

	[Tooltip("摇杆松手时触发，参数为 (0,0)")]
	public Vector2Event onJoystickEnd = new Vector2Event();

	private RectTransform _rectTransform;

	private int _pointerId = -1;

	private Vector2 _bgOrigin;

	public Vector2 Direction { get; private set; }

	public bool IsActive { get; private set; }

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		if (_joystickBg != null)
		{
			_joystickBg.anchoredPosition = _initialCenter;
		}
		if (_joystickHandle != null)
		{
			_joystickHandle.anchoredPosition = Vector2.zero;
		}
	}

	private void OnDisable()
	{
		ForceReset();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (_pointerId == -1)
		{
			_pointerId = eventData.pointerId;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(GetBgParent(), eventData.position, eventData.pressEventCamera, out var localPoint);
			_bgOrigin = localPoint;
			if (_joystickBg != null)
			{
				_joystickBg.anchoredPosition = localPoint;
			}
			if (_joystickHandle != null)
			{
				_joystickHandle.anchoredPosition = Vector2.zero;
			}
			Direction = Vector2.zero;
			IsActive = true;
			onJoystickStart?.Invoke(Direction);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.pointerId == _pointerId)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(GetBgParent(), eventData.position, eventData.pressEventCamera, out var localPoint);
			Vector2 vector = localPoint - _bgOrigin;
			if (vector.magnitude > _maxRadius)
			{
				vector = vector.normalized * _maxRadius;
			}
			if (_joystickHandle != null)
			{
				_joystickHandle.anchoredPosition = vector;
			}
			Vector2 direction = ((_maxRadius > 0f) ? (vector / _maxRadius) : Vector2.zero);
			if (direction.magnitude < _deadZone)
			{
				direction = Vector2.zero;
			}
			Direction = direction;
			onJoystickMove?.Invoke(Direction);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.pointerId == _pointerId)
		{
			ForceReset();
			onJoystickEnd?.Invoke(Vector2.zero);
		}
	}

	private RectTransform GetBgParent()
	{
		if (_joystickBg != null && _joystickBg.parent is RectTransform result)
		{
			return result;
		}
		return _rectTransform;
	}

	private void ForceReset()
	{
		_pointerId = -1;
		IsActive = false;
		Direction = Vector2.zero;
		if (_joystickBg != null)
		{
			_joystickBg.anchoredPosition = _initialCenter;
		}
		if (_joystickHandle != null)
		{
			_joystickHandle.anchoredPosition = Vector2.zero;
		}
	}
}
