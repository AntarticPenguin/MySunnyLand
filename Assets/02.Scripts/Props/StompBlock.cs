using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompBlock : MonoBehaviour
{
	#region Variables
	public GameObject _effectPrefab;
	public float _distance;

	private LayerMask _whatIsPlayer;
	private int _whatIsGround;
	private Rigidbody2D _rigidbody;
	private BoxCollider2D _collider;
	private bool _isGrounded;
	#endregion

	#region Unity Methods
	void Start()
    {
		_whatIsPlayer = LayerMask.GetMask(TagAndLayer.Layer.Player);
		_whatIsGround = LayerMask.GetMask(TagAndLayer.Layer.Ground) | LayerMask.GetMask(TagAndLayer.Layer.Platform);
		_rigidbody = GetComponent<Rigidbody2D>();
		_rigidbody.simulated = false;

		_collider = GetComponent<BoxCollider2D>();
		_isGrounded = false;
	}

    void Update()
    {
		if (_isGrounded)
			return;

		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _distance, _whatIsPlayer);
		if(hit.collider != null)
		{
			_rigidbody.simulated = true;
		}
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((_whatIsGround & (1 << collision.gameObject.layer)) != 0)
		{
			_isGrounded = true;

			Vector3 effectPosition = _collider.bounds.center;
			effectPosition.y -= _collider.bounds.extents.y;
			GameObject effect = Instantiate(_effectPrefab, effectPosition, Quaternion.identity);
			effect.GetComponent<AnimationCallback>()?.InitEvent(null, delegate
			{
				Destroy(effect);
			});
		}

		if(collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			int normalY = (int)collision.GetContact(0).normal.y;
			if (1 <= normalY)
			{
				collision.gameObject.GetComponent<PlayerController>()?.Kill();
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, Vector2.down * _distance);
	}
	#endregion
}
