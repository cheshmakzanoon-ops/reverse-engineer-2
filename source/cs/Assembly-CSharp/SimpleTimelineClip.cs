using System;

[Serializable]
public abstract class SimpleTimelineClip
{
	public double start;

	public double duration;

	public double timeScale;

	public double end => start + duration;

	public abstract ESimpleTimelineClipType ClipType { get; }

	public abstract bool OnBehaviourPlay(SimpleTimelineFrameTime frame);

	public abstract void OnBehaviourPause();

	public abstract void OnBehaviourDestroy();
}
