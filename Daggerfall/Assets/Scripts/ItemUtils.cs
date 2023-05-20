using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemUtils
{
    public const int firstFemaleArchive = 245;
        public const int firstMaleArchive = 249;
        private const int chooseAtRandom = -1;

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

        // Enchantment point/gold value data for item powers
        static readonly int[] extraSpellPtsEnchantPts = { 0x1F4, 0x1F4, 0x1F4, 0x1F4, 0xC8, 0xC8, 0xC8, 0x2BC, 0x320, 0x384, 0x3E8 };
        static readonly int[] potentVsEnchantPts = { 0x320, 0x384, 0x3E8, 0x4B0 };
        static readonly int[] regensHealthEnchantPts = { 0x0FA0, 0x0BB8, 0x0BB8 };
        static readonly int[] vampiricEffectEnchantPts = { 0x7D0, 0x3E8 };
        static readonly int[] increasedWeightAllowanceEnchantPts = { 0x190, 0x258 };
        static readonly int[] improvesTalentsEnchantPts = { 0x1F4, 0x258, 0x258 };
        static readonly int[] goodRepWithEnchantPts = { 0x3E8, 0x3E8, 0x3E8, 0x3E8, 0x3E8, 0x1388 };
        static readonly int[][] enchantmentPtsForItemPowerArrays = { null, null, null, extraSpellPtsEnchantPts, potentVsEnchantPts, regensHealthEnchantPts,
                                                                    vampiricEffectEnchantPts, increasedWeightAllowanceEnchantPts, null, null, null, null, null,
                                                                    improvesTalentsEnchantPts, goodRepWithEnchantPts};
        static readonly ushort[] enchantmentPointCostsForNonParamTypes = { 0, 0x0F448, 0x0F63C, 0x0FF9C, 0x0FD44, 0, 0, 0, 0x384, 0x5DC, 0x384, 0x64, 0x2BC };


    /// <summary>
    /// Generates armour.
    /// </summary>
    /// <param name="gender">Gender armor is created for.</param>
    /// <param name="race">Race armor is created for.</param>
    /// <param name="armor">Type of armor item to create.</param>
    /// <param name="material">Material of armor.</param>
    /// <param name="variant">Visual variant of armor. If -1, a random variant is chosen.</param>
    /// <returns>DaggerfallUnityItem</returns>
    public static Item CreateArmor(Genders gender, Races race, Armor armor, ArmorMaterialTypes material, int variant = -1)
    {
        // Create item
        int groupIndex = Items.GetGroupIndex(ItemGroups.Armor, (int)armor);
        var newItem = new Item(ItemGroups.Armor, groupIndex);

        ApplyArmorSettings(newItem, gender, race, material, variant);

        return newItem;
    }

    /// <summary>
    /// Creates a generic item from group and template index.
    /// </summary>
    /// <param name="itemGroup">Item group.</param>
    /// <param name="templateIndex">Template index.</param>
    /// <returns>DaggerfallUnityItem.</returns>
    public static Item CreateItem(ItemGroups itemGroup, int templateIndex)
    {
        // Handle custom items
        // if (templateIndex > ItemHelper.LastDFTemplate)
        // {
        //     // Allow custom item classes to be instantiated when registered
        //     Type itemClassType;
        //     if (DaggerfallUnity.Instance.ItemHelper.GetCustomItemClass(templateIndex, out itemClassType))
        //         return (DaggerfallUnityItem)Activator.CreateInstance(itemClassType);
        //     else
        //         return new DaggerfallUnityItem(itemGroup, templateIndex);
        // }

        // Create classic item
        int groupIndex = Items.GetGroupIndex(itemGroup, templateIndex);
        if (groupIndex == -1)
        {
            Debug.LogErrorFormat("ItemBuilder.CreateItem() encountered an item with an invalid GroupIndex. Check you're passing 'template index' matching a value in ItemEnums - e.g. (int)Weapons.Dagger NOT a 'group index' (e.g. 0).");
            return null;
        }
        var newItem = new Item(itemGroup, groupIndex);

        return newItem;
    }

    /// <summary>
    /// Creates random armor.
    /// </summary>
    /// <param name="playerLevel">Player level for material type.</param>
    /// <param name="gender">Gender armor is created for.</param>
    /// <param name="race">Race armor is created for.</param>
    /// <returns>DaggerfallUnityItem</returns>
    public static Item CreateRandomArmor(int playerLevel, Genders gender, Races race)
    {
        // Create a random armor type, including any custom items registered as armor
        Array enumArray = Items.GetEnumArray(ItemGroups.Armor);
        int[] customItemTemplates = Items.GetCustomItemsForGroup(ItemGroups.Armor);

        int groupIndex = UnityEngine.Random.Range(0, enumArray.Length + customItemTemplates.Length);
        Item newItem;
        if (groupIndex < enumArray.Length)
            newItem = new Item(ItemGroups.Armor, groupIndex);
        else
            newItem = CreateItem(ItemGroups.Armor, customItemTemplates[groupIndex - enumArray.Length]);

        ApplyArmorSettings(newItem, gender, race, FormulaUtils.RandomArmorMaterial(playerLevel));

        return newItem;
    }

    /// <summary>Set gender, body morphology and material of armor</summary>
    public static void ApplyArmorSettings(Item armor, Genders gender, Races race, ArmorMaterialTypes material, int variant = 0)
    {
        // Adjust for gender
        // if (gender == Genders.Female)
        //     armor.PlayerTextureArchive = firstFemaleArchive;
        // else
        //     armor.PlayerTextureArchive = firstMaleArchive;

        // Adjust for body morphology
        // SetRace(armor, race);

        // Adjust material
        // ApplyArmorMaterial(armor, material);

        // Adjust for variant
        if (variant >= 0)
            SetVariant(armor, variant);
        else
            RandomizeArmorVariant(armor);
    }

    public static void SetVariant(Item item, int variant)
    {
        // Range check
        int totalVariants = item.ItemTemplate.variants;
        if (variant < 0 || variant >= totalVariants)
            return;

        // Clamp to appropriate variant based on material family
        if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Cuirass))
        {
            if (item.nativeMaterialValue == (int)ArmorMaterialTypes.Leather)
                variant = 0;
            else if (item.nativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.nativeMaterialValue == (int)ArmorMaterialTypes.Chain2)
                variant = 4;
            else
                variant = Mathf.Clamp(variant, 1, 3);
        }
        else if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Greaves))
        {
            if (item.nativeMaterialValue == (int)ArmorMaterialTypes.Leather)
                variant = Mathf.Clamp(variant, 0, 1);
            else if (item.nativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.nativeMaterialValue == (int)ArmorMaterialTypes.Chain2)
                variant = 6;
            else
                variant = Mathf.Clamp(variant, 2, 5);
        }
        else if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Left_Pauldron) || item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Right_Pauldron))
        {
            if (item.nativeMaterialValue == (int)ArmorMaterialTypes.Leather)
                variant = 0;
            else if (item.nativeMaterialValue == (int)ArmorMaterialTypes.Chain || item.nativeMaterialValue == (int)ArmorMaterialTypes.Chain2)
                variant = 4;
            else
                variant = Mathf.Clamp(variant, 1, 3);
        }
        else if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Gauntlets))
        {
            if (item.nativeMaterialValue == (int)ArmorMaterialTypes.Leather)
                variant = 0;
            else
                variant = 1;
        }
        else if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Boots))
        {
            if (item.nativeMaterialValue == (int)ArmorMaterialTypes.Leather)
                variant = 0;
            else
                variant = Mathf.Clamp(variant, 1, 2);
        }

        // Store variant
        item.CurrentVariant = variant;
    }

    /// <summary>
    /// Sets a random variant of armor item.
    /// </summary>
    /// <param name="item">Item to randomize variant.</param>
    public static void RandomizeArmorVariant(Item item)
    {
        int variant = 0;

        // We only need to pick randomly where there is more than one possible variant. Otherwise we can just pass in 0 to SetVariant and
        // the correct variant will still be chosen.
        if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Cuirass) && (item.nativeMaterialValue >= (int)ArmorMaterialTypes.Iron))
        {
            variant = UnityEngine.Random.Range(1, 4);
        }
        else if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Greaves))
        {
            if (item.nativeMaterialValue == (int)ArmorMaterialTypes.Leather)
                variant = UnityEngine.Random.Range(0, 2);
            else if (item.nativeMaterialValue >= (int)ArmorMaterialTypes.Iron)
                variant = UnityEngine.Random.Range(2, 6);
        }
        else if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Left_Pauldron) || item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Right_Pauldron))
        {
            if (item.nativeMaterialValue >= (int)ArmorMaterialTypes.Iron)
                variant = UnityEngine.Random.Range(1, 4);
        }
        else if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Boots) && (item.nativeMaterialValue >= (int)ArmorMaterialTypes.Iron))
        {
            variant = UnityEngine.Random.Range(1, 3);
        }
        else if (item.IsOfTemplate(ItemGroups.Armor, (int)Armor.Helm))
        {
            variant = UnityEngine.Random.Range(0, item.ItemTemplate.variants);
        }
        SetVariant(item, variant);
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

    /// <summary>
    /// Converts Daggerfall weapon to generic API WeaponType.
    /// </summary>
    /// <param name="item">Weapon to convert.</param>
    /// <returns>WeaponTypes.</returns>
    public static WeaponTypes ConvertItemToAPIWeaponType(Item item)
    {
        // Must be a weapon
        if (item.ItemGroup != ItemGroups.Weapons)
            return WeaponTypes.None;

        // Find FPS animation set for this weapon type
        // Daggerfall re-uses the same animations for many different weapons

        // Check for a custom item weapon type, if None then continue
        WeaponTypes result = item.GetWeaponType();
        if (result != WeaponTypes.None)
            return result;

        switch (item.TemplateIndex)
        {
            case (int)Weapons.Dagger:
                result = WeaponTypes.Dagger;
                break;
            case (int)Weapons.Staff:
                result = WeaponTypes.Staff;
                break;
            case (int)Weapons.Tanto:
            case (int)Weapons.Shortsword:
            case (int)Weapons.Wakazashi:
            case (int)Weapons.Broadsword:
            case (int)Weapons.Saber:
            case (int)Weapons.Longsword:
            case (int)Weapons.Katana:
            case (int)Weapons.Claymore:
            case (int)Weapons.Dai_Katana:
                result = WeaponTypes.LongBlade;
                break;
            case (int)Weapons.Mace:
                result = WeaponTypes.Mace;
                break;
            case (int)Weapons.Flail:
                result = WeaponTypes.Flail;
                break;
            case (int)Weapons.Warhammer:
                result = WeaponTypes.Warhammer;
                break;
            case (int)Weapons.Battle_Axe:
            case (int)Weapons.War_Axe:
                result = WeaponTypes.Battleaxe;
                break;
            case (int)Weapons.Short_Bow:
            case (int)Weapons.Long_Bow:
                result = WeaponTypes.Bow;
                break;
            default:
                return WeaponTypes.None;
        }

        // Handle enchanted weapons
        if (item.IsEnchanted)
        {
            switch (result)
            {
                case WeaponTypes.Dagger:
                    result = WeaponTypes.Dagger_Magic;
                    break;
                case WeaponTypes.Staff:
                    result = WeaponTypes.Staff_Magic;
                    break;
                case WeaponTypes.LongBlade:
                    result = WeaponTypes.LongBlade_Magic;
                    break;
                case WeaponTypes.Mace:
                    result = WeaponTypes.Mace_Magic;
                    break;
                case WeaponTypes.Flail:
                    result = WeaponTypes.Flail_Magic;
                    break;
                case WeaponTypes.Warhammer:
                    result = WeaponTypes.Warhammer_Magic;
                    break;
                case WeaponTypes.Battleaxe:
                    result = WeaponTypes.Battleaxe_Magic;
                    break;
                default:
                    break;
            }
        }

        return result;
    }
}
