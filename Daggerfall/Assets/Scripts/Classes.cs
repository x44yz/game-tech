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
