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
            if (register_clear(attack.attacker) == -2) {
                return -1;
            }

            if (register_clear(attack.defender) == -2) {
                return -1;
            }

            for (int index = 0; index < attack.extrasLength; index++) {
                if (register_clear(attack.extras[index]) == -2) {
                    return -1;
                }
            }

            // 设置对应 anim
            // int anim = item_w_anim(attack.attacker, attack.hitMode);
            // if (anim < (int)AnimationType.ANIM_FIRE_SINGLE && anim != (int)AnimationType.ANIM_THROW_ANIM) {
            //     return action_melee(attack, anim);
            // } else {
            //     return action_ranged(attack, anim);
            // }
            return -1;
        }

        public const int ANIMATION_SEQUENCE_LIST_CAPACITY = 32;
        public const int ANIMATION_DESCRIPTION_LIST_CAPACITY = 55;
        public const int ANIMATION_SAD_LIST_CAPACITY = 24;
        public const int ANIMATION_SEQUENCE_FORCED = 0x01;

        // clear animation
        static int register_clear(f2Object a1)
        {
            // for (int animationSequenceIndex = 0; animationSequenceIndex < ANIMATION_SEQUENCE_LIST_CAPACITY; animationSequenceIndex++) {
            //     AnimationSequence* animationSequence = &(anim_set[animationSequenceIndex]);
            //     if (animationSequence->field_0 == -1000) {
            //         continue;
            //     }

            //     int animationDescriptionIndex;
            //     for (animationDescriptionIndex = 0; animationDescriptionIndex < animationSequence->length; animationDescriptionIndex++) {
            //         AnimationDescription* animationDescription = &(animationSequence->animations[animationDescriptionIndex]);
            //         if (a1 != animationDescription->owner || animationDescription->kind == 11) {
            //             continue;
            //         }

            //         break;
            //     }

            //     if (animationDescriptionIndex == animationSequence->length) {
            //         continue;
            //     }

            //     if ((animationSequence->flags & ANIM_SEQ_PRIORITIZED) != 0) {
            //         return -2;
            //     }

            //     anim_set_end(animationSequenceIndex);

            //     return 0;
            // }

            return -1;
        }
    }
}

