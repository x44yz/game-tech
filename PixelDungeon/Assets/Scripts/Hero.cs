using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Actor
{
    private int attackSkill = 10;
	private int defenseSkill = 5;
    public int lvl = 1;
	public int exp = 0;

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
		
		KindOfWeapon wep = rangedWeapon != null ? rangedWeapon : belongings.weapon;
		if (wep != null) {
			return (int)(attackSkill * accuracy * wep.acuracyFactor( this ));
		} else {
			return (int)(attackSkill * accuracy);
		}
	}

	public void earnExp( int exp ) {
		
		this.exp += exp;
		
		boolean levelUp = false;
		while (this.exp >= maxExp()) {
			this.exp -= maxExp();
			lvl++;
			
			HT += 5;
			HP += 5;			
			attackSkill++;
			defenseSkill++;
			
			if (lvl < 10) {
				updateAwareness();
			}
			
			levelUp = true;
		}
		
		if (levelUp) {
			
			GLog.p( TXT_NEW_LEVEL, lvl );
			sprite.showStatus( CharSprite.POSITIVE, TXT_LEVEL_UP );
			Sample.INSTANCE.play( Assets.SND_LEVELUP );
			
			Badges.validateLevelReached();
		}
		
		if (subClass == HeroSubClass.WARLOCK) {
			
			int value = Math.min( HT - HP, 1 + (Dungeon.depth - 1) / 5 );
			if (value > 0) {
				HP += value;
				sprite.emitter().burst( Speck.factory( Speck.HEALING ), 1 );
			}
			
			((Hunger)buff( Hunger.class )).satisfy( 10 );
		}
	}
	
	public int maxExp() {
		return 5 + lvl * 5;
	}
	
}
