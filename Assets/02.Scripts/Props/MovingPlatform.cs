using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    #region Variables
    public Vector2 _startPos;
    public Vector2 _targetPos;
    public bool _canLoop;
    public float _speed;

	private Transform _transform;
    
	#endregion

	#region Unity Methods
	void Start()
    {
		_transform = transform;
        transform.position = _startPos;
    }

	private void FixedUpdate()
	{
        float step = _speed * Time.fixedDeltaTime;
		transform.position = Vector2.MoveTowards(transform.position, _targetPos, step);

		//if platform has arrived at target position
		if (_canLoop && (Vector2.Distance(_transform.position, _targetPos) < step))
		{
			Vector2 tempPos = _targetPos;
			_targetPos = _startPos;
			_startPos = tempPos;
		}
	}

	private void OnValidate()
	{
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
	}
	#endregion
}
