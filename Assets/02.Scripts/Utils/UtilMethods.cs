using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilMethods
{
	public static void IgnoreLayerCollisionByName(string layerName1, string layerName2, bool ignore = true)
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(layerName1),
			LayerMask.NameToLayer(layerName2), ignore);
	}

	public static Vector2 RadianToVector2(float radian)
	{
		return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
	}

	public static Vector2 DegreeToVector2(float degree)
	{
		return RadianToVector2(degree * Mathf.Deg2Rad);
	}
}
