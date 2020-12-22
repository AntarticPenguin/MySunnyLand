using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemObject : MonoBehaviour, IInteractable
{
    #region Variables
    public int _scorePoint;
    public GameObject _effectPrefab;

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
        GameObject effect = Instantiate(_effectPrefab);
        effect.transform.position = transform.position;
        effect.transform.localScale = Vector3.one;
        effect.GetComponent<AnimationCallback>().InitEvent(null, delegate
        {
            Destroy(effect);
        });

        GameManager.Instance.AddScore(_scorePoint);
        Destroy(gameObject);
    }
    #endregion
}
