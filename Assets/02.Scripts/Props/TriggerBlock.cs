using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBlock : MonoBehaviour, ITriggerable
{
	#region Variables
	[Header("Block will be on top if swith on")]
	public float _speed;
	public bool _bSwitchOn;

	private Rigidbody2D _rigidbody;
	#endregion

	#region Unity Methods
	// Start is called before the first frame update
	void Start()
    {
		_rigidbody = GetComponent<Rigidbody2D>();
		Switch();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	#endregion

	#region Methods
	private void Switch()
	{
		if (_bSwitchOn)
		{
			_rigidbody.velocity = new Vector2(0f, _speed);
		}
		else
		{
			_rigidbody.velocity = new Vector2(0f, -_speed);
		}
	}
	#endregion

	#region ITriggerable Interface
	public void Trigger()
	{
		_bSwitchOn = !_bSwitchOn;
		Switch();
	}
	#endregion
}
