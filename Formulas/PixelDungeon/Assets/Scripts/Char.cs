
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Char : Actor
{
    public int HT; // max hp
    public int HP;

	private HashSet<Buff> _buffs = new HashSet<Buff>();

	public static bool hit( Char attacker, Char defender, bool magic ) {
		float acuRoll = Random.Float( attacker.attackSkill( defender ) );
		float defRoll = Random.Float( defender.defenseSkill( attacker ) );
        // 魔法攻击成功概率更高  
		return (magic ? acuRoll * 2 : acuRoll) >= defRoll;
	}

    public bool attack(Char enemy)
    {
        if (hit(this, enemy, false))
        {
            // 伤害减免 damage reduce
            // TODO:如果目标是 hero & 远程武器 & sniper 命中概率 100%
            int dr = Random.IntRange(0, enemy.dr());

            int dmg = damageRoll();
            // 先计算减伤
            int effectiveDamage = Mathf.Max(dmg - dr, 0);
            effectiveDamage = attackProc(enemy, effectiveDamage);
            effectiveDamage = enemy.defenseProc( this, effectiveDamage );
            // 承受具体伤害
            enemy.damage( effectiveDamage, this );

            // 如果敌人是玩家，玩家状态被打断，同时过大伤害造成屏幕闪烁
			// if (enemy == Dungeon.hero) {
			// 	Dungeon.hero.interrupt();
			// 	if (effectiveDamage > enemy.HT / 4) {
			// 		Camera.main.shake( GameMath.gate( 1, effectiveDamage / (enemy.HT / 4), 5), 0.3f );
			// 	}
			// }
            return true;
        }
        else
        {
            // 处理闪避成功的提示和声音
            return false;
        }
    }

    // 面对目标攻击到的概率
	public virtual int attackSkill( Char target ) {
		return 0;
	}
	
    // 面对敌人防御到的概率
	public virtual int defenseSkill( Char enemy ) {
		return 0;
	}
	
    public virtual int dr() {
		return 0;
	}
	
    public virtual int damageRoll() {
		return 1;
	}

    public virtual int attackProc( Char enemy, int damage ) {
		return damage;
	}

	public virtual int defenseProc( Char enemy, int damage ) {
		return damage;
	}

    public void damage( int dmg, Entity src ) {
		
		if (HP <= 0) {
			return;
		}
		
		// Buff.detach( this, Frost.class );
		
		// Class<?> srcClass = src.getClass();
		// if (immunities().contains( srcClass )) {
		// 	dmg = 0;
		// } else if (resistances().contains( srcClass )) {
		// 	dmg = Random.IntRange( 0, dmg );
		// }
		
		// if (buff( Paralysis.class ) != null) {
		// 	if (Random.Int( dmg ) >= Random.Int( HP )) {
		// 		Buff.detach( this, Paralysis.class );
		// 		if (Dungeon.visible[pos]) {
		// 			GLog.i( TXT_OUT_OF_PARALYSIS, name );
		// 		}
		// 	}
		// }
		
		HP -= dmg;
		// if (dmg > 0 || src is Char) {
		// 	sprite.showStatus( HP > HT / 2 ? 
		// 		CharSprite.WARNING : 
		// 		CharSprite.NEGATIVE,
		// 		Integer.toString( dmg ) );
		// }
		if (HP <= 0) {
			die( src );
		}
	}

	public void destroy() {
		HP = 0;
		// Actor.remove( this );
		// Actor.freeCell( pos );
	}
	
	public void die( Entity src ) {
		destroy();
		// sprite.die();
	}

	public HashSet<T> buffs<T>() where T : Buff {
		HashSet<T> filtered = new HashSet<T>();
		foreach (Buff b in _buffs) {
			if (b is T) {
				filtered.Add( (T)b );
			}
		}
		return filtered;
	}
}