using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingSpike : MonoBehaviour
{
    #region Variables
    public float _distance;

    [SerializeField] private int _damage;
    private int _whatIsPlayer;
    private Rigidbody2D _rigidbody;
	#endregion

	#region Unity Methods
	void Start()
    {
        _whatIsPlayer = LayerMask.GetMask(TagAndLayer.Layer.Player);
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.simulated = false;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _distance, _whatIsPlayer);
        if (hit.collider != null)
        {
            _rigidbody.simulated = true;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
            collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
            Destroy(gameObject);
        }
		else if (collision.gameObject.layer == LayerMask.NameToLayer(TagAndLayer.Layer.Ground)
			&& _rigidbody.velocity.magnitude > 0.2f)
		{
			Destroy(gameObject);
		}
	}

	private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * _distance);
    }
    #endregion
}
