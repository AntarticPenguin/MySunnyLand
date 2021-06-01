using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MyExtension
{
    public static T ToEnum<T>(this string inString, bool ignoreCase = true)
	{
		if (!Enum.IsDefined(typeof(T), inString))
			return default(T);
		return (T)Enum.Parse(typeof(T), inString, ignoreCase);
	}
}
