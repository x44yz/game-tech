using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using WorldTilePosition = UnityEngine.Vector2Int;

namespace angband
{
    class _object 
    {
        struct object_kind *kind;	/**< Kind of the object */
        struct ego_item *ego;		/**< Ego item info of the object, if any */
        const struct artifact *artifact;	/**< Artifact info of the object, if any */

        struct object *prev;	/**< Previous object in a pile */
        struct object *next;	/**< Next object in a pile */
        struct object *known;	/**< Known version of this object */

        uint16_t oidx;		/**< Item list index, if any */

        struct loc grid;	/**< position on map, or (0, 0) */

        uint8_t tval;		/**< Item type (from kind) */
        uint8_t sval;		/**< Item sub-type (from kind) */

        int16_t pval;		/**< Item extra-parameter */

        int16_t weight;		/**< Item weight */

        uint8_t dd;		/**< Number of damage dice */
        uint8_t ds;		/**< Number of sides on each damage die */
        int16_t ac;		/**< Normal AC */
        int16_t to_a;		/**< Plusses to AC */
        int16_t to_h;		/**< Plusses to hit */
        int16_t to_d;		/**< Plusses to damage */

        bitflag flags[OF_SIZE];	/**< Object flags */
        int16_t modifiers[OBJ_MOD_MAX];	/**< Object modifiers*/
        struct element_info el_info[ELEM_MAX];	/**< Object element info */
        bool *brands;			/**< Flag absence/presence of each brand */
        bool *slays;			/**< Flag absence/presence of each slay */
        struct curse_data *curses;	/**< Array of curse powers and timeouts */

        struct effect *effect;	/**< Effect this item produces (effects.c) */
        char *effect_msg;		/**< Message on use */
        struct activation *activation;	/**< Artifact activation, if applicable */
        random_value time;		/**< Recharge time (rods/activation) */
        int16_t timeout;		/**< Timeout Counter */

        uint8_t number;			/**< Number of items */
        bitflag notice;			/**< Attention paid to the object */

        int16_t held_m_idx;		/**< Monster holding us (if any) */
        int16_t mimicking_m_idx;	/**< Monster mimicking us (if any) */

        uint8_t origin;			/**< How this item was found */
        uint8_t origin_depth;		/**< What depth the item was found at */
        struct monster_race *origin_race;	/**< Monster race that dropped it */

        quark_t note; 			/**< Inscription index */
    };


    /**
    * A struct representing a random chance of success, such as 8 in 125 (6.4%).
    */
    public class random_chance {
        public int numerator;
        public int denominator;
    } ;

    public static class angData
    {
        // public static readonly FightMove[] tFightMoves = new FightMove[]{
        //     new FightMove(AnimationId.ANIM_STD_NUM, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_PUNCH, 0.2f, 8.0f / 30.0f, 0.0f, 0.3f, HITLEVEL_HIGH, 1, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_IDLE, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_SHUFFLE_F, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_KNEE, 4.0f / 30.0f, 0.2f, 0.0f, 0.6f, HITLEVEL_LOW, 2, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_HEAD, 4.0f / 30.0f, 0.2f, 0.0f, 0.7f, HITLEVEL_HIGH, 3, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_PUNCH, 4.0f / 30.0f, 7.0f / 30.0f, 10.0f / 30.0f, 0.4f, HITLEVEL_HIGH, 1, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_LHOOK, 8.0f / 30.0f, 10.0f / 30.0f, 0.0f, 0.4f, HITLEVEL_HIGH, 3, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_KICK, 8.0f / 30.0f, 10.0f / 30.0f, 0.0f, 0.5, HITLEVEL_MEDIUM, 2, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_LONGKICK, 8.0f / 30.0f, 10.0f / 30.0f, 0.0f, 0.5, HITLEVEL_MEDIUM, 4, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_ROUNDHOUSE, 8.0f / 30.0f, 10.0f / 30.0f, 0.0f, 0.6f, HITLEVEL_MEDIUM, 4, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_BODYBLOW, 5.0f / 30.0f, 7.0f / 30.0f, 0.0f, 0.35f, HITLEVEL_LOW, 2, 0},
        //     new FightMove(AnimationId.ANIM_STD_KICKGROUND, 10.0f / 30.0f, 14.0f / 30.0f, 0.0f, 0.4f, HITLEVEL_GROUND, 1, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_FRONT, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_BACK, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_RIGHT, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_LEFT, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_BODYBLOW, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_CHEST, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_HEAD, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_WALK, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_FLOOR, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_BEHIND, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_2IDLE, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        // };
    }
}
