using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonAnimation))]
public class SkeletonBundlePlayer : MonoBehaviour
{
	[Serializable]
	public class AnimationBundle
	{
		[SpineAnimation("", "skeleton", true, false)]
		public string name;

		public bool loop;

		public bool resetBeforePlay;

		public ParticleSystem[] particles;

		public GameObject[] shows;
	}

	[SerializeField]
	public SkeletonAnimation skeleton;

	public AnimationBundle[] bundles;

	private void Awake()
	{
		if (skeleton == null)
		{
			skeleton = GetComponent<SkeletonAnimation>();
		}
		if (bundles == null || bundles.Length == 0)
		{
			ScanAnimations();
		}
	}

	public void Play(string name)
	{
		AnimationBundle animationBundle = FindBundle(name);
		if (animationBundle != null)
		{
			PlayBundle(animationBundle);
		}
	}

	public void Play(string name, float time)
	{
		AnimationBundle animationBundle = FindBundle(name);
		if (animationBundle != null)
		{
			PlayBundle(animationBundle, time);
		}
	}

	public void UpdatePlayTime(string name, float time)
	{
		AnimationBundle animationBundle = FindBundle(name);
		if (animationBundle == null || skeleton == null || animationBundle == null || string.IsNullOrEmpty(animationBundle.name))
		{
			return;
		}
		TrackEntry current = skeleton.AnimationState.GetCurrent(0);
		if (current != null)
		{
			current.TrackTime = time;
		}
		skeleton.Update(0f);
		skeleton.LateUpdate();
		ParticleSystem[] particles = animationBundle.particles;
		foreach (ParticleSystem particleSystem in particles)
		{
			if (!(particleSystem == null))
			{
				particleSystem.Clear();
				particleSystem.Simulate(time);
				particleSystem.Play();
			}
		}
	}

	private AnimationBundle FindBundle(string name)
	{
		AnimationBundle[] array = bundles;
		foreach (AnimationBundle animationBundle in array)
		{
			if (animationBundle.name == name)
			{
				return animationBundle;
			}
		}
		return null;
	}

	private void PlayBundle(AnimationBundle bundle)
	{
		if (bundle.resetBeforePlay)
		{
			ResetAll();
		}
		PlayAnimation(bundle);
		PlayParticles(bundle);
		ShowNodes(bundle, visible: true);
	}

	private void PlayBundle(AnimationBundle bundle, float time)
	{
		if (bundle.resetBeforePlay)
		{
			ResetAll();
		}
		PlayAnimation(bundle, time);
		PlayParticles(bundle, time);
		ShowNodes(bundle, visible: true);
	}

	private void ResetAll()
	{
		if (bundles == null)
		{
			return;
		}
		AnimationBundle[] array = bundles;
		foreach (AnimationBundle animationBundle in array)
		{
			if (animationBundle == null)
			{
				continue;
			}
			if (animationBundle.particles != null)
			{
				ParticleSystem[] particles = animationBundle.particles;
				foreach (ParticleSystem particleSystem in particles)
				{
					if (!(particleSystem == null))
					{
						particleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
					}
				}
			}
			if (animationBundle.shows == null)
			{
				continue;
			}
			GameObject[] shows = animationBundle.shows;
			foreach (GameObject gameObject in shows)
			{
				if (!(gameObject == null) && gameObject.activeSelf)
				{
					gameObject.SetActive(value: false);
				}
			}
		}
	}

	private void PlayAnimation(AnimationBundle bundle)
	{
		if (!(skeleton == null) && bundle != null && !string.IsNullOrEmpty(bundle.name))
		{
			skeleton.state?.SetAnimation(0, bundle.name, bundle.loop);
		}
	}

	private void PlayAnimation(AnimationBundle bundle, float time)
	{
		if (!(skeleton == null) && bundle != null && !string.IsNullOrEmpty(bundle.name))
		{
			TrackEntry trackEntry = skeleton.state?.SetAnimation(0, bundle.name, bundle.loop);
			if (trackEntry != null)
			{
				trackEntry.TrackTime = time;
			}
		}
	}

