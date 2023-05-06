using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

// struct player_class
[Serializable]
public class ClassCfg : ICSVParser
{
    public string name;
    public int str;
    public int inte;
    public int wis;
    public int dex;
    public int con;
    public int[] c_skills;
    public int[] x_skills;

    public void ParseCSV(CSVLoader loader)
    {
        c_skills = new int[10];
        x_skills = new int[10];
        var skillVals = loader.ReadStringArray("SKILL_DISARM_PHYS");
        c_skills[(int)SKILL.SKILL_DISARM_PHYS] = Utils.ToInt(skillVals[0]);
        x_skills[(int)SKILL.SKILL_DISARM_PHYS] = Utils.ToInt(skillVals[1]);
        skillVals = loader.ReadStringArray("SKILL_DISARM_MAGIC");
        c_skills[(int)SKILL.SKILL_DISARM_MAGIC] = Utils.ToInt(skillVals[0]);
        x_skills[(int)SKILL.SKILL_DISARM_MAGIC] = Utils.ToInt(skillVals[1]);
        skillVals = loader.ReadStringArray("SKILL_DEVICE");
        c_skills[(int)SKILL.SKILL_DEVICE] = Utils.ToInt(skillVals[0]);
        x_skills[(int)SKILL.SKILL_DEVICE] = Utils.ToInt(skillVals[1]);
        skillVals = loader.ReadStringArray("SKILL_SAVE");
        c_skills[(int)SKILL.SKILL_SAVE] = Utils.ToInt(skillVals[0]);
        x_skills[(int)SKILL.SKILL_SAVE] = Utils.ToInt(skillVals[1]);
    }
}

public static class Classes
{
    public static List<ClassCfg> classCfgs;

    public static void Init()
    {
        classCfgs = CSVLoader.LoadCSV<ClassCfg>("Assets/Configs/classes.csv");
    }
}
