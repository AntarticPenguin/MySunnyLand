using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    bool CanBeDamagedByJumpAttack
	{
		get;
	}

    void TakeDamage(int damage);
}
