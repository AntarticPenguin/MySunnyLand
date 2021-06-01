using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleAttackState : State<EnemyController>
{
	#region Variables
	private EnemyControllerEagle _eagleController;
	#endregion

	public override void OnInitialized()
	{
		_eagleController = _owner as EnemyControllerEagle;
	}

	public override void OnStart()
	{
		_eagleController.AttackTarget.GetComponent<IDamagable>()?.TakeDamage(_owner.Damage);
		_stateMachine.ChangeState<EagleChaseState>();
	}
}
