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

// [Serializable]
public class Actor : MonoBehaviour
{
    public int Weight;

    protected WeaponMaterialTypes minMetalToHit;
    public WeaponMaterialTypes MinMetalToHit { get { return minMetalToHit; } set { minMetalToHit = value; } }
    protected DSkills skills;
    public DSkills tSkills { get { return skills; } set { skills.Copy(value); } }
    public int level;
    public int Level { get { return level; } set { level = value; } }

    public const int NumberBodyParts = 7;
    protected sbyte[] armorValues = new sbyte[NumberBodyParts];
    public sbyte[] ArmorValues { get { return armorValues; } set { armorValues = value; } }

    public int IncreasedArmorValueModifier { get; private set; }
    public int DecreasedArmorValueModifier { get; private set; }
    public bool ImprovedAdrenalineRush { get; set; }
    public int ChanceToHitModifier { get; private set; }

    public int MaxHealth { get { return GetMaxHealth(); } set { maxHealth = value; } }
    public int CurrentHealth { get { return GetCurrentHealth(); } set { SetHealth(value); } }

    public DStats stats = new DStats();
    public DStats Stats { get { return stats; } set { stats.Copy(value); } }

    public DFCareer career = new DFCareer();
    public DFCareer Career { get { return career; } set { career = value; } }

    protected string name;
    public string Name { get { return name; } set { name = value; } }

    public EntityTypes entityType = EntityTypes.None;
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
        // Allow an active shield effect to mitigate incoming damage from all sources
        // Testing classic shows that Shield will mitigate physical, magical, and fall damage
        // if (EntityBehaviour)
        // {
        //     EntityEffectManager manager = EntityBehaviour.GetComponent<EntityEffectManager>();
        //     if (manager)
        //     {
        //         Shield shield = (Shield)manager.FindIncumbentEffect<Shield>();
        //         if (shield != null)
        //             amount = shield.DamageShield(amount);
        //     }
        // }
        // TODO

