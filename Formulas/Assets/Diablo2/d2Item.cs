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
        public int _iDurability; // 耐久
        public int _iMaxDur; // 最大耐久
        public int _iMinDam;
        public int _iMaxDam;
        public int _iAC; // accuracy 精准度
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
        public _item_indexes IDidx = _item_indexes.IDI_NONE;
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
            return _itype == ItemType.None;
        }

        public void InitializeItem(_item_indexes itemData)
        {
            d2Item item = this;

            var pAllItem = d2Data.AllItemsList[((int)itemData)];

            // zero-initialize struct

            _itype = pAllItem.itype;
            _iCurs = pAllItem.iCurs;
            _iName = pAllItem.iName;
            _iIName = pAllItem.iName;
            _iLoc = pAllItem.iLoc;
            _iClass = pAllItem.iClass;
            _iMinDam = pAllItem.iMinDam;
            _iMaxDam = pAllItem.iMaxDam;
            _iAC = pAllItem.iMinAC;
            _iMiscId = pAllItem.iMiscId;
            _iSpell = pAllItem.iSpell;

            if (pAllItem.iMiscId == item_misc_id.IMISC_STAFF) {
                _iCharges = d2DEF.gbIsHellfire ? 18 : 40;
            }

            _iMaxCharges = _iCharges;
            _iDurability = pAllItem.iDurability;
            _iMaxDur = pAllItem.iDurability;
            _iMinStr = pAllItem.iMinStr;
            _iMinMag = pAllItem.iMinMag;
            _iMinDex = pAllItem.iMinDex;
            _ivalue = pAllItem.iValue;
            _iIvalue = pAllItem.iValue;
            _iPrePower = item_effect_type.IPL_INVALID;
            _iSufPower = item_effect_type.IPL_INVALID;
            _iMagical = item_quality.ITEM_QUALITY_NORMAL;
            IDidx = itemData;
            if (d2DEF.gbIsHellfire)
                dwBuff |= icreateinfo_flag2.CF_HELLFIRE;
        }

        public int _iCreateInfo;
        public void RecreateItem(d2Player player, _item_indexes idx, int icreateinfo, int iseed, int ivalue)
        {
            if (idx == _item_indexes.IDI_GOLD) {
                InitializeItem(_item_indexes.IDI_GOLD);
                _iSeed = iseed;
                _iCreateInfo = icreateinfo;
                _ivalue = ivalue;
                // SetPlrHandGoldCurs(item);
                // gbIsHellfire = tmpIsHellfire;
                return;
            }

            if (icreateinfo == 0) {
                InitializeItem(idx);
                _iSeed = iseed;
                // gbIsHellfire = tmpIsHellfire;
                return;
            }

            if ((icreateinfo & (int)icreateinfo_flag.CF_UNIQUE) == 0) 
            {
                if ((icreateinfo & (int)icreateinfo_flag.CF_TOWN) != 0) {
                    RecreateTownItem(player, idx, icreateinfo, iseed);
                    // gbIsHellfire = tmpIsHellfire;
                    return;
                }

                if ((icreateinfo & (int)icreateinfo_flag.CF_USEFUL) == (int)icreateinfo_flag.CF_USEFUL) {
                    SetupAllUseful(iseed, icreateinfo & (int)icreateinfo_flag.CF_LEVEL);
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

            SetupAllItems(player, idx, iseed, level, uper, onlygood, recreate, pregen);
        }

        void RecreateTownItem(d2Player player, _item_indexes idx, int icreateinfo, int iseed)
        {
            if ((icreateinfo & (int)icreateinfo_flag.CF_SMITH) != 0)
                RecreateSmithItem(player, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
            // else if ((icreateinfo & (int)icreateinfo_flag.CF_SMITHPREMIUM) != 0)
            //     RecreatePremiumItem(player, item, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
            // else if ((icreateinfo & (int)icreateinfo_flag.CF_BOY) != 0)
            //     RecreateBoyItem(player, item, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
            // else if ((icreateinfo & (int)icreateinfo_flag.CF_WITCH) != 0)
            //     RecreateWitchItem(player, item, idx, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
            // else if ((icreateinfo & (int)icreateinfo_flag.CF_HEALER) != 0)
            //     RecreateHealerItem(player, item, idx, icreateinfo & (int)icreateinfo_flag.CF_LEVEL, iseed);
        }

        void RecreateSmithItem(d2Player player, int lvl, int iseed)
        {
            d2Utils.SetRndSeed(iseed);
            _item_indexes itype = RndSmithItem(player, lvl);
            GetItemAttrs(itype, lvl);

            _iSeed = iseed;
            _iCreateInfo = lvl | (int)icreateinfo_flag.CF_SMITH;
            _iIdentified = true;
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

        void GetItemAttrs(_item_indexes itemData, int lvl)
        {
            var baseItemData = d2Data.AllItemsList[(int)(itemData)];
            _itype = baseItemData.itype;
            _iCurs = baseItemData.iCurs;
            _iName = baseItemData.iName;
            _iIName = baseItemData.iName;
            _iLoc = baseItemData.iLoc;
            _iClass = baseItemData.iClass;
            _iMinDam = baseItemData.iMinDam;
            _iMaxDam = baseItemData.iMaxDam;
            _iAC = baseItemData.iMinAC + d2Utils.GenerateRnd(baseItemData.iMaxAC - baseItemData.iMinAC + 1);
            _iFlags = baseItemData.iFlags;
            _iMiscId = baseItemData.iMiscId;
            _iSpell = baseItemData.iSpell;
            _iMagical = item_quality.ITEM_QUALITY_NORMAL;
            _ivalue = baseItemData.iValue;
            _iIvalue = baseItemData.iValue;
            _iDurability = baseItemData.iDurability;
            _iMaxDur = baseItemData.iDurability;
            _iMinStr = baseItemData.iMinStr;
            _iMinMag = baseItemData.iMinMag;
            _iMinDex = baseItemData.iMinDex;
            IDidx = itemData;
            if (d2DEF.gbIsHellfire)
                dwBuff |= icreateinfo_flag2.CF_HELLFIRE;
            _iPrePower = item_effect_type.IPL_INVALID;
            _iSufPower = item_effect_type.IPL_INVALID;

            // if (_iMiscId == item_misc_id.IMISC_BOOK)
            //     GetBookSpell(item, lvl);

            // if (d2DEF.gbIsHellfire && _iMiscId == item_misc_id.IMISC_OILOF)
            //     GetOilType(item, lvl);

            if (_itype != ItemType.Gold)
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

            // _ivalue = std::min(rndv, GOLD_MAX_LIMIT);
            // SetPlrHandGoldCurs(item);
        }

        void SetupAllUseful(int iseed, int lvl)
        {
            _iSeed = iseed;
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

            GetItemAttrs(idx, lvl);
            _iCreateInfo = lvl | (int)icreateinfo_flag.CF_USEFUL;
            SetupItem();
        }

        void SetupItem()
        {
            // setNewAnimation(MyPlayer != nullptr && MyPlayer->pLvlLoad == 0);
            _iIdentified = false;
        }

        void SetupAllItems(d2Player player, _item_indexes idx, int iseed, int lvl, int uper, bool onlygood, bool recreate, bool pregen)
        {
            _iSeed = iseed;
            d2Utils.SetRndSeed(iseed);
            GetItemAttrs(idx, lvl / 2);
            _iCreateInfo = lvl;

            if (pregen)
                _iCreateInfo |= (int)icreateinfo_flag.CF_PREGEN;
            if (onlygood)
                _iCreateInfo |= (int)icreateinfo_flag.CF_ONLYGOOD;

            if (uper == 15)
                _iCreateInfo |= (int)icreateinfo_flag.CF_UPER15;
            else if (uper == 1)
                _iCreateInfo |= (int)icreateinfo_flag.CF_UPER1;

            if (_iMiscId != item_misc_id.IMISC_UNIQUE) {
                int iblvl = -1;
                if (d2Utils.GenerateRnd(100) <= 10 || d2Utils.GenerateRnd(100) <= lvl) {
                    iblvl = lvl;
                }
                if (iblvl == -1 && _iMiscId == item_misc_id.IMISC_STAFF) {
                    iblvl = lvl;
                }
                if (iblvl == -1 && _iMiscId == item_misc_id.IMISC_RING) {
                    iblvl = lvl;
                }
                if (iblvl == -1 && _iMiscId == item_misc_id.IMISC_AMULET) {
                    iblvl = lvl;
                }
                if (onlygood)
                    iblvl = lvl;
                if (uper == 15)
                    iblvl = lvl + 4;
                if (iblvl != -1) {
                    _unique_items uid = CheckUnique(iblvl, uper, recreate);
                    if (uid == _unique_items.UITEM_INVALID) {
                        GetItemBonus(player, iblvl / 2, iblvl, onlygood, true);
                    } else {
                        GetUniqueItem(player, uid);
                    }
                }
                if (_iMagical != item_quality.ITEM_QUALITY_UNIQUE)
                    ItemRndDur();
            } else {
                if (_iLoc != item_equip_type.ILOC_UNEQUIPABLE) {
                    GetUniqueItem(player, (_unique_items)iseed); // uid is stored in iseed for uniques
                }
            }
            SetupItem();
        }

        public static bool[] UniqueItemFlags = new bool[128];
        /** Unique item ID, used as an index into UniqueItemList */
        public int _iUid = 0;
        void GetUniqueItem(d2Player player, _unique_items uid)
        {
            UniqueItemFlags[(int)uid] = true;

            foreach (var power in d2Data.UniqueItems[(int)uid].powers) 
            {
                if (power.type == item_effect_type.IPL_INVALID)
                    break;
                SaveItemPower(player, power);
            }

            _iIName = d2Data.UniqueItems[(int)uid].UIName;
            _iIvalue = d2Data.UniqueItems[(int)uid].UIValue;

            if (_iMiscId == item_misc_id.IMISC_UNIQUE)
                _iSeed = (int)uid;

            _iUid = (int)uid;
            _iMagical = item_quality.ITEM_QUALITY_UNIQUE;
            _iCreateInfo |= (int)icreateinfo_flag.CF_UNIQUE;
        }

        // Item indestructible durability
        public const int DUR_INDESTRUCTIBLE = 255; // 不可破坏
        void ItemRndDur()
        {
            if (_iDurability > 0 && _iDurability != DUR_INDESTRUCTIBLE)
                _iDurability = d2Utils.GenerateRnd(_iMaxDur / 2) + (_iMaxDur / 4) + 1;
        }

        public void GenerateNewSeed()
        {
            _iSeed = d2Utils.AdvanceRndSeed();
        }

        _unique_items CheckUnique(int lvl, int uper, bool recreate)
        {
            return _unique_items.UITEM_INVALID;

            // std::bitset<128> uok = {};

            // if (d2Utils.GenerateRnd(100) > uper)
            //     return _unique_items.UITEM_INVALID;

            // int numu = 0;
            // for (int j = 0; UniqueItems[j].UIItemId != UITYPE_INVALID; j++) {
            //     if (!IsUniqueAvailable(j))
            //         break;
            //     if (UniqueItems[j].UIItemId == AllItemsList[IDidx].iItemId
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
        void GetItemBonus(d2Player player, int minlvl, int maxlvl, bool onlygood, bool allowspells)
        {
            if (minlvl > 25)
                minlvl = 25;

            switch (_itype) {
            case ItemType.Sword:
            case ItemType.Axe:
            case ItemType.Mace:
                GetItemPower(player, minlvl, maxlvl, AffixItemType.Weapon, onlygood);
                break;
            case ItemType.Bow:
                GetItemPower(player, minlvl, maxlvl, AffixItemType.Bow, onlygood);
                break;
            case ItemType.Shield:
                GetItemPower(player, minlvl, maxlvl, AffixItemType.Shield, onlygood);
                break;
            case ItemType.LightArmor:
            case ItemType.Helm:
            case ItemType.MediumArmor:
            case ItemType.HeavyArmor:
                GetItemPower(player, minlvl, maxlvl, AffixItemType.Armor, onlygood);
                break;
            case ItemType.Staff:
                if (allowspells)
                    GetStaffSpell(player, maxlvl, onlygood);
                else
                    GetItemPower(player, minlvl, maxlvl, AffixItemType.Staff, onlygood);
                break;
            case ItemType.Ring:
            case ItemType.Amulet:
                GetItemPower(player, minlvl, maxlvl, AffixItemType.Misc, onlygood);
                break;
            case ItemType.None:
            case ItemType.Misc:
            case ItemType.Gold:
                break;
            }
        }

        void GetItemPower(d2Player player, int minlvl, int maxlvl, AffixItemType flgs, bool onlygood)
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
                    _iMagical = item_quality.ITEM_QUALITY_MAGIC;
                    SaveItemAffix(player, d2Data.ItemPrefixes[preidx]);
                    _iPrePower = d2Data.ItemPrefixes[preidx].power.type;
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
                    _iMagical = item_quality.ITEM_QUALITY_MAGIC;
                    SaveItemAffix(player, d2Data.ItemSuffixes[sufidx]);
                    _iSufPower = d2Data.ItemSuffixes[sufidx].power.type;
                }
            }

            _iIName = GenerateMagicItemName(_iName, preidx, sufidx);
            // if (!StringInPanel(_iIName)) 
            // {
            //     CopyUtf8(_iIName, GenerateMagicItemName(_(d2Data.AllItemsList[(int)IDidx].iSName), preidx, sufidx), sizeof(_iIName));
            // }
            if (preidx != -1 || sufidx != -1)
                CalcItemValue();
        }

        void CalcItemValue()
        {
            int v = _iVMult1 + _iVMult2;
            if (v > 0) {
                v *= _ivalue;
            }
            if (v < 0) {
                v = _ivalue / v;
            }
            v = _iVAdd1 + _iVAdd2 + v;
            _iIvalue = Math.Max(v, 1);
        }

        string GenerateMagicItemName(string baseNamel, int preidx, int sufidx)
        {
            // if (preidx != -1 && sufidx != -1) {
            //     string_view fmt = _(/* TRANSLATORS: Constructs item names. Format: {Prefix} {Item} of {Suffix}. Example: King's Long Sword of the Whale */ "{0} {1} of {2}");
            //     return fmt::format(fmt::runtime(fmt), _(ItemPrefixes[preidx].PLName), baseNamel, _(ItemSuffixes[sufidx].PLName));
            // } else if (preidx != -1) {
            //     string_view fmt = _(/* TRANSLATORS: Constructs item names. Format: {Prefix} {Item}. Example: King's Long Sword */ "{0} {1}");
            //     return fmt::format(fmt::runtime(fmt), _(ItemPrefixes[preidx].PLName), baseNamel);
            // } else if (sufidx != -1) {
            //     string_view fmt = _(/* TRANSLATORS: Constructs item names. Format: {Item} of {Suffix}. Example: Long Sword of the Whale */ "{0} of {1}");
            //     return fmt::format(fmt::runtime(fmt), baseNamel, _(ItemSuffixes[sufidx].PLName));
            // }

            return baseNamel;
        }

        bool IsSuffixValidForItemType(int i, AffixItemType flgs)
        {
            AffixItemType itemTypes = d2Data.ItemSuffixes[i].PLIType;

            if (!d2DEF.gbIsHellfire) {
                if (i > 94)
                    return false;

                if ((i >= 0 && i <= 1)
                    || (i >= 14 && i <= 15)
                    || (i >= 21 && i <= 22)
                    || (i >= 34 && i <= 36)
                    || (i >= 41 && i <= 44)
                    || (i >= 60 && i <= 63))
                    itemTypes &= ~AffixItemType.Staff;
            }

            return d2Utils.HasAnyOf(flgs, itemTypes);
        }

        bool IsPrefixValidForItemType(int i, AffixItemType flgs)
        {
            AffixItemType itemTypes = d2Data.ItemPrefixes[i].PLIType;

            if (!d2DEF.gbIsHellfire) {
                if (i > 82)
                    return false;

                if (i >= 12 && i <= 20)
                    itemTypes &= ~AffixItemType.Staff;
            }

            return d2Utils.HasAnyOf(flgs, itemTypes);
        }

        int PLVal(int pv, int p1, int p2, int minv, int maxv)
        {
            if (p1 == p2)
                return minv;
            if (minv == maxv)
                return minv;
            return minv + (maxv - minv) * (100 * (pv - p1) / (p2 - p1)) / 100;
        }

        public int _iVAdd1 = 0;
        public int _iVMult1 = 0;
        public int _iVAdd2 = 0;
        public int _iVMult2 = 0;

        void SaveItemAffix(d2Player player, PLStruct affix)
        {
            var power = affix.power;
            int value = SaveItemPower(player, power);

            value = PLVal(value, power.param1, power.param2, affix.minVal, affix.maxVal);
            if (_iVAdd1 != 0 || _iVMult1 != 0) {
                _iVAdd2 = value;
                _iVMult2 = affix.multVal;
            } else {
                _iVAdd1 = value;
                _iVMult1 = affix.multVal;
            }
        }

        int CalculateToHitBonus(int level)
        {
            switch (level) {
            case -50:
                return -d2Utils.RndPL(6, 10);
            case -25:
                return -d2Utils.RndPL(1, 5);
            case 20:
                return d2Utils.RndPL(1, 5);
            case 36:
                return d2Utils.RndPL(6, 10);
            case 51:
                return d2Utils.RndPL(11, 15);
            case 66:
                return d2Utils.RndPL(16, 20);
            case 81:
                return d2Utils.RndPL(21, 30);
            case 96:
                return d2Utils.RndPL(31, 40);
            case 111:
                return d2Utils.RndPL(41, 50);
            case 126:
                return d2Utils.RndPL(51, 75);
            case 151:
                return d2Utils.RndPL(76, 100);
            default:
                Debug.LogError("Unknown to hit bonus");
                return 0;
            }
        }

        int SaveItemPower(d2Player player, ItemPower power)
        {
            if (!d2DEF.gbIsHellfire) {
                if (power.type == item_effect_type.IPL_TARGAC) {
                    power.param1 = 1 << power.param1;
                    power.param2 = 3 << power.param2;
                }
            }

            int r = d2Utils.RndPL(power.param1, power.param2);

            switch (power.type) {
            case item_effect_type.IPL_TOHIT:
                _iPLToHit += r;
                break;
            case item_effect_type.IPL_TOHIT_CURSE:
                _iPLToHit -= r;
                break;
            case item_effect_type.IPL_DAMP:
                _iPLDam += r;
                break;
            case item_effect_type.IPL_DAMP_CURSE:
                _iPLDam -= r;
                break;
            case item_effect_type.IPL_DOPPELGANGER:
                _iDamAcFlags |= ItemSpecialEffectHf.Doppelganger;
                break;
            case item_effect_type.IPL_TOHIT_DAMP:
                r = d2Utils.RndPL(power.param1, power.param2);
                _iPLDam += r;
                _iPLToHit += CalculateToHitBonus(power.param1);
                break;
            case item_effect_type.IPL_TOHIT_DAMP_CURSE:
                _iPLDam -= r;
                _iPLToHit += CalculateToHitBonus(-power.param1);
                break;
            case item_effect_type.IPL_ACP:
                _iPLAC += r;
                break;
            case item_effect_type.IPL_ACP_CURSE:
                _iPLAC -= r;
                break;
            case item_effect_type.IPL_SETAC:
                _iAC = r;
                break;
            case item_effect_type.IPL_AC_CURSE:
                _iAC -= r;
                break;
            case item_effect_type.IPL_FIRERES:
                _iPLFR += r;
                break;
            case item_effect_type.IPL_LIGHTRES:
                _iPLLR += r;
                break;
            case item_effect_type.IPL_MAGICRES:
                _iPLMR += r;
                break;
            case item_effect_type.IPL_ALLRES:
                _iPLFR = Math.Max(_iPLFR + r, 0);
                _iPLLR = Math.Max(_iPLLR + r, 0);
                _iPLMR = Math.Max(_iPLMR + r, 0);
                break;
            case item_effect_type.IPL_SPLLVLADD:
                _iSplLvlAdd = r;
                break;
            case item_effect_type.IPL_CHARGES:
                _iCharges *= power.param1;
                _iMaxCharges = _iCharges;
                break;
            case item_effect_type.IPL_SPELL:
                _iSpell = (spell_id)(power.param1);
                _iCharges = power.param2;
                _iMaxCharges = power.param2;
                break;
            case item_effect_type.IPL_FIREDAM:
                _iFlags |= ItemSpecialEffect.FireDamage;
                _iFlags &= ~ItemSpecialEffect.LightningDamage;
                _iFMinDam = power.param1;
                _iFMaxDam = power.param2;
                _iLMinDam = 0;
                _iLMaxDam = 0;
                break;
            case item_effect_type.IPL_LIGHTDAM:
                _iFlags |= ItemSpecialEffect.LightningDamage;
                _iFlags &= ~ItemSpecialEffect.FireDamage;
                _iLMinDam = power.param1;
                _iLMaxDam = power.param2;
                _iFMinDam = 0;
                _iFMaxDam = 0;
                break;
            case item_effect_type.IPL_STR:
                _iPLStr += r;
                break;
            case item_effect_type.IPL_STR_CURSE:
                _iPLStr -= r;
                break;
            case item_effect_type.IPL_MAG:
                _iPLMag += r;
                break;
            case item_effect_type.IPL_MAG_CURSE:
                _iPLMag -= r;
                break;
            case item_effect_type.IPL_DEX:
                _iPLDex += r;
                break;
            case item_effect_type.IPL_DEX_CURSE:
                _iPLDex -= r;
                break;
            case item_effect_type.IPL_VIT:
                _iPLVit += r;
                break;
            case item_effect_type.IPL_VIT_CURSE:
                _iPLVit -= r;
                break;
            case item_effect_type.IPL_ATTRIBS:
                _iPLStr += r;
                _iPLMag += r;
                _iPLDex += r;
                _iPLVit += r;
                break;
            case item_effect_type.IPL_ATTRIBS_CURSE:
                _iPLStr -= r;
                _iPLMag -= r;
                _iPLDex -= r;
                _iPLVit -= r;
                break;
            case item_effect_type.IPL_GETHIT_CURSE:
                _iPLGetHit += r;
                break;
            case item_effect_type.IPL_GETHIT:
                _iPLGetHit -= r;
                break;
            case item_effect_type.IPL_LIFE:
                _iPLHP += r << d2DEF.HPMANAOFFSET;
                break;
            case item_effect_type.IPL_LIFE_CURSE:
                _iPLHP -= r << d2DEF.HPMANAOFFSET;
                break;
            case item_effect_type.IPL_MANA:
                _iPLMana += r << d2DEF.HPMANAOFFSET;
                // RedrawComponent(PanelDrawComponent::Mana);
                break;
            case item_effect_type.IPL_MANA_CURSE:
                _iPLMana -= r << d2DEF.HPMANAOFFSET;
                // RedrawComponent(PanelDrawComponent::Mana);
                break;
            case item_effect_type.IPL_DUR: {
                int bonus = r * _iMaxDur / 100;
                _iMaxDur += bonus;
                _iDurability += bonus;
            } break;
            case item_effect_type.IPL_CRYSTALLINE:
                _iPLDam += 140 + r * 2;
                break;
            case item_effect_type.IPL_DUR_CURSE:
                _iMaxDur -= r * _iMaxDur / 100;
                _iMaxDur = Math.Max(_iMaxDur, 1);
                _iDurability = _iMaxDur;
                break;
            case item_effect_type.IPL_INDESTRUCTIBLE:
                _iDurability = DUR_INDESTRUCTIBLE;
                _iMaxDur = DUR_INDESTRUCTIBLE;
                break;
            case item_effect_type.IPL_LIGHT:
                _iPLLight += power.param1;
                break;
            case item_effect_type.IPL_LIGHT_CURSE:
                _iPLLight -= power.param1;
                break;
            case item_effect_type.IPL_MULT_ARROWS:
                _iFlags |= ItemSpecialEffect.MultipleArrows;
                break;
            case item_effect_type.IPL_FIRE_ARROWS:
                _iFlags |= ItemSpecialEffect.FireArrows;
                _iFlags &= ~ItemSpecialEffect.LightningArrows;
                _iFMinDam = power.param1;
                _iFMaxDam = power.param2;
                _iLMinDam = 0;
                _iLMaxDam = 0;
                break;
            case item_effect_type.IPL_LIGHT_ARROWS:
                _iFlags |= ItemSpecialEffect.LightningArrows;
                _iFlags &= ~ItemSpecialEffect.FireArrows;
                _iLMinDam = power.param1;
                _iLMaxDam = power.param2;
                _iFMinDam = 0;
                _iFMaxDam = 0;
                break;
            case item_effect_type.IPL_FIREBALL:
                _iFlags |= (ItemSpecialEffect.LightningArrows | ItemSpecialEffect.FireArrows);
                _iFMinDam = power.param1;
                _iFMaxDam = power.param2;
                _iLMinDam = 0;
                _iLMaxDam = 0;
                break;
            case item_effect_type.IPL_THORNS:
                _iFlags |= ItemSpecialEffect.Thorns;
                break;
            case item_effect_type.IPL_NOMANA:
                _iFlags |= ItemSpecialEffect.NoMana;
                // RedrawComponent(PanelDrawComponent::Mana);
                break;
            case item_effect_type.IPL_ABSHALFTRAP:
                _iFlags |= ItemSpecialEffect.HalfTrapDamage;
                break;
            case item_effect_type.IPL_KNOCKBACK:
                _iFlags |= ItemSpecialEffect.Knockback;
                break;
            case item_effect_type.IPL_3XDAMVDEM:
                _iFlags |= ItemSpecialEffect.TripleDemonDamage;
                break;
            case item_effect_type.IPL_ALLRESZERO:
                _iFlags |= ItemSpecialEffect.ZeroResistance;
                break;
            case item_effect_type.IPL_STEALMANA:
                if (power.param1 == 3)
                    _iFlags |= ItemSpecialEffect.StealMana3;
                if (power.param1 == 5)
                    _iFlags |= ItemSpecialEffect.StealMana5;
                // RedrawComponent(PanelDrawComponent::Mana);
                break;
            case item_effect_type.IPL_STEALLIFE:
                if (power.param1 == 3)
                    _iFlags |= ItemSpecialEffect.StealLife3;
                if (power.param1 == 5)
                    _iFlags |= ItemSpecialEffect.StealLife5;
                // RedrawComponent(PanelDrawComponent::Health);
                break;
            case item_effect_type.IPL_TARGAC:
                if (d2DEF.gbIsHellfire)
                    _iPLEnAc = power.param1;
                else
                    _iPLEnAc += r;
                break;
            case item_effect_type.IPL_FASTATTACK:
                if (power.param1 == 1)
                    _iFlags |= ItemSpecialEffect.QuickAttack;
                if (power.param1 == 2)
                    _iFlags |= ItemSpecialEffect.FastAttack;
                if (power.param1 == 3)
                    _iFlags |= ItemSpecialEffect.FasterAttack;
                if (power.param1 == 4)
                    _iFlags |= ItemSpecialEffect.FastestAttack;
                break;
            case item_effect_type.IPL_FASTRECOVER:
                if (power.param1 == 1)
                    _iFlags |= ItemSpecialEffect.FastHitRecovery;
                if (power.param1 == 2)
                    _iFlags |= ItemSpecialEffect.FasterHitRecovery;
                if (power.param1 == 3)
                    _iFlags |= ItemSpecialEffect.FastestHitRecovery;
                break;
            case item_effect_type.IPL_FASTBLOCK:
                _iFlags |= ItemSpecialEffect.FastBlock;
                break;
            case item_effect_type.IPL_DAMMOD:
                _iPLDamMod += r;
                break;
            case item_effect_type.IPL_RNDARROWVEL:
                _iFlags |= ItemSpecialEffect.RandomArrowVelocity;
                break;
            case item_effect_type.IPL_SETDAM:
                _iMinDam = power.param1;
                _iMaxDam = power.param2;
                break;
            case item_effect_type.IPL_SETDUR:
                _iDurability = power.param1;
                _iMaxDur = power.param1;
                break;
            case item_effect_type.IPL_ONEHAND:
                _iLoc = item_equip_type.ILOC_ONEHAND;
                break;
            case item_effect_type.IPL_DRAINLIFE:
                _iFlags |= ItemSpecialEffect.DrainLife;
                break;
            case item_effect_type.IPL_RNDSTEALLIFE:
                _iFlags |= ItemSpecialEffect.RandomStealLife;
                break;
            case item_effect_type.IPL_NOMINSTR:
                _iMinStr = 0;
                break;
            case item_effect_type.IPL_INVCURS:
                _iCurs = (item_cursor_graphic)power.param1;
                break;
            case item_effect_type.IPL_ADDACLIFE:
                _iFlags |= (ItemSpecialEffect.LightningArrows | ItemSpecialEffect.FireArrows);
                _iFMinDam = power.param1;
                _iFMaxDam = power.param2;
                _iLMinDam = 1;
                _iLMaxDam = 0;
                break;
            case item_effect_type.IPL_ADDMANAAC:
                _iFlags |= (ItemSpecialEffect.LightningDamage | ItemSpecialEffect.FireDamage);
                _iFMinDam = power.param1;
                _iFMaxDam = power.param2;
                _iLMinDam = 2;
                _iLMaxDam = 0;
                break;
            case item_effect_type.IPL_FIRERES_CURSE:
                _iPLFR -= r;
                break;
            case item_effect_type.IPL_LIGHTRES_CURSE:
                _iPLLR -= r;
                break;
            case item_effect_type.IPL_MAGICRES_CURSE:
                _iPLMR -= r;
                break;
            case item_effect_type.IPL_DEVASTATION:
                _iDamAcFlags |= ItemSpecialEffectHf.Devastation;
                break;
            case item_effect_type.IPL_DECAY:
                _iDamAcFlags |= ItemSpecialEffectHf.Decay;
                _iPLDam += r;
                break;
            case item_effect_type.IPL_PERIL:
                _iDamAcFlags |= ItemSpecialEffectHf.Peril;
                break;
            case item_effect_type.IPL_JESTERS:
                _iDamAcFlags |= ItemSpecialEffectHf.Jesters;
                break;
            case item_effect_type.IPL_ACDEMON:
                _iDamAcFlags |= ItemSpecialEffectHf.ACAgainstDemons;
                break;
            case item_effect_type.IPL_ACUNDEAD:
                _iDamAcFlags |= ItemSpecialEffectHf.ACAgainstUndead;
                break;
            case item_effect_type.IPL_MANATOLIFE: {
                int portion = ((player._pMaxManaBase >> d2DEF.HPMANAOFFSET) * 50 / 100) << d2DEF.HPMANAOFFSET;
                _iPLMana -= portion;
                _iPLHP += portion;
            } break;
            case item_effect_type.IPL_LIFETOMANA: {
                int portion = ((player._pMaxHPBase >> d2DEF.HPMANAOFFSET) * 40 / 100) << d2DEF.HPMANAOFFSET;
                _iPLHP -= portion;
                _iPLMana += portion;
            } break;
            default:
                break;
            }

            return r;
        }

        int GetSpellStaffLevel(spell_id s)
        {
            if (d2DEF.gbIsSpawn) {
                switch (s) {
                case spell_id.SPL_STONE:
                case spell_id.SPL_GUARDIAN:
                case spell_id.SPL_GOLEM:
                case spell_id.SPL_APOCA:
                case spell_id.SPL_ELEMENT:
                case spell_id.SPL_FLARE:
                case spell_id.SPL_BONESPIRIT:
                    return -1;
                default:
                    break;
                }
            }

            if (!d2DEF.gbIsHellfire && s > spell_id.SPL_LASTDIABLO)
                return -1;

            return d2Data.spelldata[(int)s].sStaffLvl;
        }

        public const int MAX_SPELLS = 52;
        void GetStaffSpell(d2Player player, int lvl, bool onlygood)
        {
            if (!d2DEF.gbIsHellfire && d2Utils.FlipCoin(4)) {
                GetItemPower(player, lvl / 2, lvl, AffixItemType.Staff, onlygood);
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
                int sLevel = GetSpellStaffLevel((spell_id)(s));
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

            int minc = d2Data.spelldata[(int)bs].sStaffMin;
            int maxc = d2Data.spelldata[(int)bs].sStaffMax - minc + 1;
            _iSpell = bs;
            _iCharges = minc + d2Utils.GenerateRnd(maxc);
            _iMaxCharges = _iCharges;

            _iMinMag = d2Data.spelldata[(int)bs].sMinInt;
            int v = _iCharges * d2Data.spelldata[(int)bs].sStaffCost / 5;
            _ivalue += v;
            _iIvalue += v;
            GetStaffPower(player, lvl, (int)bs, onlygood);
        }

        void GetStaffPower(d2Player player, int lvl, int bs, bool onlygood)
        {
            int preidx = -1;
            if (d2Utils.FlipCoin(10) || onlygood) 
            {
                int nl = 0;
                int[] l = new int[256];
                for (int j = 0; d2Data.ItemPrefixes[j].power.type != item_effect_type.IPL_INVALID; j++) 
                {
                    if (!IsPrefixValidForItemType(j, AffixItemType.Staff) || d2Data.ItemPrefixes[j].PLMinLvl > lvl)
                        continue;
                    if (onlygood && !d2Data.ItemPrefixes[j].PLOk)
                        continue;
                    l[nl] = j;
                    nl++;
                    if (d2Data.ItemPrefixes[j].PLDouble) {
                        l[nl] = j;
                        nl++;
                    }
                }
                if (nl != 0) {
                    preidx = l[d2Utils.GenerateRnd(nl)];
                    _iMagical = item_quality.ITEM_QUALITY_MAGIC;
                    SaveItemAffix(player, d2Data.ItemPrefixes[preidx]);
                    _iPrePower = d2Data.ItemPrefixes[preidx].power.type;
                }
            }

            // string baseName = _(AllItemsList[IDidx].iName);
            // string shortName = _(AllItemsList[IDidx].iSName);
            // string spellName = pgettext("spell", spelldata[bs].sNameText);
            // string normalFmt = pgettext("spell", /* TRANSLATORS: Constructs item names. Format: {Item} of {Spell}. Example: War Staff of Firewall */ "{0} of {1}");

            // CopyUtf8(_iName, fmt::format(fmt::runtime(normalFmt), baseName, spellName), sizeof(_iName));
            // if (!StringInPanel(_iName)) {
            //     CopyUtf8(_iName, fmt::format(fmt::runtime(normalFmt), shortName, spellName), sizeof(_iName));
            // }

            // if (preidx != -1) {
            //     string_view magicFmt = pgettext("spell", /* TRANSLATORS: Constructs item names. Format: {Prefix} {Item} of {Spell}. Example: King's War Staff of Firewall */ "{0} {1} of {2}");
            //     string_view prefixName = _(ItemPrefixes[preidx].PLName);
            //     CopyUtf8(_iIName, fmt::format(fmt::runtime(magicFmt), prefixName, baseName, spellName), sizeof(_iIName));
            //     if (!StringInPanel(_iIName)) {
            //         CopyUtf8(_iIName, fmt::format(fmt::runtime(magicFmt), prefixName, shortName, spellName), sizeof(_iIName));
            //     }
            // } else {
            //     CopyUtf8(_iIName, _iName, sizeof(_iIName));
            // }

            CalcItemValue();
        }
    }
}
