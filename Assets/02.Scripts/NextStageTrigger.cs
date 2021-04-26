using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageTrigger : MonoBehaviour
{
	[Header("ScreenFader")]
	public ScreenFadeManager _screenFader;

	public int _sceneIndex;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			_screenFader.StartFadeFromBlack();
			SceneManager.LoadScene(_sceneIndex);
		}
	}
}
