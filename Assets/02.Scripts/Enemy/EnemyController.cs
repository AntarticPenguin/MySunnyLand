using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour, IDamagable
{
	#region Variables
	public float _healthCount;
	public PlayerDataObject _scoreObject;
	public int _killPoint;
	public GameObject _deathEffectPrefab;
	public float _detectRange = 1.5f;


	[SerializeField] private int _damage;
	private StateMachine<EnemyController> _stateMachine;
	private Transform _attackTarget;
	private Rigidbody2D _rigidbody;
	private Transform _transform;

	//pathfinding ai
	[Header("PATHFINDING AI")]
	[SerializeField] private float _speed = 100f;
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

	public bool IsAlive => (_healthCount > 0);
	public float Speed => (_speed);
	public float NextWaypointDistance => (_nextWaypointDistance);
	public float SlowDownDistance => (_slowDownDistance);
	public float EndReachedDistance => (_endReachedDistance);
	public int Damage => (_damage);
	#endregion

	#region Unity Methods
	// Start is called before the first frame update
	void Start()
	{
		_stateMachine = new StateMachine<EnemyController>(this, new PatrolState());
		_stateMachine.AddState(new ChaseState());
		_stateMachine.AddState(new AttackState());
		_stateMachine.AddState(new DeadState());

		_rigidbody = GetComponent<Rigidbody2D>();
		_transform = transform;
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

	#region IDamagable Interfaces
	public void TakeDamage(int damage)
	{
		_healthCount -= damage;
		Debug.Log("Take Damage. Remain HP: " + _healthCount);

		if (!IsAlive)
		{
			Debug.Log("ENEMY DIED");

			_healthCount = 0;
			_scoreObject.AddScore(_killPoint);
			_stateMachine.ChangeState<DeadState>();
		}
	}

	#endregion
}
