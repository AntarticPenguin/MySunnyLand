using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    //stat
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _climbSpeed = 1.5f;
    [SerializeField] private int _jumpAttackDamage = 1;

    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isClimbing;
    [SerializeField] private float _reactionPower;      //데미지를 받고 튕겨져 나가는 힘
    private bool _isFlipped;
    private Vector2 _movement;

    //jump
    public float _jumpForce = 300.0f;

    private bool _jumpInput;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _isFalling;

    //check slope
    private Vector2 _slopeNormalPerp;       //경사와 수직인 벡터
    private float _slopeDownAngle;
    private float _slopeDownAnglePrev;
    [SerializeField] private float _slopeCheckDistance = 0.5f;
    [SerializeField] private bool _isOnSlope;

    //attack
    public GameObject _projectilePrefab;

    private Vector3 _launchPosition;
    private Vector3 _launchOffset = new Vector3(0.3f, 0.15f, 0.0f);

    //animation
    private Animator _animator;
    private int _animGroundedBool;
    private int _animVerticalSpeedFloat;

    //status
    public PlayerDataObject _playerData;
    public float _invincibleTime;

    private bool _isInvincible;
    private BlinkSprite _blinkSprite;
    #endregion

    #region Properties
    public ref Vector2 Movement
	{
		get
		{
			return ref _movement;
		}
	}

	public bool IsAlive => (_playerData.Hp > 0);
    public bool IsFlipped => _isFlipped;
    public bool IsGrounded => _isGrounded;
    public bool IsClimbing
    {
        get { return _isClimbing; }
        set { _isClimbing = value; }
    }

    public bool IsJumping => _isJumping;
    public bool IsFalling => _isFalling;

    public float Speed => _speed;
    public float ClimbSpeed => _climbSpeed;
    public bool IsOnSlope => _isOnSlope;
    public ref Vector2 SlopeNormalPerp
	{
        get
		{
            return ref _slopeNormalPerp;
		}
	}

    public float ReactionPower => _reactionPower;
    public int JumpAttackDamage => _jumpAttackDamage;
    #endregion

    #region Unity Methods
    void Awake()
	{
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _bodyContacts = new ContactPoint2D[2];
        _bodyContactsFilter.SetLayerMask(LayerMask.GetMask(TagAndLayer.Layer.Ground) |
            LayerMask.GetMask(TagAndLayer.Layer.Platform));
        _previousFriction = _dynamicPhysics.friction;

        _animator = GetComponent<Animator>();
        _animGroundedBool = Animator.StringToHash(AnimatorKey.Grounded);
        _animVerticalSpeedFloat = Animator.StringToHash(AnimatorKey.VerticalSpeed);

        _blinkSprite = GetComponent<BlinkSprite>();
        _isInvincible = false;
        _invincibleTime = 3.0f;

        _groundLayerMask = LayerMask.GetMask(TagAndLayer.Layer.Ground) | LayerMask.GetMask(TagAndLayer.Layer.Platform);
        InitState();
    }

    void Start()
	{
        GameObject VCam = GameObject.FindGameObjectWithTag(TagAndLayer.Tag.VCam);
        VCam.GetComponent<CinemachineVirtualCamera>().Follow = _transform;

        UtilMethods.IgnoreLayerCollisionByName(TagAndLayer.Layer.Player,
            TagAndLayer.Layer.Enemy, false);
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update(Time.deltaTime);

        if (_stateMachine.GetCurrentStateType() == typeof(PlayerDamagedState) ||
            _stateMachine.GetCurrentStateType() == typeof(PlayerDeadState))
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
        CheckVerticalSlope();
        UpdateFriction();
        UpdateDirection();
        UpdateJump();

        _stateMachine.FixedUpdate(Time.fixedDeltaTime);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == TagAndLayer.Tag.Interactable)
        {
            collision.gameObject.GetComponent<IInteractable>()?.Interact();
        }
		else if (collision.gameObject.tag == TagAndLayer.Tag.DeadZone)
		{
			_stateMachine.ChangeState<PlayerDeadState>();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!IsClimbing && collision.gameObject.layer == LayerMask.NameToLayer(TagAndLayer.Layer.Cliff) &&
				  1.0f <= _movement.y)
		{
            _stateMachine.ChangeState<PlayerClimbState>();
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
            _rigidbody.AddForce(Vector2.up * _jumpForce);
		}

        if (!IsGrounded && _rigidbody.velocity.y < 0.0f)
            _isFalling = true;
        else if (_rigidbody.velocity.y >= 0.0f || IsGrounded)
            _isFalling = false;

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
            _isJumping = false;
        }
        else
		{
            _isGrounded = false;
            _isJumping = true;
        }

        _animator.SetBool(_animGroundedBool, _isGrounded);
    }

    private void CheckVerticalSlope()
	{
        Vector2 checkPos = _bodyCollider.bounds.center - new Vector3(0.0f, _bodyCollider.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, _slopeCheckDistance,
            _groundLayerMask);

		Debug.DrawRay(checkPos, Vector2.down, Color.red);

		if (hit)
		{
			_slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
			_slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(_slopeDownAngle >= 0 && _slopeDownAngle != _slopeDownAnglePrev)
			{
                _isOnSlope = true;
			}
            else
			{
                _isOnSlope = false;
            }
            _slopeDownAngle = _slopeDownAnglePrev;

			Debug.DrawRay(hit.point, _slopeNormalPerp, Color.red);
			Debug.DrawRay(hit.point, hit.normal, Color.green);
		}
	}

    private void UpdateFriction()
	{
		if (_stateMachine.GetCurrentStateType() == typeof(PlayerDeadState))
			return;

		if (IsGrounded && _movement.Equals(Vector2.zero))
		{
			_bodyCollider.GetContacts(_bodyContactsFilter, _bodyContacts);
			if (0.7f < _bodyContacts[0].normal.y && _bodyContacts[0].normal.y <= 1.0f)
			{
				_dynamicPhysics.friction = 0.01f;
			}
		}
		else
		{
			_dynamicPhysics.friction = 0.0f;

		}

		if (_previousFriction != _dynamicPhysics.friction)
		{
			_previousFriction = _dynamicPhysics.friction;
			_bodyCollider.enabled = false;
			_bodyCollider.enabled = true;
		}
	}

    private void InitState()
	{
        _stateMachine = new StateMachine<PlayerController>(this, new PlayerIdleState());
        _stateMachine.AddState(new PlayerMoveState());
        _stateMachine.AddState(new PlayerDamagedState());
        _stateMachine.AddState(new PlayerClimbState());
        _stateMachine.AddState(new PlayerDeadState());
    }

    public void Kill()
	{
        _stateMachine.ChangeState<PlayerDeadState>();
	}
    #endregion

    #region IDamagable Interfaces
    public bool CanBeDamagedByJumpAttack => false;

    public void TakeDamage(int damage)
	{
        if (_isInvincible)
            return;

        if (!IsAlive)
        {
            _stateMachine.ChangeState<PlayerDeadState>();
            return;
        }

        _playerData.DecreaseHp(damage);
        _isInvincible = true;

        _stateMachine.ChangeState<PlayerDamagedState>();
        StartCoroutine(ResetInvincibility(_invincibleTime));
	}

    private IEnumerator ResetInvincibility(float time)
	{
        _blinkSprite?.StartBlink(time);
        UtilMethods.IgnoreLayerCollisionByName(TagAndLayer.Layer.Player,
            TagAndLayer.Layer.Enemy);
        yield return new WaitForSeconds(time);
        _isInvincible = false;
        UtilMethods.IgnoreLayerCollisionByName(TagAndLayer.Layer.Player,
            TagAndLayer.Layer.Enemy, false);
    }
    #endregion
}
