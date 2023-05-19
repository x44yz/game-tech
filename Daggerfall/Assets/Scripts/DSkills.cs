using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Daggerfall skills collection for every entity.
/// </summary>
[Serializable]
public class DSkills
{
    #region Fields

    public const int Count = (int)Skills.Count;
    public const int PrimarySkillsCount = 3;
    public const int MajorSkillsCount = 3;
    public const int MinorSkillsCount = 6;
    const int minDefaultValue = 3;
    const int maxDefaultValue = 6;

    // Current permanent skill values
    [SerializeField] short Medical;
    [SerializeField] short Etiquette;
    [SerializeField] short Streetwise;
    [SerializeField] short Jumping;
    [SerializeField] short Orcish;
    [SerializeField] short Harpy;
    [SerializeField] short Giantish;
    [SerializeField] short Dragonish;
    [SerializeField] short Nymph;
    [SerializeField] short Daedric;
    [SerializeField] short Spriggan;
    [SerializeField] short Centaurian;
    [SerializeField] short Impish;
    [SerializeField] short Lockpicking;
    [SerializeField] short Mercantile;
    [SerializeField] short Pickpocket;
    [SerializeField] short Stealth;
    [SerializeField] short Swimming;
    [SerializeField] short Climbing;
    [SerializeField] short Backstabbing;
    [SerializeField] short Dodging;
    [SerializeField] short Running;
    [SerializeField] short Destruction;
    [SerializeField] short Restoration;
    [SerializeField] short Illusion;
    [SerializeField] short Alteration;
    [SerializeField] short Thaumaturgy;
    [SerializeField] short Mysticism;
    [SerializeField] short ShortBlade;
    [SerializeField] short LongBlade;
    [SerializeField] short HandToHand;
    [SerializeField] short Axe;
    [SerializeField] short BluntWeapon;
    [SerializeField] short Archery;
    [SerializeField] short CriticalStrike;

    // Mods are temporary changes to skill values from effects
    // Default is 0 - effects can raise/lower mod values during their lifecycle
    // This is designed so that effects are never operating on permanent skill values
    int[] mods = new int[Count];

    #endregion

    #region Constructors

