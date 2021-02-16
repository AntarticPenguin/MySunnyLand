using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour, IDamagable
{
	#region Variables
	public PlayerDataObject _playerDataObject;
	public EnemyDataObject _enemyDataObject;
	public int _killPoint;
	public GameObject _deathEffectPrefab;
	public float _detectRange = 1.5f;

	private StateMachine<EnemyController> _stateMachine;
	private Transform _attackTarget;
	private Rigidbody2D _rigidbody;
	private Transform _transform;

	private int _hp;
	private int _damage;
	private float _speed;

	//pathfinding ai
	[Header("PATHFINDING AI")]
	[SerializeField] private float _nextWaypointDistance = 0.7f;
	[SerializeField] private float _slowDownDistance = 0.6f;
	[SerializeField] private float _endReachedDistance = 0.2f;

	public Transform[] _waypoints;
	#endregion

	#region Properties
	public float DetectRange => (_detectRange);
	public Transform AttackTarget
	{
		get { return _attackTarget; }
		set { _attackTarget = value; }
	}

	public bool IsAlive => (_hp > 0);
	public float Speed => (_speed);
	public float NextWaypointDistance => (_nextWaypointDistance);
	public float SlowDownDistance => (_slowDownDistance);
	public float EndReachedDistance => (_endReachedDistance);
	public int Damage => (_damage);
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

	// Update is called once per frame
	void Update()
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

	private void FixedUpdate()
	{
		_stateMachine.FixedUpdate(Time.fixedDeltaTime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			_attackTarget = collision.transform;
			_stateMachine.ChangeState<AttackState>();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _detectRange);
	}

	#endregion

	#region Methods
	protected virtual void InitState()
	{
		Debug.Log("Test Init State");
		_stateMachine = new StateMachine<EnemyController>(this, new PatrolState());
		_stateMachine.AddState(new ChaseState());
		_stateMachine.AddState(new AttackState());
		_stateMachine.AddState(new DeadState());
	}
	#endregion

	#region IDamagable Interfaces
	public void TakeDamage(int damage)
	{
		_hp -= damage;

		if (!IsAlive)
		{
			_hp = 0;
			_playerDataObject.AddScore(_killPoint);
			_stateMachine.ChangeState<DeadState>();
		}
	}

	#endregion
}
