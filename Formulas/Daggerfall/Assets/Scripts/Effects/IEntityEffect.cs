using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to an entity effect.
/// </summary>
public interface IEntityEffect /*: IMacroContextProvider*/
{
    /// <summary>
    /// Gets effect properties.
    /// </summary>
    EffectProperties Properties { get; }

    /// <summary>
    /// Gets or sets current effect settings.
    /// </summary>
    EffectSettings Settings { get; set; }

    /// <summary>
    /// Gets or sets enchantment param for enchantment effects.
    /// If this property is null then effect is not a live enchantment.
    /// </summary>
    EnchantmentParam? EnchantmentParam { get; set; }

    /// <summary>
    /// Gets effect potion properties (if any).
    /// </summary>
    PotionProperties PotionProperties { get; }

    /// <summary>
    /// Gets the caster entity behaviour of this effect (can return null).
    /// </summary>
    Actor Caster { get; }

    /// <summary>
    /// Gets key from properties.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Group display name (used by crafting stations).
    /// </summary>
    string GroupName { get; }

    /// <summary>
    /// SubGroup display name (used by crafting stations).
    /// </summary>
    string SubGroupName { get; }

    /// <summary>
    /// Description for spellmaker.
    /// </summary>
    // TextFile.Token[] SpellMakerDescription { get; }

    /// <summary>
    /// Description for spellbook.
    /// </summary>
    // TextFile.Token[] SpellBookDescription { get; }

    /// <summary>
    /// Gets display name from properties or construct one from Group+SubGroup text in properties.
    /// This allows effects to set a custom display name or just roll with automatic names.
    /// Daggerfall appears to use first token of spellmaker/spellbook description, but we want more control for effect mods.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Gets or sets number of magic rounds remaining.
    /// </summary>
    int RoundsRemaining { get; set; }

    /// <summary>
    /// Gets flag stating if effect passed a chance check on start.
    /// If always false if effect does not support chance component.
    /// </summary>
    bool ChanceSuccess { get; }

    /// <summary>
    /// Gets array DaggerfallStats.Count items wide.
    /// Array items represent Strength, Intelligence, Willpower, etc.
    /// Effect implementation should set modifier values for stats when part of payload.
    /// For example, a "Damage Strength" effect would set the current modifier for Strength (such as -5 to Strength).
    /// Use (int)DFCareer.State.StatName to get index.
    /// </summary>
    int[] StatMods { get; }

    /// <summary>
    /// Gets array DaggerfallStats.Count items wide.
    /// Array items represent Strength, Intelligence, Willpower, etc.
    /// Allows an effect to temporarily override stat maximum value.
    /// </summary>
    int[] StatMaxMods { get; }

    /// <summary>
    /// Get array DaggerfallSkills.Count items wide.
    /// Array items represent Medical, Etiquette, Streetwise, etc.
    /// Effect implementation should set modifier values for skills when part of payload.
    /// For example, a "Tongues" effect would set the current modifier for all language skills (such as +5 to Dragonish, +5 to Giantish, and so on).
    /// Use (int)DFCareer.Skills.SkillName to get index.
    /// </summary>
    int[] SkillMods { get; }

    /// <summary>
    /// Gets array DaggerfallResistances.Count items wide.
    /// Array items represent Fire, Cold, Poison/Disease, Shock, Magic.
    /// Effect implementation should set modifier values for resistances when part of payload.
    /// For example, a "Resist Fire" effect would set the current modifier for Fire resistance (such as +30 to Fire resistance).
    /// Use (int)DFCareer.Resistances.ResistanceName to get index.
    /// </summary>
    int[] ResistanceMods { get; }

    /// <summary>
    /// Gets or sets parent bundle for this effect.
    /// Will be null for template effects.
    /// </summary>
    LiveEffectBundle ParentBundle { get; set; }

    /// <summary>
    /// True if effect has ended by calling End();
    /// </summary>
    bool HasEnded { get; }

    /// <summary>
    /// Gets total number of variants for multi-effects.
    /// </summary>
    int VariantCount { get; }

    /// <summary>
    /// Sets current variant for multi-effects.
    /// </summary>
    int CurrentVariant { get; set; }

    /// <summary>
    /// True if spell effect always lands regardless of saving throws.
    /// </summary>
    bool BypassSavingThrows { get; }

    /// <summary>
    /// Called by an ActorEffect when parent bundle is attached to host entity.
    /// Use this for setup or immediate work performed only once.
    /// </summary>
    void Start(ActorEffect manager, Actor caster = null);

    /// <summary>
    /// Called by an EntityEffect manage when parent bundle is resumed from save.
    /// </summary>
    void Resume(ActorEffect.EffectSaveData_v1 effectData, ActorEffect manager, Actor caster = null);

    /// <summary>
    /// Use this for work performed every frame.
    /// </summary>
    void ConstantEffect();

    /// <summary>
    /// Use this for any work performed every magic round.
    /// </summary>
    void MagicRound();

    /// <summary>
    /// Called when bundle lifetime is at an end.
    /// Use this for any wrap-up work.
    /// </summary>
    void End();

    /// <summary>
    /// Perform a chance roll on this effect based on chance settings.
    /// Can be used by custom chance effects that need to roll chance other than at start.
    /// </summary>
    bool RollChance();

    /// <summary>
    /// Gets array of enchantment settings supported by this effect.
    /// Can return a null or empty array, especially if effect not an enchantment.
    /// </summary>
    EnchantmentSettings[] GetEnchantmentSettings();

    /// <summary>
    /// Helper to get a specific enchantment setting based on param.
    /// Can return null if enchantment with param not found.
    /// </summary>
    EnchantmentSettings? GetEnchantmentSettings(EnchantmentParam param);

    /// <summary>
    /// Helper to check if properties contain the specified item maker flags.
    /// </summary>
    bool HasItemMakerFlags(ItemMakerFlags flags);

    /// <summary>
    /// Helper to check if properties contain the specified enchantment payload flags
    /// </summary>
    bool HasEnchantmentPayloadFlags(EnchantmentPayloadFlags flags);

    /// <summary>
    /// Enchantment payload callback for enchantment to perform custom execution based on context.
    /// These callbacks are performed directly from template, not from a live instance of effect. Do not store state in effect during callbacks.
    /// Not used by EnchantmentPayloadFlags.Held - rather, an effect instance bundle is assigned to entity's effect manager to execute as normal.
    /// </summary>
    PayloadCallbackResults? EnchantmentPayloadCallback(EnchantmentPayloadFlags context, EnchantmentParam? param = null, Actor sourceEntity = null, Actor targetEntity = null, Item sourceItem = null, int sourceDamage = 0);

    /// <summary>
    /// Gets related enchantments that will be forced onto item along with this enchantment.
    /// Only used by Soul Bound in classic gameplay.
    /// </summary>
    /// <returns></returns>
    ForcedEnchantmentSet? GetForcedEnchantments(EnchantmentParam? param = null);

    /// <summary>
    /// Enchantment can flag that it is exclusive to one or more enchantments in array provided.
    /// Used by enchanting window to prevent certain enchantments from being selected together.
    /// </summary>
    bool IsEnchantmentExclusiveTo(EnchantmentSettings[] settingsToTest, EnchantmentParam? comparerParam = null);

    /// <summary>
    /// Get effect state data to serialize.
    /// </summary>
    object GetSaveData();

    /// <summary>
    /// Restore effect state from serialized data.
    /// </summary>
    void RestoreSaveData(object dataIn);
}
