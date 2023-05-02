using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// struct player_class
public class ClassCfg : ICSVParser
{
    public string name;
    public int str;
    public int inte;
    public int wis;
    public int dex;
    public int con;
    public int skillDisarmPhys;
    public int skillDisarmMagic;
    public int skillDevice;
    public int skillSave;
    public int skillStealth;
    public int skillSearch;
    public int skillMelee;
    public int skillShoot;
    public int skillThrow;
    public int skillDig;
    public int hitdie;
    public int exp;
    public int infravision;
    public int history;
    public int age;
    public int base_hgt;
    public int mod_hgt;
    public int base_wgt;
    public int mod_wgt;

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
