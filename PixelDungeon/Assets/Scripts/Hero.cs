using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroClass
{
    NONE,
    WARRIOR,
    MAGE,
    ROGUE,
    HUNTRESS
}

public enum HeroSubClass
{
    NONE,
    GLADIATOR,
    BERSERKER,
    WARLOCK,
    BATTLEMAGE,
    ASSASSIN,
    FREERUNNER,
    SNIPER,
    WARDEN,
}

public class Hero : Actor
{

    public int lvl = 1;
	public int exp = 0;
    public int str = 10;
    public Weapon weapon;
    public float awareness = 0.1f;
    public HeroClass heroClass = HeroClass.ROGUE;
	public HeroSubClass subClass = HeroSubClass.NONE;
    public bool weakened;
	public Armor armor = null;
	public Ring ring1 = null;
	public Ring ring2 = null;
    public Inventory inventory;

    public int STR() {
		return weakened ? str - 2 : str;
	}

    public void Init(int initLvl)
    {
        inventory = new Inventory();

    	atkVal = 10;
		defVal = 5;

        for (int i = this.lvl; i < initLvl; ++i)
            LevelUp();

        if (heroClass == HeroClass.WARRIOR)
            initWarrior(this);
    }

	private static void initWarrior( Hero hero ) {
		hero.str = hero.str + 1;
		
		(hero.weapon = new ShortSword()).identify();
		new Dart( 8 ).identify().collect();
		
		// QuickSlot.primaryValue = Dart.class;
		
		new PotionOfStrength().setKnown();
	}

    public override int attackSkill( Actor target ) {
		
		int bonus = 0;
		// for (Buff buff : buffs( RingOfAccuracy.Accuracy.class )) {
		// 	bonus += ((RingOfAccuracy.Accuracy)buff).level;
		// }

        // 准确性
		float accuracy = (bonus == 0) ? 1 : (float)Mathf.Pow( 1.4f, bonus );
        // 如果是远程武器，并且只有一格距离
		// if (rangedWeapon != null && Level.distance( pos, target.pos ) == 1) {
		// 	accuracy *= 0.5f;
		// }
		
		// KindOfWeapon wep = rangedWeapon != null ? rangedWeapon : belongings.weapon;
		if (weapon != null) {
			return (int)(atkVal * accuracy * weapon.acuracyFactor( this ));
		} else {
			return (int)(defVal * accuracy);
		}
	}

	public override int defenseSkill( Actor enemy ) {
		
		int bonus = 0;
		// for (Buff buff : buffs( RingOfEvasion.Evasion.class )) {
		// 	bonus += ((RingOfEvasion.Evasion)buff).level;
		// }
		// 闪避
		float evasion = bonus == 0 ? 1 : (float)Mathf.Pow( 1.2f, bonus );
		if (paralysed) {
			evasion /= 2;
		}
		
		int aEnc = armor != null ? armor.STR - STR() : 0;
		
		// 重甲
		if (aEnc > 0) {
			return (int)(defVal * evasion / Mathf.Pow( 1.5f, aEnc ));
		} else {
			
			if (heroClass == HeroClass.ROGUE) {
				
				// if (curAction != null && subClass == HeroSubClass.FREERUNNER && !isStarving()) {
				// 	evasion *= 2;
				// }
				
				return (int)((defVal - aEnc) * evasion);
			} else {
				return (int)(defVal * evasion);
			}
		}
	}

    public void LevelUp() {
        lvl++;
        HT += 5;
        HP += 5;			
        atkVal++;
        defVal++;

        if (lvl < 10) {
            updateAwareness();
        }
    }

	public void earnExp( int exp ) {
		
		this.exp += exp;
		
		bool levelUp = false;
		while (this.exp >= maxExp()) {
			this.exp -= maxExp();

            LevelUp();

			levelUp = true;
		}
		
		if (levelUp) {
			
			// GLog.p( TXT_NEW_LEVEL, lvl );
			// sprite.showStatus( CharSprite.POSITIVE, TXT_LEVEL_UP );
			// Sample.INSTANCE.play( Assets.SND_LEVELUP );
			
			// Badges.validateLevelReached();
		}
		
		// if (subClass == HeroSubClass.WARLOCK) {
			
		// 	int value = Math.min( HT - HP, 1 + (Dungeon.depth - 1) / 5 );
		// 	if (value > 0) {
		// 		HP += value;
		// 		sprite.emitter().burst( Speck.factory( Speck.HEALING ), 1 );
		// 	}
			
		// 	((Hunger)buff( Hunger.class )).satisfy( 10 );
		// }
	}
	
	public int maxExp() {
		return 5 + lvl * 5;
	}
	
	void updateAwareness() {
		awareness = (float)(1 - Mathf.Pow( 
			(heroClass == HeroClass.ROGUE ? 0.85f : 0.90f), 
			(1 + Mathf.Min( lvl,  9 )) * 0.5f 
		));
	}

	public override int dr() {
		int dr = armor != null ? Mathf.Max( armor.DR(), 0 ) : 0;
		// Barkskin barkskin = buff( Barkskin.class );
		// if (barkskin != null) {
		// 	dr += barkskin.level();
		// }
		return dr;
	}
	
	public override int damageRoll() {
		// KindOfWeapon wep = rangedWeapon != null ? rangedWeapon : belongings.weapon;
		int dmg;
		if (weapon != null) {	
			dmg = weapon.damageRoll( this );
		} else {
			dmg = STR() > 10 ? Random.IntRange( 1, STR() - 9 ) : 1;
		}
		// return buff( Fury.class ) != null ? (int)(dmg * 1.5f) : dmg;
		return dmg;
	}

	public override int attackProc( Actor enemy, int damage ) {
		// KindOfWeapon wep = rangedWeapon != null ? rangedWeapon : belongings.weapon;
		var wep = weapon;
		if (wep != null) {
			
			wep.proc( this, enemy, damage );
			
			// switch (subClass) {
			// case HeroSubClass.GLADIATOR:
			// 	if (wep is MeleeWeapon) {
			// 		damage += Buff.affect( this, Combo.class ).hit( enemy, damage );
			// 	}
			// 	break;
			// case BATTLEMAGE:
			// 	if (wep instanceof Wand) {
			// 		Wand wand = (Wand)wep;
			// 		if (wand.curCharges >= wand.maxCharges) {
						
			// 			wand.use();
						
			// 		} else if (damage > 0) {
						
			// 			wand.curCharges++;
			// 			wand.updateQuickslot();
						
			// 			ScrollOfRecharging.charge( this );
			// 		}
			// 		damage += wand.curCharges;
			// 	}
			// case SNIPER:
			// 	if (rangedWeapon != null) {
			// 		Buff.prolong( this, SnipersMark.class, attackDelay() * 1.1f ).object = enemy.id();
			// 	}
			// 	break;
			// default:
			// 	break;
			// }
		}
		
		return damage;
	}

	public override int defenseProc( Actor enemy, int damage ) {
		
		RingOfThorns.Thorns thorns = buff( RingOfThorns.Thorns.class ); 
		if (thorns != null) {
			int dmg = Random.IntRange( 0, damage );
			if (dmg > 0) {
				enemy.damage( dmg, thorns );
			}
		}
		
		Earthroot.Armor armor = buff( Earthroot.Armor.class );
		if (armor != null) {
			damage = armor.absorb( damage );
		}
		
		if (belongings.armor != null) {
			damage = belongings.armor.proc( enemy, this, damage );
		}
		
		return damage;
	}
}
