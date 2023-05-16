using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;
using QuickDemo.FSM;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    // public ActorEffect effectManager => GetComponent<ActorEffect>();

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

    public int DecreaseFatigue(int amount, bool assignMultiplier = false)
    {
        // Optionally assign fatigue multiplier
        // This seems to be case for spell effects that damage fatigue
        if (assignMultiplier)
            amount *= FatigueMultiplier;

        return SetFatigue(currentFatigue - amount);
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
    public void AddSpell(EffectBundleSettings spell)
    {
        spellbook.Add(spell);
    }

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
            ActorEffect.BreakNormalPowerConcealmentEffects(sourceEntityBehaviour);

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
                    // playerEntity.TallyCrimeGuildRequirements(false, 1);
                    playerEntity.CrimeCommitted = PlayerEntity.Crimes.Murder;
                }
            }
        }
    }

    protected ItemCollection items = new ItemCollection();
    public ItemCollection Items { get { return items; } set { items.ReplaceAll(value); } }

    protected ItemEquipTable equipTable;
    public ItemEquipTable ItemEquipTable { get { return equipTable; } }

        /// <summary>
    /// Helper to check if specific magical concealment flags are active on player.
    /// </summary>
    /// <param name="flags">Comparison flags.</param>
    /// <returns>True if matching.</returns>
    public MagicalConcealmentFlags MagicalConcealmentFlags { get; set; }
    public bool HasConcealment(MagicalConcealmentFlags flags)
    {
        return ((MagicalConcealmentFlags & flags) == flags);
    }

    public int IncreaseMagicka(int amount)
    {
        return SetMagicka(currentMagicka + amount);
    }

    public int DecreaseMagicka(int amount)
    {
        return SetMagicka(currentMagicka - amount);
    }

    public bool IsImmuneToParalysis { get; set; }
    public bool IsImmuneToDisease { get; set; }
    public bool IsSilenced { get; set; }
    public bool IsWaterWalking { get; set; }
    public bool IsWaterBreathing { get; set; }
    // public MagicalConcealmentFlags MagicalConcealmentFlags { get; set; }
    public bool IsEnhancedClimbing { get; set; }
    public bool IsEnhancedJumping { get; set; }
    public bool IsSlowFalling { get; set; }
    public bool IsAbsorbingSpells { get; set; }
    // public int MaxMagickaModifier { get; private set; }
    // public int MaxHealthLimiter { get; private set; }
    public float IncreasedWeightAllowanceMultiplier { get; private set; }
    // public int IncreasedArmorValueModifier { get; private set; }
    // public int DecreasedArmorValueModifier { get; private set; }
    // public int ChanceToHitModifier { get; private set; }
    public bool ImprovedAcuteHearing { get; set; }
    public bool ImprovedAthleticism { get; set; }
    // public bool ImprovedAdrenalineRush { get; set; }

    bool isParalyzed = false;
    /// <summary>
    /// Gets or sets paralyzation flag.
    /// Always returns false when isImmuneToParalysis is true.
    /// Each entity type will need to act on paralyzation in their own unique way.
    /// Note: This value is intentionally not serialized. It should only be set by live effects.
    /// </summary>
    public bool IsParalyzed
    {
        get { return (!IsImmuneToParalysis && isParalyzed); }
        set { isParalyzed = value; }
    }
}

