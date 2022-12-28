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
        public int flags;

        int proto_ptr(int pid, ref Proto protoPtr)
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
                proto_ptr(critter.pid, ref proto);
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
                proto_ptr(critter.pid, ref proto);
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

        public int item_w_damage(f2Unit critter, int hitMode)
        {
            // TODO
            return 0;
            // if (critter == NULL) {
            //     return 0;
            // }

            // int minDamage = 0;
            // int maxDamage = 0;
            // int meleeDamage = 0;
            // int unarmedDamage = 0;

            // // NOTE: Uninline.
            // Object* weapon = item_hit_with(critter, hitMode);

            // if (weapon != NULL) {
            //     Proto* proto;
            //     proto_ptr(weapon->pid, &proto);

            //     minDamage = proto->item.data.weapon.minDamage;
            //     maxDamage = proto->item.data.weapon.maxDamage;

            //     int attackType = item_w_subtype(weapon, hitMode);
            //     if (attackType == ATTACK_TYPE_MELEE || attackType == ATTACK_TYPE_UNARMED) {
            //         meleeDamage = critterGetStat(critter, STAT_MELEE_DAMAGE);
            //     }
            // } else {
            //     minDamage = 1;
            //     maxDamage = critterGetStat(critter, STAT_MELEE_DAMAGE) + 2;

            //     switch (hitMode) {
            //     case HIT_MODE_STRONG_PUNCH:
            //     case HIT_MODE_JAB:
            //         unarmedDamage = 3;
            //         break;
            //     case HIT_MODE_HAMMER_PUNCH:
            //     case HIT_MODE_STRONG_KICK:
            //         unarmedDamage = 4;
            //         break;
            //     case HIT_MODE_HAYMAKER:
            //     case HIT_MODE_PALM_STRIKE:
            //     case HIT_MODE_SNAP_KICK:
            //     case HIT_MODE_HIP_KICK:
            //         unarmedDamage = 7;
            //         break;
            //     case HIT_MODE_POWER_KICK:
            //     case HIT_MODE_HOOK_KICK:
            //         unarmedDamage = 9;
            //         break;
            //     case HIT_MODE_PIERCING_STRIKE:
            //         unarmedDamage = 10;
            //         break;
            //     case HIT_MODE_PIERCING_KICK:
            //         unarmedDamage = 12;
            //         break;
            //     }
            // }

            // return roll_random(unarmedDamage + minDamage, unarmedDamage + meleeDamage + maxDamage);
        }

        int critterGetKillType(f2Unit obj)
        {
            if (obj == obj_dude) {
                int gender = critterGetStat(obj, (int)Stat.STAT_GENDER);
                if (gender == (int)Gender.GENDER_FEMALE) {
                    return (int)KillType.KILL_TYPE_WOMAN;
                }
                return (int)KillType.KILL_TYPE_MAN;
            }

            if (PID_TYPE(obj.pid) != (int)ObjType.OBJ_TYPE_CRITTER) {
                return -1;
            }

            Proto proto = null;
            proto_ptr(obj.pid, ref proto);

            return proto.critter.data.killType;
        }

        int critter_get_base_damage_type(f2Unit obj)
        {
            if (PID_TYPE(obj.pid) != (int)ObjType.OBJ_TYPE_CRITTER) {
                return 0;
            }

            Proto proto = null;
            if (proto_ptr(obj.pid, ref proto) == -1) {
                return 0;
            }

            return proto.critter.data.damageType;
        }

        int item_w_damage_type(f2Unit critter, f2Item weapon)
        {
            Proto proto = null;

            if (weapon != null) {
                proto_ptr(weapon.pid, ref proto);

                return proto.item.data.weapon.damageType;
            }

            if (critter != null) 
            {
                return critter_get_base_damage_type(critter);
            }

            return 0;
        }

        // Checks proto critter flag.
        bool critter_flag_check(int pid, int flag)
        {
            if (pid == -1) {
                return false;
            }

            if (PID_TYPE(pid) != (int)ObjType.OBJ_TYPE_CRITTER) {
                return false;
            }

            Proto proto = null;
            proto_ptr(pid, ref proto);
            return (proto.critter.data.flags & flag) != 0;
        }

        public void compute_damage(Attack attack, int ammoQuantity, int bonusDamageMultiplier)
        {
            int damagePtr;
            f2Unit critter;
            int flagsPtr;
            int knockbackDistancePtr;
            bool hasKnockbackDistancePtr;

            if ((attack.attackerFlags & (int)Dam.DAM_HIT) != 0) 
            {
                damagePtr = attack.defenderDamage;
                critter = attack.defender;
                flagsPtr = attack.defenderFlags;
                knockbackDistancePtr = attack.defenderKnockback;
                hasKnockbackDistancePtr = true;
            } 
            else 
            {
                damagePtr = attack.attackerDamage;
                critter = attack.attacker;
                flagsPtr = attack.attackerFlags;
                knockbackDistancePtr = 0;
                hasKnockbackDistancePtr = false;
            }

            // TODO: why = 0
            damagePtr = 0;
            if (FID_TYPE(critter.fid) != (int)ObjType.OBJ_TYPE_CRITTER) {
                return;
            }

            // 获得攻击者武器类型
            int damageType = weaponGetDamageType(attack.attacker, attack.weapon);
            int damageThreshold = critterGetStat(critter, (int)Stat.STAT_DAMAGE_THRESHOLD + damageType);
            int damageResistance = critterGetStat(critter, (int)Stat.STAT_DAMAGE_RESISTANCE + damageType);

            if ((flagsPtr & (int)Dam.DAM_BYPASS) != 0 && damageType != (int)DamageType.DAMAGE_TYPE_EMP) {
                damageThreshold = 20 * damageThreshold / 100;
                damageResistance = 20 * damageResistance / 100;
            } 
            else {
                // SFALL
                if (attack.weapon.item_w_perk() == (int)Perk.PERK_WEAPON_PENETRATE
                    || attack.hitMode == (int)HitMode.HIT_MODE_PALM_STRIKE
                    || attack.hitMode == (int)HitMode.HIT_MODE_PIERCING_STRIKE
                    || attack.hitMode == (int)HitMode.HIT_MODE_HOOK_KICK
                    || attack.hitMode == (int)HitMode.HIT_MODE_PIERCING_KICK) {
                    damageThreshold = 20 * damageThreshold / 100;
                }

                if (attack.attacker == obj_dude && trait_level((int)Trait.TRAIT_FINESSE)) {
                    damageResistance += 30;
                }
            }

            int damageBonus;
            if (attack.attacker == obj_dude && attack.weapon.item_w_subtype(attack.hitMode) == (int)AttackType.ATTACK_TYPE_RANGED) {
                damageBonus = 2 * perk_level(obj_dude, (int)Perk.PERK_BONUS_RANGED_DAMAGE);
            } else {
                damageBonus = 0;
            }

            int combatDifficultyDamageModifier = 100;
            if (attack.attacker.data.critter.combat.team != obj_dude.data.critter.combat.team) {
                switch (f2DEF.gCombatDifficulty) {
                case CombatDifficulty.COMBAT_DIFFICULTY_EASY:
                    combatDifficultyDamageModifier = 75;
                    break;
                case CombatDifficulty.COMBAT_DIFFICULTY_HARD:
                    combatDifficultyDamageModifier = 125;
                    break;
                }
            }

            damageResistance += attack.weapon.item_w_dr_adjust();
            if (damageResistance > 100) {
                damageResistance = 100;
            } else if (damageResistance < 0) {
                damageResistance = 0;
            }

            int damageMultiplier = bonusDamageMultiplier * attack.weapon.item_w_dam_mult();
            int damageDivisor = attack.weapon.item_w_dam_div();

            for (int index = 0; index < ammoQuantity; index++) 
            {
                int damage = item_w_damage(attack.attacker, attack.hitMode);

                damage += damageBonus;

                damage *= damageMultiplier;

                if (damageDivisor != 0) {
                    damage /= damageDivisor;
                }

                // TODO: Why we're halving it?
                damage /= 2;

                damage *= combatDifficultyDamageModifier;
                damage /= 100;

                damage -= damageThreshold;

                if (damage > 0) {
                    damage -= damage * damageResistance / 100;
                }

                if (damage > 0) {
                    damagePtr += damage;
                }
            }

            if (attack.attacker == obj_dude) 
            {
                if (perk_level(attack.attacker, (int)Perk.PERK_LIVING_ANATOMY) != 0) {
                    int kt = critterGetKillType(attack.defender);
                    if (kt != (int)KillType.KILL_TYPE_ROBOT && kt != (int)KillType.KILL_TYPE_ALIEN) {
                        damagePtr += 5;
                    }
                }

                if (perk_level(attack.attacker, (int)Perk.PERK_PYROMANIAC) != 0) {
                    if (item_w_damage_type(attack.attacker, attack.weapon) == (int)DamageType.DAMAGE_TYPE_FIRE) {
                        damagePtr += 5;
                    }
                }
            }

            if (hasKnockbackDistancePtr
                && (critter.flags & (int)ObjectFlags.OBJECT_MULTIHEX) == 0
                && (damageType == (int)DamageType.DAMAGE_TYPE_EXPLOSION || attack.weapon == null || attack.weapon.item_w_subtype(attack.hitMode) == (int)AttackType.ATTACK_TYPE_MELEE)
                && PID_TYPE(critter.pid) == (int)ObjType.OBJ_TYPE_CRITTER
                && critter_flag_check(critter.pid, (int)CritterFlags.CRITTER_NO_KNOCKBACK) == false) 
            {
                bool shouldKnockback = true;
                bool hasStonewall = false;
                if (critter == obj_dude) {
                    if (perk_level(critter, (int)Perk.PERK_STONEWALL) != 0) {
                        int chance = f2Utils.roll_random(0, 100);
                        hasStonewall = true;
                        if (chance < 50) {
                            shouldKnockback = false;
                        }
                    }
                }

                if (shouldKnockback) 
                {
                    int knockbackDistanceDivisor = attack.weapon.item_w_perk() == (int)Perk.PERK_WEAPON_KNOCKBACK ? 5 : 10;

                    knockbackDistancePtr = damagePtr / knockbackDistanceDivisor;

                    if (hasStonewall) 
                    {
                        knockbackDistancePtr /= 2;
                    }

                    // set back
                    attack.defenderKnockback = knockbackDistancePtr;
                }
            }
        }
    }
}
