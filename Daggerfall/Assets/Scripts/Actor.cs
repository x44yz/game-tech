using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;
using QuickDemo.FSM;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Diseases the player can catch. (msgs 100-116)
/// </summary>
public enum Diseases
{
    None = -1,
    WitchesPox = 0,
    Plague = 1,
    YellowFever = 2,
    StomachRot = 3,
    Consumption = 4,
    BrainFever = 5,
    SwampRot = 6,
    CalironsCurse = 7,
    Cholera = 8,
    Leprosy = 9,
    WoundRot = 10,
    RedDeath = 11,
    BloodRot = 12,
    TyphoidFever = 13,
    Dementia = 14,
    Chrondiasis = 15,
    WizardFever = 16,
}

/// <summary>
/// Skills
/// </summary>
public enum Skills
{
    None = -1,
    Medical = 0,
    Etiquette = 1,
    Streetwise = 2,
    Jumping = 3,
    Orcish = 4,
    Harpy = 5,
    Giantish = 6,
    Dragonish = 7,
    Nymph = 8,
    Daedric = 9,
    Spriggan = 10,
    Centaurian = 11,
    Impish = 12,
    Lockpicking = 13,
    Mercantile = 14,
    Pickpocket = 15,
    Stealth = 16,
    Swimming = 17,
    Climbing = 18,
    Backstabbing = 19,
    Dodging = 20,
    Running = 21,
    Destruction = 22,
    Restoration = 23,
    Illusion = 24,
    Alteration = 25,
    Thaumaturgy = 26,
    Mysticism = 27,
    ShortBlade = 28,
    LongBlade = 29,
    HandToHand = 30,
    Axe = 31,
    BluntWeapon = 32,
    Archery = 33,
    CriticalStrike = 34,
    Count = 35, // Not a valid skill
}

/// <summary>
/// Faction race value does not map to usual race ID but maps
/// to oath selection and is used only for this. Values above 7
/// are not used in-game but are guessed from FACTION.TXT.
/// </summary>
public enum FactionRaces
{
    None = -1,
    Nord = 0,
    Khajiit = 1,
    Redguard = 2,
    Breton = 3,
    Argonian = 4,
    WoodElf = 5,
    HighElf = 6,
    DarkElf = 7,
    Skakmat = 11,       // Only used on #304 "Skakmat"
    Orc = 17,           // Only used on #358 "Orsinium" in the original file
    Vampire = 18,
    Fey = 19,           // Only used on #513 "The Fey" a.k.a. Le Fay :)
}

/// <summary>
/// Primary stats.
/// </summary>
public enum Stats
{
    None = -1,
    Strength = 0,
    Intelligence = 1,
    Willpower = 2,
    Agility = 3,
    Endurance = 4,
    Personality = 5,
    Speed = 6,
    Luck = 7,
}

/// <summary>
/// Entity races.
/// </summary>
public enum Races
{
    None = -1,
    Breton = 1,
    Redguard = 2,
    Nord = 3,
    DarkElf = 4,
    HighElf = 5,
    WoodElf = 6,
    Khajiit = 7,
    Argonian = 8,
    Vampire = 9,
    Werewolf = 10,
    Wereboar = 11,
}

/// <summary>
/// Entity class careers.
/// </summary>
public enum ClassCareers
{
    None = -1,
    Mage = 0,
    Spellsword = 1,
    Battlemage = 2,
    Sorcerer = 3,
    Healer = 4,
    Nightblade = 5,
    Bard = 6,
    Burglar = 7,
    Rogue = 8,
    Acrobat = 9,
    Thief = 10,
    Assassin = 11,
    Monk = 12,
    Archer = 13,
    Ranger = 14,
    Barbarian = 15,
    Warrior = 16,
    Knight = 17,
}

/// <summary>
/// Defines various types of living entities in the world.
/// </summary>
public enum EntityTypes
{
    None,
    Player,
    CivilianNPC,
    StaticNPC,
    EnemyMonster,
    EnemyClass,
}

public class Actor : MonoBehaviour
{
    public int Weight;

    public int MinDamage;                       // Minimum damage per first hit of attack
    public int MaxDamage;                       // Maximum damage per first hit of attack
    public int MinDamage2;                      // Minimum damage per second hit of attack
    public int MaxDamage2;                      // Maximum damage per second hit of attack
    public int MinDamage3;                      // Minimum damage per third hit of attack
    public int MaxDamage3;                      // Maximum damage per third hit of attack

    protected WeaponMaterialTypes minMetalToHit;
    public WeaponMaterialTypes MinMetalToHit { get { return minMetalToHit; } set { minMetalToHit = value; } }
    protected DSkills skills;
    public DSkills Skills { get { return skills; } set { skills.Copy(value); } }
    public int Level;

    public const int NumberBodyParts = 7;
    protected sbyte[] armorValues = new sbyte[NumberBodyParts];
    public sbyte[] ArmorValues { get { return armorValues; } set { armorValues = value; } }

    public int IncreasedArmorValueModifier { get; private set; }
    public int DecreasedArmorValueModifier { get; private set; }
    public bool ImprovedAdrenalineRush { get; set; }
    public int ChanceToHitModifier { get; private set; }

    public int MaxHealth { get { return GetMaxHealth(); } set { maxHealth = value; } }
    public int CurrentHealth { get { return GetCurrentHealth(); } set { SetHealth(value); } }

    protected DStats stats = new DStats();
    public DStats Stats { get { return stats; } set { stats.Copy(value); } }

    protected DFCareer career = new DFCareer();
    public DFCareer Career { get { return career; } set { career = value; } }

    EntityTypes entityType = EntityTypes.None;
    public EntityTypes EntityType
    {
        get { return entityType; }
    }

    int careerIndex = -1;
    public int CareerIndex
    {
        get { return careerIndex; }
    }

    public int DecreaseHealth(int amount)
    {
        throw new System.NotImplementedException();
    }

    public int MaxHealthLimiter { get; private set; }
    protected int maxHealth;
    // Gets maximum health with effect limiter
    int GetMaxHealth()
    {
        // Limiter must be 1 or greater
        if (MaxHealthLimiter < 1)
            return maxHealth;

        return (MaxHealthLimiter < maxHealth) ? MaxHealthLimiter : maxHealth;
    }

    protected int currentHealth;
    int GetCurrentHealth()
    {
        return currentHealth;
    }

    public virtual int SetHealth(int amount, bool restoreMode = false)
    {
        currentHealth = (restoreMode) ? amount : Mathf.Clamp(amount, 0, MaxHealth);
        // if (currentHealth <= 0)
        //     RaiseOnDeathEvent();

        return currentHealth;
    }

    public int CurrentFatigue { get { return GetCurrentFatigue(); } set { SetFatigue(value); } }
    public const int FatigueMultiplier = 64;
    public int MaxFatigue { get { return (stats.LiveStrength + stats.LiveEndurance) * FatigueMultiplier; } }
    protected int currentFatigue;
    public virtual int SetFatigue(int amount, bool restoreMode = false)
    {
        currentFatigue = (restoreMode) ? amount : Mathf.Clamp(amount, 0, MaxFatigue);
        // if (currentFatigue <= 0 && currentHealth > 0)
        //     RaiseOnExhaustedEvent();

        return currentFatigue;
    }

    int GetCurrentFatigue()
    {
        return currentFatigue;
    }

    // public EffectManager effectManager => GetComponent<EffectManager>();

    void Awake()
    {
        skills = new DSkills();
    }
}