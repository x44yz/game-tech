using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum Weapons  //checked
// {
//     Dagger = 113,
//     Tanto = 114,
//     Staff = 115,
//     Shortsword = 116,
//     Wakazashi = 117,
//     Broadsword = 118,
//     Saber = 119,
//     Longsword = 120,
//     Katana = 121,
//     Claymore = 122,
//     Dai_Katana = 123,
//     Mace = 124,
//     Flail = 125,
//     Warhammer = 126,
//     Battle_Axe = 127,
//     War_Axe = 128,
//     Short_Bow = 129,
//     Long_Bow = 130,
//     Arrow = 131,
// }

/// <summary>
/// Weapon material values.
/// </summary>
public enum WeaponMaterialTypes
{
    None        = -1,
    Iron        = 0x0000,
    Steel       = 0x0001,
    Silver      = 0x0002,
    Elven       = 0x0003,
    Dwarven     = 0x0004,
    Mithril     = 0x0005,
    Adamantium  = 0x0006,
    Ebony       = 0x0007,
    Orcish      = 0x0008,
    Daedric     = 0x0009,
}

/// <summary>
/// Proficiency flags for forbidden weapons and weapon expertise.
/// </summary>
[Flags]
public enum ProficiencyFlags
{
    ShortBlades = 1,
    LongBlades = 2,
    HandToHand = 4,
    Axes = 8,
    BluntWeapons = 16,
    MissileWeapons = 32,
}

/// <summary>
/// Poison IDs. The first 8 are found on enemy weapons. The last 4 are created by ingesting drugs.
/// </summary>
public enum Poisons
{
    None = -1,
    Nux_Vomica = 128,
    Arsenic = 129,
    Moonseed = 130,
    Drothweed = 131,
    Somnalius = 132,
    Pyrrhic_Acid = 133,
    Magebane = 134,
    Thyrwort = 135,
    Indulcet = 136,
    Sursum = 137,
    Quaesto_Vil = 138,
    Aegrotat = 139,
}

/// <summary>
/// Base group of item.
/// </summary>
public enum ItemGroups
{
    None = -1,
    Drugs = 0,
    UselessItems1 = 1,
    Armor = 2,
    Weapons = 3,
    MagicItems = 4,
    Artifacts = 5,
    MensClothing = 6,
    Books = 7,
    Furniture = 8,
    UselessItems2 = 9,
    ReligiousItems = 10,
    Maps = 11,
    WomensClothing = 12,
    Paintings = 13,
    Gems = 14,
    PlantIngredients1 = 15,
    PlantIngredients2 = 16,
    CreatureIngredients1 = 17,
    CreatureIngredients2 = 18,
    CreatureIngredients3 = 19,
    MiscellaneousIngredients1 = 20,
    MetalIngredients = 21,
    MiscellaneousIngredients2 = 22,
    Transportation = 23,
    Deeds = 24,
    Jewellery = 25,
    QuestItems = 26,
    MiscItems = 27,
    Currency = 28,
}

/// <summary>
/// Defines a custom enchantment for items.
/// Classic enchantments use a type/param number pair in DaggerfallEnchantment.
/// Custom enchantments use a key/param string pair in CustomEnchantment.
/// </summary>
[Serializable]
public struct CustomEnchantment
{
    public string EffectKey;                                    // Define the effect used by this enchantment
    public string CustomParam;                                  // Passed back to effect to locate/invoke enchantment settings
}

public class Item
{
    public int TemplateIndex; // item id
    // 伏魔的
    public bool IsEnchanted;
    public Poisons poisonType = Poisons.None;

    static ulong currentUID = 0x2000000;
    public static ulong NextUID
    {
        get { return currentUID++; }
    }

    public ulong uid;
    ItemGroups itemGroup;
    int groupIndex;
    public float weightInKg;
    public int drawOrder;
    public int value;
    int currentVariant = 0;
    public ushort unknown;
    public int currentCondition;
    public int maxCondition;
    public byte unknown2;
    public byte typeDependentData;
    public int enchantmentPoints;
    public int stackCount = 1;
    public DEnchantment[] legacyMagic = null;
    public CustomEnchantment[] customMagic = null;
    
    const ushort identifiedMask = 0x20;
    const ushort artifactMask = 0x800;

