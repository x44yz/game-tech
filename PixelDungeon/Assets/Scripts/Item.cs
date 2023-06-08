using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public bool levelKnown = false;
    public bool cursedKnown = false;
    protected int quantity = 1;

    public Item identify()
    {
		levelKnown = true;
		cursedKnown = true;
		
		return this;
    }

	public bool collect() {
		return collect( Main.Inst.hero.inventory );
	}

    public bool collect(Inventory inventory)
    {
        inventory.AddItem(this);
        return true;
    }
}

public class Armor
{
    public int STR;
    public int DR()
    {
        return 0;
    }

	public int proc( Actor attacker, Actor defender, int damage ) {
		
		// if (glyph != null) {
		// 	damage = glyph.proc( this, attacker, defender, damage );
		// }
		
		// if (!levelKnown) {
		// 	if (--hitsToKnow <= 0) {
		// 		levelKnown = true;
		// 		GLog.w( TXT_IDENTIFY, name(), toString() );
		// 		Badges.validateItemLevelAquired( this );
		// 	}
		// }
		
		// use();
		
		return damage;
	}
}

public class Ring
{

}