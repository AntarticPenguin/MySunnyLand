using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class EnemyController : MonoBehaviour
{
	#region Variables
	[SerializeField] protected PlayerDataObject _playerDataObject;
	[SerializeField] private EnemyDataObject _enemyDataObject;
	public int _killPoint;
	public GameObject _deathEffectPrefab;

	protected StateMachine<EnemyController> _stateMachine;
	protected Rigidbody2D _rigidbody;
	protected Transform _transform;

	protected int _hp;
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

	// Update is called once per frame
	protected virtual void Update()
	{
		_stateMachine.Update(Time.deltaTime);

		if (_rigidbody.velocity.x <= -0.01f)
		{
			_transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (_rigidbody.velocity.x >= 0.01f)
		{
			_transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	protected virtual void FixedUpdate()
	{
		_stateMachine.FixedUpdate(Time.fixedDeltaTime);
	}
	#endregion

	#region Methods
	protected abstract void InitState();

	protected bool CheckJumpAttack(Collision2D collision)
	{
		float normalY = collision.GetContact(0).normal.y;
		if (-1.0f <= normalY && normalY <= -0.7f)
		{
			//플레이어를 위로 한번 바운스
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
			playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0.0f);
			playerRigidbody.AddForce(new Vector2(0, player.ReactionPower * 1.5f));
			return true;
		}

		return false;
	}
	#endregion
}
