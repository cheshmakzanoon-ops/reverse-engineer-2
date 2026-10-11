using System;
using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class UIGGGoPlayerController : MonoBehaviour
{
	public SimpleAnimation Animator;

	public Transform UIPos;

	public HitColorOrEmission HitEffect;

	private EcsWorld _world;

	private GGGoEnvClient _env;

	[NonSerialized]
	public EcsPackedEntityWithWorld Entity;

	private bool _isGrounded = true;

	private int _moveState;

	private AnimationPriority _currentAnimationPriority = AnimationPriority.Idle;

	private string _currentAnimationName = "";

	private const float CROSSFADE_DURATION = 0.1f;

	private const float FALL_ROTATION_ANGLE = 15f;

	private const float ROTATION_DURATION = 0.2f;

	private const float LAND_SCALE_MIN = 0.8f;

	private const float LAND_SCALE_BOUNCE = 1.1f;

	private bool _wasInAir;

	public EPlayerID PlayerID { get; private set; }

	public Camera Camera => _env?.Scene?.Camera;

	protected virtual void Start()
	{
		if (Animator == null)
		{
			Animator = GetComponentInChildren<SimpleAnimation>();
		}
		if (HitEffect == null)
		{
			HitEffect = GetComponent<HitColorOrEmission>();
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		if (Entity.World.IsAlive())
		{
			DataUIRender.UIPlayerUnbind uIPlayerUnbind = DataUIRender.UIRender<DataUIRender.UIPlayerUnbind>.Fetch();
			uIPlayerUnbind.Controller = this;
			FuncUI.FireRender(Entity.World, uIPlayerUnbind);
		}
	}

	private void OnEnable()
	{
		PlayAnimationWithPriority("Default", AnimationPriority.Idle);
	}

	private void Update()
	{
		UpdateGroundState();
	}

	public void Init(EcsPackedEntityWithWorld entity)
	{
		Entity = entity;
		_world = entity.World;
		_env = _world.GetShared<GGGoEnvClient>();
		PlayerID = _world.GetPool<ComponentPlayer>().Get(entity.Id).PlayerID;
	}

	public void Die()
	{
		if (PlayAnimationWithPriority("dead", AnimationPriority.Special))
		{
			PlaySound((PlayerID == EPlayerID.ID_1P) ? 6100035 : 6100034, force: true);
		}
	}

	public void Win()
	{
		if (PlayAnimationWithPriority("win", AnimationPriority.Special))
		{
			PlaySound(6100038);
		}
	}

	public void UseItem(ItemType itemType)
	{
		PlaySound(5100014);
	}

	public void Hurt(float hurtValue, float protectTime = 0f)
	{
		PlaySound((PlayerID == EPlayerID.ID_1P) ? 6100033 : 6100032);
		if (HitEffect != null)
		{
			HitEffect.Play(protectTime);
		}
	}

	public void PlaySound(int soundId, bool force = false)
	{
		if (_env != null && !_env.IsRollbacking && (force || PlayerID == _env.GetPlayerID()))
		{
			DataUISound.PlayerSound(soundId);
		}
	}

	public void OnGroundChange(bool isGrounded)
	{
		bool num = !_isGrounded && isGrounded && _wasInAir;
		_isGrounded = isGrounded;
		if (num)
		{
			PlayLandingEffect();
			_wasInAir = false;
		}
		if (!isGrounded)
		{
			_wasInAir = true;
		}
		UpdateMovementAnimation();
	}

	public void Move(int moveState)
	{
		_moveState = moveState;
		UpdateMovementAnimation();
	}

	private void UpdateGroundState()
	{
		if (_world == null || _env == null || !Entity.World.IsAlive() || !FuncGame.IsPlaying(_world))
		{
			return;
		}
		EcsPool<ComponentRigidBody> pool = _world.GetPool<ComponentRigidBody>();
		if (pool.Has(Entity.Id))
		{
			bool isGrounded = pool.Get(Entity.Id).IsGrounded;
			if (isGrounded != _isGrounded)
			{
				OnGroundChange(isGrounded);
			}
		}
	}

	private void UpdateMovementAnimation()
	{
		if (!_isGrounded)
		{
			PlayAnimationWithPriority("fall", AnimationPriority.Fall);
			if (_moveState == 1)
			{
				RotateAnimator(15f);
			}
			else if (_moveState == 2)
			{
				RotateAnimator(-15f);
			}
			else
			{
				RotateAnimator(0f);
			}
			return;
		}
		RotateAnimator(0f);
		if (_moveState == 0)
		{
			PlayAnimationWithPriority("Default", AnimationPriority.Idle);
		}
		else if (_moveState == 1)
		{
			PlayAnimationWithPriority("run_right", AnimationPriority.Move);
		}
		else if (_moveState == 2)
		{
			PlayAnimationWithPriority("run_left", AnimationPriority.Move);
		}
	}

	private bool PlayAnimationWithPriority(string animName, AnimationPriority priority)
	{
		if ((priority >= _currentAnimationPriority || _currentAnimationName != animName) && Animator != null && !string.IsNullOrEmpty(animName) && _currentAnimationName != animName)
		{
			Animator.CrossFade(animName, 0.1f);
			_currentAnimationName = animName;
			_currentAnimationPriority = priority;
			return true;
		}
		return false;
	}

	public bool PlayAnimation(string animName)
	{
		return PlayAnimationWithPriority(animName, AnimationPriority.Special);
	}

	private void PlayLandingEffect()
	{
		if (Animator != null)
		{
			Transform target = Animator.transform;
			target.DOKill();
			Sequence s = DOTween.Sequence();
			s.Append(target.DOScale(new Vector3(1.2f, 0.8f, 1f), 0.15f).SetEase(Ease.OutQuad));
			s.Append(target.DOScale(new Vector3(0.9f, 1.1f, 1f), 0.25f).SetEase(Ease.OutBounce));
			s.Append(target.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad));
		}
	}

	private void RotateAnimator(float targetAngle)
	{
		if (Animator != null)
		{
			Animator.transform.DORotateQuaternion(Quaternion.Euler(0f, targetAngle, targetAngle * 0.5f), 0.2f).SetEase(Ease.OutQuad);
		}
	}
}
