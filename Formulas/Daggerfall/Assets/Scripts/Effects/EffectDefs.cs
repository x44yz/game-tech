using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Determines how effect chance will function.
/// OnCast: is checked at cast time by ActorEffect receiving effect - effect is rejected on failure.
/// Custom: is always allowed by ActorEffect, but still generates ChanceSuccess flag on Start().
/// This allows custom chance handling elsewhere by either by the effect itself or elsewhere in effect back-end.
/// A Custom chance effect can then decide to check for ChanceSuccess at any time, even do something else entirely.
/// </summary>
public enum ChanceFunction
{
    OnCast,
    Custom,
}

/// <summary>
/// Defines properties intrinsic to an effect.
/// </summary>
[Serializable]
public struct EffectProperties
{
    public string Key;                                          // Unique key to identify effect
    public int ClassicKey;                                      // Unique key only for matching classic effect group/subgroup
    public bool ShowSpellIcon;                                  // True to make spell show icon on player HUD
    public bool SupportDuration;                                // Uses duration
    public bool SupportChance;                                  // Uses chance
    public bool SupportMagnitude;                               // Uses magnitude
    public ChanceFunction ChanceFunction;                       // Determines if chance check is done OnCast (by manager) or Custom (elsewhere)
    public TargetTypes AllowedTargets;                          // Targets allowed by this effect
    public ElementTypes AllowedElements;                        // Elements allowed by this effect
    public MagicCraftingStations AllowedCraftingStations;       // Magic stations that can consume this effect (e.g. spellmaker, itemmaker)
    public DFCareer.MagicSkills MagicSkill;                     // Related magic skill for this effect
    public EffectCosts DurationCosts;                           // Duration cost values
    public EffectCosts ChanceCosts;                             // Chance cost values
    public EffectCosts MagnitudeCosts;                          // Magnitude cost values
    public ItemMakerFlags ItemMakerFlags;                       // Item maker features
    public EnchantmentPayloadFlags EnchantmentPayloadFlags;     // How an enchantment wants to receive execution callbacks to deliver payload
    public bool DisableReflectiveEnumeration;                   // Prevents effect template from being registered automatically with broker
}

/// <summary>
/// Defines properties intrinsic to a potion.
/// Note: Under early development. Subject to change.
/// </summary>
[Serializable]
public struct PotionProperties
{
    public PotionRecipe[] Recipes;                              // Potion recipe for effect
}

/// <summary>
/// Stores an instanced effect bundle for executing effects.
/// </summary>
public class LiveEffectBundle
{
    public int version;
    public BundleTypes bundleType;
    public TargetTypes targetType;
    public ElementTypes elementType;
    public BundleRuntimeFlags runtimeFlags;
    public string name;
    public int iconIndex;
    // public SpellIcon icon;
    public Actor caster;
    public EntityTypes casterEntityType;
    public ulong casterLoadID;
    public Item fromEquippedItem;
    public Item castByItem;
    public List<IEntityEffect> liveEffects;
}

/// <summary>
/// Defines flags for additional feature support at item maker.
/// </summary>
[Flags]
public enum ItemMakerFlags
{
    None = 0,
    AllowMultiplePrimaryInstances = 1,
    AllowMultipleSecondaryInstances = 2,
    AlphaSortSecondaryList = 4,
    WeaponOnly = 8,
}

/// <summary>
/// Optional information returned to framework by enchantment payload callbacks.
/// </summary>
[Serializable]
public struct PayloadCallbackResults
{
    public int strikesModulateDamage;                           // Amount to plus/minus from damage after Strikes effect payload
    public int durabilityLoss;                                  // Amount of durability lost after callback
    public bool removeItem;                                     // Removes item from collection if true
}

/// <summary>
/// Flags to inform magic framework when enchantment effect should receive callbacks to execute its payload.
/// As Daggerfall Unity supports custom effects there is more granularity to payload execution than classic.
/// Note these are distinct from "cast when used", "cast when held", etc. Rather, the CastWhenUsed and CastWhenHeld
/// effects will deliver their payload from callbacks related to these flags.
/// </summary>
[Flags]
public enum EnchantmentPayloadFlags
{
    None = 0,
    Enchanted = 1,      // Payload executed only once when item is enchanted at item maker
    Used = 2,           // Payload executed when item is used from inventory or "use item" UI
    Equipped = 4,       // Payload executed when item is equipped - i.e. payload will execute once every time item is equipped
    Unequipped = 8,     // Payload executed when item is unequipped - i.e. payload will execute once every time item is unequipped
    Held = 16,          // Payload executed for duration item is equipped - i.e. effect bundle will be persistently attached to entity until unequipped
    Strikes = 32,       // Payload executed when a weapon item strikes another entity
    Breaks = 64,        // Payload executed when item breaks after durability reaches zero or less
    MagicRound = 128,   // Payload executed once per magic round
    RerollEffect = 256, // Payload executed when effects are recast on item
}

