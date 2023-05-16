using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

public class ItemTemplate : ICSVParser
{
    public int index;                           // Index of this item in list
    public string name;                         // Display name
    public float baseWeight;                    // Base weight in kilograms before material, etc.
    public int hitPoints;                       // Hit points
    public int capacityOrTarget;                // Capacity of container or target of effect
    public int basePrice;                       // Base price before material, mercantile, etc. modify value
    public int enchantmentPoints;               // Base enchantment points before material
    public byte rarity;                         // Rarity of item appearing in buildings. Building quality must be at least equal this for item to appear.
    public byte variants;                       // Number of variants for wearable items, unknown for non-wearable items
    public byte drawOrderOrEffect;              // Ordering of items on paper doll (sort lowest to highest) or effect for ingredients
    public bool isBluntWeapon;                  // True for blunt weapons
    public bool isLiquid;                       // True for liquids
    public bool isOneHanded;                    // True for one-handed item/weapons
    public bool isIngredient;                   // True for ingedient items
    public int worldTextureArchive;             // World texture archive index
    public int worldTextureRecord;              // World texture record index
    public int playerTextureArchive;            // Player inventory texture archive index
    public int playerTextureRecord;             // Player inventory texture record index

    // DFU extension fields
    public bool isNotRepairable;                // Defaults to false if not specified

    public void ParseCSV(CSVLoader loader)
    {
        index = loader.ReadInt("index");
        name = loader.ReadString("name");
        baseWeight = loader.ReadFloat("baseWeight");
        hitPoints = loader.ReadInt("hitPoints");
        capacityOrTarget = loader.ReadInt("capacityOrTarget");
        basePrice = loader.ReadInt("basePrice");
        enchantmentPoints = loader.ReadInt("enchantmentPoints");
        rarity = loader.ReadByte("rarity");
        variants = loader.ReadByte("variants");
        drawOrderOrEffect = loader.ReadByte("drawOrderOrEffect");
        isBluntWeapon = loader.ReadBool("isBluntWeapon");
        isLiquid = loader.ReadBool("isLiquid");
        isOneHanded = loader.ReadBool("isOneHanded");
        isIngredient = loader.ReadBool("isIngredient");
        worldTextureArchive = loader.ReadInt("worldTextureArchive");
        worldTextureRecord = loader.ReadInt("worldTextureRecord");
        playerTextureArchive = loader.ReadInt("playerTextureArchive");
        playerTextureRecord = loader.ReadInt("playerTextureRecord");
        isNotRepairable  = loader.ReadBool("isNotRepairable");
    }
}

public class DEnchantment : ICSVStrParser
{
    public EnchantmentTypes type;
    public short param;      

    public void ParseStr(string[] values)
    {
        type = Utils.ToEnum<EnchantmentTypes>(values[0]);
        param = (short)Utils.ToInt(values[1]);
    }
}

public class MagicItemTemplate : ICSVParser
{
    public long index;                          // Index of this item in lit
    public string name;                         // Display name
    public MagicItemTypes type;                 // Type of magic item
    public byte group;                          // Group in item templates
    public byte groupIndex;                     // Group index (subgroup) in item templates
    public DEnchantment[] enchantments;// Array of legacy enchantments on this item
    public int uses;                          // Number of uses/Item condition
    public int value;                           // Only used for artifacts
    public byte material;                       // Material

    public void ParseCSV(CSVLoader loader)
    {
        index = loader.ReadInt("index");
        name = loader.ReadString("name");
        type = loader.ReadEnum<MagicItemTypes>("type");
        group = loader.ReadByte("group");
        groupIndex = loader.ReadByte("groupIndex");
        uses = loader.ReadInt("uses");
        value = loader.ReadInt("value");
        material = loader.ReadByte("material");
        enchantments = new DEnchantment[4];
        for (int i = 0; i < enchantments.Length; ++i)
        {
            var k = loader.ReadString($"enchantment{i}");
            if (string.IsNullOrEmpty(k)) break;
            var e = new DEnchantment();
            e.ParseStr(k.Split(';'));
            enchantments[i] = e;
        }
    }
}

public static class Items
{
    public static List<ItemTemplate> itemTemplates;
    public static List<MagicItemTemplate> allMagicItemTemplates;
    public static List<MagicItemTemplate> artifactItemTemplates;

    public static void Init()
    {
        itemTemplates = CSVLoader.LoadCSV<ItemTemplate>("Assets/Configs/items.csv");
        allMagicItemTemplates = CSVLoader.LoadCSV<MagicItemTemplate>("Assets/Configs/magicItems.csv");
        artifactItemTemplates = new List<MagicItemTemplate>();
        for (int i = 0; i < allMagicItemTemplates.Count; i++)
        {
            if (allMagicItemTemplates[i].type == MagicItemTypes.ArtifactClass1 ||
                allMagicItemTemplates[i].type == MagicItemTypes.ArtifactClass2)
            {
                artifactItemTemplates.Add(allMagicItemTemplates[i]);
            }
        }
    }

