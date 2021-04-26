using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class CutsceneBehaviour : MonoBehaviour
{
	[Header("Timeline")]
	public PlayableDirector _cutsceneTimeline;

	[Header("Maker Events")]
	public UnityEvent _cutsceneTimelineFinished;

	public void StartTimeline()
	{
		_cutsceneTimeline.Play();
	}

	public void TimelineFinished()
	{
		_cutsceneTimelineFinished.Invoke();
	}
}