	private void PlayParticles(AnimationBundle bundle)
	{
		if (bundle == null || bundle.particles == null)
		{
			return;
		}
		ParticleSystem[] particles = bundle.particles;
		foreach (ParticleSystem particleSystem in particles)
		{
			if (!(particleSystem == null))
			{
				particleSystem.Play();
			}
		}
	}

	private void PlayParticles(AnimationBundle bundle, float time)
	{
		if (bundle == null || bundle.particles == null)
		{
			return;
		}
		ParticleSystem[] particles = bundle.particles;
		foreach (ParticleSystem particleSystem in particles)
		{
			if (!(particleSystem == null))
			{
				particleSystem.Simulate(time);
				particleSystem.Play();
			}
		}
	}

	private void ShowNodes(AnimationBundle bundle, bool visible)
	{
		if (bundle == null || bundle.shows == null)
		{
			return;
		}
		GameObject[] shows = bundle.shows;
		foreach (GameObject gameObject in shows)
		{
			if (!(gameObject == null) && gameObject.activeSelf != visible)
			{
				gameObject.SetActive(visible);
			}
		}
	}

	public void ScanAnimations()
	{
		if (skeleton == null)
		{
			skeleton = GetComponent<SkeletonAnimation>();
		}
		if (skeleton == null)
		{
			Debug.LogWarning("[SkeletonBundlePlayer] Missing SkeletonAnimation on " + base.gameObject.name);
			return;
		}
		try
		{
			skeleton.Initialize(overwrite: false);
		}
		catch
		{
		}
		SkeletonDataAsset skeletonDataAsset = skeleton.skeletonDataAsset;
		if (skeletonDataAsset == null)
		{
			Debug.LogWarning("[SkeletonBundlePlayer] skeletonDataAsset is null on " + base.gameObject.name);
			return;
		}
		SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(quiet: true);
		if (skeletonData == null)
		{
			Debug.LogWarning("[SkeletonBundlePlayer] Failed to get SkeletonData from asset on " + base.gameObject.name);
			return;
		}
		ExposedList<Spine.Animation> animations = skeletonData.Animations;
		if (animations == null || animations.Count == 0)
		{
			Debug.LogWarning("[SkeletonBundlePlayer] No animations found in SkeletonData for " + base.gameObject.name);
			return;
		}
		Dictionary<string, AnimationBundle> dictionary = new Dictionary<string, AnimationBundle>(StringComparer.Ordinal);
		if (bundles != null)
		{
			AnimationBundle[] array = bundles;
			foreach (AnimationBundle animationBundle in array)
			{
				if (animationBundle != null && !string.IsNullOrEmpty(animationBundle.name) && !dictionary.ContainsKey(animationBundle.name))
				{
					dictionary[animationBundle.name] = animationBundle;
				}
			}
		}
		List<AnimationBundle> list = new List<AnimationBundle>(animations.Count);
		int num = 0;
		int num2 = 0;
		foreach (Spine.Animation item2 in animations)
		{
			string text = item2?.Name;
			if (!string.IsNullOrEmpty(text))
			{
				if (dictionary.TryGetValue(text, out var value))
				{
					list.Add(value);
					num2++;
					continue;
				}
				AnimationBundle item = new AnimationBundle
				{
					name = text,
					loop = false,
					resetBeforePlay = true,
					particles = null
				};
				list.Add(item);
				num++;
			}
		}
		bundles = list.ToArray();
		Debug.Log(string.Format("[{0}] Scanned animations: total={1}, kept={2}, added={3} for {4}", "SkeletonBundlePlayer", bundles.Length, num2, num, base.gameObject.name));
	}

	private void AutoInit()
	{
		if (bundles == null || bundles.Length == 0)
		{
			ScanAnimations();
		}
	}
}
