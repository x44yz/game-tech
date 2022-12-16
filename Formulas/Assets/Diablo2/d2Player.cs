using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public class d2Player : Unit
    {
        /** Maps from player_class to starting stat in strength. */
        static readonly int[] StrengthTbl = new int[]{
            30,
            20,
            15,
            25,
            20,
            40,
        };
        /** Maps from player_class to starting stat in magic. */
        static readonly int[] MagicTbl = new int[]{
            // clang-format off
            10,
            15,
            35,
            15,
            20,
            0,
            // clang-format on
        };
        /** Maps from player_class to starting stat in dexterity. */
        static readonly int[] DexterityTbl = new int[]{
            20,
            30,
            15,
            25,
            25,
            20,
        };
        /** Maps from player_class to starting stat in vitality. */
        static readonly int[] VitalityTbl = new int[]{
            25,
            20,
            20,
            20,
            20,
            25,
        };
        /** Specifies the chance to block bonus of each player class.*/
        static readonly int[] BlockBonuses = new int[] {
            30,
            20,
            10,
            25,
            25,
            30,
        };

        /** Specifies the experience point limit of each level. */
        static readonly int[] ExpLvlsTbl = new int[] {
            0,
            2000,
            4620,
            8040,
            12489,
            18258,
            25712,
            35309,
            47622,
            63364,
            83419,
            108879,
            141086,
            181683,
            231075,
            313656,
            424067,
            571190,
            766569,
            1025154,
            1366227,
            1814568,
            2401895,
            3168651,
            4166200,
            5459523,
            7130496,
            9281874,
            12042092,
            15571031,
            20066900,
            25774405,
            32994399,
            42095202,
            53525811,
            67831218,
            85670061,
            107834823,
            135274799,
            169122009,
            210720231,
            261657253,
            323800420,
            399335440,
            490808349,
            601170414,
            733825617,
            892680222,
            1082908612,
            1310707109,
            1583495809
        };

        [Header("---- PLAYER ----")]
        public HeroClass _pClass;
        public int _pLevel;
        public int armorId;


        [Header("---- RUNTIME ----")]
        public int _pStrength;
        public int _pBaseStr;
        public int _pMagic;
        public int _pBaseMag;
        public int _pDexterity; // 敏捷
        public int _pBaseDex;
        public int _pVitality;
        public int _pBaseVit;
         public int _pStatPts;
        public int _pDamageMod;
        public int _pBaseToBlk;
        public int _pHPBase;
        public int _pMaxHPBase;
        public int _pHitPoints;
        public int _pMaxHP;
        public int _pHPPer;
        public int _pManaBase;
        public int _pMaxManaBase;
        public int _pMana;
        public int _pMaxMana;
        public int _pManaPer;
        public int _pIMinDam;
        public int _pIMaxDam;
        public int _pIAC;
        public int _pIBonusDam;
        public int _pIBonusToHit;
        public int _pIBonusAC;
        public int _pIBonusDamMod;
        public int _pIGetHit;
        public int _pISplDur;
        public int _pIEnAc;
        public int _pIFMinDam;
        public int _pIFMaxDam;
        public int _pILMinDam;
        public int _pILMaxDam;
        public int pTownWarps;
        public int pDungMsgs;
        public int pLvlLoad;
	    public bool pBattleNet;
	    public bool pManaShield;
	    public int pDungMsgs2;
        public int pDiabloKillLevel;
        public int  wReflections;
        public _difficulty pDifficulty;
        public int _pMaxLvl;
	    public int _pExperience;
	    public int _pNextExper;
        public int _pArmorClass;
	    public int _pMagResist;
	    public int _pFireResist;
	    public int _pLghtResist;
        public int _pLightRad;
        public bool _pInfraFlag;
        /** @brief Bitmask using item_special_effect */
        public ItemSpecialEffect _pIFlags;
        public ItemSpecialEffectHf pDamAcFlags;
        public d2Item[] InvBody = new d2Item[(int)inv_body_loc.NUM_INVLOC];

        public spell_type _pRSplType;
        public spell_id _pRSpell;
        /** @brief Bitmask of staff spell */
        public UInt64 _pISpells;
        /** @brief Bitmask of learned spells */
        public UInt64 _pMemSpells;
        /** @brief Bitmask of abilities */
        public UInt64 _pAblSpells;
        /** @brief Bitmask of spells available via scrolls */
        public UInt64 _pScrlSpells;
        public int[] _pSplLvl = new int[64];
        public SpellFlag _pSpellFlags;

        public const int NUMLEVELS = 25;
        public bool[] _pLvlVisited = new bool[NUMLEVELS];
        public bool[] _pSLvlVisited = new bool[NUMLEVELS]; // only 10 used
        	/** @brief True when the player is transitioning between levels */
	    public bool _pLvlChanging;

        protected override void OnStart()
        {
            base.OnStart();

            InitPlayer(this, _pClass);
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        private void InitPlayer(d2Player player, HeroClass c)
        {
            player._pClass = c;

            int ic = (int)c;

            player._pBaseStr = StrengthTbl[ic];
            player._pStrength = player._pBaseStr;

            player._pBaseMag = MagicTbl[ic];
            player._pMagic = player._pBaseMag;

            player._pBaseDex = DexterityTbl[ic];
            player._pDexterity = player._pBaseDex;

            player._pBaseVit = VitalityTbl[ic];
            player._pVitality = player._pBaseVit;

            player._pStatPts = 0;
            player.pTownWarps = 0;
            player.pDungMsgs = 0;
            player.pDungMsgs2 = 0;
            player.pLvlLoad = 0;
            player.pDiabloKillLevel = 0;
            player.pDifficulty = _difficulty.DIFF_NORMAL;

            player._pLevel = 1;

            player._pBaseToBlk = BlockBonuses[ic];

            player._pHitPoints = (player._pVitality + 10) << 6;
            if (player._pClass == HeroClass.Warrior || player._pClass == HeroClass.Barbarian) {
                player._pHitPoints *= 2;
            } else if (player._pClass == HeroClass.Rogue || player._pClass == HeroClass.Monk || player._pClass == HeroClass.Bard) {
                player._pHitPoints += player._pHitPoints / 2;
            }

            player._pMaxHP = player._pHitPoints;
            player._pHPBase = player._pHitPoints;
            player._pMaxHPBase = player._pHitPoints;

            player._pMana = player._pMagic << 6;
            if (player._pClass == HeroClass.Sorcerer) {
                player._pMana *= 2;
            } else if (player._pClass == HeroClass.Bard) {
                player._pMana += player._pMana * 3 / 4;
            } else if (player._pClass == HeroClass.Rogue || player._pClass == HeroClass.Monk) {
                player._pMana += player._pMana / 2;
            }

            player._pMaxMana = player._pMana;
            player._pManaBase = player._pMana;
            player._pMaxManaBase = player._pMana;

            player._pMaxLvl = player._pLevel;
            player._pExperience = 0;
            player._pNextExper = ExpLvlsTbl[1];
            player._pArmorClass = 0;
            player._pLightRad = 10;
            player._pInfraFlag = false;

            player._pRSplType = spell_type.RSPLTYPE_SKILL;
            if (c == HeroClass.Warrior) {
                player._pAblSpells = GetSpellBitmask(spell_id.SPL_REPAIR);
                player._pRSpell = spell_id.SPL_REPAIR;
            } else if (c == HeroClass.Rogue) {
                player._pAblSpells = GetSpellBitmask(spell_id.SPL_DISARM);
                player._pRSpell = spell_id.SPL_DISARM;
            } else if (c == HeroClass.Sorcerer) {
                player._pAblSpells = GetSpellBitmask(spell_id.SPL_RECHARGE);
                player._pRSpell = spell_id.SPL_RECHARGE;
            } else if (c == HeroClass.Monk) {
                player._pAblSpells = GetSpellBitmask(spell_id.SPL_SEARCH);
                player._pRSpell = spell_id.SPL_SEARCH;
            } else if (c == HeroClass.Bard) {
                player._pAblSpells = GetSpellBitmask(spell_id.SPL_IDENTIFY);
                player._pRSpell = spell_id.SPL_IDENTIFY;
            } else if (c == HeroClass.Barbarian) {
                player._pAblSpells = GetSpellBitmask(spell_id.SPL_BLODBOIL);
                player._pRSpell = spell_id.SPL_BLODBOIL;
            }

            if (c == HeroClass.Sorcerer) {
                player._pMemSpells = GetSpellBitmask(spell_id.SPL_FIREBOLT);
                player._pRSplType = spell_type.RSPLTYPE_SPELL;
                player._pRSpell = spell_id.SPL_FIREBOLT;
            } else {
                player._pMemSpells = 0;
            }

            for (int i = 0; i < player._pSplLvl.Length; ++i) {
                player._pSplLvl[i] = 0;
            }

            player._pSpellFlags = SpellFlag.None;

            if (player._pClass == HeroClass.Sorcerer) {
                player._pSplLvl[((int)spell_id.SPL_FIREBOLT)] = 2;
            }

            // Initializing the hotkey bindings to no selection
            // std::fill(player._pSplHotKey, player._pSplHotKey + NumHotkeys, SPL_INVALID);

            // 武器动画
            // PlayerWeaponGraphic animWeaponId = PlayerWeaponGraphic::Unarmed;
            // switch (c) {
            // case HeroClass.Warrior:
            // case HeroClass.Bard:
            // case HeroClass.Barbarian:
            //     animWeaponId = PlayerWeaponGraphic::SwordShield;
            //     break;
            // case HeroClass.Rogue:
            //     animWeaponId = PlayerWeaponGraphic::Bow;
            //     break;
            // case HeroClass.Sorcerer:
            // case HeroClass.Monk:
            //     animWeaponId = PlayerWeaponGraphic::Staff;
            //     break;
            // }
            // player._pgfxnum = static_cast<uint8_t>(animWeaponId);

            for (int i = 0; i < player._pLvlVisited.Length; ++i) {
                player._pLvlVisited[i] = false;
            }

            for (int i = 0; i < 10; i++) {
                player._pSLvlVisited[i] = false;
            }

            player._pLvlChanging = false;
            player.pTownWarps = 0;
            player.pLvlLoad = 0;
            player.pBattleNet = false;
            player.pManaShield = false;
            player.pDamAcFlags = ItemSpecialEffectHf.None;
            player.wReflections = 0;

            InitDungMsgs(player);
            CreatePlrItems(player);
            // SetRndSeed(0);
        }

        void InitDungMsgs(d2Player player)
        {
            player.pDungMsgs = 0;
            player.pDungMsgs2 = 0;
        }

        public const int InventoryGridCells = 40;
        public const int MaxBeltItems = 8;
        public int[] InvGrid = new int[InventoryGridCells];
        public d2Item[] InvList = new d2Item[InventoryGridCells];
        public int _pNumInv;
        public d2Item[] SpdList = new d2Item[MaxBeltItems];
        void CreatePlrItems(d2Player player)
        {
            var objInvBody = new GameObject("InvBody");
            objInvBody.transform.SetParent(transform);
            for (int i = 0; i < ((int)inv_body_loc.NUM_INVLOC); ++i)
            {
                var item = d2Item.Create(objInvBody.transform);
                item.name = ((inv_body_loc)i).ToString();
                InvBody[i] = item;
            }
            for (int i = 0; i < player.InvBody.Length; ++i) 
            {
                var item = player.InvBody[i];
                item.clear();
            }

            // converting this to a for loop creates a `rep stosd` instruction,
            // so this probably actually was a memset
            for (int i = 0; i < player.InvGrid.Length; ++i)
                player.InvGrid[i] = 0;

            var objInvList = new GameObject("InvList");
            objInvList.transform.SetParent(transform);
            for (int i = 0; i < player.InvList.Length; ++i)
            {
                var item = d2Item.Create(objInvList.transform);
                item.name = "i" + i.ToString();
                player.InvList[i] = item;
            }
            for (int i = 0; i < player.InvList.Length; ++i) 
            {
                var item = player.InvList[i];
                item.clear();
            }

            player._pNumInv = 0;

            var objSpdList = new GameObject("SpdList");
            objSpdList.transform.SetParent(transform);
            for (int i = 0; i < player.SpdList.Length; ++i)
            {
                var item = d2Item.Create(objSpdList.transform);
                item.name = "i" + i.ToString();
                player.SpdList[i] = item;
            }
            for (int i = 0; i < player.SpdList.Length; ++i) 
            {
                var item = player.SpdList[i];
                item.clear();
            }

            switch (player._pClass) 
            {
            case HeroClass.Warrior:
                InitializeItem(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)], _item_indexes.IDI_WARRIOR);
                GenerateNewSeed(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]);

                InitializeItem(player.InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)], _item_indexes.IDI_WARRSHLD);
                GenerateNewSeed(player.InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]);

                {
                    Item club;
                    InitializeItem(club, _item_indexes.IDI_WARRCLUB);
                    GenerateNewSeed(club);
                    AutoPlaceItemInInventorySlot(player, 0, club, true);
                }

                InitializeItem(player.SpdList[0], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[0]);

                InitializeItem(player.SpdList[1], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[1]);
                break;
            case HeroClass.Rogue:
                InitializeItem(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)], _item_indexes.IDI_ROGUE);
                GenerateNewSeed(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]);

                InitializeItem(player.SpdList[0], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[0]);

                InitializeItem(player.SpdList[1], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[1]);
                break;
            case HeroClass.Sorcerer:
                InitializeItem(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)], d2DEF.gbIsHellfire ? _item_indexes.IDI_SORCERER : _item_indexes.IDI_SORCERER_DIABLO);
                GenerateNewSeed(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]);

                InitializeItem(player.SpdList[0], d2DEF.gbIsHellfire ? _item_indexes.IDI_HEAL : _item_indexes.IDI_MANA);
                GenerateNewSeed(player.SpdList[0]);

                InitializeItem(player.SpdList[1], d2DEF.gbIsHellfire ? _item_indexes.IDI_HEAL : _item_indexes.IDI_MANA);
                GenerateNewSeed(player.SpdList[1]);
                break;

            case HeroClass.Monk:
                InitializeItem(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)], _item_indexes.IDI_SHORTSTAFF);
                GenerateNewSeed(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]);
                InitializeItem(player.SpdList[0], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[0]);

                InitializeItem(player.SpdList[1], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[1]);
                break;
            case HeroClass.Bard:
                InitializeItem(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)], _item_indexes.IDI_BARDSWORD);
                GenerateNewSeed(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]);

                InitializeItem(player.InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)], _item_indexes.IDI_BARDDAGGER);
                GenerateNewSeed(player.InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]);
                InitializeItem(player.SpdList[0], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[0]);

                InitializeItem(player.SpdList[1], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[1]);
                break;
            case HeroClass.Barbarian:
                InitializeItem(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)], _item_indexes.IDI_BARBARIAN);
                GenerateNewSeed(player.InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]);

                InitializeItem(player.InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)], _item_indexes.IDI_WARRSHLD);
                GenerateNewSeed(player.InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]);
                InitializeItem(player.SpdList[0], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[0]);

                InitializeItem(player.SpdList[1], _item_indexes.IDI_HEAL);
                GenerateNewSeed(player.SpdList[1]);
                break;
            }

            Item &goldItem = player.InvList[player._pNumInv];
            MakeGoldStack(goldItem, 100);

            player._pNumInv++;
            player.InvGrid[30] = player._pNumInv;

            player._pGold = goldItem._ivalue;

            CalcPlrItemVals(player, false);
        }

        void InitializeItem(d2Item item, _item_indexes itemData)
        {
            auto &pAllItem = AllItemsList[static_cast<size_t>(itemData)];

            // zero-initialize struct
            item = {};

            item._itype = pAllItem.itype;
            item._iCurs = pAllItem.iCurs;
            CopyUtf8(item._iName, _(pAllItem.iName), sizeof(item._iName));
            CopyUtf8(item._iIName, _(pAllItem.iName), sizeof(item._iIName));
            item._iLoc = pAllItem.iLoc;
            item._iClass = pAllItem.iClass;
            item._iMinDam = pAllItem.iMinDam;
            item._iMaxDam = pAllItem.iMaxDam;
            item._iAC = pAllItem.iMinAC;
            item._iMiscId = pAllItem.iMiscId;
            item._iSpell = pAllItem.iSpell;

            if (pAllItem.iMiscId == IMISC_STAFF) {
                item._iCharges = gbIsHellfire ? 18 : 40;
            }

            item._iMaxCharges = item._iCharges;
            item._iDurability = pAllItem.iDurability;
            item._iMaxDur = pAllItem.iDurability;
            item._iMinStr = pAllItem.iMinStr;
            item._iMinMag = pAllItem.iMinMag;
            item._iMinDex = pAllItem.iMinDex;
            item._ivalue = pAllItem.iValue;
            item._iIvalue = pAllItem.iValue;
            item._iPrePower = IPL_INVALID;
            item._iSufPower = IPL_INVALID;
            item._iMagical = ITEM_QUALITY_NORMAL;
            item.IDidx = static_cast<_item_indexes>(itemData);
            if (gbIsHellfire)
                item.dwBuff |= CF_HELLFIRE;
        }


        /**
        * @brief Gets a value that represents the specified spellID in 64bit bitmask format.
        * For example:
        *  - spell ID  1: 0000.0000.0000.0000.0000.0000.0000.0000.0000.0000.0000.0000.0000.0000.0000.0001
        *  - spell ID 43: 0000.0000.0000.0000.0000.0100.0000.0000.0000.0000.0000.0000.0000.0000.0000.0000
        * @param spellId The id of the spell to get a bitmask for.
        * @return A 64bit bitmask representation for the specified spell.
        */
        public UInt64 GetSpellBitmask(spell_id spellId)
        {
            return GetSpellBitmask((int)spellId);
        }
        public UInt64 GetSpellBitmask(int spellId)
        {
            return 1ul << (spellId - 1);
        }

        public override void HitTarget(Unit target)
        {
            base.HitTarget(target);
            Debug.Log("xx-- d2Player.HitTarget");

            var mon = target as d2Monster;
            if (mon != null)
                PlayerHitMonster(this, mon);
        }

        // 近战伤害
        public int GetMeleeToHit()
        {
            int hper = _pLevel + _pDexterity / 2 + _pIBonusToHit + d2DEF.BaseHitChance;
            if (_pClass == HeroClass.Warrior)
                hper += 20;
            return hper;
        }

        // armor piercing: 穿甲
        public int GetMeleePiercingToHit()
        {
            int hper = GetMeleeToHit();
            // in hellfire armor piercing ignores % of enemy armor instead, no way to include it here
            if (!d2DEF.gbIsHellfire)
                hper += _pIEnAc;
            return hper;
        }

        // 返回怪物的护甲（在 hellfire 模式计算穿甲之后）
        public int CalculateArmorPierce(int monsterArmor, bool isMelee)
        {
            int tmac = monsterArmor;
            if (_pIEnAc > 0) 
            {
                if (d2DEF.gbIsHellfire) 
                {
                    int pIEnAc = _pIEnAc - 1;
                    if (pIEnAc > 0)
                        tmac >>= pIEnAc;
                    else
                        tmac -= tmac / 4;
                }
                if (isMelee && _pClass == HeroClass.Barbarian) 
                {
                    tmac -= monsterArmor / 8;
                }
            }
            if (tmac < 0)
                tmac = 0;

            return tmac;
        }

        // adjacentDamage: 临近伤害，可以理解为溅射伤害
        public bool PlayerHitMonster(d2Player player, d2Monster monster, bool adjacentDamage = false)
        {
            // 击中概率，用于 miss 判定
            // hper = hit percent?
            int hper = 0;
            
            if (!monster.isPossibleToHit)
                return false;

            if (adjacentDamage)
            {
                if (player._pLevel > 20)
                    hper -= 30;
                else
                    hper -= (35 - player._pLevel) * 2;
            }
            
            int hit = d2Utils.GenerateRnd(100);
            if (monster.mode == MonsterMode.Petrified) 
            {
                hit = 0;
            }

            hper += player.GetMeleePiercingToHit() - player.CalculateArmorPierce(monster.armorClass, true);
            hper = Mathf.Clamp(hper, 5, 95);

            // if (monster.tryLiftGargoyle())
            // {
            //     return true;
            // }

            Debug.Log($"xx-- PlayerHitMonster > hit:{hit} hper:{hper}");

            if (hit >= hper) {
        #if _DEBUG
                if (!DebugGodMode)
        #endif
                Debug.LogWarning("xx-- PlayerHitMonster miss");
                return false;
            }

            if (d2DEF.gbIsHellfire && d2Utils.HasAllOf(player._pIFlags, ItemSpecialEffect.FireDamage | ItemSpecialEffect.LightningDamage)) 
            {
                int midam = player._pIFMinDam + d2Utils.GenerateRnd(player._pIFMaxDam - player._pIFMinDam);
                // AddMissile(player.position.tile, player.position.temp, player._pdir, MIS_SPECARROW, TARGET_MONSTERS, player.getId(), midam, 0);
            }

            int mind = player._pIMinDam;
            int maxd = player._pIMaxDam;
            int dam = d2Utils.GenerateRnd(maxd - mind + 1) + mind;
            dam += dam * player._pIBonusDam / 100;
            dam += player._pIBonusDamMod;
            int dam2 = dam << 6;
            dam += player._pDamageMod;
            
            // 暴击
            if (player._pClass == HeroClass.Warrior || player._pClass == HeroClass.Barbarian) 
            {
                // 等级越高暴击概率越高
                if (d2Utils.GenerateRnd(100) < player._pLevel) 
                {
                    dam *= 2;
                }
            }

            ItemType phanditype = ItemType.None;
            if (player.InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._itype == ItemType.Sword || player.InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._itype == ItemType.Sword) 
            {
                phanditype = ItemType.Sword;
            }
            if (player.InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._itype == ItemType.Mace || player.InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._itype == ItemType.Mace) 
            {
                phanditype = ItemType.Mace;
            }

            switch (monster.data.monsterClass) 
            {
            case MonsterClass.Undead:
                if (phanditype == ItemType.Sword) {
                    dam -= dam / 2;
                } else if (phanditype == ItemType.Mace) {
                    dam += dam / 2;
                }
                break;
            case MonsterClass.Animal:
                if (phanditype == ItemType.Mace) {
                    dam -= dam / 2;
                } else if (phanditype == ItemType.Sword) {
                    dam += dam / 2;
                }
                break;
            case MonsterClass.Demon:
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.TripleDemonDamage)) {
                    dam *= 3;
                }
                break;
            }

            if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.Devastation) && d2Utils.GenerateRnd(100) < 5) {
                dam *= 3;
            }

            // if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.Doppelganger) && monster.type().type != MT_DIABLO && !monster.isUnique() && d2Utils.GenerateRnd(100) < 10) 
            // {
            //     AddDoppelganger(monster);
            // }

            dam <<= 6;
            if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.Jesters)) 
            {
                int r = d2Utils.GenerateRnd(201);
                if (r >= 100)
                    r = 100 + (r - 100) * 5;
                dam = dam * r / 100;
            }

            if (adjacentDamage)
                dam >>= 2;

            // 本地玩家
            // if (&player == MyPlayer) 
            {
                if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.Peril)) 
                {
                    dam2 += player._pIGetHit << 6;
                    if (dam2 >= 0)
                    {
                        ApplyPlrDamage(player, 0, 1, dam2);
                    }
                    dam *= 2;
                }
        #if _DEBUG
                if (DebugGodMode) {
                    dam = monster.hitPoints; /* ensure monster is killed with one hit */
                }
        #endif
                ApplyMonsterDamage(monster, dam);
            }

            int skdam = 0;
            if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.RandomStealLife)) 
            {
                skdam = d2Utils.GenerateRnd(dam / 8);
                player._pHitPoints += skdam;
                if (player._pHitPoints > player._pMaxHP) {
                    player._pHitPoints = player._pMaxHP;
                }
                player._pHPBase += skdam;
                if (player._pHPBase > player._pMaxHPBase) {
                    player._pHPBase = player._pMaxHPBase;
                }
                // RedrawComponent(PanelDrawComponent::Health);
            }

            // 窃取魔法效果
            if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealMana3 | ItemSpecialEffect.StealMana5) && d2Utils.HasNoneOf(player._pIFlags, ItemSpecialEffect.NoMana)) 
            {
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealMana3)) {
                    skdam = 3 * dam / 100;
                }
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealMana5)) {
                    skdam = 5 * dam / 100;
                }
                player._pMana += skdam;
                if (player._pMana > player._pMaxMana) 
                {
                    player._pMana = player._pMaxMana;
                }
                player._pManaBase += skdam;
                if (player._pManaBase > player._pMaxManaBase) 
                {
                    player._pManaBase = player._pMaxManaBase;
                }
                // RedrawComponent(PanelDrawComponent::Mana);
            }

            // 窃取 hp 
            if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealLife3 | ItemSpecialEffect.StealLife5)) 
            {
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealLife3)) {
                    skdam = 3 * dam / 100;
                }
                if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.StealLife5)) {
                    skdam = 5 * dam / 100;
                }
                player._pHitPoints += skdam;
                if (player._pHitPoints > player._pMaxHP) 
                {
                    player._pHitPoints = player._pMaxHP;
                }
                player._pHPBase += skdam;
                if (player._pHPBase > player._pMaxHPBase) 
                {
                    player._pHPBase = player._pMaxHPBase;
                }
                // RedrawComponent(PanelDrawComponent::Health);
            }

            // 播放特效
            if ((monster.hitPoints >> 6) <= 0)
            {
                // M_StartKill(monster, player);
            } 
            else 
            {
                // if (monster.mode != MonsterMode.Petrified && d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.Knockback))
                //     M_GetKnockback(monster);
                // M_StartHit(monster, player, dam);
            }
            return true;
        }

        void ApplyPlrDamage(d2Player player, int dam, int minHP = 0, int frac = 0, int earflag = 0)
        {
            // int totalDamage = (dam << 6) + frac;
            // if (totalDamage > 0 && player.pManaShield) 
            // {
            //     int8_t manaShieldLevel = player._pSplLvl[SPL_MANASHIELD];
            //     if (manaShieldLevel > 0) {
            //         totalDamage += totalDamage / -player.GetManaShieldDamageReduction();
            //     }
            //     if (&player == MyPlayer)
            //         RedrawComponent(PanelDrawComponent::Mana);
            //     if (player._pMana >= totalDamage) {
            //         player._pMana -= totalDamage;
            //         player._pManaBase -= totalDamage;
            //         totalDamage = 0;
            //     } else {
            //         totalDamage -= player._pMana;
            //         if (manaShieldLevel > 0) {
            //             totalDamage += totalDamage / (player.GetManaShieldDamageReduction() - 1);
            //         }
            //         player._pMana = 0;
            //         player._pManaBase = player._pMaxManaBase - player._pMaxMana;
            //         if (&player == MyPlayer)
            //             NetSendCmd(true, CMD_REMSHIELD);
            //     }
            // }

            // if (totalDamage == 0)
            //     return;

            // RedrawComponent(PanelDrawComponent::Health);
            // player._pHitPoints -= totalDamage;
            // player._pHPBase -= totalDamage;
            // if (player._pHitPoints > player._pMaxHP) {
            //     player._pHitPoints = player._pMaxHP;
            //     player._pHPBase = player._pMaxHPBase;
            // }
            // int minHitPoints = minHP << 6;
            // if (player._pHitPoints < minHitPoints) {
            //     SetPlayerHitPoints(player, minHitPoints);
            // }
            // if (player._pHitPoints >> 6 <= 0) {
            //     SyncPlrKill(player, earflag);
            // }
        }

        void ApplyMonsterDamage(d2Monster monster, int damage)
        {
            monster.hitPoints -= damage;

            if (monster.hitPoints >> 6 <= 0) 
            {
                // delta_kill_monster(monster, monster.position.tile, *MyPlayer);
                // NetSendCmdLocParam1(false, CMD_MONSTDEATH, monster.position.tile, monster.getId());
                return;
            }

            // delta_monster_hp(monster, *MyPlayer);
            // NetSendCmdMonDmg(false, monster.getId(), damage);
        }
    }
}

