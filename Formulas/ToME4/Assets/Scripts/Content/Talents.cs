using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentCfg : ICSVParser
{
    public string name;
    public string shortName;
    public string image;
    public string type;
    public int points;
    public int stamina;
    public int cooldown;
    public int range;

    public void ParseCSV(CSVLoader loader)
    {
        name = loader.ReadString("NAME");
        shortName = loader.ReadString("SHORT_NAME");
        image = loader.ReadString("IMAGE");
        type = loader.ReadString("TYPE");
        points = loader.ReadInt("POINTS");
        stamina = loader.ReadInt("STAMINA");
        cooldown = loader.ReadInt("COOLDOWN");
        range = loader.ReadInt("RANGE");
    }
}

public static class Talents
{
    public static List<TalentCfg> talentCfgs;

    public static void Init()
    {
        talentCfgs = CSVLoader.LoadCSV<TalentCfg>("Assets/Configs/talents.csv");
    }
}
