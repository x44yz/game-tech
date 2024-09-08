using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diablo
{
	public class InputManager : MonoSingleton<InputManager> 
	{
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Debug.Log("xx-- click left");	
				//CmdManager.Instance.SendCmd(CmdType.START_ATTACK);
			}
		}
	}
}
