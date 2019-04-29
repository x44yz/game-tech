using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://diablo2.diablowiki.net/Diablo_Characters
public static class PlayerConfig
{
	public static readonly int[,] baseAttributes = new int[,]
	{
		// STR MAG DEX VIT
		{30, 10, 20, 25},	// Warrior
		{20, 15, 30, 20},	// Rogue
		{15, 35, 15, 20}	// Sorcerer
	};

	public static readonly int[,] maxAttributes = new int[,] 
	{
		// STR MAG DEX VIT
		{250, 50, 60, 100},	// Warrior
		{55, 70, 250, 80},	// Rogue
		{45, 250, 85, 80}	// Sorcerer
	};

	public static readonly string[] images = new string[]
	{
		"",
	};
}
