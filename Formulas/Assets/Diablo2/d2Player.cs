using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public class d2Player : Unit
    {
        [Header("---- D2UNIT ----")]
        public int charId;
        public int weaponId;
        public int armorId;

        [Header("---- PLAYER ----")]
        public HeroClass _pClass;
        public int _pLevel;
        public int _pDexterity; // 敏捷
        public int _pIEnAc;
        public int _pIMinDam;
        public int _pIMaxDam;
        public int _pIBonusDam;
        public int _pIBonusToHit;
        public int _pIBonusAC;
        public int _pIBonusDamMod;
        public int _pDamageMod;
        public int _pIFMinDam;
        public int _pIFMaxDam;
        public int _pILMinDam;
        public int _pILMaxDam;
        public int _pIGetHit;
        public int _pMaxHPBase;
        public int _pHitPoints;
        public int _pMaxHP;
        public int _pHPBase;
        public int _pManaBase;
        public int _pMaxManaBase;
        public int _pMana;
        public int _pMaxMana;
        public int _pManaPer;
        /** @brief Bitmask using item_special_effect */
        public ItemSpecialEffect _pIFlags;
        public ItemSpecialEffectHf pDamAcFlags;
        public d2Item[] InvBody = new d2Item[(int)inv_body_loc.NUM_INVLOC];

        protected override void OnStart()
        {
            base.OnStart();

            var objInvBody = new GameObject("invbody");
            objInvBody.transform.SetParent(transform);
            for (int i = 0; i < ((int)inv_body_loc.NUM_INVLOC); ++i)
            {
                var item = d2Item.Create(objInvBody.transform);
                item.name = ((inv_body_loc)i).ToString();
                InvBody[i] = item;
            }
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        public override void HitTarget(Unit target)
        {
            base.HitTarget(target);
            Debug.Log("xx-- d2Player.HitTarget");

            var mon = target as d2Monster;
            if (mon != null)
                PlayerHitMonster(this, mon);
        }

        // 近战伤害
        public int GetMeleeToHit()
        {
            int hper = _pLevel + _pDexterity / 2 + _pIBonusToHit + d2DEF.BaseHitChance;
            if (_pClass == HeroClass.Warrior)
                hper += 20;
            return hper;
        }

        // armor piercing: 穿甲
        public int GetMeleePiercingToHit()
        {
            int hper = GetMeleeToHit();
            // in hellfire armor piercing ignores % of enemy armor instead, no way to include it here
            if (!d2DEF.gbIsHellfire)
                hper += _pIEnAc;
            return hper;
        }

        // 返回怪物的护甲（在 hellfire 模式计算穿甲之后）
        public int CalculateArmorPierce(int monsterArmor, bool isMelee)
        {
            int tmac = monsterArmor;
            if (_pIEnAc > 0) 
            {
                if (d2DEF.gbIsHellfire) 
                {
                    int pIEnAc = _pIEnAc - 1;
                    if (pIEnAc > 0)
                        tmac >>= pIEnAc;
                    else
                        tmac -= tmac / 4;
                }
                if (isMelee && _pClass == HeroClass.Barbarian) 
                {
                    tmac -= monsterArmor / 8;
                }
            }
            if (tmac < 0)
                tmac = 0;

            return tmac;
        }

        // adjacentDamage: 临近伤害，可以理解为溅射伤害
        public bool PlayerHitMonster(d2Player player, d2Monster monster, bool adjacentDamage = false)
        {
            int hper = 0;
            
            if (!monster.isPossibleToHit)
                return false;

            if (adjacentDamage)
            {
                if (player._pLevel > 20)
                    hper -= 30;
                else
                    hper -= (35 - player._pLevel) * 2;
            }
            
            int hit = d2Utils.GenerateRnd(100);
            if (monster.mode == MonsterMode.Petrified) 
            {
                hit = 0;
            }

            hper += player.GetMeleePiercingToHit() - player.CalculateArmorPierce(monster.armorClass, true);
            hper = Mathf.Clamp(hper, 5, 95);

            if (monster.tryLiftGargoyle())
            {
                return true;
            }

            if (hit >= hper) {
        #if _DEBUG
                if (!DebugGodMode)
        #endif
                return false;
            }

            if (d2DEF.gbIsHellfire && d2Utils.HasAllOf(player._pIFlags, ItemSpecialEffect.FireDamage | ItemSpecialEffect.LightningDamage)) 
            {
                int midam = player._pIFMinDam + d2Utils.GenerateRnd(player._pIFMaxDam - player._pIFMinDam);
                // AddMissile(player.position.tile, player.position.temp, player._pdir, MIS_SPECARROW, TARGET_MONSTERS, player.getId(), midam, 0);
            }

            int mind = player._pIMinDam;
            int maxd = player._pIMaxDam;
            int dam = d2Utils.GenerateRnd(maxd - mind + 1) + mind;
            dam += dam * player._pIBonusDam / 100;
            dam += player._pIBonusDamMod;
            int dam2 = dam << 6;
            dam += player._pDamageMod;
            
            if (player._pClass == HeroClass.Warrior || player._pClass == HeroClass.Barbarian) 
            {
                if (d2Utils.GenerateRnd(100) < player._pLevel) 
                {
                    dam *= 2;
                }
            }

            ItemType phanditype = ItemType.None;
            if (player.InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._itype == ItemType.Sword || player.InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._itype == ItemType.Sword) {
                phanditype = ItemType.Sword;
            }
            if (player.InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._itype == ItemType.Mace || player.InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._itype == ItemType.Mace) {
                phanditype = ItemType.Mace;
            }

            switch (monster.data.monsterClass) 
            {
            case MonsterClass.Undead:
                if (phanditype == ItemType.Sword) {
                    dam -= dam / 2;
                } else if (phanditype == ItemType.Mace) {
                    dam += dam / 2;
                }
                break;
            case MonsterClass.Animal:
                if (phanditype == ItemType.Mace) {
                    dam -= dam / 2;
                } else if (phanditype == ItemType.Sword) {
                    dam += dam / 2;
                }
                break;
            case MonsterClass.Demon:
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.TripleDemonDamage)) {
                    dam *= 3;
                }
                break;
            }

            if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.Devastation) && d2Utils.GenerateRnd(100) < 5) {
                dam *= 3;
            }

            // if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.Doppelganger) && monster.type().type != MT_DIABLO && !monster.isUnique() && d2Utils.GenerateRnd(100) < 10) 
            // {
            //     AddDoppelganger(monster);
            // }

            dam <<= 6;
            if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.Jesters)) 
            {
                int r = d2Utils.GenerateRnd(201);
                if (r >= 100)
                    r = 100 + (r - 100) * 5;
                dam = dam * r / 100;
            }

            if (adjacentDamage)
                dam >>= 2;

            // 本地玩家
            // if (&player == MyPlayer) 
            {
                if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.Peril)) 
                {
                    dam2 += player._pIGetHit << 6;
                    if (dam2 >= 0)
                    {
                        ApplyPlrDamage(player, 0, 1, dam2);
                    }
                    dam *= 2;
                }
        #if _DEBUG
                if (DebugGodMode) {
                    dam = monster.hitPoints; /* ensure monster is killed with one hit */
                }
        #endif
                ApplyMonsterDamage(monster, dam);
            }

            int skdam = 0;
            if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.RandomStealLife)) 
            {
                skdam = d2Utils.GenerateRnd(dam / 8);
                player._pHitPoints += skdam;
                if (player._pHitPoints > player._pMaxHP) {
                    player._pHitPoints = player._pMaxHP;
                }
                player._pHPBase += skdam;
                if (player._pHPBase > player._pMaxHPBase) {
                    player._pHPBase = player._pMaxHPBase;
                }
                // RedrawComponent(PanelDrawComponent::Health);
            }

            // 窃取魔法效果
            if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealMana3 | ItemSpecialEffect.StealMana5) && d2Utils.HasNoneOf(player._pIFlags, ItemSpecialEffect.NoMana)) 
            {
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealMana3)) {
                    skdam = 3 * dam / 100;
                }
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealMana5)) {
                    skdam = 5 * dam / 100;
                }
                player._pMana += skdam;
                if (player._pMana > player._pMaxMana) 
                {
                    player._pMana = player._pMaxMana;
                }
                player._pManaBase += skdam;
                if (player._pManaBase > player._pMaxManaBase) 
                {
                    player._pManaBase = player._pMaxManaBase;
                }
                // RedrawComponent(PanelDrawComponent::Mana);
            }

            // 窃取 hp 
            if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealLife3 | ItemSpecialEffect.StealLife5)) 
            {
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealLife3)) {
                    skdam = 3 * dam / 100;
                }
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealLife5)) {
                    skdam = 5 * dam / 100;
                }
                player._pHitPoints += skdam;
                if (player._pHitPoints > player._pMaxHP) 
                {
                    player._pHitPoints = player._pMaxHP;
                }
                player._pHPBase += skdam;
                if (player._pHPBase > player._pMaxHPBase) 
                {
                    player._pHPBase = player._pMaxHPBase;
                }
                // RedrawComponent(PanelDrawComponent::Health);
            }

            // 播放特效
            if ((monster.hitPoints >> 6) <= 0)
            {
                // M_StartKill(monster, player);
            } 
            else 
            {
                // if (monster.mode != MonsterMode.Petrified && d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.Knockback))
                //     M_GetKnockback(monster);
                // M_StartHit(monster, player, dam);
            }
            return true;
        }

        void ApplyPlrDamage(d2Player player, int dam, int minHP = 0, int frac = 0, int earflag = 0)
        {
            // int totalDamage = (dam << 6) + frac;
            // if (totalDamage > 0 && player.pManaShield) 
            // {
            //     int8_t manaShieldLevel = player._pSplLvl[SPL_MANASHIELD];
            //     if (manaShieldLevel > 0) {
            //         totalDamage += totalDamage / -player.GetManaShieldDamageReduction();
            //     }
            //     if (&player == MyPlayer)
            //         RedrawComponent(PanelDrawComponent::Mana);
            //     if (player._pMana >= totalDamage) {
            //         player._pMana -= totalDamage;
            //         player._pManaBase -= totalDamage;
            //         totalDamage = 0;
            //     } else {
            //         totalDamage -= player._pMana;
            //         if (manaShieldLevel > 0) {
            //             totalDamage += totalDamage / (player.GetManaShieldDamageReduction() - 1);
            //         }
            //         player._pMana = 0;
            //         player._pManaBase = player._pMaxManaBase - player._pMaxMana;
            //         if (&player == MyPlayer)
            //             NetSendCmd(true, CMD_REMSHIELD);
            //     }
            // }

            // if (totalDamage == 0)
            //     return;

            // RedrawComponent(PanelDrawComponent::Health);
            // player._pHitPoints -= totalDamage;
            // player._pHPBase -= totalDamage;
            // if (player._pHitPoints > player._pMaxHP) {
            //     player._pHitPoints = player._pMaxHP;
            //     player._pHPBase = player._pMaxHPBase;
            // }
            // int minHitPoints = minHP << 6;
            // if (player._pHitPoints < minHitPoints) {
            //     SetPlayerHitPoints(player, minHitPoints);
            // }
            // if (player._pHitPoints >> 6 <= 0) {
            //     SyncPlrKill(player, earflag);
            // }
        }

        void ApplyMonsterDamage(d2Monster monster, int damage)
        {
            monster.hitPoints -= damage;

            if (monster.hitPoints >> 6 <= 0) 
            {
                // delta_kill_monster(monster, monster.position.tile, *MyPlayer);
                // NetSendCmdLocParam1(false, CMD_MONSTDEATH, monster.position.tile, monster.getId());
                return;
            }

            // delta_monster_hp(monster, *MyPlayer);
            // NetSendCmdMonDmg(false, monster.getId(), damage);
        }
    }
}

