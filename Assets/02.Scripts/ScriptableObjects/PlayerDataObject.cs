using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="PlayerData", menuName ="ScriptableObjects/PlayerData")]
public class PlayerDataObject : ScriptableObject, ISerializationCallbackReceiver
{
	#region Variables
	public int _initialLife;
	public int _initialHp;
	public int _initialMaxHp;
	public int _maxHpLimit;

	//Runtime Value
	[NonSerialized]
	private int _lifeRuntime;

	[NonSerialized]
	private int _hpRuntime;

	[NonSerialized]
	private int _currentMaxHpRunTime;

	[NonSerialized]
	private int _totalScoreRunTime;

	//event
	public event Action<int> OnChangedScore;
	public event Action<int> OnChangedLife;
	public event Action<int> OnChangedHp;
	#endregion

	#region Properties
	public int Life => _lifeRuntime;
	public int Hp => _hpRuntime;
	public int CurrentMaxHp => _currentMaxHpRunTime;
	public int MaxHpLimit => _maxHpLimit;
	public int TotalScore => _totalScoreRunTime;
	#endregion

	public void OnBeforeSerialize()
	{

	}

	public void OnAfterDeserialize()
	{
		_lifeRuntime = _initialLife;
		_hpRuntime = _initialHp;
		_currentMaxHpRunTime = (_initialMaxHp > _maxHpLimit) ? _maxHpLimit : _initialMaxHp;

		_totalScoreRunTime = 0;
	}

	public void AddScore(int score)
	{
		_totalScoreRunTime += score;
		OnChangedScore?.Invoke(_totalScoreRunTime);
	}

	public void AddLife(int amount)
	{
		_lifeRuntime += amount;
		OnChangedLife?.Invoke(_lifeRuntime);
	}

	public void DecreaseLife()
	{
		if (_lifeRuntime < 0)
		{
			_lifeRuntime = 0;
			return;
		}
		_lifeRuntime--;
		OnChangedLife?.Invoke(_lifeRuntime);
	}

	public void AddHp(int amount)
	{
		_hpRuntime += amount;
		if (_hpRuntime >= _currentMaxHpRunTime)
			_hpRuntime = _currentMaxHpRunTime;
		OnChangedHp?.Invoke(_hpRuntime);
	}

	public void DecreaseHp(int amount)
	{
		_hpRuntime -= amount;
		OnChangedHp?.Invoke(_hpRuntime);
	}

	public void ResetHp()
	{
		_hpRuntime = _currentMaxHpRunTime;
		OnChangedHp?.Invoke(_hpRuntime);
	}

	public void AddMaxHp(int amount)
	{
		_currentMaxHpRunTime += amount;
		if(_currentMaxHpRunTime >= _maxHpLimit)
			_currentMaxHpRunTime = _maxHpLimit;
	}
}
