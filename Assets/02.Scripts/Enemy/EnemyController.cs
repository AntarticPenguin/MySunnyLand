using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class EnemyController : MonoBehaviour
{
	#region Variables
	public EnemyDataObject _enemyDataObject;
	public Transform _stunEffectOffset;

	[SerializeField]
	protected PlayerDataObject _playerDataObject;

	protected StateMachine<EnemyController> _stateMachine;
	protected Rigidbody2D _rigidbody;
	protected Transform _transform;

	protected int _hp;
	protected bool _bFacingRight = true;

	private int _damage;
	private float _speed;
	

	private bool _isActiveController = true;
	#endregion

	#region Properties
	public bool IsAlive => (_hp > 0);
	public float Speed => (_speed);
	public int Damage => (_damage);

	public bool IsActiveController
	{
		get
		{
			return _isActiveController;
		}
		set
		{
			_isActiveController = value;
		}
	}
	#endregion

	#region Unity Methods
	protected virtual void Awake()
	{
		_hp = _enemyDataObject.Hp;
		_damage = _enemyDataObject.Damage;
		_speed = _enemyDataObject.Speed;

		_rigidbody = GetComponent<Rigidbody2D>();
		_transform = transform;

		InitState();
	}

	protected virtual void Start()
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(TagAndLayer.Layer.Enemy),
			LayerMask.NameToLayer(TagAndLayer.Layer.Enemy), true);
	}

	protected virtual void Update()
	{
		_stateMachine.Update(Time.deltaTime);
		UpdateDirection();
	}

	protected virtual void FixedUpdate()
	{
		_stateMachine.FixedUpdate(Time.fixedDeltaTime);
	}
	#endregion

	#region Methods
	protected abstract void InitState();

	protected virtual void UpdateDirection()
	{
		if (_rigidbody.velocity.x <= -0.01f && _bFacingRight)
		{
			Flip();
		}
		else if (_rigidbody.velocity.x >= 0.01f && !_bFacingRight)
		{
			Flip();
		}
	}

	protected bool CheckJumpAttack(Collision2D collision)
	{
		float normalY = collision.GetContact(0).normal.y;
		if (-1.0f <= normalY && normalY <= -0.65f)
		{
			//플레이어를 위로 한번 바운스
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			player.Bounce(_enemyDataObject.Elasticity);
			return true;
		}

		return false;
	}

	protected void Flip()
	{
		_bFacingRight = !_bFacingRight;
		_transform.Rotate(0f, 180f, 0f);
	}
	#endregion
}
