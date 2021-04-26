using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenFadeManager : MonoBehaviour
{
    [Header("Cutscenes")]
    public CutsceneBehaviour _fadeFromBlackTimeline;
    public CutsceneBehaviour _fadeToBlackTimeline;

	[Header("Fade When Starting Scene")]
	public bool _startSceneWithFadeFromBlack;

    public event Action _fadeFromBlackFinishedEvent;
    public event Action _fadeToBlackFinishedEvent;

	private void Start()
	{
		if(_startSceneWithFadeFromBlack)
		{
			StartFadeFromBlack();
		}
	}

	public void StartFadeFromBlack()
	{
		_fadeFromBlackTimeline.StartTimeline();
	}

	public void StartFadeToBlack()
	{
		_fadeToBlackTimeline.StartTimeline();
	}

	public void FadeFromBlackFinished()
	{
        _fadeFromBlackFinishedEvent?.Invoke();
	}

    public void FadeToBlackFinished()
	{
        _fadeToBlackFinishedEvent?.Invoke();
	}
}
