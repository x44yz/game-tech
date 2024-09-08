using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter
{
    static int critter_get_hits(f2Object critter)
    {
        return PID_TYPE(critter.pid) == (int)ObjType.OBJ_TYPE_CRITTER ? critter.data.critter.hp : 0;
    }

    static int critter_get_poison(f2Object critter)
    {
        return PID_TYPE(critter.pid) == (int)ObjType.OBJ_TYPE_CRITTER ? critter.data.critter.poison : 0;
    }

    static int critter_get_rads(f2Object obj)
    {
        return PID_TYPE(obj.pid) == (int)ObjType.OBJ_TYPE_CRITTER ? obj.data.critter.radiation : 0;
    }

    static int stat_get_base_direct(f2Object critter, int stat)
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

    static int stat_get_bonus(f2Object critter, int stat)
    {
        if (stat >= 0 && stat < (int)Stat.SAVEABLE_STAT_COUNT) {
            Proto proto = null;
            proto_ptr(critter.pid, ref proto);
            return proto.critter.data.bonusStats[stat];
        }

        return 0;
    }

    // Returns stat modifier depending on selected traits.
    static int trait_adjust_stat(int stat)
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

    static int stat_get_base(f2Object critter, int stat)
    {
        int value = stat_get_base_direct(critter, stat);

        if (critter == obj_dude) {
            value += trait_adjust_stat(stat);
        }

        return value;
    }

    // 获得右手物品
    public static f2Object inven_right_hand(f2Object critter)
    {
        int i;
        Inventory inventory;
        f2Object item;

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
    public static f2Object inven_left_hand(f2Object critter)
    {
        int i;
        Inventory inventory;
        f2Object item;

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

    static f2Object inven_worn(f2Object critter)
    {
        int i;
        Inventory inventory;
        f2Object item;

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

    static bool isInCombat()
    {
        return f2Test.isInCombat();
    }

    public static f2Object combat_turn_obj;
    public static f2Object combat_whose_turn()
    {
        if (isInCombat()) {
            return combat_turn_obj;
        } else {
            return null;
        }
    }

    public static int critterGetStat(f2Object critter, Stat stat)
    {
        return critterGetStat(critter, (int)stat);
    }
    
    public static int critterGetStat(f2Object critter, int stat)
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

                                f2Object item2 = inven_right_hand(critter);
                                if (item2 != null) {
                                    if (item_get_type(item2) == (int)ItemType.ITEM_TYPE_WEAPON) {
                                        // TODO: 有什么武器是没动画的？
                                        if (item_w_anim_code(item2) != (int)WeaponAnimation.WEAPON_ANIMATION_NONE) {
                                            hasWeapon = true;
                                        }
                                    }
                                }

                                if (!hasWeapon) {
                                    f2Object item1 = inven_left_hand(critter);
                                    if (item1 != null) {
                                        if (item_get_type(item1) == (int)ItemType.ITEM_TYPE_WEAPON) {
                                            if (item_w_anim_code(item1) != (int)WeaponAnimation.WEAPON_ANIMATION_NONE) {
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
                value += f2Test.game_time() / GAME_TIME_TICKS_PER_YEAR;
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

                        f2Object item2 = inven_right_hand(critter);
                        if (item2 != null && item2.pid == (int)ProtoID.PROTO_ID_MIRRORED_SHADES) {
                            hasMirrorShades = true;
                        }

                        f2Object item1 = inven_left_hand(critter);
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

            value = Mathf.Clamp(value, stat_data[stat].minimumValue, stat_data[stat].maximumValue);
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

    static int skill_points(f2Object obj, int skill)
    {
        if (!skillIsValid(skill)) {
            return 0;
        }

        Proto proto = null;
        proto_ptr(obj.pid, ref proto);

        return proto.critter.data.skills[skill];
    }

    static int[] tag_skill = new int[NUM_TAGGED_SKILLS];
    static bool skill_is_tagged(int skill)
    {
        return skill == tag_skill[0]
            || skill == tag_skill[1]
            || skill == tag_skill[2]
            || skill == tag_skill[3];
    }

    static int skill_level(f2Object critter, int skill)
    {
        if (!skillIsValid(skill)) {
            return -5;
        }

        // 升级技能点
        int baseValue = skill_points(critter, skill);
        if (baseValue < 0) {
            return baseValue;
        }

        SkillDescription skillDescription = (skill_data[skill]);

        int v7 = critterGetStat(critter, skillDescription.stat1);
        if (skillDescription.stat2 != -1) {
            v7 += critterGetStat(critter, skillDescription.stat2);
        }

        int value = skillDescription.defaultValue + skillDescription.statModifier * v7 + baseValue * skillDescription.field_20;

        if (critter == obj_dude) {
            if (skill_is_tagged(skill)) 
            {
                value += baseValue * skillDescription.field_20;

                if (perk_level(critter, (int)Perk.PERK_TAG) != 0 || skill != tag_skill[3]) {
                    value += 20;
                }
            }

            // 天赋
            value += trait_adjust_skill(skill);
            // perk
            value += perk_adjust_skill(critter, skill);
            // 难度（Easy+20，Normal+0，Hard-10）
            value += skill_game_difficulty(skill);
        }

        if (value > 300) {
            value = 300;
        }

        return value;
    }

    public static int critterGetKillType(f2Object obj)
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

    public static int critter_get_base_damage_type(f2Object obj)
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

    // Checks proto critter flag.
    public static bool critter_flag_check(int pid, int flag)
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

    public static int obj_dist(f2Object object1, f2Object object2)
    {
        return 0;
        // if (object1 == NULL || object2 == NULL) {
        //     return 0;
        // }

        // int distance = tile_dist(object1->tile, object2->tile);

        // if ((object1->flags & OBJECT_MULTIHEX) != 0) {
        //     distance -= 1;
        // }

        // if ((object2->flags & OBJECT_MULTIHEX) != 0) {
        //     distance -= 1;
        // }

        // if (distance < 0) {
        //     distance = 0;
        // }

        // return distance;
    }

    public static bool is_pc_flag(int state)
    {
        Proto proto = null;
        proto_ptr(obj_dude.pid, ref proto);
        return (proto.critter.data.flags & (1 << state)) != 0;
    }

    static bool critter_is_dead(f2Object critter)
    {
        if (critter == null) {
            return false;
        }

        if (PID_TYPE(critter.pid) != (int)ObjType.OBJ_TYPE_CRITTER) {
            return false;
        }

        if (critterGetStat(critter, (int)Stat.STAT_CURRENT_HIT_POINTS) <= 0) {
            return true;
        }

        if ((critter.data.critter.combat.results & (int)Dam.DAM_DEAD) != 0) {
            return true;
        }

        return false;
    }

    static int critter_set_who_hit_me(f2Object a1, f2Object a2)
    {
        if (a1 == null) {
            return -1;
        }

        if (a2 != null && FID_TYPE(a2.fid) != (int)ObjType.OBJ_TYPE_CRITTER) {
            return -1;
        }

        if (PID_TYPE(a1.pid) == (int)ObjType.OBJ_TYPE_CRITTER) 
        {
            int howMuch = 0;
            if (a2 == null || a1.data.critter.combat.team != a2.data.critter.combat.team || (stat_result(a1, (int)Stat.STAT_INTELLIGENCE, -1, ref howMuch) < 2 && (!isPartyMember(a1) || !isPartyMember(a2)))) {
                a1.data.critter.combat.whoHitMe = a2;
                if (a2 == obj_dude) {
                    reaction_set(a1, -3);
                }
            }
        }

        return 0;
    }

    static void stat_recalc_derived(f2Object critter)
    {
        int strength = critterGetStat(critter, Stat.STAT_STRENGTH); // 力量
        int perception = critterGetStat(critter, Stat.STAT_PERCEPTION); // 感知
        int endurance = critterGetStat(critter, Stat.STAT_ENDURANCE); // 耐力
        int intelligence = critterGetStat(critter, Stat.STAT_INTELLIGENCE); // 智商
        int agility = critterGetStat(critter, Stat.STAT_AGILITY); // 敏捷
        int luck = critterGetStat(critter, Stat.STAT_LUCK); // 幸运

        Proto proto = null;
        proto_ptr(critter.pid, ref proto);
        CritterProtoData data = proto.critter.data;

        // 基础属性，没有 item/perk/trait/skill 加成
        data.baseStats[(int)Stat.STAT_MAXIMUM_HIT_POINTS] = stat_get_base(critter, (int)Stat.STAT_STRENGTH) + stat_get_base(critter, (int)Stat.STAT_ENDURANCE) * 2 + 15;
        data.baseStats[(int)Stat.STAT_MAXIMUM_ACTION_POINTS] = agility / 2 + 5;
        data.baseStats[(int)Stat.STAT_ARMOR_CLASS] = agility; 
        data.baseStats[(int)Stat.STAT_MELEE_DAMAGE] = Mathf.Max(strength - 5, 1);
        data.baseStats[(int)Stat.STAT_CARRY_WEIGHT] = 25 * strength + 25;
        data.baseStats[(int)Stat.STAT_SEQUENCE] = 2 * perception;
        data.baseStats[(int)Stat.STAT_HEALING_RATE] = Mathf.Max(endurance / 3, 1);
        data.baseStats[(int)Stat.STAT_CRITICAL_CHANCE] = luck;
        data.baseStats[(int)Stat.STAT_BETTER_CRITICALS] = 0;
        data.baseStats[(int)Stat.STAT_RADIATION_RESISTANCE] = 2 * endurance;
        data.baseStats[(int)Stat.STAT_POISON_RESISTANCE] = 5 * endurance;
    }

    static bool statIsValid(int stat)
    {
        return stat >= 0 && stat < (int)Stat.STAT_COUNT;
    }

    // 设置基础值 SPECIAL，其他基础值都是通过这个计算出来的
    static int stat_set_base(f2Object critter, int stat, int value)
    {
        if (!statIsValid(stat)) {
            return -5;
        }

        if (stat >= 0 && stat < (int)Stat.SAVEABLE_STAT_COUNT) {
            if (stat > (int)Stat.STAT_LUCK && stat <= (int)Stat.STAT_POISON_RESISTANCE) {
                // Cannot change base value of derived stats.
                return -1;
            }

            if (critter == obj_dude) {
                value -= trait_adjust_stat(stat);
            }

            if (value < stat_data[stat].minimumValue) {
                return -2;
            }

            if (value > stat_data[stat].maximumValue) {
                return -3;
            }

            Proto proto = null;
            proto_ptr(critter.pid, ref proto);
            proto.critter.data.baseStats[stat] = value;

            if (stat >= (int)Stat.STAT_STRENGTH && stat <= (int)Stat.STAT_LUCK) {
                stat_recalc_derived(critter);
            }

            return 0;
        }

        switch ((Stat)stat) {
        case Stat.STAT_CURRENT_HIT_POINTS:
            return critter_adjust_hits(critter, value - critter_get_hits(critter));
        // case Stat.STAT_CURRENT_POISON_LEVEL:
        //     return critter_adjust_poison(critter, value - critter_get_poison(critter));
        // case Stat.STAT_CURRENT_RADIATION_LEVEL:
        //     return critter_adjust_rads(critter, value - critter_get_rads(critter));
        }

        // Should be unreachable
        return 0;
    }
}
