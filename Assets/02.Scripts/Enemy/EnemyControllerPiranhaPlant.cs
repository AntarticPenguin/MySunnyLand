using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerPiranhaPlant : EnemyController, IDamagable
{
	#region Variables
	public GameObject _bulletPrefab;
	public Transform _shootOffset;

	[SerializeField]
	private float _detectRadius = 3.0f;

	[SerializeField]
	private float _attackCoolTime;

	private LayerMask _whoIsPlayer;
	private int _hurtAnimHash;
	private Vector2 _lookDirection;
	#endregion

	#region Properties
	public float AttackCoolTime => _attackCoolTime;
	#endregion

	#region Unity Methods
	protected override void Awake()
	{
		base.Awake();

		_hurtAnimHash = Animator.StringToHash("Hurt");
		_whoIsPlayer = LayerMask.GetMask(TagAndLayer.Layer.Player);
	}

	protected override void Update()
	{
		base.Update();

		if (_stateMachine.CurrentState.GetType() == typeof(EnemyStunState))
			return;

		Collider2D result = Physics2D.OverlapCircle(_transform.position, _detectRadius, _whoIsPlayer);
		if(result != null)
		{
			_lookDirection = result.gameObject.transform.position - _transform.position;
			UpdateDirection();

			_stateMachine.ChangeState<PiranhaAttackState>();
		}
		else
		{
			_stateMachine.ChangeState<EnemyIdleState>();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			if(CanBeDamagedByJumpAttack && CheckJumpAttack(collision))
			{
				TakeDamage(collision.gameObject.GetComponent<PlayerController>().JumpAttackDamage);
				return;
			}

			collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(Damage);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _detectRadius);
	}

	#endregion

	#region Methods
	protected override void InitState()
    {
		_stateMachine = new StateMachine<EnemyController>(this, new EnemyIdleState());
		_stateMachine.AddState(new PiranhaAttackState());
		_stateMachine.AddState(new EnemyStunState());
		_stateMachine.AddState(new EnemyDeadState());
    }

	protected override void UpdateDirection()
	{
		if (_lookDirection.x < 0.0f && _bFacingRight)
		{
			Flip();
		}
		else if(_lookDirection.x > 0.0f && !_bFacingRight)
		{
			Flip();
		}
	}

	#endregion

	#region IDamagable Interface
	public bool CanBeDamagedByJumpAttack => true;

	public void TakeDamage(int damage)
	{
		_hp -= damage;

		if (!IsAlive)
		{
			_hp = 0;
			_playerDataObject.AddScore(_enemyDataObject.KillPoint);
			_stateMachine.ChangeState<EnemyDeadState>();
			return;
		}

		_stateMachine.ChangeState<EnemyStunState>();
	}
	#endregion
}
