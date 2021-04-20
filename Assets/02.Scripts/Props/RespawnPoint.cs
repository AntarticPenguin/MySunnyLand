using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RespawnPoint : MonoBehaviour
{
	#region Variables
	public GameObject _effectPrefab;
	public bool _isStartingPoint;

	private bool _isActivated = false;
	private Animator _animator;
	private int _gainDoorHash;
	private int _openDoorHash;
	#endregion

	#region Unity Methods
	private void Start()
	{
		_animator = GetComponent<Animator>();
		_gainDoorHash = Animator.StringToHash("GainDoor");
		_openDoorHash = Animator.StringToHash("OpenDoor");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (_isActivated)
		{
			return;
		}

		if(collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			_isActivated = true;
			_animator.Play(_gainDoorHash);
			GameManager.Instance.SetRespawnPoint(this);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
		Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
	}
	#endregion

	#region Helper Methods
	public void PlayOpenDoor()
	{
		_animator.Play(_openDoorHash);
	}
	#endregion
}
