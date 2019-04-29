using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
	Warrior,
	Rogue,
	Sorcerer
}

public enum PlayerAttr : byte
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

	public Player Create(PlayerClass pc)
	{
		stats.pclass = pc;

		stats.strength = PlayerConfig.baseAttributes[pc][PlayerAttr.STR];
		stats.baseStrength = stats.strength;

		stats.magic = PlayerConfig.baseAttributes[pc][PlayerAttr.MAG];
		stats.baseMagic = stats.magic;

		stats.dexterity = PlayerConfig.baseAttributes[pc][PlayerAttr.DEX];
		stats.baseDexterity = stats.dexterity;

		stats.vitality = PlayerConfig.baseAttributes[pc][PlayerAttr.VIT];
		stats.baseVitality = stats.vitality;

		if (pc == PlayerClass.Rogue)
			stats.damage = stats.level * (stats.strength + stats.dev) / 200;
		else
			stats.damage = stats.strength * stats.level / 100;

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
