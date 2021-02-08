using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public Animator _animator;

	private int _levelToLoad;
	private int _fadeInHash;
	private int _triggerFadeOutHash;

	private void Start()
	{
		_fadeInHash = Animator.StringToHash("FadeIn");
		_triggerFadeOutHash = Animator.StringToHash("FadeOut");

		_animator.Play(_fadeInHash);
	}

	public void LoadLevel(int sceneIndex)
	{
		_levelToLoad = sceneIndex;
		_animator.SetTrigger(_triggerFadeOutHash);
	}

	public void OnFadeComplete()
	{
		SceneManager.LoadScene(_levelToLoad);
	}
}
