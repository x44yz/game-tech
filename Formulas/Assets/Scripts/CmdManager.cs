using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdManager : MonoSingleton<CmdManager> 
{
	private Queue<Command> cmds = new Queue<Command>();

	public void SendCmd(CmdType ctype)
	{
		
	}

	private void Update()
	{
		// parse cmd
		
	}
}