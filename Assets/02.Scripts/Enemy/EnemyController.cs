using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
	#region Variables
	public float _healthCount;
	public int _killPoint;
	public GameObject _deathEffectPrefab;

	private StateMachine<EnemyController> _stateMachine;
	#endregion

	#region Properties
	public bool IsAlive => (_healthCount > 0);
	#endregion

	#region Unity Methods
	// Start is called before the first frame update
	void Start()
	{
		_stateMachine = new StateMachine<EnemyController>(this, new PatrolState());
		_stateMachine.AddState(new ChaseState());
		_stateMachine.AddState(new AttackState());
		_stateMachine.AddState(new DeadState());
	}

	// Update is called once per frame
	void Update()
	{
		_stateMachine.Update(Time.deltaTime);
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
			GameManager.Instance.AddScore(_killPoint);
			_stateMachine.ChangeState<DeadState>();
		}
	}

	#endregion
}
