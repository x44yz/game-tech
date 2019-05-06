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
	public enum Status
	{
		STAND,
		WALK,
		ATTACK,
		BLOCK,
		GOTHIT,
		DEATH,
	}

	// NOTE:
	// 实际的 Cooldown 应该与攻击动画有关
//	public const float TIME_ATK_COOLDOWN = 2;

	// public int strength;		// 力量
	// public int magic;				// 意志
	// public int dexterity;		// 敏捷
	// public int vitality;		// 活力

	// public int hp;
	// public int mana;
	// public PlayerStats stats;

// 	private bool bAttack = false;
//	private float atkCdTick = 0f;

	public Status status = Status.STAND;
	public int enac;	// enhanced accuracy

	public int pclass;
	public int strength;
	public int magic;
	public int dexterity;
	public int vitality;
	public int baseStrength;
	public int baseMagic;
	public int baseDexterity;
	public int baseVitality;
	public int level;
	public int maxLevel;
	public int damage;
	// hp 与 vitality 有关
	public int hp;
	public int maxHP;
	public int baseHP;
	public int baseMaxHP;
	// mana 与 magic 有关
	public int mana;
	public int maxMana;
	public int baseMana;
	public int baseMaxMana;
	public int exp;
	public int maxExp;
	public int bonusToHit;
	public int minDamage;
	public int maxDamage;
	public int damageMod;
	public int bonusDamageMod;
	public int bonusDamage;

	public int itemMinDamage;
	public int itemMaxDamage;
	public int itemBonusDamage;
	public int itemBonusDamageMod;
	public int itemBonusToHit;

	// 
	public int blockFrames;

	public Item[] invBody = new Item[(int)InvBodyLoc.INVLOC_COUNT];

	public Player Create(int pc)
	{
		pclass = pc;

		strength = PlayerConfig.baseAttributes[pc, PlayerAttr.STR];
		baseStrength = strength;

		magic = PlayerConfig.baseAttributes[pc, PlayerAttr.MAG];
		baseMagic = magic;

		dexterity = PlayerConfig.baseAttributes[pc, PlayerAttr.DEX];
		baseDexterity = dexterity;

		vitality = PlayerConfig.baseAttributes[pc, PlayerAttr.VIT];
		baseVitality = vitality;

		//
		level = 1;
		maxLevel = 1;
		exp = 0;
		maxExp = 0;

		if (pclass == PlayerClass.Rogue)
			damage = level * (strength + dexterity) / 200;
		else
			damage = strength * level / 100;

		// hp
		hp = (vitality + 10) << 6;
		if (pclass == PlayerClass.Warrior)
			hp *= 2;
		else if (pclass == PlayerClass.Rogue)
			hp += hp >> 1;

		maxHP = hp;
		baseHP = hp;
		baseMaxHP = hp;

		// mana
		mana = magic << 6;
		if (pclass == PlayerClass.Sorcerer)
			mana *= 2;
		else if (pclass == PlayerClass.Rogue)
			mana += mana >> 1;

		maxMana = mana;
		baseMana = mana;
		baseMaxMana = mana;

		return null;
	}

	// http://bfed2.diablomods.ru/site/index.php?page=gm_damage
	// Final Damage = (((Normal Damge x 1.5)(only if ethereal)
	//							x(1 + Enhanced Damage on Weapon/100) + Bonuses to Minimum/Maximum Damage)
	//							x(1 + Strength or Dexterity/100 + Off-Weapon Enhanced Damage/100 + Skill Damage Bonus%/100)+Elemental Damage)
	//							x(1 - Skill Damage Penalty/100)
	//							x2(only if a critical or deadly strike is scored)

	protected override void Start() 
	{
		base.Start();

		faceDir = FaceDir.RIGHT;
	}

	protected override void Update() 
	{
		if (status == Status.STAND)
		{
			DoStand();
		}
		else if (status == Status.ATTACK)
		{
			DoAttack(target);
		}
		else if (status == Status.BLOCK)
		{
			DoBlock();
		}
		else if (status == Status.GOTHIT)
		{
			DoGotHit();
		}
		else if (status == Status.DEATH)
		{
			DoDeath();
		}

		// TODO:
		// Validate Player
	}

	private bool DoStand()
	{
		return false;
	}

	private bool DoAttack(Actor enemy)
	{
		// play sound
		// play ani
		// PlayAnimation("Attack");
		// state = State.Attack;
		// ani.PlayAnimation(ActorAniState.Attack);

		// if (CheckHitTarget(target))
		// {
		// 	target.TakeDamage(damage);
		// 	target.CheckDeath();
		// }
		// aniframe == num
		if (true) 
		{
			if (target != null)
			{

			}
		}

		return false;
	}

	private bool DoBlock()
	{
		// TODO:
		// if anim frame >= blockFrames
		// 其实就是 anim 结束
		StartStand();
		
	}

	private bool DoGotHit()
	{
		// check got hit anim frame end
		StartStand();
		
		return false;
	}

	private bool DoDeath()
	{
		return false;
	}

	public void StartStand()
	{
		
	}

	public void StartAttack()
	{
		status = Status.ATTACK;
	}

	private bool HitMonster(Monster mt)
	{
		int hper = dexterity >> 1 + level + 50 - (mt.armorClass - enac);
		if (pclass == PlayerClass.Warrior)
		{
			hper += 20;
		}

		hper += bonusToHit;
		if (hper < 5)
			hper = 5;
		if (hp > 95)
			hper = 95;

		int hit = Utils.Rand(4, 100);
		if (hit < hper)
		{
			int damage = minDamage + Utils.Rand(5, maxDamage - minDamage + 1);
			damage += damageMod + bonusDamageMod + damage * bonusDamage / 100;
			if (pclass == PlayerClass.Warrior)
			{
				// 6 级之后才有暴击
				if (Utils.Rand(6, 100) < level)
					damage *= 2;
			}

			// TODO:
			// damage 修正，比如装备对某种怪物有暴击

			// TODO:
			// 倍乘 6 次原因？
			int skdam = damage << 6;
			mt.hp -= skdam;

			// TODO:
			// handle steal hp or mana

			// 倍乘 6 是否将 float > int 计算? 
			if ((mt.hp >> 6) <= 0)
			{
				// 需要马上更新状态，否则 monster 逻辑会在下一帧执行
				mt.StartKill();
			}
			else
			{
				mt.StartHit(skdam);
			}
		}

		return true;
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

	public void SetStrength(int v)
	{

	}

	// 计算 Item 对角色属性的影响
	public void CalcItemVals()
	{
		int mind = 0; // min damage
		int maxd = 0; // max damage

		int bdam = 0; // bonus damage
		int btohit = 0; // bonus chance to hit
		int bac = 0; // bonus accuracy
		// TODO:
		// accuracy 精准度，影响甚么？
		int dmod = 0; // bonus damage mod

		for (int i = 0; i < (int)InvBodyLoc.INVLOC_COUNT; ++i)
		{
			Item it = invBody[i];
			if (it.type == ItemType.NONE)
				continue;

			mind += it.minDamage;
			maxd += it.maxDamage;

			// TODO:
			// 为什么要限定 Normal
			if (it.quality == ItemQuality.NORMAL)
			{
				bdam += it.plDamage;
				btohit += it.plToHit;
				dmod += it.plDamageMod;
			}
		}

		// TODO:
		// 确保 > 0 的原因是？
		if (mind == 0 && maxd == 0)
		{
			mind = 1;
			maxd = 1;
		}

		//
		itemMinDamage = mind;
		itemMaxDamage = maxd;
		itemBonusDamage = bdam;
		itemBonusToHit = btohit;
		itemBonusDamageMod = dmod;

		CalcDamageMod();
	}

	private void CalcDamageMod()
	{
		int dam = 0;
		if (pclass == PlayerClass.Rogue)
			dam = level * (strength + dexterity) / 200;
		else
			dam  = level * strength / 100;
		damageMod = dam;
	}
}
