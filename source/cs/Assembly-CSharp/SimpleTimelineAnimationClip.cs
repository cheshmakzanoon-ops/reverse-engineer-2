using System;

[Serializable]
public class SimpleTimelineAnimationClip : SimpleTimelineClip
{
	public SimpleAnimation animation;

	public string state;

	public override ESimpleTimelineClipType ClipType => ESimpleTimelineClipType.AnimationClip;

	public override bool OnBehaviourPlay(SimpleTimelineFrameTime frame)
	{
		if (animation == null)
		{
			return true;
		}
		if (!animation.IsInitialized || !animation.isActiveAndEnabled)
		{
			return false;
		}
		SimpleAnimation.State state = animation.GetState(this.state);
		if (state == null)
		{
			return true;
		}
		if (frame.cross > 0f)
		{
			animation.CrossFade(this.state, frame.cross);
		}
		else
		{
			animation.RewindAndPlay(this.state);
		}
		animation.SetStateSpeed(this.state, (float)timeScale);
		float length = state.length;
		animation.SampleAnimationAtTime(this.state, frame.local / length);
		return true;
	}

	public override void OnBehaviourPause()
	{
		animation?.SetStateSpeed(state, 0f);
	}

	public override void OnBehaviourDestroy()
	{
		animation?.SetStateSpeed(state, 0f);
	}
}
