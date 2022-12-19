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
                    if (uid == _unique_items.UITEM_INVALID) {
                        GetItemBonus(player, item, iblvl / 2, iblvl, onlygood, true);
                    } else {
                        GetUniqueItem(player, item, uid);
                    }
                }
                if (item._iMagical != item_quality.ITEM_QUALITY_UNIQUE)
                    ItemRndDur(item);
            } else {
                if (item._iLoc != item_equip_type.ILOC_UNEQUIPABLE) {
                    GetUniqueItem(player, item, (_unique_items)iseed); // uid is stored in iseed for uniques
                }
            }
            SetupItem(item);
        }

        public static bool[] UniqueItemFlags = new bool[128];
        /** Unique item ID, used as an index into UniqueItemList */
        public int _iUid = 0;
        void GetUniqueItem(d2Player player, d2Item item, _unique_items uid)
        {
            UniqueItemFlags[(int)uid] = true;

            foreach (var power in d2Data.UniqueItems[(int)uid].powers) 
            {
                if (power.type == item_effect_type.IPL_INVALID)
                    break;
                SaveItemPower(player, item, power);
            }

            item._iIName = d2Data.UniqueItems[(int)uid].UIName;
            item._iIvalue = d2Data.UniqueItems[(int)uid].UIValue;

            if (item._iMiscId == item_misc_id.IMISC_UNIQUE)
                item._iSeed = (int)uid;

            item._iUid = (int)uid;
            item._iMagical = item_quality.ITEM_QUALITY_UNIQUE;
            item._iCreateInfo |= (int)icreateinfo_flag.CF_UNIQUE;
        }

        // Item indestructible durability
        public const int DUR_INDESTRUCTIBLE = 255;
        void ItemRndDur(d2Item item)
        {
            if (item._iDurability > 0 && item._iDurability != DUR_INDESTRUCTIBLE)
                item._iDurability = d2Utils.GenerateRnd(item._iMaxDur / 2) + (item._iMaxDur / 4) + 1;
        }

        _unique_items CheckUnique(d2Item item, int lvl, int uper, bool recreate)
        {
            return _unique_items.UITEM_INVALID;

            // std::bitset<128> uok = {};

            // if (d2Utils.GenerateRnd(100) > uper)
            //     return _unique_items.UITEM_INVALID;

            // int numu = 0;
            // for (int j = 0; UniqueItems[j].UIItemId != UITYPE_INVALID; j++) {
            //     if (!IsUniqueAvailable(j))
            //         break;
            //     if (UniqueItems[j].UIItemId == AllItemsList[item.IDidx].iItemId
            //         && lvl >= UniqueItems[j].UIMinLvl
            //         && (recreate || !UniqueItemFlags[j] || gbIsMultiplayer)) {
            //         uok[j] = true;
            //         numu++;
            //     }
            // }

            // if (numu == 0)
            //     return _unique_items.UITEM_INVALID;

            // d2Utils.AdvanceRndSeed();
            // int itemData = 0;
            // while (numu > 0) {
            //     if (uok[itemData])
            //         numu--;
            //     if (numu > 0)
            //         itemData = (itemData + 1) % 128;
            // }

            // return (_unique_items)itemData;
        }

        // item 加成
        void GetItemBonus(d2Player player, d2Item item, int minlvl, int maxlvl, bool onlygood, bool allowspells)
        {
            if (minlvl > 25)
                minlvl = 25;

            switch (item._itype) {
            case ItemType.Sword:
            case ItemType.Axe:
            case ItemType.Mace:
                GetItemPower(player, item, minlvl, maxlvl, AffixItemType.Weapon, onlygood);
                break;
            case ItemType.Bow:
                GetItemPower(player, item, minlvl, maxlvl, AffixItemType.Bow, onlygood);
                break;
            case ItemType.Shield:
                GetItemPower(player, item, minlvl, maxlvl, AffixItemType.Shield, onlygood);
                break;
            case ItemType.LightArmor:
            case ItemType.Helm:
            case ItemType.MediumArmor:
            case ItemType.HeavyArmor:
                GetItemPower(player, item, minlvl, maxlvl, AffixItemType.Armor, onlygood);
                break;
            case ItemType.Staff:
                if (allowspells)
                    GetStaffSpell(player, item, maxlvl, onlygood);
                else
                    GetItemPower(player, item, minlvl, maxlvl, AffixItemType.Staff, onlygood);
                break;
            case ItemType.Ring:
            case ItemType.Amulet:
                GetItemPower(player, item, minlvl, maxlvl, AffixItemType.Misc, onlygood);
                break;
            case ItemType.None:
            case ItemType.Misc:
            case ItemType.Gold:
                break;
            }
        }

        void GetItemPower(d2Player player, d2Item item, int minlvl, int maxlvl, AffixItemType flgs, bool onlygood)
        {
            int[] l = new int[256];
            goodorevil goe;

            bool allocatePrefix = d2Utils.FlipCoin(4);
            bool allocateSuffix = !d2Utils.FlipCoin(3);
            if (!allocatePrefix && !allocateSuffix) {
                // At least try and give each item a prefix or suffix
                if (d2Utils.FlipCoin())
                    allocatePrefix = true;
                else
                    allocateSuffix = true;
            }
            int preidx = -1;
            int sufidx = -1;
            goe = goodorevil.GOE_ANY;
            if (!onlygood && !d2Utils.FlipCoin(3))
                onlygood = true;
            if (allocatePrefix) {
                int nt = 0;
                for (int j = 0; d2Data.ItemPrefixes[j].power.type != item_effect_type.IPL_INVALID; j++) {
                    if (!IsPrefixValidForItemType(j, flgs))
                        continue;
                    if (d2Data.ItemPrefixes[j].PLMinLvl < minlvl || d2Data.ItemPrefixes[j].PLMinLvl > maxlvl)
                        continue;
                    if (onlygood && !d2Data.ItemPrefixes[j].PLOk)
                        continue;
                    if (d2Utils.HasAnyOf(flgs, AffixItemType.Staff) && d2Data.ItemPrefixes[j].power.type == item_effect_type.IPL_CHARGES)
                        continue;
                    l[nt] = j;
                    nt++;
                    if (d2Data.ItemPrefixes[j].PLDouble) {
                        l[nt] = j;
                        nt++;
                    }
                }
                if (nt != 0) {
                    preidx = l[d2Utils.GenerateRnd(nt)];
                    item._iMagical = item_quality.ITEM_QUALITY_MAGIC;
                    SaveItemAffix(player, item, d2Data.ItemPrefixes[preidx]);
                    item._iPrePower = d2Data.ItemPrefixes[preidx].power.type;
                    goe = d2Data.ItemPrefixes[preidx].PLGOE;
                }
            }
            if (allocateSuffix) {
                int nl = 0;
                for (int j = 0; d2Data.ItemSuffixes[j].power.type != item_effect_type.IPL_INVALID; j++) {
                    if (IsSuffixValidForItemType(j, flgs)
                        && d2Data.ItemSuffixes[j].PLMinLvl >= minlvl && d2Data.ItemSuffixes[j].PLMinLvl <= maxlvl
                        && !((goe == goodorevil.GOE_GOOD && d2Data.ItemSuffixes[j].PLGOE == goodorevil.GOE_EVIL) || (goe == goodorevil.GOE_EVIL && d2Data.ItemSuffixes[j].PLGOE == goodorevil.GOE_GOOD))
                        && (!onlygood || d2Data.ItemSuffixes[j].PLOk)) {
                        l[nl] = j;
                        nl++;
                    }
                }
                if (nl != 0) {
                    sufidx = l[d2Utils.GenerateRnd(nl)];
                    item._iMagical = item_quality.ITEM_QUALITY_MAGIC;
                    SaveItemAffix(player, item, d2Data.ItemSuffixes[sufidx]);
                    item._iSufPower = d2Data.ItemSuffixes[sufidx].power.type;
                }
            }

            item._iIName = GenerateMagicItemName(item._iName, preidx, sufidx);
            if (!StringInPanel(item._iIName)) {
                CopyUtf8(item._iIName, GenerateMagicItemName(_(d2Data.AllItemsList[(int)item.IDidx].iSName), preidx, sufidx), sizeof(item._iIName));
            }
            if (preidx != -1 || sufidx != -1)
                CalcItemValue(item);
        }

        int SaveItemPower(d2Player player, d2Item item, ItemPower power)
        {
            if (!d2DEF.gbIsHellfire) {
                if (power.type == item_effect_type.IPL_TARGAC) {
                    power.param1 = 1 << power.param1;
                    power.param2 = 3 << power.param2;
                }
            }

            int r = RndPL(power.param1, power.param2);

            switch (power.type) {
            case item_effect_type.IPL_TOHIT:
                item._iPLToHit += r;
                break;
            case item_effect_type.IPL_TOHIT_CURSE:
                item._iPLToHit -= r;
                break;
            case item_effect_type.IPL_DAMP:
                item._iPLDam += r;
                break;
            case item_effect_type.IPL_DAMP_CURSE:
                item._iPLDam -= r;
                break;
            case item_effect_type.IPL_DOPPELGANGER:
                item._iDamAcFlags |= ItemSpecialEffectHf.Doppelganger;
                break;
            case item_effect_type.IPL_TOHIT_DAMP:
                r = RndPL(power.param1, power.param2);
                item._iPLDam += r;
                item._iPLToHit += CalculateToHitBonus(power.param1);
                break;
            case item_effect_type.IPL_TOHIT_DAMP_CURSE:
                item._iPLDam -= r;
                item._iPLToHit += CalculateToHitBonus(-power.param1);
                break;
            case item_effect_type.IPL_ACP:
                item._iPLAC += r;
                break;
            case item_effect_type.IPL_ACP_CURSE:
                item._iPLAC -= r;
                break;
            case item_effect_type.IPL_SETAC:
                item._iAC = r;
                break;
            case item_effect_type.IPL_AC_CURSE:
                item._iAC -= r;
                break;
            case item_effect_type.IPL_FIRERES:
                item._iPLFR += r;
                break;
            case item_effect_type.IPL_LIGHTRES:
                item._iPLLR += r;
                break;
            case item_effect_type.IPL_MAGICRES:
                item._iPLMR += r;
                break;
            case item_effect_type.IPL_ALLRES:
                item._iPLFR = Math.Max(item._iPLFR + r, 0);
                item._iPLLR = Math.Max(item._iPLLR + r, 0);
                item._iPLMR = Math.Max(item._iPLMR + r, 0);
                break;
            case item_effect_type.IPL_SPLLVLADD:
                item._iSplLvlAdd = r;
                break;
            case item_effect_type.IPL_CHARGES:
                item._iCharges *= power.param1;
                item._iMaxCharges = item._iCharges;
                break;
            case item_effect_type.IPL_SPELL:
                item._iSpell = (spell_id)(power.param1);
                item._iCharges = power.param2;
                item._iMaxCharges = power.param2;
                break;
            case item_effect_type.IPL_FIREDAM:
                item._iFlags |= ItemSpecialEffect.FireDamage;
                item._iFlags &= ~ItemSpecialEffect.LightningDamage;
                item._iFMinDam = power.param1;
                item._iFMaxDam = power.param2;
                item._iLMinDam = 0;
                item._iLMaxDam = 0;
                break;
            case item_effect_type.IPL_LIGHTDAM:
                item._iFlags |= ItemSpecialEffect.LightningDamage;
                item._iFlags &= ~ItemSpecialEffect.FireDamage;
                item._iLMinDam = power.param1;
                item._iLMaxDam = power.param2;
                item._iFMinDam = 0;
                item._iFMaxDam = 0;
                break;
            case item_effect_type.IPL_STR:
                item._iPLStr += r;
                break;
            case item_effect_type.IPL_STR_CURSE:
                item._iPLStr -= r;
                break;
            case item_effect_type.IPL_MAG:
                item._iPLMag += r;
                break;
            case item_effect_type.IPL_MAG_CURSE:
                item._iPLMag -= r;
                break;
            case item_effect_type.IPL_DEX:
                item._iPLDex += r;
                break;
            case item_effect_type.IPL_DEX_CURSE:
                item._iPLDex -= r;
                break;
            case item_effect_type.IPL_VIT:
                item._iPLVit += r;
                break;
            case item_effect_type.IPL_VIT_CURSE:
                item._iPLVit -= r;
                break;
            case item_effect_type.IPL_ATTRIBS:
                item._iPLStr += r;
                item._iPLMag += r;
                item._iPLDex += r;
                item._iPLVit += r;
                break;
            case item_effect_type.IPL_ATTRIBS_CURSE:
                item._iPLStr -= r;
                item._iPLMag -= r;
                item._iPLDex -= r;
                item._iPLVit -= r;
                break;
            case item_effect_type.IPL_GETHIT_CURSE:
                item._iPLGetHit += r;
                break;
            case item_effect_type.IPL_GETHIT:
                item._iPLGetHit -= r;
                break;
            case item_effect_type.IPL_LIFE:
                item._iPLHP += r << 6;
                break;
            case item_effect_type.IPL_LIFE_CURSE:
                item._iPLHP -= r << 6;
                break;
            case item_effect_type.IPL_MANA:
                item._iPLMana += r << 6;
                // RedrawComponent(PanelDrawComponent::Mana);
                break;
            case item_effect_type.IPL_MANA_CURSE:
                item._iPLMana -= r << 6;
                // RedrawComponent(PanelDrawComponent::Mana);
                break;
            case item_effect_type.IPL_DUR: {
                int bonus = r * item._iMaxDur / 100;
                item._iMaxDur += bonus;
                item._iDurability += bonus;
            } break;
            case item_effect_type.IPL_CRYSTALLINE:
                item._iPLDam += 140 + r * 2;
                break;
            case item_effect_type.IPL_DUR_CURSE:
                item._iMaxDur -= r * item._iMaxDur / 100;
                item._iMaxDur = Math.Max(item._iMaxDur, 1);
                item._iDurability = item._iMaxDur;
                break;
            case item_effect_type.IPL_INDESTRUCTIBLE:
                item._iDurability = DUR_INDESTRUCTIBLE;
                item._iMaxDur = DUR_INDESTRUCTIBLE;
                break;
            case item_effect_type.IPL_LIGHT:
                item._iPLLight += power.param1;
                break;
            case item_effect_type.IPL_LIGHT_CURSE:
                item._iPLLight -= power.param1;
                break;
            case item_effect_type.IPL_MULT_ARROWS:
                item._iFlags |= ItemSpecialEffect.MultipleArrows;
                break;
            case item_effect_type.IPL_FIRE_ARROWS:
                item._iFlags |= ItemSpecialEffect.FireArrows;
                item._iFlags &= ~ItemSpecialEffect.LightningArrows;
                item._iFMinDam = power.param1;
                item._iFMaxDam = power.param2;
                item._iLMinDam = 0;
                item._iLMaxDam = 0;
                break;
            case item_effect_type.IPL_LIGHT_ARROWS:
                item._iFlags |= ItemSpecialEffect.LightningArrows;
                item._iFlags &= ~ItemSpecialEffect.FireArrows;
                item._iLMinDam = power.param1;
                item._iLMaxDam = power.param2;
                item._iFMinDam = 0;
                item._iFMaxDam = 0;
                break;
            case item_effect_type.IPL_FIREBALL:
                item._iFlags |= (ItemSpecialEffect.LightningArrows | ItemSpecialEffect.FireArrows);
                item._iFMinDam = power.param1;
                item._iFMaxDam = power.param2;
                item._iLMinDam = 0;
                item._iLMaxDam = 0;
                break;
            case item_effect_type.IPL_THORNS:
                item._iFlags |= ItemSpecialEffect.Thorns;
                break;
            case item_effect_type.IPL_NOMANA:
                item._iFlags |= ItemSpecialEffect.NoMana;
                // RedrawComponent(PanelDrawComponent::Mana);
                break;
            case item_effect_type.IPL_ABSHALFTRAP:
                item._iFlags |= ItemSpecialEffect.HalfTrapDamage;
                break;
            case item_effect_type.IPL_KNOCKBACK:
                item._iFlags |= ItemSpecialEffect.Knockback;
                break;
            case item_effect_type.IPL_3XDAMVDEM:
                item._iFlags |= ItemSpecialEffect.TripleDemonDamage;
                break;
            case item_effect_type.IPL_ALLRESZERO:
                item._iFlags |= ItemSpecialEffect.ZeroResistance;
                break;
            case item_effect_type.IPL_STEALMANA:
                if (power.param1 == 3)
                    item._iFlags |= ItemSpecialEffect.StealMana3;
                if (power.param1 == 5)
                    item._iFlags |= ItemSpecialEffect.StealMana5;
                // RedrawComponent(PanelDrawComponent::Mana);
                break;
            case item_effect_type.IPL_STEALLIFE:
                if (power.param1 == 3)
                    item._iFlags |= ItemSpecialEffect.StealLife3;
                if (power.param1 == 5)
                    item._iFlags |= ItemSpecialEffect.StealLife5;
                // RedrawComponent(PanelDrawComponent::Health);
                break;
            case item_effect_type.IPL_TARGAC:
                if (d2DEF.gbIsHellfire)
                    item._iPLEnAc = power.param1;
                else
                    item._iPLEnAc += r;
                break;
            case item_effect_type.IPL_FASTATTACK:
                if (power.param1 == 1)
                    item._iFlags |= ItemSpecialEffect.QuickAttack;
                if (power.param1 == 2)
                    item._iFlags |= ItemSpecialEffect.FastAttack;
                if (power.param1 == 3)
                    item._iFlags |= ItemSpecialEffect.FasterAttack;
                if (power.param1 == 4)
                    item._iFlags |= ItemSpecialEffect.FastestAttack;
                break;
            case item_effect_type.IPL_FASTRECOVER:
                if (power.param1 == 1)
                    item._iFlags |= ItemSpecialEffect.FastHitRecovery;
                if (power.param1 == 2)
                    item._iFlags |= ItemSpecialEffect.FasterHitRecovery;
                if (power.param1 == 3)
                    item._iFlags |= ItemSpecialEffect.FastestHitRecovery;
                break;
            case item_effect_type.IPL_FASTBLOCK:
                item._iFlags |= ItemSpecialEffect.FastBlock;
                break;
            case item_effect_type.IPL_DAMMOD:
                item._iPLDamMod += r;
                break;
            case item_effect_type.IPL_RNDARROWVEL:
                item._iFlags |= ItemSpecialEffect.RandomArrowVelocity;
                break;
            case item_effect_type.IPL_SETDAM:
                item._iMinDam = power.param1;
                item._iMaxDam = power.param2;
                break;
            case item_effect_type.IPL_SETDUR:
                item._iDurability = power.param1;
                item._iMaxDur = power.param1;
                break;
            case item_effect_type.IPL_ONEHAND:
                item._iLoc = item_equip_type.ILOC_ONEHAND;
                break;
            case item_effect_type.IPL_DRAINLIFE:
                item._iFlags |= ItemSpecialEffect.DrainLife;
                break;
            case item_effect_type.IPL_RNDSTEALLIFE:
                item._iFlags |= ItemSpecialEffect.RandomStealLife;
                break;
            case item_effect_type.IPL_NOMINSTR:
                item._iMinStr = 0;
                break;
            case item_effect_type.IPL_INVCURS:
                item._iCurs = (item_cursor_graphic)power.param1;
                break;
            case item_effect_type.IPL_ADDACLIFE:
                item._iFlags |= (ItemSpecialEffect.LightningArrows | ItemSpecialEffect.FireArrows);
                item._iFMinDam = power.param1;
                item._iFMaxDam = power.param2;
                item._iLMinDam = 1;
                item._iLMaxDam = 0;
                break;
            case item_effect_type.IPL_ADDMANAAC:
                item._iFlags |= (ItemSpecialEffect.LightningDamage | ItemSpecialEffect.FireDamage);
                item._iFMinDam = power.param1;
                item._iFMaxDam = power.param2;
                item._iLMinDam = 2;
                item._iLMaxDam = 0;
                break;
            case item_effect_type.IPL_FIRERES_CURSE:
                item._iPLFR -= r;
                break;
            case item_effect_type.IPL_LIGHTRES_CURSE:
                item._iPLLR -= r;
                break;
            case item_effect_type.IPL_MAGICRES_CURSE:
                item._iPLMR -= r;
                break;
            case item_effect_type.IPL_DEVASTATION:
                item._iDamAcFlags |= ItemSpecialEffectHf.Devastation;
                break;
            case item_effect_type.IPL_DECAY:
                item._iDamAcFlags |= ItemSpecialEffectHf.Decay;
                item._iPLDam += r;
                break;
            case item_effect_type.IPL_PERIL:
                item._iDamAcFlags |= ItemSpecialEffectHf.Peril;
                break;
            case item_effect_type.IPL_JESTERS:
                item._iDamAcFlags |= ItemSpecialEffectHf.Jesters;
                break;
            case item_effect_type.IPL_ACDEMON:
                item._iDamAcFlags |= ItemSpecialEffectHf.ACAgainstDemons;
                break;
            case item_effect_type.IPL_ACUNDEAD:
                item._iDamAcFlags |= ItemSpecialEffectHf.ACAgainstUndead;
                break;
            case item_effect_type.IPL_MANATOLIFE: {
                int portion = ((player._pMaxManaBase >> 6) * 50 / 100) << 6;
                item._iPLMana -= portion;
                item._iPLHP += portion;
            } break;
            case item_effect_type.IPL_LIFETOMANA: {
                int portion = ((player._pMaxHPBase >> 6) * 40 / 100) << 6;
                item._iPLHP -= portion;
                item._iPLMana += portion;
            } break;
            default:
                break;
            }

            return r;
        }

        public const int MAX_SPELLS = 52;
        void GetStaffSpell(d2Player player, d2Item item, int lvl, bool onlygood)
        {
            if (!d2DEF.gbIsHellfire && d2Utils.FlipCoin(4)) {
                GetItemPower(player, item, lvl / 2, lvl, AffixItemType.Staff, onlygood);
                return;
            }

            int maxSpells = d2DEF.gbIsHellfire ? MAX_SPELLS : 37;
            int l = lvl / 2;
            if (l == 0)
                l = 1;
            int rv = d2Utils.GenerateRnd(maxSpells) + 1;

            if (d2DEF.gbIsSpawn && lvl > 10)
                lvl = 10;

            int s = (int)spell_id.SPL_FIREBOLT;
            spell_id bs = spell_id.SPL_NULL;
            while (rv > 0) {
                int sLevel = GetSpellStaffLevel(static_cast<spell_id>(s));
                if (sLevel != -1 && l >= sLevel) {
                    rv--;
                    bs = (spell_id)(s);
                }
                s++;
                if (!d2DEF.gbIsMultiplayer && s == (int)spell_id.SPL_RESURRECT)
                    s = (int)spell_id.SPL_TELEKINESIS;
                if (!d2DEF.gbIsMultiplayer && s == (int)spell_id.SPL_HEALOTHER)
                    s = (int)spell_id.SPL_FLARE;
                if (s == maxSpells)
                    s = (int)spell_id.SPL_FIREBOLT;
            }

            int minc = spelldata[bs].sStaffMin;
            int maxc = spelldata[bs].sStaffMax - minc + 1;
            item._iSpell = bs;
            item._iCharges = minc + d2Utils.GenerateRnd(maxc);
            item._iMaxCharges = item._iCharges;

            item._iMinMag = spelldata[bs].sMinInt;
            int v = item._iCharges * spelldata[bs].sStaffCost / 5;
            item._ivalue += v;
            item._iIvalue += v;
            GetStaffPower(player, item, lvl, bs, onlygood);
        }
    }
}
