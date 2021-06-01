using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
	public enum eDirection
	{
		UP = 0,
		DOWN = 180,
		LEFT = 90,
		RIGHT = -90,
	}

	#region Variables
	public eDirection _direction;

	[SerializeField] private int _damage;
	private float _offset = 0.06f;
	#endregion

	#region Unity Methods
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == TagAndLayer.Tag.Player)
		{
			collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
		}
	}

	private void OnValidate()
	{
		if (GameObject.Find("Grid") == null)
			return;

		Grid grid = GameObject.Find("Grid").GetComponent<Grid>();
		Vector3Int cellPosition = grid.WorldToCell(transform.position);
		transform.position = grid.GetCellCenterWorld(cellPosition);
		transform.eulerAngles = new Vector3(0, 0, (int)_direction);

		Vector3 newPosition = transform.position;
		switch (_direction)
		{
			case eDirection.UP:
				newPosition.y -= _offset;
				break;
			case eDirection.DOWN:
				newPosition.y += _offset;
				break;
			case eDirection.LEFT:
				newPosition.x += _offset;
				break;
			case eDirection.RIGHT:
				newPosition.x -= _offset;
				break;
			default:
				break;
		}
		transform.position = newPosition;
	}
	#endregion
}
