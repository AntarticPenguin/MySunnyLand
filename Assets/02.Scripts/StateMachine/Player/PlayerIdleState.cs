using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerController>
{
	#region
	private Animator _animator;
	private int _animIdleHash;
	#endregion

	public override void OnInitialized()
	{
		_animator = _owner.GetComponent<Animator>();
		_animIdleHash = Animator.StringToHash("Idle");
	}

	public override void OnStart()
	{
		_animator.Play(_animIdleHash);
	}

	public override void Update(float deltaTime)
	{
		if (!_owner.Movement.Equals(Vector2.zero))
		{
			_stateMachine.ChangeState<PlayerMoveState>();
		}
	}
}
