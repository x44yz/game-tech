
public class Armor : EquipableItem
{
    public int tier; // 等级
    public int STR;

    // 伤害减免
	public int DR() {
        return tier * (2 + effectiveLevel());
		// return tier * (2 + effectiveLevel() + (glyph == null ? 0 : 1));
	}
}