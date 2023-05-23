using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

//just putting this here for now, should probably be moved in the future
[Serializable]
public struct EffectRecordData : ICSVStrParser
{
    public int type;                    //indicates the main type of the effect, if -1 this effect should be ignore
    public int subType;                 //indicates subtype (for example, health, stamina magicka for a dmg effect).  If -1 there is no subtype
    public int descriptionTextIndex;    //+1200 = index into TEXT.RSC for the effect's description in spellbook & merchant
    public int spellMakerTextIndex;     //+1500 = index into TEXT.RSC for the effect's description in spell maker
    public int durationBase;
    public int durationMod;
    public int durationPerLevel;
    public int chanceBase;
    public int chanceMod;
    public int chancePerLevel;
    public int magnitudeBaseLow;
    public int magnitudeBaseHigh;
    public int magnitudeLevelBase;
    public int magnitudeLevelHigh;
    public int magnitudePerLevel;

    public void ParseStr(string[] values)
    {
        type = Utils.ToInt(values[0]);
        subType = Utils.ToInt(values[1]);
        descriptionTextIndex = Utils.ToInt(values[2]);
        spellMakerTextIndex = Utils.ToInt(values[3]);
        durationBase = Utils.ToInt(values[4]);
        durationMod = Utils.ToInt(values[5]);
        durationPerLevel = Utils.ToInt(values[6]);
        chanceBase = Utils.ToInt(values[7]);
        chanceMod = Utils.ToInt(values[8]);
        chancePerLevel = Utils.ToInt(values[9]);
        magnitudeBaseLow = Utils.ToInt(values[10]);
        magnitudeBaseHigh = Utils.ToInt(values[11]);
        magnitudeLevelBase = Utils.ToInt(values[12]);
        magnitudeLevelHigh = Utils.ToInt(values[13]);
        magnitudePerLevel = Utils.ToInt(values[14]);
    }
}

[Serializable]
public class SpellRecordData : ICSVParser
{
    public string spellName = "";       //name of spell - if starts w/ !, shouldn't be visibile to player
    public int element      = -1;       //element spell uses
    public int rangeType    = -1;       //touch, caster, area around caster etc.
    public int cost         = -1;       //spell cost
    public int index        = -1;       //index of spell.  Some from SPELLS.STD file, others are player spells
    public int icon         = -1;       //icon index
    public EffectRecordData[] effects;  //each spell has 1-3 effects

    public void ParseCSV(CSVLoader loader)
    {
        spellName = loader.ReadString("spellName");
        element = loader.ReadInt("element");
        rangeType = loader.ReadInt("rangeType");
        cost = loader.ReadInt("cost");
        index = loader.ReadInt("index");
        icon = loader.ReadInt("icon");
        effects = new EffectRecordData[3];
        for (int i = 0; i < effects.Length; ++i)
        {
            var k = loader.ReadStringArray($"effect{i}");
            effects[i].ParseStr(k);
        }
    }
}

public class Spells
{
    public static List<SpellRecordData> spellRecordDatas;

    public static void Init()
    {
        spellRecordDatas = CSVLoader.LoadCSV<SpellRecordData>("Assets/Configs/spells.csv");
    }

    public static bool GetClassicSpellRecord(int id, out SpellRecordData spellOut)
    {
        for (int i = 0; i < spellRecordDatas.Count; ++i)
        {
            if (spellRecordDatas[i].index == id)
            {
                spellOut = spellRecordDatas[i];
                return true;
            }
        }

        Debug.LogError("xx-- cant find spell > " + id);
        spellOut = new SpellRecordData();
        return false;
    }
}
