using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        public static bool is_hit_from_front(f2Object a1, f2Object a2)
        {
            // int diff = a1.rotation - a2.rotation;
            // if (diff < 0) {
            //     diff = -diff;
            // }

            // return diff != 0 && diff != 1 && diff != 5;
            return true;
        }

        static int action_attack(Attack attack)
        {
            // if (register_clear(attack->attacker) == -2) {
            //     return -1;
            // }

            // if (register_clear(attack->defender) == -2) {
            //     return -1;
            // }

            // for (int index = 0; index < attack->extrasLength; index++) {
            //     if (register_clear(attack->extras[index]) == -2) {
            //         return -1;
            //     }
            // }

            // int anim = item_w_anim(attack->attacker, attack->hitMode);
            // if (anim < ANIM_FIRE_SINGLE && anim != ANIM_THROW_ANIM) {
            //     return action_melee(attack, anim);
            // } else {
            //     return action_ranged(attack, anim);
            // }
            return -1;
        }
    }
}

