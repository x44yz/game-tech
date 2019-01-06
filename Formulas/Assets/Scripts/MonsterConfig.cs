using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterConfig
{
	public string name;
	public int lvl;
	public int exp;
	public int[] hp = new int[2];
	public int speed;	// move speed
	// tc / rune 都是掉落
	public int tc;	// treasure class
	public string rune; // 符文
	public int[] attack = new int[2];
	public int attackRating;
}
