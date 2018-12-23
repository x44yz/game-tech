using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
	Warrior,
	Rogue,
	Sorcerer
}

public enum PlayerAttribute : byte
{
	STR = 0,	// strength
	MAG = 1,	// magic
	DEX = 2,	// dexterity
	VIT = 3,	// vitality
}

public class Player 
{
	public int strength;	// 力量
	public int magic;	// 意志
	public int dexterity;	// 敏捷
	public int vitality; // 活力

	public int hp;
	public int mana;

	public static Player Create(PlayerClass pc)
	{
		return null;
	}
}
