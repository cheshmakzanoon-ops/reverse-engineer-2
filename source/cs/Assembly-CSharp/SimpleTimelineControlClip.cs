using System;
using UnityEngine;
using UnityEngine.Timeline;

[Serializable]
public class SimpleTimelineControlClip : SimpleTimelineClip
{
	public GameObject gameObject;

	public bool controlActivation;

	public ActivationControlPlayable.PostPlaybackState postPlayback;

	public bool updateParticle = true;

	public uint particleRandomSeed;

	private bool _initialized;

	private ParticleSystem[] _particles;

	private bool _initActived;

	public override ESimpleTimelineClipType ClipType => ESimpleTimelineClipType.ControlClip;

	public override bool OnBehaviourPlay(SimpleTimelineFrameTime frame)
	{
		Initialize();
		ActivityPlay(frame);
		ParticlePlay(frame);
		return true;
	}

	public override void OnBehaviourPause()
	{
		Initialize();
		ParticlePause();
		ActivityPause();
	}

	public override void OnBehaviourDestroy()
	{
		if (_particles != null)
		{
			for (int i = 0; i < _particles.Length; i++)
			{
				if (_particles[i] != null)
				{
					_particles[i].Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
			}
		}
		if (controlActivation && gameObject != null)
		{
			switch (postPlayback)
			{
			case ActivationControlPlayable.PostPlaybackState.Active:
				gameObject.SetActive(value: true);
				break;
			case ActivationControlPlayable.PostPlaybackState.Inactive:
				gameObject.SetActive(value: false);
				break;
			case ActivationControlPlayable.PostPlaybackState.Revert:
				gameObject.SetActive(_initActived);
				break;
			}
		}
	}

	public void Initialize()
	{
		if (_initialized || gameObject == null)
		{
			return;
		}
		_initialized = true;
		if (updateParticle && _particles == null)
		{
			_particles = gameObject.GetComponentsInChildren<ParticleSystem>(includeInactive: true) ?? Array.Empty<ParticleSystem>();
			ParticleSystem[] particles = _particles;
			foreach (ParticleSystem particleSystem in particles)
			{
				particleSystem.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
				particleSystem.useAutoRandomSeed = false;
				if (particleRandomSeed != 0)
				{
					particleSystem.randomSeed = particleRandomSeed;
				}
				else
				{
					particleSystem.randomSeed = (uint)UnityEngine.Random.Range(0, int.MaxValue);
				}
			}
		}
		if (controlActivation)
		{
			_initActived = gameObject.activeSelf;
		}
	}

	public ParticleSystem[] GetParticles()
	{
		return _particles;
	}

	private void ParticlePlay(SimpleTimelineFrameTime frame)
	{
		if (!updateParticle || _particles == null)
		{
			return;
		}
		float local = frame.local;
		for (int i = 0; i < _particles.Length; i++)
		{
			ParticleSystem particleSystem = _particles[i];
			particleSystem.Clear(withChildren: false);
			if (local >= 0.1f)
			{
				particleSystem.Simulate(local, withChildren: false, restart: true);
				particleSystem.Play(withChildren: false);
			}
			else
			{
				particleSystem.Play(withChildren: false);
			}
		}
	}

	private void ParticlePause()
	{
		if (!updateParticle || _particles == null)
		{
			return;
		}
		for (int i = 0; i < _particles.Length; i++)
		{
			if (_particles[i] != null)
			{
				_particles[i].Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
	}

	private void ActivityPlay(SimpleTimelineFrameTime frame)
	{
		if (controlActivation && gameObject != null)
		{
			gameObject.SetActive(value: true);
		}
	}

	private void ActivityPause()
	{
		if (controlActivation && gameObject != null)
		{
			gameObject.SetActive(value: false);
		}
	}
}
