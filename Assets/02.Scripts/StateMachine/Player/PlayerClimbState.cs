using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : State<PlayerController>
{
	#region
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private int _animClimbSpeed;
	private int _animClimbBool;
	private float _climbSpeed;

	ContactFilter2D filter;
	List<Collider2D> results = new List<Collider2D>();
	#endregion

	public override void OnInitialized()
	{
		_rigidbody = _owner.GetComponent<Rigidbody2D>();
		_animator = _owner.GetComponent<Animator>();
		_animClimbSpeed = Animator.StringToHash(AnimatorKey.ClimbSpeed);
		_animClimbBool = Animator.StringToHash(AnimatorKey.Climb);
		_climbSpeed = _owner.ClimbSpeed;

		filter.SetLayerMask(LayerMask.GetMask(TagAndLayer.Layer.Ground));
	}

	public override void OnStart()
	{
		Debug.Log("Climb State");
		Physics2D.IgnoreLayerCollision(
			LayerMask.NameToLayer(TagAndLayer.Layer.Player),
			LayerMask.NameToLayer(TagAndLayer.Layer.Ground)
		);

		_owner.IsClimbing = true;
		_animator.SetBool(_animClimbBool, true);
		_rigidbody.gravityScale = 0.0f;
	}

	public override void Update(float deltaTime)
	{
		_animator.SetFloat(_animClimbSpeed, Mathf.Clamp(_rigidbody.velocity.magnitude, 0, 1));

		int count = _rigidbody.OverlapCollider(filter, results);
		if (count > 0)
			Debug.Log("is touching");
		else if (count == 0)
			Debug.Log("nothing");

		if (_owner.Movement.y <= -1.0f && _owner.IsGrounded)
		{
			//절벽을 타고 내려와 땅에 닿았을 경우
			_stateMachine.ChangeState<PlayerIdleState>();
		}
		else if(_owner.Movement.y >= 1.0f && count == 0 && _owner.IsGrounded)
		{

			_stateMachine.ChangeState<PlayerIdleState>();
		}
	}

	public override void FixedUpdate(float fixedDeltaTime)
	{
		_rigidbody.velocity = new Vector2(_owner.Movement.x * _climbSpeed, _owner.Movement.y * _climbSpeed);
	}

	public override void OnExit()
	{
		Physics2D.IgnoreLayerCollision(
			LayerMask.NameToLayer(TagAndLayer.Layer.Player),
			LayerMask.NameToLayer(TagAndLayer.Layer.Ground),
			false
		);

		_owner.IsClimbing = false;
		_rigidbody.gravityScale = 1.0f;
		_rigidbody.velocity = Vector2.zero;
		_animator.SetBool(_animClimbBool, false);
	}
}
