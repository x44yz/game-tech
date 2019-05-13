using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager> 
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			CmdManager.Instance.SendCmd(CmdType.START_ATTACK);
		}
	}
}