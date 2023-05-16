using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public uint timeHealthLeechLastUsed;
    public uint timeEffectsLastRerolled;
    
    const ushort identifiedMask = 0x20;
    const ushort artifactMask = 0x800;

    // Potion recipe
    int potionRecipeKey;

    // Time for magically-created item to disappear
    // uint timeForItemToDisappear = 0;

    /// <summary>
    /// Gets/sets the key of the potion recipe allocated to this item.
    /// Has a side effect (ugh, sorry) of populating the item value from the recipe price.
    /// (due to value not being encapsulated) Also populates texture record for potions.
    /// </summary>
    public int PotionRecipeKey
    {
        get { return potionRecipeKey; }
        set {
            PotionRecipe potionRecipe = Effects.GetPotionRecipe(value);
            if (potionRecipe != null)
            {
                potionRecipeKey = value;
                this.value = potionRecipe.Price;
                // if (IsPotion)
                //     worldTextureRecord = potionRecipe.TextureRecord;
            }
        }
    }

    /// <summary>
    /// Gets/sets the time for this item to disappear.
    /// </summary>
    public uint TimeForItemToDisappear
    {
        get { return timeForItemToDisappear; }
        set { timeForItemToDisappear = value; }
    }

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
    /// Checks if this item is a potion.
    /// </summary>
    public bool IsPotion
    {
        get { return ItemGroup == ItemGroups.UselessItems1 && TemplateIndex == (int)UselessItems1.Glass_Bottle; }
    }

    /// <summary>
    /// Checks if this is an ingredient.
    /// </summary>
    public bool IsIngredient
    {
        get { return ItemTemplate.isIngredient; }
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
            if (removeFromCollectionWhenBreaks != null /*&& !DaggerfallUnity.Settings.AllowMagicRepairs*/)
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

    /// <summary>
    /// Creates a new copy of this item.
    /// </summary>
    /// <returns>Cloned item.</returns>
    public Item Clone()
    {
        return new Item(this);
    }


    /// <summary>
    /// Gets current variant of this item.
    /// </summary>
    public virtual int CurrentVariant
    {
        get { return currentVariant; }
        set { SetCurrentVariant(value); }
    }

    /// <summary>
    /// Sets current variant and clamps within valid range.
    /// </summary>
    void SetCurrentVariant(int variant)
    {
        if (variant < 0)
            currentVariant = 0;
        else if (variant >= TotalVariants)
            currentVariant = TotalVariants - 1;
        else
            currentVariant = variant;
    }

    /// <summary>
    /// Gets total variants of this item.
    /// </summary>
    public int TotalVariants
    {
        get { return ItemTemplate.variants; }
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
            ActorEffect ownerEffectManager = owner.GetComponent<ActorEffect>();
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

    /// <summary>
    /// Checks if this item is of any shield type.
    /// </summary>
    public bool IsShield
    {
        get { return GetIsShield(); }
    }

    // Check if this is a shield
    bool GetIsShield()
    {
        if (ItemGroup == ItemGroups.Armor)
        {
            if (TemplateIndex == (int)Armor.Kite_Shield ||
                TemplateIndex == (int)Armor.Round_Shield ||
                TemplateIndex == (int)Armor.Tower_Shield ||
                TemplateIndex == (int)Armor.Buckler)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Get the body part that matches to an equip slot.
    /// Used in armor calculations.
    /// </summary>
    public static BodyParts GetBodyPartForEquipSlot(EquipSlots equipSlot)
    {
        switch (equipSlot)
        {
            case EquipSlots.Head:
                return BodyParts.Head;
            case EquipSlots.RightArm:
                return BodyParts.RightArm;
            case EquipSlots.LeftArm:
                return BodyParts.LeftArm;
            case EquipSlots.ChestArmor:
                return BodyParts.Chest;
            case EquipSlots.Gloves:
                return BodyParts.Hands;
            case EquipSlots.LegsArmor:
                return BodyParts.Legs;
            case EquipSlots.Feet:
                return BodyParts.Feet;

            default:
                return BodyParts.None;
        }
    }

    public virtual int GetMaterialArmorValue()
    {
        int result = 0;
        if (!IsShield)
        {
            switch (nativeMaterialValue)
            {
                case (int)ArmorMaterialTypes.Leather:
                    result = 3;
                    break;
                case (int)ArmorMaterialTypes.Chain:
                case (int)ArmorMaterialTypes.Chain2:
                    result = 6;
                    break;
                case (int)ArmorMaterialTypes.Iron:
                    result = 7;
                    break;
                case (int)ArmorMaterialTypes.Steel:
                case (int)ArmorMaterialTypes.Silver:
                    result = 9;
                    break;
                case (int)ArmorMaterialTypes.Elven:
                    result = 11;
                    break;
                case (int)ArmorMaterialTypes.Dwarven:
                    result = 13;
                    break;
                case (int)ArmorMaterialTypes.Mithril:
                case (int)ArmorMaterialTypes.Adamantium:
                    result = 15;
                    break;
                case (int)ArmorMaterialTypes.Ebony:
                    result = 17;
                    break;
                case (int)ArmorMaterialTypes.Orcish:
                    result = 19;
                    break;
                case (int)ArmorMaterialTypes.Daedric:
                    result = 21;
                    break;
            }
        }
        else
        {
            return GetShieldArmorValue();
        }

        // Armor artifact appear to use armor rating divided by 2 rounded down
        if (IsArtifact && ItemGroup == ItemGroups.Armor)
            result /= 2;

        return result;
    }

    /// <summary>
    /// Checks if this item is an artifact.
    /// </summary>
    public bool IsArtifact
    {
        get { return (flags & artifactMask) > 0; }
    }

    public virtual int GetShieldArmorValue()
    {
        switch (TemplateIndex)
        {
            case (int)Armor.Buckler:
                return 1;
            case (int)Armor.Round_Shield:
                return 2;
            case (int)Armor.Kite_Shield:
                return 3;
            case (int)Armor.Tower_Shield:
                return 4;

            default:
                return 0;
        }
    }

    /// <summary>
    /// Get body parts protected by a shield.
    /// </summary>
    public virtual BodyParts[] GetShieldProtectedBodyParts()
    {
        switch (TemplateIndex)
        {
            case (int)Armor.Buckler:
                return new BodyParts[] { BodyParts.LeftArm, BodyParts.Hands };
            case (int)Armor.Round_Shield:
            case (int)Armor.Kite_Shield:
                return new BodyParts[] { BodyParts.LeftArm, BodyParts.Hands, BodyParts.Legs };
            case (int)Armor.Tower_Shield:
                return new BodyParts[] { BodyParts.Head, BodyParts.LeftArm, BodyParts.Hands, BodyParts.Legs };

            default:
                return new BodyParts[] { };
        }
    }
}
