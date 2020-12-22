using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationCallback : MonoBehaviour
{
	#region Variables
	private Action _beginCallback = null;
	private Action _endCallback = null;
	#endregion

	#region Methods
	public void InitEvent(Action beginCallback, Action endCallback)
	{
		_beginCallback = beginCallback;
		_endCallback = endCallback;
	}

	public void OnBeginEvent()
	{
		_beginCallback?.Invoke();
	}

	public void OnEndEvent()
	{
		_endCallback?.Invoke();
	}
	#endregion
}
