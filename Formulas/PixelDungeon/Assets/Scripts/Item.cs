
public class Item
{
    private int level = 0;
	private int durability;

    public Item()
    {
        durability = maxDurability();
    }

	public int effectiveLevel() {
		return isBroken() ? 0 : level;
	}

	public bool isBroken() {
		return durability <= 0;
	}

	public int maxDurability( int lvl ) {
		return 1;
	}
	
	public int maxDurability() {
		return maxDurability( level );
	}
}

public class EquipableItem : Item
{

}

public class KindOfWeapon : EquipableItem
{
    // 精度
	public virtual float acuracyFactor( Hero hero ) {
		return 1f;
	}

	public int damageRoll( Hero owner ) {
	    // return Random.NormalIntRange( min(), max() );
        return 1;
	}
}

public class Weapon : KindOfWeapon
{

}

public class MissileWeapon : Weapon
{

}