using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State<EnemyController>
{
	#region Variables
	private Transform _transform;
	private int _layerMask;
	#endregion
	public override void OnInitialized()
	{
		_transform = _owner.transform;
		_layerMask = LayerMask.GetMask(TagAndLayer.Layer.Player);
	}

	public override void OnStart()
	{
		
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
}
