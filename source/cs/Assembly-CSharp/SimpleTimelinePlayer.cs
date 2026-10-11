using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SimpleTimelinePlayer : MonoBehaviour
{
	public bool hideWhenStopped = true;

	public bool playOnAwake = true;

	public List<SimpleTimelineDirector> timelines = new List<SimpleTimelineDirector>();

	private SimpleTimelineDirector playingDirector;

	public void OnEnable()
	{
		if (hideWhenStopped && timelines != null && timelines.Count > 0)
		{
			for (int i = 0; i < timelines.Count; i++)
			{
				if (i != 0 || !playOnAwake)
				{
					timelines[i].timelineDirector.gameObject.SetActive(value: false);
				}
			}
		}
		if (playOnAwake && timelines != null && timelines.Count > 0)
		{
			Play(timelines[0].name);
		}
	}

	public void OnDisable()
	{
	}

	public void Update()
	{
		playingDirector?.Update();
	}

	public float Play(string name = null, float normalized = 0f)
	{
		if (playingDirector != null)
		{
			playingDirector.Stop(hideWhenStopped);
			playingDirector = null;
		}
		SimpleTimelineDirector director;
		float duration = GetDuration(name, out director);
		director?.Play(normalized);
		playingDirector = director;
		return duration;
	}

	public float CrossFade(string name = null, float time = 0.1f)
	{
		if (playingDirector != null)
		{
			playingDirector.StopWithoutAnimation(hideWhenStopped);
			playingDirector = null;
		}
		SimpleTimelineDirector director;
		float duration = GetDuration(name, out director);
		director?.PlayCrossAnimation(time);
		playingDirector = director;
		return duration;
	}

	public float PlayOrigin(string name = null, float normalized = 0f)
	{
		for (int i = 0; i < timelines.Count; i++)
		{
			timelines[i].Stop(hideWhenStopped);
		}
		SimpleTimelineDirector director = GetDirector(name);
		director.timelineDirector.gameObject.SetActive(value: true);
		director.timelineDirector.enabled = true;
		director.timelineDirector.time = director.initTime + normalized * director.duration;
		director.timelineDirector.Play(director.origin);
		return director.duration;
	}

	public double GetDuration(string name)
	{
		SimpleTimelineDirector director;
		return GetDuration(name, out director);
	}

	public float GetDuration(string name, out SimpleTimelineDirector director)
	{
		director = GetDirector(name);
		return director?.duration ?? 0f;
	}

	public SimpleTimelineDirector GetDirector(string name)
	{
		for (int i = 0; i < timelines.Count; i++)
		{
			if (timelines[i]?.name == name)
			{
				return timelines[i];
			}
		}
		return null;
	}

	private void OnValidate()
	{
		if (timelines == null)
		{
			timelines = new List<SimpleTimelineDirector>();
		}
		if (timelines.Count <= 0)
		{
			timelines = new List<SimpleTimelineDirector>();
			PlayableDirector[] componentsInChildren = GetComponentsInChildren<PlayableDirector>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				SimpleTimelineDirector simpleTimelineDirector = new SimpleTimelineDirector();
				timelines.Add(simpleTimelineDirector);
				simpleTimelineDirector.timelineDirector = componentsInChildren[i];
				simpleTimelineDirector.name = simpleTimelineDirector.timelineDirector.name;
				simpleTimelineDirector.origin = simpleTimelineDirector.timelineDirector.playableAsset as TimelineAsset;
			}
		}
	}
}
