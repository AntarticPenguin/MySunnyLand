using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    #region Variables
    public Vector2 _startPos;
    public Vector2 _targetPos;
    public bool _canLoop;
    public float _speed;

    private Rigidbody2D _rigidbody;
    
	#endregion

	#region Unity Methods
	void Start()
    {
        transform.position = _startPos;

        _rigidbody = GetComponent<Rigidbody2D>();
    }

	private void FixedUpdate()
	{
        float step = _speed * Time.fixedDeltaTime;
        _rigidbody.position = Vector2.MoveTowards(_rigidbody.position, _targetPos, step);

        //if platform has arrived at target position
        if (_canLoop && (Vector2.Distance(_rigidbody.position, _targetPos) < step))
		{
			Vector2 tempPos = _targetPos;
			_targetPos = _startPos;
			_startPos = tempPos;
		}
	}
	#endregion
}
