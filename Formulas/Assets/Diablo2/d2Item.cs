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

        void RecreateTownItem(d2Player player, d2Item item, _item_indexes idx, int icreateinfo, int iseed)
        {
            if ((icreateinfo & (int)icreateinfo_flag.CF_SMITH) != 0)
                RecreateSmithItem(player, item, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
            // else if ((icreateinfo & (int)icreateinfo_flag.CF_SMITHPREMIUM) != 0)
            //     RecreatePremiumItem(player, item, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
            // else if ((icreateinfo & (int)icreateinfo_flag.CF_BOY) != 0)
            //     RecreateBoyItem(player, item, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
            // else if ((icreateinfo & (int)icreateinfo_flag.CF_WITCH) != 0)
            //     RecreateWitchItem(player, item, idx, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
            // else if ((icreateinfo & (int)icreateinfo_flag.CF_HEALER) != 0)
            //     RecreateHealerItem(player, item, idx, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
        }

        void RecreateSmithItem(d2Player player, d2Item item, int lvl, int iseed)
        {
            d2Utils.SetRndSeed(iseed);
            _item_indexes itype = RndSmithItem(player, lvl);
            GetItemAttrs(item, itype, lvl);

            item._iSeed = iseed;
            item._iCreateInfo = lvl | (int)icreateinfo_flag.CF_SMITH;
            item._iIdentified = true;
        }

        _item_indexes RndSmithItem(d2Player player, int lvl)
        {
            return RndVendorItem(SmithItemOk, true, player, 0, lvl);
        }

        bool SmithItemOk(d2Player player, int i)
        {
            if (d2Data.AllItemsList[i].itype == ItemType.Misc)
                return false;
            if (d2Data.AllItemsList[i].itype == ItemType.Gold)
                return false;
            if (d2Data.AllItemsList[i].itype == ItemType.Staff /*&& (!gbIsHellfire || IsValidSpell(AllItemsList[i].iSpell))*/)
                return false;
            if (d2Data.AllItemsList[i].itype == ItemType.Ring)
                return false;
            if (d2Data.AllItemsList[i].itype == ItemType.Amulet)
                return false;

            return true;
        }

        // template <bool (*Ok)(const Player &, int), bool ConsiderDropRate = false>
        _item_indexes RndVendorItem(Func<d2Player, int, bool> Ok, bool ConsiderDropRate, d2Player player, int minlvl, int maxlvl)
        {
            var ril = new List<_item_indexes>();

            for (int i = (int)_item_indexes.IDI_WARRIOR; i <= (int)_item_indexes.IDI_LAST; i++) 
            {
                if (!IsItemAvailable(i))
                    continue;
                if (d2Data.AllItemsList[i].iRnd == item_drop_rate.IDROP_NEVER)
                    continue;
                if (!Ok(player, i))
                    continue;
                if (d2Data.AllItemsList[i].iMinMLvl < minlvl || d2Data.AllItemsList[i].iMinMLvl > maxlvl)
                    continue;

                ril.Add((_item_indexes)i);
                if (ril.Count >= 512) // limit rnd count
                    break;

                if (!ConsiderDropRate || d2Data.AllItemsList[i].iRnd != item_drop_rate.IDROP_DOUBLE)
                    continue;

                ril.Add((_item_indexes)i);
                if (ril.Count >= 512) // limit rnd count
                    break;
            }

            return ril[d2Utils.GenerateRnd(ril.Count)];
        }

        bool IsItemAvailable(int i)
        {
            if (i < 0 || i > (int)_item_indexes.IDI_LAST)
                return false;

        //     if (gbIsSpawn) {
        //         if (i >= 62 && i <= 71)
        //             return false; // Medium and heavy armors
        //         if (IsAnyOf(i, 105, 107, 108, 110, 111, 113))
        //             return false; // Unavailable scrolls
        //     }

        //     if (gbIsHellfire)
        //         return true;

        //     return (
        //             i != IDI_MAPOFDOOM                   // Cathedral Map
        //             && i != IDI_LGTFORGE                 // Bovine Plate
        //             && (i < IDI_OIL || i > IDI_GREYSUIT) // Hellfire exclusive items
        //             && (i < 83 || i > 86)                // Oils
        //             && i != 92                           // Scroll of Search
        //             && (i < 161 || i > 165)              // Runes
        //             && i != IDI_SORCERER                 // Short Staff of Mana
        //             )
        //         || (
        //             // Bard items are technically Hellfire-exclusive
        //             // but are just normal items with adjusted stats.
        //             *sgOptions.Gameplay.testBard && IsAnyOf(i, IDI_BARDSWORD, IDI_BARDDAGGER));
        //
            return true; 
        }

        void GetItemAttrs(d2Item item, _item_indexes itemData, int lvl)
        {
            var baseItemData = d2Data.AllItemsList[(int)(itemData)];
            item._itype = baseItemData.itype;
            item._iCurs = baseItemData.iCurs;
            item._iName = baseItemData.iName;
            item._iIName = baseItemData.iName;
            item._iLoc = baseItemData.iLoc;
            item._iClass = baseItemData.iClass;
            item._iMinDam = baseItemData.iMinDam;
            item._iMaxDam = baseItemData.iMaxDam;
            item._iAC = baseItemData.iMinAC + d2Utils.GenerateRnd(baseItemData.iMaxAC - baseItemData.iMinAC + 1);
            item._iFlags = baseItemData.iFlags;
            item._iMiscId = baseItemData.iMiscId;
            item._iSpell = baseItemData.iSpell;
            item._iMagical = item_quality.ITEM_QUALITY_NORMAL;
            item._ivalue = baseItemData.iValue;
            item._iIvalue = baseItemData.iValue;
            item._iDurability = baseItemData.iDurability;
            item._iMaxDur = baseItemData.iDurability;
            item._iMinStr = baseItemData.iMinStr;
            item._iMinMag = baseItemData.iMinMag;
            item._iMinDex = baseItemData.iMinDex;
            item.IDidx = itemData;
            if (d2DEF.gbIsHellfire)
                item.dwBuff |= icreateinfo_flag2.CF_HELLFIRE;
            item._iPrePower = item_effect_type.IPL_INVALID;
            item._iSufPower = item_effect_type.IPL_INVALID;

            // if (item._iMiscId == item_misc_id.IMISC_BOOK)
            //     GetBookSpell(item, lvl);

            // if (d2DEF.gbIsHellfire && item._iMiscId == item_misc_id.IMISC_OILOF)
            //     GetOilType(item, lvl);

            if (item._itype != ItemType.Gold)
                return;

            // 产生的金币根据关卡和难度决定
            // int rndv;
            // int itemlevel = ItemsGetCurrlevel();
            // switch (sgGameInitInfo.nDifficulty) {
            // case DIFF_NORMAL:
            //     rndv = 5 * itemlevel + GenerateRnd(10 * itemlevel);
            //     break;
            // case DIFF_NIGHTMARE:
            //     rndv = 5 * (itemlevel + 16) + GenerateRnd(10 * (itemlevel + 16));
            //     break;
            // case DIFF_HELL:
            //     rndv = 5 * (itemlevel + 32) + GenerateRnd(10 * (itemlevel + 32));
            //     break;
            // }
            // if (leveltype == DTYPE_HELL)
            //     rndv += rndv / 8;

            // item._ivalue = std::min(rndv, GOLD_MAX_LIMIT);
            // SetPlrHandGoldCurs(item);
        }

        void SetupAllUseful(d2Item item, int iseed, int lvl)
        {
            item._iSeed = iseed;
            d2Utils.SetRndSeed(iseed);

            _item_indexes idx;

            if (d2DEF.gbIsHellfire) {
                switch (d2Utils.GenerateRnd(7)) {
                case 0:
                    idx = _item_indexes.IDI_PORTAL;
                    if (lvl <= 1)
                        idx = _item_indexes.IDI_HEAL;
                    break;
                case 1:
                case 2:
                    idx =_item_indexes. IDI_HEAL;
                    break;
                case 3:
                    idx = _item_indexes.IDI_PORTAL;
                    if (lvl <= 1)
                        idx = _item_indexes.IDI_MANA;
                    break;
                case 4:
                case 5:
                    idx = _item_indexes.IDI_MANA;
                    break;
                default:
                    idx = _item_indexes.IDI_OIL;
                    break;
                }
            } else {
                idx = d2Utils.PickRandomlyAmong<_item_indexes>(new List<_item_indexes>{ _item_indexes.IDI_MANA, _item_indexes.IDI_HEAL });

                if (lvl > 1 && d2Utils.FlipCoin(3))
                    idx = _item_indexes.IDI_PORTAL;
            }

            GetItemAttrs(item, idx, lvl);
            item._iCreateInfo = lvl | (int)icreateinfo_flag.CF_USEFUL;
            SetupItem(item);
        }

        void SetupItem(d2Item item)
        {
            // item.setNewAnimation(MyPlayer != nullptr && MyPlayer->pLvlLoad == 0);
            item._iIdentified = false;
        }

        void SetupAllItems(d2Player player, d2Item item, _item_indexes idx, int iseed, int lvl, int uper, bool onlygood, bool recreate, bool pregen)
        {
            item._iSeed = iseed;
            d2Utils.SetRndSeed(iseed);
            GetItemAttrs(item, idx, lvl / 2);
            item._iCreateInfo = lvl;

            if (pregen)
                item._iCreateInfo |= (int)icreateinfo_flag.CF_PREGEN;
            if (onlygood)
                item._iCreateInfo |= (int)icreateinfo_flag.CF_ONLYGOOD;

            if (uper == 15)
                item._iCreateInfo |= (int)icreateinfo_flag.CF_UPER15;
            else if (uper == 1)
                item._iCreateInfo |= (int)icreateinfo_flag.CF_UPER1;

            if (item._iMiscId != item_misc_id.IMISC_UNIQUE) {
                int iblvl = -1;
                if (d2Utils.GenerateRnd(100) <= 10 || d2Utils.GenerateRnd(100) <= lvl) {
                    iblvl = lvl;
                }
                if (iblvl == -1 && item._iMiscId == item_misc_id.IMISC_STAFF) {
                    iblvl = lvl;
                }
                if (iblvl == -1 && item._iMiscId == item_misc_id.IMISC_RING) {
                    iblvl = lvl;
                }
                if (iblvl == -1 && item._iMiscId == item_misc_id.IMISC_AMULET) {
                    iblvl = lvl;
                }
                if (onlygood)
                    iblvl = lvl;
                if (uper == 15)
                    iblvl = lvl + 4;
                if (iblvl != -1) {
                    _unique_items uid = CheckUnique(item, iblvl, uper, recreate);
                    if (uid == UITEM_INVALID) {
                        GetItemBonus(player, item, iblvl / 2, iblvl, onlygood, true);
                    } else {
                        GetUniqueItem(player, item, uid);
                    }
                }
                if (item._iMagical != ITEM_QUALITY_UNIQUE)
                    ItemRndDur(item);
            } else {
                if (item._iLoc != ILOC_UNEQUIPABLE) {
                    GetUniqueItem(player, item, (_unique_items)iseed); // uid is stored in iseed for uniques
                }
            }
            SetupItem(item);
        }
    }
}
