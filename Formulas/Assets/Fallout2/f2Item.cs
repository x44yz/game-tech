using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        // Maps weapon extended flags to weapon class
        public static int[] attack_subtype = new int[9]{
            (int)AttackType.ATTACK_TYPE_NONE, // 0 // None
            (int)AttackType.ATTACK_TYPE_UNARMED, // 1 // Punch // Brass Knuckles, Power First
            (int)AttackType.ATTACK_TYPE_UNARMED, // 2 // Kick?
            (int)AttackType.ATTACK_TYPE_MELEE, // 3 // Swing //  Sledgehammer (prim), Club, Knife (prim), Spear (prim), Crowbar
            (int)AttackType.ATTACK_TYPE_MELEE, // 4 // Thrust // Sledgehammer (sec), Knife (sec), Spear (sec)
            (int)AttackType.ATTACK_TYPE_THROW, // 5 // Throw // Rock,
            (int)AttackType.ATTACK_TYPE_RANGED, // 6 // Single // 10mm SMG (prim), Rocket Launcher, Hunting Rifle, Plasma Rifle, Laser Pistol
            (int)AttackType.ATTACK_TYPE_RANGED, // 7 // Burst // 10mm SMG (sec), Minigun
            (int)AttackType.ATTACK_TYPE_RANGED, // 8 // Continous // Only: Flamer, Improved Flamer, Flame Breath
        };

        // Returns true if [item] is an natural weapon of it's owner.
        //
        // See [ItemProtoExtendedFlags_NaturalWeapon] for more details on natural weapons.
        public static int item_is_hidden(f2Object obj)
        {
            Proto proto = null;

            if (PID_TYPE(obj.pid) != (int)ObjType.OBJ_TYPE_ITEM) {
                return 0;
            }

            if (proto_ptr(obj.pid, ref proto) == -1) {
                return 0;
            }

            return proto.item.extendedFlags & (int)ItemProtoExtendedFlags.ItemProtoExtendedFlags_NaturalWeapon;
        }

        // Calculates total weight of the items in inventory.
        // NOTE:
        // sfall use same function like f2Object
        static int item_total_weight(f2Object obj)
        {
            if (obj == null) {
                return 0;
            }

            int weight = 0;

            Inventory inventory = (obj.data.inventory);
            for (int index = 0; index < inventory.length; index++) 
            {
                InventoryItem inventoryItem = (inventory.items[index]);
                f2Object item = inventoryItem.item;
                weight += item_weight(item) * inventoryItem.quantity;
            }

            // // TODO:
            // // 上面不是已经加过 weight，下面又统计没有装备的，是否重复？
            if (FID_TYPE(obj.fid) == (int)ObjType.OBJ_TYPE_CRITTER) {
                f2Object item2 = inven_right_hand(obj);
                if (item2 != null) 
                {
                    // 不在右手
                    if ((item2.flags & (uint)ObjectFlags.OBJECT_IN_RIGHT_HAND) == 0) {
                        weight += item_weight(item2);
                    }
                }

                f2Object item1 = inven_left_hand(obj);
                if (item1 != null) {
                    if ((item1.flags & (uint)ObjectFlags.OBJECT_IN_LEFT_HAND) == 0) {
                        weight += item_weight(item1);
                    }
                }

                f2Object armor = inven_worn(obj);
                if (armor != null) 
                {
                    if ((armor.flags & (uint)ObjectFlags.OBJECT_WORN) == 0) 
                    {
                        weight += item_weight(armor);
                    }
                }
            }
            return weight;
        }

        public static int item_weight(f2Object item)
        {
            if (item == null) {
                return 0;
            }

            Proto proto = null;
            proto_ptr(item.pid, ref proto);
            int weight = proto.item.weight;

            // NOTE: Uninline.
            if (item_is_hidden(item) != 0) {
                weight = 0;
            }

            int itemType = proto.item.type;
            if (itemType == (int)ItemType.ITEM_TYPE_ARMOR) 
            {
                switch ((ProtoID)proto.pid) {
                case ProtoID.PROTO_ID_POWER_ARMOR:
                case ProtoID.PROTO_ID_HARDENED_POWER_ARMOR:
                case ProtoID.PROTO_ID_ADVANCED_POWER_ARMOR:
                case ProtoID.PROTO_ID_ADVANCED_POWER_ARMOR_MK_II:
                    weight /= 2;
                    break;
                }
            } 
            else if (itemType == (int)ItemType.ITEM_TYPE_CONTAINER) 
            {
                weight += item_total_weight(item);
            } 
            else if (itemType == (int)ItemType.ITEM_TYPE_WEAPON) 
            {
                // NOTE: Uninline.
                int ammoQuantity = item_w_curr_ammo(item);
                if (ammoQuantity > 0) 
                {
                    // NOTE: Uninline.
                    int ammoTypePid = item_w_ammo_pid(item);
                    if (ammoTypePid != -1) 
                    {
                        Proto ammoProto = null;
                        if (proto_ptr(ammoTypePid, ref ammoProto) != -1) 
                        {
                            weight += ammoProto.item.weight * ((ammoQuantity - 1) / ammoProto.item.data.ammo.quantity + 1);
                        }
                    }
                }
            }

            return weight;
        }

        public static int item_w_ammo_pid(f2Object weapon)
        {
            if (weapon == null) {
                return -1;
            }

            if (item_get_type(weapon) != (int)ItemType.ITEM_TYPE_WEAPON) {
                return -1;
            }

            return weapon.data.item.weapon.ammoTypePid;
        }

        public static int item_w_curr_ammo(f2Object ammoOrWeapon)
        {
            if (ammoOrWeapon == null) {
                return 0;
            }

            Proto proto = null;
            proto_ptr(ammoOrWeapon.pid, ref proto);

            // NOTE: Looks like the condition jumps were erased during compilation only
            // because ammo's quantity and weapon's ammo quantity coincidently stored
            // in the same offset relative to [Object].
            if (proto.item.type == (int)ItemType.ITEM_TYPE_AMMO) {
                return ammoOrWeapon.data.item.ammo.quantity;
            } else {
                return ammoOrWeapon.data.item.weapon.ammoQuantity;
            }
        }

        public static int item_get_type(f2Object item)
        {
            if (item == null) {
                return (int)ItemType.ITEM_TYPE_MISC;
            }

            if (PID_TYPE(item.pid) != (int)ObjType.OBJ_TYPE_ITEM) {
                return (int)ItemType.ITEM_TYPE_MISC;
            }

            if (item.pid == (int)ProtoID.PROTO_ID_SHIV) {
                return (int)ItemType.ITEM_TYPE_MISC;
            }

            Proto proto = null;
            proto_ptr(item.pid, ref proto);

            return proto.item.type;
        }

        public static int item_w_anim(f2Object critter, int hitMode)
        {
            // NOTE: Uninline.
            // Object* weapon = item_hit_with(critter, hitMode);
            // return item_w_anim_weap(weapon, hitMode);
            // TODO
            return 0;
        }

        public static int item_w_anim_code(f2Object weapon)
        {
            if (weapon == null) {
                return -1;
            }

            Proto proto = null;
            proto_ptr(weapon.pid, ref proto);

            return proto.item.data.weapon.animationCode;
        }

        public static int item_w_perk(f2Object weapon)
        {
            if (weapon == null) {
                return -1;
            }

            Proto proto = null;
            proto_ptr(weapon.pid, ref proto);

            return proto.item.data.weapon.perk;
        }

        // GetAttackTypeForHitMode
        public static int item_w_subtype(f2Object weapon, int hitMode)
        {
            if (weapon == null) {
                return (int)AttackType.ATTACK_TYPE_UNARMED;
            }

            Proto proto = null;
            proto_ptr(weapon.pid, ref proto);

            int index;
            if (hitMode == (int)HitMode.HIT_MODE_LEFT_WEAPON_PRIMARY || hitMode == (int)HitMode.HIT_MODE_RIGHT_WEAPON_PRIMARY) {
                index = proto.item.extendedFlags & 0xF;
            } else {
                index = (proto.item.extendedFlags & 0xF0) >> 4;
            }

            return attack_subtype[index];
        }

        public static int item_w_dr_adjust(f2Object weapon)
        {
            // NOTE: Uninline.
            int ammoTypePid = item_w_ammo_pid(weapon);
            if (ammoTypePid == -1) {
                return 0;
            }

            Proto proto = null;
            if (proto_ptr(ammoTypePid, ref proto) == -1) {
                return 0;
            }

            return proto.item.data.ammo.damageResistanceModifier;
        }

        public static int item_w_dam_mult(f2Object weapon)
        {
            // NOTE: Uninline.
            int ammoTypePid = item_w_ammo_pid(weapon);
            if (ammoTypePid == -1) {
                return 1;
            }

            Proto proto = null;
            if (proto_ptr(ammoTypePid, ref proto) == -1) {
                return 1;
            }

            return proto.item.data.ammo.damageMultiplier;
        }

        public static int item_w_dam_div(f2Object weapon)
        {
            // NOTE: Uninline.
            int ammoTypePid = item_w_ammo_pid(weapon);
            if (ammoTypePid == -1) {
                return 1;
            }

            Proto proto = null;
            if (proto_ptr(ammoTypePid, ref proto) == -1) {
                return 1;
            }

            return proto.item.data.ammo.damageDivisor;
        }

        public static int item_w_damage_type(f2Object critter, f2Object weapon)
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

        public static f2Object item_hit_with(f2Object critter, int hitMode)
        {
            switch ((HitMode)hitMode) {
            case HitMode.HIT_MODE_LEFT_WEAPON_PRIMARY:
            case HitMode.HIT_MODE_LEFT_WEAPON_SECONDARY:
            case HitMode.HIT_MODE_LEFT_WEAPON_RELOAD:
                return inven_left_hand(critter);
            case HitMode.HIT_MODE_RIGHT_WEAPON_PRIMARY:
            case HitMode.HIT_MODE_RIGHT_WEAPON_SECONDARY:
            case HitMode.HIT_MODE_RIGHT_WEAPON_RELOAD:
                return inven_right_hand(critter);
            }

            return null;
        }

        public static int item_w_damage(f2Object critter, int hitMode)
        {
            if (critter == null) {
                return 0;
            }

            int minDamage = 0;
            int maxDamage = 0;
            int meleeDamage = 0;
            int unarmedDamage = 0;

            // NOTE: Uninline.
            f2Object weapon = item_hit_with(critter, hitMode);

            if (weapon != null) {
                Proto proto = null;
                proto_ptr(weapon.pid, ref proto);

                minDamage = proto.item.data.weapon.minDamage;
                maxDamage = proto.item.data.weapon.maxDamage;

                int attackType = item_w_subtype(weapon, hitMode);
                if (attackType == (int)AttackType.ATTACK_TYPE_MELEE || attackType == (int)AttackType.ATTACK_TYPE_UNARMED) {
                    meleeDamage = critterGetStat(critter, (int)Stat.STAT_MELEE_DAMAGE);
                }
            } else {
                minDamage = 1;
                maxDamage = critterGetStat(critter, (int)Stat.STAT_MELEE_DAMAGE) + 2;

                switch ((HitMode)hitMode) {
                case HitMode.HIT_MODE_STRONG_PUNCH:
                case HitMode.HIT_MODE_JAB:
                    unarmedDamage = 3;
                    break;
                case HitMode.HIT_MODE_HAMMER_PUNCH:
                case HitMode.HIT_MODE_STRONG_KICK:
                    unarmedDamage = 4;
                    break;
                case HitMode.HIT_MODE_HAYMAKER:
                case HitMode.HIT_MODE_PALM_STRIKE:
                case HitMode.HIT_MODE_SNAP_KICK:
                case HitMode.HIT_MODE_HIP_KICK:
                    unarmedDamage = 7;
                    break;
                case HitMode.HIT_MODE_POWER_KICK:
                case HitMode.HIT_MODE_HOOK_KICK:
                    unarmedDamage = 9;
                    break;
                case HitMode.HIT_MODE_PIERCING_STRIKE:
                    unarmedDamage = 10;
                    break;
                case HitMode.HIT_MODE_PIERCING_KICK:
                    unarmedDamage = 12;
                    break;
                }
            }

            return roll_random(unarmedDamage + minDamage, unarmedDamage + meleeDamage + maxDamage);
        }

        public static int item_w_range(f2Object critter, int hitMode)
        {
            int range;
            int v12;

            // NOTE: Uninline.
            f2Object weapon = item_hit_with(critter, hitMode);

            if (weapon != null && hitMode != 4 && hitMode != 5 && (hitMode < 8 || hitMode > 19)) 
            {
                Proto proto = null;
                proto_ptr(weapon.pid, ref proto);
                if (hitMode == (int)HitMode.HIT_MODE_LEFT_WEAPON_PRIMARY || hitMode == (int)HitMode.HIT_MODE_RIGHT_WEAPON_PRIMARY) {
                    range = proto.item.data.weapon.maxRange1;
                } else {
                    range = proto.item.data.weapon.maxRange2;
                }

                if (item_w_subtype(weapon, hitMode) == (int)AttackType.ATTACK_TYPE_THROW) 
                {
                    if (critter == obj_dude) {
                        v12 = critterGetStat(critter, (int)Stat.STAT_STRENGTH) + 2 * perk_level(critter, (int)Perk.PERK_HEAVE_HO);
                    } else {
                        v12 = critterGetStat(critter, (int)Stat.STAT_STRENGTH);
                    }

                    int maxRange = 3 * v12;
                    if (range >= maxRange) {
                        range = maxRange;
                    }
                }

                return range;
            }

            if (critter_flag_check(critter.pid, (int)CritterFlags.CRITTER_LONG_LIMBS)) {
                return 2;
            }

            return 1;
        }

        public static int item_w_max_ammo(f2Object ammoOrWeapon)
        {
            if (ammoOrWeapon == null) {
                return 0;
            }

            Proto proto = null;
            proto_ptr(ammoOrWeapon.pid, ref proto);

            if (proto.item.type == (int)ItemType.ITEM_TYPE_AMMO) {
                return proto.item.data.ammo.quantity;
            } else {
                return proto.item.data.weapon.ammoCapacity;
            }
        }

        static int item_w_compute_ammo_cost(f2Object obj, ref int inout_a2)
        {
            int pid;

            // TODO
            // if (inout_a2 == NULL) {
            //     return -1;
            // }

            if (obj == null) {
                return 0;
            }

            pid = obj.pid;
            if (pid == (int)ProtoID.PROTO_ID_SUPER_CATTLE_PROD || pid == (int)ProtoID.PROTO_ID_MEGA_POWER_FIST) {
                inout_a2 *= 2;
            }

            return 0;
        }

        // Returns action points required for hit mode.
        static int item_w_mp_cost(f2Object critter, int hitMode, bool aiming)
        {
            int actionPoints = 0;

            // // NOTE: Uninline.
            // Object* weapon = item_hit_with(critter, hitMode);

            // if (hitMode == HIT_MODE_LEFT_WEAPON_RELOAD || hitMode == HIT_MODE_RIGHT_WEAPON_RELOAD) {
            //     if (weapon != NULL) {
            //         Proto* proto;
            //         proto_ptr(weapon->pid, &proto);
            //         if (proto->item.data.weapon.perk == PERK_WEAPON_FAST_RELOAD) {
            //             return 1;
            //         }

            //         if (weapon->pid == PROTO_ID_SOLAR_SCORCHER) {
            //             return 0;
            //         }
            //     }
            //     return 2;
            // }

            // switch (hitMode) {
            // case HIT_MODE_PALM_STRIKE:
            //     actionPoints = 6;
            //     break;
            // case HIT_MODE_PIERCING_STRIKE:
            //     actionPoints = 8;
            //     break;
            // case HIT_MODE_STRONG_KICK:
            // case HIT_MODE_SNAP_KICK:
            // case HIT_MODE_POWER_KICK:
            //     actionPoints = 4;
            //     break;
            // case HIT_MODE_HIP_KICK:
            // case HIT_MODE_HOOK_KICK:
            //     actionPoints = 7;
            //     break;
            // case HIT_MODE_PIERCING_KICK:
            //     actionPoints = 9;
            //     break;
            // default:
            //     // TODO: Inverse conditions.
            //     if (weapon != NULL && hitMode != HIT_MODE_PUNCH && hitMode != HIT_MODE_KICK && hitMode != HIT_MODE_STRONG_PUNCH && hitMode != HIT_MODE_HAMMER_PUNCH && hitMode != HIT_MODE_HAYMAKER) {
            //         if (hitMode == HIT_MODE_LEFT_WEAPON_PRIMARY || hitMode == HIT_MODE_RIGHT_WEAPON_PRIMARY) {
            //             // NOTE: Uninline.
            //             actionPoints = item_w_primary_mp_cost(weapon);
            //         } else {
            //             // NOTE: Uninline.
            //             actionPoints = item_w_secondary_mp_cost(weapon);
            //         }

            //         if (critter == obj_dude) {
            //             if (trait_level(TRAIT_FAST_SHOT)) {
            //                 if (item_w_range(critter, hitMode) > 2) {
            //                     actionPoints--;
            //                 }
            //             }
            //         }
            //     } else {
            //         actionPoints = 3;
            //     }
            //     break;
            // }

            // if (critter == obj_dude) {
            //     int attackType = item_w_subtype(weapon, hitMode);

            //     if (perkHasRank(obj_dude, PERK_BONUS_HTH_ATTACKS)) {
            //         if (attackType == ATTACK_TYPE_MELEE || attackType == ATTACK_TYPE_UNARMED) {
            //             actionPoints -= 1;
            //         }
            //     }

            //     if (perkHasRank(obj_dude, PERK_BONUS_RATE_OF_FIRE)) {
            //         if (attackType == ATTACK_TYPE_RANGED) {
            //             actionPoints -= 1;
            //         }
            //     }
            // }

            // if (aiming) {
            //     actionPoints += 1;
            // }

            // if (actionPoints < 1) {
            //     actionPoints = 1;
            // }

            return actionPoints;
        }
    }
}
