using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    /*
    First 5 bits store level
    6th bit stores onlygood flag
    7th bit stores uper15 flag - uper means unique percent, this flag is true for unique monsters and loot from them has 15% to become unique
    8th bit stores uper1 flag - this is loot from normal monsters, which has 1% to become unique
    9th bit stores info if item is unique
    10th bit stores info if item is a basic one from griswold
    11th bit stores info if item is a premium one from griswold
    12th bit stores info if item is from wirt
    13th bit stores info if item is from adria
    14th bit stores info if item is from pepin
    15th bit stores pregen flag

    combining CF_UPER15 and CF_UPER1 flags (CF_USEFUL) is used to mark potions and town portal scrolls created on the ground
    CF_TOWN is combining all store flags and indicates if item has been bought from a NPC
    */
    enum icreateinfo_flag {
        // clang-format off
        CF_LEVEL        = (1 << 6) - 1,
        CF_ONLYGOOD     = 1 << 6,
        CF_UPER15       = 1 << 7,
        CF_UPER1        = 1 << 8,
        CF_UNIQUE       = 1 << 9,
        CF_SMITH        = 1 << 10,
        CF_SMITHPREMIUM = 1 << 11,
        CF_BOY          = 1 << 12,
        CF_WITCH        = 1 << 13,
        CF_HEALER       = 1 << 14,
        CF_PREGEN       = 1 << 15,

        CF_USEFUL = CF_UPER15 | CF_UPER1,
        CF_TOWN   = CF_SMITH | CF_SMITHPREMIUM | CF_BOY | CF_WITCH | CF_HEALER,
        // clang-format on
    };

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

        public void InitializeItem(_item_indexes itemData)
        {
            d2Item item = this;

            var pAllItem = d2Data.AllItemsList[((int)itemData)];

            // zero-initialize struct

            item._itype = pAllItem.itype;
            item._iCurs = pAllItem.iCurs;
            item._iName = pAllItem.iName;
            item._iIName = pAllItem.iName;
            item._iLoc = pAllItem.iLoc;
            item._iClass = pAllItem.iClass;
            item._iMinDam = pAllItem.iMinDam;
            item._iMaxDam = pAllItem.iMaxDam;
            item._iAC = pAllItem.iMinAC;
            item._iMiscId = pAllItem.iMiscId;
            item._iSpell = pAllItem.iSpell;

            if (pAllItem.iMiscId == item_misc_id.IMISC_STAFF) {
                item._iCharges = d2DEF.gbIsHellfire ? 18 : 40;
            }

            item._iMaxCharges = item._iCharges;
            item._iDurability = pAllItem.iDurability;
            item._iMaxDur = pAllItem.iDurability;
            item._iMinStr = pAllItem.iMinStr;
            item._iMinMag = pAllItem.iMinMag;
            item._iMinDex = pAllItem.iMinDex;
            item._ivalue = pAllItem.iValue;
            item._iIvalue = pAllItem.iValue;
            item._iPrePower = item_effect_type.IPL_INVALID;
            item._iSufPower = item_effect_type.IPL_INVALID;
            item._iMagical = item_quality.ITEM_QUALITY_NORMAL;
            item.IDidx = itemData;
            if (d2DEF.gbIsHellfire)
                item.dwBuff |= icreateinfo_flag2.CF_HELLFIRE;
        }

        public int _iCreateInfo;
        public void RecreateItem(d2Player player, d2Item item, _item_indexes idx, int icreateinfo, int iseed, int ivalue)
        {
            if (idx == _item_indexes.IDI_GOLD) {
                item.InitializeItem(_item_indexes.IDI_GOLD);
                item._iSeed = iseed;
                item._iCreateInfo = icreateinfo;
                item._ivalue = ivalue;
                // SetPlrHandGoldCurs(item);
                // gbIsHellfire = tmpIsHellfire;
                return;
            }

            if (icreateinfo == 0) {
                item.InitializeItem(idx);
                item._iSeed = iseed;
                // gbIsHellfire = tmpIsHellfire;
                return;
            }

            if ((icreateinfo & (int)icreateinfo_flag.CF_UNIQUE) == 0) 
            {
                if ((icreateinfo & (int)icreateinfo_flag.CF_TOWN) != 0) {
                    RecreateTownItem(player, item, idx, icreateinfo, iseed);
                    // gbIsHellfire = tmpIsHellfire;
                    return;
                }

                if ((icreateinfo & (int)icreateinfo_flag.CF_USEFUL) == (int)icreateinfo_flag.CF_USEFUL) {
                    SetupAllUseful(item, iseed, icreateinfo & (int)icreateinfo_flag.CF_LEVEL);
                    // gbIsHellfire = tmpIsHellfire;
                    return;
                }
            }

            int level = icreateinfo & (int)icreateinfo_flag.CF_LEVEL;

            int uper = 0;
            if ((icreateinfo & (int)icreateinfo_flag.CF_UPER1) != 0)
                uper = 1;
            if ((icreateinfo & (int)icreateinfo_flag.CF_UPER15) != 0)
                uper = 15;

            bool onlygood = (icreateinfo & (int)icreateinfo_flag.CF_ONLYGOOD) != 0;
            bool recreate = (icreateinfo & (int)icreateinfo_flag.CF_UNIQUE) != 0;
            bool pregen = (icreateinfo & (int)icreateinfo_flag.CF_PREGEN) != 0;

            SetupAllItems(player, item, idx, iseed, level, uper, onlygood, recreate, pregen);
        }

    }
}
