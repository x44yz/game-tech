using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public class f2Item
    {
        public int pid;
        public int flags;

        public int item_weight()
        {
            // TODO
            return 0;
        }

        public int item_get_type()
        {
            // TODO
            return 0;
        }

        public int item_w_anim_code()
        {
            // TODO
            return 0;
        }

        public int item_w_perk()
        {
            // TODO
            return 0;
        }

        // GetAttackTypeForHitMode
        public int item_w_subtype(int hitMode)
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

        public int item_w_dr_adjust()
        {
            // TODO
            return 0;
        }

        public int item_w_dam_mult()
        {
            // TODO
            return 0;
        }

        public int item_w_dam_div()
        {
            // TODO
            return 0;
        }

        public int item_w_damage(int hitMode)
        {
            // TODO
            return 0;
        }
    }
}
