using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

/// <summary>
/// Weapon material values.
/// </summary>
public enum WeaponMaterialTypes
{
    None        = -1,
    Iron        = 0x0000,
    Steel       = 0x0001,
    Silver      = 0x0002,
    Elven       = 0x0003,
    Dwarven     = 0x0004,
    Mithril     = 0x0005,
    Adamantium  = 0x0006,
    Ebony       = 0x0007,
    Orcish      = 0x0008,
    Daedric     = 0x0009,
}

/// <summary>
/// Proficiency flags for forbidden weapons and weapon expertise.
/// </summary>
[Flags]
public enum ProficiencyFlags
{
    ShortBlades = 1,
    LongBlades = 2,
    HandToHand = 4,
    Axes = 8,
    BluntWeapons = 16,
    MissileWeapons = 32,
}

/// <summary>
/// Poison IDs. The first 8 are found on enemy weapons. The last 4 are created by ingesting drugs.
/// </summary>
public enum Poisons
{
    None = -1,
    Nux_Vomica = 128,
    Arsenic = 129,
    Moonseed = 130,
    Drothweed = 131,
    Somnalius = 132,
    Pyrrhic_Acid = 133,
    Magebane = 134,
    Thyrwort = 135,
    Indulcet = 136,
    Sursum = 137,
    Quaesto_Vil = 138,
    Aegrotat = 139,
}

public class Item : MonoBehaviour
{
    public int TemplateIndex; // item id
    // 伏魔的
    public bool IsEnchanted;
    public Poisons poisonType = Poisons.None;

    public virtual int GetBaseDamageMin()
    {
        return FormulaUtils.CalculateWeaponMinDamage((Weapons)TemplateIndex);
    }

    public virtual int GetBaseDamageMax()
    {
        return FormulaUtils.CalculateWeaponMaxDamage((Weapons)TemplateIndex);
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
}
