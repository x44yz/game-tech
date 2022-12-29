using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public enum ProtoID
    {
        PROTO_ID_POWER_ARMOR = 3,
        PROTO_ID_SMALL_ENERGY_CELL = 38,
        PROTO_ID_MICRO_FUSION_CELL = 39,
        PROTO_ID_STIMPACK = 40,
        PROTO_ID_MONEY = 41,
        PROTO_ID_FIRST_AID_KIT = 47,
        PROTO_ID_RADAWAY = 48,
        PROTO_ID_DYNAMITE_I = 51,
        PROTO_ID_GEIGER_COUNTER_I = 52,
        PROTO_ID_MENTATS = 53,
        PROTO_ID_STEALTH_BOY_I = 54,
        PROTO_ID_MOTION_SENSOR = 59,
        PROTO_ID_BIG_BOOK_OF_SCIENCE = 73,
        PROTO_ID_DEANS_ELECTRONICS = 76,
        PROTO_ID_FLARE = 79,
        PROTO_ID_FIRST_AID_BOOK = 80,
        PROTO_ID_PLASTIC_EXPLOSIVES_I = 85,
        PROTO_ID_SCOUT_HANDBOOK = 86,
        PROTO_ID_BUFF_OUT = 87,
        PROTO_ID_DOCTORS_BAG = 91,
        PROTO_ID_GUNS_AND_BULLETS = 102,
        PROTO_ID_NUKA_COLA = 106,
        PROTO_ID_PSYCHO = 110,
        PROTO_ID_BEER = 124,
        PROTO_ID_BOOZE = 125,
        PROTO_ID_SUPER_STIMPACK = 144,
        PROTO_ID_MOLOTOV_COCKTAIL = 159,
        PROTO_ID_LIT_FLARE = 205,
        PROTO_ID_DYNAMITE_II = 206, // armed
        PROTO_ID_GEIGER_COUNTER_II = 207,
        PROTO_ID_PLASTIC_EXPLOSIVES_II = 209, // armed
        PROTO_ID_STEALTH_BOY_II = 210,
        PROTO_ID_HARDENED_POWER_ARMOR = 232,
        PROTO_ID_JET = 259,
        PROTO_ID_JET_ANTIDOTE = 260,
        PROTO_ID_HEALING_POWDER = 273,
        PROTO_ID_DECK_OF_TRAGIC_CARDS = 304,
        PROTO_ID_CATS_PAW_ISSUE_5 = 331,
        PROTO_ID_ADVANCED_POWER_ARMOR = 348,
        PROTO_ID_ADVANCED_POWER_ARMOR_MK_II = 349,
        PROTO_ID_SHIV = 383,
        PROTO_ID_SOLAR_SCORCHER = 390,
        PROTO_ID_SUPER_CATTLE_PROD = 399,
        PROTO_ID_MEGA_POWER_FIST = 407,
        PROTO_ID_FIELD_MEDIC_FIRST_AID_KIT = 408,
        PROTO_ID_PARAMEDICS_BAG = 409,
        PROTO_ID_RAMIREZ_BOX_CLOSED = 431,
        PROTO_ID_MIRRORED_SHADES = 433,
        PROTO_ID_RAIDERS_MAP = 444,
        PROTO_ID_CAR_TRUNK = 455,
        PROTO_ID_PIP_BOY_LINGUAL_ENHANCER = 499,
        PROTO_ID_PIP_BOY_MEDICAL_ENHANCER = 516,
        PROTO_ID_SURVEY_MAP = 523,
    };

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

    public enum ItemType {
        ITEM_TYPE_ARMOR,
        ITEM_TYPE_CONTAINER,
        ITEM_TYPE_DRUG,
        ITEM_TYPE_WEAPON,
        ITEM_TYPE_AMMO,
        ITEM_TYPE_MISC,
        ITEM_TYPE_KEY,
        ITEM_TYPE_COUNT,
    };

    public enum WeaponAnimation {
        WEAPON_ANIMATION_NONE,
        WEAPON_ANIMATION_KNIFE, // d
        WEAPON_ANIMATION_CLUB, // e
        WEAPON_ANIMATION_HAMMER, // f
        WEAPON_ANIMATION_SPEAR, // g
        WEAPON_ANIMATION_PISTOL, // h
        WEAPON_ANIMATION_SMG, // i
        WEAPON_ANIMATION_SHOTGUN, // j
        WEAPON_ANIMATION_LASER_RIFLE, // k
        WEAPON_ANIMATION_MINIGUN, // l
        WEAPON_ANIMATION_LAUNCHER, // m
        WEAPON_ANIMATION_COUNT,
    };

    // Available skills.
    public enum Skill 
    {
        SKILL_SMALL_GUNS,
        SKILL_BIG_GUNS,
        SKILL_ENERGY_WEAPONS,
        SKILL_UNARMED,
        SKILL_MELEE_WEAPONS,
        SKILL_THROWING,
        SKILL_FIRST_AID,
        SKILL_DOCTOR,
        SKILL_SNEAK,
        SKILL_LOCKPICK,
        SKILL_STEAL,
        SKILL_TRAPS,
        SKILL_SCIENCE,
        SKILL_REPAIR,
        SKILL_SPEECH,
        SKILL_BARTER,
        SKILL_GAMBLING,
        SKILL_OUTDOORSMAN,
        SKILL_COUNT,
    };

    public enum DamageType {
        DAMAGE_TYPE_NORMAL,
        DAMAGE_TYPE_LASER,
        DAMAGE_TYPE_FIRE,
        DAMAGE_TYPE_PLASMA,
        DAMAGE_TYPE_ELECTRICAL,
        DAMAGE_TYPE_EMP,
        DAMAGE_TYPE_EXPLOSION,
        DAMAGE_TYPE_COUNT,
    };

    public enum HitMode 
    {
        HIT_MODE_LEFT_WEAPON_PRIMARY = 0,
        HIT_MODE_LEFT_WEAPON_SECONDARY = 1,
        HIT_MODE_RIGHT_WEAPON_PRIMARY = 2,
        HIT_MODE_RIGHT_WEAPON_SECONDARY = 3,
        HIT_MODE_PUNCH = 4,
        HIT_MODE_KICK = 5,
        HIT_MODE_LEFT_WEAPON_RELOAD = 6,
        HIT_MODE_RIGHT_WEAPON_RELOAD = 7,

        // Punch Level 2
        HIT_MODE_STRONG_PUNCH = 8,

        // Punch Level 3
        HIT_MODE_HAMMER_PUNCH = 9,

        // Punch Level 4 aka 'Lightning Punch'
        HIT_MODE_HAYMAKER = 10,

        // Punch Level 5 aka 'Chop Punch'
        HIT_MODE_JAB = 11,

        // Punch Level 6 aka 'Dragon Punch'
        HIT_MODE_PALM_STRIKE = 12,

        // Punch Level 7 aka 'Force Punch'
        HIT_MODE_PIERCING_STRIKE = 13,

        // Kick Level 2
        HIT_MODE_STRONG_KICK = 14,

        // Kick Level 3
        HIT_MODE_SNAP_KICK = 15,

        // Kick Level 4 aka 'Roundhouse Kick'
        HIT_MODE_POWER_KICK = 16,

        // Kick Level 5
        HIT_MODE_HIP_KICK = 17,

        // Kick Level 6 aka 'Jump Kick'
        HIT_MODE_HOOK_KICK = 18,

        // Kick Level 7 aka 'Death Blossom Kick'
        HIT_MODE_PIERCING_KICK = 19,
        HIT_MODE_COUNT,
        FIRST_ADVANCED_PUNCH_HIT_MODE = HIT_MODE_STRONG_PUNCH,
        LAST_ADVANCED_PUNCH_HIT_MODE = HIT_MODE_PIERCING_STRIKE,
        FIRST_ADVANCED_KICK_HIT_MODE = HIT_MODE_STRONG_KICK,
        LAST_ADVANCED_KICK_HIT_MODE = HIT_MODE_PIERCING_KICK,
        FIRST_ADVANCED_UNARMED_HIT_MODE = FIRST_ADVANCED_PUNCH_HIT_MODE,
        LAST_ADVANCED_UNARMED_HIT_MODE = LAST_ADVANCED_KICK_HIT_MODE,
    };

    public enum AttackType {
        ATTACK_TYPE_NONE,
        ATTACK_TYPE_UNARMED,
        ATTACK_TYPE_MELEE,
        ATTACK_TYPE_THROW,
        ATTACK_TYPE_RANGED,
        ATTACK_TYPE_COUNT,
    };

    public enum CombatDifficulty {
        COMBAT_DIFFICULTY_EASY,
        COMBAT_DIFFICULTY_NORMAL,
        COMBAT_DIFFICULTY_HARD,
    };

    public enum GameDifficulty {
        GAME_DIFFICULTY_EASY,
        GAME_DIFFICULTY_NORMAL,
        GAME_DIFFICULTY_HARD,
    };

    public enum KillType {
        KILL_TYPE_MAN,
        KILL_TYPE_WOMAN,
        KILL_TYPE_CHILD,
        KILL_TYPE_SUPER_MUTANT,
        KILL_TYPE_GHOUL,
        KILL_TYPE_BRAHMIN,
        KILL_TYPE_RADSCORPION,
        KILL_TYPE_RAT,
        KILL_TYPE_FLOATER,
        KILL_TYPE_CENTAUR,
        KILL_TYPE_ROBOT,
        KILL_TYPE_DOG,
        KILL_TYPE_MANTIS,
        KILL_TYPE_DEATH_CLAW,
        KILL_TYPE_PLANT,
        KILL_TYPE_GECKO,
        KILL_TYPE_ALIEN,
        KILL_TYPE_GIANT_ANT,
        KILL_TYPE_BIG_BAD_BOSS,
        KILL_TYPE_COUNT,
    };

    public enum CritterFlags {
        CRITTER_BARTER = 0x02,
        CRITTER_NO_STEAL = 0x20,
        CRITTER_NO_DROP = 0x40,
        CRITTER_NO_LIMBS = 0x80,
        CRITTER_NO_AGE = 0x100,
        CRITTER_NO_HEAL = 0x200,
        CRITTER_INVULNERABLE = 0x400,
        CRITTER_FLAT = 0x800,
        CRITTER_SPECIAL_DEATH = 0x1000,
        CRITTER_LONG_LIMBS = 0x2000,
        CRITTER_NO_KNOCKBACK = 0x4000,
    };

    public enum Gender {
        GENDER_MALE,
        GENDER_FEMALE,
        GENDER_COUNT,
    };

    public enum ItemProtoExtendedFlags {
        ItemProtoExtendedFlags_BigGun = 0x0100,
        ItemProtoExtendedFlags_IsTwoHanded = 0x0200,
        ItemProtoExtendedFlags_0x0800 = 0x0800,
        ItemProtoExtendedFlags_0x1000 = 0x1000,
        ItemProtoExtendedFlags_0x2000 = 0x2000,
        ItemProtoExtendedFlags_0x8000 = 0x8000,

        // This flag is used on weapons to indicate that's an natural (integral)
        // part of it's owner, for example Claw, or Robot's Rocket Launcher. Items
        // with this flag on do count toward total weight and cannot be dropped.
        ItemProtoExtendedFlags_NaturalWeapon = 0x08000000,
    };

    public enum HitLocation {
        HIT_LOCATION_HEAD,
        HIT_LOCATION_LEFT_ARM,
        HIT_LOCATION_RIGHT_ARM,
        HIT_LOCATION_TORSO,
        HIT_LOCATION_RIGHT_LEG,
        HIT_LOCATION_LEFT_LEG,
        HIT_LOCATION_EYES,
        HIT_LOCATION_GROIN,
        HIT_LOCATION_UNCALLED,
        HIT_LOCATION_COUNT,
        HIT_LOCATION_SPECIFIC_COUNT = HIT_LOCATION_COUNT - 1,
    };

    // Basic animations: 0-19
    // Knockdown and death: 20-35
    // Change positions: 36-37
    // Weapon: 38-47
    // Single-frame death animations (the last frame of knockdown and death animations): 48-63
    public enum AnimationType {
        ANIM_STAND = 0,
        ANIM_WALK = 1,
        ANIM_JUMP_BEGIN = 2,
        ANIM_JUMP_END = 3,
        ANIM_CLIMB_LADDER = 4,
        ANIM_FALLING = 5,
        ANIM_UP_STAIRS_RIGHT = 6,
        ANIM_UP_STAIRS_LEFT = 7,
        ANIM_DOWN_STAIRS_RIGHT = 8,
        ANIM_DOWN_STAIRS_LEFT = 9,
        ANIM_MAGIC_HANDS_GROUND = 10,
        ANIM_MAGIC_HANDS_MIDDLE = 11,
        ANIM_MAGIC_HANDS_UP = 12,
        ANIM_DODGE_ANIM = 13,
        ANIM_HIT_FROM_FRONT = 14,
        ANIM_HIT_FROM_BACK = 15,
        ANIM_THROW_PUNCH = 16,
        ANIM_KICK_LEG = 17,
        ANIM_THROW_ANIM = 18,
        ANIM_RUNNING = 19,
        ANIM_FALL_BACK = 20,
        ANIM_FALL_FRONT = 21,
        ANIM_BAD_LANDING = 22,
        ANIM_BIG_HOLE = 23,
        ANIM_CHARRED_BODY = 24,
        ANIM_CHUNKS_OF_FLESH = 25,
        ANIM_DANCING_AUTOFIRE = 26,
        ANIM_ELECTRIFY = 27,
        ANIM_SLICED_IN_HALF = 28,
        ANIM_BURNED_TO_NOTHING = 29,
        ANIM_ELECTRIFIED_TO_NOTHING = 30,
        ANIM_EXPLODED_TO_NOTHING = 31,
        ANIM_MELTED_TO_NOTHING = 32,
        ANIM_FIRE_DANCE = 33,
        ANIM_FALL_BACK_BLOOD = 34,
        ANIM_FALL_FRONT_BLOOD = 35,
        ANIM_PRONE_TO_STANDING = 36,
        ANIM_BACK_TO_STANDING = 37,
        ANIM_TAKE_OUT = 38,
        ANIM_PUT_AWAY = 39,
        ANIM_PARRY_ANIM = 40,
        ANIM_THRUST_ANIM = 41,
        ANIM_SWING_ANIM = 42,
        ANIM_POINT = 43,
        ANIM_UNPOINT = 44,
        ANIM_FIRE_SINGLE = 45,
        ANIM_FIRE_BURST = 46,
        ANIM_FIRE_CONTINUOUS = 47,
        ANIM_FALL_BACK_SF = 48,
        ANIM_FALL_FRONT_SF = 49,
        ANIM_BAD_LANDING_SF = 50,
        ANIM_BIG_HOLE_SF = 51,
        ANIM_CHARRED_BODY_SF = 52,
        ANIM_CHUNKS_OF_FLESH_SF = 53,
        ANIM_DANCING_AUTOFIRE_SF = 54,
        ANIM_ELECTRIFY_SF = 55,
        ANIM_SLICED_IN_HALF_SF = 56,
        ANIM_BURNED_TO_NOTHING_SF = 57,
        ANIM_ELECTRIFIED_TO_NOTHING_SF = 58,
        ANIM_EXPLODED_TO_NOTHING_SF = 59,
        ANIM_MELTED_TO_NOTHING_SF = 60,
        ANIM_FIRE_DANCE_SF = 61,
        ANIM_FALL_BACK_BLOOD_SF = 62,
        ANIM_FALL_FRONT_BLOOD_SF = 63,
        ANIM_CALLED_SHOT_PIC = 64,
        ANIM_COUNT = 65,
        FIRST_KNOCKDOWN_AND_DEATH_ANIM = ANIM_FALL_BACK,
        LAST_KNOCKDOWN_AND_DEATH_ANIM = ANIM_FALL_FRONT_BLOOD,
        FIRST_SF_DEATH_ANIM = ANIM_FALL_BACK_SF,
        LAST_SF_DEATH_ANIM = ANIM_FALL_FRONT_BLOOD_SF,
    };

    public enum Roll {
        ROLL_CRITICAL_FAILURE,
        ROLL_FAILURE,
        ROLL_SUCCESS,
        ROLL_CRITICAL_SUCCESS,
    };

    public enum DudeState {
        DUDE_STATE_SNEAKING = 0, // 潜行
        DUDE_STATE_LEVEL_UP_AVAILABLE = 3,
        DUDE_STATE_ADDICTED = 4,
    };

    public static class f2DEF
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
    }
}