    public Item()
    {
        uid = NextUID;
    }

    public Item(ItemGroups itemGroup, int groupIndex)
    {
        uid = NextUID;
        SetItem(itemGroup, groupIndex);
    }

    public Item(Item item)
    {
        uid = NextUID;
        FromItem(item);
    }

    // Horses, carts, arrows and maps are not counted against encumbrance.
    public float EffectiveUnitWeightInKg()
    {
        if (ItemGroup == ItemGroups.Transportation || TemplateIndex == (int)Weapons.Arrow ||
            IsOfTemplate(ItemGroups.MiscItems, (int)MiscItems.Map))
            return 0f;
        return weightInKg;
    }

    /// <summary>
    /// Creates from another item instance.
    /// </summary>
    void FromItem(Item other)
    {
        // shortName = other.shortName;
        itemGroup = other.itemGroup;
        groupIndex = other.groupIndex;
        // playerTextureArchive = other.playerTextureArchive;
        // playerTextureRecord = other.playerTextureRecord;
        // worldTextureArchive = other.worldTextureArchive;
        // worldTextureRecord = other.worldTextureRecord;
        nativeMaterialValue = other.nativeMaterialValue;
        // dyeColor = other.dyeColor;
        weightInKg = other.weightInKg;
        drawOrder = other.drawOrder;
        currentVariant = other.currentVariant;
        value = other.value;
        unknown = other.unknown;
        flags = other.flags;
        currentCondition = other.currentCondition;
        maxCondition = other.maxCondition;
        unknown2 = other.unknown2;
        stackCount = other.stackCount;
        enchantmentPoints = other.enchantmentPoints;
        // message = other.message;
        // potionRecipeKey = other.potionRecipeKey;
        // timeHealthLeechLastUsed = other.timeHealthLeechLastUsed;
        // artifactIndexBitfield = other.artifactIndexBitfield;

        // isQuestItem = other.isQuestItem;
        // questUID = other.questUID;
        // questItemSymbol = other.questItemSymbol;

        if (other.legacyMagic != null)
            legacyMagic = (DEnchantment[])other.legacyMagic.Clone();

        if (other.customMagic != null)
            customMagic = (CustomEnchantment[])other.customMagic.Clone();
    }

    /// <summary>
    /// Gets or sets item group.
    /// Setting will reset item data from new template.
    /// </summary>
    public ItemGroups ItemGroup
    {
        get { return itemGroup; }
        set { SetItem(value, groupIndex); }
    }

    /// <summary>
    /// Gets or sets item group index.
    /// Setting will reset item data from new template.
    /// </summary>
    public virtual int GroupIndex
    {
        get { return groupIndex; }
        set { SetItem(itemGroup, value); }
    }

    /// <summary>
    /// Sets item from group and index.
    /// Resets item data from new template.
    /// Retains existing UID.
    /// </summary>
    /// <param name="itemGroup">Item group.</param>
    /// <param name="groupIndex">Item group index.</param>
    public void SetItem(ItemGroups itemGroup, int groupIndex)
    {
        // Hand off for artifacts
        if (itemGroup == ItemGroups.Artifacts)
        {
            SetArtifact(itemGroup, groupIndex);
            return;
        }

        // Get template data
        ItemTemplate itemTemplate = Items.GetItemTemplate(itemGroup, groupIndex);

        // Assign new data
        // shortName = TextManager.Instance.GetLocalizedItemName(itemTemplate.index, itemTemplate.name);
        this.itemGroup = itemGroup;
        this.groupIndex = groupIndex;
        // playerTextureArchive = itemTemplate.playerTextureArchive;
        // playerTextureRecord = itemTemplate.playerTextureRecord;
        // worldTextureArchive = itemTemplate.worldTextureArchive;
        // worldTextureRecord = itemTemplate.worldTextureRecord;
        nativeMaterialValue = 0;
        // dyeColor = DyeColors.Unchanged;
        weightInKg = itemTemplate.baseWeight;
        drawOrder = itemTemplate.drawOrderOrEffect;
        currentVariant = 0;
        value = itemTemplate.basePrice;
        unknown = 0;
        flags = 0;
        currentCondition = itemTemplate.hitPoints;
        maxCondition = itemTemplate.hitPoints;
        unknown2 = 0;
        typeDependentData = 0;
        enchantmentPoints = itemTemplate.enchantmentPoints;
        // message = (itemGroup == ItemGroups.Paintings) ? UnityEngine.Random.Range(0, 65536) : 0;
        stackCount = 1;
    }

