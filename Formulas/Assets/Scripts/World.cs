using System;
using UnityEngine;
using System.Collections.Generic;

public class World : MonoSingleton<World>
{
	private List<Player> plrs = new List<Player>();

	// public Actor FindNearestActor(Actor actor, ActorType atype)
	// {
	// 	throw new System.NotImplementedException();
	// }

	public Player GetPlayer(int playerId)
	{
		Debug.Assert(playerId >= 0 && playerId < plrs.Count, "CHECK");
		return plrs[playerId];
	}
}