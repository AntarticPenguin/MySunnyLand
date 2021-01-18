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
}
