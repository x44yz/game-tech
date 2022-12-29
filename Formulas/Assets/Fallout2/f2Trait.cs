using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        public const int TRAITS_MAX_SELECTED_COUNT = 2;

        // List of selected traits.
        // 特征/天赋
        public static int[] pc_trait = new int[TRAITS_MAX_SELECTED_COUNT];

        // Returns `true` if the specified trait is selected.
        public static bool trait_level(int trait)
        {
            return pc_trait[0] == trait || pc_trait[1] == trait;
        }

        // Returns skill modifier depending on selected traits.
        public static int trait_adjust_skill(int skill)
        {
            int modifier = 0;

            if (trait_level((int)Trait.TRAIT_GIFTED)) {
                modifier -= 10;
            }

            if (trait_level((int)Trait.TRAIT_GOOD_NATURED)) {
                switch ((Skill)skill) {
                case Skill.SKILL_SMALL_GUNS:
                case Skill.SKILL_BIG_GUNS:
                case Skill.SKILL_ENERGY_WEAPONS:
                case Skill.SKILL_UNARMED:
                case Skill.SKILL_MELEE_WEAPONS:
                case Skill.SKILL_THROWING:
                    modifier -= 10;
                    break;
                case Skill.SKILL_FIRST_AID:
                case Skill.SKILL_DOCTOR:
                case Skill.SKILL_SPEECH:
                case Skill.SKILL_BARTER:
                    modifier += 15;
                    break;
                }
            }

            return modifier;
        }
    }
}