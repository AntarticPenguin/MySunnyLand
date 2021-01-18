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
    private Vector3 _flippedScale = new Vector3(-1.0f, 1.0f, 1.0f);
    private LayerMask _groundLayerMask;

    private StateMachine<PlayerController> _stateMachine;

    //movement
    public float _speed = 3.0f;
    public float _smoothDamping = 0.05f;
    public float _climbSpeed = 1.5f;

    private bool _isFlipped;
    private Vector2 _movement;
    public bool _isGrounded;
    public bool _isClimbing;

    //jump
    public float _jumpForce = 300.0f;

    private bool _jumpInput;

    //attack
    public GameObject _projectilePrefab;

    private Vector3 _launchPosition;
    private Vector3 _launchOffset = new Vector3(0.3f, 0.15f, 0.0f);

    //animation
    private Animator _animator;
    private int _animGroundedBool;
    private int _animVerticalSpeedFloat;

    //status
    public int _healthCount;
    public float _invincibleTime;

    private bool _isInvincible;
	#endregion

	#region Properties
	public ref Vector2 Movement
	{
		get
		{
			return ref _movement;
		}
	}

	public bool IsAlive => (_healthCount > 0);
    public bool IsFlipped => _isFlipped;
    public bool IsGrounded => _isGrounded;
    public bool IsClimbing
    {
        get { return _isClimbing; }
        set { _isClimbing = value; }
    }

    public float Speed => _speed;
    public float ClimbSpeed => _climbSpeed;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _bodyContacts = new ContactPoint2D[2];
        _bodyContactsFilter.SetLayerMask(LayerMask.GetMask(TagAndLayer.Layer.Ground) |
            LayerMask.GetMask(TagAndLayer.Layer.Platform));
        _previousFriction = _dynamicPhysics.friction;

        _stateMachine = new StateMachine<PlayerController>(this, new PlayerIdleState());
        _stateMachine.AddState(new PlayerMoveState());
        _stateMachine.AddState(new PlayerDamagedState());
        _stateMachine.AddState(new PlayerClimbState());

        _animator = GetComponent<Animator>();
        _animGroundedBool = Animator.StringToHash(AnimatorKey.Grounded);
        _animVerticalSpeedFloat = Animator.StringToHash(AnimatorKey.VerticalSpeed);

        _isInvincible = false;
        _invincibleTime = 3.0f;

        _groundLayerMask = LayerMask.GetMask(TagAndLayer.Layer.Ground) | LayerMask.GetMask(TagAndLayer.Layer.Platform);
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update(Time.deltaTime);

        if (_stateMachine.GetCurrentStateType() == typeof(PlayerDamagedState))
            return;

        //move
        _movement = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
		{
            _movement.x = -1.0f;
            _isFlipped = true;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
		{
            _movement.x = 1.0f;
            _isFlipped = false;
        }

        //climb
        if (Input.GetKey(KeyCode.UpArrow))
		{
            _movement.y = 1.0f;
		}
        else if(Input.GetKey(KeyCode.DownArrow))
		{
            _movement.y = -1.0f;
		}
     
        //jump
        if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
		{
            _jumpInput = true;
		}

        //attack
        if(Input.GetKeyDown(KeyCode.Z))
		{
            GameObject go = Instantiate(_projectilePrefab, _launchPosition, Quaternion.identity);
            Projectile projectile = go.GetComponent<Projectile>();
            if(projectile != null)
            {
                projectile._owner = gameObject;
                Vector2 direction = Vector2.right * (_isFlipped ? -1.0f : 1.0f);
                projectile._direction = direction;
            }
		}
    }

	private void FixedUpdate()
	{
        CheckGround();
        UpdateFriction();
        UpdateDirection();
        UpdateJump();

        _stateMachine.FixedUpdate(Time.fixedDeltaTime);
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
            _stateMachine.ChangeState<PlayerClimbState>();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		
	}
	#endregion

	#region Methods

	private void UpdateDirection()
	{
		//update sprite flipped
		if (!_isFlipped)
		{
			_transform.localScale = Vector3.one;
			_launchPosition = new Vector3(transform.position.x + _launchOffset.x, transform.position.y - _launchOffset.y);
		}
		else
		{
			_transform.localScale = _flippedScale;
			_launchPosition = new Vector3(transform.position.x - _launchOffset.x, transform.position.y - _launchOffset.y);
		}
	}

	private void UpdateJump()
	{
		if (_stateMachine.GetCurrentStateType() == typeof(PlayerDamagedState))
		{
            _animator.SetFloat(_animVerticalSpeedFloat, 0);
            return;
        }
			
		if (_jumpInput)
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
        Collider2D hitCollider = Physics2D.OverlapBox(startPos, _groundCheckBox, 0, _groundLayerMask);

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
        if(IsGrounded && _movement.Equals(Vector2.zero))
        {
            _bodyCollider.GetContacts(_bodyContactsFilter, _bodyContacts);
            if(0.7 < _bodyContacts[0].normal.y && _bodyContacts[0].normal.y <= 1)
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
        if (_isInvincible)
            return;

        _healthCount -= damage;
        _isInvincible = true;
        _stateMachine.ChangeState<PlayerDamagedState>();

        if (!IsAlive)
        {
            _healthCount = 0;
            Debug.Log("YOU DIED");
            return;
        }

        StartCoroutine(ResetInvincibility(_invincibleTime));
	}

    private IEnumerator ResetInvincibility(float coolTime)
	{
        UtilMethods.IgnoreLayerCollisionByName(TagAndLayer.Layer.Player,
            TagAndLayer.Layer.Enemy);
        yield return new WaitForSeconds(coolTime);
        _isInvincible = false;
        UtilMethods.IgnoreLayerCollisionByName(TagAndLayer.Layer.Player,
            TagAndLayer.Layer.Enemy, false);
    }
    #endregion
}
