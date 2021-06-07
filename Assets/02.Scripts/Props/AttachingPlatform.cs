using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AttachingPlatform : MonoBehaviour
{
	#region Variables
	public bool _ignoreRotation;

	private PlayerController _target;
	#endregion

	#region Unity Methods
	private void Start()
	{
		_target = null;
	}

	private void FixedUpdate()
	{
		if (!_ignoreRotation && _target != null)
		{
			if (_target.FacingRight)
				_target.transform.rotation = transform.rotation;
			else
				_target.transform.rotation = transform.rotation * Quaternion.Euler(0, 180f, 0f);
		}
	}

	//Attaching the player to the platform
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			collision.gameObject.transform.SetParent(transform);
			_target = collision.gameObject.GetComponent<PlayerController>();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			collision.gameObject.transform.SetParent(null);

			if (_target.FacingRight)
			{
				_target.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			}
			else
			{
				_target.transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
			}

			_target = null;
		}
	}
	#endregion
}
