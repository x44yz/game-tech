using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CfgMonster
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

public static class MonsterConfigs
{
	public static List<CfgMonster> configs = new List<CfgMonster>();

	public static void Init()
	{
		CfgMonster cfg = new CfgMonster();
		cfg.name = "Foul Crow";
		cfg.lvl = 4;
		cfg.exp = 22;
		cfg.hp[0] = 2; cfg.hp[1] = 6;
		cfg.speed = 4;
		cfg.attack[0] = 1; cfg.attack[1] = 2;
		cfg.attackRating = 23;

		cfg = new CfgMonster();
		cfg.name = "Blood Hawk";
		cfg.lvl = 6;
		cfg.exp = 29;
	}
}
