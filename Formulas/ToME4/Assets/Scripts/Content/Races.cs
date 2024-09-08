using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCfg : ICSVParser
{
    public string name;
    public string metaClass;
    public int str; // Strength
    public int con; // Constitution 体质
    public int dex; // Dexterity 敏捷
    public int mag; // Magic 魔法
    public int wil; // Willpower 意志
    public int cun; // Cunning 灵巧

    public void ParseCSV(CSVLoader loader)
    {
        name = loader.ReadString("NAME");
        metaClass = loader.ReadString("META_CLASS");
        str = loader.ReadInt("STR");
        con = loader.ReadInt("CON");
        dex = loader.ReadInt("DEX");
        mag = loader.ReadInt("MAG");
        wil = loader.ReadInt("WIL");
        cun = loader.ReadInt("CUN");
    }
}

public static class Races
{
    public static List<RaceCfg> raceCfgs;

    public static void Init()
    {
        raceCfgs = CSVLoader.LoadCSV<RaceCfg>("Assets/Configs/races.csv");
    }
}
