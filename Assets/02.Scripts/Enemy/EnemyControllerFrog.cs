using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerFrog : EnemyController, IDamagable
{
	#region Variables
	[SerializeField] private float _jumpAngle = 40.0f;
	[SerializeField, Range(1, 10)] private int _jumpMinCooltime = 1;
	[SerializeField, Range(1, 10)] private int _jumpMaxCooltime = 4;

	private Collider2D _collider;
	private LayerMask _groundLayerMask;
	private Animator _animator;
	private int _animVerticalSpeed;
	private int _animGroundBool;
	#endregion

	#region Properties
	public float JumpAngle => _jumpAngle;
	public int JumpMinCoolTime => _jumpMinCooltime;
	public int JumpMaxCooltime => _jumpMaxCooltime;
	#endregion

	#region Unity Methods
	protected override void Awake()
	{
		base.Awake();

		_collider = GetComponent<Collider2D>();
		_groundLayerMask = LayerMask.GetMask(TagAndLayer.Layer.Ground) | LayerMask.GetMask(TagAndLayer.Layer.Platform);

		_animator = GetComponent<Animator>();
		_animVerticalSpeed = Animator.StringToHash("VerticalSpeed");
		_animGroundBool = Animator.StringToHash("Ground");

		IsActiveController = false;
	}

	protected override void FixedUpdate()
	{
		if (!IsActiveController)
			return;

		base.FixedUpdate();

		if(_collider.IsTouchingLayers(_groundLayerMask))
		{
			_animator.SetBool(_animGroundBool, true);
		}
		else
		{
			_animator.SetBool(_animGroundBool, false);
			_animator.SetFloat(_animVerticalSpeed, _rigidbody.velocity.y);
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
	#endregion

	#region Methods
	protected override void InitState()
	{
		_stateMachine = new StateMachine<EnemyController>(this, new FrogJumpState());
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
