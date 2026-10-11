using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineHelper
{
	public static GameObject GetGenericBindingByTrackName(PlayableDirector director, string trackName)
	{
		if (director == null)
		{
			return null;
		}
		TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
		if (timelineAsset == null)
		{
			return null;
		}
		foreach (PlayableBinding output in timelineAsset.outputs)
		{
			if (output.streamName == trackName)
			{
				Object genericBinding = director.GetGenericBinding(output.sourceObject);
				if (genericBinding is GameObject result)
				{
					return result;
				}
				if (genericBinding is Component { gameObject: var gameObject })
				{
					return gameObject;
				}
			}
		}
		return null;
	}

	public static List<GameObject> GetGenericBindingListByTrackName(PlayableDirector director, string trackName)
	{
		if (director == null)
		{
			return new List<GameObject>();
		}
		TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
		if (timelineAsset == null)
		{
			return new List<GameObject>();
		}
		List<GameObject> list = new List<GameObject>();
		foreach (PlayableBinding output in timelineAsset.outputs)
		{
			if (output.streamName == trackName)
			{
				Object genericBinding = director.GetGenericBinding(output.sourceObject);
				if (genericBinding is GameObject item)
				{
					list.Add(item);
				}
				else if (genericBinding is Component component)
				{
					list.Add(component.gameObject);
				}
			}
		}
		return list;
	}
}
