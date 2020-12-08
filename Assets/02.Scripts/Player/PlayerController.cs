using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    public CircleCollider2D _circleCollider;
    public Vector2 boxSize;

    //movement
    public float _speed = 3.0f;
    private Vector3 _flippedScale = new Vector3(-1.0f, 1.0f, 1.0f);
    private Vector2 _refVelocity = Vector2.zero;
    public float _smoothDamping = 0.05f;

    //jump
    private bool _jumpInput;
    public float _jumpForce = 500.0f;

    private Vector2 _movement;

    public bool _isGrounded;

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
        _isGrounded = true;

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
        UpdateMovement();
        UpdateDirection();
        UpdateJump();
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
            _animator.SetFloat(_animSpeedFloat, Mathf.Abs(_rigidbody.velocity.x));
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
        float distance = _circleCollider.bounds.extents.y + 0.05f;
        Vector3 startPos = _circleCollider.bounds.center;
        startPos.y -= _circleCollider.bounds.extents.y;
        Collider2D hitCollider = Physics2D.OverlapBox(startPos, boxSize, 0, LayerMask.GetMask("Ground"));

        if (hitCollider != null)
            _isGrounded = true;
        else
            _isGrounded = false;

        _animator.SetBool(_animGroundedBool, _isGrounded);


    }

	private void OnDrawGizmos()
	{
        Vector3 startPos = _circleCollider.bounds.center;
        startPos.y -= _circleCollider.bounds.extents.y;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(startPos, boxSize);
	}
	#endregion
}
