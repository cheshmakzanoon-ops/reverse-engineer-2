using UnityEngine;

public class DominatorCockatriceUnlockTimelineAudioController : MonoBehaviour
{
	[SerializeField]
	private int _soundId;

	public void Awake()
	{
		if (_soundId > 0)
		{
			GameEntry.Sound.PlayTimeline(_soundId);
		}
	}
}
