using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

// [Serializable]
public class Monster : Actor
{
    public int ID;                              // ID of this mobile

    public int MinDamage;                       // Minimum damage per first hit of attack
    public int MaxDamage;                       // Maximum damage per first hit of attack
    public int MinDamage2;                      // Minimum damage per second hit of attack
    public int MaxDamage2;                      // Maximum damage per second hit of attack
    public int MinDamage3;                      // Minimum damage per third hit of attack
    public int MaxDamage3;                      // Maximum damage per third hit of attack

    public MobileAffinity Affinity;             // Affinity of mobile
    public MobileEnemy mobileEnemy;

    public DFCareer.EnemyGroups GetEnemyGroup()
    {
        return FormulaUtils.GetEnemyEntityEnemyGroup(this);
    }

    public void ApplyEnemySettings(int enemyIndex)
    {
        MobileEnemy mobileEnemy = null;
        for (int i = 0; i < MobileEnemies.Enemies.Length; ++i)
        {
            if (MobileEnemies.Enemies[i].ID == enemyIndex)
            {
                mobileEnemy = MobileEnemies.Enemies[i];
                break;
            }
        }

        if (enemyIndex >= 0 && enemyIndex <= 42)
        {
            entityType = EntityTypes.EnemyMonster;
            SetEnemyCareer(mobileEnemy, EntityType);
        }
        else if (enemyIndex >= 128 && enemyIndex <= 146)
        {
            entityType = EntityTypes.EnemyClass;
            SetEnemyCareer(mobileEnemy, EntityType);
        }
        // else if (DaggerfallEntity.GetCustomCareerTemplate(enemyIndex) != null)
        // {
        //     if (DaggerfallEntity.IsClassEnemyId(enemyIndex))
        //     {
        //         entityBehaviour.EntityType = EntityTypes.EnemyClass;
        //     }
        //     else
        //     {
        //         entityBehaviour.EntityType = EntityTypes.EnemyMonster;
        //     }
        //     entity.SetEnemyCareer(mobileEnemy, entityBehaviour.EntityType);
        // }
        else
        {
            entityType = EntityTypes.None;
        }
    }

 // From FALL.EXE offset 0x1C0F14
    static byte[] ImpSpells            = { 0x07, 0x0A, 0x1D, 0x2C };
    static byte[] GhostSpells          = { 0x22 };
    static byte[] OrcShamanSpells      = { 0x06, 0x07, 0x16, 0x19, 0x1F };
    static byte[] WraithSpells         = { 0x1C, 0x1F };
    static byte[] FrostDaedraSpells    = { 0x10, 0x14 };
    static byte[] FireDaedraSpells     = { 0x0E, 0x19 };
    static byte[] DaedrothSpells       = { 0x16, 0x17, 0x1F };
    static byte[] VampireSpells        = { 0x33 };
    static byte[] SeducerSpells        = { 0x34, 0x43 };
    static byte[] VampireAncientSpells = { 0x08, 0x32 };
    static byte[] DaedraLordSpells     = { 0x08, 0x0A, 0x0E, 0x3C, 0x43 };
    static byte[] LichSpells           = { 0x08, 0x0A, 0x0E, 0x22, 0x3C };
    static byte[] AncientLichSpells    = { 0x08, 0x0A, 0x0E, 0x1D, 0x1F, 0x22, 0x3C };
    static byte[][] EnemyClassSpells   = { FrostDaedraSpells, DaedrothSpells, OrcShamanSpells, VampireAncientSpells, DaedraLordSpells, LichSpells, AncientLichSpells };