    /// <summary>
    /// Sets item by merging item template and artifact template data.
    /// Result is a normal DaggerfallUnityItem with properties of both base template and magic item template.
    /// </summary>
    /// <param name="groupIndex">Artifact group index.</param>
    public void SetArtifact(ItemGroups itemGroup, int groupIndex)
    {
        // Must be an artifact type
        if (itemGroup != ItemGroups.Artifacts)
            throw new Exception("An attempt was made to SetArtifact() with non-artifact ItemGroups value.");

        // Get artifact template
        MagicItemTemplate magicItemTemplate = Items.GetArtifactTemplate(groupIndex);

        // Get base item template data, this is the fundamental item type being expanded
        ItemTemplate itemTemplate = Items.GetItemTemplate((ItemGroups)magicItemTemplate.group, magicItemTemplate.groupIndex);

        // Get artifact texture indices
        // int archive, record;
        // DaggerfallUnity.Instance.ItemHelper.GetArtifactTextureIndices((ArtifactsSubTypes)groupIndex, out archive, out record);

        // Correct material value for armor artifacts
        int materialValue = magicItemTemplate.material;
        if (magicItemTemplate.group == (int)ItemGroups.Armor)
            materialValue = 0x200 + materialValue;

        // Assign new data
        // shortName = TextManager.Instance.GetLocalizedMagicItemName((int)magicItemTemplate.index, magicItemTemplate.name);
        this.itemGroup = (ItemGroups)magicItemTemplate.group;
        this.groupIndex = magicItemTemplate.groupIndex;
        // artifactIndexBitfield = groupIndex << 1 | 1;
        // playerTextureArchive = archive;
        // playerTextureRecord = record;
        // worldTextureArchive = archive;                  // Not sure about artifact world textures, just using player texture for now
        // worldTextureRecord = record;
        nativeMaterialValue = materialValue;
        // dyeColor = DyeColors.Unchanged;
        weightInKg = itemTemplate.baseWeight;
        drawOrder = itemTemplate.drawOrderOrEffect;
        currentVariant = 0;
        value = magicItemTemplate.value;
        unknown = 0;
        flags = artifactMask | identifiedMask;      // Set as artifact & identified.
        currentCondition = magicItemTemplate.uses;
        maxCondition = magicItemTemplate.uses;
        unknown2 = 0;
        typeDependentData = 0;
        enchantmentPoints = 0;
        // message = 0;
        stackCount = 1;

        // All artifacts have magical effects
        bool foundEnchantment = false;
        legacyMagic = new DEnchantment[magicItemTemplate.enchantments.Length];
        for (int i = 0; i < magicItemTemplate.enchantments.Length; i++)
        {
            legacyMagic[i] = magicItemTemplate.enchantments[i];
            if (legacyMagic[i].type != EnchantmentTypes.None)
                foundEnchantment = true;
        }

        // Discard list if no enchantment found
        if (!foundEnchantment)
            legacyMagic = null;
    }

    public ushort flags;

    public virtual int GetBaseDamageMin()
    {
        return FormulaUtils.CalculateWeaponMinDamage((Weapons)TemplateIndex);
    }

    public virtual int GetBaseDamageMax()
    {
        return FormulaUtils.CalculateWeaponMaxDamage((Weapons)TemplateIndex);
    }

    public bool IsAStack() 
    {
        return stackCount > 1;
    }

    public int nativeMaterialValue;
    /// <summary>
    /// Gets native material value.
    /// </summary>
    public virtual int NativeMaterialValue
    {
        get { return nativeMaterialValue; }
    }

