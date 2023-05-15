using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lycanthropy variants.
/// </summary>
public enum LycanthropyTypes
{
    None = 0,
    Werewolf = 1,
    Wereboar = 2,
}

/// <summary>
/// Flags to modify behaviour of assigning effect bundles.
/// </summary>
[Flags]
public enum AssignBundleFlags
{
    None = 0,
    ShowNonPlayerFailures = 1,
    BypassSavingThrows = 2,
    SpecialInfection = 4,
    BypassChance = 8,
}

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

/// <summary>
/// Flags defining which magic crafting stations can serve this effect.
/// What constitutes a magic crafting station is likely to expand over time.
/// For example, custom character creator is potentially a type of crafting station for advantages/disadvantages.
/// Just leaving as main three stations (spellmaker, potionmaker, itemmaker) for now.
/// </summary>
[Flags]
public enum MagicCraftingStations
{
    None = 0,
    SpellMaker = 1,
    PotionMaker = 2,
    ItemMaker = 4,
}

public class Effects
{
    public const int CurrentSpellVersion = 1;

    public const TargetTypes TargetFlags_None = TargetTypes.None;
        public const TargetTypes TargetFlags_Self = TargetTypes.CasterOnly;
        public const TargetTypes TargetFlags_Other = TargetTypes.ByTouch | TargetTypes.SingleTargetAtRange | TargetTypes.AreaAroundCaster | TargetTypes.AreaAtRange;
        public const TargetTypes TargetFlags_All = TargetTypes.CasterOnly | TargetTypes.ByTouch | TargetTypes.SingleTargetAtRange | TargetTypes.AreaAroundCaster | TargetTypes.AreaAtRange;

        public const ElementTypes ElementFlags_None = ElementTypes.None;
        public const ElementTypes ElementFlags_MagicOnly = ElementTypes.Magic;
        public const ElementTypes ElementFlags_All = ElementTypes.Fire | ElementTypes.Cold | ElementTypes.Poison | ElementTypes.Shock | ElementTypes.Magic;

    public const MagicCraftingStations MagicCraftingFlags_None = MagicCraftingStations.None;
    public const MagicCraftingStations MagicCraftingFlags_All = MagicCraftingStations.SpellMaker | MagicCraftingStations.PotionMaker | MagicCraftingStations.ItemMaker;

    /// <summary>
    /// Maps classic target index to TargetTypes.
    /// </summary>
    /// <param name="targetIndex">Classic target index.</param>
    /// <returns>TargetTypes.</returns>
    public static TargetTypes ClassicTargetIndexToTargetType(int targetIndex)
    {
        switch (targetIndex)
        {
            case 0:
                return TargetTypes.CasterOnly;
            case 1:
                return TargetTypes.ByTouch;
            case 2:
                return TargetTypes.SingleTargetAtRange;
            case 3:
                return TargetTypes.AreaAroundCaster;
            case 4:
                return TargetTypes.AreaAtRange;
            default:
                throw new Exception("ClassicTargetIndexToTargetType() encountered an unknown target index.");
        }
    }

    /// <summary>
    /// Maps classic element index to ElementTypes enum.
    /// </summary>
    /// <param name="elementIndex">Classic element index.</param>
    /// <returns>ElementTypes.</returns>
    public static ElementTypes ClassicElementIndexToElementType(int elementIndex)
    {
        switch (elementIndex)
        {
            case 0:
                return ElementTypes.Fire;
            case 1:
                return ElementTypes.Cold;
            case 2:
                return ElementTypes.Poison;
            case 3:
                return ElementTypes.Shock;
            case 4:
                return ElementTypes.Magic;
            default:
                throw new Exception("ClassicElementIndexToElementType() encountered an unknown element index.");
        }
    }

    /// <summary>
    /// Gets effect template from classic effect record data, if one is available.
    /// </summary>
    /// <param name="effectRecordData">Classic effect record data.</param>
    /// <returns>IEntityEffect of template found or null if no matching template found.</returns>
    public static IEntityEffect GetEffectTemplateFromClassicEffectRecordData(EffectRecordData effectRecordData)
    {
        // Ignore unused effect
        if (effectRecordData.type == -1)
            return null;

        // Get effect type/subtype
        int type, subType;
        type = effectRecordData.type;
        subType = (effectRecordData.subType < 0) ? 255 : effectRecordData.subType; // Entity effect keys use 255 instead of -1 for subtype

        // Check if effect template is implemented for this slot - instant fail if effect not implemented
        int classicKey = BaseEntityEffect.MakeClassicKey((byte)type, (byte)subType);

        // Attempt to find the effect template
        IEntityEffect result = GetEffectTemplate(classicKey);
        if (result == null)
            Debug.LogWarningFormat("Could not find effect template for type={0} subType={1}", type, subType);

        return result;
    }

    /// <summary>
    /// Generate EffectSettings from classic EffectRecordData.
    /// </summary>
    /// <param name="effectRecordData">Classic effect record data.</param>
    /// <returns>EffectSettings.</returns>
    public static EffectSettings ClassicEffectRecordToEffectSettings(EffectRecordData effectRecordData, bool supportDuration, bool supportChance, bool supportMagnitude)
    {
        EffectSettings effectSettings = BaseEntityEffect.DefaultEffectSettings();
        if (supportDuration)
        {
            effectSettings.DurationBase = effectRecordData.durationBase;
            effectSettings.DurationPlus = effectRecordData.durationMod;
            effectSettings.DurationPerLevel = Math.Max(effectRecordData.durationPerLevel, 1);
        }

        if (supportChance)
        {
            effectSettings.ChanceBase = effectRecordData.chanceBase;
            effectSettings.ChancePlus = effectRecordData.chanceMod;
            effectSettings.ChancePerLevel = Math.Max(effectRecordData.chancePerLevel, 1);
        }

        if (supportMagnitude)
        {
            effectSettings.MagnitudeBaseMin = effectRecordData.magnitudeBaseLow;
            effectSettings.MagnitudeBaseMax = effectRecordData.magnitudeBaseHigh;
            effectSettings.MagnitudePlusMin = effectRecordData.magnitudeLevelBase;
            effectSettings.MagnitudePlusMax = effectRecordData.magnitudeLevelHigh;
            effectSettings.MagnitudePerLevel = Math.Max(effectRecordData.magnitudePerLevel, 1);
        }

        return effectSettings;
    }

    /// <summary>
    /// Generate EffectEntry from classic EffectRecordData.
    /// </summary>
    /// <param name="effectRecordData">Classic effect record data.</param>
    /// <returns>EffectEntry.</returns>
    public static bool ClassicEffectRecordToEffectEntry(EffectRecordData effectRecordData, out EffectEntry effectEntryOut)
    {
        // Get template
        IEntityEffect effectTemplate = GetEffectTemplateFromClassicEffectRecordData(effectRecordData);
        if (effectTemplate == null)
        {
            effectEntryOut = new EffectEntry();
            return false;
        }

        // Get settings and create entry
        EffectSettings effectSettings = ClassicEffectRecordToEffectSettings(
            effectRecordData,
            effectTemplate.Properties.SupportDuration,
            effectTemplate.Properties.SupportChance,
            effectTemplate.Properties.SupportMagnitude);
        effectEntryOut = new EffectEntry(effectTemplate.Key, effectSettings);

        return true;
    }

