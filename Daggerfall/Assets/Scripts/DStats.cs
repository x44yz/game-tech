using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Daggerfall stats collection for every entity.
/// </summary>
[Serializable]
public class DStats
{
    #region Fields

    public const int Count = 8;
    const int defaultValue = 50;

    // Current permanent stat values
    [SerializeField] int Strength;
    [SerializeField] int Intelligence;
    [SerializeField] int Willpower;
    [SerializeField] int Agility;
    [SerializeField] int Endurance;
    [SerializeField] int Personality;
    [SerializeField] int Speed;
    [SerializeField] int Luck;

    // Mods are temporary changes to stat values from effects
    // Default is 0 - effects can raise/lower mod values during their lifecycle
    // This is designed so that effects are never operating on permanent stat values
    int[] mods = new int[Count];
    int[] maxMods = new int[Count];

    #endregion

    #region Properties

    public int LiveStrength { get { return GetLiveStatValue(Stats.Strength); } }
    public int LiveIntelligence { get { return GetLiveStatValue(Stats.Intelligence); } }
    public int LiveWillpower { get { return GetLiveStatValue(Stats.Willpower); } }
    public int LiveAgility { get { return GetLiveStatValue(Stats.Agility); } }
    public int LiveEndurance { get { return GetLiveStatValue(Stats.Endurance); } }
    public int LivePersonality { get { return GetLiveStatValue(Stats.Personality); } }
    public int LiveSpeed { get { return GetLiveStatValue(Stats.Speed); } }
    public int LiveLuck { get { return GetLiveStatValue(Stats.Luck); } }

    public int PermanentStrength { get { return GetPermanentStatValue(Stats.Strength); } }
    public int PermanentIntelligence { get { return GetPermanentStatValue(Stats.Intelligence); } }
    public int PermanentWillpower { get { return GetPermanentStatValue(Stats.Willpower); } }
    public int PermanentAgility { get { return GetPermanentStatValue(Stats.Agility); } }
    public int PermanentEndurance { get { return GetPermanentStatValue(Stats.Endurance); } }
    public int PermanentPersonality { get { return GetPermanentStatValue(Stats.Personality); } }
    public int PermanentSpeed { get { return GetPermanentStatValue(Stats.Speed); } }
    public int PermanentLuck { get { return GetPermanentStatValue(Stats.Luck); } }

    #endregion

    #region Constructors

