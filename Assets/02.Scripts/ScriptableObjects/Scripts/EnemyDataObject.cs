using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyDefaultData")]
public class EnemyDataObject : ScriptableObject
{
	#region Variables
	[SerializeField]
	private int _hp;

	[SerializeField]
	private float _speed;

	[SerializeField]
	private int _damage;
	#endregion

	#region Properties
	public int Hp => _hp;
	public float Speed => _speed;
	public int Damage => _damage;
	#endregion

	#region Helper Methods
	#endregion
}
