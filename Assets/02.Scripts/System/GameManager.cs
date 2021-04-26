using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	#region Variables
	[Header("Player Data")]
	public PlayerDataObject _playerDataObject;

	[SerializeField]
	private int _initialLife;

	[SerializeField] [Range(1, 5)] 
	private int _initialHp;
	#endregion

	#region Unity Methods
	protected override void Awake()
	{
		if (CheckDuplicationInScene())
			return;

		base.Awake();
		DontDestroy();
	}
	#endregion

	#region Helper Methods
	public void GameOver()
	{
		UIManager.Instance.ActiveGameOverUI(true);
	}

	public void PlayerDied()
	{
		_playerDataObject.DecreaseLife();
		_playerDataObject.ResetHp();
	}
	#endregion
}
