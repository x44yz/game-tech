using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
	public float acuracyFactor( Hero hero ) {
		throw new System.NotImplementedException();
		// int encumbrance = STR - hero.STR();
		
		// if (this instanceof MissileWeapon) {
		// 	switch (hero.heroClass) {
		// 	case WARRIOR:
		// 		encumbrance += 3;
		// 		break;
		// 	case HUNTRESS:
		// 		encumbrance -= 2;
		// 		break;
		// 	default:
		// 	}
		// }
		
		// return 
		// 	(encumbrance > 0 ? (float)(ACU / Math.pow( 1.5, encumbrance )) : ACU) *
		// 	(imbue == Imbue.ACCURACY ? 1.5f : 1.0f);
	}

	public virtual int min() { return 0; }
	public virtual int max() { return 0; }

	public int damageRoll( Hero hero ) {
		
		int damage = Random.NormalIntRange( min(), max() );
		
		// if ((hero.rangedWeapon != null) == (hero.heroClass == HeroClass.HUNTRESS)) {
		// 	int exStr = hero.STR() - STR;
		// 	if (exStr > 0) {
		// 		damage += Random.IntRange( 0, exStr );
		// 	}
		// }
		
		return damage;
	}

	public void proc( Actor attacker, Actor defender, int damage ) {
		
		// if (enchantment != null) {
		// 	enchantment.proc( this, attacker, defender, damage );
		// }
		
		// if (!levelKnown) {
		// 	if (--hitsToKnow <= 0) {
		// 		levelKnown = true;
		// 		GLog.i( TXT_IDENTIFY, name(), toString() );
		// 		Badges.validateItemLevelAquired( this );
		// 	}
		// }
		
		// use();
	}
}

public class ShortSword : Weapon
{

}

public class MissileWeapon : Weapon
{

}

public class Dart : MissileWeapon
{
    public Dart() {
		quantity = 1;
	}
	
	public Dart( int number ) {
		quantity = number;
	}
}