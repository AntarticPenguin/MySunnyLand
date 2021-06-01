using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class TriggerBridge : MonoBehaviour, ITriggerable
{
    #region Variables
    [Range(1, 100)]
    public int _width;

    [Range(1, 1)]
    public int _height;

    private Rigidbody2D _rigidbody;
    private HingeJoint2D _hinge;
    private float _spriteSize = 0.32f;
	#endregion

	#region Unity Methods
	void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;

        _hinge = GetComponent<HingeJoint2D>();
        _hinge.enabled = false;
    }

	private void OnValidate()
	{
        Vector2 newSize = new Vector2(_width * _spriteSize, _height * _spriteSize);

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.size = newSize;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = newSize;

        transform.localScale = Vector3.one;
    }
    #endregion

    #region ITriggerable interface
    public void Trigger()
	{
        Vector2 newSize = new Vector2((_width - 1) * _spriteSize, _height * _spriteSize);
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = newSize;

        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _hinge.enabled = true;
    }
	#endregion
}
