using Leopotam.EcsLite;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace MiniGame.GGGo.Client;

[ExecuteAlways]
public class GGGoCacheableSpine : GGGoCacheableResource
{
	public float TimeDiffThreshold = 0.2f;

	private SkeletonAnimation _anim;

	private SkeletonBundlePlayer _player;

	private string _playName;

	private bool _playLoop;

	private int _playAtTick;

	private GGGoEnvClient _env;

	private string _default;

	public void Awake()
	{
		_anim = base.gameObject.GetComponentInChildren<SkeletonAnimation>();
		if (!(_anim == null))
		{
			_player = _anim.gameObject.GetComponent<SkeletonBundlePlayer>();
			_default = _anim.AnimationName;
		}
	}

	public override void BindEntity(EcsPackedEntityWithWorld entity)
	{
		base.BindEntity(entity);
		_env = entity.World.GetShared<GGGoEnvClient>();
	}

	public void PlayAnimation(string animName, bool loop)
	{
		if (_env != null)
		{
			float time = CalcPlayTime(_env.LogicTickCount);
			PlayAnimationImpl(animName, loop, time);
		}
	}

	private float CalcPlayTime(int tick)
	{
		_playAtTick = tick;
		if (_env.IsRollbacking)
		{
			return (float)(_env.RollbackTargetTick - _playAtTick) * _env.LogicTickDelta.AsFloat;
		}
		return 0f;
	}

	private bool IsPlayingNoise(string name, float time)
	{
		if (_playName != name)
		{
			return false;
		}
		if (_env != null && _env.IsRollbacking && TimeDiffThreshold > 0f)
		{
			TrackEntry current = ((_player != null) ? _player.skeleton : _anim).AnimationState.GetCurrent(0);
			if (current != null && Mathf.Abs(current.TrackTime - time) < TimeDiffThreshold)
			{
				return true;
			}
		}
		return false;
	}

	private void PlayAnimationImpl(string name, bool loop, float time)
	{
		if (!IsPlayingNoise(name, time))
		{
			_playName = name;
			_playLoop = loop;
			if (_player != null)
			{
				_player.Play(name);
			}
			else if (_anim != null)
			{
				_anim.AnimationState.SetAnimation(0, name, loop);
			}
			UpdateAnimationImpl(time);
		}
	}

	private void UpdateAnimationImpl(float time)
	{
		if (_player != null)
		{
			_player.UpdatePlayTime(_playName, time);
		}
		else if (_anim != null)
		{
			TrackEntry current = _anim.AnimationState.GetCurrent(0);
			if (current != null)
			{
				current.TrackTime = time;
				_anim.Update(0f);
				_anim.LateUpdate();
			}
		}
	}

	public override IGGGoCacheableResourceState SaveState()
	{
		if (!_env.IsRollbacking)
		{
			return null;
		}
		if (_playName == _default)
		{
			return null;
		}
		return new GGGoSpineState
		{
			Name = _playName,
			PlayAtTick = _playAtTick,
			Loop = _playLoop
		};
	}

	public override void LoadState(IGGGoCacheableResourceState state)
	{
		if (!_env.IsRollbacking)
		{
			PlayAnimationImpl(_default, loop: true, 0f);
		}
		if (state is GGGoSpineState gGGoSpineState && !string.IsNullOrEmpty(gGGoSpineState.Name))
		{
			float time = CalcPlayTime(gGGoSpineState.PlayAtTick);
			PlayAnimationImpl(gGGoSpineState.Name, gGGoSpineState.Loop, time);
		}
		else
		{
			PlayAnimationImpl(_default, loop: true, 0f);
		}
	}

	private void Dump(string dump)
	{
	}
}
