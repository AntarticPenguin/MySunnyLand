using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public CircleCollider2D _circleCollider;
    public Vector2 _groundCheckBox = new Vector2(0.26f, 0.02f);
    public PhysicsMaterial2D _dynamicPhysics;

    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private ContactPoint2D[] _contacts;
    private float _previousFriction;


    //movement
    public float _speed = 3.0f;
    public float _smoothDamping = 0.05f;

    private Vector3 _flippedScale = new Vector3(-1.0f, 1.0f, 1.0f);
    private Vector2 _movement;
    private Vector2 _refVelocity = Vector2.zero;

    //jump
    public float _jumpForce = 300.0f;
    public bool _isGrounded;

    private bool _jumpInput;

    //animation
    private Animator _animator;
    private int _animSpeedFloat;
    private int _animGroundedBool;
    private int _animVerticalSpeedFloat;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _circleCollider = GetComponent<CircleCollider2D>();
        _contacts = new ContactPoint2D[2];
        _previousFriction = _dynamicPhysics.friction;

        _animator = GetComponent<Animator>();
        _animSpeedFloat = Animator.StringToHash(AnimatorKey.Speed);
        _animGroundedBool = Animator.StringToHash(AnimatorKey.Grounded);
        _animVerticalSpeedFloat = Animator.StringToHash(AnimatorKey.VerticalSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //move
        _movement = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
		{
            _movement.x = -1.0f;
		}
        else if(Input.GetKey(KeyCode.RightArrow))
		{
            _movement.x = 1.0f;
		}
        
        //jump
        if(_isGrounded && Input.GetKeyDown(KeyCode.Space))
		{
            _jumpInput = true;
		}
    }

	private void FixedUpdate()
	{
        CheckGround();
        UpdateFriction();
        UpdateMovement();
        UpdateDirection();
        UpdateJump();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == TagName.Item)
        {
            collision.gameObject.GetComponent<IInteractable>()?.Interact();
        }
    }
	#endregion

	#region Methods
	private void UpdateMovement()
	{
        Vector2 newVelocity = _rigidbody.velocity;
        if (_movement.Equals(Vector2.zero))
		{
            newVelocity.x = 0;
            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, newVelocity, ref _refVelocity, _smoothDamping);
            _animator.SetFloat(_animSpeedFloat, 0);

            return;
		}
        newVelocity.x = _movement.x * _speed;
        _rigidbody.velocity = newVelocity;

        if(_isGrounded)
		{
            _animator.SetFloat(_animSpeedFloat, Mathf.Abs(_rigidbody.velocity.x));
        }
	}

    private void UpdateDirection()
	{
        //update sprite flipped
        if (_movement.x > 0.0f)
            _transform.localScale = Vector3.one;
        else if (_movement.x < 0.0f)
            _transform.localScale = _flippedScale;
	}

    private void UpdateJump()
	{
        if(_jumpInput)
		{
            _jumpInput = false;
            _rigidbody.AddForce(new Vector2(0, _jumpForce));
		}

        _animator.SetFloat(_animVerticalSpeedFloat, _rigidbody.velocity.y);
	}

    private void CheckGround()
	{
        Vector3 startPos = _circleCollider.bounds.center;
        startPos.y -= _circleCollider.bounds.extents.y;
        Collider2D hitCollider = Physics2D.OverlapBox(startPos, _groundCheckBox, 0, LayerMask.GetMask("Ground"));

        if (hitCollider != null)
		{
            _isGrounded = true;
        }
        else
		{
            _isGrounded = false;
        }

        _animator.SetBool(_animGroundedBool, _isGrounded);
    }

    private void UpdateFriction()
	{
        if(_isGrounded && _movement.Equals(Vector2.zero))
        {
            _circleCollider.GetContacts(_contacts);
            if(0.7 < _contacts[0].normal.y && _contacts[0].normal.y < 1)
			{
                _dynamicPhysics.friction = 0.01f;
            }
        }
        else
		{
            _dynamicPhysics.friction = 0.0f;
            
        }

        if(_previousFriction != _dynamicPhysics.friction)
		{
            Debug.Log("Frction: " + _previousFriction + ", " + _dynamicPhysics.friction);
            _previousFriction = _dynamicPhysics.friction;
            _circleCollider.enabled = false;
            _circleCollider.enabled = true;
        }
    }

	private void OnDrawGizmos()
	{
        Vector3 startPos = _circleCollider.bounds.center;
        startPos.y -= _circleCollider.bounds.extents.y;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(startPos, _groundCheckBox);
	}
	#endregion
}
