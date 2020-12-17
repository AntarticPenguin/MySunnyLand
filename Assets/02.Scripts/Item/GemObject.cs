using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemObject : MonoBehaviour, IInteractable
{
    #region Variables
    public int _scorePoint;
    private bool _IsInteracted;     //중복방지
	#endregion

	#region Unity Methods
    void Awake()
	{
        _IsInteracted = false;
	}
	#endregion

	#region Methods
	public void Interact()
    {
        if (_IsInteracted)
            return;

        _IsInteracted = true;
        GameManager.Instance.AddScore(_scorePoint);
        Destroy(gameObject);
    }
    #endregion
}
