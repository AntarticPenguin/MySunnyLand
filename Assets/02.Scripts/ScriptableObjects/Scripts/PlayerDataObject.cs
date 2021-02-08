using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="PlayerData", menuName ="ScriptableObjects/PlayerData")]
public class PlayerDataObject : ScriptableObject
{
	#region Variables
	public int _life;
	public int _hp;

	private int _totalScore;
	[NonSerialized] private bool _isInitialized = false;

	//event
	public event Action<int> OnChangedScore;
	public event Action<int> OnChangedLife;
	public event Action<int> OnChangedHp;
	#endregion

	#region Properties
	public int TotalScore => _totalScore;
	#endregion

	private void OnEnable()
	{
		Debug.Log("Init");
		if (_isInitialized)
			return;

		_isInitialized = true;
		_totalScore = 0;
		_life = 5;
		_hp = 3;
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
