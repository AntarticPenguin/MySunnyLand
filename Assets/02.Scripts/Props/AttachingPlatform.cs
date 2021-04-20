using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AttachingPlatform : MonoBehaviour
{
	#region Variables
	public bool _ignoreRotation;

	private GameObject _target;
	#endregion

	#region Unity Methods
	private void Start()
	{
		_target = null;
	}

	private void FixedUpdate()
	{
		if(!_ignoreRotation && _target != null)
		{
			_target.transform.rotation = transform.rotation;
		}
	}

	//Attaching the player to the platform
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			collision.collider.transform.SetParent(transform);
			_target = collision.gameObject;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			collision.collider.transform.SetParent(null);
			collision.collider.transform.rotation = Quaternion.identity;
			_target = null;
		}
	}
	#endregion
}
