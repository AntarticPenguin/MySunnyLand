using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableObstacle : Obstacle, IAttackable
{
    #region Variables
    public int _damage;

	#endregion

	#region Unity Methods
	private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
            collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
		}
	}
	#endregion

	#region Methods
	#endregion
}
