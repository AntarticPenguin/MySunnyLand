using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaAttackState : State<EnemyController>
{
	private Animator _animator;
	private AnimationCallback _animationCallback;
	private int _attackAnimHash;

	private EnemyControllerPiranhaPlant _piranhaController;
	private Transform _shootOffset;
	private float _duration;

	#region Methods
	public override void OnInitialized()
	{
		_animator = _owner.GetComponent<Animator>();
		_animationCallback = _owner.GetComponent<AnimationCallback>();
		_attackAnimHash = Animator.StringToHash("Attack");

		_piranhaController = _owner as EnemyControllerPiranhaPlant;
		_shootOffset = _piranhaController._shootOffset;
	}

	public override void OnStart()
	{
		_animator.SetTrigger(_attackAnimHash);
		_animationCallback.InitEvent(null, delegate 
		{
			Object.Instantiate(_piranhaController._bulletPrefab, _shootOffset.position, _shootOffset.rotation);
		});

		_duration = 0.0f;
	}

	public override void Update(float deltaTime)
	{
		_duration += deltaTime;
		if(_piranhaController.AttackCoolTime <= _duration)
		{
			_duration = 0.0f;
			_animator.SetTrigger(_attackAnimHash);
		}
	}
	#endregion
}
