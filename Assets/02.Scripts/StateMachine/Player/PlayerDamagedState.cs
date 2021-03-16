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
		_animationCallback.InitEvent(null, () =>
		{
			//Debug.Log("ANimatino END");
			_stateMachine.ChangeState<PlayerIdleState>();
		});

		_animator.SetTrigger(_animHurtTrigger);

		Vector2 reverseVec = UtilMethods.DegreeToVector2(60.0f);
		reverseVec.x *= _owner.IsFlipped ? 1.0f : -1.0f;
		reverseVec.y *= 1.5f;
		_rigidbody.velocity = Vector2.zero;
		_rigidbody.AddForce(reverseVec * _owner.ReactionPower);
	}

	public override void OnExit()
	{
		
	}
}
