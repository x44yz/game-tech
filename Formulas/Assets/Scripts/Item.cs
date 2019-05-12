
public enum ItemType
{
	NONE,
}

public enum ItemQuality
{
	NORMAL,
	MAGIC,
	UNIQUE,
}

public class Item
{
	public ItemType type = ItemType.NONE;
	public ItemQuality quality = ItemQuality.NORMAL;
	public int minDamage;
	public int maxDamage;
	// TODO:
	// pl = item effect > player ?
	public int plDamage;
	public int plDamageMod;
	public int plToHit;
	public int durability; // 耐久度
}