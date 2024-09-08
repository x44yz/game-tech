// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public enum MobId
// {
//     NONE,
//     BAT,
//     RAT,
// }

// public class Mob : Actor
// {
//     public MobId mobId;
// 	public int EXP = 1;
// 	public int maxLvl = 30;
//     public bool enemySeen = true;

//     public void Init(MobId mobId)
//     {
//         this.mobId = mobId;
    
//         if (mobId == MobId.BAT)
//         {
//             HP = HT = 30;
//             defVal = 15;
//             // baseSpeed = 2f;
            
//             EXP = 7;
//             maxLvl = 15;
            
//             // flying = true;
            
//             // loot = new PotionOfHealing();
//             // lootChance = 0.125f;
//         }
//         else if (mobId == MobId.RAT)
//         {
//             HP = HT = 8;
//             defVal = 3;
//             maxLvl = 5;
//         }
//         else
//         {
//             Debug.LogError("not implement mob id > " + mobId);
//         }
//     }

// 	public override int attackSkill( Actor target ) {
// 		if (mobId == MobId.RAT) return 8;
//         Debug.LogError("not implement attackSkill > " + mobId);
//         return 0;
// 	}

// 	public override int defenseSkill( Actor enemy ) {
//         // 看见敌人&可移动
// 		return enemySeen && !paralysed ? defVal : 0;
// 	}

//     public override int dr() {
//         if (mobId == MobId.RAT) return 1;
//         else if (mobId == MobId.BAT) return 4;
//         Debug.LogError("not implement dr > " + mobId);
//         return 0;
//     }

//     public override int damageRoll()
//     {
//         if (mobId == MobId.RAT) return Random.NormalIntRange( 1, 5 );
//         Debug.LogError("not implement damgeRoll > " + mobId);
//         return 0;
//     }
// }
