using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Engine.Actor, ICombat
{
	// -- Define some basic combat stats
	public int combat_armor = 0;

	// -- Default regen
	// t.power_regen = t.power_regen or 1
	// t.life_regen = t.life_regen or 0.25 -- Life regen real slow

	// -- Default melee barehanded damage
	public int? combat_dam = 1;
}