    int careerIndex = -1;
    /// <summary>
    /// Sets enemy career and prepares entity settings.
    /// </summary>
    public void SetEnemyCareer(MobileEnemy mobileEnemy, EntityTypes entityType)
    {
        // // Try custom career first
        // career = GetCustomCareerTemplate(mobileEnemy.ID);

        // if (career != null)
        // {
        //     // Custom enemy
        //     careerIndex = mobileEnemy.ID;
        //     stats.SetPermanentFromCareer(career);

        //     if (entityType == EntityTypes.EnemyMonster)
        //     {
        //         // Default like a monster
        //         level = mobileEnemy.Level;
        //         maxHealth = Random.Range(mobileEnemy.MinHealth, mobileEnemy.MaxHealth + 1);
        //         for (int i = 0; i < ArmorValues.Length; i++)
        //         {
        //             ArmorValues[i] = (sbyte)(mobileEnemy.ArmorValue * 5);
        //         }
        //     }
        //     else
        //     {
        //         // Default like a class enemy
        //         level = GameManager.Instance.PlayerEntity.Level;
        //         maxHealth = FormulaUtils.RollEnemyClassMaxHealth(level, career.HitPointsPerLevel);
        //     }
        // }
        // else 
        if (entityType == EntityTypes.EnemyMonster)
        {
            careerIndex = mobileEnemy.ID;
            career = Classes.GetMonsterCareerTemplate((MonsterCareers)careerIndex);
            stats.SetPermanentFromCareer(career);

            // Enemy monster has predefined level, health and armor values.
            // Armor values can be modified below by equipment.
            level = mobileEnemy.Level;
            maxHealth = UnityEngine.Random.Range(mobileEnemy.MinHealth, mobileEnemy.MaxHealth + 1);
            for (int i = 0; i < ArmorValues.Length; i++)
            {
                ArmorValues[i] = (sbyte)(mobileEnemy.ArmorValue * 5);
            }
        }
        else if (entityType == EntityTypes.EnemyClass)
        {
            careerIndex = mobileEnemy.ID - 128;
            career = Classes.GetClassCareerTemplate((ClassCareers)careerIndex);
            stats.SetPermanentFromCareer(career);

            // Enemy class is levelled to player and uses similar health rules
            // City guards are 3 to 6 levels above the player
            level = Main.Inst.heroLevel;
            // if (careerIndex == (int)MobileTypes.Knight_CityWatch - 128)
            //     level += UnityEngine.Random.Range(3, 7);

            maxHealth = FormulaUtils.RollEnemyClassMaxHealth(level, career.HitPointsPerLevel);
        }
        else
        {
            career = new DFCareer();
            careerIndex = -1;
            return;
        }

        this.mobileEnemy = mobileEnemy;
        this.entityType = entityType;
        name = career.Name;
        minMetalToHit = mobileEnemy.MinMetalToHit;
        // team = mobileEnemy.Team;

        short skillsLevel = (short)((level * 5) + 30);
        if (skillsLevel > 100)
        {
            skillsLevel = 100;
        }

        for (int i = 0; i <= DSkills.Count; i++)
        {
            skills.SetPermanentSkillValue(i, skillsLevel);
        }

        // Generate loot table items
        // DaggerfallLoot.GenerateItems(mobileEnemy.LootTableKey, items);

        // Enemy classes and some monsters use equipment
        if (careerIndex == (int)MonsterCareers.Orc || careerIndex == (int)MonsterCareers.OrcShaman)
        {
            SetEnemyEquipment(0);
        }
        else if (careerIndex == (int)MonsterCareers.Centaur || careerIndex == (int)MonsterCareers.OrcSergeant)
        {
            SetEnemyEquipment(1);
        }
        else if (careerIndex == (int)MonsterCareers.OrcWarlord)
        {
            SetEnemyEquipment(2);
        }
        else if (entityType == EntityTypes.EnemyClass)
        {
            SetEnemyEquipment(UnityEngine.Random.Range(0, 2)); // 0 or 1
        }

        // Assign spell lists
        if (entityType == EntityTypes.EnemyMonster)
        {
            if (careerIndex == (int)MonsterCareers.Imp)
                SetEnemySpells(ImpSpells);
            else if (careerIndex == (int)MonsterCareers.Ghost)
                SetEnemySpells(GhostSpells);
            else if (careerIndex == (int)MonsterCareers.OrcShaman)
                SetEnemySpells(OrcShamanSpells);
            else if (careerIndex == (int)MonsterCareers.Wraith)
                SetEnemySpells(WraithSpells);
            else if (careerIndex == (int)MonsterCareers.FrostDaedra)
                SetEnemySpells(FrostDaedraSpells);
            else if (careerIndex == (int)MonsterCareers.FireDaedra)
                SetEnemySpells(FireDaedraSpells);
            else if (careerIndex == (int)MonsterCareers.Daedroth)
                SetEnemySpells(DaedrothSpells);
            else if (careerIndex == (int)MonsterCareers.Vampire)
                SetEnemySpells(VampireSpells);
            else if (careerIndex == (int)MonsterCareers.DaedraSeducer)
                SetEnemySpells(SeducerSpells);
            else if (careerIndex == (int)MonsterCareers.VampireAncient)
                SetEnemySpells(VampireAncientSpells);
            else if (careerIndex == (int)MonsterCareers.DaedraLord)
                SetEnemySpells(DaedraLordSpells);
            else if (careerIndex == (int)MonsterCareers.Lich)
                SetEnemySpells(LichSpells);
            else if (careerIndex == (int)MonsterCareers.AncientLich)
                SetEnemySpells(AncientLichSpells);
        }
        else if (entityType == EntityTypes.EnemyClass && (mobileEnemy.CastsMagic))
        {
            int spellListLevel = level / 3;
            if (spellListLevel > 6)
                spellListLevel = 6;
            SetEnemySpells(EnemyClassSpells[spellListLevel]);
        }

        // Chance of adding map
        // DaggerfallLoot.RandomlyAddMap(mobileEnemy.MapChance, items);

        // if (!string.IsNullOrEmpty(mobileEnemy.LootTableKey))
        // {
        //     // Chance of adding potion
        //     DaggerfallLoot.RandomlyAddPotion(3, items);
        //     // Chance of adding potion recipe
        //     DaggerfallLoot.RandomlyAddPotionRecipe(2, items);
        // }

        // OnLootSpawned?.Invoke(this, new EnemyLootSpawnedEventArgs { MobileEnemy = mobileEnemy, EnemyCareer = career, Items = items });

        FillVitalSigns();
    }

