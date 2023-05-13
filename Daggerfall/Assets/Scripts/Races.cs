// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // struct player_race
// [Serializable]
// public class RaceCfg : ICSVParser
// {
//     public string name;
//     public int str;
//     public int inte;
//     public int wis;
//     public int dex;
//     public int con;
//     public int skillDisarmPhys;
//     public int skillDisarmMagic;
//     public int skillDevice;
//     public int skillSave;
//     public int skillStealth;
//     public int skillSearch;
//     public int skillMelee;
//     public int skillShoot;
//     public int skillThrow;
//     public int skillDig;
//     public int hitdie;
//     public int exp;
//     public int infravision;
//     public int history;
//     public int age;
//     // public int base_hgt;
//     // public int mod_hgt;
//     // public int base_wgt;
//     // public int mod_wgt;

//     public void ParseCSV(CSVLoader loader)
//     {
//         name = loader.ReadString("NAME");
//         str = loader.ReadInt("STR");
//         inte = loader.ReadInt("INT");
//         wis = loader.ReadInt("WIS");
//         dex = loader.ReadInt("DEX");
//         con = loader.ReadInt("CON");
//         skillDisarmPhys = loader.ReadInt("SKILL_DISARM_PHYS");
//         skillDisarmMagic = loader.ReadInt("SKILL_DISARM_MAGIC");
//         skillDevice = loader.ReadInt("SKILL_DEVICE");
//         skillSave = loader.ReadInt("SKILL_SAVE");
//         skillStealth = loader.ReadInt("SKILL_STEALTH");
//         skillSearch = loader.ReadInt("SKILL_SEARCH");
//         skillMelee = loader.ReadInt("SKILL_MELEE");
//         skillShoot = loader.ReadInt("SKILL_SHOOT");
//         skillThrow = loader.ReadInt("SKILL_THROW");
//         skillDig = loader.ReadInt("SKILL_DIG");
//         hitdie = loader.ReadInt("HITDIE");
//         exp = loader.ReadInt("EXP");
//         infravision = loader.ReadInt("INFRAVISION");
//         history = loader.ReadInt("HISTORY");
//         age = loader.ReadInt("AGE");
//         // base_hgt = loader.ReadInt("BASE_HGT");
//         // mod_hgt = loader.ReadInt("MOD_HGT");
//         // base_wgt = loader.ReadInt("BASE_WGT");
//         // mod_wgt = loader.ReadInt("MOD_WGT");
//     }
// }

public static class RacesTemp
{
    // public static List<RaceCfg> raceCfgs;

    // public static void Init()
    // {
    //     raceCfgs = CSVLoader.LoadCSV<RaceCfg>("Assets/Configs/races.csv");
    // }

    public static RaceTemplate GetRaceTemplate(Races race)
    {
        switch (race)
        {
            default:
            case Races.Breton:
                return new Breton();
            case Races.Redguard:
                return new Redguard();
            case Races.Nord:
                return new Nord();
            case Races.DarkElf:
                return new DarkElf();
            case Races.HighElf:
                return new HighElf();
            case Races.WoodElf:
                return new WoodElf();
            case Races.Khajiit:
                return new Khajiit();
            case Races.Argonian:
                return new Argonian();
        }
    }
}
