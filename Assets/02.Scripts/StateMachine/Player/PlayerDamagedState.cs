using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : State<PlayerController>
{
	#region
	private Animator _animator;
	private int _animHurtTrigger;

	private Rigidbody2D _rigidbody;
	private AnimationCallback _animationCallback;
	#endregion

	public override void OnInitialized()
	{ 
		_animator = _owner.GetComponent<Animator>();
		_animHurtTrigger = Animator.StringToHash(AnimatorKey.Hurt);

		_rigidbody = _owner.GetComponent<Rigidbody2D>();
		_animationCallback = _owner.GetComponent<AnimationCallback>();
	}

	public override void OnStart()
	{
		Debug.Log("Damaged State");

		_animator.SetTrigger(_animHurtTrigger);
		_animationCallback.InitEvent(null, () =>
		{
			Debug.Log("ANimatino END");
			_stateMachine.ChangeState<PlayerIdleState>();
		});

		//add force to reverse direction
		Vector2 reverseVec = Vector2.zero;
		reverseVec.x = _owner.IsFlipped ? 1.0f : -1.0f;
		reverseVec.y = 5.0f;
		_rigidbody.velocity = Vector2.zero;
		_rigidbody.AddForce(reverseVec * 20);

		//makes player undamaged for a certain period of time
	}
}
