using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
	public override void OnInitialized()
	{
		
	}

	public override void OnStart()
	{
		_owner.AttackTarget.GetComponent<IDamagable>()?.TakeDamage(_owner.Damage);
		_stateMachine.ChangeState<ChaseState>();
	}
}
