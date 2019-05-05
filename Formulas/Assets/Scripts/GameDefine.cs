using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDefine
{
	public const int MAX_PLAYER_CLASS = 3;
}

public enum FaceDir
{
	NONE = -1,
	UP = 0,
	DOWN = 1,
	LEFT = 2,
	RIGHT = 3,
}

// inv = inventory
// logical equipment locations
public enum InvBodyLoc
{
	INVLOC_HEAD = 0,
	INVLOC_RING_LEFT = 1,
	INVLOC_RING_RIGHT = 2,
	INVLOC_AMULET = 3,
	INVLOC_HAND_LEFT = 4,
	INVLOC_HAND_RIGHT = 5,
	INVLOC_CHEST = 6,
	INVLOC_COUNT,
} 