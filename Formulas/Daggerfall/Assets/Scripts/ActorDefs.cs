using System;

/// <summary>
/// Entity genders.
/// </summary>
public enum Genders
{
    Male,
    Female,
}

/// <summary>
/// Varying visibility type for entities.
/// </summary>
[Flags]
public enum MagicalConcealmentFlags
{
    None = 0,
    InvisibleNormal = 1,
    InvisibleTrue = 2,
    BlendingNormal = 4,
    BlendingTrue = 8,
    ShadeNormal = 16,
    ShadeTrue = 32,
}

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