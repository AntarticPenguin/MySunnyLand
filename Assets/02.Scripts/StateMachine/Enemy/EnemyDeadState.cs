using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : State<EnemyController>
{
	#region Variables
	private SpriteRenderer _spriteRenderer;
	#endregion

	public override void OnInitialized()
	{
		_spriteRenderer = _owner.GetComponent<SpriteRenderer>();
	}

	public override void OnStart()
	{
		_spriteRenderer.enabled = false;

		GameObject effectPrefab = _owner._enemyDataObject._deathEffectPrefab;
		GameObject effect = Object.Instantiate(effectPrefab, _owner.transform.position, Quaternion.identity);
		effect.GetComponent<AnimationCallback>()?.InitEvent(null, delegate
		{
			Object.Destroy(effect);
		});

		Object.Destroy(_owner.gameObject);
	}
}
