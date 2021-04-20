using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
	#region Variables
	private SerializedProperty _startPosition;
	private SerializedProperty _targetPosition;
	#endregion

	#region Unity Methods
	private void OnEnable()
	{
		_startPosition = serializedObject.FindProperty("_startPos");
		_targetPosition = serializedObject.FindProperty("_targetPos");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		serializedObject.Update();

		MovingPlatform movingPlatform = target as MovingPlatform;

		EditorGUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Set Start Position", GUILayout.Width(200), GUILayout.ExpandWidth(true)))
			{
				if (_startPosition != null)
				{
					_startPosition.vector2Value = movingPlatform.transform.position;
				}
			}
		
			if (GUILayout.Button("Set Target Position", GUILayout.Width(200), GUILayout.ExpandWidth(true)))
			{
				if (_targetPosition != null)
				{
					_targetPosition.vector2Value = movingPlatform.transform.position;
				}
			}
		}
		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}
	#endregion
}
