using System;
using UnityEngine;

namespace Diablo
{
	public class Command
	{
		// public virtual void Serialize();
		// public virtual void Deserialize();
		public virtual void Handle() {}
	}

	public class CmdStartAttack : Command
	{
		public int playerId = 0;
		public int x = 0;
		public int y = 0;

		public override void Handle()
		{
			Player plr = World.Instance.GetPlayer(playerId);
			plr.destAction = ActionType.ATTACK;
			plr.destX = x;
			plr.destY = y;
		}
	}
}
