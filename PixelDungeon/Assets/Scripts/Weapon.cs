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
}
