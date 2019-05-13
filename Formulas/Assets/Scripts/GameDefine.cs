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
public class InvBodyLoc
{
	public const int HEAD = 0;
	public const int RING_LEFT = 1;
	public const int RING_RIGHT = 2;
	public const int AMULET = 3;
	public const int HAND_LEFT = 4;
	public const int HAND_RIGHT = 5;
	public const int CHEST = 6;
	public const int COUNT = 7;
}

public class ActionType
{
	public const int NONE = 0;
	public const int ATTACK = 1;
}

public enum CmdType
{
	START_ATTACK = 0,
}