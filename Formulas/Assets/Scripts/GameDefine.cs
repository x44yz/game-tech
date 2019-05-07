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
	HEAD = 0,
	RING_LEFT = 1,
	RING_RIGHT = 2,
	AMULET = 3,
	HAND_LEFT = 4,
	HAND_RIGHT = 5,
	CHEST = 6,
	COUNT,
} 