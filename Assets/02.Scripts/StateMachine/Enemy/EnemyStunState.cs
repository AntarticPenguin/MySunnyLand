using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnemyStunState : State<EnemyController>
{
	#region Variables
	private GameObject _stunEffect;
	private Transform _stunEffectOffset;
	private float _stunDuration;
	private float _stunTime;
	#endregion

	public override void OnInitialized()
	{
		_stunEffectOffset = _owner._stunEffectOffset;
		_stunEffect = Object.Instantiate(_owner._enemyDataObject._stunEffectPrefab, _stunEffectOffset.position, Quaternion.identity);
		_stunEffect.transform.SetParent(_owner.transform);
		_stunEffect.SetActive(false);

		_stunTime = _owner._enemyDataObject.StunTime;
	}

	public override void OnStart()
	{
		_stunEffect.SetActive(true);
		_stunDuration = 0.0f;
	}

	public override void Update(float deltaTime)
	{
		_stunDuration += Time.deltaTime;
		if(_stunTime <= _stunDuration)
		{
			//이전 스테이트로 돌아가 동작을 계속 수행
			_stateMachine.ChangeToPreviousState();
		}
	}

	public override void OnExit()
	{
		_stunEffect.SetActive(false);
	}
}
