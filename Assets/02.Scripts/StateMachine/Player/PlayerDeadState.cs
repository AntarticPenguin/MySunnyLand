using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : State<PlayerController>
{
	#region Variables
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private int _animDeadTrigger;
	private AnimationCallback _animationCallback;

	#endregion

	public override void OnInitialized()
	{
		_rigidbody = _owner.GetComponent<Rigidbody2D>();
		_animator = _owner.GetComponent<Animator>();
		_animDeadTrigger = Animator.StringToHash(AnimatorKey.Dead);
		_animationCallback = _owner.GetComponent<AnimationCallback>();
	}

	public override void OnStart()
	{
		_animationCallback.InitEvent(null, () =>
		{
			if(_owner._playerData.Life == 0)
			{
				GameManager.Instance.GameOver();
				return;
			}

			Object.Destroy(_owner.gameObject);
			GameManager.Instance.RespawnPlayer();
		});

		_rigidbody.velocity = Vector2.zero;
		_rigidbody.bodyType = RigidbodyType2D.Kinematic;
		_animator.SetTrigger(_animDeadTrigger);
	}
}