    public virtual int GetWeaponSkillUsed()
    {
        switch (TemplateIndex)
        {
            case (int)Weapons.Dagger:
            case (int)Weapons.Tanto:
            case (int)Weapons.Wakazashi:
            case (int)Weapons.Shortsword:
                return (int)ProficiencyFlags.ShortBlades;
            case (int)Weapons.Broadsword:
            case (int)Weapons.Longsword:
            case (int)Weapons.Saber:
            case (int)Weapons.Katana:
            case (int)Weapons.Claymore:
            case (int)Weapons.Dai_Katana:
                return (int)ProficiencyFlags.LongBlades;
            case (int)Weapons.Battle_Axe:
            case (int)Weapons.War_Axe:
                return (int)ProficiencyFlags.Axes;
            case (int)Weapons.Flail:
            case (int)Weapons.Mace:
            case (int)Weapons.Warhammer:
            case (int)Weapons.Staff:
                return (int)ProficiencyFlags.BluntWeapons;
            case (int)Weapons.Short_Bow:
            case (int)Weapons.Long_Bow:
                return (int)ProficiencyFlags.MissileWeapons;

            default:
                return (int)Skills.None;
        }
    }

    public virtual short GetWeaponSkillIDAsShort()
    {
        int skill = GetWeaponSkillUsed();
        switch (skill)
        {
            case (int)ProficiencyFlags.ShortBlades:
                return (int)Skills.ShortBlade;
            case (int)ProficiencyFlags.LongBlades:
                return (int)Skills.LongBlade;
            case (int)ProficiencyFlags.Axes:
                return (int)Skills.Axe;
            case (int)ProficiencyFlags.BluntWeapons:
                return (int)Skills.BluntWeapon;
            case (int)ProficiencyFlags.MissileWeapons:
                return (int)Skills.Archery;

            default:
                return (int)Skills.None;
        }
    }

    public Skills GetWeaponSkillID()
    {
        return (Skills)GetWeaponSkillIDAsShort();
    }

    public int GetWeaponMaterialModifier()
    {
        switch (nativeMaterialValue)
        {
            case (int)WeaponMaterialTypes.Iron:
                return -1;
            case (int)WeaponMaterialTypes.Steel:
            case (int)WeaponMaterialTypes.Silver:
                return 0;
            case (int)WeaponMaterialTypes.Elven:
                return 1;
            case (int)WeaponMaterialTypes.Dwarven:
                return 2;
            case (int)WeaponMaterialTypes.Mithril:
            case (int)WeaponMaterialTypes.Adamantium:
                return 3;
            case (int)WeaponMaterialTypes.Ebony:
                return 4;
            case (int)WeaponMaterialTypes.Orcish:
                return 5;
            case (int)WeaponMaterialTypes.Daedric:
                return 6;

            default:
                return 0;
        }
    }

    /// <summary>
    /// Generates a weapon.
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="material">Ignored for arrows</param>
    /// <returns></returns>
    public static Item CreateWeapon(Weapons weapon, WeaponMaterialTypes material)
    {
        // Create item
        int groupIndex = Items.GetGroupIndex(ItemGroups.Weapons, (int)weapon);
        var newItem = new Item(ItemGroups.Weapons, groupIndex);

        if (weapon == Weapons.Arrow)
        {   // Handle arrows
            newItem.stackCount = UnityEngine.Random.Range(1, 20 + 1);
            newItem.currentCondition = 0; // not sure if this is necessary, but classic does it
            newItem.nativeMaterialValue = 0;
        }
        else
        {
            ApplyWeaponMaterial(newItem, material);
        }
        return newItem;
    }

    /// <summary>Set material and adjust weapon stats accordingly</summary>
    public static void ApplyWeaponMaterial(Item weapon, WeaponMaterialTypes material)
    {
        weapon.nativeMaterialValue = (int)material;
        weapon = SetItemPropertiesByMaterial(weapon, material);
        // weapon.dyeColor = DaggerfallUnity.Instance.ItemHelper.GetWeaponDyeColor(material);

        // Female characters use archive - 1 (i.e. 233 rather than 234) for weapons
        // if (GameManager.Instance.PlayerEntity.Gender == Genders.Female)
        //     weapon.PlayerTextureArchive -= 1;
    }

