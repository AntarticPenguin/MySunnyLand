using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerSwitch : MonoBehaviour, IInteractable
{
    #region Variables
    public GameObject _triggerTarget;
	public Sprite _activatedSprite;

	private bool _isActivated;
	#endregion

	#region Unity Methods
	private void Start()
	{
		_isActivated = false;
	}
	#endregion

	#region IInteractable interface
	public void Interact()
	{
		if (_isActivated)
			return;

		if(_triggerTarget != null)
		{
			_isActivated = true;
			GetComponent<SpriteRenderer>().sprite = _activatedSprite;
			_triggerTarget.GetComponent<ITriggerable>()?.Trigger();
		}
	}
	#endregion
}
