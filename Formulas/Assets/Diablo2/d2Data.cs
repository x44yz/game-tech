using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public struct MonsterData 
    {
        public string name;
        public string assetsSuffix;
        public string soundSuffix;
        public string trnFile;
        public MonsterAvailability availability;
        public int width;
        public int image;
        public bool hasSpecial;
        public bool hasSpecialSound;
        public int[] frames;
        public int[] rate;
        public int minDunLvl;
        public int maxDunLvl;
        public int level;
        public int hitPointsMinimum;
        public int hitPointsMaximum;
        public _mai_id ai;
        /**
        * @brief Denotes monster's abilities defined in @p monster_flag as bitflags
        * For usage, see @p MonstersData in monstdat.cpp
        */
        public int abilityFlags;
        public int intelligence;
        public int toHit;
        public int animFrameNum;
        public int minDamage;
        public int maxDamage;
        public int toHitSpecial;
        public int animFrameNumSpecial;
        public int minDamageSpecial;
        public int maxDamageSpecial;
        public int armorClass;
        public MonsterClass monsterClass;
        /** Using monster_resistance as bitflags */
        public int resistance;
        /** Using monster_resistance as bitflags */
        public int resistanceHell;
        public int selectionType; // TODO Create enum
        /** Using monster_treasure */
        public int treasure;
        public int exp;
    };

}
