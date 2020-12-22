using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableObstacle : Obstacle, IAttackable
{
    #region Variables
    public int _damage;

	private bool _isColliding;
	private float _coolTime;
	#endregion

	#region Unity Methods
	private void Awake()
	{
		_isColliding = false;
		_coolTime = 3.0f;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (_isColliding)
			return;

		if(collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			_isColliding = true;
            collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
		}

		StartCoroutine(ResetCollision());
	}
	#endregion

	#region Methods
	private IEnumerator ResetCollision()
	{
		yield return new WaitForSeconds(_coolTime);
		_isColliding = false;
	}
	#endregion
}
