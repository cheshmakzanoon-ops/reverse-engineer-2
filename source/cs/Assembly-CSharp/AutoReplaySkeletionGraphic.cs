using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class AutoReplaySkeletionGraphic : MonoBehaviour
{
	private List<SkeletonGraphic> _skeletonGraphics = new List<SkeletonGraphic>();

	private void Awake()
	{
		_skeletonGraphics.Clear();
		_skeletonGraphics.AddRange(GetComponentsInChildren<SkeletonGraphic>(includeInactive: true));
	}

	private void OnEnable()
	{
		foreach (SkeletonGraphic skeletonGraphic in _skeletonGraphics)
		{
			if (skeletonGraphic == null)
			{
				continue;
			}
			Skeleton skeleton = skeletonGraphic.Skeleton;
			Spine.AnimationState animationState = skeletonGraphic.AnimationState;
			if (skeleton != null && animationState != null)
			{
				ExposedList<Spine.Animation> animations = skeleton.Data.Animations;
				if (animations != null && animations.Count > 0)
				{
					animationState.ClearTracks();
					string animationName = ((!string.IsNullOrEmpty(skeletonGraphic.startingAnimation)) ? skeletonGraphic.startingAnimation : animations.Items[0].Name);
					animationState.SetAnimation(0, animationName, loop: true);
				}
			}
		}
	}
}
