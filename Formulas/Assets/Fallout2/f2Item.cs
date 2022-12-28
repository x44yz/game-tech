using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public class f2Item
    {
        public static int FID_TYPE(int value) => f2Utils.FID_TYPE(value);
        public static int PID_TYPE(int value) => f2Utils.PID_TYPE(value);
        public static int SID_TYPE(int value) => f2Utils.SID_TYPE(value);
        public static int proto_ptr(int pid, ref Proto protoPtr) => f2Utils.proto_ptr(pid, ref protoPtr);

        public int pid;
        public int flags;

        // Returns true if [item] is an natural weapon of it's owner.
        //
        // See [ItemProtoExtendedFlags_NaturalWeapon] for more details on natural weapons.
        public static int item_is_hidden(f2Item obj)
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

        public static int item_weight(f2Item item)
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
                if (ammoQuantity > 0) {
                    // NOTE: Uninline.
                    int ammoTypePid = item_w_ammo_pid(item);
                    if (ammoTypePid != -1) {
                        Proto* ammoProto;
                        if (proto_ptr(ammoTypePid, &ammoProto) != -1) {
                            weight += ammoProto->item.weight * ((ammoQuantity - 1) / ammoProto->item.data.ammo.quantity + 1);
                        }
                    }
                }
            }

            return weight;
        }

        public static int item_get_type(f2Item item)
        {
            // TODO
            return 0;
        }

        public static int item_w_anim_code(f2Item item)
        {
            // TODO
            return 0;
        }

        public static int item_w_perk(f2Item item)
        {
            // TODO
            return 0;
        }

        // GetAttackTypeForHitMode
        public static int item_w_subtype(f2Item item, int hitMode)
        {
            // if (weapon == NULL) {
            //     return ATTACK_TYPE_UNARMED;
            // }

            // Proto* proto;
            // proto_ptr(weapon->pid, &proto);

            // int index;
            // if (hitMode == HIT_MODE_LEFT_WEAPON_PRIMARY || hitMode == HIT_MODE_RIGHT_WEAPON_PRIMARY) {
            //     index = proto->item.extendedFlags & 0xF;
            // } else {
            //     index = (proto->item.extendedFlags & 0xF0) >> 4;
            // }

            // return attack_subtype[index];
            // TODO
            return 0;
        }

        public static int item_w_dr_adjust(f2Item item)
        {
            // TODO
            return 0;
        }

        public static int item_w_dam_mult(f2Item item)
        {
            // TODO
            return 0;
        }

        public static int item_w_dam_div(f2Item item)
        {
            // TODO
            return 0;
        }

        public static int item_w_damage(f2Item item, int hitMode)
        {
            // TODO
            return 0;
        }
    }
}
