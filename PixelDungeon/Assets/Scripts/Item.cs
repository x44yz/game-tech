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

}

public class Ring
{

}