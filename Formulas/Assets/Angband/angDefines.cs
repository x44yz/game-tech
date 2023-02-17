using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angband
{
    public enum eWeaponFire {
        WEAPON_FIRE_MELEE, // 近战武器
        WEAPON_FIRE_INSTANT_HIT, // 枪
        WEAPON_FIRE_PROJECTILE, // 手雷，火箭彈
        WEAPON_FIRE_AREA_EFFECT, // 比如火焰器
        WEAPON_FIRE_USE // 比如引爆雷
    };

    public enum ePedPieceTypes
    {
        PEDPIECE_TORSO, // 躯干
        PEDPIECE_MID,
        PEDPIECE_LEFTARM,
        PEDPIECE_RIGHTARM,
        PEDPIECE_LEFTLEG,
        PEDPIECE_RIGHTLEG,
        PEDPIECE_HEAD,
    };

    public enum eWeaponType
    {
        WEAPONTYPE_UNARMED,
        WEAPONTYPE_BASEBALLBAT,
        WEAPONTYPE_COLT45,
        WEAPONTYPE_UZI,
        WEAPONTYPE_SHOTGUN,
        WEAPONTYPE_AK47,
        WEAPONTYPE_M16,
        WEAPONTYPE_SNIPERRIFLE,
        WEAPONTYPE_ROCKETLAUNCHER,
        WEAPONTYPE_FLAMETHROWER,
        WEAPONTYPE_MOLOTOV,
        WEAPONTYPE_GRENADE,
        WEAPONTYPE_DETONATOR,
        WEAPONTYPE_HELICANNON,
        WEAPONTYPE_LAST_WEAPONTYPE,
        WEAPONTYPE_ARMOUR,
        WEAPONTYPE_RAMMEDBYCAR,
        WEAPONTYPE_RUNOVERBYCAR,
        WEAPONTYPE_EXPLOSION,
        WEAPONTYPE_UZI_DRIVEBY,
        WEAPONTYPE_DROWNING,
        WEAPONTYPE_FALL,
        WEAPONTYPE_UNIDENTIFIED,
        
        WEAPONTYPE_TOTALWEAPONS = WEAPONTYPE_LAST_WEAPONTYPE,
        WEAPONTYPE_TOTAL_INVENTORY_WEAPONS = 13,
    };

    public enum AnimationId
    {
        ANIM_STD_WALK,
        ANIM_STD_RUN,
        ANIM_STD_RUNFAST,
        ANIM_STD_IDLE,
        ANIM_STD_STARTWALK,
        ANIM_STD_RUNSTOP1,
        ANIM_STD_RUNSTOP2,
        ANIM_STD_IDLE_CAM,
        ANIM_STD_IDLE_HBHB,
        ANIM_STD_IDLE_TIRED,
        ANIM_STD_IDLE_BIGGUN,
        ANIM_STD_CHAT,
        ANIM_STD_HAILTAXI,
        ANIM_STD_KO_FRONT,
        ANIM_STD_KO_LEFT,
        ANIM_STD_KO_BACK,
        ANIM_STD_KO_RIGHT,
        ANIM_STD_KO_SHOT_FACE,
        ANIM_STD_KO_SHOT_STOMACH,
        ANIM_STD_KO_SHOT_ARM_L,
        ANIM_STD_KO_SHOT_ARM_R,
        ANIM_STD_KO_SHOT_LEG_L,
        ANIM_STD_KO_SHOT_LEG_R,
        ANIM_STD_SPINFORWARD_LEFT,
        ANIM_STD_SPINFORWARD_RIGHT,
        ANIM_STD_HIGHIMPACT_FRONT,
        ANIM_STD_HIGHIMPACT_LEFT,
        ANIM_STD_HIGHIMPACT_BACK,
        ANIM_STD_HIGHIMPACT_RIGHT,
        ANIM_STD_HITBYGUN_FRONT,
        ANIM_STD_HITBYGUN_LEFT,
        ANIM_STD_HITBYGUN_BACK,
        ANIM_STD_HITBYGUN_RIGHT,
        ANIM_STD_HIT_FRONT,
        ANIM_STD_HIT_LEFT,
        ANIM_STD_HIT_BACK,
        ANIM_STD_HIT_RIGHT,
        ANIM_STD_HIT_FLOOR,

        /* names made up */
    // #if GTA_VERSION <= GTA3_PS2_160
        ANIM_STD_HIT_BODY,
    // #endif
        ANIM_STD_HIT_BODYBLOW,
        ANIM_STD_HIT_CHEST,
        ANIM_STD_HIT_HEAD,
        ANIM_STD_HIT_WALK,
        /**/

        ANIM_STD_HIT_WALL,
        ANIM_STD_HIT_FLOOR_FRONT,
        ANIM_STD_HIT_BEHIND,
        ANIM_STD_PUNCH,
        ANIM_STD_KICKGROUND,

        /* names made up */
        ANIM_STD_WEAPON_BAT_H,
        ANIM_STD_WEAPON_BAT_V,
        ANIM_STD_WEAPON_HGUN_BODY,
        ANIM_STD_WEAPON_AK_BODY,
        ANIM_STD_WEAPON_PUMP,
        ANIM_STD_WEAPON_SNIPER,
        ANIM_STD_WEAPON_THROW,
        /**/

        ANIM_STD_THROW_UNDER,

        /* names made up */
        ANIM_STD_START_THROW,
        /**/

        ANIM_STD_DETONATE,

        /* names made up */
        ANIM_STD_HGUN_RELOAD,
        ANIM_STD_AK_RELOAD,
    // #ifdef PC_PLAYER_CONTROLS
        // maybe wrong define, but unused anyway
        ANIM_FPS_PUNCH,
        ANIM_FPS_BAT,
        ANIM_FPS_UZI,
        ANIM_FPS_PUMP,
        ANIM_FPS_AK,
        ANIM_FPS_M16,
        ANIM_FPS_ROCKET,
    // #endif
        /**/

        ANIM_STD_FIGHT_IDLE,
        ANIM_STD_FIGHT_2IDLE,
        ANIM_STD_FIGHT_SHUFFLE_F,

        /* names made up */
        ANIM_STD_FIGHT_BODYBLOW,
        ANIM_STD_FIGHT_HEAD,
        ANIM_STD_FIGHT_KICK,
        ANIM_STD_FIGHT_KNEE,
        ANIM_STD_FIGHT_LHOOK,
        ANIM_STD_FIGHT_PUNCH,
        ANIM_STD_FIGHT_ROUNDHOUSE,
        ANIM_STD_FIGHT_LONGKICK,
        /**/

        ANIM_STD_PARTIAL_PUNCH,
        ANIM_STD_JACKEDCAR_RHS,
        ANIM_STD_JACKEDCAR_LO_RHS,
        ANIM_STD_JACKEDCAR_LHS,
        ANIM_STD_JACKEDCAR_LO_LHS,
        ANIM_STD_QUICKJACK,
        ANIM_STD_QUICKJACKED,
        ANIM_STD_CAR_ALIGN_DOOR_LHS,
        ANIM_STD_CAR_ALIGNHI_DOOR_LHS,
        ANIM_STD_CAR_OPEN_DOOR_LHS,
        ANIM_STD_CARDOOR_LOCKED_LHS,
        ANIM_STD_CAR_PULL_OUT_PED_LHS,
        ANIM_STD_CAR_PULL_OUT_PED_LO_LHS,
        ANIM_STD_CAR_GET_IN_LHS,
        ANIM_STD_CAR_GET_IN_LO_LHS,
        ANIM_STD_CAR_CLOSE_DOOR_LHS,
        ANIM_STD_CAR_CLOSE_DOOR_LO_LHS,
        ANIM_STD_CAR_CLOSE_DOOR_ROLLING_LHS,
        ANIM_STD_CAR_CLOSE_DOOR_ROLLING_LO_LHS,
        ANIM_STD_GETOUT_LHS,
        ANIM_STD_GETOUT_LO_LHS,
        ANIM_STD_CAR_CLOSE_LHS,
        ANIM_STD_CAR_ALIGN_DOOR_RHS,
        ANIM_STD_CAR_ALIGNHI_DOOR_RHS,
        ANIM_STD_CAR_OPEN_DOOR_RHS,
        ANIM_STD_CARDOOR_LOCKED_RHS,
        ANIM_STD_CAR_PULL_OUT_PED_RHS,
        ANIM_STD_CAR_PULL_OUT_PED_LO_RHS,
        ANIM_STD_CAR_GET_IN_RHS,
        ANIM_STD_CAR_GET_IN_LO_RHS,
        ANIM_STD_CAR_CLOSE_DOOR_RHS,
        ANIM_STD_CAR_CLOSE_DOOR_LO_RHS,
        ANIM_STD_CAR_SHUFFLE_RHS,
        ANIM_STD_CAR_SHUFFLE_LO_RHS,
        ANIM_STD_CAR_SIT,
        ANIM_STD_CAR_SIT_LO,
        ANIM_STD_CAR_SIT_P,
        ANIM_STD_CAR_SIT_P_LO,
        ANIM_STD_CAR_DRIVE_LEFT,
        ANIM_STD_CAR_DRIVE_RIGHT,
        ANIM_STD_CAR_DRIVE_LEFT_LO,
        ANIM_STD_CAR_DRIVE_RIGHT_LO,
        ANIM_STD_CAR_DRIVEBY_LEFT,
        ANIM_STD_CAR_DRIVEBY_RIGHT,
        ANIM_STD_CAR_LOOKBEHIND,
        ANIM_STD_BOAT_DRIVE,
        ANIM_STD_GETOUT_RHS,
        ANIM_STD_GETOUT_LO_RHS,
        ANIM_STD_CAR_CLOSE_RHS,
        ANIM_STD_CAR_HOOKERTALK,
        ANIM_STD_COACH_OPEN_LHS,
        ANIM_STD_COACH_OPEN_RHS,
        ANIM_STD_COACH_GET_IN_LHS,
        ANIM_STD_COACH_GET_IN_RHS,
        ANIM_STD_COACH_GET_OUT_LHS,
        ANIM_STD_TRAIN_GETIN,
        ANIM_STD_TRAIN_GETOUT,
        ANIM_STD_CRAWLOUT_LHS,
        ANIM_STD_CRAWLOUT_RHS,
        ANIM_STD_VAN_OPEN_DOOR_REAR_LHS,
        ANIM_STD_VAN_GET_IN_REAR_LHS,
        ANIM_STD_VAN_CLOSE_DOOR_REAR_LHS,
        ANIM_STD_VAN_GET_OUT_REAR_LHS,
        ANIM_STD_VAN_OPEN_DOOR_REAR_RHS,
        ANIM_STD_VAN_GET_IN_REAR_RHS,
        ANIM_STD_VAN_CLOSE_DOOR_REAR_RHS,
        ANIM_STD_VAN_GET_OUT_REAR_RHS,
        ANIM_STD_GET_UP,
        ANIM_STD_GET_UP_LEFT,
        ANIM_STD_GET_UP_RIGHT,
        ANIM_STD_GET_UP_FRONT,
        ANIM_STD_JUMP_LAUNCH,
        ANIM_STD_JUMP_GLIDE,
        ANIM_STD_JUMP_LAND,
        ANIM_STD_FALL,
        ANIM_STD_FALL_GLIDE,
        ANIM_STD_FALL_LAND,
        ANIM_STD_FALL_COLLAPSE,
        ANIM_STD_EVADE_STEP,
        ANIM_STD_EVADE_DIVE,
        ANIM_STD_XPRESS_SCRATCH,
        ANIM_STD_ROADCROSS,
        ANIM_STD_TURN180,
        ANIM_STD_ARREST,
        ANIM_STD_DROWN,
        ANIM_MEDIC_CPR,
        ANIM_STD_DUCK_DOWN,
        ANIM_STD_DUCK_LOW,
        ANIM_STD_RBLOCK_SHOOT,

        /* names made up */
        ANIM_STD_THROW_UNDER2,
        /**/

        ANIM_STD_HANDSUP,
        ANIM_STD_HANDSCOWER,
        ANIM_STD_PARTIAL_FUCKU,
        ANIM_STD_PHONE_IN,
        ANIM_STD_PHONE_OUT,
        ANIM_STD_PHONE_TALK,

        ANIM_STD_NUM
    };

    // TODO: This is eFightState on mobile.
    public enum PedFightMoves
    {
        FIGHTMOVE_NULL,
        // Attacker
        FIGHTMOVE_STDPUNCH,
        FIGHTMOVE_IDLE,
        FIGHTMOVE_SHUFFLE_F,
        FIGHTMOVE_KNEE,
        FIGHTMOVE_HEADBUTT,
        FIGHTMOVE_PUNCHJAB,
        FIGHTMOVE_PUNCHHOOK,
        FIGHTMOVE_KICK,
        FIGHTMOVE_LONGKICK,
        FIGHTMOVE_ROUNDHOUSE,
        FIGHTMOVE_BODYBLOW,
        FIGHTMOVE_GROUNDKICK,
        // Opponent
        FIGHTMOVE_HITFRONT,
        FIGHTMOVE_HITBACK,
        FIGHTMOVE_HITRIGHT,
        FIGHTMOVE_HITLEFT,
        FIGHTMOVE_HITBODY,
        FIGHTMOVE_HITCHEST,
        FIGHTMOVE_HITHEAD,
        FIGHTMOVE_HITBIGSTEP,
        FIGHTMOVE_HITONFLOOR,
        FIGHTMOVE_HITBEHIND,
        FIGHTMOVE_IDLE2NORM,
        NUM_FIGHTMOVES
    };

}
