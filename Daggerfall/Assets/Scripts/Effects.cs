using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Supported bundle types.
/// This helps determine lifetime and usage of a bundle.
/// </summary>
public enum BundleTypes
{
    None,
    Spell,
    Disease,
    Poison,
    HeldMagicItem,
    Potion,
}

/// <summary>
/// How an effect bundle targets entities in world space.
/// Can be used as flags by effect system to declare supported targets.
/// </summary>
[Flags]
public enum TargetTypes
{
    None = 0,
    CasterOnly = 1,
    ByTouch = 2,
    SingleTargetAtRange = 4,
    AreaAroundCaster = 8,
    AreaAtRange = 16,
}

/// <summary>
/// How effect bundle manifests for cast animations, billboard effects, resist checks, etc.
/// Can be used as flags by effect system to declare supported elements.
/// </summary>
[Flags]
public enum ElementTypes
{
    None = 0,
    Fire = 1,
    Cold = 2,
    Poison = 4,
    Shock = 8,
    Magic = 16,
}

/// <summary>
/// Special bundle flags to control additional runtime behaviours.
/// </summary>
[Flags]
public enum BundleRuntimeFlags
{
    /// <summary>No flags.</summary>
    None = 0,
    /// <summary>If bundle is from a held magic item, it will be removed at recast time and recast callbacks executed on item.</summary>
    ItemRecastEnabled = 1,
}

/// <summary>
/// Flexible enchantment parameter for either a classic effect or a custom effect.
/// This is stored with EffectEntry for enchantment bundles and assigned to live effect instance.
/// Formalising this to a data structure allows for expanding custom enchantment params later.
/// </summary>
[Serializable]
public struct EnchantmentParam
{
    public short ClassicParam;                                  // Classic echantment param
    public string CustomParam;                                  // Custom enchantment param
}

/// <summary>
/// For storing effect in bundle settings.
/// </summary>
[Serializable]
public struct EffectEntry
{
    public string Key;
    public EffectSettings Settings;
    public EnchantmentParam? EnchantmentParam;

    public EffectEntry(string key, EnchantmentParam? enchantmentParam = null)
    {
        Key = key;
        Settings = new EffectSettings();
        EnchantmentParam = enchantmentParam;
    }

    public EffectEntry(string key, EffectSettings settings, EnchantmentParam? enchantmentParam = null)
    {
        Key = key;
        Settings = settings;
        EnchantmentParam = enchantmentParam;
    }
}

/// <summary>
/// Stores an effect group / subgroup pair as read from classic save.
/// This is only used when importing character spellbook from classic.
/// During startup any legacy spells will be migrated to Daggerfall Unity spells
/// provided all classic group / subgroup pairs can be resolved to a known effect key.
/// </summary>
[Serializable]
public struct LegacyEffectEntry
{
    public int Group;
    public int SubGroup;
    public EffectSettings Settings;
}

/// <summary>
/// Duration, Chance, Magnitude settings for an effect.
/// </summary>
[Serializable]
public struct EffectSettings
{
    public int DurationBase;
    public int DurationPlus;
    public int DurationPerLevel;

    public int ChanceBase;
    public int ChancePlus;
    public int ChancePerLevel;

    public int MagnitudeBaseMin;
    public int MagnitudeBaseMax;
    public int MagnitudePlusMin;
    public int MagnitudePlusMax;
    public int MagnitudePerLevel;
}

/// <summary>
/// Settings for an entity effect bundle.
/// </summary>
[Serializable]
public struct EffectBundleSettings
{
    public int Version;
    public BundleTypes BundleType;
    public TargetTypes TargetType;
    public ElementTypes ElementType;
    public BundleRuntimeFlags RuntimeFlags;
    public string Name;
    public int IconIndex;
    // public SpellIcon Icon;
    public bool MinimumCastingCost;
    public bool NoCastingAnims;
    public string Tag;
    public EffectEntry[] Effects;
    public LegacyEffectEntry[] LegacyEffects;
    public int? StandardSpellIndex;
}