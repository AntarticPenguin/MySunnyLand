using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveController : MonoBehaviour
{
	#region Variables
	private bool _isActive;
	private EnemyController _enemyController;
	private Transform _transform;

	[SerializeField] private float _radius;
	#endregion

	#region Unity Methods
	private void Start()
	{
		_enemyController = GetComponent<EnemyController>();
		_isActive = _enemyController.IsActiveController;
		_transform = transform;
	}

	private void Update()
    {
		if (_isActive)
			return;

		Collider2D collider = Physics2D.OverlapCircle(_transform.position, _radius, LayerMask.GetMask(TagAndLayer.Layer.Player));
		if(collider != null)
		{
			_isActive = true;
			_enemyController.IsActiveController = true;
		}
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _radius);
	}
	#endregion
}