    public DStats()
    {
        SetDefaults();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Check if all permanent stat values are at max.
    /// </summary>
    /// <returns>True if all at max.</returns>
    public bool IsAllMax()
    {
        int max = FormulaUtils.MaxStatValue();
        return (
            PermanentStrength == max &&
            PermanentIntelligence == max &&
            PermanentWillpower == max &&
            PermanentAgility == max &&
            PermanentEndurance == max &&
            PermanentPersonality == max &&
            PermanentSpeed == max &&
            PermanentLuck == max);
    }

    /// <summary>
    /// Set default value to each stat.
    /// </summary>
    public void SetDefaults()
    {
        Strength = defaultValue;
        Intelligence = defaultValue;
        Willpower = defaultValue;
        Agility = defaultValue;
        Endurance = defaultValue;
        Personality = defaultValue;
        Speed = defaultValue;
        Luck = defaultValue;
        Array.Clear(mods, 0, Count);
        Array.Clear(maxMods, 0, Count);
    }

    /// <summary>
    /// Copy contents of another DStats into this one.
    /// Does not copy active effect mods.
    /// </summary>
    /// <param name="other">Stats collection to copy from.</param>
    public void Copy(DStats other)
    {
        Strength = other.Strength;
        Intelligence = other.Intelligence;
        Willpower = other.Willpower;
        Agility = other.Agility;
        Endurance = other.Endurance;
        Personality = other.Personality;
        Speed = other.Speed;
        Luck = other.Luck;
    }

    /// <summary>
    /// Create a new copy of this stat collection.
    /// Does not copy active effect mods.
    /// </summary>
    /// <returns>New DStats which is a copy of this DStats.</returns>
    public DStats Clone()
    {
        DStats newStats = new DStats();
        newStats.Copy(this);

        return newStats;
    }

    #endregion

    #region Getters

    /// <summary>
    /// Gets live stat value by enum, including effect mods.
    /// </summary>
    /// <param name="stat">Stat to get.</param>
    /// <returns>Stat value.</returns>
    public int GetLiveStatValue(Stats stat)
    {
        int value = GetPermanentStatValue(stat) + mods[(int)stat];
        int maxValue = FormulaUtils.MaxStatValue() + maxMods[(int)stat];

        // Clamp live stat to 0-maxValue (accounting for any max value mods)
        value = Mathf.Clamp(value, 0, maxValue);

        return (short)value;
    }

    /// <summary>
    /// Gets live stat value by index, including effect mods.
    /// </summary>
    /// <param name="index">Index of stat.</param>
    /// <returns>Stat value.</returns>
    public int GetLiveStatValue(int index)
    {
        if (index < 0 || index >= Count)
            return 0;

        return GetLiveStatValue((Stats)index);
    }

    /// <summary>
    /// Gets permanent stat value by index, does not include effect mods.
    /// </summary>
    /// <param name="index">Index of stat.</param>
    /// <returns>Stat value.</returns>
    public int GetPermanentStatValue(int index)
    {
        if (index < 0 || index >= Count)
            return 0;

        return GetPermanentStatValue((Stats)index);
    }

    /// <summary>
    /// Gets permanent stat value by enum, does not include effect mods.
    /// </summary>
    /// <param name="stat">Stat to get.</param>
    /// <returns>Stat value.</returns>
    public int GetPermanentStatValue(Stats stat)
    {
        switch (stat)
        {
            case Stats.Strength:
                return Strength;
            case Stats.Intelligence:
                return Intelligence;
            case Stats.Willpower:
                return Willpower;
            case Stats.Agility:
                return Agility;
            case Stats.Endurance:
                return Endurance;
            case Stats.Personality:
                return Personality;
            case Stats.Speed:
                return Speed;
            case Stats.Luck:
                return Luck;
            default:
                return 0;
        }
    }

    /// <summary>
    /// Assign mods from effect manager.
    /// </summary>
    public void AssignMods(int[] statMods, int[] statMaxMods)
    {
        Array.Copy(statMods, mods, Count);
        Array.Copy(statMaxMods, maxMods, Count);
    }

    #endregion

    #region Setters

    /// <summary>
    /// Sets permanent stat value by enum, does not change effect mods.
    /// </summary>
    /// <param name="stat">Stat to set.</param>
    /// <param name="value">Stat value.</param>
    public void SetPermanentStatValue(Stats stat, int value)
    {
        switch (stat)
        {
            case Stats.Strength:
                Strength = value;
                break;
            case Stats.Intelligence:
                Intelligence = value;
                break;
            case Stats.Willpower:
                Willpower = value;
                break;
            case Stats.Agility:
                Agility = value;
                break;
            case Stats.Endurance:
                Endurance = value;
                break;
            case Stats.Personality:
                Personality = value;
                break;
            case Stats.Speed:
                Speed = value;
                break;
            case Stats.Luck:
                Luck = value;
                break;
        }
    }

    /// <summary>
    /// Sets permanent stat value by index, does not change effect mods.
    /// </summary>
    /// <param name="index">Index of stat.</param>
    /// <param name="value">Stat value.</param>
    public void SetPermanentStatValue(int index, int value)
    {
        SetPermanentStatValue((Stats)index, value);
    }

    /// <summary>
    /// Set permanent stat values from career, does not change effect mods.
    /// </summary>
    /// <param name="career">Career to set stats from.</param>
    public void SetPermanentFromCareer(DFCareer career)
    {
        if (career != null)
        {
            Strength = career.Strength;
            Intelligence = career.Intelligence;
            Willpower = career.Willpower;
            Agility = career.Agility;
            Endurance = career.Endurance;
            Personality = career.Personality;
            Speed = career.Speed;
            Luck = career.Luck;
        }
    }

    #endregion
}