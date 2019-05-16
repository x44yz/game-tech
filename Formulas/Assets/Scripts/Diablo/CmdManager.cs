using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diablo
{
	public class CmdManager : MonoSingleton<CmdManager> 
	{
		private Queue<Command> cmds = new Queue<Command>();

		public void SendCmd(CmdType ctype)
		{
			
		}

		private void Update()
		{
			// parse cmd
			while (cmds.Count > 0)
			{
				Command cmd = cmds.Dequeue();
				cmd.Handle();
			}
		}
	}
}