    public DSkills()
    {
        SetDefaults();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Set default value to each skill.
    /// </summary>
    public void SetDefaults()
    {
        for (int i = 0; i < Count; i++)
        {
            SetPermanentSkillValue(i, (short)UnityEngine.Random.Range(minDefaultValue, maxDefaultValue + 1));
        }
        Array.Clear(mods, 0, Count);
    }

    /// <summary>
    /// Copy contents of another Skills into this one.
    /// Does not copy active effect mods.
    /// </summary>
    /// <param name="other">Skilla collection to copy from.</param>
    public void Copy(DSkills other)
    {
        for (int i = 0; i < Count; i++)
        {
            SetPermanentSkillValue(i, other.GetPermanentSkillValue(i));
        }
    }

    /// <summary>
    /// Create a new copy of this stat collection.
    /// Does not copy active effect mods.
    /// </summary>
    /// <returns>New DaggerfallStats which is a copy of this DaggerfallStats.</returns>
    public DSkills Clone()
    {
        DSkills newSkills = new DSkills();
        newSkills.Copy(this);

        return newSkills;
    }

    #endregion

    #region Getters

    /// <summary>
    /// Gets live skill value by enum, including effect mods.
    /// </summary>
    /// <param name="skill">Skill to get.</param>
    /// <returns>Skill value.</returns>
    public short GetLiveSkillValue(Skills skill)
    {
        int mod = mods[(int)skill];
        int value = GetPermanentSkillValue(skill) + mod;

        // TODO: Any other clamping or processing

        return (short)value;
    }

    /// <summary>
    /// Gets live skill value by index, including effect mods.
    /// </summary>
    /// <param name="index">Index of skill.</param>
    /// <returns>Skill value.</returns>
    public short GetLiveSkillValue(int index)
    {
        if (index < 0 || index >= Count)
            return 0;

        return GetLiveSkillValue((Skills)index);
    }

    /// <summary>
    /// Gets permanent skill value by index, does not include effect mods.
    /// </summary>
    /// <param name="index">Index of skill.</param>
    /// <returns>Skill value.</returns>
    public short GetPermanentSkillValue(int index)
    {
        if (index < 0 || index >= Count)
            return 0;

        return GetPermanentSkillValue((Skills)index);
    }

    /// <summary>
    /// Gets permanent skill value by enum, does not include effect mods.
    /// </summary>
    /// <param name="skill">Skill to get.</param>
    /// <returns>Skill value.</returns>
    public short GetPermanentSkillValue(Skills skill)
    {
        switch (skill)
        {
            case Skills.Medical:
                return Medical;
            case Skills.Etiquette:
                return Etiquette;
            case Skills.Streetwise:
                return Streetwise;
            case Skills.Jumping:
                return Jumping;
            case Skills.Orcish:
                return Orcish;
            case Skills.Harpy:
                return Harpy;
            case Skills.Giantish:
                return Giantish;
            case Skills.Dragonish:
                return Dragonish;
            case Skills.Nymph:
                return Nymph;
            case Skills.Daedric:
                return Daedric;
            case Skills.Spriggan:
                return Spriggan;
            case Skills.Centaurian:
                return Centaurian;
            case Skills.Impish:
                return Impish;
            case Skills.Lockpicking:
                return Lockpicking;
            case Skills.Mercantile:
                return Mercantile;
            case Skills.Pickpocket:
                return Pickpocket;
            case Skills.Stealth:
                return Stealth;
            case Skills.Swimming:
                return Swimming;
            case Skills.Climbing:
                return Climbing;
            case Skills.Backstabbing:
                return Backstabbing;
            case Skills.Dodging:
                return Dodging;
            case Skills.Running:
                return Running;
            case Skills.Destruction:
                return Destruction;
            case Skills.Restoration:
                return Restoration;
            case Skills.Illusion:
                return Illusion;
            case Skills.Alteration:
                return Alteration;
            case Skills.Thaumaturgy:
                return Thaumaturgy;
            case Skills.Mysticism:
                return Mysticism;
            case Skills.ShortBlade:
                return ShortBlade;
            case Skills.LongBlade:
                return LongBlade;
            case Skills.HandToHand:
                return HandToHand;
            case Skills.Axe:
                return Axe;
            case Skills.BluntWeapon:
                return BluntWeapon;
            case Skills.Archery:
                return Archery;
            case Skills.CriticalStrike:
                return CriticalStrike;
            default:
                return 0;
        }
    }

    #endregion

    #region Setters

    /// <summary>
    /// Sets permanent skill value by enum, does not change effect mods.
    /// </summary>
    /// <param name="skill">Skill to set.</param>
    /// <param name="value">Skill value.</param>
    public void SetPermanentSkillValue(Skills skill, int value)
    {
        SetPermanentSkillValue(skill, (short)value);
    }

    public void SetPermanentSkillValue(Skills skill, short value)
    {
        switch (skill)
        {
            case Skills.Medical:
                Medical = value;
                break;
            case Skills.Etiquette:
                Etiquette = value;
                break;
            case Skills.Streetwise:
                Streetwise = value;
                break;
            case Skills.Jumping:
                Jumping = value;
                break;
            case Skills.Orcish:
                Orcish = value;
                break;
            case Skills.Harpy:
                Harpy = value;
                break;
            case Skills.Giantish:
                Giantish = value;
                break;
            case Skills.Dragonish:
                Dragonish = value;
                break;
            case Skills.Nymph:
                Nymph = value;
                break;
            case Skills.Daedric:
                Daedric = value;
                break;
            case Skills.Spriggan:
                Spriggan = value;
                break;
            case Skills.Centaurian:
                Centaurian = value;
                break;
            case Skills.Impish:
                Impish = value;
                break;
            case Skills.Lockpicking:
                Lockpicking = value;
                break;
            case Skills.Mercantile:
                Mercantile = value;
                break;
            case Skills.Pickpocket:
                Pickpocket = value;
                break;
            case Skills.Stealth:
                Stealth = value;
                break;
            case Skills.Swimming:
                Swimming = value;
                break;
            case Skills.Climbing:
                Climbing = value;
                break;
            case Skills.Backstabbing:
                Backstabbing = value;
                break;
            case Skills.Dodging:
                Dodging = value;
                break;
            case Skills.Running:
                Running = value;
                break;
            case Skills.Destruction:
                Destruction = value;
                break;
            case Skills.Restoration:
                Restoration = value;
                break;
            case Skills.Illusion:
                Illusion = value;
                break;
            case Skills.Alteration:
                Alteration = value;
                break;
            case Skills.Thaumaturgy:
                Thaumaturgy = value;
                break;
            case Skills.Mysticism:
                Mysticism = value;
                break;
            case Skills.ShortBlade:
                ShortBlade = value;
                break;
            case Skills.LongBlade:
                LongBlade = value;
                break;
            case Skills.HandToHand:
                HandToHand = value;
                break;
            case Skills.Axe:
                Axe = value;
                break;
            case Skills.BluntWeapon:
                BluntWeapon = value;
                break;
            case Skills.Archery:
                Archery = value;
                break;
            case Skills.CriticalStrike:
                CriticalStrike = value;
                break;
        }
    }

    /// <summary>
    /// Sets permanent skill value by index, does not change effect mods.
    /// </summary>
    /// <param name="index">Index of skill.</param>
    /// <param name="value">Skill value.</param>
    public void SetPermanentSkillValue(int index, short value)
    {
        if (index < 0 || index >= Count)
            return;

        SetPermanentSkillValue((Skills)index, value);
    }

    /// <summary>
    /// Assign mods from effect manager.
    /// </summary>
    public void AssignMods(int[] skillMods)
    {
        Array.Copy(skillMods, mods, Count);
    }

    #endregion

    #region Static Methods

    public static Stats GetPrimaryStat(Skills skill)
    {
        switch (skill)
        {
            case Skills.Medical:
                return Stats.Intelligence;
            case Skills.Etiquette:
                return Stats.Personality;
            case Skills.Streetwise:
                return Stats.Personality;
            case Skills.Jumping:
                return Stats.Strength;
            case Skills.Orcish:
                return Stats.Intelligence;
            case Skills.Harpy:
                return Stats.Intelligence;
            case Skills.Giantish:
                return Stats.Intelligence;
            case Skills.Dragonish:
                return Stats.Intelligence;
            case Skills.Nymph:
                return Stats.Intelligence;
            case Skills.Daedric:
                return Stats.Intelligence;
            case Skills.Spriggan:
                return Stats.Intelligence;
            case Skills.Centaurian:
                return Stats.Intelligence;
            case Skills.Impish:
                return Stats.Intelligence;
            case Skills.Lockpicking:
                return Stats.Intelligence;
            case Skills.Mercantile:
                return Stats.Personality;
            case Skills.Pickpocket:
                return Stats.Agility;
            case Skills.Stealth:
                return Stats.Agility;
            case Skills.Swimming:
                return Stats.Endurance;
            case Skills.Climbing:
                return Stats.Strength;
            case Skills.Backstabbing:
                return Stats.Agility;
            case Skills.Dodging:
                return Stats.Speed;
            case Skills.Running:
                return Stats.Speed;
            case Skills.Destruction:
                return Stats.Willpower;
            case Skills.Restoration:
                return Stats.Willpower;
            case Skills.Illusion:
                return Stats.Willpower;
            case Skills.Alteration:
                return Stats.Willpower;
            case Skills.Thaumaturgy:
                return Stats.Willpower;
            case Skills.Mysticism:
                return Stats.Willpower;
            case Skills.ShortBlade:
                return Stats.Agility;
            case Skills.LongBlade:
                return Stats.Agility;
            case Skills.HandToHand:
                return Stats.Agility;
            case Skills.Axe:
                return Stats.Strength;
            case Skills.BluntWeapon:
                return Stats.Strength;
            case Skills.Archery:
                return Stats.Agility;
            case Skills.CriticalStrike:
                return Stats.Agility;
            default:
                return (Stats)(-1);
        }
    }

    public static Stats GetPrimaryStat(int index)
    {
        return GetPrimaryStat((Skills)index);
    }

    public static int GetAdvancementMultiplier(Skills skill)
    {
        switch (skill)
        {
            case Skills.Medical:
                return 12;
            case Skills.Etiquette:
            case Skills.Streetwise:
                return 1;
            case Skills.Jumping:
                return 5;
            case Skills.Orcish:
            case Skills.Harpy:
            case Skills.Giantish:
            case Skills.Dragonish:
            case Skills.Nymph:
            case Skills.Daedric:
            case Skills.Spriggan:
            case Skills.Centaurian:
            case Skills.Impish:
                return 15;
            case Skills.Lockpicking:
                return 2;
            case Skills.Mercantile:
                return 1;
            case Skills.Pickpocket:
            case Skills.Stealth:
                return 2;
            case Skills.Swimming:
                return 1;
            case Skills.Climbing:
                return 2;
            case Skills.Backstabbing:
                return 1;
            case Skills.Dodging:
                return 4;
            case Skills.Running:
                return 50;
            case Skills.Destruction:
                return 1;
            case Skills.Restoration:
                return 2;
            case Skills.Illusion:
            case Skills.Alteration:
                return 1;
            case Skills.Thaumaturgy:
                return 2;
            case Skills.Mysticism:
                return 1;
            case Skills.ShortBlade:
            case Skills.LongBlade:
            case Skills.HandToHand:
            case Skills.Axe:
            case Skills.BluntWeapon:
                return 2;
            case Skills.Archery:
                return 1;
            case Skills.CriticalStrike:
                return 8;
            default:
                return 0;
        }
    }

    public static bool IsLanguageSkill(Skills skill)
    {
        switch (skill)
        {
            case Skills.Orcish:
            case Skills.Harpy:
            case Skills.Giantish:
            case Skills.Dragonish:
            case Skills.Nymph:
            case Skills.Daedric:
            case Skills.Spriggan:
            case Skills.Centaurian:
            case Skills.Impish:
                return true;
            default:
                return false;
        }
    }

    #endregion
}
