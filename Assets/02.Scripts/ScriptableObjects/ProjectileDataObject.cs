using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileDataObject : ScriptableObject
{
	[SerializeField]
	private float _speed;

	[SerializeField]
	private int _damage;

	[SerializeField]
	private float _lifeTime;

	public float Speed => _speed;
	public int Damage => _damage;
	public float LifeTime => _lifeTime;
}