    /// <summary>
    /// Helps bridge classic item index pair back to item template index.
    /// </summary>
    /// <param name="group">Group enum to retrieve.</param>
    /// <return>Array of group enum values.</return>
    public static Array GetEnumArray(ItemGroups group)
    {
        switch (group)
        {
            case ItemGroups.Drugs:
                return Enum.GetValues(typeof(Drugs));
            case ItemGroups.UselessItems1:
                return Enum.GetValues(typeof(UselessItems1));
            case ItemGroups.Armor:
                return Enum.GetValues(typeof(Armor));
            case ItemGroups.Weapons:
                return Enum.GetValues(typeof(Weapons));
            case ItemGroups.MagicItems:
                return Enum.GetValues(typeof(MagicItemSubTypes));
            case ItemGroups.Artifacts:
                return Enum.GetValues(typeof(ArtifactsSubTypes));
            case ItemGroups.MensClothing:
                return Enum.GetValues(typeof(MensClothing));
            case ItemGroups.Books:
                return Enum.GetValues(typeof(Books));
            case ItemGroups.Furniture:
                return Enum.GetValues(typeof(Furniture));
            case ItemGroups.UselessItems2:
                return Enum.GetValues(typeof(UselessItems2));
            case ItemGroups.ReligiousItems:
                return Enum.GetValues(typeof(ReligiousItems));
            case ItemGroups.Maps:
                return Enum.GetValues(typeof(Maps));
            case ItemGroups.WomensClothing:
                return Enum.GetValues(typeof(WomensClothing));
            case ItemGroups.Paintings:
                return Enum.GetValues(typeof(Paintings));
            case ItemGroups.Gems:
                return Enum.GetValues(typeof(Gems));
            case ItemGroups.PlantIngredients1:
                return Enum.GetValues(typeof(PlantIngredients1));
            case ItemGroups.PlantIngredients2:
                return Enum.GetValues(typeof(PlantIngredients2));
            case ItemGroups.CreatureIngredients1:
                return Enum.GetValues(typeof(CreatureIngredients1));
            case ItemGroups.CreatureIngredients2:
                return Enum.GetValues(typeof(CreatureIngredients2));
            case ItemGroups.CreatureIngredients3:
                return Enum.GetValues(typeof(CreatureIngredients3));
            case ItemGroups.MiscellaneousIngredients1:
                return Enum.GetValues(typeof(MiscellaneousIngredients1));
            case ItemGroups.MetalIngredients:
                return Enum.GetValues(typeof(MetalIngredients));
            case ItemGroups.MiscellaneousIngredients2:
                return Enum.GetValues(typeof(MiscellaneousIngredients2));
            case ItemGroups.Transportation:
                return Enum.GetValues(typeof(Transportation));
            case ItemGroups.Deeds:
                return Enum.GetValues(typeof(Deeds));
            case ItemGroups.Jewellery:
                return Enum.GetValues(typeof(Jewellery));
            case ItemGroups.QuestItems:
                return Enum.GetValues(typeof(QuestItems));
            case ItemGroups.MiscItems:
                return Enum.GetValues(typeof(MiscItems));
            case ItemGroups.Currency:
                return Enum.GetValues(typeof(Currency));
            default:
                throw new Exception("Error: Item group not found.");
        }
    }


    /// <summary>
    /// Gets item group index from group and template index.
    /// </summary>
    /// <returns>Item group index, or -1 if not found.</returns>
    public static int GetGroupIndex(ItemGroups itemGroup, int templateIndex)
    {
        // Items added by mods are after last DF template, and groupIndex == templateIndex
        // if (templateIndex > LastDFTemplate)
        //     return templateIndex;

        Array values = GetEnumArray(itemGroup);
        for (int i = 0; i < values.Length; i++)
        {
            int checkTemplateIndex = Convert.ToInt32(values.GetValue(i));
            if (checkTemplateIndex == templateIndex)
                return i;
        }

        return -1;
    }


    static Dictionary<ItemGroups, List<int>> customItemGroups = new Dictionary<ItemGroups, List<int>>();
    public static int[] GetCustomItemsForGroup(ItemGroups itemGroup)
    {
        if (customItemGroups.ContainsKey(itemGroup))
            return customItemGroups[itemGroup].ToArray();
        return new int[0];
    }

    /// <summary>
    /// Gets item template data using group and index.
    /// </summary>
    public static ItemTemplate GetItemTemplate(ItemGroups itemGroup, int groupIndex)
    {
        Array values = GetEnumArray(itemGroup);
        if (groupIndex < 0 || groupIndex >= values.Length)
        {
            string message = string.Format("Item index out of range: Group={0} Index={1}", itemGroup.ToString(), groupIndex);
            Debug.Log(message);
            return new ItemTemplate();
        }

        int templateIndex = Convert.ToInt32(values.GetValue(groupIndex));

        return itemTemplates[templateIndex];
    }

    /// <summary>
    /// Gets item template from direct template index.
    /// </summary>
    public static ItemTemplate GetItemTemplate(int templateIndex)
    {
        if (templateIndex < 0 || templateIndex >= itemTemplates.Count)
        {
            string message = string.Format("Item template index out of range: TemplateIndex={0}", templateIndex);
            Debug.Log(message);
            return new ItemTemplate();
        }

        return itemTemplates[templateIndex];
    }

    /// <summary>
    /// Gets artifact template from magic item template data.
    /// </summary>
    public static MagicItemTemplate GetArtifactTemplate(int artifactIndex)
    {
        if (artifactIndex < 0 || artifactIndex >= artifactItemTemplates.Count)
        {
            string message = string.Format("Artifact template index out of range: ArtifactIndex={0}", artifactIndex);
            Debug.Log(message);
            return new MagicItemTemplate();
        }

        return artifactItemTemplates[artifactIndex];
    }

    // /// <summary>
    // /// Gets item group index from group and template index.
    // /// </summary>
    // /// <returns>Item group index, or -1 if not found.</returns>
    // public static int GetGroupIndex(ItemGroups itemGroup, int templateIndex)
    // {
    //     Array values = GetEnumArray(itemGroup);
    //     for (int i = 0; i < values.Length; i++)
    //     {
    //         int checkTemplateIndex = Convert.ToInt32(values.GetValue(i));
    //         if (checkTemplateIndex == templateIndex)
    //             return i;
    //     }

    //     return -1;
    // }
}
