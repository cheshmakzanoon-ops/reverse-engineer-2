using UnityEngine;

public class WorldCityVfxAsync : AsyncMono<WorldCityVfxAsync>
{
	public ParticleSystem mainParticle;

	public void PlayParticle(float sec)
	{
		if (!(mainParticle == null))
		{
			mainParticle.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
			mainParticle.time = sec;
			mainParticle.Play(withChildren: true);
		}
	}
}
