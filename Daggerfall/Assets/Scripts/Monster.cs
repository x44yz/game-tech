using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

/// <summary>
/// Entity monster careers.
/// </summary>
public enum MonsterCareers
{
    None = -1,
    Rat = 0,
    Imp = 1,
    Spriggan = 2,
    GiantBat = 3,
    GrizzlyBear = 4,
    SabertoothTiger = 5,
    Spider = 6,
    Orc = 7,
    Centaur = 8,
    Werewolf = 9,
    Nymph = 10,
    Slaughterfish = 11,
    OrcSergeant = 12,
    Harpy = 13,
    Wereboar = 14,
    SkeletalWarrior = 15,
    Giant = 16,
    Zombie = 17,
    Ghost = 18,
    Mummy = 19,
    GiantScorpion = 20,
    OrcShaman = 21,
    Gargoyle = 22,
    Wraith = 23,
    OrcWarlord = 24,
    FrostDaedra = 25,
    FireDaedra = 26,
    Daedroth = 27,
    Vampire = 28,
    DaedraSeducer = 29,
    VampireAncient = 30,
    DaedraLord = 31,
    Lich = 32,
    AncientLich = 33,
    Dragonling = 34,
    FireAtronach = 35,
    IronAtronach = 36,
    FleshAtronach = 37,
    IceAtronach = 38,
    Horse_Invalid = 39,             // Not used and no matching texture (294 missing). Crashes DF when spawned in-game.
    Dragonling_Alternate = 40,      // Another dragonling. Seems to work fine when spawned in-game.
    Dreugh = 41,
    Lamia = 42,
}

/// <summary>
/// Mobile affinity for resists/weaknesses, grouping, etc.
/// This could be extended into a set of flags for multi-affinity creatures.
/// </summary>
public enum MobileAffinity
{
    None,               // No special affinity
    Daylight,           // Daylight creatures (centaur, giant, nymph, spriggan, harpy, dragonling)
    Darkness,           // Darkness creatures (imp, gargoyle, orc, vampires, werecreatures)
    Undead,             // Undead monsters (skeleton, liches, zombie, mummy, ghosts)
    Animal,             // Animals (bat, rat, bear, tiger, spider, scorpion)
    Daedra,             // Daedra (daedroth, fire, frost, lord, seducer)
    Golem,              // Golems (flesh, fire, frost, iron)
    Water,              // Water creatures (dreugh, slaughterfish, lamia)
    Human,              // A human creature
}

// [Serializable]
public class Monster : Actor
{
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
        //         maxHealth = FormulaHelper.RollEnemyClassMaxHealth(level, career.HitPointsPerLevel);
        //     }
        // }
        // else 
        if (entityType == EntityTypes.EnemyMonster)
        {
            careerIndex = mobileEnemy.ID;
            career = GetMonsterCareerTemplate((MonsterCareers)careerIndex);
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

        for (int i = 0; i <= DaggerfallSkills.Count; i++)
        {
            skills.SetPermanentSkillValue(i, skillsLevel);
        }

        // Generate loot table items
        DaggerfallLoot.GenerateItems(mobileEnemy.LootTableKey, items);

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
        DaggerfallLoot.RandomlyAddMap(mobileEnemy.MapChance, items);

        if (!string.IsNullOrEmpty(mobileEnemy.LootTableKey))
        {
            // Chance of adding potion
            DaggerfallLoot.RandomlyAddPotion(3, items);
            // Chance of adding potion recipe
            DaggerfallLoot.RandomlyAddPotionRecipe(2, items);
        }

        OnLootSpawned?.Invoke(this, new EnemyLootSpawnedEventArgs { MobileEnemy = mobileEnemy, EnemyCareer = career, Items = items });

        FillVitalSigns();
    }


    /// <summary>
    /// Gets monster career template.
    /// Currently read from MONSTER.BSA. Would like to migrate this to a custom JSON format later.
    /// </summary>
    /// <param name="career"></param>
    /// <returns></returns>
    public static DFCareer GetMonsterCareerTemplate(MonsterCareers career)
    {
        MonsterFile monsterFile = new MonsterFile();
        if (!monsterFile.Load(Path.Combine(DaggerfallUnity.Instance.Arena2Path, MonsterFile.Filename), FileUsage.UseMemory, true))
            throw new Exception("Could not load " + MonsterFile.Filename);

        return monsterFile.GetMonsterClass((int)career);
    }

}