    // This array is used to pick random material values.
    // The array is traversed, subtracting each value from a sum until the sum is less than the next value.
    // Steel through Daedric, or Iron if sum is less than the first value.
    public static readonly byte[] materialsByModifier = { 64, 128, 10, 21, 13, 8, 5, 3, 2, 5 };
    // Weight multipliers by material type. Iron through Daedric. Weight is baseWeight * value / 4.
    static readonly short[] weightMultipliersByMaterial = { 4, 5, 4, 4, 3, 4, 4, 2, 4, 5 };
    // Value multipliers by material type. Iron through Daedric. Value is baseValue * ( 3 * value).
    static readonly short[] valueMultipliersByMaterial = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 };
    // Condition multipliers by material type. Iron through Daedric. MaxCondition is baseMaxCondition * value / 4.
    static readonly short[] conditionMultipliersByMaterial = { 4, 6, 6, 8, 12, 16, 20, 24, 28, 32 };

    /// <summary>
    /// Sets properties for a weapon or piece of armor based on its material.
    /// </summary>
    /// <param name="item">Item to have its properties modified.</param>
    /// <param name="material">Material to use to apply properties.</param>
    /// <returns>DaggerfallUnityItem</returns>
    public static Item SetItemPropertiesByMaterial(Item item, WeaponMaterialTypes material)
    {
        item.value *= 3 * valueMultipliersByMaterial[(int)material];
        item.weightInKg = CalculateWeightForMaterial(item, material);
        item.maxCondition = item.maxCondition * conditionMultipliersByMaterial[(int)material] / 4;
        item.currentCondition = item.maxCondition;

        return item;
    }

    static float CalculateWeightForMaterial(Item item, WeaponMaterialTypes material)
    {
        int quarterKgs = (int)(item.weightInKg * 4);
        float matQuarterKgs = (float)(quarterKgs * weightMultipliersByMaterial[(int)material]) / 4;
        return Mathf.Round(matQuarterKgs) / 4;
    }

    // 魔法创造的物品消失时间
    // Time for magically-created item to disappear
    uint timeForItemToDisappear = 0;

    // 是否是一个召唤物品
    /// <summary>
    /// Get flag checking if this is a summoned item.
    /// Summoned items have a specific future game time they expire.
    /// Non-summoned items have 0 in this field.
    /// </summary>
    public bool IsSummoned
    {
        get { return timeForItemToDisappear != 0; }
    }

    /// <summary>
    /// Determines if item is stackable.
    /// Only ingredients, potions, gold pieces, oil and arrows are stackable,
    /// but equipped items, enchanted ingredients and quest items are never stackable.
    /// </summary>
    /// <returns>True if item stackable.</returns>
    public virtual bool IsStackable()
    {
        if (IsSummoned)
        {
            // Only allowing summoned arrows to stack at this time
            // But they should only stack with other summoned arrows
            if (!IsOfTemplate(ItemGroups.Weapons, (int)Weapons.Arrow))
                return false;
        }

        // Equipped, quest and enchanted items cannot stack.
        if (IsEquipped || IsQuestItem || IsEnchanted)
            return false;

        return FormulaUtils.IsItemStackable(this);
    }

   /// <summary>
    /// Checks if item is of both group and template index.
    /// </summary>
    /// <param name="itemGroup">Item group to check.</param>
    /// <param name="templateIndex">Template index to check.</param>
    /// <returns>True if item matches both group and template index.</returns>
    public bool IsOfTemplate(ItemGroups itemGroup, int templateIndex)
    {
        if (ItemGroup == itemGroup && TemplateIndex == templateIndex)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Checks if item is of template index.
    /// </summary>
    /// <param name="templateIndex">Template index to check.</param>
    /// <returns>True if item matches template index.</returns>
    public bool IsOfTemplate(int templateIndex)
    {
        return (TemplateIndex == templateIndex);
    }

    // References current slot if equipped
    EquipSlots equipSlot = EquipSlots.None;
    /// <summary>
    /// Checks if this item is equipped.
    /// </summary>
    public bool IsEquipped
    {
        get { return equipSlot != EquipSlots.None; }
    }
    /// <summary>
    /// Gets temp equip slot.
    /// </summary>
    public EquipSlots EquipSlot
    {
        get { return equipSlot; }
        set { equipSlot = value; }
    }

    // TODO:@dongl1n
    // why None
    public virtual EquipSlots GetEquipSlot()
    {
        return EquipSlots.None;
    }

    public virtual ItemHands GetItemHands()
    {
        return ItemHands.None;
    }

    public bool IsQuestItem => false;

    public void LowerCondition(int amount, Actor unequipFromOwner = null, ItemCollection removeFromCollectionWhenBreaks = null)
    {
        currentCondition -= amount;
        if (currentCondition <= 0)
        {
            // Handle breaking - if AllowMagicRepairs enabled then item will not disappear
            currentCondition = 0;
            ItemBreaks(unequipFromOwner);
            if (removeFromCollectionWhenBreaks != null && !DaggerfallUnity.Settings.AllowMagicRepairs)
                removeFromCollectionWhenBreaks.RemoveItem(this);
        }
    }

    public void UnequipItem(Actor owner)
    {
        if (owner == null)
            return;

        foreach (EquipSlots slot in Enum.GetValues(typeof(EquipSlots)))
        {
            if (owner.ItemEquipTable.GetItem(slot) == this)
            {
                owner.ItemEquipTable.UnequipItem(slot);
                owner.UpdateEquippedArmorValues(this, false);
            }
        }
    }

    protected void ItemBreaks(Actor owner)
    {
        // Classic does not have the plural version of this string, and uses the short name rather than the long one.
        // Also the classic string says "is" instead of "has"
        // string itemBroke = "";
        // if (TemplateIndex == (int)Armor.Boots || TemplateIndex == (int)Armor.Gauntlets || TemplateIndex == (int)Armor.Greaves)
        //     itemBroke = TextManager.Instance.GetLocalizedText("itemHasBrokenPlural");
        // else
        //     itemBroke = TextManager.Instance.GetLocalizedText("itemHasBroken");
        // itemBroke = itemBroke.Replace("%s", LongName);
        // DaggerfallUI.Instance.PopupMessage(itemBroke);

        // Unequip item if owner specified
        if (owner != null)
            UnequipItem(owner);
        else
            return;

        // Breaks payload callback on owner effect manager
        if (owner)
        {
            EffectManager ownerEffectManager = owner.GetComponent<EffectManager>();
            if (ownerEffectManager)
                ownerEffectManager.DoItemEnchantmentPayloads(EnchantmentPayloadFlags.Breaks, this, owner.Items, owner);
        }
    }

    /// <summary>
    /// Get this item's template data.
    /// </summary>
    public ItemTemplate ItemTemplate
    {
        get { return GetCachedItemTemplate(); }
    }

    // Item template is cached for faster checks
    // Does not need to be serialized
    ItemTemplate cachedItemTemplate;
    ItemGroups cachedItemGroup = ItemGroups.None;
    int cachedGroupIndex = -1;
    /// <summary>
    /// Caches item template.
    /// </summary>
    ItemTemplate GetCachedItemTemplate()
    {
        if (itemGroup != cachedItemGroup || groupIndex != cachedGroupIndex)
        {
            cachedItemTemplate = Items.GetItemTemplate(itemGroup, groupIndex);
            cachedItemGroup = itemGroup;
            cachedGroupIndex = groupIndex;
        }

        return cachedItemTemplate;
    }
}

