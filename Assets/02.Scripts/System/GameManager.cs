using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	#region Variables
	private int _totalScore;
	public Action<int> OnScoreChanged;
	#endregion

	#region Unity Methods;
	protected override void Awake()
	{
		base.Awake();
		_totalScore = 0;
		OnScoreChanged += ScoreUI.Instance.UpdateScore;
	}
	#endregion

	public void AddScore(int point)
	{
		_totalScore += point;
		OnScoreChanged?.Invoke(_totalScore);
	}
}
