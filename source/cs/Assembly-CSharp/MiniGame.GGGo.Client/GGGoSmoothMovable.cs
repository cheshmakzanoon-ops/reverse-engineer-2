using UnityEngine;

namespace MiniGame.GGGo.Client;

public class GGGoSmoothMovable : MonoBehaviour
{
	[Header("平滑时间配置")]
	[Tooltip("正常跟手时的平滑时间，越小越跟手")]
	public float smoothTimeMin = 0.02f;

	[Tooltip("大偏差回滚时的平滑时间，越大越柔和")]
	public float smoothTimeMax = 0.25f;

	[Header("动态阈值配置")]
	[Tooltip("基础容错距离(静止时的允许误差)")]
	public float baseThreshold = 0.1f;

	[Tooltip("速度系数。允许误差 = baseThreshold + logicSpeed * smoothTimeMin * speedFactor")]
	[Range(1f, 5f)]
	public float speedFactor = 1.5f;

	[Tooltip("回滚判定的最大距离阈值(超过此距离直接使用smoothTimeMax)")]
	public float catchUpDistanceThreshold = 2f;

	[Tooltip("最大移动速度限制")]
	public float maxSmoothSpeed = 20f;

	[Tooltip("瞬移阈值")]
	public float teleportThreshold = 5f;

	private Vector3 _logicPos;

	private Vector3 _logicVelocity;

	private float _lastLogicUpdateTime;

	private Vector3 _visualPos;

	private Vector3 _smoothVelocity;

	private bool _isInitialized;

	public void UpdatePosition(Vector3 position, float interval)
	{
		if (!_isInitialized)
		{
			_logicPos = position;
			_visualPos = position;
			_lastLogicUpdateTime = Time.time;
			_isInitialized = true;
			return;
		}
		if (interval > 0.0001f)
		{
			_logicVelocity = (position - _logicPos) / interval;
		}
		_logicPos = position;
		_lastLogicUpdateTime = Time.time;
	}

	private void LateUpdate()
	{
		if (!_isInitialized)
		{
			return;
		}
		float num = Time.time - _lastLogicUpdateTime;
		Vector3 vector = _logicPos + _logicVelocity * num;
		float num2 = Vector3.Distance(_visualPos, vector);
		if (num2 > teleportThreshold)
		{
			ForceSetPosition(vector);
			return;
		}
		float magnitude = _logicVelocity.magnitude;
		float num3 = baseThreshold + magnitude * smoothTimeMin * speedFactor;
		float smoothTime;
		if (num2 <= num3)
		{
			smoothTime = smoothTimeMin;
		}
		else
		{
			float value = (num2 - num3) / (catchUpDistanceThreshold - num3);
			value = Mathf.Clamp01(value);
			value = value * value * (3f - 2f * value);
			smoothTime = Mathf.Lerp(smoothTimeMin, smoothTimeMax, value);
		}
		_visualPos = Vector3.SmoothDamp(_visualPos, vector, ref _smoothVelocity, smoothTime, maxSmoothSpeed, Time.deltaTime);
		base.transform.localPosition = _visualPos;
	}

	private void ForceSetPosition(Vector3 pos)
	{
		_visualPos = pos;
		_smoothVelocity = Vector3.zero;
		base.transform.localPosition = pos;
	}
}
