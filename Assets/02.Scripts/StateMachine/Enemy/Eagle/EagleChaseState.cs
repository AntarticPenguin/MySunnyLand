using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EagleChaseState : State<EnemyController>
{
	#region Variables
	private Seeker _seeker;
	private Rigidbody2D _rigidbody2D;
	private Transform _transform;
	private EnemyControllerEagle _eagleController;

	//pathfinding variables
	private Path _path;
	private int _currentWaypoint = 0;
	private bool _reachEndOfPath;

	private float _calcDuration = 0.0f;
	private float _calcCooltime = 0.5f;
	#endregion

	public override void OnInitialized()
	{
		_seeker = _owner.GetComponent<Seeker>();
		_rigidbody2D = _owner.GetComponent<Rigidbody2D>();
		_transform = _owner.transform;
		_eagleController = _owner as EnemyControllerEagle;
	}

	public override void OnStart()
	{
		if (_eagleController.AttackTarget == null)
		{
			_stateMachine.ChangeState<EaglePatrolState>();
			return;
		}
			
		_seeker.StartPath(_transform.position, _eagleController.AttackTarget.position, OnPathComplete);
	}

	public override void Update(float deltaTime)
	{
		_calcDuration += deltaTime;
		if(_calcDuration >= _calcCooltime && _seeker.IsDone())
		{
			_calcDuration = 0.0f;
			_seeker.StartPath(_transform.position, _eagleController.AttackTarget.position, OnPathComplete);
		}
	}

	public override void FixedUpdate(float fixedDeltaTime)
	{
		if(_path == null)
		{
			return;
		}

		if(_currentWaypoint >= _path.vectorPath.Count)
		{
			_reachEndOfPath = true;
			Debug.Log("Arrived");
			return;
		}
		else
		{
			_reachEndOfPath = false;
		}

		Vector2 dir = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody2D.position).normalized;
		Vector2 force = dir * _owner.Speed * Time.fixedDeltaTime;
		_rigidbody2D.AddForce(force);

		float distance = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWaypoint]);
		if(distance < _eagleController.NextWaypointDistance)
		{
			_currentWaypoint++;
		}
	}

	public void OnPathComplete(Path p)
	{
		if(!p.error)
		{
			_path = p;
			_currentWaypoint = 0;
		}
	}
}