    public void SetEnemySpells(byte[] spellList)
    {
        // Enemies don't follow same rule as player for maximum spell points
        MaxMagicka = 10 * level + 100;
        currentMagicka = MaxMagicka;
        skills.SetPermanentSkillValue(Skills.Destruction, 80);
        skills.SetPermanentSkillValue(Skills.Restoration, 80);
        skills.SetPermanentSkillValue(Skills.Illusion, 80);
        skills.SetPermanentSkillValue(Skills.Alteration, 80);
        skills.SetPermanentSkillValue(Skills.Thaumaturgy, 80);
        skills.SetPermanentSkillValue(Skills.Mysticism, 80);

        // Add spells to enemy from standard list
        foreach (byte spellID in spellList)
        {
            SpellRecordData spellData;
            Spells.GetClassicSpellRecord(spellID, out spellData);
            if (spellData.index == -1)
            {
                Debug.LogError("Failed to locate enemy spell in standard spells list.");
                continue;
            }

            EffectBundleSettings bundle; 
            if (!Effects.ClassicSpellRecordDataToEffectBundleSettings(spellData, BundleTypes.Spell, out bundle))
            {
                Debug.LogError("Failed to create effect bundle for enemy spell: " + spellData.spellName);
                continue;
            }
            AddSpell(bundle);
        }
    }


