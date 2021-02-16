using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="PlayerData", menuName ="ScriptableObjects/PlayerData")]
public class PlayerDataObject : ScriptableObject
{
	#region Variables
	private int _life;
	private int _hp;

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
		_life = life;
		_hp = hp;
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
		OnChangedHp?.Invoke(_hp);
	}

	public void DecreaseHp(int amount)
	{
		_hp -= amount;
		OnChangedHp?.Invoke(_hp);
	}
}
