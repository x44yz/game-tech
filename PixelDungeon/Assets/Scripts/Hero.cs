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
    private int atkVal = 10;
	private int defVal = 5;
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

    public int STR {
        set { str = value; }
        get {
		    return weakened ? str - 2 : str;
        }
	}

    public void Init(int initLvl)
    {
        inventory = new Inventory();

        for (int i = this.lvl; i < initLvl; ++i)
            LevelUp();

        if (heroClass == HeroClass.WARRIOR)
            initWarrior(this);
    }

	private static void initWarrior( Hero hero ) {
		hero.STR = hero.STR + 1;
		
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
		// awareness = (float)(1 - Math.pow( 
		// 	(heroClass == HeroClass.ROGUE ? 0.85 : 0.90), 
		// 	(1 + Math.min( lvl,  9 )) * 0.5 
		// ));
        throw new System.NotImplementedException();
	}
}
