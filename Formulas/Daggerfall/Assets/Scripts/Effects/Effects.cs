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
        int classicKey = BaseEffect.MakeClassicKey((byte)type, (byte)subType);

        // Attempt to find the effect template
        IEntityEffect result = Effects.GetEffectTemplate(classicKey);
        if (result == null)
            Debug.LogWarningFormat("Could not find effect template for type={0} subType={1}", type, subType);

        return result;
    }

    /// <summary>
    /// Gets classic spell record data.
    /// </summary>
    /// <param name="id">ID of spell.</param>
    /// <param name="spellOut">Spell record data (if found).</param>
    /// <returns>True if spell found, otherwise false.</returns>
    // public static bool GetClassicSpellRecord(int id, out SpellRecordData spellOut)
    // {
    //     // if (standardSpells.ContainsKey(id))
    //     // {
    //     //     spellOut = standardSpells[id];
    //     //     return true;
    //     // }
    // }

    /// <summary>
    /// Generate EffectSettings from classic EffectRecordData.
    /// </summary>
    /// <param name="effectRecordData">Classic effect record data.</param>
    /// <returns>EffectSettings.</returns>
    public static EffectSettings ClassicEffectRecordToEffectSettings(EffectRecordData effectRecordData, bool supportDuration, bool supportChance, bool supportMagnitude)
    {
        EffectSettings effectSettings = BaseEffect.DefaultEffectSettings();
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

    static readonly Dictionary<int, string> classicEffectMapping = new Dictionary<int, string>();
    static readonly Dictionary<string, BaseEffect> magicEffectTemplates = new Dictionary<string, BaseEffect>();
    static readonly Dictionary<int, BaseEffect> potionEffectTemplates = new Dictionary<int, BaseEffect>();
    // static readonly Dictionary<int, SpellRecordData> standardSpells = new Dictionary<int, SpellRecordData>();
    // static readonly Dictionary<string, CustomSpellBundleOffer> customSpellBundleOffers = new Dictionary<string, CustomSpellBundleOffer>();
    /// <summary>
    /// Gets PotionRecipe from effect that matches the recipeKey provided.
    /// </summary>
    /// <param name="recipeKey">Hashcode of a set of ingredients.</param>
    /// <returns>PotionRecipe if the key matches one from an effect, otherwise null.</returns>
    public static PotionRecipe GetPotionRecipe(int recipeKey)
    {
        if (potionEffectTemplates.ContainsKey(recipeKey))
        {
            foreach (PotionRecipe recipe in potionEffectTemplates[recipeKey].PotionProperties.Recipes)
                if (recipe.GetHashCode() == recipeKey)
                    return recipe;
        }
        return null;
    }

    /// <summary>
    /// Gets IEntityEffect from PotionRecipe.
    /// </summary>
    /// <param name="recipe">Input recipe.</param>
    /// <returns>IEntityEffect if this recipe is linked to an effect, otherwise null.</returns>
    public static IEntityEffect GetPotionRecipeEffect(PotionRecipe recipe)
    {
        if (recipe != null)
        {
            int recipeKey = recipe.GetHashCode();
            if (potionEffectTemplates.ContainsKey(recipeKey))
                return potionEffectTemplates[recipeKey];
        }

        return null;
    }

    /// <summary>
    /// Determine if a key exists in the templates dictionary.
    /// </summary>
    /// <param name="key">Key for template.</param>
    /// <returns>True if template exists.</returns>
    public static bool HasEffectTemplate(string key)
    {
        return magicEffectTemplates.ContainsKey(key);
    }

    /// <summary>
    /// Determine if a classic key exists in the templates dictionary.
    /// </summary>
    /// <param name="classicKey">Classic key for template.</param>
    /// <returns>True if template exists.</returns>
    public static bool HasEffectTemplate(int classicKey)
    {
        return classicEffectMapping.ContainsKey(classicKey);
    }

    /// <summary>
    /// Gets interface to effect template.
    /// Use this to query properties to all effects with this key.
    /// </summary>
    /// <param name="key">Effect key.</param>
    /// <returns>Interface to effect template only (has default effect settings).</returns>
    public static IEntityEffect GetEffectTemplate(string key)
    {
        if (!HasEffectTemplate(key))
            return null;

        return magicEffectTemplates[key];
    }

    /// <summary>
    /// Gets interface to effect template from classic key.
    /// </summary>
    /// <param name="classicKey">Classic key.</param>
    /// <returns>Interface to effect template only (has default effect settings).</returns>
    public static IEntityEffect GetEffectTemplate(int classicKey)
    {
        if (!HasEffectTemplate(classicKey))
            return null;

        return magicEffectTemplates[classicEffectMapping[classicKey]];
    }

    /// <summary>
    /// Creates a new instance of effect with specified settings.
    /// Use this to create a new effect with unique settings for actual use.
    /// </summary>
    /// <param name="effectEntry">EffectEntry with effect settings.</param>
    /// <returns>Interface to new effect instance.</returns>
    public static IEntityEffect InstantiateEffect(EffectEntry effectEntry)
    {
        return InstantiateEffect(effectEntry.Key, effectEntry.Settings);
    }

    /// <summary>
    /// Creates a new instance of effect with specified settings.
    /// Use this to create a new effect with unique settings for actual use.
    /// </summary>
    /// <param name="key">Effect key.</param>
    /// <param name="settings">Effect settings.</param>
    /// <returns>Interface to new effect instance.</returns>
    public static IEntityEffect InstantiateEffect(string key, EffectSettings settings)
    {
        if (!HasEffectTemplate(key))
            return null;

        IEntityEffect effectTemplate = magicEffectTemplates[key];
        IEntityEffect effectInstance = Activator.CreateInstance(effectTemplate.GetType()) as IEntityEffect;
        effectInstance.Settings = settings;
        effectInstance.CurrentVariant = effectTemplate.CurrentVariant;

        return effectInstance;
    }
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
        Settings = BaseEffect.DefaultEffectSettings();
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




