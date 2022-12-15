using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public enum d2UnitType
    {
        Player,
        Monster,
    }

    public enum MonsterMode 
    {
        Stand,
        /** Movement towards N, NW, or NE */
        MoveNorthwards,
        /** Movement towards S, SW, or SE */
        MoveSouthwards,
        /** Movement towards W or E */
        MoveSideways,
        MeleeAttack,
        HitRecovery,
        Death,
        SpecialMeleeAttack,
        FadeIn,
        FadeOut,
        RangedAttack,
        SpecialStand,
        SpecialRangedAttack,
        Delay,
        Charge,
        Petrified, // 石化
        Heal,
        Talk,
    };

    public enum HeroClass
    {
        Warrior,
        Rogue,
        Sorcerer,
        Monk,
        Bard,
        Barbarian,

        LAST = Barbarian
    };

    [Flags]
    public enum ItemSpecialEffect
    {
        // clang-format off
        None                   = 0,
        RandomStealLife        = 1 << 1,
        RandomArrowVelocity    = 1 << 2,
        FireArrows             = 1 << 3,
        FireDamage             = 1 << 4,
        LightningDamage        = 1 << 5,
        DrainLife              = 1 << 6,
        MultipleArrows         = 1 << 9,
        Knockback              = 1 << 11,
        StealMana3             = 1 << 13,
        StealMana5             = 1 << 14,
        StealLife3             = 1 << 15,
        StealLife5             = 1 << 16,
        QuickAttack            = 1 << 17,
        FastAttack             = 1 << 18,
        FasterAttack           = 1 << 19,
        FastestAttack          = 1 << 20,
        FastHitRecovery        = 1 << 21,
        FasterHitRecovery      = 1 << 22,
        FastestHitRecovery     = 1 << 23,
        FastBlock              = 1 << 24,
        LightningArrows        = 1 << 25,
        Thorns                 = 1 << 26,
        NoMana                 = 1 << 27,
        HalfTrapDamage         = 1 << 28,
        TripleDemonDamage      = 1 << 30,
        ZeroResistance         = 1 << 31,
        // clang-format on
    };

    public enum ItemType
    {
        Misc,
        Sword,
        Axe,
        Bow,
        Mace,
        Shield,
        LightArmor,
        Helm,
        MediumArmor,
        HeavyArmor,
        Staff,
        Gold,
        Ring,
        Amulet,
        None = -1,
    };

    // Logical equipment locations
    public enum inv_body_loc 
    {
        INVLOC_HEAD,
        INVLOC_RING_LEFT,
        INVLOC_RING_RIGHT,
        INVLOC_AMULET,
        INVLOC_HAND_LEFT,
        INVLOC_HAND_RIGHT,
        INVLOC_CHEST,
        NUM_INVLOC,
    };

    public class d2Unit : Unit
    {
        public const int BaseHitChance = 50;
        public const bool gbIsHellfire = false;

        [Header("---- D2UNIT ----")]
        public d2UnitType unitType;
        public int charId;
        public int weaponId;
        public int armorId;
        public int armorClass;

        [Header("---- PLAYER ----")]
        HeroClass _pClass;
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
        /** @brief Bitmask using item_special_effect */
        public ItemSpecialEffect _pIFlags;
        public d2Item[] InvBody = new d2Item[(int)inv_body_loc.NUM_INVLOC];

        [Header("---- MONSTER ----")]
        public MonsterMode mode;

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnHit(Unit attacker)
        {
            base.OnHit(attacker);

            var d2Attacker = attacker as d2Unit;
            if (d2Attacker.unitType == d2UnitType.Player && unitType == d2UnitType.Monster)
                PlayerHitMonster(d2Attacker, this);
            else
                Debug.LogError("not implement onhit > " + d2Attacker.unitType + " - " + unitType);
        }

        public int GenerateRnd(int maxExclusive) 
        {
            return UnityEngine.Random.Range(0, maxExclusive);
        }

        // adjacentDamage: 临近伤害，可以理解为溅射伤害
        private bool PlayerHitMonster(d2Unit player, d2Unit monster, bool adjacentDamage = false)
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
            
            int hit = GenerateRnd(100);
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

            if (gbIsHellfire && HasAllOf(((int)player._pIFlags), (int)(ItemSpecialEffect.FireDamage | ItemSpecialEffect.LightningDamage))) 
            {
                int midam = player._pIFMinDam + GenerateRnd(player._pIFMaxDam - player._pIFMinDam);
                AddMissile(player.position.tile, player.position.temp, player._pdir, MIS_SPECARROW, TARGET_MONSTERS, player.getId(), midam, 0);
            }
            int mind = player._pIMinDam;
            int maxd = player._pIMaxDam;
            int dam = GenerateRnd(maxd - mind + 1) + mind;
            dam += dam * player._pIBonusDam / 100;
            dam += player._pIBonusDamMod;
            int dam2 = dam << 6;
            dam += player._pDamageMod;
            if (player._pClass == HeroClass.Warrior || player._pClass == HeroClass.Barbarian) 
            {
                if (GenerateRnd(100) < player._pLevel) 
                {
                    dam *= 2;
                }
            }

            ItemType phanditype = ItemType.None;
            if (player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType::Sword || player.InvBody[INVLOC_HAND_RIGHT]._itype == ItemType::Sword) {
                phanditype = ItemType::Sword;
            }
            if (player.InvBody[INVLOC_HAND_LEFT]._itype == ItemType::Mace || player.InvBody[INVLOC_HAND_RIGHT]._itype == ItemType::Mace) {
                phanditype = ItemType::Mace;
            }

            switch (monster.data().monsterClass) 
            {
            case MonsterClass::Undead:
                if (phanditype == ItemType::Sword) {
                    dam -= dam / 2;
                } else if (phanditype == ItemType::Mace) {
                    dam += dam / 2;
                }
                break;
            case MonsterClass::Animal:
                if (phanditype == ItemType::Mace) {
                    dam -= dam / 2;
                } else if (phanditype == ItemType::Sword) {
                    dam += dam / 2;
                }
                break;
            case MonsterClass::Demon:
                if (HasAnyOf(player._pIFlags, ItemSpecialEffect.TripleDemonDamage)) {
                    dam *= 3;
                }
                break;
            }

            if (HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf::Devastation) && GenerateRnd(100) < 5) {
                dam *= 3;
            }

            if (HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf::Doppelganger) && monster.type().type != MT_DIABLO && !monster.isUnique() && GenerateRnd(100) < 10) {
                AddDoppelganger(monster);
            }

            dam <<= 6;
            if (HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf::Jesters)) {
                int r = GenerateRnd(201);
                if (r >= 100)
                    r = 100 + (r - 100) * 5;
                dam = dam * r / 100;
            }

            if (adjacentDamage)
                dam >>= 2;

            if (&player == MyPlayer) {
                if (HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf::Peril)) {
                    dam2 += player._pIGetHit << 6;
                    if (dam2 >= 0) {
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
            if (HasAnyOf(player._pIFlags, ItemSpecialEffect.RandomStealLife)) {
                skdam = GenerateRnd(dam / 8);
                player._pHitPoints += skdam;
                if (player._pHitPoints > player._pMaxHP) {
                    player._pHitPoints = player._pMaxHP;
                }
                player._pHPBase += skdam;
                if (player._pHPBase > player._pMaxHPBase) {
                    player._pHPBase = player._pMaxHPBase;
                }
                RedrawComponent(PanelDrawComponent::Health);
            }
            if (HasAnyOf(player._pIFlags, ItemSpecialEffect.StealMana3 | ItemSpecialEffect.StealMana5) && HasNoneOf(player._pIFlags, ItemSpecialEffect.NoMana)) {
                if (HasAnyOf(player._pIFlags, ItemSpecialEffect.StealMana3)) {
                    skdam = 3 * dam / 100;
                }
                if (HasAnyOf(player._pIFlags, ItemSpecialEffect.StealMana5)) {
                    skdam = 5 * dam / 100;
                }
                player._pMana += skdam;
                if (player._pMana > player._pMaxMana) {
                    player._pMana = player._pMaxMana;
                }
                player._pManaBase += skdam;
                if (player._pManaBase > player._pMaxManaBase) {
                    player._pManaBase = player._pMaxManaBase;
                }
                RedrawComponent(PanelDrawComponent::Mana);
            }
            if (HasAnyOf(player._pIFlags, ItemSpecialEffect.StealLife3 | ItemSpecialEffect.StealLife5)) {
                if (HasAnyOf(player._pIFlags, ItemSpecialEffect.StealLife3)) {
                    skdam = 3 * dam / 100;
                }
                if (HasAnyOf(player._pIFlags, ItemSpecialEffect.StealLife5)) {
                    skdam = 5 * dam / 100;
                }
                player._pHitPoints += skdam;
                if (player._pHitPoints > player._pMaxHP) {
                    player._pHitPoints = player._pMaxHP;
                }
                player._pHPBase += skdam;
                if (player._pHPBase > player._pMaxHPBase) {
                    player._pHPBase = player._pMaxHPBase;
                }
                RedrawComponent(PanelDrawComponent::Health);
            }
            if ((monster.hitPoints >> 6) <= 0) {
                M_StartKill(monster, player);
            } else {
                if (monster.mode != MonsterMode::Petrified && HasAnyOf(player._pIFlags, ItemSpecialEffect.Knockback))
                    M_GetKnockback(monster);
                M_StartHit(monster, player, dam);
            }
            return true;
        }

        public bool HasAllOf(int lhs, int test)
        {
            return (lhs & test) == test;
        }

        public bool isPossibleToHit
        {
            // TODO
            get { return true; }
        }

        // 近战伤害
        public int GetMeleeToHit()
        {
            int hper = _pLevel + _pDexterity / 2 + _pIBonusToHit + BaseHitChance;
            if (_pClass == HeroClass.Warrior)
                hper += 20;
            return hper;
        }

        // armor piercing: 穿甲
        public int GetMeleePiercingToHit()
        {
            int hper = GetMeleeToHit();
            // in hellfire armor piercing ignores % of enemy armor instead, no way to include it here
            if (!gbIsHellfire)
                hper += _pIEnAc;
            return hper;
        }

        // 返回怪物的护甲（在 hellfire 模式计算穿甲之后）
        public int CalculateArmorPierce(int monsterArmor, bool isMelee)
        {
            int tmac = monsterArmor;
            if (_pIEnAc > 0) {
                if (gbIsHellfire) {
                    int pIEnAc = _pIEnAc - 1;
                    if (pIEnAc > 0)
                        tmac >>= pIEnAc;
                    else
                        tmac -= tmac / 4;
                }
                if (isMelee && _pClass == HeroClass.Barbarian) {
                    tmac -= monsterArmor / 8;
                }
            }
            if (tmac < 0)
                tmac = 0;

            return tmac;
        }

        public bool tryLiftGargoyle()
        {
            // TODO
            return false;
        }
    }
}

