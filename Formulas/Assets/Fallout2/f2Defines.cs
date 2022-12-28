using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public enum Perk 
    {
        PERK_AWARENESS,
        PERK_BONUS_HTH_ATTACKS,
        PERK_BONUS_HTH_DAMAGE,
        PERK_BONUS_MOVE,
        PERK_BONUS_RANGED_DAMAGE,
        PERK_BONUS_RATE_OF_FIRE,
        PERK_EARLIER_SEQUENCE,
        PERK_FASTER_HEALING,
        PERK_MORE_CRITICALS,
        PERK_NIGHT_VISION,
        PERK_PRESENCE,
        PERK_RAD_RESISTANCE,
        PERK_TOUGHNESS,
        PERK_STRONG_BACK,
        PERK_SHARPSHOOTER,
        PERK_SILENT_RUNNING,
        PERK_SURVIVALIST,
        PERK_MASTER_TRADER,
        PERK_EDUCATED,
        PERK_HEALER,
        PERK_FORTUNE_FINDER,
        PERK_BETTER_CRITICALS,
        PERK_EMPATHY,
        PERK_SLAYER,
        PERK_SNIPER,
        PERK_SILENT_DEATH,
        PERK_ACTION_BOY,
        PERK_MENTAL_BLOCK,
        PERK_LIFEGIVER,
        PERK_DODGER,
        PERK_SNAKEATER,
        PERK_MR_FIXIT,
        PERK_MEDIC,
        PERK_MASTER_THIEF,
        PERK_SPEAKER,
        PERK_HEAVE_HO,
        PERK_FRIENDLY_FOE,
        PERK_PICKPOCKET,
        PERK_GHOST,
        PERK_CULT_OF_PERSONALITY,
        PERK_SCROUNGER,
        PERK_EXPLORER,
        PERK_FLOWER_CHILD,
        PERK_PATHFINDER,
        PERK_ANIMAL_FRIEND,
        PERK_SCOUT,
        PERK_MYSTERIOUS_STRANGER,
        PERK_RANGER,
        PERK_QUICK_POCKETS,
        PERK_SMOOTH_TALKER,
        PERK_SWIFT_LEARNER,
        PERK_TAG,
        PERK_MUTATE,
        PERK_NUKA_COLA_ADDICTION,
        PERK_BUFFOUT_ADDICTION,
        PERK_MENTATS_ADDICTION,
        PERK_PSYCHO_ADDICTION,
        PERK_RADAWAY_ADDICTION,
        PERK_WEAPON_LONG_RANGE,
        PERK_WEAPON_ACCURATE,
        PERK_WEAPON_PENETRATE,
        PERK_WEAPON_KNOCKBACK,
        PERK_POWERED_ARMOR,
        PERK_COMBAT_ARMOR,
        PERK_WEAPON_SCOPE_RANGE,
        PERK_WEAPON_FAST_RELOAD,
        PERK_WEAPON_NIGHT_SIGHT,
        PERK_WEAPON_FLAMEBOY,
        PERK_ARMOR_ADVANCED_I,
        PERK_ARMOR_ADVANCED_II,
        PERK_JET_ADDICTION,
        PERK_TRAGIC_ADDICTION,
        PERK_ARMOR_CHARISMA,
        PERK_GECKO_SKINNING,
        PERK_DERMAL_IMPACT_ARMOR,
        PERK_DERMAL_IMPACT_ASSAULT_ENHANCEMENT,
        PERK_PHOENIX_ARMOR_IMPLANTS,
        PERK_PHOENIX_ASSAULT_ENHANCEMENT,
        PERK_VAULT_CITY_INOCULATIONS,
        PERK_ADRENALINE_RUSH,
        PERK_CAUTIOUS_NATURE,
        PERK_COMPREHENSION,
        PERK_DEMOLITION_EXPERT,
        PERK_GAMBLER,
        PERK_GAIN_STRENGTH,
        PERK_GAIN_PERCEPTION,
        PERK_GAIN_ENDURANCE,
        PERK_GAIN_CHARISMA,
        PERK_GAIN_INTELLIGENCE,
        PERK_GAIN_AGILITY,
        PERK_GAIN_LUCK,
        PERK_HARMLESS,
        PERK_HERE_AND_NOW,
        PERK_HTH_EVADE,
        PERK_KAMA_SUTRA_MASTER,
        PERK_KARMA_BEACON,
        PERK_LIGHT_STEP,
        PERK_LIVING_ANATOMY,
        PERK_MAGNETIC_PERSONALITY,
        PERK_NEGOTIATOR,
        PERK_PACK_RAT,
        PERK_PYROMANIAC,
        PERK_QUICK_RECOVERY,
        PERK_SALESMAN,
        PERK_STONEWALL,
        PERK_THIEF,
        PERK_WEAPON_HANDLING,
        PERK_VAULT_CITY_TRAINING,
        PERK_ALCOHOL_RAISED_HIT_POINTS,
        PERK_ALCOHOL_RAISED_HIT_POINTS_II,
        PERK_ALCOHOL_LOWERED_HIT_POINTS,
        PERK_ALCOHOL_LOWERED_HIT_POINTS_II,
        PERK_AUTODOC_RAISED_HIT_POINTS,
        PERK_AUTODOC_RAISED_HIT_POINTS_II,
        PERK_AUTODOC_LOWERED_HIT_POINTS,
        PERK_AUTODOC_LOWERED_HIT_POINTS_II,
        PERK_EXPERT_EXCREMENT_EXPEDITOR,
        PERK_WEAPON_ENHANCED_KNOCKOUT,
        PERK_JINXED,
        PERK_COUNT,
    };

    // 特征/天赋
    // Available traits.
    public enum Trait 
    {
        TRAIT_FAST_METABOLISM,
        TRAIT_BRUISER,
        TRAIT_SMALL_FRAME,
        TRAIT_ONE_HANDER,
        TRAIT_FINESSE,
        TRAIT_KAMIKAZE,
        TRAIT_HEAVY_HANDED,
        TRAIT_FAST_SHOT,
        TRAIT_BLOODY_MESS,
        TRAIT_JINXED,
        TRAIT_GOOD_NATURED,
        TRAIT_CHEM_RELIANT,
        TRAIT_CHEM_RESISTANT,
        TRAIT_SEX_APPEAL,
        TRAIT_SKILLED,
        TRAIT_GIFTED,
        TRAIT_COUNT,
    };

    public enum ObjectFlags : uint
    {
        OBJECT_HIDDEN = 0x01,

        // Specifies that the object should not be saved to the savegame file.
        //
        // This flag is used in these situations:
        //  - To prevent saving of system objects like dude (which has separate
        // saving routine), egg, mouse cursors, etc.
        //  - To prevent saving of temporary objects (projectiles, explosion
        // effects, etc.).
        //  - To prevent saving of objects which cannot be removed for some reason,
        // like objects trying to delete themselves from scripting engine (used
        // together with `OBJECT_HIDDEN` to prevent affecting game world).
        OBJECT_NO_SAVE = 0x04,
        OBJECT_FLAT = 0x08,
        OBJECT_NO_BLOCK = 0x10,
        OBJECT_LIGHTING = 0x20,

        // Specifies that the object should not be removed (freed) from the game
        // world for whatever reason.
        //
        // This flag is used to prevent freeing of system objects like dude, egg,
        // mouse cursors, etc.
        OBJECT_NO_REMOVE = 0x400,
        OBJECT_MULTIHEX = 0x800,
        OBJECT_NO_HIGHLIGHT = 0x1000,
        OBJECT_QUEUED = 0x2000, // set if there was/is any event for the object
        OBJECT_TRANS_RED = 0x4000,
        OBJECT_TRANS_NONE = 0x8000,
        OBJECT_TRANS_WALL = 0x10000,
        OBJECT_TRANS_GLASS = 0x20000,
        OBJECT_TRANS_STEAM = 0x40000,
        OBJECT_TRANS_ENERGY = 0x80000,
        OBJECT_IN_LEFT_HAND = 0x1000000,
        OBJECT_IN_RIGHT_HAND = 0x2000000,
        OBJECT_WORN = 0x4000000,
        OBJECT_WALL_TRANS_END = 0x10000000,
        OBJECT_LIGHT_THRU = 0x20000000,
        OBJECT_SEEN = 0x40000000,
        OBJECT_SHOOT_THRU = 0x80000000,

        OBJECT_IN_ANY_HAND = OBJECT_IN_LEFT_HAND | OBJECT_IN_RIGHT_HAND,
        OBJECT_EQUIPPED = OBJECT_IN_ANY_HAND | OBJECT_WORN,
        OBJECT_FLAG_0xFC000 = OBJECT_TRANS_ENERGY | OBJECT_TRANS_STEAM | OBJECT_TRANS_GLASS | OBJECT_TRANS_WALL | OBJECT_TRANS_NONE | OBJECT_TRANS_RED,
        OBJECT_OPEN_DOOR = OBJECT_SHOOT_THRU | OBJECT_LIGHT_THRU | OBJECT_NO_BLOCK,
    };

    public enum ObjType
    {
        OBJ_TYPE_ITEM,
        OBJ_TYPE_CRITTER,
        OBJ_TYPE_SCENERY,
        OBJ_TYPE_WALL,
        OBJ_TYPE_TILE,
        OBJ_TYPE_MISC,
        OBJ_TYPE_INTERFACE,
        OBJ_TYPE_INVENTORY,
        OBJ_TYPE_HEAD,
        OBJ_TYPE_BACKGROUND,
        OBJ_TYPE_SKILLDEX,
        OBJ_TYPE_COUNT,
    };

    // Available stats.
    public enum Stat {
        STAT_STRENGTH,   // 力量
        STAT_PERCEPTION, // 感知
        STAT_ENDURANCE,
        STAT_CHARISMA,
        STAT_INTELLIGENCE,
        STAT_AGILITY,
        STAT_LUCK,
        STAT_MAXIMUM_HIT_POINTS,
        STAT_MAXIMUM_ACTION_POINTS, // 最大 ap
        STAT_ARMOR_CLASS,
        STAT_UNARMED_DAMAGE,
        STAT_MELEE_DAMAGE,
        STAT_CARRY_WEIGHT,
        STAT_SEQUENCE,
        STAT_HEALING_RATE,
        STAT_CRITICAL_CHANCE,
        STAT_BETTER_CRITICALS,
        STAT_DAMAGE_THRESHOLD,
        STAT_DAMAGE_THRESHOLD_LASER,
        STAT_DAMAGE_THRESHOLD_FIRE,
        STAT_DAMAGE_THRESHOLD_PLASMA,
        STAT_DAMAGE_THRESHOLD_ELECTRICAL,
        STAT_DAMAGE_THRESHOLD_EMP,
        STAT_DAMAGE_THRESHOLD_EXPLOSION,
        STAT_DAMAGE_RESISTANCE,
        STAT_DAMAGE_RESISTANCE_LASER,
        STAT_DAMAGE_RESISTANCE_FIRE,
        STAT_DAMAGE_RESISTANCE_PLASMA,
        STAT_DAMAGE_RESISTANCE_ELECTRICAL,
        STAT_DAMAGE_RESISTANCE_EMP,
        STAT_DAMAGE_RESISTANCE_EXPLOSION,
        STAT_RADIATION_RESISTANCE,
        STAT_POISON_RESISTANCE,
        STAT_AGE, // 年龄
        STAT_GENDER,
        STAT_CURRENT_HIT_POINTS,
        STAT_CURRENT_POISON_LEVEL, // 中毒
        STAT_CURRENT_RADIATION_LEVEL, // 辐射
        STAT_COUNT,

        // Number of primary stats.
        PRIMARY_STAT_COUNT = 7,

        // Number of SPECIAL stats (primary + secondary).
        SPECIAL_STAT_COUNT = 33,

        // Number of saveable stats (i.e. excluding CURRENT pseudostats).
        SAVEABLE_STAT_COUNT = 35,
    };

    public enum Dam 
    {
        DAM_KNOCKED_OUT = 0x01,
        DAM_KNOCKED_DOWN = 0x02,
        DAM_CRIP_LEG_LEFT = 0x04,
        DAM_CRIP_LEG_RIGHT = 0x08,
        DAM_CRIP_ARM_LEFT = 0x10,
        DAM_CRIP_ARM_RIGHT = 0x20,
        DAM_BLIND = 0x40, // 致盲
        DAM_DEAD = 0x80,
        DAM_HIT = 0x100,
        DAM_CRITICAL = 0x200,
        DAM_ON_FIRE = 0x400,
        DAM_BYPASS = 0x800,
        DAM_EXPLODE = 0x1000,
        DAM_DESTROY = 0x2000,
        DAM_DROP = 0x4000,
        DAM_LOSE_TURN = 0x8000,
        DAM_HIT_SELF = 0x10000,
        DAM_LOSE_AMMO = 0x20000,
        DAM_DUD = 0x40000,
        DAM_HURT_SELF = 0x80000,
        DAM_RANDOM_HIT = 0x100000,
        DAM_CRIP_RANDOM = 0x200000,
        DAM_BACKWASH = 0x400000,
        DAM_PERFORM_REVERSE = 0x800000,
        DAM_CRIP_LEG_ANY = DAM_CRIP_LEG_LEFT | DAM_CRIP_LEG_RIGHT,
        DAM_CRIP_ARM_ANY = DAM_CRIP_ARM_LEFT | DAM_CRIP_ARM_RIGHT,
        DAM_CRIP = DAM_CRIP_LEG_ANY | DAM_CRIP_ARM_ANY | DAM_BLIND,
    };

    public static class f2DEF
    {
        public const int EXPLOSION_TARGET_COUNT = 6;
        // 60 * 60 * 10
        public const int GAME_TIME_TICKS_PER_HOUR = 36000;
        // 24 * 60 * 60 * 10
        public const int GAME_TIME_TICKS_PER_DAY = 864000;
        // 365 * 24 * 60 * 60 * 10
        public const int GAME_TIME_TICKS_PER_YEAR = 315360000;
    }
}
