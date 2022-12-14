using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    [System.Serializable]
    public class CharStatsCfg
    {
        public string className;
        public int str;
        public int dex;
        public int energy;
        public int vit;
        public int tot;
        public int stamina;
        public int hpAdd;
        public int percentStr;
        public int percentDex;
        public int percentInt;
        public int percentVit;
        public int manaRegen;
        public int toHitFactor;
        public int walkVelocity;
        public int runVelocity;
        public int runDrain;
        public string comment;
        public int lifePerLevel;
        public int staminaPerLevel;
        public int manaPerLevel;
        public int lifePerVitality;
        public int staminPerVitality;
        public int manaPerMagic;
        public int statPerLevel;
        public int refWalk;
        public int refRun;
        public int refSwing;
        public int refSpell;
        public int refGetHit;
        public int refBow;
        public int blockFactor;
        public string startSkill;
        public string[] skills;
        public string strAllSkills;
        public string strSkillTab1;
        public string strSkillTab2;
        public string strSkillTab3;
        public string strClassOnly;
        public string baseWClass;
        public StartingItem[] startingItems;
        [System.NonSerialized]
        public string token;
        [System.NonSerialized]
        public string code;
        [System.NonSerialized]
        public string classNameLower;


        public static List<CharStatsCfg> sheet;

        public static CharStatsCfg Find(string className)
        {
            return sheet.Find(info => info.className == className);
        }

        public static CharStatsCfg FindByCode(string code)
        {
            return sheet.Find(info => info.code == code);
        }

        [System.Serializable]
        public struct StartingItem
        {
            public string code;
            public string loc;
            public int count;

            public static void LoadRecord(ref CharStatsCfg.StartingItem record, DatasheetStream stream)
            {
                stream.Read(ref record.code);
                stream.Read(ref record.loc);
                stream.Read(ref record.count);
            }
        }

        static Dictionary<string, string> tokens = new Dictionary<string, string>
        {
            { "Amazon", "AM" },
            { "Sorceress", "SO" },
            { "Necromancer", "NE" },
            { "Paladin", "PA" },
            { "Barbarian", "BA" },
            { "Druid", "DZ" },
            { "Assassin", "AI" },
        };

        static Dictionary<string, string> codes = new Dictionary<string, string>
        {
            { "Amazon", "ama" },
            { "Sorceress", "sor" },
            { "Necromancer", "nec" },
            { "Paladin", "pal" },
            { "Barbarian", "bar" },
            { "Druid", "dru" },
            { "Assassin", "ass" },
        };

        public static void Load()
        {
            var filename = Application.dataPath + "/Diablo2/Configs/CharStats.csv";
            sheet = DataMgr.Load<CharStatsCfg>(filename, 1, LoadRecord);
            sheet.RemoveAll(row => row.baseWClass == null);
            foreach(var info in sheet)
            {
                info.token = tokens[info.className];
                info.code = codes[info.className];
                info.classNameLower = info.className.ToLower();
            }
        }

        public static void LoadRecord(CharStatsCfg record, DatasheetStream stream)
        {
                    stream.Read(ref record.className);
                    stream.Read(ref record.str);
                    stream.Read(ref record.dex);
                    stream.Read(ref record.energy);
                    stream.Read(ref record.vit);
                    stream.Read(ref record.tot);
                    stream.Read(ref record.stamina);
                    stream.Read(ref record.hpAdd);
                    stream.Read(ref record.percentStr);
                    stream.Read(ref record.percentDex);
                    stream.Read(ref record.percentInt);
                    stream.Read(ref record.percentVit);
                    stream.Read(ref record.manaRegen);
                    stream.Read(ref record.toHitFactor);
                    stream.Read(ref record.walkVelocity);
                    stream.Read(ref record.runVelocity);
                    stream.Read(ref record.runDrain);
                    stream.Read(ref record.comment);
                    stream.Read(ref record.lifePerLevel);
                    stream.Read(ref record.staminaPerLevel);
                    stream.Read(ref record.manaPerLevel);
                    stream.Read(ref record.lifePerVitality);
                    stream.Read(ref record.staminPerVitality);
                    stream.Read(ref record.manaPerMagic);
                    stream.Read(ref record.statPerLevel);
                    stream.Read(ref record.refWalk);
                    stream.Read(ref record.refRun);
                    stream.Read(ref record.refSwing);
                    stream.Read(ref record.refSpell);
                    stream.Read(ref record.refGetHit);
                    stream.Read(ref record.refBow);
                    stream.Read(ref record.blockFactor);
                    stream.Read(ref record.startSkill);
                    record.skills = new string[10];
                        stream.Read(ref record.skills[0]);
                        stream.Read(ref record.skills[1]);
                        stream.Read(ref record.skills[2]);
                        stream.Read(ref record.skills[3]);
                        stream.Read(ref record.skills[4]);
                        stream.Read(ref record.skills[5]);
                        stream.Read(ref record.skills[6]);
                        stream.Read(ref record.skills[7]);
                        stream.Read(ref record.skills[8]);
                        stream.Read(ref record.skills[9]);
                    stream.Read(ref record.strAllSkills);
                    stream.Read(ref record.strSkillTab1);
                    stream.Read(ref record.strSkillTab2);
                    stream.Read(ref record.strSkillTab3);
                    stream.Read(ref record.strClassOnly);
                    stream.Read(ref record.baseWClass);
                    record.startingItems = new CharStatsCfg.StartingItem[10];
                        StartingItem.LoadRecord(ref record.startingItems[0], stream);
                        StartingItem.LoadRecord(ref record.startingItems[1], stream);
                        StartingItem.LoadRecord(ref record.startingItems[2], stream);
                        StartingItem.LoadRecord(ref record.startingItems[3], stream);
                        StartingItem.LoadRecord(ref record.startingItems[4], stream);
                        StartingItem.LoadRecord(ref record.startingItems[5], stream);
                        StartingItem.LoadRecord(ref record.startingItems[6], stream);
                        StartingItem.LoadRecord(ref record.startingItems[7], stream);
                        StartingItem.LoadRecord(ref record.startingItems[8], stream);
                        StartingItem.LoadRecord(ref record.startingItems[9], stream);
        }
    }
}
