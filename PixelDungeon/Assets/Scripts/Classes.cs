using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

// struct player_class
[Serializable]
public class ClassCfg : ICSVParser
{
    public string Name;
    public int Strength;
    public int Intelligence;
    public int Willpower;
    public int Agility;
    public int Endurance;
    public int Personality;
    public int Speed;
    public int Luck;
    public int ResistanceFlags;
    public int ImmunityFlags;
    public int LowToleranceFlags;
    public int CriticalWeaknessFlags;
    public int AbilityFlagsAndSpellPointsBitfield;
    public int RapidHealing;
    public int Regeneration;
    public int SpellAbsorptionFlags;
    public int AttackModifierFlags;
    public int ForbiddenMaterialsFlags;
    public int WeaponArmorShieldsBitfield;
    public int PrimarySkill1;
    public int PrimarySkill2;
    public int PrimarySkill3;
    public int MajorSkill1;
    public int MajorSkill2;
    public int MajorSkill3;
    public int MinorSkill1;
    public int MinorSkill2;
    public int MinorSkill3;
    public int MinorSkill4;
    public int MinorSkill5;
    public int MinorSkill6;
    public int HitPointsPerLevel;
    public int AdvancementMultiplier;
    public int Unknown1;
    public int[] Unknown2;

    public void ParseCSV(CSVLoader loader)
    {
        Name = loader.ReadString("Name");
        Strength = loader.ReadInt("Strength");
        Intelligence = loader.ReadInt("Intelligence");
        Willpower = loader.ReadInt("Willpower");
        Agility = loader.ReadInt("Agility");
        Endurance = loader.ReadInt("Endurance");
        Personality = loader.ReadInt("Personality");
        Speed = loader.ReadInt("Speed");
        Luck = loader.ReadInt("Luck");
        ResistanceFlags = loader.ReadInt("ResistanceFlags");
        ImmunityFlags = loader.ReadInt("ImmunityFlags");
        LowToleranceFlags = loader.ReadInt("LowToleranceFlags");
        CriticalWeaknessFlags = loader.ReadInt("CriticalWeaknessFlags");
        AbilityFlagsAndSpellPointsBitfield = loader.ReadInt("AbilityFlagsAndSpellPointsBitfield");
        RapidHealing = loader.ReadInt("RapidHealing");
        Regeneration = loader.ReadInt("Regeneration");
        SpellAbsorptionFlags = loader.ReadInt("SpellAbsorptionFlags");
        AttackModifierFlags = loader.ReadInt("AttackModifierFlags");
        ForbiddenMaterialsFlags = loader.ReadInt("ForbiddenMaterialsFlags");
        WeaponArmorShieldsBitfield = loader.ReadInt("WeaponArmorShieldsBitfield");
        PrimarySkill1 = loader.ReadInt("PrimarySkill1");
        PrimarySkill2 = loader.ReadInt("PrimarySkill2");
        PrimarySkill3 = loader.ReadInt("PrimarySkill3");
        MajorSkill1 = loader.ReadInt("MajorSkill1");
        MajorSkill2 = loader.ReadInt("MajorSkill2");
        MajorSkill3 = loader.ReadInt("MajorSkill3");
        MinorSkill1 = loader.ReadInt("MinorSkill1");
        MinorSkill2 = loader.ReadInt("MinorSkill2");
        MinorSkill3 = loader.ReadInt("MinorSkill3");
        MinorSkill4 = loader.ReadInt("MinorSkill4");
        MinorSkill5 = loader.ReadInt("MinorSkill5");
        MinorSkill6 = loader.ReadInt("MinorSkill6");
        HitPointsPerLevel = loader.ReadInt("HitPointsPerLevel");
        AdvancementMultiplier = loader.ReadInt("AdvancementMultiplier");
        Unknown1 = loader.ReadInt("Unknown1");
        Unknown2 = loader.ReadIntArray("Unknown2");
    }
}

public static class Classes
{

    public static void Init()
    {
        var classCfgs = CSVLoader.LoadCSV<ClassCfg>("Assets/Configs/classes.csv");
    }
}
