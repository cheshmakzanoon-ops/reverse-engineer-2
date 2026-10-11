using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class SystemGameOverCinematicClient : IEcsInitSystem, IEcsSystem, IEcsRunSystem, IEcsDestroySystem
{
	private enum Phase
	{
		Idle,
		Cinematic,
		Recover
	}

	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnvClient> _env;

	private const float SlowMoScale = 0.5f;

	private const float CinematicTime = 2.5f;

	private const float CameraLerpSpeed = 4f;

	private const float ZoomInAmount = 0.85f;

	private const float MinAllowedOrthoRatio = 0.5f;

	private const float ZoomLerpSpeed = 3f;

	private const float RecoverSpeed = 2f;

	private const float FocusFootRoomRatio = 0.9f;

	private const float FocusSafetyPadding = 20f;

	private Phase _phase;

	private float _timer;

	private float _targetRollY;

	private float _targetCamX;

	private float _targetCamY;

	private float _originCamX;

	private float _originCamY;

	private float _originOrthoSize;

	private float _targetOrthoSize;

	private bool _played;

	private float _maxRollY;

	public void Init(IEcsSystems systems)
	{
		_phase = Phase.Idle;
		_timer = 0f;
		_played = false;
		Time.timeScale = 1f;
	}

	public void Destroy(IEcsSystems systems)
	{
		Time.timeScale = 1f;
		GGGoEnvClient value = _env.Value;
		if (value != null)
		{
			value.IgnoreInput = false;
			value.Cinematic.OverrideRoll = false;
		}
	}

	public void Run(IEcsSystems systems)
	{
		GGGoEnvClient value = _env.Value;
		switch (_phase)
		{
		case Phase.Idle:
			if (value.Cinematic.Triggered && !_played)
			{
				EnterCinematic(value);
			}
			break;
		case Phase.Cinematic:
			UpdateCinematic(value);
			break;
		case Phase.Recover:
			UpdateRecover(value);
			break;
		}
	}

	private void EnterCinematic(GGGoEnvClient env)
	{
		_phase = Phase.Cinematic;
		_timer = 0f;
		_played = true;
		env.IgnoreInput = true;
		if (!env.Cinematic.SkipEffects)
		{
			Time.timeScale = 0.5f;
			env.Cinematic.OverrideRoll = true;
			int lastEntity;
			float asFloat = (FuncRegion.GetRegionsLastPos(_world.Value, out lastEntity) + env.Level.RangeVertical.Y).AsFloat;
			_maxRollY = asFloat * env.Scene.PreUnit;
			_targetRollY = FindTargetRollY(env);
			Camera camera = env.Scene?.Camera;
			if (camera != null)
			{
				_originOrthoSize = camera.orthographicSize;
				_targetOrthoSize = FindTargetOrthoSize(env, _originOrthoSize * 0.85f);
				_originCamX = camera.transform.localPosition.x;
				_originCamY = camera.transform.localPosition.y;
				_targetCamX = FindTargetCamX(env, camera, _targetOrthoSize);
				_targetRollY = FindTargetRollY(env, _targetOrthoSize);
				_targetCamY = FindTargetCamY(env, _targetOrthoSize, _targetRollY);
			}
		}
	}

	private void UpdateCinematic(GGGoEnvClient env)
	{
		if (env.Cinematic.SkipEffects)
		{
			_timer += Time.unscaledDeltaTime * 5f;
		}
		else
		{
			_timer += Time.unscaledDeltaTime;
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			GGGoRollBackground gGGoRollBackground = env.Scene?.RollRoot;
			Camera camera = env.Scene?.Camera;
			if (gGGoRollBackground != null)
			{
				Vector3 localPosition = gGGoRollBackground.transform.localPosition;
				localPosition.y = Mathf.Lerp(localPosition.y, _targetRollY, unscaledDeltaTime * 4f);
				gGGoRollBackground.transform.localPosition = localPosition;
				gGGoRollBackground.RollBgManual(localPosition.y);
			}
			if (camera != null)
			{
				camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, _targetOrthoSize, unscaledDeltaTime * 3f);
				Vector3 localPosition2 = camera.transform.localPosition;
				localPosition2.x = Mathf.Lerp(localPosition2.x, _targetCamX, unscaledDeltaTime * 4f);
				localPosition2.y = Mathf.Lerp(localPosition2.y, _targetCamY, unscaledDeltaTime * 4f);
				camera.transform.localPosition = localPosition2;
				float b = ((env.Scene.transform.parent != null) ? env.Scene.transform.parent.lossyScale.y : 1f);
				float num = camera.orthographicSize / Mathf.Max(0.0001f, b);
				float num2 = ((gGGoRollBackground != null) ? gGGoRollBackground.transform.localPosition.y : 0f) + camera.transform.localPosition.y;
				float num3 = num2 - num;
				float num4 = num2 + num;
				float num5 = env.Cinematic.FocusLogicBottomY * env.Scene.PreUnit;
				float num6 = env.Cinematic.FocusLogicTopY * env.Scene.PreUnit;
				Debug.Log($"[Cinematic] frame camY={camera.transform.localPosition.y:F1} rollY={((gGGoRollBackground != null) ? gGGoRollBackground.transform.localPosition.y : 0f):F1} centerY={num2:F1} ortho={camera.orthographicSize:F1} visibleBottom={num3:F1} visibleTop={num4:F1} focusBottom={num5:F1} focusTop={num6:F1}");
			}
		}
		if (_timer >= 2.5f)
		{
			Time.timeScale = 1f;
			env.Cinematic.Done = true;
			env.Cinematic.OverrideRoll = false;
			_phase = Phase.Recover;
		}
	}

	private void UpdateRecover(GGGoEnvClient env)
	{
		if (env.Cinematic.SkipEffects)
		{
			return;
		}
		Camera camera = env.Scene?.Camera;
		if (!(camera == null))
		{
			float deltaTime = Time.deltaTime;
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, _originOrthoSize, deltaTime * 2f);
			if (Mathf.Abs(camera.orthographicSize - _originOrthoSize) < 0.01f)
			{
				camera.orthographicSize = _originOrthoSize;
			}
			Vector3 localPosition = camera.transform.localPosition;
			localPosition.x = Mathf.Lerp(localPosition.x, _originCamX, deltaTime * 2f);
			localPosition.y = Mathf.Lerp(localPosition.y, _originCamY, deltaTime * 2f);
			if (Mathf.Abs(localPosition.x - _originCamX) < 0.5f)
			{
				localPosition.x = _originCamX;
			}
			if (Mathf.Abs(localPosition.y - _originCamY) < 0.5f)
			{
				localPosition.y = _originCamY;
			}
			camera.transform.localPosition = localPosition;
		}
	}

	private float FindTargetRollY(GGGoEnvClient env)
	{
		return FindTargetRollY(env, _originOrthoSize * 0.85f);
	}

	private float FindTargetRollY(GGGoEnvClient env, float orthoSize)
	{
		return Mathf.Clamp(_maxRollY, _maxRollY, 0f);
	}

	private float FindTargetCamY(GGGoEnvClient env, float orthoSize, float targetRollY)
	{
		if (env.Cinematic.FocusLogicBottomY <= float.MinValue || env.Cinematic.FocusLogicTopY <= float.MinValue)
		{
			return _originCamY;
		}
		float preUnit = env.Scene.PreUnit;
		float num = env.Cinematic.FocusLogicBottomY * preUnit;
		float num2 = env.Cinematic.FocusLogicTopY * preUnit;
		float b = ((env.Scene.transform.parent != null) ? env.Scene.transform.parent.lossyScale.y : 1f);
		float num3 = orthoSize / Mathf.Max(0.0001f, b);
		float num4 = num3 * 0.9f;
		float num5 = num3 * 0.100000024f;
		float num6 = num2 - num5 + 20f;
		float num7 = num - num4 - 20f;
		float num8 = Mathf.Clamp(num7, num6, num7) - targetRollY;
		Debug.Log($"[Cinematic] targetCamY={num8:F1} rollY={targetRollY:F1} bottomY={num:F1} topY={num2:F1} camHalfH={num3:F1} minCenterY={num6:F1} maxCenterY={num7:F1}");
		return num8;
	}

	private float FindTargetOrthoSize(GGGoEnvClient env, float desiredOrthoSize)
	{
		float min = _originOrthoSize * 0.5f;
		float originOrthoSize = _originOrthoSize;
		float value = desiredOrthoSize;
		if (env.Cinematic.FocusLogicBottomY > float.MinValue && env.Cinematic.FocusLogicTopY > float.MinValue)
		{
			float preUnit = env.Scene.PreUnit;
			float num = env.Cinematic.FocusLogicBottomY * preUnit;
			float num2 = env.Cinematic.FocusLogicTopY * preUnit;
			float num3 = Mathf.Max(0f, num2 - num);
			float b = ((env.Scene.transform.parent != null) ? env.Scene.transform.parent.lossyScale.y : 1f);
			float b2 = Mathf.Max((num3 + 40f) * 0.5f, 20f / Mathf.Max(0.0001f, 0.9f), 20f / Mathf.Max(0.0001f, 0.100000024f)) * Mathf.Max(0.0001f, b);
			value = Mathf.Min(desiredOrthoSize, b2);
		}
		return Mathf.Clamp(value, min, originOrthoSize);
	}

	private float FindTargetCamX(GGGoEnvClient env, Camera cam, float orthoSize)
	{
		if (env.Cinematic.FocusLogicX <= float.MinValue)
		{
			return _originCamX;
		}
		float preUnit = env.Scene.PreUnit;
		float num = env.Cinematic.FocusLogicX * preUnit;
		float num2 = (float)env.Level.RangeHorizon.Y * preUnit;
		float b = ((env.Scene.transform.parent != null) ? env.Scene.transform.parent.lossyScale.x : 1f);
		float num3 = orthoSize * cam.aspect / Mathf.Max(0.0001f, b);
		float num4 = Mathf.Max(0f, num2 - num3);
		float num5 = Mathf.Clamp(num, 0f - num4, num4);
		Debug.Log($"[Cinematic] camX: playerPx={num:F1} levelHalfPx={num2:F1} camHalfW={num3:F1} maxOffset={num4:F1} clamped={num5:F1}");
		return num5;
	}
}
