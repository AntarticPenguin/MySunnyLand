using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
	#region Variables
	public ProjectileDataObject _data;
	public Rigidbody2D _rigidbody;

	[SerializeField]
	private LayerMask _blockingMask;

	[SerializeField]
	private LayerMask _targetMask;
	#endregion

	#region Unity Methods
	void Start()
	{
		_rigidbody.velocity = transform.right * _data.Speed;
		Destroy(gameObject, _data.LifeTime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((_blockingMask & (1 << collision.gameObject.layer)) != 0)
		{
			if ((_targetMask & (1 << collision.gameObject.layer)) != 0)
			{
				collision.GetComponent<IDamagable>()?.TakeDamage(_data.Damage);
			}

			Destroy(gameObject);
		}
	}
	#endregion
}
