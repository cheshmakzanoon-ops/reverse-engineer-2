using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.GGGo.Client;

public class SystemAutoRollClient : IEcsInitSystem, IEcsSystem, IEcsRunSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnvClient> _env;

	private float _clientDistance;

	private float _clientRollSpeed;

	private bool _isRollingToDestination;

	private float _lastRollSpeed;

	public void Init(IEcsSystems systems)
	{
		GGGoScene scene = _env.Value.Scene;
		if (!(scene == null))
		{
			if (scene.BgStart != null)
			{
				scene.BgStart.gameObject.SetActive(value: true);
			}
			GGGoRollBackground rollRoot = scene.RollRoot;
			float preUnit = _env.Value.Scene.PreUnit;
			float asFloat = _env.Value.Distance.AsFloat;
			rollRoot.Init(asFloat, preUnit);
		}
	}

	public void Run(IEcsSystems systems)
	{
		GGGoEnvClient value = _env.Value;
		if (value.IsPaused)
		{
			value.Scene.RollRoot.RollDist(value.Distance.AsFloat, 0f);
		}
		else
		{
			if (value.Cinematic.OverrideRoll)
			{
				return;
			}
			if (value.GameOver && value.GameResult != null && value.GameResult.IsDestination)
			{
				int lastEntity;
				float asFloat = (FuncRegion.GetRegionsLastPos(_world.Value, out lastEntity) + _env.Value.Level.RangeVertical.Y).AsFloat;
				if (!_isRollingToDestination)
				{
					_isRollingToDestination = true;
					_clientDistance = value.Scene.RollRoot.transform.localPosition.y / value.Scene.PreUnit;
					_clientRollSpeed = _lastRollSpeed;
				}
				if (_clientDistance >= asFloat || _clientRollSpeed <= 0f)
				{
					_clientDistance = asFloat;
					_clientRollSpeed = 0f;
					value.Scene.RollRoot.RollDist(asFloat, 0f);
				}
				else
				{
					value.Scene.RollRoot.RollDist(_clientDistance, _clientRollSpeed);
					_clientDistance += _clientRollSpeed * value.LogicTickDelta.AsFloat;
				}
			}
			else
			{
				_isRollingToDestination = false;
				if (value.GameOver)
				{
					value.Scene.RollRoot.RollDist(value.Distance.AsFloat, 0f);
					return;
				}
				_lastRollSpeed = value.RollSpeed.AsFloat;
				value.Scene.RollRoot.RollDist(value.Distance.AsFloat, value.RollSpeed.AsFloat);
			}
		}
	}
}
