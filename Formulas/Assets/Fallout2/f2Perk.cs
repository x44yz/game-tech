using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        // An array of perk ranks for each party member.
        public static PerkRankData perkLevelDataList = null;

        // Returns true if perk is valid.
        public static bool perkIsValid(int perk)
        {
            return perk >= 0 && perk < (int)Perk.PERK_COUNT;
        }

        public static int partyMemberMaxCount = 0;
        // List of party members, it's length is [partyMemberMaxCount] + 20.
        // static PartyMember partyMemberList = NULL;

        public static PerkRankData perkGetLevelData(f2Object critter)
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
        public static bool perkHasRank(f2Object critter, Perk perk)
        {
            return perk_level(critter, (int)perk) != 0;
        }

        public static int perk_level(f2Object critter, int perk)
        {
            if (!perkIsValid(perk)) {
                return 0;
            }

            PerkRankData ranksData = perkGetLevelData(critter);
            return ranksData.ranks[perk];
        }

        // Returns modifier to specified skill accounting for perks.
        public static int perk_adjust_skill(f2Object critter, int skill)
        {
            int modifier = 0;

            switch ((Skill)skill) {
            case Skill.SKILL_FIRST_AID:
                if (perkHasRank(critter, Perk.PERK_MEDIC)) {
                    modifier += 10;
                }

                if (perkHasRank(critter, Perk.PERK_VAULT_CITY_TRAINING)) {
                    modifier += 5;
                }

                break;
            case Skill.SKILL_DOCTOR:
                if (perkHasRank(critter, Perk.PERK_MEDIC)) {
                    modifier += 10;
                }

                if (perkHasRank(critter, Perk.PERK_LIVING_ANATOMY)) {
                    modifier += 10;
                }

                if (perkHasRank(critter, Perk.PERK_VAULT_CITY_TRAINING)) {
                    modifier += 5;
                }

                break;
            case Skill.SKILL_SNEAK:
            case Skill.SKILL_LOCKPICK:
            case Skill.SKILL_STEAL:
            case Skill.SKILL_TRAPS:
                if (skill == (int)Skill.SKILL_SNEAK)
                {
                    if (perkHasRank(critter, Perk.PERK_GHOST)) {
                        // TODO
                        // int lightIntensity = obj_get_visible_light(obj_dude);
                        // if (lightIntensity > 45875) {
                        //     modifier += 20;
                        // }
                    }
                }

                if (perkHasRank(critter, Perk.PERK_THIEF)) {
                    modifier += 10;
                }

                if (skill == (int)Skill.SKILL_LOCKPICK || skill == (int)Skill.SKILL_STEAL) {
                    if (perkHasRank(critter, Perk.PERK_MASTER_THIEF)) {
                        modifier += 15;
                    }
                }

                if (skill == (int)Skill.SKILL_STEAL) {
                    if (perkHasRank(critter, Perk.PERK_HARMLESS)) {
                        modifier += 20;
                    }
                }

                break;
            case Skill.SKILL_SCIENCE:
            case Skill.SKILL_REPAIR:
                if (perkHasRank(critter, Perk.PERK_MR_FIXIT)) {
                    modifier += 10;
                }

                break;
            case Skill.SKILL_SPEECH:
            case Skill.SKILL_BARTER:
                if (skill == (int)Skill.SKILL_SPEECH)
                {
                    if (perkHasRank(critter, Perk.PERK_SPEAKER)) {
                        modifier += 20;
                    }

                    if (perkHasRank(critter, Perk.PERK_EXPERT_EXCREMENT_EXPEDITOR)) {
                        modifier += 5;
                    }
                }

                if (perkHasRank(critter, Perk.PERK_NEGOTIATOR)) {
                    modifier += 10;
                }

                if (skill == (int)Skill.SKILL_BARTER) {
                    if (perkHasRank(critter, Perk.PERK_SALESMAN)) {
                        modifier += 20;
                    }
                }

                break;
            case Skill.SKILL_GAMBLING:
                if (perkHasRank(critter, Perk.PERK_GAMBLER)) {
                    modifier += 20;
                }

                break;
            case Skill.SKILL_OUTDOORSMAN:
                if (perkHasRank(critter, Perk.PERK_RANGER)) {
                    modifier += 15;
                }

                if (perkHasRank(critter, Perk.PERK_SURVIVALIST)) {
                    modifier += 25;
                }

                break;
            }

            return modifier;
        }
    }
}
