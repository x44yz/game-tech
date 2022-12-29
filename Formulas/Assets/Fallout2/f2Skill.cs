using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        // Returns true if skill is valid.
        public static bool skillIsValid(int skill)
        {
            return skill >= 0 && skill < (int)Skill.SKILL_COUNT;
        }

        public static int skill_game_difficulty(int skill)
        {
            switch ((Skill)skill) {
            case Skill.SKILL_FIRST_AID:
            case Skill.SKILL_DOCTOR:
            case Skill.SKILL_SNEAK:
            case Skill.SKILL_LOCKPICK:
            case Skill.SKILL_STEAL:
            case Skill.SKILL_TRAPS:
            case Skill.SKILL_SCIENCE:
            case Skill.SKILL_REPAIR:
            case Skill.SKILL_SPEECH:
            case Skill.SKILL_BARTER:
            case Skill.SKILL_GAMBLING:
            case Skill.SKILL_OUTDOORSMAN:
                var gameDifficulty = f2DEF.gGameDifficulty;
                // config_get_value(&game_config, GAME_CONFIG_PREFERENCES_KEY, GAME_CONFIG_GAME_DIFFICULTY_KEY, &gameDifficulty);

                if (gameDifficulty == GameDifficulty.GAME_DIFFICULTY_HARD) {
                    return -10;
                } else if (gameDifficulty == GameDifficulty.GAME_DIFFICULTY_EASY) {
                    return 20;
                }
                break;
            }

            return 0;
        }
    }
}