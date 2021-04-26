using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class RespawnManager : SingletonMonoBehaviour<RespawnManager>
{
    [Header("ScreenFade Manager")]
    public ScreenFadeManager _screenfadeManager;

	[Header("Player Prefab")]
	public GameObject _playerPrefab;

	private RespawnPoint _currentRespawnPoint;

    public event Action _respawnEvent;

	protected override void Awake()
	{
		base.Awake();

		NewSceneStart();
	}

	private void OnEnable()
	{
		_respawnEvent += _screenfadeManager.StartFadeFromBlack;
	}

	private void OnDisable()
	{
		_respawnEvent -= _screenfadeManager.StartFadeFromBlack;
	}

	private void NewSceneStart()
	{
		GameObject[] findObjects = GameObject.FindGameObjectsWithTag(TagAndLayer.Tag.Respawn);
		for(int i = 0; i < findObjects.Length; i++)
		{
			RespawnPoint respawnPoint = findObjects[i].GetComponent<RespawnPoint>();
			if(respawnPoint._isStartingPoint)
			{
				_currentRespawnPoint = respawnPoint;
				break;
			}
		}

		Instantiate(_playerPrefab, _currentRespawnPoint.transform.position, Quaternion.identity);
	}

	public void SetRespawnPoint(RespawnPoint newPoint)
	{
		_currentRespawnPoint = newPoint;
	}

	public void RespawnPlayer()
	{
		_respawnEvent?.Invoke();
		_currentRespawnPoint.PlayOpenDoor();
		Instantiate(_playerPrefab, _currentRespawnPoint.transform.position, Quaternion.identity);
	}
}