/// <summary>
/// A list of mobile enemy types with ID range 0-42 (monsters) and 128-146 (humanoids).
/// Do not extend this enum.
/// </summary>
public enum MobileTypes
{
    // Monster IDs are 0-42
    Rat,
    Imp,
    Spriggan,
    GiantBat,
    GrizzlyBear,
    SabertoothTiger,
    Spider,
    Orc,
    Centaur,
    Werewolf,
    Nymph,
    Slaughterfish,
    OrcSergeant,
    Harpy,
    Wereboar,
    SkeletalWarrior,
    Giant,
    Zombie,
    Ghost,
    Mummy,
    GiantScorpion,
    OrcShaman,
    Gargoyle,
    Wraith,
    OrcWarlord,
    FrostDaedra,
    FireDaedra,
    Daedroth,
    Vampire,
    DaedraSeducer,
    VampireAncient,
    DaedraLord,
    Lich,
    AncientLich,
    Dragonling,
    FireAtronach,
    IronAtronach,
    FleshAtronach,
    IceAtronach,
    Horse_Invalid,              // Not used and no matching texture (294 missing). Crashes DF when spawned in-game.
    Dragonling_Alternate,       // Another dragonling. Seems to work fine when spawned in-game.
    Dreugh,
    Lamia,

    // Humanoid IDs are 128-146
    Mage = 128,
    Spellsword,
    Battlemage,
    Sorcerer,
    Healer,
    Nightblade,
    Bard,
    Burglar,
    Rogue,
    Acrobat,
    Thief,
    Assassin,
    Monk,
    Archer,
    Ranger,
    Barbarian,
    Warrior,
    Knight,
    Knight_CityWatch,           // Just called Knight in-game, but renamed CityWatch here for uniqueness. HALT!

    // No enemy type
    None = (int)0xffff,
}

/// <summary>
/// Mobile animation states.
/// </summary>
public enum MobileStates
{
    Move,                   // Records 0-4      (Flying and swimming mobs also uses this animation set for idle)
    PrimaryAttack,          // Records 5-9      (Usually a melee attack animation)
    Hurt,                   // Records 10-14    (Mob has been struck)
    Idle,                   // Records 15-19    (Frost and ice Daedra have animated idle states)
    RangedAttack1,          // Records 20-24    (Bow attack)
    Spell,                  // Records 20-24 or, if absent, copy of PrimaryAttack
    RangedAttack2,          // Records 25-29    (Bow attack on 475, 489, 490 only, absent on other humanoids)
    SeducerTransform1,      // Record 23        (Crouch and grow wings)
    SeducerTransform2,      // Record 22        (Stand and spread wings)
}

/// <summary>
/// Defines a single forced enchantment effect with param.
/// </summary>
public struct ForcedEnchantment
{
    public string key;
    public EnchantmentParam param;

    public ForcedEnchantment(string key, short classicParam = -1)
    {
        this.key = key;
        param = new EnchantmentParam() { ClassicParam = classicParam, CustomParam = string.Empty };
    }
}

/// <summary>
/// Contains a set of forced effects keyed to a valid mobile type.
/// </summary>
public struct ForcedEnchantmentSet
{
    public MobileTypes soulType;
    public ForcedEnchantment[] forcedEffects;
}

/// <summary>
/// Allows tuning of cost per setting.
/// </summary>
[Serializable]
public struct EffectCosts
{
    public float OffsetGold;                                    // Increase base gold cost
    public float Factor;                                        // Scaling factor applied to spellpoint cost
    public float CostA;                                         // First magic number related to costs
    public float CostB;                                         // Second magic number related to costs
}


/// <summary>
/// Usage flags for custom spell bundle offer.
/// Informs other systems if they can use this spell.
/// </summary>
[Flags]
public enum CustomSpellBundleOfferUsage
{
    None = 0,
    SpellsForSale = 1,
    CastWhenUsedEnchantment = 2,
    CastWhenHeldEnchantment = 4,
    CastWhenStrikesEnchantment = 8,
    All = 15,
}

/// <summary>
/// A custom spell bundle offer.
/// </summary>
public struct CustomSpellBundleOffer
{
    public string Key;
    public CustomSpellBundleOfferUsage Usage;
    public EffectBundleSettings BundleSetttings;
    public int EnchantmentCost;
}

/// <summary>
/// Settings for a basic disease effect.
/// </summary>
[Serializable]
public struct DiseaseData
{
    // Affected stats
    public byte STR;
    public byte INT;
    public byte WIL;
    public byte AGI;
    public byte END;
    public byte PER;
    public byte SPD;
    public byte LUC;
    public byte HEA;
    public byte FAT;
    public byte SPL;
    public byte minDamage;
    public byte maxDamage;
    public byte daysOfSymptomsMin; // 0xFF means never-ending
    public byte daysOfSymptomsMax;

    // Constructor
    public DiseaseData(byte STRp, byte INTp,
        byte WILp, byte AGIp, byte ENDp, byte PERp,
        byte SPDp, byte LUCp, byte HEAp, byte FATp,
        byte SPLp, byte minDamagep, byte maxDamagep,
        byte daysOfSymptomsMinp, byte daysOfSymptomsMaxp)
    {
        STR = STRp;
        INT = INTp;
        WIL = WILp;
        AGI = AGIp;
        END = ENDp;
        PER = PERp;
        SPD = SPDp;
        LUC = LUCp;
        HEA = HEAp;
        FAT = FATp;
        SPL = SPLp;
        minDamage = minDamagep;
        maxDamage = maxDamagep;
        daysOfSymptomsMin = daysOfSymptomsMinp;
        daysOfSymptomsMax = daysOfSymptomsMaxp;
    }
}