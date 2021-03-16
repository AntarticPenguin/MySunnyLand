using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
	#region Variables
	public GameObject _owner;
	public Vector2 _direction = Vector2.zero;

	private Rigidbody2D _rigidbody;
	private float _power;
	private int _damage;
	private float _duration;
	private bool _isCollided;

	private LayerMask _blockMask;

	#endregion

	#region Unity Methods
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_power = 4.0f;
		_damage = 1;
		_duration = 3.0f;
		_isCollided = false;

		_blockMask = LayerMask.GetMask(TagAndLayer.Layer.Ground) | LayerMask.GetMask(TagAndLayer.Layer.Platform);

		if (!_direction.Equals(Vector2.zero))
		{
			_rigidbody.velocity = _direction * _power;
		}
		else
		{
			_rigidbody.velocity = transform.right * _power;
		}

		Destroy(gameObject, _duration);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (_isCollided)
		{
			return;
		}

		if(((1 << collision.gameObject.layer) & _blockMask) != 0)
		{
			Destroy(gameObject);
			return;
		}

		if (collision.gameObject.tag == TagAndLayer.Tag.Enemy)
		{
			Collider2D projectileCollider = GetComponent<Collider2D>();
			projectileCollider.enabled = false;
			_isCollided = true;

			collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);

			Destroy(gameObject);
		}
	}
	#endregion
}
