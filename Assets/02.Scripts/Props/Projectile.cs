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
	#endregion

	#region Unity Methods
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_power = 4.0f;
		_damage = 1;
		_duration = 3.0f;
		_isCollided = false;

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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(_isCollided)
		{
			return;
		}

		if (collision.tag == TagAndLayer.Tag.Enemy)
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
