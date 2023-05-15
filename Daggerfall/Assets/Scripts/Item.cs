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

    ulong uid;
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
    
    const ushort identifiedMask = 0x20;
    const ushort artifactMask = 0x800;

    public Item(ItemGroups itemGroup, int groupIndex)
    {
        uid = NextUID;
        SetItem(itemGroup, groupIndex);
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
}