    /// <summary>
    /// Generate EffectBundleSettings from classic SpellRecordData.
    /// </summary>
    /// <param name="spellRecordData">Classic spell record data.</param>
    /// <param name="bundleType">Type of bundle to create.</param>
    /// <param name="effectBundleSettingsOut">Effect bundle created by conversion.</param>
    /// <returns>True if successful, otherwise false.</returns>
    public static bool ClassicSpellRecordDataToEffectBundleSettings(SpellRecordData spellRecordData, BundleTypes bundleType, out EffectBundleSettings effectBundleSettingsOut)
    {
        // Spell record data must have effect records
        if (spellRecordData.effects == null || spellRecordData.effects.Length == 0)
        {
            effectBundleSettingsOut = new EffectBundleSettings();
            return false;
        }

        // Create bundle
        effectBundleSettingsOut = new EffectBundleSettings()
        {
            Version = CurrentSpellVersion,
            BundleType = bundleType,
            TargetType = ClassicTargetIndexToTargetType(spellRecordData.rangeType),
            ElementType = ClassicElementIndexToElementType(spellRecordData.element),
            Name = $"Spell-{spellRecordData.index}",
            IconIndex = spellRecordData.icon,
            // Icon = new SpellIcon(),
            StandardSpellIndex = spellRecordData.index, 
        };
        // effectBundleSettingsOut.Icon.index = effectBundleSettingsOut.IconIndex;

        // Assign effects
        List<EffectEntry> foundEffects = new List<EffectEntry>();
        for (int i = 0; i < spellRecordData.effects.Length; i++)
        {
            // Skip unused effect slots
            if (spellRecordData.effects[i].type == -1)
                continue;

            // Fix bad Free Action spell data from SPELLS.STD at runtime
            // Spell index 10 effect 0 references Cure Paralyzation effect type=3/subType=2 instead of the intended Free Action effect
            // Patch the type/subType values to match Free Action effect type=26/subType=-1
            // Note player will need to re-equip enchanted items or wait for next reroll tick before correct effect is applied
            if (spellRecordData.index == 10 && i == 0 && spellRecordData.effects[i].type == 3 && spellRecordData.effects[i].subType == 2)
            {
                spellRecordData.effects[i].type = 26;
                spellRecordData.effects[i].subType = -1;
            }

            // Get entry from effect
            EffectEntry entry;
            if (!ClassicEffectRecordToEffectEntry(spellRecordData.effects[i], out entry))
                continue;

            // Assign to valid effects
            foundEffects.Add(entry);
        }

        // Must have assigned at least one valid effect
        if (foundEffects.Count == 0)
            return false;

        // Assign effects to bundle
        effectBundleSettingsOut.Effects = foundEffects.ToArray();

        return true;
    }

    readonly Dictionary<int, string> classicEffectMapping = new Dictionary<int, string>();
    readonly Dictionary<string, BaseEntityEffect> magicEffectTemplates = new Dictionary<string, BaseEntityEffect>();
    readonly Dictionary<int, BaseEntityEffect> potionEffectTemplates = new Dictionary<int, BaseEntityEffect>();
    readonly Dictionary<int, SpellRecordData> standardSpells = new Dictionary<int, SpellRecordData>();
    readonly Dictionary<string, CustomSpellBundleOffer> customSpellBundleOffers = new Dictionary<string, CustomSpellBundleOffer>();
    /// <summary>
    /// Gets PotionRecipe from effect that matches the recipeKey provided.
    /// </summary>
    /// <param name="recipeKey">Hashcode of a set of ingredients.</param>
    /// <returns>PotionRecipe if the key matches one from an effect, otherwise null.</returns>
    public PotionRecipe GetPotionRecipe(int recipeKey)
    {
        if (potionEffectTemplates.ContainsKey(recipeKey))
        {
            foreach (PotionRecipe recipe in potionEffectTemplates[recipeKey].PotionProperties.Recipes)
                if (recipe.GetHashCode() == recipeKey)
                    return recipe;
        }
        return null;
    }
}

/// <summary>
/// Determines how effect chance will function.
/// OnCast: is checked at cast time by EffectManager receiving effect - effect is rejected on failure.
/// Custom: is always allowed by EffectManager, but still generates ChanceSuccess flag on Start().
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
/// Implements a potion recipe to support equality comparison, hashing, and string output.
/// Internally the recipe is just an array of item template IDs abstracted to an Ingredient type.
/// A recipe can be constructed from any Ingredient[] array or int[] array of item template IDs.
/// </summary>
public class PotionRecipe : IEqualityComparer<PotionRecipe.Ingredient[]>
{
    // Classic potion recipe mapping to DFU recipe keys:
    //  Stamina, Orc Strength, Healing, Waterwalking, Restore Power, Resist Fire, Resist Frost, Resist Shock, Cure Disease, Slow Falling,
    //  Water Breathing, Heal True, Levitation, Resist Poison, Free Action, Cure Poison, Chameleon Form, Shadow Form, Invisibility, Purification
    public static readonly int[] classicRecipeKeys = { 221871, 239524, 4975678, 5017404, 5188896, 111516185, 4826108, 216843, 224588, 220192,
                                                        240081, 4937012, 228890, 221117, 4870452, 5361377, 112080144, 4842851, 4815872, 2031019196 };

    #region Fields

    Ingredient[] ingredients = null;
    int textureRecord = 11;

    #endregion

    #region Properties

    public string DisplayName
    {
        get { return GetDisplayName(); }
    }

    /// <summary>
    /// The display name of this potion recipe.
    /// </summary>
    public string DisplayNameKey { get; set; }

    /// <summary>
    /// Custom display name of this potion recipe.
    /// If set, this property will be returned by DisplayName.
    /// </summary>
    public string CustomDisplayName { get; set; }

    /// <summary>
    /// The price of this potion recipe.
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// The texture record from archive 205 to use for this potion, default = 11.
    /// </summary>
    public int TextureRecord
    {
        get { return textureRecord; }
        set { textureRecord = value; }
    }

    /// <summary>
    /// Gets or sets effect settings for this recipe.
    /// </summary>
    public EffectSettings Settings { get; set; }

    /// <summary>
    /// Gets or sets potion recipe ingredients. Ingredients must be sorted by id.
    /// </summary>
    public Ingredient[] Ingredients
    {
        get { return ingredients; }
        set { ingredients = value; }
    }

