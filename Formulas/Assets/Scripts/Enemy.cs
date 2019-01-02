using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
	protected override void Start() 
	{
		base.Start();

		faceDir = FaceDir.LEFT;
	}
}
