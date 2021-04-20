using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State<PlayerController>
{
	#region Variables
	private Rigidbody2D _rigidbody;
	private float _speed;

	private Animator _animator;
	private int _animSpeedFloat;
	#endregion
	public override void OnInitialized()
	{
		_rigidbody = _owner.GetComponent<Rigidbody2D>();
		_animator = _owner.GetComponent<Animator>();
		_animSpeedFloat = Animator.StringToHash(AnimatorKey.Speed); ;
		_speed = _owner.Speed;
	}

	public override void OnStart()
	{

	}

	public override void Update(float deltaTime)
	{
		if (_owner.IsGrounded)
		{
			_animator.SetFloat(_animSpeedFloat, Mathf.Abs(_rigidbody.velocity.x));
		}
	}

	public override void FixedUpdate(float fixedDeltaTime)
	{
		Vector2 movement = _owner.Movement;
		Vector2 newVelocity = _rigidbody.velocity;

		if (movement.Equals(Vector2.zero) && _rigidbody.velocity.magnitude > 0.0f)
		{
			newVelocity.x = 0;
			_rigidbody.velocity = newVelocity;
			_animator.SetFloat(_animSpeedFloat, 0);

			if (_owner.IsGrounded && _rigidbody.velocity.Equals(Vector2.zero))
			{
				_stateMachine.ChangeState<PlayerIdleState>();
				return;
			}

			return;
		}

		if (_owner.IsGrounded && !_owner.IsOnSlope)
		{
			newVelocity.Set(movement.x * _speed, _rigidbody.velocity.y);
		}
		else if (_owner.IsGrounded && _owner.IsOnSlope && !_owner.IsJumping)
		{
			newVelocity.Set(_speed * _owner.SlopeNormalPerp.x * -movement.x, _speed * _owner.SlopeNormalPerp.y * -movement.x);
		}

		if (_owner.IsJumping)
		{
			if (_rigidbody.velocity.x > 0 && movement.x < 0)
			{
				newVelocity.x -= _speed * fixedDeltaTime;
			}
			else if (_rigidbody.velocity.x < 0 && movement.x > 0)
			{
				newVelocity.x += _speed * fixedDeltaTime;
			}
			else if((int)_rigidbody.velocity.x == 0 && movement.x != 0)		//Jump in place with movement
			{
				newVelocity.x += _speed * fixedDeltaTime * movement.x * 2;
			}
		}

		_rigidbody.velocity = newVelocity;
	}
}
