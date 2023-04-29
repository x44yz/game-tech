// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using NaughtyAttributes;

// namespace angband
// {
//     public class angWeapon : MonoBehaviour
//     {
//         [Dropdown("GetWeaponName")]
//         public string weaponName;
//         public static List<string> weaponNames = null;
//         private List<string> GetWeaponName()
//         {
//             if (weaponNames == null)
//             {
//                 weaponNames = new List<string>();
//                 foreach (var wp in CWeaponInfo.weaponInfos)
//                 {
//                     weaponNames.Add(wp.m_Name);
//                 }
//             }
//             return weaponNames;
//         }

//         [Header("RUNTIME")]
//         public eWeaponType m_eWeaponType;
//         public eWeaponFire m_eWeaponFire;
//         public int m_nDamage;

//         private void Start() 
//         {
//             var wpInfo = CWeaponInfo.GetWeaponInfo(weaponName);
//             // m_eWeaponType = wpInfo.m_eWeaponFire
//             m_eWeaponFire = wpInfo.m_eWeaponFire;
//             m_nDamage = wpInfo.m_nDamage;
//         }

//         public bool Fire(angPlayer shooter, Vector3 fireSource)
//         {
//             if (m_eWeaponFire == eWeaponFire.WEAPON_FIRE_MELEE)
//                 FireMelee(shooter, fireSource);
//             else
//                 Debug.LogError("not implement weapon fire > " + m_eWeaponFire);

//             return true;
//         }

//         public bool FireMelee(angPlayer shooter, Vector3 fireSource)
//         {
//             // 遍历所有附近的对象
//             for (int i = 0; i < shooter.m_numNearPeds; i++)
//             {
//                 var victimPed = shooter.m_nearPeds[i];
//                 // 满足以下条件：
//                 // pedType 不同 | 正在寻找的目标 | 不是 leader | 1 / 31 概率
//                 // 检查是否在射击范围内
//                 // 对方非玩家并且不在起身PED_GETUP保护状态下
//                 {
//                     var isBat = m_eWeaponType == eWeaponType.WEAPONTYPE_BASEBALLBAT;
//                     bool anim2Playing = false; // 是否是二段动画，用于棒球棒重击判断
//                     int localDir = 0;

//                     if (shooter.IsPlayer() && isBat && anim2Playing)
//                         victimPed.InflictDamage(shooter, m_eWeaponType, 100.0f, ePedPieceTypes.PEDPIECE_TORSO, localDir); 
//                     else if (shooter.IsPlayer() && shooter.m_bAdrenalineActive)
//                         victimPed.InflictDamage(shooter, m_eWeaponType, 3.5f * m_nDamage, ePedPieceTypes.PEDPIECE_TORSO, localDir);
//                     else
//                     {
//                         // 对玩家使用棒球棒伤害加倍
//                         if (victimPed.IsPlayer() && isBat ) // wtf, it's not fair
//                             victimPed.InflictDamage(shooter, m_eWeaponType, 2.0f*m_nDamage, ePedPieceTypes.PEDPIECE_TORSO, localDir);
//                         else
//                             victimPed.InflictDamage(shooter, m_eWeaponType, m_nDamage, ePedPieceTypes.PEDPIECE_TORSO, localDir);
//                     }
//                 }
//             }
//             return true;
//         }
//     }
// }
