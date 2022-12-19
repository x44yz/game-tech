using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public enum goodorevil
    {
        GOE_ANY,
        GOE_EVIL,
        GOE_GOOD,
    };

    public enum AffixItemType
    {
        // clang-format off
        None      = 0,
        Misc      = 1 << 0,
        Bow       = 1 << 1,
        Staff     = 1 << 2,
        Weapon    = 1 << 3,
        Shield    = 1 << 4,
        Armor     = 1 << 5,
        // clang-format on
    };

    public enum CharacterAttribute
    {
        Strength,
        Magic,
        Dexterity,
        Vitality,

        FIRST = Strength,
        LAST = Vitality
    };

    public enum _unique_items
    {
        UITEM_CLEAVER,
        UITEM_SKCROWN,
        UITEM_INFRARING,
        UITEM_OPTAMULET,
        UITEM_TRING,
        UITEM_HARCREST,
        UITEM_STEELVEIL,
        UITEM_ARMOFVAL,
        UITEM_GRISWOLD,
        UITEM_BOVINE,
        UITEM_RIFTBOW,
        UITEM_NEEDLER,
        UITEM_CELESTBOW,
        UITEM_DEADLYHUNT,
        UITEM_BOWOFDEAD,
        UITEM_BLKOAKBOW,
        UITEM_FLAMEDART,
        UITEM_FLESHSTING,
        UITEM_WINDFORCE,
        UITEM_EAGLEHORN,
        UITEM_GONNAGALDIRK,
        UITEM_DEFENDER,
        UITEM_GRYPHONCLAW,
        UITEM_BLACKRAZOR,
        UITEM_GIBBOUSMOON,
        UITEM_ICESHANK,
        UITEM_EXECUTIONER,
        UITEM_BONESAW,
        UITEM_SHADHAWK,
        UITEM_WIZSPIKE,
        UITEM_LGTSABRE,
        UITEM_FALCONTALON,
        UITEM_INFERNO,
        UITEM_DOOMBRINGER,
        UITEM_GRIZZLY,
        UITEM_GRANDFATHER,
        UITEM_MANGLER,
        UITEM_SHARPBEAK,
        UITEM_BLOODLSLAYER,
        UITEM_CELESTAXE,
        UITEM_WICKEDAXE,
        UITEM_STONECLEAV,
        UITEM_AGUHATCHET,
        UITEM_HELLSLAYER,
        UITEM_MESSERREAVER,
        UITEM_CRACKRUST,
        UITEM_JHOLMHAMM,
        UITEM_CIVERBS,
        UITEM_CELESTSTAR,
        UITEM_BARANSTAR,
        UITEM_GNARLROOT,
        UITEM_CRANBASH,
        UITEM_SCHAEFHAMM,
        UITEM_DREAMFLANGE,
        UITEM_STAFFOFSHAD,
        UITEM_IMMOLATOR,
        UITEM_STORMSPIRE,
        UITEM_GLEAMSONG,
        UITEM_THUNDERCALL,
        UITEM_PROTECTOR,
        UITEM_NAJPUZZLE,
        UITEM_MINDCRY,
        UITEM_RODOFONAN,
        UITEM_SPIRITSHELM,
        UITEM_THINKINGCAP,
        UITEM_OVERLORDHELM,
        UITEM_FOOLSCREST,
        UITEM_GOTTERDAM,
        UITEM_ROYCIRCLET,
        UITEM_TORNFLESH,
        UITEM_GLADBANE,
        UITEM_RAINCLOAK,
        UITEM_LEATHAUT,
        UITEM_WISDWRAP,
        UITEM_SPARKMAIL,
        UITEM_SCAVCARAP,
        UITEM_NIGHTSCAPE,
        UITEM_NAJPLATE,
        UITEM_DEMONSPIKE,
        UITEM_DEFLECTOR,
        UITEM_SKULLSHLD,
        UITEM_DRAGONBRCH,
        UITEM_BLKOAKSHLD,
        UITEM_HOLYDEF,
        UITEM_STORMSHLD,
        UITEM_BRAMBLE,
        UITEM_REGHA,
        UITEM_BLEEDER,
        UITEM_CONSTRICT,
        UITEM_ENGAGE,
        UITEM_INVALID = -1,
    };

    public enum monster_treasure
    {
        // clang-format off
        T_MASK    = 0xFFF,
        T_NODROP = 0x4000, // monster doesn't drop any loot
        T_UNIQ    = 0x8000, // use combined with unique item's ID - for example butcher's cleaver = T_UNIQ+UITEM_CLEAVE
        // clang-format on
    };

    public class monster_flag {
        // clang-format off
        public const int MFLAG_HIDDEN          = 1 << 0;
        public const int MFLAG_LOCK_ANIMATION  = 1 << 1;
        public const int MFLAG_ALLOW_SPECIAL   = 1 << 2;
        public const int MFLAG_NOHEAL          = 1 << 3;
        public const int MFLAG_TARGETS_MONSTER = 1 << 4;
        public const int MFLAG_GOLEM           = 1 << 5;
        public const int MFLAG_QUEST_COMPLETE  = 1 << 6;
        public const int MFLAG_KNOCKBACK       = 1 << 7;
        public const int MFLAG_SEARCH          = 1 << 8;
        public const int MFLAG_CAN_OPEN_DOOR   = 1 << 9;
        public const int MFLAG_NO_ENEMY        = 1 << 10;
        public const int MFLAG_BERSERK         = 1 << 11;
        public const int MFLAG_NOLIFESTEAL     = 1 << 12;
        // clang-format on
    };

    public class monster_resistance 
    {
        // clang-format off
        public const int RESIST_MAGIC     = 1 << 0;
        public const int RESIST_FIRE      = 1 << 1;
        public const int RESIST_LIGHTNING = 1 << 2;
        public const int IMMUNE_MAGIC     = 1 << 3;
        public const int IMMUNE_FIRE      = 1 << 4;
        public const int IMMUNE_LIGHTNING = 1 << 5;
        public const int IMMUNE_ACID      = 1 << 7;
        // clang-format on
    };

    public enum Direction {
        South,
        SouthWest,
        West,
        NorthWest,
        North,
        NorthEast,
        East,
        SouthEast,
        NoDirection
    };

    public enum icreateinfo_flag2 {
        // clang-format off
        CF_HELLFIRE = 1,
        // clang-format on
    };

    public enum item_quality 
    {
        ITEM_QUALITY_NORMAL,
        ITEM_QUALITY_MAGIC,
        ITEM_QUALITY_UNIQUE,
    };

    public enum item_effect_type
    {
        IPL_TOHIT,
        IPL_TOHIT_CURSE,
        IPL_DAMP,
        IPL_DAMP_CURSE,
        IPL_TOHIT_DAMP,
        IPL_TOHIT_DAMP_CURSE,
        IPL_ACP,
        IPL_ACP_CURSE,
        IPL_FIRERES,
        IPL_LIGHTRES,
        IPL_MAGICRES,
        IPL_ALLRES,
        IPL_SPLLVLADD = 14,
        IPL_CHARGES,
        IPL_FIREDAM,
        IPL_LIGHTDAM,
        IPL_STR = 19,
        IPL_STR_CURSE,
        IPL_MAG,
        IPL_MAG_CURSE,
        IPL_DEX,
        IPL_DEX_CURSE,
        IPL_VIT,
        IPL_VIT_CURSE,
        IPL_ATTRIBS,
        IPL_ATTRIBS_CURSE,
        IPL_GETHIT_CURSE,
        IPL_GETHIT,
        IPL_LIFE,
        IPL_LIFE_CURSE,
        IPL_MANA,
        IPL_MANA_CURSE,
        IPL_DUR,
        IPL_DUR_CURSE,
        IPL_INDESTRUCTIBLE,
        IPL_LIGHT,
        IPL_LIGHT_CURSE,
        IPL_MULT_ARROWS = 41, /* only used in hellfire */
        IPL_FIRE_ARROWS,
        IPL_LIGHT_ARROWS,
        IPL_INVCURS,
        IPL_THORNS,
        IPL_NOMANA,
        IPL_FIREBALL = 50, /* only used in hellfire */
        IPL_ABSHALFTRAP = 52,
        IPL_KNOCKBACK,
        IPL_STEALMANA = 55,
        IPL_STEALLIFE,
        IPL_TARGAC,
        IPL_FASTATTACK,
        IPL_FASTRECOVER,
        IPL_FASTBLOCK,
        IPL_DAMMOD,
        IPL_RNDARROWVEL,
        IPL_SETDAM,
        IPL_SETDUR,
        IPL_NOMINSTR,
        IPL_SPELL,
        IPL_ONEHAND = 68,
        IPL_3XDAMVDEM,
        IPL_ALLRESZERO,
        IPL_DRAINLIFE = 72,
        IPL_RNDSTEALLIFE,
        IPL_SETAC = 75,
        IPL_ADDACLIFE,
        IPL_ADDMANAAC,
        IPL_AC_CURSE = 79,
        IPL_LASTDIABLO = IPL_AC_CURSE,
        IPL_FIRERES_CURSE,
        IPL_LIGHTRES_CURSE,
        IPL_MAGICRES_CURSE,
        IPL_DEVASTATION = 84,
        IPL_DECAY,
        IPL_PERIL,
        IPL_JESTERS,
        IPL_CRYSTALLINE,
        IPL_DOPPELGANGER,
        IPL_ACDEMON,
        IPL_ACUNDEAD,
        IPL_MANATOLIFE,
        IPL_LIFETOMANA,
        IPL_INVALID = -1,
    };

    public enum item_drop_rate 
    {
        IDROP_NEVER,
        IDROP_REGULAR,
        IDROP_DOUBLE,
    };

    public enum item_class 
    {
        ICLASS_NONE,
        ICLASS_WEAPON,
        ICLASS_ARMOR,
        ICLASS_MISC,
        ICLASS_GOLD,
        ICLASS_QUEST,
    };

    public enum item_equip_type 
    {
        ILOC_NONE,
        ILOC_ONEHAND,
        ILOC_TWOHAND,
        ILOC_ARMOR,
        ILOC_HELM,
        ILOC_RING,
        ILOC_AMULET,
        ILOC_UNEQUIPABLE,
        ILOC_BELT,
        ILOC_INVALID = -1,
    };

    public enum item_misc_id
    {
        IMISC_NONE,
        IMISC_USEFIRST,
        IMISC_FULLHEAL,
        IMISC_HEAL,
        IMISC_0x4, // Unused
        IMISC_0x5, // Unused
        IMISC_MANA,
        IMISC_FULLMANA,
        IMISC_0x8, // Unused
        IMISC_0x9, // Unused
        IMISC_ELIXSTR,
        IMISC_ELIXMAG,
        IMISC_ELIXDEX,
        IMISC_ELIXVIT,
        IMISC_0xE,  // Unused
        IMISC_0xF,  // Unused
        IMISC_0x10, // Unused
        IMISC_0x11, // Unused
        IMISC_REJUV,
        IMISC_FULLREJUV,
        IMISC_USELAST,
        IMISC_SCROLL,
        IMISC_SCROLLT,
        IMISC_STAFF,
        IMISC_BOOK,
        IMISC_RING,
        IMISC_AMULET,
        IMISC_UNIQUE,
        IMISC_0x1C, // Unused
        IMISC_OILFIRST,
        IMISC_OILOF, /* oils are beta or hellfire only */
        IMISC_OILACC,
        IMISC_OILMAST,
        IMISC_OILSHARP,
        IMISC_OILDEATH,
        IMISC_OILSKILL,
        IMISC_OILBSMTH,
        IMISC_OILFORT,
        IMISC_OILPERM,
        IMISC_OILHARD,
        IMISC_OILIMP,
        IMISC_OILLAST,
        IMISC_MAPOFDOOM,
        IMISC_EAR,
        IMISC_SPECELIX,
        IMISC_0x2D, // Unused
        IMISC_RUNEFIRST,
        IMISC_RUNEF,
        IMISC_RUNEL,
        IMISC_GR_RUNEL,
        IMISC_GR_RUNEF,
        IMISC_RUNES,
        IMISC_RUNELAST,
        IMISC_AURIC,
        IMISC_NOTE,
        IMISC_INVALID = -1,
    };

    public enum unique_base_item
    {
        UITYPE_NONE,
        UITYPE_SHORTBOW,
        UITYPE_LONGBOW,
        UITYPE_HUNTBOW,
        UITYPE_COMPBOW,
        UITYPE_WARBOW,
        UITYPE_BATTLEBOW,
        UITYPE_DAGGER,
        UITYPE_FALCHION,
        UITYPE_CLAYMORE,
        UITYPE_BROADSWR,
        UITYPE_SABRE,
        UITYPE_SCIMITAR,
        UITYPE_LONGSWR,
        UITYPE_BASTARDSWR,
        UITYPE_TWOHANDSWR,
        UITYPE_GREATSWR,
        UITYPE_CLEAVER,
        UITYPE_LARGEAXE,
        UITYPE_BROADAXE,
        UITYPE_SMALLAXE,
        UITYPE_BATTLEAXE,
        UITYPE_GREATAXE,
        UITYPE_MACE,
        UITYPE_MORNSTAR,
        UITYPE_SPIKCLUB,
        UITYPE_MAUL,
        UITYPE_WARHAMMER,
        UITYPE_FLAIL,
        UITYPE_LONGSTAFF,
        UITYPE_SHORTSTAFF,
        UITYPE_COMPSTAFF,
        UITYPE_QUARSTAFF,
        UITYPE_WARSTAFF,
        UITYPE_SKULLCAP,
        UITYPE_HELM,
        UITYPE_GREATHELM,
        UITYPE_CROWN,
        UITYPE_38,
        UITYPE_RAGS,
        UITYPE_STUDARMOR,
        UITYPE_CLOAK,
        UITYPE_ROBE,
        UITYPE_CHAINMAIL,
        UITYPE_LEATHARMOR,
        UITYPE_BREASTPLATE,
        UITYPE_CAPE,
        UITYPE_PLATEMAIL,
        UITYPE_FULLPLATE,
        UITYPE_BUCKLER,
        UITYPE_SMALLSHIELD,
        UITYPE_LARGESHIELD,
        UITYPE_KITESHIELD,
        UITYPE_GOTHSHIELD,
        UITYPE_RING,
        UITYPE_55,
        UITYPE_AMULET,
        UITYPE_SKCROWN,
        UITYPE_INFRARING,
        UITYPE_OPTAMULET,
        UITYPE_TRING,
        UITYPE_HARCREST,
        UITYPE_MAPOFDOOM,
        UITYPE_ELIXIR,
        UITYPE_ARMOFVAL,
        UITYPE_STEELVEIL,
        UITYPE_GRISWOLD,
        UITYPE_LGTFORGE,
        UITYPE_LAZSTAFF,
        UITYPE_BOVINE,
        UITYPE_INVALID = -1,
    };

    /// Item graphic IDs; frame_num-11 of objcurs.cel.
    public enum item_cursor_graphic 
    {
        // clang-format off
        ICURS_POTION_OF_FULL_MANA         = 0,
        ICURS_SCROLL_OF                   = 1,
        ICURS_GOLD_SMALL                  = 4,
        ICURS_GOLD_MEDIUM                 = 5,
        ICURS_GOLD_LARGE                  = 6,
        ICURS_RING_OF_TRUTH               = 10,
        ICURS_RING                        = 12,
        ICURS_SPECTRAL_ELIXIR             = 15,
        ICURS_GOLDEN_ELIXIR               = 17,
        ICURS_EMPYREAN_BAND               = 18,
        ICURS_EAR_SORCERER                = 19,
        ICURS_EAR_WARRIOR                 = 20,
        ICURS_EAR_ROGUE                   = 21,
        ICURS_BLOOD_STONE                 = 25,
        ICURS_OIL                         = 30,
        ICURS_ELIXIR_OF_VITALITY          = 31,
        ICURS_POTION_OF_HEALING           = 32,
        ICURS_POTION_OF_FULL_REJUVENATION = 33,
        ICURS_ELIXIR_OF_MAGIC             = 34,
        ICURS_POTION_OF_FULL_HEALING      = 35,
        ICURS_ELIXIR_OF_DEXTERITY         = 36,
        ICURS_POTION_OF_REJUVENATION      = 37,
        ICURS_ELIXIR_OF_STRENGTH          = 38,
        ICURS_POTION_OF_MANA              = 39,
        ICURS_BRAIN                       = 40,
        ICURS_OPTIC_AMULET                = 44,
        ICURS_AMULET                      = 45,
        ICURS_DAGGER                      = 51,
        ICURS_BLADE                       = 56,
        ICURS_BASTARD_SWORD               = 57,
        ICURS_MACE                        = 59,
        ICURS_LONG_SWORD                  = 60,
        ICURS_BROAD_SWORD                 = 61,
        ICURS_FALCHION                    = 62,
        ICURS_MORNING_STAR                = 63,
        ICURS_SHORT_SWORD                 = 64,
        ICURS_CLAYMORE                    = 65,
        ICURS_CLUB                        = 66,
        ICURS_SABRE                       = 67,
        ICURS_SPIKED_CLUB                 = 70,
        ICURS_SCIMITAR                    = 72,
        ICURS_FULL_HELM                   = 75,
        ICURS_MAGIC_ROCK                  = 76,
        ICURS_THE_UNDEAD_CROWN            = 78,
        ICURS_HELM                        = 82,
        ICURS_BUCKLER                     = 83,
        ICURS_VIEL_OF_STEEL               = 85,
        ICURS_BOOK_GREY                   = 86,
        ICURS_BOOK_RED                    = 87,
        ICURS_BOOK_BLUE                   = 88,
        ICURS_BLACK_MUSHROOM              = 89,
        ICURS_SKULL_CAP                   = 90,
        ICURS_CAP                         = 91,
        ICURS_HARLEQUIN_CREST             = 93,
        ICURS_CROWN                       = 95,
        ICURS_MAP_OF_THE_STARS            = 96,
        ICURS_FUNGAL_TOME                 = 97,
        ICURS_GREAT_HELM                  = 98,
        ICURS_BATTLE_AXE                  = 101,
        ICURS_HUNTERS_BOW                 = 102,
        ICURS_FIELD_PLATE                 = 103,
        ICURS_SMALL_SHIELD                = 105,
        ICURS_CLEAVER                     = 106,
        ICURS_STUDDED_LEATHER_ARMOR       = 107,
        ICURS_SHORT_STAFF                 = 109,
        ICURS_TWO_HANDED_SWORD            = 110,
        ICURS_CHAIN_MAIL                  = 111,
        ICURS_SMALL_AXE                   = 112,
        ICURS_KITE_SHIELD                 = 113,
        ICURS_SCALE_MAIL                  = 114,
        ICURS_SHORT_BOW                   = 118,
        ICURS_LONG_BATTLE_BOW             = 119,
        ICURS_LONG_WAR_BOW                = 120,
        ICURS_WAR_HAMMER                  = 121,
        ICURS_MAUL                        = 122,
        ICURS_LONG_STAFF                  = 123,
        ICURS_WAR_STAFF                   = 124,
        ICURS_TAVERN_SIGN                 = 126,
        ICURS_HARD_LEATHER_ARMOR          = 127,
        ICURS_RAGS                        = 128,
        ICURS_QUILTED_ARMOR               = 129,
        ICURS_FLAIL                       = 131,
        ICURS_TOWER_SHIELD                = 132,
        ICURS_COMPOSITE_BOW               = 133,
        ICURS_GREAT_SWORD                 = 134,
        ICURS_LEATHER_ARMOR               = 135,
        ICURS_SPLINT_MAIL                 = 136,
        ICURS_ROBE                        = 137,
        ICURS_ANVIL_OF_FURY               = 140,
        ICURS_BROAD_AXE                   = 141,
        ICURS_LARGE_AXE                   = 142,
        ICURS_GREAT_AXE                   = 143,
        ICURS_AXE                         = 144,
        ICURS_LARGE_SHIELD                = 147,
        ICURS_GOTHIC_SHIELD               = 148,
        ICURS_CLOAK                       = 149,
        ICURS_CAPE                        = 150,
        ICURS_FULL_PLATE_MAIL             = 151,
        ICURS_GOTHIC_PLATE                = 152,
        ICURS_BREAST_PLATE                = 153,
        ICURS_RING_MAIL                   = 154,
        ICURS_STAFF_OF_LAZARUS            = 155,
        ICURS_ARKAINES_VALOR              = 157,
        ICURS_SHORT_WAR_BOW               = 165,
        ICURS_COMPOSITE_STAFF             = 166,
        ICURS_SHORT_BATTLE_BOW            = 167,
        ICURS_GOLD                        = 168,
        ICURS_AURIC_AMULET                = 180,
        ICURS_RUNE_BOMB                   = 187,
        ICURS_THEODORE                    = 188,
        ICURS_TORN_NOTE_1                 = 189,
        ICURS_TORN_NOTE_2                 = 190,
        ICURS_TORN_NOTE_3                 = 191,
        ICURS_RECONSTRUCTED_NOTE          = 192,
        ICURS_RUNE_OF_FIRE                = 193,
        ICURS_GREATER_RUNE_OF_FIRE        = 194,
        ICURS_RUNE_OF_LIGHTNING           = 195,
        ICURS_GREATER_RUNE_OF_LIGHTNING   = 196,
        ICURS_RUNE_OF_STONE               = 197,
        ICURS_GREY_SUIT                   = 198,
        ICURS_BROWN_SUIT                  = 199,
        ICURS_BOVINE                      = 226,
        // clang-format on
    };

    public enum _item_indexes 
    { // TODO defines all indexes in AllItemsList
        IDI_GOLD,
        IDI_WARRIOR,
        IDI_WARRSHLD,
        IDI_WARRCLUB,
        IDI_ROGUE,
        IDI_SORCERER,
        IDI_CLEAVER,
        IDI_FIRSTQUEST = IDI_CLEAVER,
        IDI_SKCROWN,
        IDI_INFRARING,
        IDI_ROCK,
        IDI_OPTAMULET,
        IDI_TRING,
        IDI_BANNER,
        IDI_HARCREST,
        IDI_STEELVEIL,
        IDI_GLDNELIX,
        IDI_ANVIL,
        IDI_MUSHROOM,
        IDI_BRAIN,
        IDI_FUNGALTM,
        IDI_SPECELIX,
        IDI_BLDSTONE,
        IDI_MAPOFDOOM,
        IDI_LASTQUEST = IDI_MAPOFDOOM,
        IDI_EAR,
        IDI_HEAL,
        IDI_MANA,
        IDI_IDENTIFY,
        IDI_PORTAL,
        IDI_ARMOFVAL,
        IDI_FULLHEAL,
        IDI_FULLMANA,
        IDI_GRISWOLD,
        IDI_LGTFORGE,
        IDI_LAZSTAFF,
        IDI_RESURRECT,
        IDI_OIL,
        IDI_SHORTSTAFF,
        IDI_BARDSWORD,
        IDI_BARDDAGGER,
        IDI_RUNEBOMB,
        IDI_THEODORE,
        IDI_AURIC,
        IDI_NOTE1,
        IDI_NOTE2,
        IDI_NOTE3,
        IDI_FULLNOTE,
        IDI_BROWNSUIT,
        IDI_GREYSUIT,
        IDI_BOOK1 = 114,
        IDI_BOOK2,
        IDI_BOOK3,
        IDI_BOOK4,
        IDI_BARBARIAN = 139,
        IDI_RUNEOFSTONE = 165,
        IDI_SORCERER_DIABLO,

        IDI_LAST = IDI_SORCERER_DIABLO,
        IDI_NONE = -1,
    };

    public enum SpellFlag 
    {
        // clang-format off
        None         = 0,
        Etherealize  = 1 << 0,
        RageActive   = 1 << 1,
        RageCooldown = 1 << 2,
        // bits 3-7 are unused
        // clang-format on
    };

    public enum spell_type 
    {
        RSPLTYPE_SKILL,
        RSPLTYPE_SPELL,
        RSPLTYPE_SCROLL,
        RSPLTYPE_CHARGES,
        RSPLTYPE_INVALID,
    };

    public enum spell_id
    {
        SPL_NULL,
        SPL_FIREBOLT,
        SPL_HEAL,
        SPL_LIGHTNING,
        SPL_FLASH,
        SPL_IDENTIFY,
        SPL_FIREWALL,
        SPL_TOWN,
        SPL_STONE,
        SPL_INFRA,
        SPL_RNDTELEPORT,
        SPL_MANASHIELD,
        SPL_FIREBALL,
        SPL_GUARDIAN,
        SPL_CHAIN,
        SPL_WAVE,
        SPL_DOOMSERP,
        SPL_BLODRIT,
        SPL_NOVA,
        SPL_INVISIBIL,
        SPL_FLAME,
        SPL_GOLEM,
        SPL_BLODBOIL,
        SPL_TELEPORT,
        SPL_APOCA,
        SPL_ETHEREALIZE,
        SPL_REPAIR,
        SPL_RECHARGE,
        SPL_DISARM,
        SPL_ELEMENT,
        SPL_CBOLT,
        SPL_HBOLT,
        SPL_RESURRECT,
        SPL_TELEKINESIS,
        SPL_HEALOTHER,
        SPL_FLARE,
        SPL_BONESPIRIT,
        SPL_LASTDIABLO = SPL_BONESPIRIT,
        SPL_MANA,
        SPL_MAGI,
        SPL_JESTER,
        SPL_LIGHTWALL,
        SPL_IMMOLAT,
        SPL_WARP,
        SPL_REFLECT,
        SPL_BERSERK,
        SPL_FIRERING,
        SPL_SEARCH,
        SPL_RUNEFIRE,
        SPL_RUNELIGHT,
        SPL_RUNENOVA,
        SPL_RUNEIMMOLAT,
        SPL_RUNESTONE,

        SPL_LAST = SPL_RUNESTONE,
        SPL_INVALID = -1,
    };

    public enum _difficulty 
    {
        DIFF_NORMAL,
        DIFF_NIGHTMARE,
        DIFF_HELL,

        DIFF_LAST = DIFF_HELL,
    };

    public class d2DEF
    {
        public const int BaseHitChance = 50;
        /** Indicate if we have loaded the Hellfire expansion data */
        public const bool gbIsHellfire = false;
        public const bool gbIsMultiplayer = false;
        public const _difficulty gbDifficulty = _difficulty.DIFF_NORMAL;
        /** Indicate if we only have access to demo data */
        public const bool gbIsSpawn = false;
    }

    public enum _monster_id
    {
        MT_NZOMBIE,
        MT_BZOMBIE,
        MT_GZOMBIE,
        MT_YZOMBIE,
        MT_RFALLSP,
        MT_DFALLSP,
        MT_YFALLSP,
        MT_BFALLSP,
        MT_WSKELAX,
        MT_TSKELAX,
        MT_RSKELAX,
        MT_XSKELAX,
        MT_RFALLSD,
        MT_DFALLSD,
        MT_YFALLSD,
        MT_BFALLSD,
        MT_NSCAV,
        MT_BSCAV,
        MT_WSCAV,
        MT_YSCAV,
        MT_WSKELBW,
        MT_TSKELBW,
        MT_RSKELBW,
        MT_XSKELBW,
        MT_WSKELSD,
        MT_TSKELSD,
        MT_RSKELSD,
        MT_XSKELSD,
        MT_INVILORD,
        MT_SNEAK,
        MT_STALKER,
        MT_UNSEEN,
        MT_ILLWEAV,
        MT_LRDSAYTR,
        MT_NGOATMC,
        MT_BGOATMC,
        MT_RGOATMC,
        MT_GGOATMC,
        MT_FIEND,
        MT_BLINK,
        MT_GLOOM,
        MT_FAMILIAR,
        MT_NGOATBW,
        MT_BGOATBW,
        MT_RGOATBW,
        MT_GGOATBW,
        MT_NACID,
        MT_RACID,
        MT_BACID,
        MT_XACID,
        MT_SKING,
        MT_CLEAVER,
        MT_FAT,
        MT_MUDMAN,
        MT_TOAD,
        MT_FLAYED,
        MT_WYRM,
        MT_CAVSLUG,
        MT_DVLWYRM,
        MT_DEVOUR,
        MT_NMAGMA,
        MT_YMAGMA,
        MT_BMAGMA,
        MT_WMAGMA,
        MT_HORNED,
        MT_MUDRUN,
        MT_FROSTC,
        MT_OBLORD,
        MT_BONEDMN,
        MT_REDDTH,
        MT_LTCHDMN,
        MT_UDEDBLRG,
        MT_INCIN,
        MT_FLAMLRD,
        MT_DOOMFIRE,
        MT_HELLBURN,
        MT_STORM,
        MT_RSTORM,
        MT_STORML,
        MT_MAEL,
        MT_BIGFALL,
        MT_WINGED,
        MT_GARGOYLE,
        MT_BLOODCLW,
        MT_DEATHW,
        MT_MEGA,
        MT_GUARD,
        MT_VTEXLRD,
        MT_BALROG,
        MT_NSNAKE,
        MT_RSNAKE,
        MT_BSNAKE,
        MT_GSNAKE,
        MT_NBLACK,
        MT_RTBLACK,
        MT_BTBLACK,
        MT_RBLACK,
        MT_UNRAV,
        MT_HOLOWONE,
        MT_PAINMSTR,
        MT_REALWEAV,
        MT_SUCCUBUS,
        MT_SNOWWICH,
        MT_HLSPWN,
        MT_SOLBRNR,
        MT_COUNSLR,
        MT_MAGISTR,
        MT_CABALIST,
        MT_ADVOCATE,
        MT_GOLEM,
        MT_DIABLO,
        MT_DARKMAGE,
        MT_HELLBOAR,
        MT_STINGER,
        MT_PSYCHORB,
        MT_ARACHNON,
        MT_FELLTWIN,
        MT_HORKSPWN,
        MT_VENMTAIL,
        MT_NECRMORB,
        MT_SPIDLORD,
        MT_LASHWORM,
        MT_TORCHANT,
        MT_HORKDMN,
        MT_DEFILER,
        MT_GRAVEDIG,
        MT_TOMBRAT,
        MT_FIREBAT,
        MT_SKLWING,
        MT_LICH,
        MT_CRYPTDMN,
        MT_HELLBAT,
        MT_BONEDEMN,
        MT_ARCHLICH,
        MT_BICLOPS,
        MT_FLESTHNG,
        MT_REAPER,
        MT_NAKRUL,
        NUM_MTYPES,
        MT_INVALID = -1,
    };

    public enum MonsterAvailability
    {
        Never,
        Always,
        Retail,
    };

    public enum _mai_id 
    {
        AI_ZOMBIE,
        AI_FAT,
        AI_SKELSD,
        AI_SKELBOW,
        AI_SCAV,
        AI_RHINO,
        AI_GOATMC,
        AI_GOATBOW,
        AI_FALLEN,
        AI_MAGMA,
        AI_SKELKING,
        AI_BAT,
        AI_GARG,
        AI_CLEAVER,
        AI_SUCC,
        AI_SNEAK,
        AI_STORM,
        AI_FIREMAN,
        AI_GARBUD,
        AI_ACID,
        AI_ACIDUNIQ,
        AI_GOLUM,
        AI_ZHAR,
        AI_SNOTSPIL,
        AI_SNAKE,
        AI_COUNSLR,
        AI_MEGA,
        AI_DIABLO,
        AI_LAZARUS,
        AI_LAZHELP,
        AI_LACHDAN,
        AI_WARLORD,
        AI_FIREBAT,
        AI_TORCHANT,
        AI_HORKDMN,
        AI_LICH,
        AI_ARCHLICH,
        AI_PSYCHORB,
        AI_NECROMORB,
        AI_BONEDEMON,
        AI_INVALID = -1,
    };

    public enum MonsterClass 
    {
        Undead,
        Demon,
        Animal,
    };

    public enum ItemSpecialEffectHf 
    {
        // clang-format off
        None               = 0,
        Devastation        = 1 << 0,
        Decay              = 1 << 1,
        Peril              = 1 << 2,
        Jesters            = 1 << 3,
        Doppelganger       = 1 << 4,
        ACAgainstDemons    = 1 << 5,
        ACAgainstUndead    = 1 << 6,
        // clang-format on
    };

    public enum MonsterMode 
    {
        Stand,
        /** Movement towards N, NW, or NE */
        MoveNorthwards,
        /** Movement towards S, SW, or SE */
        MoveSouthwards,
        /** Movement towards W or E */
        MoveSideways,
        MeleeAttack,
        HitRecovery,
        Death,
        SpecialMeleeAttack,
        FadeIn,
        FadeOut,
        RangedAttack,
        SpecialStand,
        SpecialRangedAttack,
        Delay,
        Charge,
        Petrified, // 石化
        Heal,
        Talk,
    };

    public enum HeroClass
    {
        Warrior,
        Rogue,
        Sorcerer,
        Monk,
        Bard,
        Barbarian,

        LAST = Barbarian
    };

    [Flags]
    public enum ItemSpecialEffect
    {
        // clang-format off
        None                   = 0,
        RandomStealLife        = 1 << 1,
        RandomArrowVelocity    = 1 << 2,
        FireArrows             = 1 << 3,
        FireDamage             = 1 << 4,
        LightningDamage        = 1 << 5,
        DrainLife              = 1 << 6,
        MultipleArrows         = 1 << 9,
        Knockback              = 1 << 11,
        StealMana3             = 1 << 13,
        StealMana5             = 1 << 14,
        StealLife3             = 1 << 15,
        StealLife5             = 1 << 16,
        QuickAttack            = 1 << 17,
        FastAttack             = 1 << 18,
        FasterAttack           = 1 << 19,
        FastestAttack          = 1 << 20,
        FastHitRecovery        = 1 << 21,
        FasterHitRecovery      = 1 << 22,
        FastestHitRecovery     = 1 << 23,
        FastBlock              = 1 << 24,
        LightningArrows        = 1 << 25,
        Thorns                 = 1 << 26,
        NoMana                 = 1 << 27,
        HalfTrapDamage         = 1 << 28,
        TripleDemonDamage      = 1 << 30,
        ZeroResistance         = 1 << 31,
        // clang-format on
    };

    public enum ItemType
    {
        Misc,
        Sword,
        Axe,
        Bow,
        Mace,
        Shield,
        LightArmor,
        Helm,
        MediumArmor,
        HeavyArmor,
        Staff,
        Gold,
        Ring,
        Amulet,
        None = -1,
    };

    // Logical equipment locations
    public enum inv_body_loc 
    {
        INVLOC_HEAD,
        INVLOC_RING_LEFT,
        INVLOC_RING_RIGHT,
        INVLOC_AMULET,
        INVLOC_HAND_LEFT,
        INVLOC_HAND_RIGHT,
        INVLOC_CHEST,
        NUM_INVLOC,
    };
}