/// <summary>
/// Equipment slots available to equip items.
/// Indices match Daggerfall's legacy equip slots for import.
/// Some unknowns still need to be resolved.
/// </summary>
public enum EquipSlots
{
    None = -1,
    Amulet0 = 0,            // Amulets / Torcs
    Amulet1 = 1,
    Bracelet0 = 2,          // Bracelets
    Bracelet1 = 3,
    Ring0 = 4,              // Rings
    Ring1 = 5,
    Bracer0 = 6,            // Bracers
    Bracer1 = 7,
    Mark0 = 8,              // Marks
    Mark1 = 9,
    Crystal0 = 10,          // Gems
    Crystal1 = 11,
    Head = 12,              // Helm
    RightArm = 13,          // Right pauldron
    Cloak1 = 14,            // Cloaks
    LeftArm = 15,           // Left pauldron
    Cloak2 = 16,            // Cloaks
    ChestClothes = 17,      // Shirt / Straps / Armband / Eodoric / Tunic / Surcoat / Plain robes / etc.
    ChestArmor = 18,        // Cuirass
    RightHand = 19,         // Right weapon / Two-handed weapon
    Gloves = 20,            // Gauntlets
    LeftHand = 21,          // Left weapon / Shields
    Unknown1 = 22,
    LegsArmor = 23,         // Greaves
    LegsClothes = 24,       // Khajiit suit / Loincloth / Skirt / etc.
    Unknown2 = 25,
    Feet = 26,              // Boots / Shoes / Sandals / etc.
}