using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SimpleTimelineDirector
{
	[Serializable]
	public class TimePoint
	{
		public ESimpleTimelineClipType type;

		public int index;

		public float time;

		public bool active;
	}

	public string name;

	public float initTime;

	public float duration;

	public PlayableDirector timelineDirector;

	public TimelineAsset origin;

	public TimelineAsset optimization;

	public List<SimpleTimelineControlClip> controlClips;

	public List<SimpleTimelineAnimationClip> animationClips;

	public List<TimePoint> timePoints;

	[NonSerialized]
	private int _timepointIndex;

	[NonSerialized]
	private SimpleTimelineFrameTime _runtime;

	[NonSerialized]
	private List<SimpleTimelineClip> _playFailedList;

	public DirectorUpdateMode mode { get; private set; } = DirectorUpdateMode.GameTime;

	public float GameTime
	{
		get
		{
			if (mode != DirectorUpdateMode.GameTime)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}
	}

	public float GameDeltaTime
	{
		get
		{
			if (mode != DirectorUpdateMode.GameTime)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}
	}

	public void Play(float normalizedTime = 0f)
	{
		mode = timelineDirector.timeUpdateMode;
		ClearFailedPlayClips();
		timelineDirector.gameObject.SetActive(value: true);
		_runtime.start = GameTime - duration * normalizedTime;
		_timepointIndex = 0;
		timelineDirector.Stop();
		if (optimization != null)
		{
			timelineDirector.enabled = true;
			timelineDirector.time = initTime + duration * normalizedTime;
			timelineDirector.Play(optimization);
		}
		else
		{
			timelineDirector.Stop();
			timelineDirector.enabled = false;
		}
		UpdateTimePointsOnPlay(0f);
	}

	public void PlayCrossAnimation(float crossTime)
	{
		mode = timelineDirector.timeUpdateMode;
		ClearFailedPlayClips();
		timelineDirector.gameObject.SetActive(value: true);
		_runtime.start = GameTime;
		_timepointIndex = 0;
		timelineDirector.Stop();
		if (optimization != null)
		{
			timelineDirector.enabled = true;
			timelineDirector.time = initTime;
			timelineDirector.Play(optimization);
		}
		else
		{
			timelineDirector.Stop();
			timelineDirector.enabled = false;
		}
		UpdateTimePointsOnPlay(crossTime);
	}

	public void Stop(bool hideOnStopped)
	{
		StopWithoutAnimation(hideOnStopped: false);
		foreach (SimpleTimelineAnimationClip animationClip in animationClips)
		{
			animationClip.OnBehaviourDestroy();
		}
		if (hideOnStopped)
		{
			timelineDirector.gameObject.SetActive(value: false);
		}
	}

	public void StopWithoutAnimation(bool hideOnStopped)
	{
		ClearFailedPlayClips();
		timelineDirector.Stop();
		foreach (SimpleTimelineControlClip controlClip in controlClips)
		{
			controlClip.OnBehaviourDestroy();
		}
		if (hideOnStopped)
		{
			timelineDirector.gameObject.SetActive(value: false);
		}
	}

	public void Update()
	{
		if (timePoints != null && timePoints.Count != 0)
		{
			_runtime.running = GameTime - _runtime.start + initTime;
			float loop = _runtime.loop;
			_runtime.loop = _runtime.running % duration;
			_runtime.elapsed = GameDeltaTime;
			_runtime.cross = 0f;
			_ = _runtime;
			if (loop > _runtime.loop)
			{
				UpdateTimePoints(duration + 1f);
				_timepointIndex = 0;
			}
			UpdateTimePoints(_runtime.loop);
		}
	}

	private void UpdateTimePointsOnPlay(float cross)
	{
		_runtime.running = GameTime - _runtime.start + initTime;
		_runtime.loop = _runtime.running % duration;
		_runtime.elapsed = 0f;
		_runtime.cross = cross;
		UpdateTimePoints(_runtime.loop);
	}

	private void UpdateTimePoints(float runtime)
	{
		if (_playFailedList != null)
		{
			for (int num = _playFailedList.Count - 1; num >= 0; num--)
			{
				SimpleTimelineClip simpleTimelineClip = _playFailedList[num];
				if ((double)_runtime.local >= simpleTimelineClip.end)
				{
					_playFailedList.RemoveAtSwapBack(num);
				}
				else
				{
					_runtime.local = _runtime.loop - (float)simpleTimelineClip.start;
					if (simpleTimelineClip.OnBehaviourPlay(_runtime))
					{
						_playFailedList.RemoveAtSwapBack(num);
					}
				}
			}
		}
		while (_timepointIndex < timePoints.Count && timePoints[_timepointIndex].time <= runtime)
		{
			TimePoint timePoint = timePoints[_timepointIndex];
			SimpleTimelineClip clipByTypeAndIndex = GetClipByTypeAndIndex(timePoint.type, timePoint.index);
			if (clipByTypeAndIndex != null)
			{
				if (timePoint.active)
				{
					_runtime.local = _runtime.loop - timePoint.time;
					if (!clipByTypeAndIndex.OnBehaviourPlay(_runtime))
					{
						if (_playFailedList == null)
						{
							_playFailedList = new List<SimpleTimelineClip>();
						}
						_playFailedList.Add(clipByTypeAndIndex);
					}
				}
				else
				{
					clipByTypeAndIndex?.OnBehaviourPause();
				}
			}
			_timepointIndex++;
		}
	}

	private SimpleTimelineClip GetClipByTypeAndIndex(ESimpleTimelineClipType type, int index)
	{
		if (type == ESimpleTimelineClipType.AnimationClip && index >= 0 && index < animationClips.Count)
		{
			return animationClips[index];
		}
		if (type == ESimpleTimelineClipType.ControlClip && index >= 0 && index < controlClips.Count)
		{
			return controlClips[index];
		}
		return null;
	}

	private void ClearFailedPlayClips()
	{
		if (_playFailedList != null)
		{
			_playFailedList.Clear();
		}
	}
}
