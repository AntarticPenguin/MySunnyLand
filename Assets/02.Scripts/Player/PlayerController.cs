using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    #region Variables
    public Collider2D _bodyCollider;
    public Vector2 _groundCheckBox = new Vector2(0.26f, 0.02f);
    public PhysicsMaterial2D _dynamicPhysics;

    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private ContactPoint2D[] _bodyContacts;
    private ContactFilter2D _bodyContactsFilter;
    private float _previousFriction;

    //movement
    public float _speed = 3.0f;
    public float _smoothDamping = 0.05f;
    public float _climbSpeed = 1.5f;

    private Vector3 _flippedScale = new Vector3(-1.0f, 1.0f, 1.0f);
    private Vector2 _movement;
    private Vector2 _refVelocity = Vector2.zero;
    public bool _isGrounded;
    public bool _isClimbing;

    //jump
    public float _jumpForce = 300.0f;

    private bool _jumpInput;

    //animation
    private Animator _animator;
    private int _animSpeedFloat;
    private int _animGroundedBool;
    private int _animVerticalSpeedFloat;
    private int _animHurtTrigger;
    private int _animClimbBool;
    private int _animClimbingOrderBool;

    //status
    public int _hp;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _bodyContacts = new ContactPoint2D[2];
        _bodyContactsFilter.SetLayerMask(LayerMask.GetMask(TagAndLayer.Layer.Ground));
        _previousFriction = _dynamicPhysics.friction;

        _animator = GetComponent<Animator>();
        _animSpeedFloat = Animator.StringToHash(AnimatorKey.Speed);
        _animGroundedBool = Animator.StringToHash(AnimatorKey.Grounded);
        _animVerticalSpeedFloat = Animator.StringToHash(AnimatorKey.VerticalSpeed);
        _animHurtTrigger = Animator.StringToHash(AnimatorKey.Hurt);
        _animClimbBool = Animator.StringToHash(AnimatorKey.Climb);
        _animClimbingOrderBool = Animator.StringToHash(AnimatorKey.ClimbingOrder);
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

        //climb
        if(Input.GetKey(KeyCode.UpArrow))
		{
            _movement.y = 1.0f;
		}
        else if(Input.GetKey(KeyCode.DownArrow))
		{
            _movement.y = -1.0f;
		}

        if(_isClimbing && !_movement.Equals(Vector2.zero))
		{
            bool order = _animator.GetBool(_animClimbingOrderBool);
            _animator.SetBool(_animClimbingOrderBool, !order);
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
        if (collision.gameObject.tag == TagAndLayer.Tag.Item)
        {
            collision.gameObject.GetComponent<IInteractable>()?.Interact();
        }
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer(TagAndLayer.Layer.Cliff) &&
				  1.0f <= _movement.y)
		{
			_isClimbing = true;
			_rigidbody.gravityScale = 0.0f;
			_animator.SetBool(_animClimbBool, true);
		}
        else if(_isClimbing && _isGrounded)
		{
            //절벽을 타고 내려와 땅에 닿았을 경우
            _isClimbing = false;
            _rigidbody.gravityScale = 1.0f;
            _animator.SetBool(_animClimbBool, false);
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (_isClimbing &&
			collision.gameObject.layer == LayerMask.NameToLayer(TagAndLayer.Layer.Cliff))
		{
			_isClimbing = false;
			_rigidbody.gravityScale = 1f;
            _animator.SetBool(_animClimbBool, false);
        }
	}
	#endregion

	#region Methods
	private void UpdateMovement()
	{
        if(_isClimbing)
		{
            _rigidbody.velocity = new Vector2(_movement.x * _climbSpeed, _movement.y * _climbSpeed);
        }
        else
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

            if (_isGrounded)
            {
                _animator.SetFloat(_animSpeedFloat, Mathf.Abs(_rigidbody.velocity.x));
            }
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
        Vector3 startPos = _bodyCollider.bounds.center;
        startPos.y -= _bodyCollider.bounds.extents.y;
        Collider2D hitCollider = Physics2D.OverlapBox(startPos, _groundCheckBox, 0, LayerMask.GetMask(TagAndLayer.Layer.Ground));

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
            _bodyCollider.GetContacts(_bodyContacts);
            if(0.7 < _bodyContacts[0].normal.y && _bodyContacts[0].normal.y < 1)
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
            _previousFriction = _dynamicPhysics.friction;
            _bodyCollider.enabled = false;
            _bodyCollider.enabled = true;
        }
    }

	private void OnDrawGizmos()
	{
        Vector3 startPos = _bodyCollider.bounds.center;
        startPos.y -= _bodyCollider.bounds.extents.y;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(startPos, _groundCheckBox);
	}
	#endregion

	#region IDamagable Interfaces
	public void TakeDamage(int damage)
	{
        _hp -= damage;
        if(_hp <= 0)
		{
            _hp = 0;
            Debug.Log("YOU DIED");
		}

        _animator.SetTrigger(_animHurtTrigger);
	}
	#endregion
}