    /// <summary>
    /// Gets monster career template.
    /// Currently read from MONSTER.BSA. Would like to migrate this to a custom JSON format later.
    /// </summary>
    /// <param name="career"></param>
    /// <returns></returns>
    // public static DFCareer GetMonsterCareerTemplate(MonsterCareers career)
    // {
    //     MonsterFile monsterFile = new MonsterFile();
    //     if (!monsterFile.Load(Path.Combine(DaggerfallUnity.Instance.Arena2Path, MonsterFile.Filename), FileUsage.UseMemory, true))
    //         throw new Exception("Could not load " + MonsterFile.Filename);

    //     return monsterFile.GetMonsterClass((int)career);
    // }

    public void SetEnemyEquipment(int variant)
    {
        // Assign the enemies starting equipment.
        AssignEnemyEquipment(Main.Inst.hero, this, variant);

        // Initialize armor values to 100 (no armor)
        for (int i = 0; i < ArmorValues.Length; i++)
        {
            ArmorValues[i] = 100;
        }
        // Calculate armor values from equipment
        for (int i = (int)EquipSlots.Head; i < (int)EquipSlots.Feet; i++)
        {
            var item = ItemEquipTable.GetItem((EquipSlots)i);
            if (item != null && item.ItemGroup == ItemGroups.Armor)
            {
                UpdateEquippedArmorValues(item, true);
            }
        }

        if (entityType == EntityTypes.EnemyClass)
        {
            // Clamp to maximum armor value of 60. In classic this also applies for monsters.
            // Note: Classic sets the value to 60 if it is > 50, which seems like an oversight.
            for (int i = 0; i < ArmorValues.Length; i++)
            {
                if (ArmorValues[i] > 60)
                {
                    ArmorValues[i] = 60;
                }
            }
        }
        else
        {
            // Note: In classic, the above applies for equipment-using monsters as well as enemy classes.
            // The resulting armor values are often 60. Due to the +40 to hit against monsters this makes
            // monsters with equipment very easy to hit, and 60 is a worse value than any value monsters
            // have in their definition. To avoid this, in DF Unity the equipment values are only used if
            // they are better than the value in the definition.
            for (int i = 0; i < ArmorValues.Length; i++)
            {
                if (ArmorValues[i] > (sbyte)(mobileEnemy.ArmorValue * 5))
                {
                    ArmorValues[i] = (sbyte)(mobileEnemy.ArmorValue * 5);
                }
            }
        }
    }

