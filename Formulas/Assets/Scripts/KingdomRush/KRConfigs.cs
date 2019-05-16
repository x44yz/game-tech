
public class KRCfgTower
{
	public string id;
	public string name;
	public int level;
	public int buildCost;
	public int minDamage;
	public int maxDamage;
	public float attackRate;
	public int scope;
	public string[] upgrades;
}

public static class KRConfigs
{
	public static KRCfgTower[] towers = new KRCfgTower[]{
			new KRCfgTower(){id="archer", name="Archer Tower", level=1, buildCost=70, minDamage=4, maxDamage=6, attackRate=0.8, scope=280 },
		};
}