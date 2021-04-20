using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="PlayerData", menuName ="ScriptableObjects/PlayerData")]
public class PlayerDataObject : ScriptableObject
{
	#region Variables
	private const int MAX_HP_LIMIT = 5;

	private int _life;
	private int _hp;
	private int _currentMaxHp;			//현재 maxHp
	private int _maxHpLimit;            //상한 Hp

	private int _totalScore;
	[NonSerialized] private bool _isInitialized = false;

	//event
	public event Action<int> OnChangedScore;
	public event Action<int> OnChangedLife;
	public event Action<int> OnChangedHp;
	#endregion

	#region Properties
	public int Life => _life;
	public int Hp => _hp;
	public int CurrentMaxHp => _currentMaxHp;
	public int MaxHpLimit => _maxHpLimit;
	public int TotalScore => _totalScore;
	#endregion

	private void OnEnable()
	{
		if (_isInitialized)
			return;

		_isInitialized = true;
		_totalScore = 0;
	}

	/// <summary>
	/// Initialized by Player Class
	/// </summary>
	public void Initialize(int life, int hp)
	{
		Debug.Log("Init player data");
		_maxHpLimit = MAX_HP_LIMIT;
		_currentMaxHp = (hp >= MaxHpLimit) ? MaxHpLimit : hp;
		_hp = CurrentMaxHp;

		_life = life;

		OnChangedHp?.Invoke(_hp);
		OnChangedLife?.Invoke(_life);
	}

	public void AddScore(int score)
	{
		_totalScore += score;
		OnChangedScore?.Invoke(_totalScore);
	}

	public void AddLife(int amount)
	{
		_life += amount;
		OnChangedLife?.Invoke(_life);
	}

	public void DecreaseLife()
	{
		if (_life < 0)
		{
			_life = 0;
			return;
		}
		_life--;
		OnChangedLife?.Invoke(_life);
	}

	public void AddHp(int amount)
	{
		_hp += amount;
		if (_hp >= _currentMaxHp)
			_hp = _currentMaxHp;
		OnChangedHp?.Invoke(_hp);
	}

	public void DecreaseHp(int amount)
	{
		_hp -= amount;
		OnChangedHp?.Invoke(_hp);
	}

	public void ResetHp()
	{
		_hp = _currentMaxHp;
		OnChangedHp?.Invoke(_hp);
	}
}