    // variant 变量
    public void AssignEnemyStartingEquipment(Hero player, Monster enemyEntity, 
        int variant)
    {
        int itemLevel = player.Level;
        Genders playerGender = player.Gender;
        Races race = player.Race;
        int chance = 0;

        // City watch never have items above iron or steel
        // if (enemyEntity.EntityType == EntityTypes.EnemyClass && enemyEntity.MobileEnemy.ID == (int)MobileTypes.Knight_CityWatch)
        //     itemLevel = 1;

        if (variant == 0)
        {
            // right-hand weapon
            int item = UnityEngine.Random.Range((int)Weapons.Broadsword, (int)(Weapons.Longsword) + 1);
            Item weapon = ItemUtils.CreateWeapon((Weapons)item, FormulaUtils.RandomMaterial(itemLevel));
            enemyEntity.ItemEquipTable.EquipItem(weapon, true, false);
            enemyEntity.Items.AddItem(weapon);

            chance = 50;

            // left-hand shield
            item = UnityEngine.Random.Range((int)Armor.Buckler, (int)(Armor.Round_Shield) + 1);
            if (Dice100.SuccessRoll(chance))
            {
                Item armor = ItemUtils.CreateArmor(playerGender, race, (Armor)item, FormulaUtils.RandomArmorMaterial(itemLevel));
                enemyEntity.ItemEquipTable.EquipItem(armor, true, false);
                enemyEntity.Items.AddItem(armor);
            }
            // left-hand weapon
            else if (Dice100.SuccessRoll(chance))
            {
                item = UnityEngine.Random.Range((int)Weapons.Dagger, (int)(Weapons.Shortsword) + 1);
                weapon = ItemUtils.CreateWeapon((Weapons)item, FormulaUtils.RandomMaterial(itemLevel));
                enemyEntity.ItemEquipTable.EquipItem(weapon, true, false);
                enemyEntity.Items.AddItem(weapon);
            }
        }
        else
        {
            // right-hand weapon
            int item = UnityEngine.Random.Range((int)Weapons.Claymore, (int)(Weapons.Battle_Axe) + 1);
            Item weapon = ItemUtils.CreateWeapon((Weapons)item, FormulaUtils.RandomMaterial(itemLevel));
            enemyEntity.ItemEquipTable.EquipItem(weapon, true, false);
            enemyEntity.Items.AddItem(weapon);

            if (variant == 1)
                chance = 75;
            else if (variant == 2)
                chance = 90;
        }
        // helm
        if (Dice100.SuccessRoll(chance))
        {
            Item armor = ItemUtils.CreateArmor(playerGender, race, Armor.Helm, FormulaUtils.RandomArmorMaterial(itemLevel));
            enemyEntity.ItemEquipTable.EquipItem(armor, true, false);
            enemyEntity.Items.AddItem(armor);
        }
        // right pauldron
        if (Dice100.SuccessRoll(chance))
        {
            Item armor = ItemUtils.CreateArmor(playerGender, race, Armor.Right_Pauldron, FormulaUtils.RandomArmorMaterial(itemLevel));
            enemyEntity.ItemEquipTable.EquipItem(armor, true, false);
            enemyEntity.Items.AddItem(armor);
        }
        // left pauldron
        if (Dice100.SuccessRoll(chance))
        {
            Item armor = ItemUtils.CreateArmor(playerGender, race, Armor.Left_Pauldron, FormulaUtils.RandomArmorMaterial(itemLevel));
            enemyEntity.ItemEquipTable.EquipItem(armor, true, false);
            enemyEntity.Items.AddItem(armor);
        }
        // cuirass
        if (Dice100.SuccessRoll(chance))
        {
            Item armor = ItemUtils.CreateArmor(playerGender, race, Armor.Cuirass, FormulaUtils.RandomArmorMaterial(itemLevel));
            enemyEntity.ItemEquipTable.EquipItem(armor, true, false);
            enemyEntity.Items.AddItem(armor);
        }
        // greaves
        if (Dice100.SuccessRoll(chance))
        {
            Item armor = ItemUtils.CreateArmor(playerGender, race, Armor.Greaves, FormulaUtils.RandomArmorMaterial(itemLevel));
            enemyEntity.ItemEquipTable.EquipItem(armor, true, false);
            enemyEntity.Items.AddItem(armor);
        }
        // boots
        if (Dice100.SuccessRoll(chance))
        {
            Item armor = ItemUtils.CreateArmor(playerGender, race, Armor.Boots, FormulaUtils.RandomArmorMaterial(itemLevel));
            enemyEntity.ItemEquipTable.EquipItem(armor, true, false);
            enemyEntity.Items.AddItem(armor);
        }

        // Chance for poisoned weapon
        if (player.Level > 1)
        {
            Item weapon = enemyEntity.ItemEquipTable.GetItem(EquipSlots.RightHand);
            if (weapon != null && (enemyEntity.EntityType == EntityTypes.EnemyClass || enemyEntity.MobileEnemy.ID == (int)MobileTypes.Orc
                    || enemyEntity.ID == (int)MobileTypes.Centaur || enemyEntity.MobileEnemy.ID == (int)MobileTypes.OrcSergeant))
            {
                int chanceToPoison = 5;
                if (enemyEntity.MobileEnemy.ID == (int)MobileTypes.Assassin)
                    chanceToPoison = 60;

                if (Dice100.SuccessRoll(chanceToPoison))
                {
                    // Apply poison
                    weapon.poisonType = (Poisons)UnityEngine.Random.Range(128, 135 + 1);
                }
            }
        }
    }

}