    /// <summary>
    /// Retrieves a list of secondary effect keys.
    /// </summary>
    public List<string> SecondaryEffects { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor.
    /// </summary>
    public PotionRecipe()
    {
        Settings = BaseEntityEffect.DefaultEffectSettings();
    }

    /// <summary>
    /// Ingredient[] array constructor.
    /// </summary>
    /// <param name="displayNameKey">Potion name key to use for this recipe.</param>
    /// <param name="price">Value of potion in gp.</param>
    /// <param name="settings">Settings for this potion recipe.</param>
    /// <param name="ingredients">Ingredient array.</param>
    public PotionRecipe(string displayNameKey, int price, EffectSettings settings, params Ingredient[] ingredients)
    {
        DisplayNameKey = displayNameKey;
        Price = price;
        this.Settings = settings;
        Array.Sort(ingredients);
        this.ingredients = ingredients;
    }

    /// <summary>
    /// Ingredient[] array constructor - for finding and comparisons.
    /// </summary>
    /// <param name="ids">Ingredient ids list.</param>
    public PotionRecipe(List<int> ids)
    {
        ids.Sort();
        if (ids != null && ids.Count > 0)
        {
            ingredients = new Ingredient[ids.Count];
            for (int i = 0; i < ids.Count; i++)
                ingredients[i].id = ids[i];
        }
    }

    /// <summary>
    /// int[] array of item template IDs constructor.
    /// </summary>
    /// <param name="displayName">Potion name key to use for this recipe.</param>
    /// <param name="price">Value of potion in gp.</param>
    /// <param name="settings">Settings for this potion recipe.</param>
    /// <param name="ids">Array of item template IDs.</param>
    public PotionRecipe(string displayNameKey, int price, EffectSettings settings, params int[] ids)
    {
        DisplayNameKey = displayNameKey;
        Price = price;
        this.Settings = settings;
        Array.Sort(ids);
        if (ids != null && ids.Length > 0)
        {
            ingredients = new Ingredient[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                ingredients[i].id = ids[i];
            }
        }
    }

    #endregion

    #region Structures

    /// <summary>
    /// Abstracts an item ID into an ingredient.
    /// </summary>
    public struct Ingredient
    {
        public int id;

        public Ingredient(int id)
        {
            this.id = id;
        }
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Compare another PotionRecipe class with this one.
    /// </summary>
    /// <param name="other">Other potion recipe class.</param>
    /// <returns>True if both recipes are equal.</returns>
    public override bool Equals(object other)
    {
        if (other == null || !(other is PotionRecipe))
            return false;

        return Equals((other as PotionRecipe).Ingredients);
    }

    /// <summary>
    /// Gets hash code for this recipe.
    /// </summary>
    /// <returns>Hash code.</returns>
    public override int GetHashCode()
    {
        return GetHashCode(ingredients);
    }

    /// <summary>
    /// Gets string listing all ingredients.
    /// </summary>
    /// <returns>Ingredient list.</returns>
    public override string ToString()
    {
        if (ingredients == null || ingredients.Length == 0)
            return string.Empty;

        string result = string.Empty;
        for (int i = 0; i < ingredients.Length; i++)
        {
            ItemTemplate template = Items.GetItemTemplate(ingredients[i].id);
            result += template.name;
            if (i < ingredients.Length - 1)
                result += ", ";
            else
                result += ".";
        }

        return result;
    }

    #endregion

    #region Public Methods

    public string GetDisplayName()
    {
        // Allow for custom display name when set
        if (!string.IsNullOrEmpty(CustomDisplayName))
            return CustomDisplayName;

        // Resolve default name based on localization key
        if (string.IsNullOrEmpty(DisplayNameKey))
            return "unknownPowers";
        else
            return DisplayNameKey;
    }

    /// <summary>
    /// Checks if recipe is defined.
    /// </summary>
    /// <returns>True if recipe has at least one ingredient.</returns>
    public bool HasRecipe()
    {
        return (ingredients != null && ingredients.Length > 0);
    }

    /// <summary>
    /// Adds secondary effects to this potion recipe.
    /// </summary>
    /// <param name="effectKey">The EffectKey of the effect to add.</param>
    public void AddSecondaryEffect(string effectKey)
    {
        if (SecondaryEffects == null)
            SecondaryEffects = new List<string>();
        SecondaryEffects.Add(effectKey);
    }

    /// <summary>
    /// Compare a recipe with this one.
    /// </summary>
    /// <param name="ingredients">Other recipe.</param>
    /// <returns>True if other recipe equal with this one.</returns>
    public bool Equals(Ingredient[] ingredients)
    {
        return Equals(this.ingredients, ingredients);
    }

    /// <summary>
    /// Compare two recipes for equality.
    /// </summary>
    /// <param name="ingredients1">First recipe.</param>
    /// <param name="ingredients2">Second recipe.</param>
    /// <returns>True if recipes are equal.</returns>
    public bool Equals(Ingredient[] ingredients1, Ingredient[] ingredients2)
    {
        if (ingredients1 == null || ingredients2 == null || ingredients1.Length != ingredients2.Length)
            return false;

        for (int i = 0; i < ingredients1.Length; i++)
        {
            if (ingredients1[i].id != ingredients2[i].id)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Gets a hash code for recipe from ingredients.
    /// Note: Using hash code calculation from:
    /// https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
    /// </summary>
    /// <param name="ingredients">Ingredients.</param>
    /// <returns>Hash code.</returns>
    public int GetHashCode(Ingredient[] ingredients)
    {
        if (ingredients == null || ingredients.Length == 0)
            return 0;

        int hash = 17;
        for (int i = 0; i < ingredients.Length; i++)
        {
            hash = hash * 23 + ingredients[i].id;
        }

        return hash;
    }

    #endregion
}

/// <summary>
/// Settings for a single enchantment on item.
/// </summary>
[Serializable]
public struct EnchantmentSettings : IEquatable<EnchantmentSettings>
{
    public int Version;
    public string EffectKey;
    public EnchantmentTypes ClassicType;
    public short ClassicParam;
    public string CustomParam;
    public string PrimaryDisplayName;
    public string SecondaryDisplayName;
    public int EnchantCost;
    public int ParentEnchantment;

    public bool Equals(EnchantmentSettings other)
    {
        return
            Version == other.Version &&
            EffectKey == other.EffectKey &&
            ClassicType == other.ClassicType &&
            ClassicParam == other.ClassicParam &&
            CustomParam == other.CustomParam &&
            PrimaryDisplayName == other.PrimaryDisplayName &&
            SecondaryDisplayName == other.SecondaryDisplayName &&
            EnchantCost == other.EnchantCost;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is EnchantmentSettings))
            return false;

        return Equals((EnchantmentSettings)obj);
    }

    public static bool operator == (EnchantmentSettings enchantment1, EnchantmentSettings enchantment2)
    {
        if (((object)enchantment1) == null || ((object)enchantment2) == null)
            return System.Object.Equals(enchantment1, enchantment2);

        return enchantment1.Equals(enchantment2);
    }

    public static bool operator != (EnchantmentSettings enchantment1, EnchantmentSettings enchantment2)
    {
        if (((object)enchantment1) == null || ((object)enchantment2) == null)
            return !System.Object.Equals(enchantment1, enchantment2);

        return !(enchantment1.Equals(enchantment2));
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Version.GetHashCode();
        if (!string.IsNullOrEmpty(EffectKey)) hash = hash * 23 + EffectKey.GetHashCode();
        hash = hash * 23 + ClassicType.GetHashCode();
        hash = hash * 23 + ClassicParam.GetHashCode();
        if (!string.IsNullOrEmpty(CustomParam)) hash = hash * 23 + CustomParam.GetHashCode();
        if (!string.IsNullOrEmpty(PrimaryDisplayName)) hash = hash * 23 + PrimaryDisplayName.GetHashCode();
        if (!string.IsNullOrEmpty(SecondaryDisplayName)) hash = hash * 23 + SecondaryDisplayName.GetHashCode();
        hash = hash * 23 + EnchantCost.GetHashCode();

        return hash;
    }
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
    /// Called by an EffectManager when parent bundle is attached to host entity.
    /// Use this for setup or immediate work performed only once.
    /// </summary>
    void Start(EffectManager manager, Actor caster = null);

    /// <summary>
    /// Called by an EntityEffect manage when parent bundle is resumed from save.
    /// </summary>
    void Resume(EffectManager.EffectSaveData_v1 effectData, EffectManager manager, Actor caster = null);

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

/// <summary>
/// Base implementation of an entity effect.
/// Entity effects are like "actions" for spells, potions, items, advantages, diseases, etc.
/// They generally perform work against one or more entities (e.g. damage or restore health).
/// Some effects perform highly custom operations unique to player (e.g. anchor/teleport UI).
/// Magic effects are scripted in C# so they have full access to engine and UI as required.
/// Classic magic effects are included in build for cross-platform compatibility.
/// Custom effects can be added later using mod system (todo:).
/// </summary>
public abstract class BaseEntityEffect : IEntityEffect
{
    #region Fields

    protected EffectProperties properties = new EffectProperties();
    protected EffectSettings settings = new EffectSettings();
    protected PotionProperties potionProperties = new PotionProperties();
    protected Actor caster = null;
    protected EffectManager manager = null;
    protected int variantCount = 1;
    protected int currentVariant = 0;
    protected bool bypassSavingThrows = false;

    int roundsRemaining;
    bool chanceSuccess = false;
    int[] statMods = new int[DStats.Count];
    int[] statMaxMods = new int[DStats.Count];
    int[] skillMods = new int[DSkills.Count];
    int[] resistanceMods = new int[DResistances.Count];
    LiveEffectBundle parentBundle;
    bool effectEnded = false;

    #endregion

    #region Enums

    public enum ClassicEffectFamily
    {
        Spells,
        PowersAndSideEffects,
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor.
    /// </summary>
    protected BaseEntityEffect()
    {
        // Set default properties
        properties.ShowSpellIcon = true;
        properties.SupportDuration = false;
        properties.SupportChance = false;
        properties.SupportMagnitude = false;
        properties.AllowedTargets = TargetTypes.CasterOnly;
        properties.AllowedElements = ElementTypes.Magic;
        properties.AllowedCraftingStations = MagicCraftingStations.None;
        properties.MagicSkill = DFCareer.MagicSkills.None;

        // Set default settings
        settings = DefaultEffectSettings();

        // Allow effect to set own properties
        SetProperties();
        SetPotionProperties();
    }

    #endregion

    #region IEntityEffect Properties

    public virtual EffectProperties Properties
    {
        get { return properties; }
    }

    public EffectSettings Settings
    {
        get { return settings; }
        set { settings = value; }
    }

    public EnchantmentParam? EnchantmentParam { get; set; }

    public virtual PotionProperties PotionProperties
    {
        get { return potionProperties; }
    }

    public Actor Caster
    {
        get { return caster; }
    }

    public virtual int RoundsRemaining
    {
        get { return roundsRemaining; }
        set { roundsRemaining = value; }
    }

    public virtual bool ChanceSuccess
    {
        get { return chanceSuccess; }
    }

    public int[] StatMods
    {
        get { return statMods; }
    }

    public int[] StatMaxMods
    {
        get { return statMaxMods; }
    }

    public int[] SkillMods
    {
        get { return skillMods; }
    }

    public int[] ResistanceMods
    {
        get { return resistanceMods; }
    }

    public string Key
    {
        get { return Properties.Key; }
    }

    public virtual string GroupName
    {
        get { return string.Empty; }
    }

    public virtual string SubGroupName
    {
        get { return string.Empty; }
    }

    public virtual string DisplayName
    {
        get { return GetDisplayName(); }
    }

    // public virtual TextFile.Token[] SpellMakerDescription
    // {
    //     get { return null; }
    // }

    // public virtual TextFile.Token[] SpellBookDescription
    // {
    //     get { return null; }
    // }

    public LiveEffectBundle ParentBundle
    {
        get { return parentBundle; }
        set { parentBundle = value; }
    }

    public bool HasEnded
    {
        get { return effectEnded; }
        protected set { effectEnded = value; }
    }

    public int VariantCount
    {
        get { return variantCount; }
    }

    public int CurrentVariant
    {
        get { return currentVariant; }
        set { currentVariant = Mathf.Clamp(value, 0, variantCount - 1); }
    }

    public bool BypassSavingThrows
    {
        get { return bypassSavingThrows; }
    }

    #endregion

    #region IEntityEffect Virtual Methods

    public abstract void SetProperties();

    public virtual void SetPotionProperties()
    {
    }

    /// <summary>
    /// Gets all enchantment settings supported by this effect.
    /// Effects supporting MagicCraftingStations.ItemMaker must implement this method to become usable in item maker.
    /// The effect must also be able to execute enchantment payload when handed back settings. 
    /// </summary>
    /// <returns>EnchantmentSettings array. Can return null or empty array.</returns>
    public virtual EnchantmentSettings[] GetEnchantmentSettings()
    {
        return null;
    }

    /// <summary>
    /// Helper to get specific enchantment based on param.
    /// </summary>
    /// <param name="param">Param of enchantment to retrieve.</param>
    /// <returns>EnchantmentSettings for specificed param or null if not found.</returns>
    public virtual EnchantmentSettings? GetEnchantmentSettings(EnchantmentParam param)
    {
        // Get all enchantment settings for this effect
        EnchantmentSettings[] allSettings = GetEnchantmentSettings();
        if (allSettings == null || allSettings.Length == 0)
            return null;

        // Locate matching param
        bool usingCustomParam = !string.IsNullOrEmpty(param.CustomParam);
        foreach(EnchantmentSettings settings in allSettings)
        {
            if (usingCustomParam && param.CustomParam == settings.CustomParam)
                return settings;
            else if (!usingCustomParam && param.ClassicParam == settings.ClassicParam)
                return settings;
        }

        return null;
    }

    /// <summary>
    /// Helper to check properties carry specified item maker flags.
    /// </summary>
    /// <param name="flags">Flags to check.</param>
    /// <returns>True if flags specified.</returns>
    public virtual bool HasItemMakerFlags(ItemMakerFlags flags)
    {
        return (Properties.ItemMakerFlags & flags) == flags;
    }

    /// <summary>
    /// Helper to check if properties contain the specified enchantment payload flags
    /// </summary>
    /// <param name="flags">Flags to check.</param>
    /// <returns>True if flags specified.</returns>
    public virtual bool HasEnchantmentPayloadFlags(EnchantmentPayloadFlags flags)
    {
        return (Properties.EnchantmentPayloadFlags & flags) == flags;
    }

    /// <summary>
    /// Enchantment payload callback for enchantment to perform custom execution based on context.
    /// These callbacks are performed directly from template, not from a live instance of effect. Do not store state in effect during callbacks.
    /// Not used by EnchantmentPayloadFlags.Held - rather, an effect instance bundle is assigned to entity's effect manager to execute as normal.
    /// </summary>
    public virtual PayloadCallbackResults? EnchantmentPayloadCallback(EnchantmentPayloadFlags context, EnchantmentParam? param = null, Actor sourceEntity = null, Actor targetEntity = null, Item sourceItem = null, int sourceDamage = 0)
    {
        return null;
    }

    /// <summary>
    /// Enchantment can flag that it is exclusive to one or more enchantments in array provided.
    /// Used by enchanting window to prevent certain enchantments from being selected together.
    /// </summary>
    public virtual bool IsEnchantmentExclusiveTo(EnchantmentSettings[] settingsToTest, EnchantmentParam? comparerParam = null)
    {
        return false;
    }

    /// <summary>
    /// Gets related enchantments that will be forced onto item along with this enchantment.
    /// Only used by Soul Bound in classic gameplay.
    /// </summary>
    public virtual ForcedEnchantmentSet? GetForcedEnchantments(EnchantmentParam? param = null)
    {
        return null;
    }

    /// <summary>
    /// Starts effect running when first attached to an entity.
    /// Executes a MagicRound() tick immediately.
    /// Child classes must call base.Start() when overriding.
    /// NOTE: Start() is only called when effect is first instantiated - it not called again on load, see Resume().
    /// </summary>
    public virtual void Start(EffectManager manager, Actor caster = null)
    {
        this.manager = manager;
        this.caster = caster;
        SetDuration();
        SetChanceSuccess();
    }

    /// <summary>
    /// Restarts effect running after deserialization. Does not execute a MagicRound() tick.
    /// </summary>
    public virtual void Resume(EffectManager.EffectSaveData_v1 effectData, EffectManager manager, Actor caster = null)
    {
        this.manager = manager;
        this.caster = caster;
        roundsRemaining = effectData.roundsRemaining;
        chanceSuccess = effectData.chanceSuccess;
        statMods = effectData.statMods;
        statMaxMods = (effectData.statMaxMods != null) ? effectData.statMaxMods : new int[DStats.Count];
        skillMods = effectData.skillMods;
        variantCount = effectData.variantCount;
        currentVariant = effectData.currentVariant;
        effectEnded = effectData.effectEnded;
    }

    /// <summary>
    /// Called to perform any cleanup at end of lifetime, or when manually removed from host.
    /// </summary>
    public virtual void End()
    {
        effectEnded = true;
    }

    /// <summary>
    /// Called for effects that need to perform work each frame, such as setting a toggle in entity.
    /// </summary>
    public virtual void ConstantEffect()
    {
    }

    /// <summary>
    /// Called to execute effect payload on host and count down a magic round.
    /// Child classes must call base.MagicRound() when overriding to properly count rounds.
    /// </summary>
    public virtual void MagicRound()
    {
        RemoveRound();
    }

    /// <summary>
    /// Called to remove a magic round.
    /// Child classes should call base.RemoveRound() when overriding to properly count rounds.
    /// Otherwise child class will need to manually count rounds.
    /// </summary>
    /// <returns></returns>
    protected virtual int RemoveRound()
    {
        if (roundsRemaining <= 0)
            return 0;
        else
            return --roundsRemaining;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the attribute modifier of this effect.
    /// </summary>
    /// <param name="stat">Attribute to query.</param>
    /// <returns>Current attribute modifier.</returns>
    public int GetAttributeMod(DFCareer.Stats stat)
    {
        if (stat == DFCareer.Stats.None)
            return 0;

        return statMods[(int)stat];
    }

    /// <summary>
    /// Gets the attribute maximum modifier of this effect.
    /// </summary>
    /// <param name="stat">Attribute to query.</param>
    /// <returns>Current attribute maximum modifier.</returns>
    public int GetAttributeMaximumMod(DFCareer.Stats stat)
    {
        if (stat == DFCareer.Stats.None)
            return 0;

        return statMaxMods[(int)stat];
    }

    /// <summary>
    /// Gets the skill modifier of the effect.
    /// </summary>
    /// <param name="skill">Skill to query.</param>
    /// <returns>Current skill modifier.</returns>
    protected int GetSkillMod(DFCareer.Skills skill)
    {
        if (skill == DFCareer.Skills.None)
            return 0;

        return skillMods[(int)skill];
    }

    /// <summary>
    /// Heal attribute damage by amount.
    /// Does nothing if this effect does not damage attributes.
    /// Attribute will not heal past 0.
    /// </summary>
    /// <param name="stat">Attribute to heal.</param>
    /// <param name="amount">Amount to heal. Must be positive value.</param>
    public virtual void HealAttributeDamage(DFCareer.Stats stat, int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("EntityEffect.HealAttributeDamage() received a negative value for amount - ignoring.");
            return;
        }

        int result = GetAttributeMod(stat) + amount;
        if (result > 0)
            result = 0;

        SetStatMod(stat, result);
        Debug.LogFormat("Healed {0}'s {1} by {2} points", GetPeeredEntityBehaviour(manager).name, stat.ToString(), amount);
    }

    /// <summary>
    /// Cure all attribute damage by this effect.
    /// </summary>
    public virtual void CureAttributeDamage()
    {
        for (int i = 0; i < DStats.Count; i++)
        {
            int amount = GetAttributeMod((DFCareer.Stats)i);
            if (amount < 0)
                HealAttributeDamage((DFCareer.Stats)i, Mathf.Abs(amount));
        }
    }

    /// <summary>
    /// Heal skill damage by amount.
    /// Does nothing if this effect does not damage skills.
    /// Skill will not heal past 0.
    /// </summary>
    /// <param name="skill">Skill to heal.</param>
    /// <param name="amount">Amount to heal. Must be positive value.</param>
    public virtual void HealSkillDamage(DFCareer.Skills skill, int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("EntityEffect.HealSkillDamage() received a negative value for amount - ignoring.");
            return;
        }

        int result = GetSkillMod(skill) + amount;
        if (result > 0)
            result = 0;

        SetSkillMod(skill, result);
        Debug.LogFormat("Healed {0}'s {1} by {2} points", GetPeeredEntityBehaviour(manager).name, skill.ToString(), amount);
    }

    /// <summary>
    /// Cure all skill damage by this effect.
    /// </summary>
    public virtual void CureSkillDamage()
    {
        for (int i = 0; i < DSkills.Count; i++)
        {
            int amount = GetSkillMod((DFCareer.Skills)i);
            if (amount < 0)
                HealSkillDamage((DFCareer.Skills)i, Mathf.Abs(amount));
        }
    }

    /// <summary>
    /// Checks if all damaged attributes are healed back to 0.
    /// </summary>
    /// <returns>True if all attributes have returned to baseline.</returns>
    public bool AllAttributesHealed()
    {
        for (int i = 0; i < StatMods.Length; i++)
        {
            if (StatMods[i] < 0)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if all damaged skills are healed back to 0.
    /// </summary>
    /// <returns>True if all skills have returned to baseline.</returns>
    public bool AllSkillsHealed()
    {
        for (int i = 0; i < SkillMods.Length; i++)
        {
            if (SkillMods[i] < 0)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Gets final chance value based on caster level and effect settings.
    /// </summary>
    /// <returns>Chance value.</returns>
    public int ChanceValue()
    {
        int casterLevel = (caster) ? FormulaUtils.CalculateCasterLevel(caster, this) : 1;
        //Debug.LogFormat("{5} ChanceValue {0} = base + plus * (level/chancePerLevel) = {1} + {2} * ({3}/{4})", settings.ChanceBase + settings.ChancePlus * (int)Mathf.Floor(casterLevel / settings.ChancePerLevel), settings.ChanceBase, settings.ChancePlus, casterLevel, settings.ChancePerLevel, Key);
        return settings.ChanceBase + settings.ChancePlus * (int)Mathf.Floor(casterLevel / settings.ChancePerLevel);
    }

    /// <summary>
    /// Performs a chance roll for this effect based on chance settings.
    /// </summary>
    /// <returns>True if chance roll succeeded.</returns>
    public virtual bool RollChance()
    {
        if (!Properties.SupportChance)
            return false;

        bool outcome = Dice100.SuccessRoll(ChanceValue());

        //Debug.LogFormat("Effect '{0}' has a {1}% chance of succeeding and rolled {2} for a {3}", Key, ChanceValue(), roll, (outcome) ? "success" : "fail");

        return outcome;
    }

    /// <summary>
    /// Helper to compare the settings of this effect with another effect.
    /// Used to determine if the Duration, Chance, Magnitude settings are equivalent in both effects.
    /// </summary>
    /// <param name="other">Other effect for comparison.</param>
    public virtual bool CompareSettings(IEntityEffect other)
    {
        return this.Settings.Equals(other.Settings);
    }

    #endregion

    #region Protected Helpers

    protected Actor GetPeeredEntityBehaviour(EffectManager manager)
    {
        // Return cached entity behaviour or attempt to get component directly
        if (manager && manager.EntityBehaviour)
            return manager.EntityBehaviour;
        else
            return manager.GetComponent<Actor>();
    }

    protected int GetMagnitude(Actor caster = null)
    {
        if (caster == null)
            Debug.LogWarningFormat("GetMagnitude() for {0} has no caster. Using caster level 1 for magnitude.", Properties.Key);

        if (manager == null)
            Debug.LogWarningFormat("GetMagnitude() for {0} has no parent manager.", Properties.Key);

        int magnitude = 0;
        if (Properties.SupportMagnitude)
        {
            int casterLevel = (caster) ? FormulaUtils.CalculateCasterLevel(caster, this) : 1;
            int baseMagnitude = UnityEngine.Random.Range(settings.MagnitudeBaseMin, settings.MagnitudeBaseMax + 1);
            int plusMagnitude = UnityEngine.Random.Range(settings.MagnitudePlusMin, settings.MagnitudePlusMax + 1);
            int multiplier = (int)Mathf.Floor(casterLevel / settings.MagnitudePerLevel);
            magnitude = baseMagnitude + plusMagnitude * multiplier;
        }

        int initialMagnitude = magnitude;
        if (ParentBundle.targetType != TargetTypes.CasterOnly)
            magnitude = FormulaUtils.ModifyEffectAmount(this, manager.EntityBehaviour, magnitude);

        // Output "Save versus spell made." when magnitude is fully reduced to 0 by saving throw
        // if (initialMagnitude > 0 && magnitude == 0)
        //     DaggerfallUI.AddHUDText(TextManager.Instance.GetLocalizedText("saveVersusSpellMade"));

        return magnitude;
    }

    protected void PlayerAggro()
    {
        // Caster must be player
        if (caster != Main.Inst.hero)
            return;

        // Get peered entity gameobject
        Actor entityBehaviour = GetPeeredEntityBehaviour(manager);
        if (!entityBehaviour)
            return;

        // Cause aggro based on attack source
        entityBehaviour.HandleAttackFromSource(caster);
    }

    protected void SetStatMod(DFCareer.Stats stat, int value)
    {
        if (stat == DFCareer.Stats.None)
            return;

        statMods[(int)stat] = value;
    }

    protected void SetStatMaxMod(DFCareer.Stats stat, int value)
    {
        if (stat == DFCareer.Stats.None)
            return;

        statMaxMods[(int)stat] = value;
    }

    protected void ChangeStatMod(DFCareer.Stats stat, int amount)
    {
        if (stat == DFCareer.Stats.None)
            return;

        statMods[(int)stat] += amount;
    }

    protected void ChangeStatMaxMod(DFCareer.Stats stat, int amount)
    {
        if (stat == DFCareer.Stats.None)
            return;

        statMaxMods[(int)stat] += amount;
    }

    protected void SetSkillMod(DFCareer.Skills skill, int value)
    {
        if (skill == DFCareer.Skills.None)
            return;

        skillMods[(int)skill] = value;
    }

    protected void ChangeSkillMod(DFCareer.Skills skill, int amount)
    {
        if (skill == DFCareer.Skills.None)
            return;

        skillMods[(int)skill] += amount;
    }

    protected void SetResistanceMod(DFCareer.Elements resistance, int amount)
    {
        if (resistance == DFCareer.Elements.None)
            return;

        resistanceMods[(int)resistance] = amount;
    }

    protected void ChanceResistanceMod(DFCareer.Elements resistance, int amount)
    {
        if (resistance == DFCareer.Elements.None)
            return;

        resistanceMods[(int)resistance] += amount;
    }

    protected void AssignPotionRecipes(params PotionRecipe[] recipes)
    {
        if (recipes == null || recipes.Length == 0)
            return;

        potionProperties.Recipes = recipes;
    }

    #endregion

    #region Private Methods

    string GetDisplayName()
    {
        // Manufacture a default display name from group names
        // Effects can override DisplayName property to set a custom display name
        string groupName = GroupName;
        string subGroupName = SubGroupName;
        if (!string.IsNullOrEmpty(groupName) && !string.IsNullOrEmpty(subGroupName))
            return string.Format("{0} {1}", groupName, subGroupName);
        else if (!string.IsNullOrEmpty(groupName) && string.IsNullOrEmpty(subGroupName))
            return groupName;
        else
            return "noName";
    }

    void SetDuration()
    {
        int casterLevel = (caster) ? FormulaUtils.CalculateCasterLevel(caster, this) : 1;
        if (Properties.SupportDuration)
        {
            // Multiplier clamped at 1 or player can lose a round depending on spell settings and level
            int durationPerLevelMultiplier = (int)Mathf.Floor(casterLevel / settings.DurationPerLevel);
            if (durationPerLevelMultiplier < 1)
                durationPerLevelMultiplier = 1;
            roundsRemaining = settings.DurationBase + settings.DurationPlus * durationPerLevelMultiplier;
        }
        else
            roundsRemaining = 0;

        //Debug.LogFormat("Effect '{0}' will run for {1} magic rounds", Key, roundsRemaining);
    }

    void SetChanceSuccess()
    {
        chanceSuccess = RollChance();
    }

    #endregion

    #region Static Methods

    public static EffectSettings DefaultEffectSettings()
    {
        EffectSettings defaultSettings = new EffectSettings();

        // Default duration is 1 + 1 per level
        defaultSettings.DurationBase = 1;
        defaultSettings.DurationPlus = 1;
        defaultSettings.DurationPerLevel = 1;

        // Default chance is 1 + 1 per level
        defaultSettings.ChanceBase = 1;
        defaultSettings.ChancePlus = 1;
        defaultSettings.ChancePerLevel = 1;

        // Default magnitude is 1-1 + 1-1 per level
        defaultSettings.MagnitudeBaseMin = 1;
        defaultSettings.MagnitudeBaseMax = 1;
        defaultSettings.MagnitudePlusMin = 1;
        defaultSettings.MagnitudePlusMax = 1;
        defaultSettings.MagnitudePerLevel = 1;

        return defaultSettings;
    }

    public static EffectSettings SetEffectDuration(EffectSettings settings, int durationBase, int durationPlus, int durationPerLevel)
    {
        settings.DurationBase = durationBase;
        settings.DurationPlus = durationPlus;
        settings.DurationPerLevel = durationPerLevel;

        return settings;
    }

    public static EffectSettings SetEffectChance(EffectSettings settings, int chanceBase, int chancePlus, int chancePerLevel)
    {
        settings.ChanceBase = chanceBase;
        settings.ChancePlus = chancePlus;
        settings.ChancePerLevel = chancePerLevel;

        return settings;
    }

    public static EffectSettings SetEffectMagnitude(EffectSettings settings, int magnitudeBaseMin, int magnitudeBaseMax, int magnitudePlusMin, int magnitudePlusMax, int magnitudePerLevel)
    {
        settings.MagnitudeBaseMin = magnitudeBaseMin;
        settings.MagnitudeBaseMax = magnitudeBaseMax;
        settings.MagnitudePlusMin = magnitudePlusMin;
        settings.MagnitudePlusMax = magnitudePlusMax;
        settings.MagnitudePerLevel = magnitudePerLevel;

        return settings;
    }

    public static int MakeClassicKey(byte groupIndex, byte subgroupIndex, ClassicEffectFamily family = ClassicEffectFamily.Spells)
    {
        return ((int)family << 16) + (groupIndex << 8) + subgroupIndex;
    }

    public static void ReverseClasicKey(int key, out byte groupIndex, out byte subgroupIndex, out ClassicEffectFamily family)
    {
        family = (ClassicEffectFamily)(key >> 16);
        groupIndex = (byte)(key >> 8);
        subgroupIndex = (byte)(key & 0xff);
    }

    public static EffectCosts MakeEffectCosts(float costA, float costB, float offsetGold = 0)
    {
        EffectCosts costs = new EffectCosts();
        costs.OffsetGold = offsetGold;
        costs.CostA = costA;
        costs.CostB = costB;

        return costs;
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Override to provide save data for effect.
    /// Not all effects need to be stateful.
    /// </summary>
    public virtual object GetSaveData()
    {
        return null;
    }

    /// <summary>
    /// Override to restore save data for effect.
    /// </summary>
    public virtual void RestoreSaveData(object dataIn)
    {
    }

    #endregion
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
/// Some effects in Daggerfall add to the state an existing like-kind effect (the incumbent)
/// rather than become instantiated as a new effect on the host entity.
/// One example is a drain effect which only adds to the magnitude of incumbent drain for same stat.
/// Another example is an effect which tops up the duration of same effect in progress.
/// This class establishes a base for these incumbent effects to coordinate.
/// NOTES:
///  Unflagged incumbent effects (IsIncumbent == false) do not persist beyond AddState() call.
///  They will never receive a single MagicRound() call and are never saved/loaded.
///  The flagged incumbent (IsIncumbent == true) receives MagicRound() calls and is saved/load as normal.
/// </summary>
public abstract class IncumbentEffect : BaseEntityEffect
{
    bool isIncumbent = false;

    public override void Start(EffectManager manager, Actor caster = null)
    {
        base.Start(manager, caster);
        AttachHost();
    }

    public override void Resume(EffectManager.EffectSaveData_v1 effectData, EffectManager manager, Actor caster = null)
    {
        base.Resume(effectData, manager, caster);
        isIncumbent = effectData.isIncumbent;
    }

    public bool IsIncumbent
    {
        get { return isIncumbent; }
    }

    void AttachHost()
    {
        IncumbentEffect incumbent = FindIncumbent();
        if (incumbent == null)
        {
            // First instance of effect on this host becomes flagged incumbent
            isIncumbent = true;
            BecomeIncumbent();

            //Debug.LogFormat("Creating incumbent effect '{0}' on host '{1}'", DisplayName, manager.name);
        }
        else
        {
            // Subsequent instances add to state of flagged incumbent
            AddState(incumbent);

            //Debug.LogFormat("Adding state to incumbent effect '{0}' on host '{1}'", incumbent.DisplayName, incumbent.manager.name);
        }
    }

    IncumbentEffect FindIncumbent()
    {
        // Search for any incumbents on this host matching group
        LiveEffectBundle[] bundles = manager.EffectBundles;
        foreach (LiveEffectBundle bundle in bundles)
        {
            if (bundle.bundleType == ParentBundle.bundleType)
            {
                foreach (IEntityEffect effect in bundle.liveEffects)
                {
                    if (effect is IncumbentEffect)
                    {
                        // Effect must be flagged incumbent and agree with like-kind test
                        IncumbentEffect other = effect as IncumbentEffect;
                        if (other.IsIncumbent && other.IsLikeKind(this))
                            return other;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Resign as incumbent effect.
    /// This allows an incumbent to immediately allow for a new incumbent to take over its post.
    /// Useful for when incumbent does not want to receive any further AddState() calls and cannot wait for magic round tick to expire.
    /// </summary>
    protected void ResignAsIncumbent()
    {
        isIncumbent = false;
    }

    protected virtual void BecomeIncumbent()
    {
    }

    protected abstract bool IsLikeKind(IncumbentEffect other);
    protected abstract void AddState(IncumbentEffect incumbent);
}


/// <summary>
/// Spell Absorption
/// </summary>
public class SpellAbsorption : IncumbentEffect
{
    public static readonly string EffectKey = "SpellAbsorption";

    public override void SetProperties()
    {
        properties.Key = EffectKey;
        properties.ClassicKey = MakeClassicKey(20, 255);
        properties.SupportDuration = true;
        properties.SupportChance = true;
        properties.ChanceFunction = ChanceFunction.Custom;
        properties.AllowedTargets = Effects.TargetFlags_All;
        properties.AllowedElements = Effects.ElementFlags_MagicOnly;
        properties.AllowedCraftingStations = MagicCraftingStations.SpellMaker;
        properties.MagicSkill = DFCareer.MagicSkills.Restoration;
        properties.DurationCosts = MakeEffectCosts(28, 140);
        properties.ChanceCosts = MakeEffectCosts(28, 140);
    }

    // public override string GroupName => TextManager.Instance.GetLocalizedText("spellAbsorption");
    // public override TextFile.Token[] SpellMakerDescription => DaggerfallUnity.Instance.TextProvider.GetRSCTokens(1568);
    // public override TextFile.Token[] SpellBookDescription => DaggerfallUnity.Instance.TextProvider.GetRSCTokens(1268);

    protected override bool IsLikeKind(IncumbentEffect other)
    {
        return (other.Key == Key) ? true : false;
    }

    protected override void AddState(IncumbentEffect incumbent)
    {
        incumbent.RoundsRemaining += RoundsRemaining;
    }
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
/// Allows an effect to override player's racial display information such as race name and portrait.
/// Used for vampirism and lycanthropy and possibly could be used for future racial overrides.
/// Considered a minimal implementation at this time for core game to support vamp/were only.
/// Only intended to be used on player entity. Will be permanent until removed.
/// Only a single racial override incumbent effect can be active on player at one time.
/// </summary>
public abstract class RacialOverrideEffect : IncumbentEffect
{
    #region Fields

    protected int forcedRoundsRemaining = 1;

    #endregion

    #region Properties

    /// <summary>
    /// Allow racial override to suppress Combat Voices option as required.
    /// </summary>
    public virtual bool SuppressOptionalCombatVoices
    {
        get { return false; }
    }

    /// <summary>
    /// Allow racial override to suppress paper doll body and items to show background only.
    /// </summary>
    public virtual bool SuppressPaperDollBodyAndItems
    {
        get { return false; }
    }

    /// <summary>
    /// Allows racial override to suppress crimes by player.
    /// </summary>
    public virtual bool SuppressCrime
    {
        get { return false; }
    }

    /// <summary>
    /// Allows racial override to suppress population spawns.
    /// </summary>
    public virtual bool SuppressPopulationSpawns
    {
        get { return false; }
    }

    #endregion

    #region Overrides

    // Always present at least one round remaining so effect system does not remove
    public override int RoundsRemaining
    {
        get { return forcedRoundsRemaining; }
    }

    // Racial overrides are permanent until removed so we manage our own lifecycle
    protected override int RemoveRound()
    {
        return forcedRoundsRemaining;
    }

    protected override bool IsLikeKind(IncumbentEffect other)
    {
        return (other is RacialOverrideEffect);
    }

    protected override void AddState(IncumbentEffect incumbent)
    {
        return;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Called by WeaponManager when player hits an entity with a weapon (includes hand-to-hand).
    /// Target entity may be null, racial overrides should handle this.
    /// </summary>
    public virtual void OnWeaponHitEntity(Hero playerEntity, Actor targetEntity = null)
    {
    }

    /// <summary>
    /// Checks if custom race can initiate fast travel.
    /// Return true to allow fast travel or false to block it.
    /// </summary>
    public virtual bool CheckFastTravel(Hero playerEntity)
    {
        return true;
    }

    /// <summary>
    /// Checks if custom race can initiate rest.
    /// Return true to allow rest or false to block it.
    /// </summary>
    public virtual bool CheckStartRest(Hero playerEntity)
    {
        return true;
    }

    /// <summary>
    /// Starts custom racial quest.
    ///  * Called every 38 days with isCureQuest = false
    ///  * Called every 84 days with isCureQuest = true
    /// Mainly used by vampirism and lycanthropy in core.
    /// Custom racial override effects can ignore this virtual to start and manage quests however they like.
    /// </summary>
    /// <param name="isCureQuest">True when this should start cure quest.</param>
    public virtual void StartQuest(bool isCureQuest)
    {
    }

    /// <summary>
    /// Allow racial override to suppress inventory UI.
    /// Some care might need to be taken by other systems this does not crash game like classic.
    /// </summary>
    /// <param name="suppressInventoryMessage">Optional message to display when inventory suppressed.</param>
    /// <returns>True if inventory should be suppressed.</returns>
    public virtual bool GetSuppressInventory(out string suppressInventoryMessage)
    {
        suppressInventoryMessage = string.Empty;
        return false;
    }

    /// <summary>
    /// Allow racial overrides to suppress talk UI.
    /// </summary>
    /// <param name="suppressTalkMessage">Optional message to display when talk suppressed.</param>
    /// <returns>True if talk should be suppressed.</returns>
    public virtual bool GetSuppressTalk(out string suppressTalkMessage)
    {
        suppressTalkMessage = string.Empty;
        return false;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets custom race exposed by this override
    /// </summary>
    public abstract RaceTemplate CustomRace { get; }

    #endregion
}

/// <summary>
/// Uber-effect used to deliver passive special advantages and disadvantages to player.
/// More active specials (e.g. critical strike, disallowed armour types) are handled in related systems.
/// NOTES:
///  * This effect is a work in progress and will be added to over time.
///  * Could also be assigned to other entities but at this time only using on player.
/// </summary>
public class PassiveSpecialsEffect : IncumbentEffect
{
    #region Fields

    public static readonly string EffectKey = "Passive-Specials";
    const int sunDamageAmount = 12;
    const int holyDamageAmount = 12;
    const int regenerateAmount = 1;
    const int sunDamagePerRounds = 4;
    const int holyDamagePerRounds = 4;
    const int regeneratePerRounds = 4;

    int forcedRoundsRemaining = 1;
    Actor entityBehaviour;

    #endregion

    #region Overrides

    public override void SetProperties()
    {
        properties.Key = EffectKey;
        properties.ShowSpellIcon = false;
        bypassSavingThrows = true;
    }

    public override int RoundsRemaining
    {
        get { return forcedRoundsRemaining; }
    }

    protected override int RemoveRound()
    {
        return forcedRoundsRemaining;
    }

    protected override bool IsLikeKind(IncumbentEffect other)
    {
        return (other is PassiveSpecialsEffect);
    }

    protected override void AddState(IncumbentEffect incumbent)
    {
        return;
    }

    public override void Start(EffectManager manager, Actor caster = null)
    {
        base.Start(manager, caster);
        CacheReferences();
    }

    public override void Resume(EffectManager.EffectSaveData_v1 effectData, EffectManager manager, Actor caster = null)
    {
        base.Resume(effectData, manager, caster);
        CacheReferences();
    }

    public override void ConstantEffect()
    {
        base.ConstantEffect();

        // Execute constant advantages/disadvantages
        if (entityBehaviour)
        {
            LightPoweredMagery();
            DarknessPoweredMagery();
        }
    }

    public override void MagicRound()
    {
        base.MagicRound();

        // Execute round-based effects
        if (entityBehaviour)
        {
            RegenerateHealth();
            DamageFromSunlight();
            DamageFromHolyPlaces();
        }
    }

    #endregion

    #region Regeneration

    void RegenerateHealth()
    {
        // // This special only triggers once every regeneratePerRounds
        // if (GameManager.Instance.EntityEffectBroker.MagicRoundsSinceStartup % regeneratePerRounds != 0)
        //     return;

        // // Check for regenerate conditions
        // bool regenerate = false;
        // switch(entityBehaviour.Entity.Career.Regeneration)
        // {
        //     default:
        //     case DFCareer.RegenerationFlags.None:
        //         return;
        //     case DFCareer.RegenerationFlags.Always:
        //         regenerate = true;
        //         break;
        //     case DFCareer.RegenerationFlags.InDarkness:
        //         regenerate = DaggerfallUnity.Instance.WorldTime.Now.IsNight || GameManager.Instance.PlayerEnterExit.WorldContext == WorldContext.Dungeon;
        //         break;
        //     case DFCareer.RegenerationFlags.InLight:
        //         regenerate = DaggerfallUnity.Instance.WorldTime.Now.IsDay && GameManager.Instance.PlayerEnterExit.WorldContext != WorldContext.Dungeon;
        //         break;
        //     case DFCareer.RegenerationFlags.InWater:
        //         regenerate = manager.IsPlayerEntity && (GameManager.Instance.PlayerMotor.IsSwimming || GameManager.Instance.PlayerMotor.OnExteriorWater == PlayerMotor.OnExteriorWaterMethod.Swimming);
        //         break;
        // }

        // // Tick regeneration when conditions are right
        // if (regenerate)
        //     entityBehaviour.Entity.IncreaseHealth(regenerateAmount);
    }

    #endregion

    #region Sun Damage

    void DamageFromSunlight()
    {
        // // This special only triggers once every sunDamagePerRounds
        // if (GameManager.Instance.EntityEffectBroker.MagicRoundsSinceStartup % sunDamagePerRounds != 0)
        //     return;

        // // From entity career (e.g. vampire enemy mobile)
        // bool fromCareer = entityBehaviour.Entity.Career.DamageFromSunlight;

        // // From player race (e.g. vampire curse)
        // bool fromRace = (manager.IsPlayerEntity) ?
        //     ((entityBehaviour.Entity as PlayerEntity).RaceTemplate.SpecialAbilities & DFCareer.SpecialAbilityFlags.SunDamage) == DFCareer.SpecialAbilityFlags.SunDamage :
        //     false;

        // // Must have career or race active
        // if (!fromCareer && !fromRace)
        //     return;

        // // Apply damage while in sunlight
        // if (GameManager.Instance.PlayerEnterExit.IsPlayerInSunlight)
        // {
        //     entityBehaviour.Entity.DecreaseHealth(sunDamageAmount);
        //     //Debug.LogFormat("Applied {0} points of sun damage after {1} magic (game minutes)", sunDamageAmount, sunDamagePerRounds);
        // }
    }

    #endregion

    #region Holy Damage

    void DamageFromHolyPlaces()
    {
        // This special only triggers once every holyDamagePerRounds
        // if (GameManager.Instance.EntityEffectBroker.MagicRoundsSinceStartup % holyDamagePerRounds != 0)
        //     return;

        // // From entity career (e.g. vampire enemy mobile)
        // bool fromCareer = entityBehaviour.Entity.Career.DamageFromHolyPlaces;

        // // From player race (e.g. vampire curse)
        // bool fromRace = (manager.IsPlayerEntity) ?
        //     ((entityBehaviour.Entity as PlayerEntity).RaceTemplate.SpecialAbilities & DFCareer.SpecialAbilityFlags.HolyDamage) == DFCareer.SpecialAbilityFlags.HolyDamage :
        //     false;

        // // Must have career or race active
        // if (!fromCareer && !fromRace)
        //     return;

        // // Apply damage when inside a holy place
        // if (GameManager.Instance.PlayerEnterExit.IsPlayerInHolyPlace)
        // {
        //     entityBehaviour.Entity.DecreaseHealth(holyDamageAmount);
        //     //Debug.LogFormat("Applied {0} points of holy damage after {1} magic rounds (game minutes)", holyDamageAmount, holyDamagePerRounds);
        // }
    }

    #endregion

    #region Light & Dark Powered Magery

    void LightPoweredMagery()
    {
        // Entity suffers darkness disadvantage at night or inside dungeons and buildings
        // if (DaggerfallUnity.Instance.WorldTime.Now.IsNight || GameManager.Instance.PlayerEnterExit.IsPlayerInside)
        // {
        //     // Disadvantage has two variants
        //     switch (entityBehaviour.Entity.Career.LightPoweredMagery)
        //     {
        //         case DFCareer.LightMageryFlags.ReducedPowerInDarkness:
        //             entityBehaviour.Entity.ChangeMaxMagickaModifier((int)(entityBehaviour.Entity.RawMaxMagicka * -0.33f));  // 33% less magicka in darkness
        //             break;

        //         case DFCareer.LightMageryFlags.UnableToCastInDarkness:
        //             entityBehaviour.Entity.ChangeMaxMagickaModifier(-10000000);                                             // 0 magicka in light
        //             break;
        //     }
        // }
    }

    void DarknessPoweredMagery()
    {
        // Entity suffers light disadvantage during the day while outside
        // if (GameManager.Instance.PlayerEnterExit.IsPlayerInSunlight)
        // {
        //     // Disadvantage has two variants
        //     switch (entityBehaviour.Entity.Career.DarknessPoweredMagery)
        //     {
        //         case DFCareer.DarknessMageryFlags.ReducedPowerInLight:
        //             entityBehaviour.Entity.ChangeMaxMagickaModifier((int)(entityBehaviour.Entity.RawMaxMagicka * -0.33f));  // 33% less magicka in light
        //             break;

        //         case DFCareer.DarknessMageryFlags.UnableToCastInLight:
        //             entityBehaviour.Entity.ChangeMaxMagickaModifier(-10000000);                                             // 0 magicka in light
        //             break;
        //     }
        // }
    }

    #endregion

    #region Private Methods

    void CacheReferences()
    {
        // Cache reference to peered entity behaviour
        if (!entityBehaviour)
            entityBehaviour = GetPeeredEntityBehaviour(manager);
    }

    #endregion
}