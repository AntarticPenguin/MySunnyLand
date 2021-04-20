using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagAndLayer
{
	public class Tag
	{
		public const string Camera = "MainCamera";
		public const string Player = "Player";
		public const string Interactable = "Interactable";
		public const string Obstacle = "Obstacle";
		public const string Enemy = "Enemy";
		public const string DeadZone = "DeadZone";
		public const string Respawn = "Respawn";
		public const string VCam = "VCam";
	}
	
	public class Layer
	{
		public const string Ground = "Ground";
		public const string Cliff = "Cliff";
		public const string Player = "Player";
		public const string Enemy = "Enemy";
		public const string Platform = "Platform";
	}
}
