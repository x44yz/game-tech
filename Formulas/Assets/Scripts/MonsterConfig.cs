using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterAIType
{
	AI_ZOMBIE,
}

public class CfgMonster
{
	public string name;
	public int minLevel;
	public int maxLevel;
	public int level;
	public int minHP;
	public int maxHP;
	public MonsterAIType aiType;
	public int minDamage;
	public int maxDamage;
}

public static class MonsterConfigs
{
	//public static List<CfgMonster> configs = new List<CfgMonster>();

	public static CfgMonster[] configs = new CfgMonster[]{
		new CfgMonster(){name="Zombie", minLevel=1, maxLevel=3, level=1, minHP=4, maxHP=7, aiType=MonsterAIType.AI_ZOMBIE, },
	};

	public static void Init()
	{
		// CfgMonster cfg = new CfgMonster();
		// cfg.name = "Foul Crow";
		// cfg.lvl = 4;
		// cfg.exp = 22;
		// cfg.hp[0] = 2; cfg.hp[1] = 6;
		// cfg.walkSpeed = 4;
		// cfg.attack[0] = 1; cfg.attack[1] = 2;
		// cfg.attackRating = 23;

		// cfg = new CfgMonster();
		// cfg.name = "Blood Hawk";
		// cfg.lvl = 6;
		// cfg.exp = 29;
		// cfg.hp[0] = 2; cfg.hp[1] = 6;
		// cfg.walkSpeed = 4;
		// cfg.attack[0] = 2; cfg.attack[1] = 3;
		// cfg.attackRating = 41;

		// //
		// cfg = new CfgMonster();
		// cfg.name = "Fallen";
		// cfg.lvl = 1;
		// cfg.exp = 18;
		// cfg.hp[0] = 1; cfg.hp[1] = 4;
		// cfg.walkSpeed = 5;
		// cfg.attack[0] = 1; cfg.attack[1] = 2;
		// cfg.attackRating = 8;

		// cfg = new CfgMonster();
		// cfg.name = "Carver";
		// cfg.lvl = 5;
		// cfg.exp = 42;
		// cfg.hp[0] = 4; cfg.hp[1] = 9;
		// cfg.walkSpeed = 5;
		// cfg.attack[0] = 2; cfg.attack[1] = 4;
		// cfg.attackRating = 31;

		// //
		// cfg = new CfgMonster();
		// cfg.name = "Dark Hunter";
		// cfg.lvl = 2;
		// cfg.exp = 31;
		// cfg.hp[0] = 5; cfg.hp[1] = 9;
		// cfg.walkSpeed = 5;
		// cfg.runSpeed = 5;
		// cfg.attack[0] = 1; cfg.attack[1] = 3;
		// cfg.attackRating = 12;

		// cfg = new CfgMonster();
		// cfg.name = "Vile Hunter";
		// cfg.lvl = 5;
		// cfg.exp = 54;
		// cfg.hp[0] = 10; cfg.hp[1] = 17;
		// cfg.walkSpeed = 6;
		// cfg.runSpeed = 8;
		// cfg.attack[0] = 2; cfg.attack[1] = 5;
		// cfg.attackRating = 31;
	}
}
