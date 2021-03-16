using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	#region Variables
	public GameObject _playerPrefab;
	public PlayerDataObject _playerDataObject;

	[SerializeField] private int _initialLife;
	[SerializeField] private int _initialHp;
	[SerializeField] private SavePoint _initialRespawnPoint;        //씬 시작시 사용될 최초 리스폰포인트
	private SavePoint _respawnPoint;
	#endregion

	#region Unity Methods
	private void Start()
	{
		if(_initialRespawnPoint != null)
		{
			_respawnPoint = _initialRespawnPoint;
			Instantiate(_playerPrefab, _initialRespawnPoint.transform.position, Quaternion.identity);
			InitPlayerData();
		}
		else
		{
			Debug.LogError("Set initial respawn point!!");
		}
	}
	#endregion

	#region Helper Methods
	public void SetRespawnPoint(SavePoint newPoint)
	{
		_respawnPoint = newPoint;
	}

	public void RespawnPlayer()
	{
		_playerDataObject.DecreaseLife();
		_playerDataObject.ResetHp();
		_respawnPoint.PlayOpenDoor();
		Instantiate(_playerPrefab, _respawnPoint.transform.position, Quaternion.identity);
	}

	public void GameOver()
	{
		UIManager.Instance.ActiveGameOverUI(true);
	}

	private void InitPlayerData()
	{
		_playerDataObject.Initialize(_initialLife, _initialHp);
	}
	#endregion
}
