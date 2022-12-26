using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using WorldTilePosition = UnityEngine.Vector2Int;

namespace gta3
{
    public struct FightMove
    {
        public AnimationId animId;
        public float startFireTime;
        public float endFireTime;
        public float comboFollowOnTime;
        public float strikeRadius;
        public int hitLevel; // FightMoveHitLevel
        public int damage;
        public int flags;
    };

    public static class g3Data
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
