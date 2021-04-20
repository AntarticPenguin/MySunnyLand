using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkSprite : MonoBehaviour
{
	public float _blinkInterval;

	private SpriteRenderer _sprite;
	private Color _origColor;
	private float _blinkDuration;

	#region Unity Method
	private void Start()
	{
		_sprite = GetComponentInChildren<SpriteRenderer>();
		_origColor = _sprite.color;

		_blinkDuration = 0.0f;
	}
	#endregion

	public void StartBlink(float time)
	{
		StartCoroutine(BlinkCoroutine(time));
	}

	private IEnumerator BlinkCoroutine(float time)
	{
		_blinkDuration = 0.0f;
		float checkInterval = 0.0f;
		while(_blinkDuration <= time)
		{
			_blinkDuration += Time.deltaTime;
			checkInterval += Time.deltaTime;

			if(checkInterval >= _blinkInterval)
			{
				checkInterval = 0.0f;
				_sprite.color = (_sprite.color == Color.clear) ? _origColor : Color.clear;
			}
			yield return null;
		}
		_sprite.color = _origColor;
		yield return null;
	}
}
