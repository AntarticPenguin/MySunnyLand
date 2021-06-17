using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerSwitch : MonoBehaviour, IInteractable
{
    #region Variables
    public GameObject[] _triggerTargets;
	public Sprite _activatedSprite;
	public Text _textUI;
	public bool _canRepeat;

	private Sprite _deactivatedSprite;
	private bool _isActivated;
	#endregion

	#region Unity Methods
	private void Start()
	{
		_isActivated = false;
		_deactivatedSprite = GetComponent<SpriteRenderer>().sprite;
		_textUI.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			_textUI.enabled = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			_textUI.enabled = false;
		}
	}

	#endregion

	#region IInteractable interface
	public void Interact()
	{
		if (_isActivated && !_canRepeat)
			return;

		_isActivated = !_isActivated;

		if (_triggerTargets != null)
		{
			GetComponent<SpriteRenderer>().sprite = _isActivated ? _activatedSprite : _deactivatedSprite;
			for(int i = 0; i < _triggerTargets.Length; i++)
			{
				_triggerTargets[i].GetComponent<ITriggerable>()?.Trigger();
			}
			
		}
	}
	#endregion
}
