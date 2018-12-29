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

public class Player : Actor
{
	// NOTE:
	// 实际的 Cooldown 应该与攻击动画有关
	public const float TIME_ATK_COOLDOWN = 2;

	public int strength;		// 力量
	public int magic;				// 意志
	public int dexterity;		// 敏捷
	public int vitality;		// 活力

	public int hp;
	public int mana;

	public Actor target = null;

	private bool bAttack = false;
	private float atkCdTick = 0f;

	// public static Player Create(PlayerClass pc)
	// {
	// 	return null;
	// }

	private void Update() 
	{
		float dt = Time.deltaTime;

		if (target != null)
		{
			// attack
			if (Input.GetKeyDown(KeyCode.J) && !bAttack)
			{
				if (CanAttack(target))
				{
					bAttack = true;
					atkCdTick = 0f;

					Attack(target);
				}
			}
		}

		if (bAttack)
		{
			atkCdTick += dt;
			if (atkCdTick >= TIME_ATK_COOLDOWN)
			{
				bAttack = false;
				atkCdTick = 0f;
			}
		}
	}

	public int GetDamage()
	{
		return 5;
	}

	public void Attack(Actor enemy)
	{
		// play sound
		// play ani
		PlayAnimation("Attack");

		if (CheckAttack(enemy))
		{
			enemy.TakeDamage(GetDamage());
			enemy.CheckDeath();
		}
	}

	public bool CanAttack(Actor enemy)
	{
		// 检查距离和自身，敌方情况
		return true;
	}

	public bool CheckAttack(Actor enemy)
	{
		// TODO
		// 计算敌人闪避
		return true;
	}
}
