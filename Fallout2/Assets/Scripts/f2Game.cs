using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        public const CombatDifficulty gCombatDifficulty = CombatDifficulty.COMBAT_DIFFICULTY_NORMAL;
        public const GameDifficulty gGameDifficulty = GameDifficulty.GAME_DIFFICULTY_NORMAL;

        // The minimum value of SPECIAL stat.
        public const int PRIMARY_STAT_MIN = 1;
        // The maximum value of SPECIAL stat.
        public const int PRIMARY_STAT_MAX = 10;

        public const int EXPLOSION_TARGET_COUNT = 6;
        // 60 * 60 * 10
        public const int GAME_TIME_TICKS_PER_HOUR = 36000;
        // 24 * 60 * 60 * 10
        public const int GAME_TIME_TICKS_PER_DAY = 864000;
        // 365 * 24 * 60 * 60 * 10
        public const int GAME_TIME_TICKS_PER_YEAR = 315360000;

        public const int STAT_INVALID = -1;
        // max number of tagged skills
        public const int NUM_TAGGED_SKILLS = 4;

        public static f2Object obj_dude; // 当前选中的 unit
        public static f2Object inven_dude = null; // 当前查看背包的 unit

        // TODO: Rather complex, but understandable, needs testing.
        // static int make_straight_path_func(f2Object a1, int from, int to, StraightPathNode pathNodes, f2Object a5, int a6, PathBuilderCallback* callback)
        // {
        // }

        static int tile_num_beyond(int from, int to, int distance)
        {
            // TODO
            return 0;
        }

        // Roll D10 against specified stat.
        //
        // This function is intended to be used with one of SPECIAL stats (which are
        // capped at 10, hence d10), not with artitrary stat, but does not enforce it.
        //
        // An optional [modifier] can be supplied as a bonus (or penalty) to the stat's
        // value.
        //
        // Upon return [howMuch] will be set to difference between stat's value
        // (accounting for given [modifier]) and d10 roll, which can be positive (or
        // zero) when roll succeeds, or negative when roll fails. Set [howMuch] to
        // `NULL` if you're not interested in this value.
        //
        // 0x4AFA78
        static int stat_result(f2Object critter, int stat, int modifier, ref int howMuch)
        {
            int value = critterGetStat(critter, stat) + modifier;
            int chance = roll_random(PRIMARY_STAT_MIN, PRIMARY_STAT_MAX);

            // if (howMuch != NULL) 
            {
                howMuch = value - chance;
            }

            if (chance <= value) {
                return (int)Roll.ROLL_SUCCESS;
            }

            return (int)Roll.ROLL_FAILURE;
        }

        // tile_num_in_direction
        public static int tile_num_in_direction(int tile, int rotation, int distance)
        {
            // int newTile = tile;
            // for (int index = 0; index < distance; index++) {
            //     if (tile_on_edge(newTile)) {
            //         break;
            //     }

            //     int parity = (newTile % grid_width) & 1;
            //     newTile += dir_tile[parity][rotation];
            // }

            // return newTile;
            return 0;
        }

        static int intface_get_attack(ref int hitMode, ref bool aiming)
        {
            // if (interfaceWindow == -1) {
            //     return -1;
            // }

            // *aiming = false;

            // switch (itemButtonItems[itemCurrentItem].action) {
            // case INTERFACE_ITEM_ACTION_PRIMARY_AIMING:
            //     *aiming = true;
            //     // FALLTHROUGH
            // case INTERFACE_ITEM_ACTION_PRIMARY:
            //     *hitMode = itemButtonItems[itemCurrentItem].primaryHitMode;
            //     return 0;
            // case INTERFACE_ITEM_ACTION_SECONDARY_AIMING:
            //     *aiming = true;
            //     // FALLTHROUGH
            // case INTERFACE_ITEM_ACTION_SECONDARY:
            //     *hitMode = itemButtonItems[itemCurrentItem].secondaryHitMode;
            //     return 0;
            // }

            return -1;
        }

        static void intface_update_move_points(int actionPointsLeft, int bonusActionPoints)
        {
        }

        // Returns `true` if specified object is a party member.
        static bool isPartyMember(f2Object obj)
        {
            return false;
            // if (object == NULL) {
            //     return false;
            // }

            // if (object->id < 18000) {
            //     return false;
            // }

            // bool isPartyMember = false;

            // for (int index = 0; index < partyMemberCount; index++) {
            //     if (partyMemberList[index].object == object) {
            //         isPartyMember = true;
            //         break;
            //     }
            // }

            // return isPartyMember;
        }

        static int reaction_set(f2Object critter, int value)
        {
            // scr_set_local_var(critter->sid, 0, value);
            return 0;
        }

        static int obj_dist_with_tile(f2Object object1, int tile1, f2Object object2, int tile2)
        {
            return 0;
            // if (object1 == NULL || object2 == NULL) {
            //     return 0;
            // }

            // int distance = tile_dist(tile1, tile2);

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
    }
}
