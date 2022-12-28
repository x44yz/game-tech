using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public class f2Unit : Unit
    {
        public const int TRAITS_MAX_SELECTED_COUNT = 2;

        public static int FID_TYPE(int value) => ((value) & 0xF000000) >> 24;
        public static int PID_TYPE(int value) => (value) >> 24;
        public static int SID_TYPE(int value) => (value) >> 24;

        public static f2Unit obj_dude; // 当前选中的 unit
        public static f2Unit inven_dude = null; // 当前查看背包的 unit

        // List of selected traits.
        // 特征/天赋
        public static int[] pc_trait = new int[TRAITS_MAX_SELECTED_COUNT];
        // An array of perk ranks for each party member.
        public static PerkRankData perkLevelDataList = null;

        public int pid; // proto 配置 id
        public int fid;
        public ObjectData data;

        int protoGetProto(int pid, ref Proto protoPtr)
        {
            // TODO
            protoPtr = null;
            return 0;
            // *protoPtr = NULL;

            // if (pid == -1) {
            //     return -1;
            // }

            // if (pid == 0x1000000) {
            //     *protoPtr = (Proto*)&gDudeProto;
            //     return 0;
            // }

            // ProtoList* protoList = &(_protoLists[PID_TYPE(pid)]);
            // ProtoListExtent* protoListExtent = protoList->head;
            // while (protoListExtent != NULL) {
            //     for (int index = 0; index < protoListExtent->length; index++) {
            //         Proto* proto = (Proto*)protoListExtent->proto[index];
            //         if (pid == proto->pid) {
            //             *protoPtr = proto;
            //             return 0;
            //         }
            //     }
            //     protoListExtent = protoListExtent->next;
            // }

            // if (protoList->head != NULL && protoList->tail != NULL) {
            //     if (PROTO_LIST_EXTENT_SIZE * protoList->length - (PROTO_LIST_EXTENT_SIZE - protoList->tail->length) > PROTO_LIST_MAX_ENTRIES) {
            //         _proto_remove_some_list(PID_TYPE(pid));
            //     }
            // }

            // return _proto_load_pid(pid, protoPtr);
        }

        int critter_get_hits(f2Unit critter)
        {
            return PID_TYPE(critter.pid) == (int)ObjType.OBJ_TYPE_CRITTER ? critter.data.critter.hp : 0;
        }

        int critter_get_poison(f2Unit critter)
        {
            return PID_TYPE(critter.pid) == (int)ObjType.OBJ_TYPE_CRITTER ? critter.data.critter.poison : 0;
        }

        int critter_get_rads(f2Unit obj)
        {
            return PID_TYPE(obj.pid) == (int)ObjType.OBJ_TYPE_CRITTER ? obj.data.critter.radiation : 0;
        }

        int stat_get_base_direct(f2Unit critter, int stat)
        {
            if (stat >= 0 && stat < (int)Stat.SAVEABLE_STAT_COUNT) 
            {
                Proto proto = null;
                protoGetProto(critter.pid, ref proto);
                return proto.critter.data.baseStats[stat];
            } 
            else 
            {
                switch ((Stat)stat) {
                case Stat.STAT_CURRENT_HIT_POINTS:
                    return critter_get_hits(critter);
                case Stat.STAT_CURRENT_POISON_LEVEL:
                    return critter_get_poison(critter);
                case Stat.STAT_CURRENT_RADIATION_LEVEL:
                    return critter_get_rads(critter);
                }
            }

            return 0;
        }

        int stat_get_bonus(f2Unit critter, int stat)
        {
            if (stat >= 0 && stat < (int)Stat.SAVEABLE_STAT_COUNT) {
                Proto proto = null;
                protoGetProto(critter.pid, ref proto);
                return proto.critter.data.bonusStats[stat];
            }

            return 0;
        }

        // Returns `true` if the specified trait is selected.
        bool trait_level(int trait)
        {
            return pc_trait[0] == trait || pc_trait[1] == trait;
        }

        // Returns stat modifier depending on selected traits.
        int trait_adjust_stat(int stat)
        {
            int modifier = 0;

            switch ((Stat)stat) {
            case Stat.STAT_STRENGTH:
                if (trait_level((int)Trait.TRAIT_GIFTED)) {
                    modifier += 1;
                }
                if (trait_level((int)Trait.TRAIT_BRUISER)) {
                    modifier += 2;
                }
                break;
            case Stat.STAT_PERCEPTION:
                if (trait_level((int)Trait.TRAIT_GIFTED)) {
                    modifier += 1;
                }
                break;
            case Stat.STAT_ENDURANCE:
                if (trait_level((int)Trait.TRAIT_GIFTED)) {
                    modifier += 1;
                }
                break;
            case Stat.STAT_CHARISMA:
                if (trait_level((int)Trait.TRAIT_GIFTED)) {
                    modifier += 1;
                }
                break;
            case Stat.STAT_INTELLIGENCE:
                if (trait_level((int)Trait.TRAIT_GIFTED)) {
                    modifier += 1;
                }
                break;
            case Stat.STAT_AGILITY:
                if (trait_level((int)Trait.TRAIT_GIFTED)) {
                    modifier += 1;
                }
                if (trait_level((int)Trait.TRAIT_SMALL_FRAME)) {
                    modifier += 1;
                }
                break;
            case Stat.STAT_LUCK:
                if (trait_level((int)Trait.TRAIT_GIFTED)) {
                    modifier += 1;
                }
                break;
            case Stat.STAT_MAXIMUM_ACTION_POINTS:
                if (trait_level((int)Trait.TRAIT_BRUISER)) {
                    modifier -= 2;
                }
                break;
            case Stat.STAT_ARMOR_CLASS:
                if (trait_level((int)Trait.TRAIT_KAMIKAZE)) {
                    modifier -= stat_get_base_direct(obj_dude, (int)Stat.STAT_ARMOR_CLASS);
                }
                break;
            case Stat.STAT_MELEE_DAMAGE:
                if (trait_level((int)Trait.TRAIT_HEAVY_HANDED)) {
                    modifier += 4;
                }
                break;
            case Stat.STAT_CARRY_WEIGHT:
                if (trait_level((int)Trait.TRAIT_SMALL_FRAME)) {
                    modifier -= 10 * stat_get_base_direct(obj_dude, (int)Stat.STAT_STRENGTH);
                }
                break;
            case Stat.STAT_SEQUENCE:
                if (trait_level((int)Trait.TRAIT_KAMIKAZE)) {
                    modifier += 5;
                }
                break;
            case Stat.STAT_HEALING_RATE:
                if (trait_level((int)Trait.TRAIT_FAST_METABOLISM)) {
                    modifier += 2;
                }
                break;
            case Stat.STAT_CRITICAL_CHANCE:
                if (trait_level((int)Trait.TRAIT_FINESSE)) {
                    modifier += 10;
                }
                break;
            case Stat.STAT_BETTER_CRITICALS:
                if (trait_level((int)Trait.TRAIT_HEAVY_HANDED)) {
                    modifier -= 30;
                }
                break;
            case Stat.STAT_RADIATION_RESISTANCE:
                if (trait_level((int)Trait.TRAIT_FAST_METABOLISM)) {
                    modifier -= -stat_get_base_direct(obj_dude, (int)Stat.STAT_RADIATION_RESISTANCE);
                }
                break;
            case Stat.STAT_POISON_RESISTANCE:
                if (trait_level((int)Trait.TRAIT_FAST_METABOLISM)) {
                    modifier -= -stat_get_base_direct(obj_dude, (int)Stat.STAT_POISON_RESISTANCE);
                }
                break;
            }

            return modifier;
        }

        int stat_get_base(f2Unit critter, int stat)
        {
            int value = stat_get_base_direct(critter, stat);

            if (critter == obj_dude) {
                value += trait_adjust_stat(stat);
            }

            return value;
        }

        // 获得右手物品
        f2Item inven_right_hand(f2Unit critter)
        {
            int i;
            Inventory inventory;
            f2Item item;

            // if (i_rhand != NULL && critter == inven_dude) {
            //     return i_rhand;
            // }

            // 返回 hand item
            inventory = (critter.data.inventory);
            for (i = 0; i < inventory.length; i++) {
                item = inventory.items[i].item;
                if ((item.flags & (uint)ObjectFlags.OBJECT_IN_RIGHT_HAND) != 0) 
                {
                    return item;
                }
            }
            return null;
        }

        // 获得左手物品
        f2Item inven_left_hand(f2Unit critter)
        {
            int i;
            Inventory inventory;
            f2Item item;

            // if (i_lhand != NULL && critter == inven_dude) {
            //     return i_lhand;
            // }

            inventory = (critter.data.inventory);
            for (i = 0; i < inventory.length; i++) {
                item = inventory.items[i].item;
                if ((item.flags & (uint)ObjectFlags.OBJECT_IN_LEFT_HAND) != 0) {
                    return item;
                }
            }
            return null;
        }

        f2Item inven_worn(f2Unit critter)
        {
            int i;
            Inventory inventory;
            f2Item item;

            // if (i_worn != NULL && critter == inven_dude) {
            //     return i_worn;
            // }

            inventory = (critter.data.inventory);
            for (i = 0; i < inventory.length; i++) {
                item = inventory.items[i].item;
                if ((item.flags & (uint)ObjectFlags.OBJECT_WORN) != 0) 
                {
                    return item;
                }
            }
            return null;
        }

        // Calculates total weight of the items in inventory.
        int item_total_weight(f2Unit obj)
        {
            if (obj == null) {
                return 0;
            }

            int weight = 0;

            Inventory inventory = (obj.data.inventory);
            for (int index = 0; index < inventory.length; index++) 
            {
                InventoryItem inventoryItem = (inventory.items[index]);
                f2Item item = inventoryItem.item;
                weight += item.item_weight() * inventoryItem.quantity;
            }

            // TODO:
            // 上面不是已经加过 weight，下面又统计没有装备的，是否重复？
            if (FID_TYPE(obj.fid) == (int)ObjType.OBJ_TYPE_CRITTER) {
                f2Item item2 = inven_right_hand(obj);
                if (item2 != null) 
                {
                    // 不在右手
                    if ((item2.flags & (uint)ObjectFlags.OBJECT_IN_RIGHT_HAND) == 0) {
                        weight += item2.item_weight();
                    }
                }

                f2Item item1 = inven_left_hand(obj);
                if (item1 != null) {
                    if ((item1.flags & (uint)ObjectFlags.OBJECT_IN_LEFT_HAND) == 0) {
                        weight += item1.item_weight();
                    }
                }

                f2Item armor = inven_worn(obj);
                if (armor != null) 
                {
                    if ((armor.flags & (uint)ObjectFlags.OBJECT_WORN) == 0) 
                    {
                        weight += armor.item_weight();
                    }
                }
            }
            return weight;
        }

        bool isInCombat()
        {
            return f2Test.isInCombat();
        }

        // Returns true if perk is valid.
        static bool perkIsValid(int perk)
        {
            return perk >= 0 && perk < (int)Perk.PERK_COUNT;
        }

        public static int partyMemberMaxCount = 0;
        // List of party members, it's length is [partyMemberMaxCount] + 20.
        // static PartyMember partyMemberList = NULL;

        PerkRankData perkGetLevelData(f2Unit critter)
        {
            if (critter == obj_dude) {
                return perkLevelDataList;
            }

            // for (int index = 1; index < partyMemberMaxCount; index++) {
            //     if (critter.pid == partyMemberPidList[index]) {
            //         return perkLevelDataList + index;
            //     }
            // }

            Debug.LogError("Error: perkGetLevelData: Can't find party member match!");
            return perkLevelDataList;
        }

        // has_perk
        // perk 技能点
        bool perkHasRank(f2Unit critter, Perk perk)
        {
            return perk_level(critter, (int)perk) != 0;
        }

        int perk_level(f2Unit critter, int perk)
        {
            if (!perkIsValid(perk)) {
                return 0;
            }

            PerkRankData ranksData = perkGetLevelData(critter);
            return ranksData.ranks[perk];
        }

        public static f2Unit combat_turn_obj;
        public f2Unit combat_whose_turn()
        {
            if (isInCombat()) {
                return combat_turn_obj;
            } else {
                return null;
            }
        }

        int critterGetStat(f2Unit critter, int stat)
        {
            int value;
            if (stat >= 0 && stat < (int)Stat.SAVEABLE_STAT_COUNT) 
            {
                value = stat_get_base(critter, stat);
                value += stat_get_bonus(critter, stat);

                // 在基础值和 bonus 之外需要实时计算
                switch ((Stat)stat) {
                case Stat.STAT_PERCEPTION:
                    if ((critter.data.critter.combat.results & (int)Dam.DAM_BLIND) != 0) 
                    {
                        value -= 5;
                    }
                    break;
                case Stat.STAT_MAXIMUM_ACTION_POINTS:
                    // if (1) {
                        // 计算超重，1 weight / 40 == 1 ap
                        int remainingCarryWeight = critterGetStat(critter, (int)Stat.STAT_CARRY_WEIGHT) - item_total_weight(critter);
                        if (remainingCarryWeight < 0) {
                            value -= -remainingCarryWeight / 40 + 1;
                        }
                    // }
                    break;
                case Stat.STAT_ARMOR_CLASS:
                    if (isInCombat()) {
                        // 不在战斗的 unit
                        if (combat_whose_turn() != critter) {
                            int actionPointsMultiplier = 1;
                            int hthEvadeBonus = 0;

                            if (critter == obj_dude) {
                                if (perkHasRank(critter, Perk.PERK_HTH_EVADE)) {
                                    bool hasWeapon = false;

                                    f2Item item2 = inven_right_hand(critter);
                                    if (item2 != null) {
                                        if (item2.item_get_type() == (int)ItemType.ITEM_TYPE_WEAPON) {
                                            // TODO: 有什么武器是没动画的？
                                            if (item2.item_w_anim_code() != (int)WeaponAnimation.WEAPON_ANIMATION_NONE) {
                                                hasWeapon = true;
                                            }
                                        }
                                    }

                                    if (!hasWeapon) {
                                        f2Item item1 = inven_left_hand(critter);
                                        if (item1 != null) {
                                            if (item1.item_get_type() == (int)ItemType.ITEM_TYPE_WEAPON) {
                                                if (item1.item_w_anim_code() != (int)WeaponAnimation.WEAPON_ANIMATION_NONE) {
                                                    hasWeapon = true;
                                                }
                                            }
                                        }
                                    }

                                    // 如果没有武器
                                    if (!hasWeapon) {
                                        actionPointsMultiplier = 2;
                                        hthEvadeBonus = skill_level(critter, (int)Skill.SKILL_UNARMED) / 12;
                                    }
                                }
                            }
                            value += hthEvadeBonus;
                            value += critter.data.critter.combat.ap * actionPointsMultiplier;
                        }
                    }
                    break;
                case Stat.STAT_AGE:
                    value += f2Test.game_time() / f2DEF.GAME_TIME_TICKS_PER_YEAR;
                    break;
                }

                // 当前 dude
                if (critter == obj_dude) 
                {
                    switch ((Stat)stat) {
                    case Stat.STAT_STRENGTH:
                        if (perkHasRank(critter, Perk.PERK_GAIN_STRENGTH)) {
                            value++;
                        }

                        if (perkHasRank(critter, Perk.PERK_ADRENALINE_RUSH)) {
                            if (critterGetStat(critter, (int)Stat.STAT_CURRENT_HIT_POINTS) < (critterGetStat(critter, (int)Stat.STAT_MAXIMUM_HIT_POINTS) / 2)) {
                                value++;
                            }
                        }
                        break;
                    case Stat.STAT_PERCEPTION:
                        if (perkHasRank(critter, Perk.PERK_GAIN_PERCEPTION)) {
                            value++;
                        }
                        break;
                    case Stat.STAT_ENDURANCE:
                        if (perkHasRank(critter, Perk.PERK_GAIN_ENDURANCE)) {
                            value++;
                        }
                        break;
                    case Stat.STAT_CHARISMA:
                        // if (1) {
                            if (perkHasRank(critter, Perk.PERK_GAIN_CHARISMA)) {
                                value++;
                            }

                            bool hasMirrorShades = false;

                            f2Item item2 = inven_right_hand(critter);
                            if (item2 != null && item2.pid == (int)ProtoID.PROTO_ID_MIRRORED_SHADES) {
                                hasMirrorShades = true;
                            }

                            f2Item item1 = inven_left_hand(critter);
                            if (item1 != null && item1.pid == (int)ProtoID.PROTO_ID_MIRRORED_SHADES) {
                                hasMirrorShades = true;
                            }

                            if (hasMirrorShades) {
                                value++;
                            }
                        // }
                        break;
                    case Stat.STAT_INTELLIGENCE:
                        if (perkHasRank(critter, Perk.PERK_GAIN_INTELLIGENCE)) {
                            value++;
                        }
                        break;
                    case Stat.STAT_AGILITY:
                        if (perkHasRank(critter, Perk.PERK_GAIN_AGILITY)) {
                            value++;
                        }
                        break;
                    case Stat.STAT_LUCK:
                        if (perkHasRank(critter, Perk.PERK_GAIN_LUCK)) {
                            value++;
                        }
                        break;
                    case Stat.STAT_MAXIMUM_HIT_POINTS:
                        if (perkHasRank(critter, Perk.PERK_ALCOHOL_RAISED_HIT_POINTS)) {
                            value += 2;
                        }

                        if (perkHasRank(critter, Perk.PERK_ALCOHOL_RAISED_HIT_POINTS_II)) {
                            value += 4;
                        }

                        if (perkHasRank(critter, Perk.PERK_ALCOHOL_LOWERED_HIT_POINTS)) {
                            value -= 2;
                        }

                        if (perkHasRank(critter, Perk.PERK_ALCOHOL_LOWERED_HIT_POINTS_II)) {
                            value -= 4;
                        }

                        if (perkHasRank(critter, Perk.PERK_AUTODOC_RAISED_HIT_POINTS)) {
                            value += 2;
                        }

                        if (perkHasRank(critter, Perk.PERK_AUTODOC_RAISED_HIT_POINTS_II)) {
                            value += 4;
                        }

                        if (perkHasRank(critter, Perk.PERK_AUTODOC_LOWERED_HIT_POINTS)) {
                            value -= 2;
                        }

                        if (perkHasRank(critter, Perk.PERK_AUTODOC_LOWERED_HIT_POINTS_II)) {
                            value -= 4;
                        }
                        break;
                    case Stat.STAT_DAMAGE_RESISTANCE:
                    case Stat.STAT_DAMAGE_RESISTANCE_EXPLOSION:
                        if (perkHasRank(critter, Perk.PERK_DERMAL_IMPACT_ARMOR)) {
                            value += 5;
                        } else if (perkHasRank(critter, Perk.PERK_DERMAL_IMPACT_ASSAULT_ENHANCEMENT)) {
                            value += 10;
                        }
                        break;
                    case Stat.STAT_DAMAGE_RESISTANCE_LASER:
                    case Stat.STAT_DAMAGE_RESISTANCE_FIRE:
                    case Stat.STAT_DAMAGE_RESISTANCE_PLASMA:
                        if (perkHasRank(critter, Perk.PERK_PHOENIX_ARMOR_IMPLANTS)) {
                            value += 5;
                        } else if (perkHasRank(critter, Perk.PERK_PHOENIX_ASSAULT_ENHANCEMENT)) {
                            value += 10;
                        }
                        break;
                    case Stat.STAT_RADIATION_RESISTANCE:
                    case Stat.STAT_POISON_RESISTANCE:
                        if (perkHasRank(critter, Perk.PERK_VAULT_CITY_INOCULATIONS)) {
                            value += 10;
                        }
                        break;
                    }
                }

                value = Mathf.Clamp(value, f2Data.stat_data[stat].minimumValue, f2Data.stat_data[stat].maximumValue);
            } 
            else 
            {
                switch ((Stat)stat) {
                case Stat.STAT_CURRENT_HIT_POINTS:
                    value = critter_get_hits(critter);
                    break;
                case Stat.STAT_CURRENT_POISON_LEVEL:
                    value = critter_get_poison(critter);
                    break;
                case Stat.STAT_CURRENT_RADIATION_LEVEL:
                    value = critter_get_rads(critter);
                    break;
                default:
                    value = 0;
                    break;
                }
            }

            return value;
        }

        int skill_level(f2Unit critter, int skill)
        {
            // TODO
            return 0;

            // if (!skillIsValid(skill)) {
            //     return -5;
            // }

            // int baseValue = skill_points(critter, skill);
            // if (baseValue < 0) {
            //     return baseValue;
            // }

            // SkillDescription* skillDescription = &(skill_data[skill]);

            // int v7 = critterGetStat(critter, skillDescription->stat1);
            // if (skillDescription->stat2 != -1) {
            //     v7 += critterGetStat(critter, skillDescription->stat2);
            // }

            // int value = skillDescription->defaultValue + skillDescription->statModifier * v7 + baseValue * skillDescription->field_20;

            // if (critter == obj_dude) {
            //     if (skill_is_tagged(skill)) {
            //         value += baseValue * skillDescription->field_20;

            //         if (!perk_level(critter, PERK_TAG) || skill != tag_skill[3]) {
            //             value += 20;
            //         }
            //     }

            //     value += trait_adjust_skill(skill);
            //     value += perk_adjust_skill(critter, skill);
            //     value += skill_game_difficulty(skill);
            // }

            // if (value > 300) {
            //     value = 300;
            // }

            // return value;
        }

        int weaponGetDamageType(f2Unit critter, f2Item weapon)
        {
            // TODO
            // Proto* proto;

            // if (weapon != NULL) {
            //     protoGetProto(weapon->pid, &proto);

            //     return proto->item.data.weapon.damageType;
            // }

            // if (critter != NULL) {
            //     return critterGetDamageType(critter);
            // }

            return 0;
        }

        public void attackComputeDamage(Attack attack, int ammoQuantity, int bonusDamageMultiplier)
        {
            int damagePtr;
            f2Unit critter;
            int flagsPtr;
            int knockbackDistancePtr;

            if ((attack.attackerFlags & (int)Dam.DAM_HIT) != 0) 
            {
                damagePtr = attack.defenderDamage;
                critter = attack.defender;
                flagsPtr = attack.defenderFlags;
                knockbackDistancePtr = attack.defenderKnockback;
            } 
            else 
            {
                damagePtr = attack.attackerDamage;
                critter = attack.attacker;
                flagsPtr = attack.attackerFlags;
                knockbackDistancePtr = 0;
            }

            // TODO: why = 0
            damagePtr = 0;
            if (FID_TYPE(critter->fid) != OBJ_TYPE_CRITTER) {
                return;
            }

            // 获得攻击者武器类型
            int damageType = weaponGetDamageType(attack.attacker, attack.weapon);
            int damageThreshold = critterGetStat(critter, STAT_DAMAGE_THRESHOLD + damageType);
            int damageResistance = critterGetStat(critter, STAT_DAMAGE_RESISTANCE + damageType);

            if ((*flagsPtr & DAM_BYPASS) != 0 && damageType != DAMAGE_TYPE_EMP) {
                damageThreshold = 20 * damageThreshold / 100;
                damageResistance = 20 * damageResistance / 100;
            } else {
                // SFALL
                if (weaponGetPerk(attack->weapon) == PERK_WEAPON_PENETRATE
                    || unarmedIsPenetrating(attack->hitMode)) {
                    damageThreshold = 20 * damageThreshold / 100;
                }

                if (attack->attacker == gDude && traitIsSelected(TRAIT_FINESSE)) {
                    damageResistance += 30;
                }
            }

            int damageBonus;
            if (attack->attacker == gDude && weaponGetAttackTypeForHitMode(attack->weapon, attack->hitMode) == ATTACK_TYPE_RANGED) {
                damageBonus = 2 * perkGetRank(gDude, PERK_BONUS_RANGED_DAMAGE);
            } else {
                damageBonus = 0;
            }

            int combatDifficultyDamageModifier = 100;
            if (attack->attacker->data.critter.combat.team != gDude->data.critter.combat.team) {
                switch (settings.preferences.combat_difficulty) {
                case COMBAT_DIFFICULTY_EASY:
                    combatDifficultyDamageModifier = 75;
                    break;
                case COMBAT_DIFFICULTY_HARD:
                    combatDifficultyDamageModifier = 125;
                    break;
                }
            }

            // SFALL: Damage mod.
            DamageCalculationContext context;
            context.attack = attack;
            context.damagePtr = damagePtr;
            context.damageResistance = damageResistance;
            context.damageThreshold = damageThreshold;
            context.damageBonus = damageBonus;
            context.bonusDamageMultiplier = bonusDamageMultiplier;
            context.combatDifficultyDamageModifier = combatDifficultyDamageModifier;

            if (gDamageCalculationType == DAMAGE_CALCULATION_TYPE_GLOVZ || gDamageCalculationType == DAMAGE_CALCULATION_TYPE_GLOVZ_WITH_DAMAGE_MULTIPLIER_TWEAK) {
                damageModCalculateGlovz(&context);
            } else if (gDamageCalculationType == DAMAGE_CALCULATION_TYPE_YAAM) {
                damageModCalculateYaam(&context);
            } else {
                damageResistance += weaponGetAmmoDamageResistanceModifier(attack->weapon);
                if (damageResistance > 100) {
                    damageResistance = 100;
                } else if (damageResistance < 0) {
                    damageResistance = 0;
                }

                int damageMultiplier = bonusDamageMultiplier * weaponGetAmmoDamageMultiplier(attack->weapon);
                int damageDivisor = weaponGetAmmoDamageDivisor(attack->weapon);

                for (int index = 0; index < ammoQuantity; index++) {
                    int damage = weaponGetDamage(attack->attacker, attack->hitMode);

                    damage += damageBonus;

                    damage *= damageMultiplier;

                    if (damageDivisor != 0) {
                        damage /= damageDivisor;
                    }

                    // TODO: Why we're halving it?
                    // NOTE：因为默认 bonusDamageMultiplier = 2
                    damage /= 2;

                    damage *= combatDifficultyDamageModifier;
                    damage /= 100;

                    damage -= damageThreshold;

                    if (damage > 0) {
                        damage -= damage * damageResistance / 100;
                    }

                    if (damage > 0) {
                        *damagePtr += damage;
                    }
                }
            }

            if (attack->attacker == gDude) {
                if (perkGetRank(attack->attacker, PERK_LIVING_ANATOMY) != 0) {
                    int kt = critterGetKillType(attack->defender);
                    if (kt != KILL_TYPE_ROBOT && kt != KILL_TYPE_ALIEN) {
                        *damagePtr += 5;
                    }
                }

                if (perkGetRank(attack->attacker, PERK_PYROMANIAC) != 0) {
                    if (weaponGetDamageType(attack->attacker, attack->weapon) == DAMAGE_TYPE_FIRE) {
                        *damagePtr += 5;
                    }
                }
            }

            if (knockbackDistancePtr != NULL
                && (critter->flags & OBJECT_MULTIHEX) == 0
                && (damageType == DAMAGE_TYPE_EXPLOSION || attack->weapon == NULL || weaponGetAttackTypeForHitMode(attack->weapon, attack->hitMode) == ATTACK_TYPE_MELEE)
                && PID_TYPE(critter->pid) == OBJ_TYPE_CRITTER
                && !_critter_flag_check(critter->pid, CRITTER_NO_KNOCKBACK)) {
                bool shouldKnockback = true;
                bool hasStonewall = false;
                if (critter == gDude) {
                    if (perkGetRank(critter, PERK_STONEWALL) != 0) {
                        int chance = randomBetween(0, 100);
                        hasStonewall = true;
                        if (chance < 50) {
                            shouldKnockback = false;
                        }
                    }
                }

                if (shouldKnockback) {
                    int knockbackDistanceDivisor = weaponGetPerk(attack->weapon) == PERK_WEAPON_KNOCKBACK ? 5 : 10;

                    *knockbackDistancePtr = *damagePtr / knockbackDistanceDivisor;

                    if (hasStonewall) {
                        *knockbackDistancePtr /= 2;
                    }
                }
            }
        }
    }
}
