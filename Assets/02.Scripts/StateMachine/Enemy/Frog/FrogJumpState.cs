using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogJumpState : State<EnemyController>
{
	#region Variables
	private Rigidbody2D _rigidbody;
	private float _duration = 0.0f;
	private float _power;
	private EnemyControllerFrog _frogController;
	private Vector2 _angleVector = Vector2.zero;
	private float _changeDirection;
	#endregion

	public override void OnInitialized()
	{
		_frogController = _owner as EnemyControllerFrog;
		_rigidbody = _owner.GetComponent<Rigidbody2D>();
		_power = _owner.Speed;
		_angleVector = UtilMethods.DegreeToVector2(_frogController.JumpAngle);
	}

	public override void OnStart()
	{
		//first init direction
		_changeDirection = Random.value > 0.5f ? 1.0f : -1.0f;
	}

	public override void FixedUpdate(float fixedDeltaTime)
	{
		_duration += fixedDeltaTime;
		if (_duration >= Random.Range(_frogController.JumpMinCoolTime, _frogController.JumpMaxCooltime))
		{
			_duration = 0.0f;
			_angleVector.x *= _changeDirection;
			_rigidbody.AddForce(_angleVector * _power, ForceMode2D.Impulse);

			//set next direction
			_changeDirection = Random.value > 0.5f ? 1.0f : -1.0f;
		}
	}

	public override void OnExit()
	{
		
	}
}
