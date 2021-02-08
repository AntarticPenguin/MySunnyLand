using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextStageTrigger : MonoBehaviour
{
	public LevelLoader _levelLoader;
	public int _sceneIndex;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			_levelLoader.LoadLevel(_sceneIndex);
		}
	}
}
