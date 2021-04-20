using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointPlatform : MonoBehaviour
{
	#region Variables
    public float _speed;
    public float _lowerAngle;
    public float _upperAngle;

    private Quaternion _from;
    private Quaternion _to;
	#endregion

	#region Unity Methods
	void Start()
    {
        _from = Quaternion.Euler(new Vector3(0, 0, _lowerAngle));
        _to = Quaternion.Euler(new Vector3(0, 0, _upperAngle));
    }

    void Update()
    {
        float time = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.time * _speed, 1));
        transform.rotation = Quaternion.Slerp(_from, _to, time);
    }
	#endregion
}
