using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerEagle : EnemyController, IDamagable
{
	#region Variables
	public Transform[] _waypoints;
	public float _detectRange = 1.5f;

	private Transform _attackTarget;

	[Header("PATHFINDING AI")]
	[SerializeField] private float _nextWaypointDistance = 0.7f;
	[SerializeField] private float _slowDownDistance = 0.6f;
	[SerializeField] private float _endReachedDistance = 0.2f;
	#endregion

	#region Properties
	public float DetectRange => (_detectRange);
	public Transform AttackTarget
	{
		get { return _attackTarget; }
		set { _attackTarget = value; }
	}
	public float NextWaypointDistance => (_nextWaypointDistance);
	public float SlowDownDistance => (_slowDownDistance);
	public float EndReachedDistance => (_endReachedDistance);

	#endregion

	#region Unity Methods
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{ 
			if(CanBeDamagedByJumpAttack && CheckJumpAttack(collision))
			{
				TakeDamage(collision.gameObject.GetComponent<PlayerController>().JumpAttackDamage);
				return;
			}

			_attackTarget = collision.transform;
			_stateMachine.ChangeState<EagleAttackState>();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _detectRange);
	}
	#endregion

	#region Methods
	protected override void InitState()
	{
		_stateMachine = new StateMachine<EnemyController>(this, new EaglePatrolState());
		_stateMachine.AddState(new EagleChaseState());
		_stateMachine.AddState(new EagleAttackState());
		_stateMachine.AddState(new EnemyDeadState());
	}
	#endregion

	#region IDamagable Interfaces
	public bool CanBeDamagedByJumpAttack => true;

	public void TakeDamage(int damage)
	{
		_hp -= damage;

		if (!IsAlive)
		{
			_hp = 0;
			_playerDataObject.AddScore(_killPoint);
			_stateMachine.ChangeState<EnemyDeadState>();
		}
	}
	#endregion
}
