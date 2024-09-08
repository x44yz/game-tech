using System;
using UnityEngine;

public class Hero : Char
{
    public int _STR;
    public int lvl = 1;
    public int exp = 0;
	private int _attackSkill = 10;
	private int _defenseSkill = 5;

    public MissileWeapon rangedWeapon = null;

    public Belongings belongings;
    public bool weakened = false;

	public int STR() {
		return weakened ? _STR - 2 : _STR;
	}

    private void Awake() 
    {
        belongings = new Belongings(this);
    }

    // 攻击
    public override int attackSkill( Char target ) 
    {
		int bonus = 0;
        // buff 添加精度  
		foreach (Buff buff in buffs<RingOfAccuracyBuff>()) {
			bonus += ((RingOfAccuracyBuff)buff).level;
		}
		float accuracy = (bonus == 0) ? 1 : (float)Mathf.Pow( 1.4f, bonus );
        // 如果远程武器近战使用，精度减半
		// if (rangedWeapon != null && Level.distance( pos, target.pos ) == 1) {
		// 	accuracy *= 0.5f;
		// }
		
		KindOfWeapon wep = rangedWeapon != null ? rangedWeapon : belongings.weapon;
		if (wep != null) {
			return (int)(_attackSkill * accuracy * wep.acuracyFactor( this ));
		} else {
			return (int)(_attackSkill * accuracy);
		}
	}

    // 
    public override int defenseSkill( Char enemy ) 
    {
		int bonus = 0;
        // buff + 僵住的时候防御减半
		// for (Buff buff : buffs( RingOfEvasion.Evasion.class )) {
		// 	bonus += ((RingOfEvasion.Evasion)buff).level;
		// }
        // 闪避
		float evasion = bonus == 0 ? 1 : (float)Mathf.Pow( 1.2f, bonus );
		// if (paralysed) {
		// 	evasion /= 2;
		// }
		
		int aEnc = belongings.armor != null ? belongings.armor.STR - STR() : 0;
		
        // 如果护甲超重，防御力失衡
		if (aEnc > 0) {
			return (int)(_defenseSkill * evasion / Mathf.Pow( 1.5f, aEnc ));
		} 
        else {
			
            // 如果是盗贼，护甲越低，闪避越高
			// if (heroClass == HeroClass.ROGUE) {
				
			// 	if (curAction != null && subClass == HeroSubClass.FREERUNNER && !isStarving()) {
			// 		evasion *= 2;
			// 	}
				
			// 	return (int)((_defenseSkill - aEnc) * evasion);
			// } else {
				return (int)(_defenseSkill * evasion);
			// }
		}
	}

    // 伤害减免 damage reduce
	public override int dr() {
		int dr = belongings.armor != null ? Mathf.Max( belongings.armor.DR(), 0 ) : 0;
		// 硬化皮肤
        // Barkskin barkskin = buff( Barkskin.class );
		// if (barkskin != null) {
		// 	dr += barkskin.level();
		// }
		return dr;
	}

    // 武器随机伤害
	public override int damageRoll() {
		KindOfWeapon wep = rangedWeapon != null ? rangedWeapon : belongings.weapon;
		int dmg;
		if (wep != null) {	
			dmg = wep.damageRoll( this );
		} else {
			dmg = STR() > 10 ? Random.IntRange( 1, STR() - 9 ) : 1;
		}
        // 狂暴状态伤害加深
		// return buff( Fury.class ) != null ? (int)(dmg * 1.5f) : dmg;
        return dmg;
	}
}