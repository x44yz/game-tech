using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    [Serializable]
    public class d2Item
    {
        public ItemType _itype;

        public item_drop_rate _iRnd;
        public item_class _iClass;
        public item_equip_type _iLoc;
        public item_cursor_graphic _iCurs;
        public unique_base_item _iItemId;
        public string _iName;
        public string _iIName;
        public int _iMinMLvl;
        public int _iDurability;
        public int _iMaxDur;
        public int _iMinDam;
        public int _iMaxDam;
        public int _iAC;
        public int _iMinAC;
        public int _iMaxAC;
        public int _iMinStr;
        public int _iMinMag;
        public int _iMinDex;
        public ItemSpecialEffect _iFlags; // ItemSpecialEffect as bit flags
        public item_misc_id _iMiscId;
        public spell_id _iSpell;
        public bool _iUsable;
        public int _iValue;
        public int _iCharges;
        public int _iMaxCharges;
        public int _ivalue;
        public int _iIvalue;
        public item_effect_type _iPrePower;
        public item_effect_type _iSufPower;
        public item_quality _iMagical;
        public _item_indexes IDidx;
        public icreateinfo_flag2 dwBuff;
        public int _iSeed;
        public bool _iStatFlag;
        public bool _iIdentified;
        public int _iPLDam = 0;
        public int _iPLToHit = 0;
        public int _iPLAC = 0;
        public int _iPLStr = 0;
        public int _iPLMag = 0;
        public int _iPLDex = 0;
        public int _iPLVit = 0;
        public int _iPLFR = 0;
        public int _iPLLR = 0;
        public int _iPLMR = 0;
        public int _iPLMana = 0;
        public int _iPLHP = 0;
        public int _iPLDamMod = 0;
        public int _iPLGetHit = 0;
        public int _iPLLight = 0;
        public ItemSpecialEffectHf _iDamAcFlags = ItemSpecialEffectHf.None;
        public int _iSplLvlAdd = 0;
        public int _iFMinDam = 0;
        public int _iFMaxDam = 0;
        public int _iLMinDam = 0;
        public int _iLMaxDam = 0;
        public int _iPLEnAc = 0;

        // public static d2Item Create(Transform parent)
        // {
        //     var obj = new GameObject();
        //     obj.transform.parent = parent;
        //     var item = obj.AddComponent<d2Item>();
        //     return item;
        // }

        public void clear()
        {
            _itype = ItemType.None;
        }

        public bool isEmpty()
        {
            // TODO
            return false;
        }
    }
}
