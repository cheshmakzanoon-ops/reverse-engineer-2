using UnityEngine;

namespace MiniGame.GGGo.Client;

public class HitColorOrEmission : MonoBehaviour
{
	public Renderer TargetRenderer;

	public Color HitColor = Color.red;

	public Color AltColor = Color.white;

	public float Duration = 0.5f;

	public float Interval = 0.2f;

	public HitColorChannel ColorChannel;

	private MPBController _mpb;

	private float _timer;

	private bool _playing;

	private float _blinkElapsed;

	private void Awake()
	{
		if (TargetRenderer == null)
		{
			TargetRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
		}
		if (TargetRenderer == null)
		{
			Debug.LogError("HitColorOrEmission requires a Renderer component assigned to targetRenderer.");
			base.enabled = false;
			return;
		}
		_mpb = new MPBController(TargetRenderer);
		switch (ColorChannel)
		{
		case HitColorChannel.ForceEmission:
			_mpb.ForceEmission = true;
			break;
		case HitColorChannel.ForceColor:
			_mpb.ForceEmission = false;
			break;
		case HitColorChannel.Both:
			_mpb.ForceEmission = null;
			break;
		default:
			_mpb.ForceEmission = null;
			break;
		}
	}

	private void Update()
	{
		if (_playing)
		{
			float deltaTime = Time.deltaTime;
			_timer -= deltaTime;
			_blinkElapsed += deltaTime;
			float num = Mathf.Max(Interval, 1E-05f);
			float t = Mathf.PingPong(_blinkElapsed, num) / num;
			Color color = Color.Lerp(HitColor, AltColor, t);
			if (ColorChannel == HitColorChannel.Both)
			{
				_mpb.SetBothColors(color);
			}
			else
			{
				_mpb.SetColor(color);
			}
			if (_timer <= 0f)
			{
				Stop();
			}
		}
	}

	public void Play(float duration = 0f)
	{
		float num = ((duration > 0f) ? duration : Duration);
		if (num <= 0f)
		{
			_mpb.SetColor(HitColor);
			Stop();
		}
		else if (_playing)
		{
			_timer = Mathf.Max(_timer, num);
		}
		else
		{
			_timer = num;
			_playing = true;
			_blinkElapsed = 0f;
		}
	}

	public void Stop()
	{
		_playing = false;
		_mpb.Clear();
	}
}