        return SetHealth(currentHealth - amount);
    }

    public int MaxHealthLimiter { get; private set; }
    public int maxHealth;
    // Gets maximum health with effect limiter
    int GetMaxHealth()
    {
        // Limiter must be 1 or greater
        if (MaxHealthLimiter < 1)
            return maxHealth;

        return (MaxHealthLimiter < maxHealth) ? MaxHealthLimiter : maxHealth;
    }

    public int currentHealth;
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
    // 疲劳值
    public int currentFatigue;
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

    public int MagicResist { get { return FormulaUtils.MagicResist(stats.LiveWillpower); } }

    // 魔法值
    public int currentMagicka;
    public int maxMagicka;
    public int MaxMagicka { get { return GetMaxMagicka(); } set { maxMagicka = value; } }
    public int MaxMagickaModifier { get; private set; }
    // Gets maximum magicka with effect modifier
    public int CurrentMagicka { get { return GetCurrentMagicka(); } set { SetMagicka(value); } }
    public virtual int SetMagicka(int amount, bool restoreMode = false)
    {
        currentMagicka = (restoreMode) ? amount : Mathf.Clamp(amount, 0, MaxMagicka);
        // if (currentMagicka <= 0)
        //     RaiseOnMagickaDepletedEvent();
        return currentMagicka;
    }
    int GetCurrentMagicka()
    {
        return currentMagicka;
    }
    
    int GetMaxMagicka()
    {
        int effectiveMagicka = GetRawMaxMagicka() + MaxMagickaModifier;
        if (effectiveMagicka < 0)
            effectiveMagicka = 0;

        return effectiveMagicka;
    }
    // Gets raw maximum magicka without modifier
    int GetRawMaxMagicka()
    {
        // Player's maximum magicka determined by career and intelligence, enemies are set by level elsewhere
        if (career != null && this == Main.Inst.hero)
            return FormulaUtils.SpellPoints(stats.LiveIntelligence, career.SpellPointMultiplierValue);
        else
            return maxMagicka;
    }

    public void FillVitalSigns()
    {
        currentHealth = MaxHealth;
        currentFatigue = MaxFatigue;
        currentMagicka = MaxMagicka;
    }

    private void Awake() {
        skills = new DSkills();
        equipTable = new ItemEquipTable(this);
    }

    protected List<EffectBundleSettings> spellbook = new List<EffectBundleSettings>();

    bool[] resistanceFlags = new bool[5];     // Indices map to DFCareer.Elements 0-4
    int[] resistanceChances = new int[5];

    protected DResistances resistances = new DResistances();
    public DResistances Resistances { get { return resistances; } set { resistances.Copy(value); } }
    /// <summary>
    /// Check if entity has a specific resistance flag raised.
    /// </summary>
    /// <param name="elementType">Element type.</param>
    public bool HasResistanceFlag(DFCareer.Elements elementType)
    {
        return resistanceFlags[(int)elementType];
    }

    /// <summary>
    /// Gets current total resistance chance for an element.
    /// This is only used when corresponding elemental resistance flag is raised by effect.
    /// </summary>
    /// <param name="elementType">Element type to check total resistance value of.</param>
    /// <returns>Resistance chance for that element.</returns>
    public int GetResistanceChance(DFCareer.Elements elementType)
    {
        return resistanceChances[(int)elementType];
    }

    /// <summary>
    /// Handle shared logic when player attacks entity.
    /// </summary>
    public void HandleAttackFromSource(Actor sourceEntityBehaviour)
    {
        // Break "normal power" concealment effects on source
        if (sourceEntityBehaviour && sourceEntityBehaviour.IsMagicallyConcealedNormalPower)
            EffectManager.BreakNormalPowerConcealmentEffects(sourceEntityBehaviour);

        // When source is player
        if (sourceEntityBehaviour == Main.Inst.hero)
        {
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            // Handle civilian NPC crime reporting
            if (EntityType == EntityTypes.CivilianNPC)
            {
                MobilePersonNPC mobileNpc = transform.GetComponent<MobilePersonNPC>();
                if (mobileNpc)
                {
                    // Handle assault or murder
                    if (Entity.CurrentHealth > 0)
                    {
                        playerEntity.CrimeCommitted = PlayerEntity.Crimes.Assault;
                        playerEntity.SpawnCityGuards(true);
                    }
                    else
                    {
                        if (!mobileNpc.IsGuard)
                        {
                            playerEntity.TallyCrimeGuildRequirements(false, 5);
                            playerEntity.CrimeCommitted = PlayerEntity.Crimes.Murder;
                            playerEntity.SpawnCityGuards(true);
                        }
                        else
                        {
                            playerEntity.CrimeCommitted = PlayerEntity.Crimes.Assault;
                            playerEntity.SpawnCityGuard(mobileNpc.transform.position, mobileNpc.transform.forward);
                        }

                        // Disable when dead
                        mobileNpc.Motor.gameObject.SetActive(false);
                    }
                }
            }

            // Handle equipped Azura's Star trapping slain enemy monsters
            // This is always successful if Azura's Star is empty and equipped
            if (EntityType == EntityTypes.EnemyMonster && playerEntity.IsAzurasStarEquipped && CurrentHealth <= 0)
            {
                // EnemyEntity enemyEntity = entity as EnemyEntity;
                // if (SoulTrap.FillEmptyTrapItem((MobileTypes)enemyEntity.MobileEnemy.ID, true))
                // {
                //     DaggerfallUI.AddHUDText(TextManager.Instance.GetLocalizedText("trapSuccess"), 1.5f);
                // }
            }

            // Handle mobile enemy aggro
            if (EntityType == EntityTypes.EnemyClass || EntityType == EntityTypes.EnemyMonster)
            {
                // Make enemy aggressive to player
                EnemyMotor enemyMotor = transform.GetComponent<EnemyMotor>();
                if (enemyMotor)
                {
                    // 将区域内敌人变得敌对
                    if (!enemyMotor.IsHostile)
                    {
                        GameManager.Instance.MakeEnemiesHostile();
                    }
                    enemyMotor.MakeEnemyHostileToAttacker(GameManager.Instance.PlayerEntityBehaviour);
                }

                // Handle killing guards
                EnemyEntity enemyEntity = entity as EnemyEntity;
                if (enemyEntity.MobileEnemy.ID == (int)MobileTypes.Knight_CityWatch && entity.CurrentHealth <= 0)
                {
                    playerEntity.TallyCrimeGuildRequirements(false, 1);
                    playerEntity.CrimeCommitted = PlayerEntity.Crimes.Murder;
                }
            }
        }
    }

    protected ItemCollection items = new ItemCollection();
    public ItemCollection Items { get { return items; } set { items.ReplaceAll(value); } }

    protected ItemEquipTable equipTable;
    public ItemEquipTable ItemEquipTable { get { return equipTable; } }
}