
// https://dash-quest.fandom.com/wiki/Primary_stats
public class dpPlayerStatsStruct
{
	int DPS; // damage per second 
	int DEF; // reduce incoming damage
	int SPD; // move speed
	int INT; // 智力 increase spell power
	int LCK; // luck affects random things
	int ATK; // attack speed of your weapon
	int RNG; // the range of your weapon
	int CRIT; // 暴击 %; the critical chance of your weapon
	int DODGE; // 闪避
	int RESIST; // 抗腐蚀，抵抗疾病
}

// base attributes
public class dpWeaponBA
{
	int reqLvl;
	int DAM;
	int ATK;
	int RNG;
}

// extra attributes
public class dpWeaponEA
{
	int HP;
	int MP;
	int DEF;
	int SPD;
	int INT;
	int CRIT; // %
	int DODGE; // %
	int RESIST; // %
}

public class dpArmorBA
{
	int reqLvl;
	int DEF;
	int HP;
	int SPD;
}

public class dpArmorEA
{
	int MP;
	int DAM;
	int INT;
	int ATK;
	int CRIT; // %
	int DODGE; // %
	int RESIST; // %
}