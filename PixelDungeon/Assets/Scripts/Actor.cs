using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;
using QuickDemo.FSM;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Actor : MonoBehaviour
{
    public int HT; // max hp
	public int HP;
    public int atkVal;
	public int defVal;

	public bool paralysed	= false; // 不能行动的
	public bool rooted		= false;
	public bool flying		= false;	

    public bool attack( Actor enemy ) {
		
		// boolean visibleFight = Dungeon.visible[pos] || Dungeon.visible[enemy.pos];
		
		if (hit( this, enemy, false )) {
			Debug.Log("xx-- success attack");
			
			// if (visibleFight) {
			// 	GLog.i( TXT_HIT, name, enemy.name );
			// }
			
			// FIXME
			// 伤害减免
			int dr = Random.IntRange( 0, enemy.dr() );
			// if (this is Hero && ((Hero)this).rangedWeapon != null && ((Hero)this).subClass == HeroSubClass.SNIPER ? 0 :
				// Random.IntRange( 0, enemy.dr() );
			
			int dmg = damageRoll();
			int effectiveDamage = Mathf.Max( dmg - dr, 0 );
			
			effectiveDamage = attackProc( enemy, effectiveDamage );
			effectiveDamage = enemy.defenseProc( this, effectiveDamage );
			// enemy.damage( effectiveDamage, this );
			
			// if (visibleFight) {
			// 	Sample.INSTANCE.play( Assets.SND_HIT, 1, 1, Random.Float( 0.8f, 1.25f ) );
			// }

			// if (enemy == Dungeon.hero) {
			// 	Dungeon.hero.interrupt();
			// 	if (effectiveDamage > enemy.HT / 4) {
			// 		Camera.main.shake( GameMath.gate( 1, effectiveDamage / (enemy.HT / 4), 5), 0.3f );
			// 	}
			// }
			
			// enemy.sprite.bloodBurstA( sprite.center(), effectiveDamage );
			// enemy.sprite.flash();
			
			// if (!enemy.isAlive() && visibleFight) {
			// 	if (enemy == Dungeon.hero) {
					
			// 		if (Dungeon.hero.killerGlyph != null) {
						
			// 		// FIXME
			// 		//	Dungeon.fail( Utils.format( ResultDescriptions.GLYPH, Dungeon.hero.killerGlyph.name(), Dungeon.depth ) );
			// 		//	GLog.n( TXT_KILL, Dungeon.hero.killerGlyph.name() );
						
			// 		} else {
			// 			if (Bestiary.isBoss( this )) {
			// 				Dungeon.fail( Utils.format( ResultDescriptions.BOSS, name, Dungeon.depth ) );
			// 			} else {
			// 				Dungeon.fail( Utils.format( ResultDescriptions.MOB, 
			// 					Utils.indefinite( name ), Dungeon.depth ) );
			// 			}
						
			// 			GLog.n( TXT_KILL, name );
			// 		}
					
			// 	} else {
			// 		GLog.i( TXT_DEFEAT, name, enemy.name );
			// 	}
			// }
			
			return true;
			
		} else {
			Debug.Log("xx-- attack miss");
			
			// if (visibleFight) {
			// 	String defense = enemy.defenseVerb();
			// 	enemy.sprite.showStatus( CharSprite.NEUTRAL, defense );
			// 	if (this == Dungeon.hero) {
			// 		GLog.i( TXT_YOU_MISSED, enemy.name, defense );
			// 	} else {
			// 		GLog.i( TXT_SMB_MISSED, enemy.name, defense, name );
			// 	}
				
			// 	Sample.INSTANCE.play( Assets.SND_MISS );
			// }
			
			return false;
		}
	}

	public static bool hit( Actor attacker, Actor defender, bool magic ) {
		float acuRoll = Random.Float( attacker.attackSkill( defender ) );
		float defRoll = Random.Float( defender.defenseSkill( attacker ) );
		return (magic ? acuRoll * 2 : acuRoll) >= defRoll;
	}
	
	public virtual int attackSkill( Actor target ) {
		return 0;
	}
	
	public virtual int defenseSkill( Actor enemy ) {
		return 0;
	}

	public virtual int dr(){
		return 0;
	}

	public virtual int damageRoll() {
		return 1;
	}

	public virtual int attackProc( Actor enemy, int damage ) {
		return damage;
	}
	
	public virtual int defenseProc( Actor enemy, int damage ) {
		return damage;
	}
}

