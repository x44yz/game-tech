using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

public enum MagicItemTypes
{
    RegularMagicItem,
    ArtifactClass1,
    ArtifactClass2,
}

/// <summary>
/// Enchantment types
/// </summary>
public enum EnchantmentTypes
{
    None = -1,
    CastWhenUsed = 0,
    CastWhenHeld = 1,
    CastWhenStrikes = 2,
    ExtraSpellPts = 3,
    PotentVs = 4,
    RegensHealth = 5,
    VampiricEffect = 6,
    IncreasedWeightAllowance = 7,
    RepairsObjects = 8,
    AbsorbsSpells = 9,
    EnhancesSkill = 10,
    FeatherWeight = 11,
    StrengthensArmor = 12,
    ImprovesTalents = 13,
    GoodRepWith = 14,
    SoulBound = 15,
    ItemDeteriorates = 16,
    UserTakesDamage = 17,
    VisionProblems = 18,
    WalkingProblems = 19,
    LowDamageVs = 20,
    HealthLeech = 21,
    BadReactionsFrom = 22,
    ExtraWeight = 23,
    WeakensArmor = 24,
    BadRepWith = 25,
    SpecialArtifactEffect = 26,
}

public enum Drugs //checked
    {
        Indulcet = 78,
        Sursum = 79,
        Quaesto_Vil = 80,
        Aegrotat = 81,
    }

    public enum UselessItems1 //checked
    {
        Glass_Jar = 82,
        Glass_Bottle = 83,
        Decanter = 84,
        Clay_Jar = 85,
        Small_Sack = 86,
        Large_Sack = 87,
        Quiver = 88,
        Backpack = 89,
        Small_Chest = 90,
        Wine_Rack = 91,
        Large_Chest = 92,
    }

    public enum Armor   //checked
    {
        Cuirass = 102,
        Gauntlets = 103,
        Greaves = 104,
        Left_Pauldron = 105,
        Right_Pauldron = 106,
        Helm = 107,
        Boots = 108,
        Buckler = 109,
        Round_Shield = 110,
        Kite_Shield = 111,
        Tower_Shield = 112,
    }

    public enum Weapons  //checked
    {
        Dagger = 113,
        Tanto = 114,
        Staff = 115,
        Shortsword = 116,
        Wakazashi = 117,
        Broadsword = 118,
        Saber = 119,
        Longsword = 120,
        Katana = 121,
        Claymore = 122,
        Dai_Katana = 123,
        Mace = 124,
        Flail = 125,
        Warhammer = 126,
        Battle_Axe = 127,
        War_Axe = 128,
        Short_Bow = 129,
        Long_Bow = 130,
        Arrow = 131,
    }

    public enum MagicItemSubTypes                   // Not mapped to a specific item template index
    {
        MagicItem,
    }

    public enum ArtifactsSubTypes                   // Mapped to artifact definitions in MAGIC.DEF
    {
        None = -1,
        Masque_of_Clavicus = 0,
        Mehrunes_Razor = 1,
        Mace_of_Molag_Bal = 2,
        Hircine_Ring = 3,
        Sanguine_Rose = 4,
        Oghma_Infinium = 5,
        Wabbajack = 6,
        Ring_of_Namira = 7,
        Skull_of_Corruption = 8,
        Azuras_Star = 9,
        Volendrung = 10,
        Warlocks_Ring = 11,
        Auriels_Bow = 12,
        Necromancers_Amulet = 13,
        Chrysamere = 14,
        Lords_Mail = 15,
        Staff_of_Magnus = 16,
        Ring_of_Khajiit = 17,
        Ebony_Mail = 18,
        Auriels_Shield = 19,
        Spell_Breaker = 20,
        Skeletons_Key = 21,
        Ebony_Blade = 22,
    }

    public enum MensClothing  //check
    {
        Straps = 141,
        Armbands = 142,
        Kimono = 143,
        Fancy_Armbands = 144,
        Sash = 145,
        Eodoric = 146,
        Shoes = 147,
        Tall_Boots = 148,
        Boots = 149,
        Sandals = 150,
        Casual_pants = 151,
        Breeches = 152,
        Short_skirt = 153,
        Casual_cloak = 154,
        Formal_cloak = 155,
        Khajiit_suit = 156,
        Dwynnen_surcoat = 157,
        Short_tunic = 158,
        Formal_tunic = 159,
        Toga = 160,
        Reversible_tunic = 161,
        Loincloth = 162,
        Plain_robes = 163,
        Priest_robes = 164,
        Short_shirt = 165,
        Short_shirt_with_belt = 166,
        Long_shirt = 167,
        Long_shirt_with_belt = 168,
        Short_shirt_closed_top = 169,
        Short_shirt_closed_top2 = 170,
        Long_shirt_closed_top = 171,
        Long_shirt_closed_top2 = 172,
        Open_Tunic = 173,
        Wrap = 174,
        Long_Skirt = 175,
        Anticlere_Surcoat = 176,
        Challenger_Straps = 177,
        Short_shirt_unchangeable = 178,
        Long_shirt_unchangeable = 179,
        Vest = 180,
        Champion_straps = 181,
    }

    public enum Books
    {
        Book0 = 277,
        Book1 = 277,
        Book2 = 277,
        Book3 = 277,
    }

    public enum Furniture
    {
        Plain_single_bed = 217,
        Fancy_single_bed = 218,
        Plain_double_bed = 219,
        Fancy_double_bed = 220,
        Large_oak_table = 221,
        Large_cherry_table = 222,
        Large_mahogany_table = 223,
        Large_teak_table = 224,
        Small_oak_table = 225,
        Small_cherry_table = 226,
        Small_mahogany_table = 227,
        Small_teak_table = 228,
        Oak_chair = 229,
        Cherry_chair = 230,
        Mahogany_chair = 231,
        Teak_chair = 232,
        Curtains = 233,
        Fancy_curtains = 234,
        Large_pillow = 235,
        Small_pillow = 236,
        Small_plain_rug = 237,
        Large_plain_rug = 238,
        Small_fine_rug = 239,
        Large_fine_rug = 240,
        Large_tapestry = 241,
        Medium_tapestry = 242,
        Small_tapestry = 243,
        Large_skins = 244,
        Small_skins = 245,
    }

    public enum UselessItems2 //checked
    {
        Torch = 247,
        Lantern = 248,
        Bandage = 249,
        Oil = 252,
        Candle = 253,
        Parchment = 279,
    }

    public enum ReligiousItems  //checked
    {
        Prayer_beads = 258,
        Rare_symbol = 259,
        Common_symbol = 260,
        Bell = 261,
        Holy_water = 262,
        Talisman = 263,
        Religious_item = 264,
        Small_statue = 265,
        Icon = 267,
        Scarab = 268,
        Holy_candle = 269,
        Holy_dagger = 270,
        Holy_tome = 271,
    }

  public enum Maps //checked
    {
        Map = 287,
    }

    public enum WomensClothing  //checked
    {
        Brassier = 182,
        Formal_brassier = 183,
        Peasant_blouse = 184,
        Eodoric = 185,
        Shoes = 186,
        Tall_boots = 187,
        Boots = 188,
        Sandals = 189,
        Casual_pants = 190,
        Casual_cloak = 191,
        Formal_cloak = 192,
        Khajiit_suit = 193,
        Formal_eodoric = 194,
        Evening_gown = 195,
        Day_gown = 196,
        Casual_dress = 197,
        Strapless_dress = 198,
        Loincloth = 199,
        Plain_robes = 200,
        Priestess_robes = 201,
        Short_shirt = 202,
        Short_shirt_belt = 203,
        Long_shirt = 204,
        Long_shirt_belt = 205,
        Short_shirt_closed = 206,
        Short_shirt_closed_belt = 207,
        Long_shirt_closed = 208,
        Long_shirt_closed_belt = 209,
        Open_tunic = 210,
        Wrap = 211,
        Long_skirt = 212,
        Tights = 213,
        Short_shirt_unchangeable = 214,
        Long_shirt_unchangeable = 215,
        Vest = 216,
    }

    public enum Paintings                           // DEFEDIT sets subgroup to 255? ... correct group # though
    {
        Painting = 284,
    }

    public enum Gems  //checked
    {
        Ruby = 0,
        Emerald = 1,
        Sapphire = 2,
        Diamond = 3,
        Jade = 4,
        Turquoise = 5,
        Malachite = 6,
        Amber = 7,
    }

    public enum PlantIngredients1 //checked
    {
        Twigs = 8,
        Green_leaves = 9,
        Red_flowers = 10,
        Yellow_flowers = 11,
        Root_tendrils = 12,
        Root_bulb = 13,
        Pine_branch = 14,
        Green_berries = 15,
        Red_berries = 16,
        Yellow_berries = 17,
        Clover = 18,
        Red_rose = 19,
        Yellow_rose = 20,
        Red_poppy = 23,
        Golden_poppy = 25,
    }

    public enum PlantIngredients2  //checked
    {
        Twigs = 8,
        Green_leaves = 9,
        Red_flowers = 10,
        Yellow_flowers = 11,
        Root_tendrils = 12,
        Root_bulb = 13,
        Green_berries = 15,
        Red_berries = 16,
        Yellow_berries = 17,
        Black_rose = 21,
        White_rose = 22,
        Black_poppy = 24,
        White_poppy = 26,
        Ginkgo_leaves = 27,
        Bamboo = 28,
        Palm = 29,
        Aloe = 30,
        Fig = 31,
        Cactus = 32,
    }

    public enum CreatureIngredients1  //checked
    {
        Werewolfs_blood = 33,
        Fairy_dragon_scales = 35,
        Wraith_essence = 38,
        Ectoplasm = 39,
        Ghouls_tongue = 40,
        Spider_venom = 41,
        Troll_blood = 42,
        Snake_venom = 43,
        Gorgon_snake = 44,
        Lich_dust = 45,
        Giant_blood = 50,
        Basilisk_eye = 51,
        Daedra_heart = 53,
        Saints_hair = 54,
        Orcs_blood = 61,
    }

    public enum CreatureIngredients2  //checked
    {
        Dragons_scales = 46,
        Giant_scorpion_stinger = 47,
        Small_scorpion_stinger = 48,
        Mummy_wrappings = 49,
        Gryphon_Feather = 52,
    }

    public enum CreatureIngredients3 //checked
    {
        Wereboar_tusk = 34,
        Nymph_hair = 36,
        Unicorn_horn = 37,
    }

    public enum MiscellaneousIngredients1  //checked
    {
        Holy_relic = 55,
        Big_tooth = 56,
        Medium_tooth = 57,
        Small_tooth = 58,
        Pure_water = 59,
        Rain_water = 60,
        Elixir_vitae = 62,
        Nectar = 63,
        Ichor = 64,
    }

    public enum MetalIngredients  //Checked
    {
        Mercury = 65,
        Tin = 66,
        Brass = 67,
        Lodestone = 68,
        Sulphur = 69,
        Lead = 70,
        Iron = 71,
        Copper = 72,
        Silver = 73,
        Gold = 74,
        Platinum = 75,
    }

    public enum MiscellaneousIngredients2 //checked
    {
        Ivory = 76,
        Pearl = 77,
    }

    public enum Transportation  //Checked
    {
        Small_cart = 93,
        Horse = 94,
        Rowboat = 95,
        Large_boat = 96,
        Small_ship = 97,
        Large_Galley = 98,
    }

    public enum Deeds  //checked
    {
        Deed_to_townhouse,
        Deed_to_house,
        Deed_to_manor,
    }

    public enum Jewellery  //checked
    {
        Amulet = 133,
        Bracer = 134,
        Ring = 135,
        Bracelet = 136,
        Mark = 137,
        Torc = 138,
        Cloth_amulet = 139,
        Wand = 140,
    }

    public enum QuestItems  //checked
    {
        Telescope = 254,
        Scales = 255,
        Globe = 256,
        Skeleton = 257,
        Totem = 280,
        Dead_body = 281,
        Mantella = 282,
        Finger = 283,
    }

    public enum MiscItems  //checked
    {
        Spellbook = 132,
        Soul_trap = 274,
        Letter_of_credit = 275,
        Unused,
        Potion_recipe = 278,
        Dead_Body = 281,
        House_Deed = 285,
        Ship_Deed = 286,
        Map = 287,
    }

    public enum Currency  //checked
    {
        Gold_pieces = 276,
    }

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
    public int param;      

    public void ParseStr(string[] values)
    {
        type = Utils.ToEnum<EnchantmentTypes>(values[0]);
        param = Utils.ToInt(values[1]);
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

    /// <summary>
    /// Gets item group index from group and template index.
    /// </summary>
    /// <returns>Item group index, or -1 if not found.</returns>
    public static int GetGroupIndex(ItemGroups itemGroup, int templateIndex)
    {
        Array values = GetEnumArray(itemGroup);
        for (int i = 0; i < values.Length; i++)
        {
            int checkTemplateIndex = Convert.ToInt32(values.GetValue(i));
            if (checkTemplateIndex == templateIndex)
                return i;
        }

        return -1;
    }
}
