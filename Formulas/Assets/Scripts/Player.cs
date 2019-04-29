using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass
{
	public const int Warrior = 0;
	public const int Rogue = 1;
	public const int Sorcerer = 2;
}

public class PlayerAttr
{
	public const int STR = 0; // strength
	public const int MAG = 1; // magic
	public const int DEX = 2; // dexterity
	public const int VIT = 3; // vitality
}

public class Player : Actor
{
	// NOTE:
	// 实际的 Cooldown 应该与攻击动画有关
//	public const float TIME_ATK_COOLDOWN = 2;

	// public int strength;		// 力量
	// public int magic;				// 意志
	// public int dexterity;		// 敏捷
	// public int vitality;		// 活力

	// public int hp;
	// public int mana;
	public PlayerStats stats;

// 	private bool bAttack = false;
//	private float atkCdTick = 0f;

	public Player Create(int pc)
	{
		stats.pclass = pc;

		stats.strength = PlayerConfig.baseAttributes[pc, PlayerAttr.STR];
		stats.baseStrength = stats.strength;

		stats.magic = PlayerConfig.baseAttributes[pc, PlayerAttr.MAG];
		stats.baseMagic = stats.magic;

		stats.dexterity = PlayerConfig.baseAttributes[pc, PlayerAttr.DEX];
		stats.baseDexterity = stats.dexterity;

		stats.vitality = PlayerConfig.baseAttributes[pc, PlayerAttr.VIT];
		stats.baseVitality = stats.vitality;

		//
		stats.level = 1;
		stats.maxLevel = 1;
		stats.exp = 0;
		stats.maxExp = 0;

		if (stats.pclass == PlayerClass.Rogue)
			stats.damage = stats.level * (stats.strength + stats.dexterity) / 200;
		else
			stats.damage = stats.strength * stats.level / 100;

		// hp
		stats.hp = (stats.vitality + 10) << 6;
		if (stats.pclass == PlayerClass.Warrior)
			stats.hp *= 2;
		else if (stats.pclass == PlayerClass.Rogue)
			stats.hp += stats.hp >> 1;

		stats.maxHP = stats.hp;
		stats.baseHP = stats.hp;
		stats.baseMaxHP = stats.hp;

		// mana
		stats.mana = stats.magic << 6;
		if (stats.pclass == PlayerClass.Sorcerer)
			stats.mana *= 2;
		else if (stats.pclass == PlayerClass.Rogue)
			stats.mana += stats.mana >> 1;

		stats.maxMana = stats.mana;
		stats.baseMana = stats.mana;
		stats.baseMaxMana = stats.mana;

		return null;
	}

	// http://bfed2.diablomods.ru/site/index.php?page=gm_damage
	// Final Damage = (((Normal Damge x 1.5)(only if ethereal)
	//							x(1 + Enhanced Damage on Weapon/100) + Bonuses to Minimum/Maximum Damage)
	//							x(1 + Strength or Dexterity/100 + Off-Weapon Enhanced Damage/100 + Skill Damage Bonus%/100)+Elemental Damage)
	//							x(1 - Skill Damage Penalty/100)
	//							x2(only if a critical or deadly strike is scored)
	public int damage { 
		get {

			return 5;
		} 
	}

	protected override void Start() 
	{
		base.Start();

		faceDir = FaceDir.RIGHT;
	}

	protected override void Update() 
	{
		if (state == State.Normal)
		{
			if (Input.GetKeyDown(KeyCode.J) && CanAttack(target))
			{
				DoAttack(target);
			}
		}
		else if (state == State.Attack)
		{
			Debug.Assert(target, "CHECK: target cant be null.");
			if (ani.curAniState != ActorAniState.Attack)
				state = State.Normal;
		}

		// if (bAttack)
		// {
		// 	atkCdTick += dt;
		// 	if (atkCdTick >= TIME_ATK_COOLDOWN)
		// 	{
		// 		bAttack = false;
		// 		atkCdTick = 0f;
		// 	}
		// }
	}

	public void DoAttack(Actor enemy)
	{
		// play sound
		// play ani
		// PlayAnimation("Attack");
		state = State.Attack;
		ani.PlayAnimation(ActorAniState.Attack);

		if (CheckHitTarget(target))
		{
			target.TakeDamage(damage);
			target.CheckDeath();
		}
	}

	public bool CanAttack(Actor enemy)
	{
		if (enemy == null)
			return false;

		// 检查距离和自身，敌方情况
		return true;
	}

	// 命中率
	public bool CheckHitTarget(Actor enemy)
	{
		// TODO
		// 计算敌人闪避
		return true;
	}
}
