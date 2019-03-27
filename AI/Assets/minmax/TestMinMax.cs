using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestMinMax : MonoBehaviour
{
	public const float BOARD_WIDTH = 108f;

	public Transform tfBoardCell0x0;

	public GameObject objO = null;
	public GameObject objX = null;

	private void Update() 
	{
		
	}

	public void OnClickBoardCell(int cidx)
	{
		Debug.Log("xx-- OnClickBoardCell > " + cidx);
	}
}
