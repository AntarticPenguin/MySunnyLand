using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyDefaultData")]
public class EnemyDataObject : ScriptableObject
{
	#region Variables
	public GameObject _deathEffectPrefab;
	public GameObject _stunEffectPrefab;

	[SerializeField]
	private int _hp;

	[SerializeField]
	private float _speed;

	[SerializeField]
	private int _damage;

	[SerializeField]
	private int _killPoint;

	[SerializeField]
	private float _stunTime;

	//탄성. 플레이어가 점프공격을 했을 때, 몬스터의 탄성 정도에 따라 높이 튀어오름
	[SerializeField]
	private float _elasticity = 300.0f;
	#endregion

	#region Properties
	public int Hp => _hp;
	public float Speed => _speed;
	public int Damage => _damage;
	public int KillPoint => _killPoint;
	public float Elasticity => _elasticity;
	public float StunTime => _stunTime;
	#endregion

	#region Helper Methods
	#endregion
}
