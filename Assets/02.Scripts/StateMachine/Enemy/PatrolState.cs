using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State<EnemyController>
{
	#region Variables
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private int _layerMask;

	//waypoint
	private int _waypointIndex;
	private float _stoppingDistance;
	private float _waitTime;
	private float _calcWaitTime;
	private float _slowdownDistance;
	private Transform _targetWaypoint;
	#endregion

	#region Properties
	public Transform[] Waypoints => _owner._waypoints;
	#endregion

	public override void OnInitialized()
	{
		_transform = _owner.transform;
		_rigidbody = _owner.GetComponent<Rigidbody2D>();
		_layerMask = LayerMask.GetMask(TagAndLayer.Layer.Player);

		_waypointIndex = 1;				//object start at index 0
		_stoppingDistance = 0.025f;
		_waitTime = 3.0f;
		_calcWaitTime = 0.0f;
		_slowdownDistance = 0.8f;
	}

	public override void OnStart()
	{
		_calcWaitTime = 0.0f;
		_targetWaypoint = FindNextWaypoint();
	}

	public override void Update(float deltaTime)
	{
		Collider2D collider = Physics2D.OverlapCircle(_transform.position, _owner.DetectRange, _layerMask);
		if(collider != null)
		{
			_owner.AttackTarget = collider.transform;
			_stateMachine.ChangeState<ChaseState>();
		}
	}

	public override void FixedUpdate(float fixedDeltaTime)
	{
		if(_targetWaypoint != null)
		{
			Vector2 direction = (_targetWaypoint.position - _transform.position).normalized;

			float remainDistance = Vector2.Distance(_transform.position, _targetWaypoint.position);
			if(remainDistance <= _slowdownDistance)
			{
				_rigidbody.velocity *= (1 - fixedDeltaTime * _rigidbody.drag);
			}
			else
			{
				_rigidbody.velocity = direction * _owner.Speed * fixedDeltaTime;
			}

			if (remainDistance <= _stoppingDistance)
			{
				_rigidbody.velocity = Vector2.zero;
				_calcWaitTime += fixedDeltaTime;
				if(_calcWaitTime >= _waitTime)
				{
					_targetWaypoint = FindNextWaypoint();
					_calcWaitTime = 0.0f;
				}
			}
		}
	}

	#region Helper Methods
	private Transform FindNextWaypoint()
	{
		_targetWaypoint = null;
		
		if(Waypoints.Length > 0)
		{
			_targetWaypoint = Waypoints[_waypointIndex];
			_waypointIndex = (_waypointIndex + 1) % Waypoints.Length;
		}

		return _targetWaypoint;
	}
	#endregion
}
