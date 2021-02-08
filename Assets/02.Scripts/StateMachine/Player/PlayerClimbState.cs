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
	private float _originalGravity;

	ContactFilter2D _groundFilter;
	List<Collider2D> _groundFilterResult = new List<Collider2D>();

	ContactFilter2D _cliffFilter;
	List<Collider2D> _cliffFilterResult = new List<Collider2D>();
	#endregion	

	public override void OnInitialized()
	{
		_rigidbody = _owner.GetComponent<Rigidbody2D>();
		_animator = _owner.GetComponent<Animator>();
		_animClimbSpeed = Animator.StringToHash(AnimatorKey.ClimbSpeed);
		_animClimbBool = Animator.StringToHash(AnimatorKey.Climb);
		_climbSpeed = _owner.ClimbSpeed;

		_groundFilter.SetLayerMask(LayerMask.GetMask(TagAndLayer.Layer.Ground));
		_cliffFilter.SetLayerMask(LayerMask.GetMask(TagAndLayer.Layer.Cliff));
		_cliffFilter.useTriggers = true;
	}

	public override void OnStart()
	{
		UtilMethods.IgnoreLayerCollisionByName(TagAndLayer.Layer.Player,
			TagAndLayer.Layer.Ground);

		_originalGravity = _rigidbody.gravityScale;
		_rigidbody.gravityScale = 0.0f;

		_owner.IsClimbing = true;
		_animator.SetBool(_animClimbBool, true);
	}

	public override void Update(float deltaTime)
	{
		_animator.SetFloat(_animClimbSpeed, Mathf.Clamp(_rigidbody.velocity.magnitude, 0, 1));

		//위로 등반 중 groundTile에 닿아있는지 체크
		int overlapGround = _rigidbody.OverlapCollider(_groundFilter, _groundFilterResult);
		int overlapCliff = _rigidbody.OverlapCollider(_cliffFilter, _cliffFilterResult);

		if (_owner.Movement.y <= -1.0f && _owner.IsGrounded)
		{
			//내려가다 땅에 닿았을 경우
			_stateMachine.ChangeState<PlayerIdleState>();
		}
		else if (_owner.Movement.y >= 1.0f && overlapGround == 0 && _owner.IsGrounded)
		{
			//올라가다 땅에 닿았을 경우
			_stateMachine.ChangeState<PlayerIdleState>();
		}
		else if(overlapCliff == 0 && overlapGround == 0)
		{
			//좌우로 움직이다가 절벽을 벗어났을 경우
			_stateMachine.ChangeState<PlayerIdleState>();
		}
	}

	public override void FixedUpdate(float fixedDeltaTime)
	{
		_rigidbody.velocity = new Vector2(_owner.Movement.x * _climbSpeed, _owner.Movement.y * _climbSpeed);
	}

	public override void OnExit()
	{
		UtilMethods.IgnoreLayerCollisionByName(TagAndLayer.Layer.Player,
			TagAndLayer.Layer.Ground, false);

		_owner.IsClimbing = false;
		_rigidbody.gravityScale = _originalGravity;
		_rigidbody.velocity = Vector2.zero;
		_animator.SetBool(_animClimbBool, false);
	}
}
