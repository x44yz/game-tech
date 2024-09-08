using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace df
{
    public class dfPlayer : Unit
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
        public int _pLevel; // 角色等级 

        [Header("---- RUNTIME ----")]
        public int _pStrength;
        public int _pBaseStr;
        public int _pMagic;
        public int _pBaseMag;
        public int _pDexterity; // 敏捷
        public int _pBaseDex; 
        public int _pVitality; // 活力
        public int _pBaseVit;
        public int _pStatPts;
        public int _pDamageMod;
        public int _pBaseToBlk; // 闪避（对攻击和陷阱）
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
        public bool pManaShield; // 魔法盾
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
        public dfItem[] InvBody = new dfItem[(int)inv_body_loc.NUM_INVLOC];

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
        public int _pGold;
        public bool _pBlockFlag;
        public int _pISplLvlAdd;
        public bool _pInvincible; // 无敌的
        public PLR_MODE _pmode;
        public ActorPosition position = new ActorPosition();

        protected override void OnStart()
        {
            base.OnStart();

            int newLvl = _pLevel;
            InitPlayer(_pClass);
            
            int lvCount = newLvl - _pLevel ;
            for (int i = 0; i < lvCount; ++i)
            {
                NextPlrLevel();
            }

            TestRefreshEquip();
        }

        public int animationFrame = 0;
        public int animationFrameTick = 0;
        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

            // update animation
            if (animationFrame > 0)
            {
                animationFrameTick += 1;
                // Debug.Log("xx-- update ani frame > " + animationFrame);
            }

            // update state
            bool tplayer = false;
            do {
                switch (_pmode) {
                case PLR_MODE.PM_STAND:
                case PLR_MODE.PM_NEWLVL:
                case PLR_MODE.PM_QUIT:
                    tplayer = false;
                    break;
                case PLR_MODE.PM_WALK_NORTHWARDS:
                case PLR_MODE.PM_WALK_SOUTHWARDS:
                case PLR_MODE.PM_WALK_SIDEWAYS:
                    // tplayer = DoWalk(player, _pmode);
                    tplayer = false;
                    break;
                case PLR_MODE.PM_ATTACK:
                    tplayer = DoAttack();
                    break;
                // case PLR_MODE.PM_RATTACK:
                //     tplayer = DoRangeAttack(player);
                //     break;
                // case PLR_MODE.PM_BLOCK:
                //     tplayer = DoBlock(player);
                //     break;
                // case PLR_MODE.PM_SPELL:
                //     tplayer = DoSpell(player);
                //     break;
                case PLR_MODE.PM_GOTHIT:
                    tplayer = DoGotHit();
                    break;
                // case PLR_MODE.PM_DEATH:
                //     tplayer = DoDeath(player);
                //     break;
                }
                // CheckNewPath(player, tplayer);
            } while (tplayer);
        }

        private void InitPlayer(HeroClass c)
        {
            _pClass = c;

            int ic = (int)c;

            _pBaseStr = StrengthTbl[ic];
            _pStrength = _pBaseStr;

            _pBaseMag = MagicTbl[ic];
            _pMagic = _pBaseMag;

            _pBaseDex = DexterityTbl[ic];
            _pDexterity = _pBaseDex;

            _pBaseVit = VitalityTbl[ic];
            _pVitality = _pBaseVit;

            _pStatPts = 0;
            pTownWarps = 0;
            pDungMsgs = 0;
            pDungMsgs2 = 0;
            pLvlLoad = 0;
            pDiabloKillLevel = 0;
            pDifficulty = _difficulty.DIFF_NORMAL;

            _pLevel = 1;

            _pBaseToBlk = BlockBonuses[ic];

            _pHitPoints = (_pVitality + 10) << dfDEF.HPMANAOFFSET;
            if (_pClass == HeroClass.Warrior || _pClass == HeroClass.Barbarian) {
                _pHitPoints *= 2;
            } else if (_pClass == HeroClass.Rogue || _pClass == HeroClass.Monk || _pClass == HeroClass.Bard) {
                _pHitPoints += _pHitPoints / 2;
            }

            _pMaxHP = _pHitPoints;
            _pHPBase = _pHitPoints;
            _pMaxHPBase = _pHitPoints;

            _pMana = _pMagic << dfDEF.HPMANAOFFSET;
            if (_pClass == HeroClass.Sorcerer) {
                _pMana *= 2;
            } else if (_pClass == HeroClass.Bard) {
                _pMana += _pMana * 3 / 4;
            } else if (_pClass == HeroClass.Rogue || _pClass == HeroClass.Monk) {
                _pMana += _pMana / 2;
            }

            _pMaxMana = _pMana;
            _pManaBase = _pMana;
            _pMaxManaBase = _pMana;

            _pMaxLvl = _pLevel;
            _pExperience = 0;
            _pNextExper = ExpLvlsTbl[1];
            _pArmorClass = 0;
            _pLightRad = 10;
            _pInfraFlag = false;

            _pRSplType = spell_type.RSPLTYPE_SKILL;
            if (c == HeroClass.Warrior) {
                _pAblSpells = GetSpellBitmask(spell_id.SPL_REPAIR);
                _pRSpell = spell_id.SPL_REPAIR;
            } else if (c == HeroClass.Rogue) {
                _pAblSpells = GetSpellBitmask(spell_id.SPL_DISARM);
                _pRSpell = spell_id.SPL_DISARM;
            } else if (c == HeroClass.Sorcerer) {
                _pAblSpells = GetSpellBitmask(spell_id.SPL_RECHARGE);
                _pRSpell = spell_id.SPL_RECHARGE;
            } else if (c == HeroClass.Monk) {
                _pAblSpells = GetSpellBitmask(spell_id.SPL_SEARCH);
                _pRSpell = spell_id.SPL_SEARCH;
            } else if (c == HeroClass.Bard) {
                _pAblSpells = GetSpellBitmask(spell_id.SPL_IDENTIFY);
                _pRSpell = spell_id.SPL_IDENTIFY;
            } else if (c == HeroClass.Barbarian) {
                _pAblSpells = GetSpellBitmask(spell_id.SPL_BLODBOIL);
                _pRSpell = spell_id.SPL_BLODBOIL;
            }

            if (c == HeroClass.Sorcerer) {
                _pMemSpells = GetSpellBitmask(spell_id.SPL_FIREBOLT);
                _pRSplType = spell_type.RSPLTYPE_SPELL;
                _pRSpell = spell_id.SPL_FIREBOLT;
            } else {
                _pMemSpells = 0;
            }

            for (int i = 0; i < _pSplLvl.Length; ++i) {
                _pSplLvl[i] = 0;
            }

            _pSpellFlags = SpellFlag.None;

            if (_pClass == HeroClass.Sorcerer) {
                _pSplLvl[((int)spell_id.SPL_FIREBOLT)] = 2;
            }

            // Initializing the hotkey bindings to no selection
            // std::fill(_pSplHotKey, _pSplHotKey + NumHotkeys, SPL_INVALID);

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
            // _pgfxnum = static_cast<uint8_t>(animWeaponId);

            for (int i = 0; i < _pLvlVisited.Length; ++i) {
                _pLvlVisited[i] = false;
            }

            for (int i = 0; i < 10; i++) {
                _pSLvlVisited[i] = false;
            }

            _pLvlChanging = false;
            pTownWarps = 0;
            pLvlLoad = 0;
            pBattleNet = false;
            pManaShield = false;
            pDamAcFlags = ItemSpecialEffectHf.None;
            wReflections = 0;

            InitDungMsgs();
            CreatePlrItems();
            // SetRndSeed(0);
        }

        void InitDungMsgs()
        {
            pDungMsgs = 0;
            pDungMsgs2 = 0;
        }

        public const int InventoryGridCells = 40;
        public const int MaxBeltItems = 8;
        public int[] InvGrid = new int[InventoryGridCells];
        public dfItem[] InvList = new dfItem[InventoryGridCells];
        public int _pNumInv;
        public dfItem[] SpdList = new dfItem[MaxBeltItems];
        void CreatePlrItems()
        {
            // var objInvBody = new GameObject("InvBody");
            // objInvBody.transform.SetParent(transform);
            // for (int i = 0; i < ((int)inv_body_loc.NUM_INVLOC); ++i)
            // {
            //     var item = d2Item.Create(objInvBody.transform);
            //     item.name = ((inv_body_loc)i).ToString();
            //     InvBody[i] = item;
            // }
            for (int i = 0; i < InvBody.Length; ++i) 
            {
                var item = new dfItem();
                InvBody[i] = item;
                item.clear();
            }

            // converting this to a for loop creates a `rep stosd` instruction,
            // so this probably actually was a memset
            for (int i = 0; i < InvGrid.Length; ++i)
                InvGrid[i] = 0;

            for (int i = 0; i < InvList.Length; ++i) 
            {
                var item = new dfItem();
                InvList[i] = item;
                item.clear();
            }

            _pNumInv = 0;

            for (int i = 0; i < SpdList.Length; ++i) 
            {
                var item = new dfItem();
                SpdList[i] = item;
                item.clear();
            }

            switch (_pClass) 
            {
            case HeroClass.Warrior:
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].InitializeItem(_item_indexes.IDI_WARRIOR);
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].GenerateNewSeed();

                InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].InitializeItem(_item_indexes.IDI_WARRSHLD);
                InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].GenerateNewSeed();

                {
                    dfItem club = new dfItem();
                    club.InitializeItem(_item_indexes.IDI_WARRCLUB);
                    club.GenerateNewSeed();
                    AutoPlaceItemInInventorySlot(0, club, true);
                }

                SpdList[0].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[0].GenerateNewSeed();

                SpdList[1].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[1].GenerateNewSeed();
                break;
            case HeroClass.Rogue:
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].InitializeItem(_item_indexes.IDI_ROGUE);
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].GenerateNewSeed();

                SpdList[0].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[0].GenerateNewSeed();

                SpdList[1].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[1].GenerateNewSeed();
                break;
            case HeroClass.Sorcerer:
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].InitializeItem(dfDEF.gbIsHellfire ? _item_indexes.IDI_SORCERER : _item_indexes.IDI_SORCERER_DIABLO);
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].GenerateNewSeed();

                SpdList[0].InitializeItem(dfDEF.gbIsHellfire ? _item_indexes.IDI_HEAL : _item_indexes.IDI_MANA);
                SpdList[0].GenerateNewSeed();

                SpdList[1].InitializeItem(dfDEF.gbIsHellfire ? _item_indexes.IDI_HEAL : _item_indexes.IDI_MANA);
                SpdList[1].GenerateNewSeed();
                break;

            case HeroClass.Monk:
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].InitializeItem(_item_indexes.IDI_SHORTSTAFF);
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].GenerateNewSeed();
                SpdList[0].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[0].GenerateNewSeed();

                SpdList[1].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[1].GenerateNewSeed();
                break;
            case HeroClass.Bard:
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].InitializeItem(_item_indexes.IDI_BARDSWORD);
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].GenerateNewSeed();

                InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].InitializeItem(_item_indexes.IDI_BARDDAGGER);
                InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].GenerateNewSeed();
                SpdList[0].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[0].GenerateNewSeed();

                SpdList[1].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[1].GenerateNewSeed();
                break;
            case HeroClass.Barbarian:
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].InitializeItem(_item_indexes.IDI_BARBARIAN);
                InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].GenerateNewSeed();

                InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].InitializeItem(_item_indexes.IDI_WARRSHLD);
                InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].GenerateNewSeed();
                SpdList[0].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[0].GenerateNewSeed();

                SpdList[1].InitializeItem(_item_indexes.IDI_HEAL);
                SpdList[1].GenerateNewSeed();
                break;
            }

            dfItem goldItem = InvList[_pNumInv];
            MakeGoldStack(goldItem, 100);

            _pNumInv++;
            InvGrid[30] = _pNumInv;

            _pGold = goldItem._ivalue;

            CalcPlrItemVals(false);
        }

        bool AutoPlaceItemInInventorySlot(int slotIndex, dfItem item, bool persistItem)
        {
            // int yy = (slotIndex > 0) ? (10 * (slotIndex / 10)) : 0;

            // Size itemSize = GetInventorySize(item);
            // for (int j = 0; j < itemSize.height; j++) {
            //     if (yy >= InventoryGridCells) {
            //         return false;
            //     }
            //     int xx = (slotIndex > 0) ? (slotIndex % 10) : 0;
            //     for (int i = 0; i < itemSize.width; i++) {
            //         if (xx >= 10 || InvGrid[xx + yy] != 0) {
            //             return false;
            //         }
            //         xx++;
            //     }
            //     yy += 10;
            // }

            // if (persistItem) {
            //     InvList[_pNumInv] = item;
            //     _pNumInv++;

            //     AddItemToInvGrid(player, slotIndex, _pNumInv, itemSize);
            //     CalcScrolls();
            // }

            return true;
        }

        void MakeGoldStack(dfItem goldItem, int value)
        {
            goldItem.InitializeItem(_item_indexes.IDI_GOLD);
            goldItem.GenerateNewSeed();
            goldItem._iStatFlag = true;
            goldItem._ivalue = value;
            // SetPlrHandGoldCurs(goldItem);
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

        // public override void HitTarget(Unit target)
        // {
        //     base.HitTarget(target);
        //     Debug.Log("xx-- d2HitTarget");

        //     var mon = target as dfMonster;
        //     if (mon != null)
        //         PlrHitMonster(mon);
        // }

        public int GetArmor()
        {
            return _pIBonusAC + _pIAC + _pDexterity / 5;
        }

        // 近战伤害
        public int GetMeleeToHit()
        {
            int hper = _pLevel + _pDexterity / 2 + _pIBonusToHit + dfDEF.BaseHitChance;
            if (_pClass == HeroClass.Warrior)
                hper += 20;
            return hper;
        }

        // armor piercing: 穿甲
        public int GetMeleePiercingToHit()
        {
            int hper = GetMeleeToHit();
            // in hellfire armor piercing ignores % of enemy armor instead, no way to include it here
            if (!dfDEF.gbIsHellfire)
                hper += _pIEnAc;
            return hper;
        }

        // 返回怪物的护甲（在 hellfire 模式计算穿甲之后）
        public int CalculateArmorPierce(int monsterArmor, bool isMelee)
        {
            int tmac = monsterArmor;
            if (_pIEnAc > 0) 
            {
                if (dfDEF.gbIsHellfire) 
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

        public void AttackMonster(dfMonster monster)
        {
            StartPlayerAttack();
            PlrHitMonster(monster);
        }

        // adjacentDamage: 临近伤害，可以理解为溅射伤害
        public bool PlrHitMonster(dfMonster monster, bool adjacentDamage = false)
        {
            // 击中概率，用于 miss 判定
            // hper = hit percent?
            int hper = 0;
            
            if (!monster.isPossibleToHit)
                return false;

            if (adjacentDamage)
            {
                if (_pLevel > 20)
                    hper -= 30;
                else
                    hper -= (35 - _pLevel) * 2;
            }
            
            int hit = dfUtils.GenerateRnd(100);
            // 石化状态，必然命中
            if (monster.mode == MonsterMode.Petrified) 
            {
                hit = 0;
            }

            hper += GetMeleePiercingToHit() - CalculateArmorPierce(monster.armorClass, true);
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
                // Debug.LogWarning("xx-- PlayerHitMonster miss");
                dfTest.Inst.ShowMiss(monster);
                return false;
            }

            if (dfDEF.gbIsHellfire && dfUtils.HasAllOf(_pIFlags, ItemSpecialEffect.FireDamage | ItemSpecialEffect.LightningDamage)) 
            {
                int midam = _pIFMinDam + dfUtils.GenerateRnd(_pIFMaxDam - _pIFMinDam);
                // AddMissile(position.tile, position.temp, _pdir, MIS_SPECARROW, TARGET_MONSTERS, getId(), midam, 0);
            }

            int mind = _pIMinDam;
            int maxd = _pIMaxDam;
            int dam = dfUtils.GenerateRnd(maxd - mind + 1) + mind;
            dam += dam * _pIBonusDam / 100;
            dam += _pIBonusDamMod;
            int dam2 = dam << dfDEF.HPMANAOFFSET;
            dam += _pDamageMod;
            
            // 暴击
            if (_pClass == HeroClass.Warrior || _pClass == HeroClass.Barbarian) 
            {
                // 等级越高暴击概率越高
                if (dfUtils.GenerateRnd(100) < _pLevel) 
                {
                    dam *= 2;
                }
            }

            ItemType phanditype = ItemType.None;
            if (InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._itype == ItemType.Sword || InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._itype == ItemType.Sword) 
            {
                phanditype = ItemType.Sword;
            }
            if (InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._itype == ItemType.Mace || InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._itype == ItemType.Mace) 
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
                if (dfUtils.HasAnyOf(_pIFlags, ItemSpecialEffect.TripleDemonDamage)) {
                    dam *= 3;
                }
                break;
            }

            if (dfUtils.HasAnyOf(pDamAcFlags, ItemSpecialEffectHf.Devastation) && dfUtils.GenerateRnd(100) < 5) {
                dam *= 3;
            }

            // if (d2Utils.HasAnyOf(pDamAcFlags, ItemSpecialEffectHf.Doppelganger) && monster.type().type != MT_DIABLO && !monster.isUnique() && d2Utils.GenerateRnd(100) < 10) 
            // {
            //     AddDoppelganger(monster);
            // }

            dam <<= dfDEF.HPMANAOFFSET;
            if (dfUtils.HasAnyOf(pDamAcFlags, ItemSpecialEffectHf.Jesters)) 
            {
                int r = dfUtils.GenerateRnd(201);
                if (r >= 100)
                    r = 100 + (r - 100) * 5;
                dam = dam * r / 100;
            }

            if (adjacentDamage)
                dam >>= 2;

            // 本地玩家
            // if (&player == MyPlayer) 
            {
                if (dfUtils.HasAnyOf(pDamAcFlags, ItemSpecialEffectHf.Peril)) 
                {
                    dam2 += _pIGetHit << dfDEF.HPMANAOFFSET;
                    if (dam2 >= 0)
                    {
                        ApplyPlrDamage(0, 1, dam2);
                    }
                    dam *= 2;
                }
        #if _DEBUG
                if (DebugGodMode) {
                    dam = monster.hitPoints; /* ensure monster is killed with one hit */
                }
        #endif
                monster.ApplyMonsterDamage(dam);
            }

            int skdam = 0;
            if (dfUtils.HasAnyOf(_pIFlags, ItemSpecialEffect.RandomStealLife)) 
            {
                skdam = dfUtils.GenerateRnd(dam / 8);
                _pHitPoints += skdam;
                if (_pHitPoints > _pMaxHP) {
                    _pHitPoints = _pMaxHP;
                }
                _pHPBase += skdam;
                if (_pHPBase > _pMaxHPBase) {
                    _pHPBase = _pMaxHPBase;
                }
                // RedrawComponent(PanelDrawComponent::Health);
            }

            // 窃取魔法效果
            if (dfUtils.HasAnyOf(_pIFlags, ItemSpecialEffect.StealMana3 | ItemSpecialEffect.StealMana5) && dfUtils.HasNoneOf(_pIFlags, ItemSpecialEffect.NoMana)) 
            {
                if (dfUtils.HasAnyOf(_pIFlags, ItemSpecialEffect.StealMana3)) {
                    skdam = 3 * dam / 100;
                }
                if (dfUtils.HasAnyOf(_pIFlags, ItemSpecialEffect.StealMana5)) {
                    skdam = 5 * dam / 100;
                }
                _pMana += skdam;
                if (_pMana > _pMaxMana) 
                {
                    _pMana = _pMaxMana;
                }
                _pManaBase += skdam;
                if (_pManaBase > _pMaxManaBase) 
                {
                    _pManaBase = _pMaxManaBase;
                }
                // RedrawComponent(PanelDrawComponent::Mana);
            }

            // 窃取 hp 
            if (dfUtils.HasAnyOf(_pIFlags, ItemSpecialEffect.StealLife3 | ItemSpecialEffect.StealLife5)) 
            {
                if (dfUtils.HasAnyOf(_pIFlags, ItemSpecialEffect.StealLife3)) {
                    skdam = 3 * dam / 100;
                }
                if (dfUtils.HasAnyOf(_pIFlags, ItemSpecialEffect.StealLife5)) {
                    skdam = 5 * dam / 100;
                }
                _pHitPoints += skdam;
                if (_pHitPoints > _pMaxHP) 
                {
                    _pHitPoints = _pMaxHP;
                }
                _pHPBase += skdam;
                if (_pHPBase > _pMaxHPBase) 
                {
                    _pHPBase = _pMaxHPBase;
                }
                // RedrawComponent(PanelDrawComponent::Health);
            }

            // 播放特效
            if ((monster.hitPoints >> dfDEF.HPMANAOFFSET) <= 0)
            {
                monster.M_StartKill(this, dam);
            } 
            else 
            {
                // if (monster.mode != MonsterMode.Petrified && d2Utils.HasAnyOf(_pIFlags, ItemSpecialEffect.Knockback))
                //     M_GetKnockback(monster);
                monster.M_StartHit(this, dam);
            }
            return true;
        }

        public int GetManaShieldDamageReduction()
        {
            int Max = 7;
            // 3 - 21
            return 24 - Math.Min(_pSplLvl[(int)spell_id.SPL_MANASHIELD], Max) * 3;
        }

        public void ApplyPlrDamage(int dam, int minHP = 0, int frac = 0, int earflag = 0)
        {
            int totalDamage = (dam << dfDEF.HPMANAOFFSET) + frac;
            if (totalDamage > 0 && pManaShield) 
            {
                int manaShieldLevel = _pSplLvl[(int)spell_id.SPL_MANASHIELD];
                if (manaShieldLevel > 0) 
                {
                    totalDamage += totalDamage / -GetManaShieldDamageReduction();
                }
                // if (&player == MyPlayer)
                    // RedrawComponent(PanelDrawComponent::Mana);
                if (_pMana >= totalDamage) 
                {
                    _pMana -= totalDamage;
                    _pManaBase -= totalDamage;
                    totalDamage = 0;
                } 
                else 
                {
                    totalDamage -= _pMana;
                    if (manaShieldLevel > 0) {
                        totalDamage += totalDamage / (GetManaShieldDamageReduction() - 1);
                    }
                    _pMana = 0;
                    _pManaBase = _pMaxManaBase - _pMaxMana;
                    // if (&player == MyPlayer)
                    //     NetSendCmd(true, CMD_REMSHIELD);
                }
            }

            if (totalDamage == 0)
                return;

            // RedrawComponent(PanelDrawComponent::Health);
            _pHitPoints -= totalDamage;
            _pHPBase -= totalDamage;
            if (_pHitPoints > _pMaxHP) {
                _pHitPoints = _pMaxHP;
                _pHPBase = _pMaxHPBase;
            }
            int minHitPoints = minHP << dfDEF.HPMANAOFFSET;
            if (_pHitPoints < minHitPoints) {
                SetPlayerHitPoints(minHitPoints);
            }
            if (_pHitPoints >> dfDEF.HPMANAOFFSET <= 0) {
                SyncPlrKill(earflag);
            }
        }

        void SyncPlrKill(int earflag)
        {
            if (_pHitPoints <= 0 && dfDEF.leveltype == dungeon_type.DTYPE_TOWN) 
            {
                SetPlayerHitPoints(64);
                return;
            }

            SetPlayerHitPoints(0);
            StartPlayerKill(earflag);
        }

        bool IsValidSpell(spell_id spl)
        {
            return spl > spell_id.SPL_NULL
                && spl <= spell_id.SPL_LAST
                && (spl <= spell_id.SPL_LASTDIABLO || dfDEF.gbIsHellfire);
        }

        public const int MaxResistance = 75;
        public const int GOLD_MAX_LIMIT = 5000;
        public int MaxGold = GOLD_MAX_LIMIT;
        void CalcPlrItemVals(bool loadgfx)
        {
            int mind = 0; // min damage
            int maxd = 0; // max damage
            int tac = 0;  // accuracy

            int bdam = 0;   // bonus damage
            int btohit = 0; // bonus chance to hit
            int bac = 0;    // bonus accuracy

            ItemSpecialEffect iflgs = ItemSpecialEffect.None; // item_special_effect flags

            ItemSpecialEffectHf pDamAcFlags = ItemSpecialEffectHf.None;

            int sadd = 0; // added strength
            int madd = 0; // added magic
            int dadd = 0; // added dexterity
            int vadd = 0; // added vitality

            UInt64 spl = 0; // bitarray for all enabled/active spells

            int fr = 0; // fire resistance
            int lr = 0; // lightning resistance
            int mr = 0; // magic resistance

            int dmod = 0; // bonus damage mod?
            int ghit = 0; // increased damage from enemies

            int lrad = 10; // light radius

            int ihp = 0;   // increased HP
            int imana = 0; // increased mana

            int spllvladd = 0; // increased spell level
            int enac = 0;      // enhanced accuracy

            int fmin = 0; // minimum fire damage
            int fmax = 0; // maximum fire damage
            int lmin = 0; // minimum lightning damage
            int lmax = 0; // maximum lightning damage

            foreach (var item in InvBody) 
            {
                if (!item.isEmpty() && item._iStatFlag) {

                    mind += item._iMinDam;
                    maxd += item._iMaxDam;
                    tac += item._iAC;

                    if (IsValidSpell(item._iSpell)) {
                        spl |= GetSpellBitmask(item._iSpell);
                    }

                    if (item._iMagical == item_quality.ITEM_QUALITY_NORMAL || item._iIdentified) {
                        bdam += item._iPLDam;
                        btohit += item._iPLToHit;
                        if (item._iPLAC != 0) {
                            int tmpac = item._iAC;
                            tmpac *= item._iPLAC;
                            tmpac /= 100;
                            if (tmpac == 0)
                                tmpac = Math.Sign(item._iPLAC);
                            bac += tmpac;
                        }
                        iflgs |= item._iFlags;
                        pDamAcFlags |= item._iDamAcFlags;
                        sadd += item._iPLStr;
                        madd += item._iPLMag;
                        dadd += item._iPLDex;
                        vadd += item._iPLVit;
                        fr += item._iPLFR;
                        lr += item._iPLLR;
                        mr += item._iPLMR;
                        dmod += item._iPLDamMod;
                        ghit += item._iPLGetHit;
                        lrad += item._iPLLight;
                        ihp += item._iPLHP;
                        imana += item._iPLMana;
                        spllvladd += item._iSplLvlAdd;
                        enac += item._iPLEnAc;
                        fmin += item._iFMinDam;
                        fmax += item._iFMaxDam;
                        lmin += item._iLMinDam;
                        lmax += item._iLMaxDam;
                    }
                }
            }

            if (mind == 0 && maxd == 0) {
                mind = 1;
                maxd = 1;

                if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Shield && InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._iStatFlag) {
                    maxd = 3;
                }

                if (InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Shield && InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._iStatFlag) {
                    maxd = 3;
                }

                if (_pClass == HeroClass.Monk) {
                    mind = Math.Max(mind, _pLevel / 2);
                    maxd = Math.Max(maxd, (int)_pLevel);
                }
            }

            if (dfUtils.HasAnyOf(_pSpellFlags, SpellFlag.RageActive)) {
                sadd += 2 * _pLevel;
                dadd += _pLevel + _pLevel / 2;
                vadd += 2 * _pLevel;
            }
            if (dfUtils.HasAnyOf(_pSpellFlags, SpellFlag.RageCooldown)) {
                sadd -= 2 * _pLevel;
                dadd -= _pLevel + _pLevel / 2;
                vadd -= 2 * _pLevel;
            }

            _pIMinDam = mind;
            _pIMaxDam = maxd;
            _pIAC = tac;
            _pIBonusDam = bdam;
            _pIBonusToHit = btohit;
            _pIBonusAC = bac;
            _pIFlags = iflgs;
            this.pDamAcFlags = pDamAcFlags;
            _pIBonusDamMod = dmod;
            _pIGetHit = ghit;

            lrad = Mathf.Clamp(lrad, 2, 15);

            if (_pLightRad != lrad) {
                // ChangeLightRadius(_plid, lrad);
                // ChangeVisionRadius(_pvid, lrad);
                _pLightRad = lrad;
            }

            _pStrength = Math.Max(0, sadd + _pBaseStr);
            _pMagic = Math.Max(0, madd + _pBaseMag);
            _pDexterity = Math.Max(0, dadd + _pBaseDex);
            _pVitality = Math.Max(0, vadd + _pBaseVit);

            if (_pClass == HeroClass.Rogue) {
                _pDamageMod = _pLevel * (_pStrength + _pDexterity) / 200;
            } else if (_pClass == HeroClass.Monk) {
                _pDamageMod = _pLevel * (_pStrength + _pDexterity) / 150;
                if ((!InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].isEmpty() && InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype != ItemType.Staff) || (!InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].isEmpty() && InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype != ItemType.Staff))
                    _pDamageMod /= 2; // Monks get half the normal damage bonus if they're holding a non-staff weapon
            } else if (_pClass == HeroClass.Bard) {
                if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Sword || InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Sword)
                    _pDamageMod = _pLevel * (_pStrength + _pDexterity) / 150;
                else if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Bow || InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Bow) {
                    _pDamageMod = _pLevel * (_pStrength + _pDexterity) / 250;
                } else {
                    _pDamageMod = _pLevel * _pStrength / 100;
                }
            } else if (_pClass == HeroClass.Barbarian) {

                if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Axe || InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Axe) {
                    _pDamageMod = _pLevel * _pStrength / 75;
                } else if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Mace || InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Mace) {
                    _pDamageMod = _pLevel * _pStrength / 75;
                } else if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Bow || InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Bow) {
                    _pDamageMod = _pLevel * _pStrength / 300;
                } else {
                    _pDamageMod = _pLevel * _pStrength / 100;
                }

                if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Shield || InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Shield) {
                    if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Shield)
                        _pIAC -= InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._iAC / 2;
                    else if (InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Shield)
                        _pIAC -= InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._iAC / 2;
                } else if (dfUtils.IsNoneOf(InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype, ItemType.Staff, ItemType.Bow) && dfUtils.IsNoneOf(InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype, ItemType.Staff, ItemType.Bow)) {
                    _pDamageMod += _pLevel * _pVitality / 100;
                }
                _pIAC += _pLevel / 4;
            } else {
                _pDamageMod = _pLevel * _pStrength / 100;
            }

            _pISpells = spl;

            // EnsureValidReadiedSpell(player);

            _pISplLvlAdd = spllvladd;
            _pIEnAc = enac;

            if (_pClass == HeroClass.Barbarian) {
                mr += _pLevel;
                fr += _pLevel;
                lr += _pLevel;
            }

            if (dfUtils.HasAnyOf(_pSpellFlags, SpellFlag.RageCooldown)) {
                mr -= _pLevel;
                fr -= _pLevel;
                lr -= _pLevel;
            }

            if (dfUtils.HasAnyOf(iflgs, ItemSpecialEffect.ZeroResistance)) {
                // reset resistances to zero if the respective special effect is active
                mr = 0;
                fr = 0;
                lr = 0;
            }

            _pMagResist = Mathf.Clamp(mr, 0, MaxResistance);
            _pFireResist = Mathf.Clamp(fr, 0, MaxResistance);
            _pLghtResist = Mathf.Clamp(lr, 0, MaxResistance);

            if (_pClass == HeroClass.Warrior) {
                vadd *= 2;
            } else if (_pClass == HeroClass.Barbarian) {
                vadd += vadd;
                vadd += (vadd / 4);
            } else if (dfUtils.IsAnyOf(_pClass, HeroClass.Rogue, HeroClass.Monk, HeroClass.Bard)) {
                vadd += vadd / 2;
            }
            ihp += (vadd << dfDEF.HPMANAOFFSET); // BUGFIX: blood boil can cause negative shifts here (see line 757)

            if (_pClass == HeroClass.Sorcerer) {
                madd *= 2;
            }
            if (dfUtils.IsAnyOf(_pClass, HeroClass.Rogue, HeroClass.Monk)) {
                madd += madd / 2;
            } else if (_pClass == HeroClass.Bard) {
                madd += (madd / 4) + (madd / 2);
            }
            imana += (madd << dfDEF.HPMANAOFFSET);

            _pMaxHP = ihp + _pMaxHPBase;
            _pHitPoints = Math.Min(ihp + _pHPBase, _pMaxHP);

            if (/*&player == MyPlayer &&*/ (_pHitPoints >> dfDEF.HPMANAOFFSET) <= 0) {
                SetPlayerHitPoints(0);
            }

            _pMaxMana = imana + _pMaxManaBase;
            _pMana = Math.Min(imana + _pManaBase, _pMaxMana);         
            _pIFMinDam = fmin;
            _pIFMaxDam = fmax;
            _pILMinDam = lmin;
            _pILMaxDam = lmax;

            _pInfraFlag = false;

            _pBlockFlag = false;
            if (_pClass == HeroClass.Monk) 
            {
                if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Staff && InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._iStatFlag) {
                    _pBlockFlag = true;
                    _pIFlags |= ItemSpecialEffect.FastBlock;
                }
                if (InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Staff && InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._iStatFlag) {
                    _pBlockFlag = true;
                    _pIFlags |= ItemSpecialEffect.FastBlock;
                }
                if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].isEmpty() && InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].isEmpty())
                    _pBlockFlag = true;
                if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._iClass == item_class.ICLASS_WEAPON && GetItemLocation(InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]) != item_equip_type.ILOC_TWOHAND && InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].isEmpty())
                    _pBlockFlag = true;
                if (InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._iClass == item_class.ICLASS_WEAPON && GetItemLocation(InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]) != item_equip_type.ILOC_TWOHAND && InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].isEmpty())
                    _pBlockFlag = true;
            }

            ItemType weaponItemType = ItemType.None;
            bool holdsShield = false;
            if (!InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)].isEmpty()
                && InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._iClass == item_class.ICLASS_WEAPON
                && InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._iStatFlag) {
                weaponItemType = InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype;
            }

            if (!InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)].isEmpty()
                && InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._iClass == item_class.ICLASS_WEAPON
                && InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._iStatFlag) {
                weaponItemType = InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype;
            }

            if (InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._itype == ItemType.Shield && InvBody[((int)inv_body_loc.INVLOC_HAND_LEFT)]._iStatFlag) {
                _pBlockFlag = true;
                holdsShield = true;
            }
            if (InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._itype == ItemType.Shield && InvBody[((int)inv_body_loc.INVLOC_HAND_RIGHT)]._iStatFlag) {
                _pBlockFlag = true;
                holdsShield = true;
            }

            // PlayerWeaponGraphic animWeaponId = holdsShield ? PlayerWeaponGraphic::UnarmedShield : PlayerWeaponGraphic::Unarmed;
            // switch (weaponItemType) {
            // case ItemType.Sword:
            //     animWeaponId = holdsShield ? PlayerWeaponGraphic::SwordShield : PlayerWeaponGraphic::Sword;
            //     break;
            // case ItemType.Axe:
            //     animWeaponId = PlayerWeaponGraphic::Axe;
            //     break;
            // case ItemType.Bow:
            //     animWeaponId = PlayerWeaponGraphic::Bow;
            //     break;
            // case ItemType.Mace:
            //     animWeaponId = holdsShield ? PlayerWeaponGraphic::MaceShield : PlayerWeaponGraphic::Mace;
            //     break;
            // case ItemType.Staff:
            //     animWeaponId = PlayerWeaponGraphic::Staff;
            //     break;
            // default:
            //     break;
            // }

            // PlayerArmorGraphic animArmorId = PlayerArmorGraphic::Light;
            if (InvBody[((int)inv_body_loc.INVLOC_CHEST)]._itype == ItemType.HeavyArmor && InvBody[((int)inv_body_loc.INVLOC_CHEST)]._iStatFlag) {
                if (_pClass == HeroClass.Monk && InvBody[((int)inv_body_loc.INVLOC_CHEST)]._iMagical == item_quality.ITEM_QUALITY_UNIQUE)
                    _pIAC += _pLevel / 2;
                // animArmorId = PlayerArmorGraphic::Heavy;
            } else if (InvBody[((int)inv_body_loc.INVLOC_CHEST)]._itype == ItemType.MediumArmor && InvBody[((int)inv_body_loc.INVLOC_CHEST)]._iStatFlag) {
                if (_pClass == HeroClass.Monk) {
                    if (InvBody[((int)inv_body_loc.INVLOC_CHEST)]._iMagical == item_quality.ITEM_QUALITY_UNIQUE)
                        _pIAC += _pLevel * 2;
                    else
                        _pIAC += _pLevel / 2;
                }
                // animArmorId = PlayerArmorGraphic::Medium;
            } else if (_pClass == HeroClass.Monk) {
                _pIAC += _pLevel * 2;
            }

            // const uint8_t gfxNum = static_cast<uint8_t>(animWeaponId) | static_cast<uint8_t>(animArmorId);
            // if (_pgfxnum != gfxNum && loadgfx) {
            //     _pgfxnum = gfxNum;
            //     ResetPlayerGFX(player);
            //     SetPlrAnims(player);
            //     previewCelSprite = std::nullopt;
            //     player_graphic graphic = getGraphic();
            //     int8_t numberOfFrames;
            //     int8_t ticksPerFrame;
            //     getAnimationFramesAndTicksPerFrame(graphic, numberOfFrames, ticksPerFrame);
            //     LoadPlrGFX(player, graphic);
            //     AnimInfo.changeAnimationData(AnimationData[static_cast<size_t>(graphic)].spritesForDirection(_pdir), numberOfFrames, ticksPerFrame);
            // } else {
            //     _pgfxnum = gfxNum;
            // }

            // if (&player == MyPlayer) 
            {
                // if (InvBody[((int)inv_body_loc.INVLOC_AMULET)].isEmpty() || InvBody[((int)inv_body_loc.INVLOC_AMULET)].IDidx != _item_indexes.IDI_AURIC) {
                //     int half = MaxGold;
                //     MaxGold = GOLD_MAX_LIMIT;

                //     if (half != MaxGold)
                //         StripTopGold(player);
                // } else {
                //     MaxGold = GOLD_MAX_LIMIT * 2;
                // }
            }

            // RedrawComponent(PanelDrawComponent::Mana);
            // RedrawComponent(PanelDrawComponent::Health);
        }

        void SetPlayerHitPoints(int val)
        {
            _pHitPoints = val;
            _pHPBase = val + _pMaxHPBase - _pMaxHP;

            // if (&player == MyPlayer) {
            //     RedrawComponent(PanelDrawComponent::Health);
            // }
        }

        item_equip_type GetItemLocation(dfItem item)
        {
            if (_pClass == HeroClass.Barbarian && item._iLoc == item_equip_type.ILOC_TWOHAND && dfUtils.IsAnyOf(item._itype, ItemType.Sword, ItemType.Mace))
                return item_equip_type.ILOC_ONEHAND;
            return item._iLoc;
        }

        void NextPlrLevel()
        {
            _pLevel++;
            _pMaxLvl++;

            CalcPlrInv(true);

            if (CalcStatDiff() < 5) 
            {
                _pStatPts = CalcStatDiff();
            } else {
                _pStatPts += 5;
            }

            _pNextExper = ExpLvlsTbl[_pLevel];

            int hp = _pClass == HeroClass.Sorcerer ? 64 : 128;

            _pMaxHP += hp;
            _pHitPoints = _pMaxHP;
            _pMaxHPBase += hp;
            _pHPBase = _pMaxHPBase;

            // if (&player == MyPlayer) {
            //     RedrawComponent(PanelDrawComponent::Health);
            // }

            int mana = 128;
            if (_pClass == HeroClass.Warrior)
                mana = 64;
            else if (_pClass == HeroClass.Barbarian)
                mana = 0;

            _pMaxMana += mana;
            _pMaxManaBase += mana;

            if (dfUtils.HasNoneOf(_pIFlags, ItemSpecialEffect.NoMana)) {
                _pMana = _pMaxMana;
                _pManaBase = _pMaxManaBase;
            }

            // if (&player == MyPlayer) {
            //     RedrawComponent(PanelDrawComponent::Mana);
            // }

            // if (ControlMode != ControlTypes::KeyboardAndMouse)
            //     FocusOnCharInfo();

            CalcPlrInv(true);
        }

        void CalcPlrInv(bool loadgfx)
        {
            // Determine the players current stats, this updates the statFlag on all equipped items that became unusable after
            //  a change in equipment.
            CalcSelfItems();

            // Determine the current item bonuses gained from usable equipped items
            CalcPlrItemVals(loadgfx);

            // if (&player == MyPlayer) 
            {
                // Now that stat gains from equipped items have been calculated, mark unusable scrolls etc
                // for (Item &item : InventoryAndBeltPlayerItemsRange { player }) 
                // {
                //     item.updateRequiredStatsCacheForPlayer(player);
                // }
                // CalcScrolls();
                // CalcPlrStaff(player);
                // if (IsStashOpen) {
                //     // If stash is open, ensure the items are displayed correctly
                //     Stash.RefreshItemStatFlags();
                // }
            }
        }

        void CalcSelfItems()
        {
            int sa = 0;
            int ma = 0;
            int da = 0;

            // first iteration is used for collecting stat bonuses from items
            // for (Item &equipment : EquippedPlayerItemsRange(player)) {
            for (int i = 0; i < InvBody.Length; ++i)
            {
                var equipment = InvBody[i];
                if (equipment == null || equipment.isEmpty())
                    continue;

                equipment._iStatFlag = true;
                if (equipment._iIdentified) {
                    sa += equipment._iPLStr;
                    ma += equipment._iPLMag;
                    da += equipment._iPLDex;
                }
            }

            bool changeflag;
            do {
                // cap stats to 0
                int currstr = Math.Max(0, sa + _pBaseStr);
                int currmag = Math.Max(0, ma + _pBaseMag);
                int currdex = Math.Max(0, da + _pBaseDex);

                changeflag = false;
                // for (Item &equipment : EquippedPlayerItemsRange(player)) {
                for (int i = 0; i < InvBody.Length; ++i)
                {
                    var equipment = InvBody[i];
                    if (equipment == null || equipment.isEmpty())
                        continue;

                    if (!equipment._iStatFlag)
                        continue;

                    if (currstr >= equipment._iMinStr
                        && currmag >= equipment._iMinMag
                        && currdex >= equipment._iMinDex)
                        continue;

                    changeflag = true;
                    equipment._iStatFlag = false;
                    if (equipment._iIdentified) {
                        sa -= equipment._iPLStr;
                        ma -= equipment._iPLMag;
                        da -= equipment._iPLDex;
                    }
                }
            } while (changeflag);
        }

        int CalcStatDiff()
        {
            int diff = 0;
            for (int i = (int)CharacterAttribute.FIRST; i <= (int)CharacterAttribute.LAST; ++i)
            {
                var attribute = (CharacterAttribute)i;
                diff += GetMaximumAttributeValue(attribute);
                diff -= GetBaseAttributeValue(attribute);
            }
            return diff;
        }

        public static readonly int[,] MaxStats = {
            // clang-format off
            { 250,  50,  60, 100 },
            {  55,  70, 250,  80 },
            {  45, 250,  85,  80 },
            { 150,  80, 150,  80 },
            { 120, 120, 120, 100 },
            { 255,   0,  55, 150 },
            // clang-format on
        };
        int GetMaximumAttributeValue(CharacterAttribute attribute)
        {
            return MaxStats[(int)_pClass, (int)attribute];
        }
        int GetBaseAttributeValue(CharacterAttribute attribute)
        {
            switch (attribute) {
            case CharacterAttribute.Dexterity:
                return _pBaseDex;
            case CharacterAttribute.Magic:
                return _pBaseMag;
            case CharacterAttribute.Strength:
                return _pBaseStr;
            case CharacterAttribute.Vitality:
                return _pBaseVit;
            default:
                Debug.LogError("Unsupported attribute");
                break;
            }
            return 0;
        }

        void ChangePlayerItems(inv_body_loc bodyLocation, _item_indexes idx)
        {
            var item = InvBody[(int)bodyLocation];
            int curlvl = 1;
            item.RecreateItem(this, idx, curlvl, 0, 1);
            CheckInvSwap(bodyLocation);

            // ReadySpellFromEquipment(bodyLocation);
        }

        void CheckInvSwap(inv_body_loc bLoc)
        {
            var item = InvBody[(int)bLoc];

            if (bLoc == inv_body_loc.INVLOC_HAND_LEFT && GetItemLocation(item) == item_equip_type.ILOC_TWOHAND) {
                InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT].clear();
            } else if (bLoc == inv_body_loc.INVLOC_HAND_RIGHT && GetItemLocation(item) == item_equip_type.ILOC_TWOHAND) {
                InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT].clear();
            }

            CalcPlrInv(true);
        }

        /**
        * @brief Return block chance
        * @param useLevel - indicate if player's level should be added to block chance (the only case where it isn't is blocking a trap)
        */
        public int GetBlockChance(bool useLevel = true)
        {
            int blkper = _pDexterity + _pBaseToBlk;
            if (useLevel)
                blkper += _pLevel * 2;
            return blkper;
        }

        public void StartPlrBlock(Direction dir)
        {
            dfTest.Inst.ShowUnitText(this, "BLOCK");

            // if (_pInvincible && _pHitPoints == 0 && &player == MyPlayer) {
            //     SyncPlrKill(player, -1);
            //     return;
            // }

            // PlaySfxLoc(IS_ISWORD, position.tile);

            // int8_t skippedAnimationFrames = 0;
            // if (HasAnyOf(_pIFlags, ItemSpecialEffect::FastBlock)) {
            //     skippedAnimationFrames = (_pBFrames - 2); // ISPL_FASTBLOCK means we cancel the animation if frame 2 was shown
            // }

            // NewPlrAnim(player, player_graphic::Block, dir, AnimationDistributionFlags::SkipsDelayOfLastFrame, skippedAnimationFrames);

            // _pmode = PM_BLOCK;
            // FixPlayerLocation(player, dir);
            // SetPlayerOld(player);
        }

        public void StartPlrHit(int dam, bool forcehit)
        {
            dfTest.Inst.ShowDamageText(this, dam);

            if (_pInvincible && _pHitPoints == 0 /*&& &player == MyPlayer8*/) 
            {
                SyncPlrKill(-1);
                return;
            }

            // // Say(HeroSpeech::ArghClang);

            // RedrawComponent(PanelDrawComponent::Health);
            if (_pClass == HeroClass.Barbarian) {
                if (dam >> dfDEF.HPMANAOFFSET < _pLevel + _pLevel / 4 && !forcehit) {
                    return;
                }
            } else if (dam >> dfDEF.HPMANAOFFSET < _pLevel && !forcehit) {
                return;
            }

            // Direction pd = _pdir;

            // int8_t skippedAnimationFrames = 0;
            // constexpr ItemSpecialEffect ZenFlags = ItemSpecialEffect::FastHitRecovery | ItemSpecialEffect::FasterHitRecovery | ItemSpecialEffect::FastestHitRecovery;
            // if (HasAllOf(_pIFlags, ZenFlags)) { // if multiple hitrecovery modes are present the skipping of frames can go so far, that they skip frames that would skip. so the additional skipping thats skipped. that means we can't add the different modes together.
            //     skippedAnimationFrames = 4;
            // } else if (HasAnyOf(_pIFlags, ItemSpecialEffect::FastestHitRecovery)) {
            //     skippedAnimationFrames = 3;
            // } else if (HasAnyOf(_pIFlags, ItemSpecialEffect::FasterHitRecovery)) {
            //     skippedAnimationFrames = 2;
            // } else if (HasAnyOf(_pIFlags, ItemSpecialEffect::FastHitRecovery)) {
            //     skippedAnimationFrames = 1;
            // } else {
            //     skippedAnimationFrames = 0;
            // }

            NewPlrAnim(player_graphic.Hit/*, pd, AnimationDistributionFlags::None, skippedAnimationFrames*/);

            _pmode = PLR_MODE.PM_GOTHIT;
            // FixPlayerLocation(player, pd);
            // FixPlrWalkTags(player);
            // dPlayer[position.tile.x][position.tile.y] = getId() + 1;
            // SetPlayerOld(player);
        }

        void StartPlayerAttack()
        {
            m_Animator.SetTrigger("Attack");
            // weapon.StartAttack();
            _pmode = PLR_MODE.PM_ATTACK;
            NewPlrAnim(player_graphic.Attack);
        }

        void StartPlayerKill(int earflag)
        {
            if (_pHitPoints <= 0 && _pmode == PLR_MODE.PM_DEATH) {
                return;
            }

            // if (&player == MyPlayer) {
            //     NetSendCmdParam1(true, CMD_PLRDEAD, earflag);
            // }

            // bool diablolevel = gbIsMultiplayer && (isOnLevel(16) || isOnArenaLevel());

            // Say(HeroSpeech::AuughUh);

            // if (_pgfxnum != 0) {
            //     if (diablolevel || earflag != 0)
            //         _pgfxnum &= ~0xFU;
            //     else
            //         _pgfxnum = 0;
            //     ResetPlayerGFX(player);
            //     SetPlrAnims(player);
            // }

            // NewPlrAnim(player, player_graphic::Death, _pdir);

            // _pBlockFlag = false;
            // _pmode = PM_DEATH;
            // _pInvincible = true;
            // SetPlayerHitPoints(player, 0);

            // if (&player != MyPlayer && earflag == 0 && !diablolevel) {
            //     for (auto &item : InvBody) {
            //         item.clear();
            //     }
            //     CalcPlrInv(player, false);
            // }

            // if (isOnActiveLevel()) {
            //     FixPlayerLocation(player, _pdir);
            //     FixPlrWalkTags(player);
            //     dFlags[position.tile.x][position.tile.y] |= DungeonFlag::DeadPlayer;
            //     SetPlayerOld(player);

            //     if (&player == MyPlayer) {
            //         RedrawComponent(PanelDrawComponent::Health);

            //         if (!HoldItem.isEmpty()) {
            //             DeadItem(player, std::move(HoldItem), { 0, 0 });
            //             NewCursor(CURSOR_HAND);
            //         }

            //         if (!diablolevel) {
            //             DropHalfPlayersGold(player);
            //             if (earflag != -1) {
            //                 if (earflag != 0) {
            //                     Item ear;
            //                     InitializeItem(ear, IDI_EAR);
            //                     CopyUtf8(ear._iName, fmt::format(fmt::runtime(_("Ear of {:s}")), _pName), sizeof(ear._iName));
            //                     CopyUtf8(ear._iIName, _pName, sizeof(ear._iIName));
            //                     switch (_pClass) {
            //                     case HeroClass::Sorcerer:
            //                         ear._iCurs = ICURS_EAR_SORCERER;
            //                         break;
            //                     case HeroClass::Warrior:
            //                         ear._iCurs = ICURS_EAR_WARRIOR;
            //                         break;
            //                     case HeroClass::Rogue:
            //                     case HeroClass::Monk:
            //                     case HeroClass::Bard:
            //                     case HeroClass::Barbarian:
            //                         ear._iCurs = ICURS_EAR_ROGUE;
            //                         break;
            //                     }

            //                     ear._iCreateInfo = _pName[0] << 8 | _pName[1];
            //                     ear._iSeed = _pName[2] << 24 | _pName[3] << 16 | _pName[4] << 8 | _pName[5];
            //                     ear._ivalue = _pLevel;

            //                     if (FindGetItem(ear._iSeed, IDI_EAR, ear._iCreateInfo) == -1) {
            //                         DeadItem(player, std::move(ear), { 0, 0 });
            //                     }
            //                 } else {
            //                     Direction pdd = _pdir;
            //                     for (auto &item : InvBody) {
            //                         pdd = Left(pdd);
            //                         DeadItem(player, item.pop(), Displacement(pdd));
            //                     }

            //                     CalcPlrInv(player, false);
            //                 }
            //             }
            //         }
            //     }
            // }
            // SetPlayerHitPoints(player, 0);
        }

        public bool AttackPlayer(dfPlayer target)
        {
            StartPlayerAttack();
            return PlrHitPlr(target);
        }

        public bool PlrHitPlr(dfPlayer target)
        {
            var attacker = this;

            if (target._pInvincible) 
            {
                Debug.LogWarning("failed PlrHitPlr because target is invincible");
                return false;
            }

            if (dfUtils.HasAnyOf(target._pSpellFlags, SpellFlag.Etherealize)) 
            {
                Debug.LogWarning("failed PlrHitPlr becase target is ethereal");
                return false;
            }

            int hit = dfUtils.GenerateRnd(100);

            int hper = attacker.GetMeleeToHit() - target.GetArmor();
            // NOTE：限定到 5-95，是为了保障在差别很大的情况下，有小概率能够击中或闪避
            // 如果 hper > 100, 限定到 100 那么可能永远击不中
            // 如果 hper < 0, 限定到 0，那么必然击中
            hper = Mathf.Clamp(hper, 5, 95);

            if (hit >= hper) 
            {
                dfTest.Inst.ShowMiss(target);
                return false;
            }

            int blk = 100;
            if ((target._pmode == PLR_MODE.PM_STAND || target._pmode == PLR_MODE.PM_ATTACK) && target._pBlockFlag) 
            {
                blk = dfUtils.GenerateRnd(100);
            }

            int blkper = target.GetBlockChance() - (attacker._pLevel * 2);
            blkper = Mathf.Clamp(blkper, 0, 100);

            if (blk < blkper)
            {
                Direction dir = dfUtils.GetDirection(target.position.tile, attacker.position.tile);
                target.StartPlrBlock(dir);
                return true;
            }

            int mind = attacker._pIMinDam;
            int maxd = attacker._pIMaxDam;
            int dam = dfUtils.GenerateRnd(maxd - mind + 1) + mind;
            dam += (dam * attacker._pIBonusDam) / 100;
            dam += attacker._pIBonusDamMod + attacker._pDamageMod;

            if (attacker._pClass == HeroClass.Warrior || attacker._pClass == HeroClass.Barbarian) {
                if (dfUtils.GenerateRnd(100) < attacker._pLevel) {
                    dam *= 2;
                }
            }
            int skdam = dam << dfDEF.HPMANAOFFSET;
            if (dfUtils.HasAnyOf(attacker._pIFlags, ItemSpecialEffect.RandomStealLife)) {
                int tac = dfUtils.GenerateRnd(skdam / 8);
                attacker._pHitPoints += tac;
                if (attacker._pHitPoints > attacker._pMaxHP) {
                    attacker._pHitPoints = attacker._pMaxHP;
                }
                attacker._pHPBase += tac;
                if (attacker._pHPBase > attacker._pMaxHPBase) {
                    attacker._pHPBase = attacker._pMaxHPBase;
                }
                // RedrawComponent(PanelDrawComponent::Health);
            }
            // if (&attacker == MyPlayer) 
            // {
            //     NetSendCmdDamage(true, target.getId(), skdam);
            // }
            target.StartPlrHit(skdam, false);

            return true;
        }

        bool DoAttack()
        {
            // if (AnimInfo.currentFrame == _pAFNum - 2) 
            // {
            //     PlaySfxLoc(PS_SWING, position.tile);
            // }

            // bool didhit = false;

            // if (AnimInfo.currentFrame == _pAFNum - 1) 
            // {
            //     Point position = position.tile + _pdir;
            //     Monster *monster = FindMonsterAtPosition(position);

            //     if (monster != nullptr) {
            //         if (CanTalkToMonst(*monster)) {
            //             position.temp.x = 0; /** @todo Looks to be irrelevant, probably just remove it */
            //             return false;
            //         }
            //     }

            //     if (!gbIsHellfire || !HasAllOf(_pIFlags, ItemSpecialEffect::FireDamage | ItemSpecialEffect::LightningDamage)) {
            //         const size_t playerId = getId();
            //         if (HasAnyOf(_pIFlags, ItemSpecialEffect::FireDamage)) {
            //             AddMissile(position, { 1, 0 }, Direction::South, MIS_WEAPEXP, TARGET_MONSTERS, playerId, 0, 0);
            //         }
            //         if (HasAnyOf(_pIFlags, ItemSpecialEffect::LightningDamage)) {
            //             AddMissile(position, { 2, 0 }, Direction::South, MIS_WEAPEXP, TARGET_MONSTERS, playerId, 0, 0);
            //         }
            //     }

            //     if (monster != nullptr) {
            //         didhit = PlrHitMonst(player, *monster);
            //     } else if (PlayerAtPosition(position) != nullptr && !friendlyMode) {
            //         didhit = PlrHitPlr(player, *PlayerAtPosition(position));
            //     } else {
            //         Object *object = FindObjectAtPosition(position, false);
            //         if (object != nullptr) {
            //             didhit = PlrHitObj(player, *object);
            //         }
            //     }
            //     if ((_pClass == HeroClass::Monk
            //             && (InvBody[INVLOC_HAND_LEFT]._itype == ItemType::Staff || InvBody[INVLOC_HAND_RIGHT]._itype == ItemType::Staff))
            //         || (_pClass == HeroClass::Bard
            //             && InvBody[INVLOC_HAND_LEFT]._itype == ItemType::Sword && InvBody[INVLOC_HAND_RIGHT]._itype == ItemType::Sword)
            //         || (_pClass == HeroClass::Barbarian
            //             && (InvBody[INVLOC_HAND_LEFT]._itype == ItemType::Axe || InvBody[INVLOC_HAND_RIGHT]._itype == ItemType::Axe
            //                 || (((InvBody[INVLOC_HAND_LEFT]._itype == ItemType::Mace && InvBody[INVLOC_HAND_LEFT]._iLoc == ILOC_TWOHAND)
            //                         || (InvBody[INVLOC_HAND_RIGHT]._itype == ItemType::Mace && InvBody[INVLOC_HAND_RIGHT]._iLoc == ILOC_TWOHAND)
            //                         || (InvBody[INVLOC_HAND_LEFT]._itype == ItemType::Sword && InvBody[INVLOC_HAND_LEFT]._iLoc == ILOC_TWOHAND)
            //                         || (InvBody[INVLOC_HAND_RIGHT]._itype == ItemType::Sword && InvBody[INVLOC_HAND_RIGHT]._iLoc == ILOC_TWOHAND))
            //                     && !(InvBody[INVLOC_HAND_LEFT]._itype == ItemType::Shield || InvBody[INVLOC_HAND_RIGHT]._itype == ItemType::Shield))))) {
            //         // playing as a class/weapon with cleave
            //         position = position.tile + Right(_pdir);
            //         monster = FindMonsterAtPosition(position);
            //         if (monster != nullptr) {
            //             if (!CanTalkToMonst(*monster) && monster->position.old == position) {
            //                 if (PlrHitMonst(player, *monster, true))
            //                     didhit = true;
            //             }
            //         }
            //         position = position.tile + Left(_pdir);
            //         monster = FindMonsterAtPosition(position);
            //         if (monster != nullptr) {
            //             if (!CanTalkToMonst(*monster) && monster->position.old == position) {
            //                 if (PlrHitMonst(player, *monster, true))
            //                     didhit = true;
            //             }
            //         }
            //     }

            //     if (didhit && DamageWeapon(player, 30)) {
            //         StartStand(player, _pdir);
            //         ClearStateVariables(player);
            //         return true;
            //     }
            // }

            if (AnimInfoRemainFrame() == 1)
            {
                if (DamageWeapon(30)) 
                {
                    StartStand(_pdir);
                    // ClearStateVariables(player);
                    return true;
                }
            }

            if (AnimInfoIsLastFrame()) 
            {
                StartStand(_pdir);
                // ClearStateVariables(player);
                return true;
            }

            return false;
        }

        void NewPlrAnim(player_graphic graphic)
        {
            animationFrameTick = 0;

            animationFrame = 30;
            if (graphic == player_graphic.Hit)
                animationFrame = 15;
            else if (graphic == player_graphic.Attack)
                animationFrame = 20;
        }

        int AnimInfoRemainFrame()
        {
            return Mathf.Max(0, animationFrame - animationFrameTick);
        }

        int AnimInfoCurrentFrame()
        {
            return animationFrameTick;
        }

        bool AnimInfoIsLastFrame()
        {
            return animationFrameTick >= animationFrame;
        }

        public Direction _pdir;
        bool DoGotHit()
        {
            // Debug.Log("xx-- do got hit > " + animationFrame);
            if (AnimInfoIsLastFrame()) 
            {
                Debug.LogError("xx-- do got hit > " + animationFrame);
                StartStand(_pdir);
                // ClearStateVariables(player);
                if (!dfUtils.FlipCoin(4)) 
                {
                    DamageArmor();
                }
                return true;
            }
            return false;
        }

        void StartStand(Direction dir)
        {
            // if (_pInvincible && _pHitPoints == 0 && &player == MyPlayer) {
            //     SyncPlrKill(player, -1);
            //     return;
            // }

            // NewPlrAnim(player, player_graphic::Stand, dir);
            _pmode = PLR_MODE.PM_STAND;
            // FixPlayerLocation(player, dir);
            // FixPlrWalkTags(player);
            // dPlayer[position.tile.x][position.tile.y] = getId() + 1;
            // SetPlayerOld(player);
        }

        void DamageArmor()
        {
            // if (&player != MyPlayer) {
            //     return;
            // }

            if (InvBody[(int)inv_body_loc.INVLOC_CHEST].isEmpty() && InvBody[(int)inv_body_loc.INVLOC_HEAD].isEmpty()) {
                Debug.Log("DamageArmor > chest & head is empty");
                return;
            }

            bool targetHead = dfUtils.FlipCoin(3);
            if (!InvBody[(int)inv_body_loc.INVLOC_CHEST].isEmpty() && InvBody[(int)inv_body_loc.INVLOC_HEAD].isEmpty()) {
                targetHead = false;
            }
            if (InvBody[(int)inv_body_loc.INVLOC_CHEST].isEmpty() && !InvBody[(int)inv_body_loc.INVLOC_HEAD].isEmpty()) {
                targetHead = true;
            }

            dfItem pi;
            if (targetHead) {
                pi = InvBody[(int)inv_body_loc.INVLOC_HEAD];
            } else {
                pi = InvBody[(int)inv_body_loc.INVLOC_CHEST];
            }
            if (pi._iDurability == dfItem.DUR_INDESTRUCTIBLE) {
                return;
            }

            pi._iDurability--;
            Debug.Log($"DamageArmor > {(targetHead ? "head" : "chest")} dur - {pi._iDurability}");
            if (pi._iDurability > 0) {
                return;
            }

            if (targetHead) {
                RemoveEquipment(inv_body_loc.INVLOC_HEAD, true);
            } else {
                RemoveEquipment(inv_body_loc.INVLOC_CHEST, true);
            }
            CalcPlrInv(true);
        }

        void RemoveEquipment(inv_body_loc bodyLocation, bool hiPri)
        {
            // if (&player == MyPlayer) {
            //     NetSendCmdDelItem(hiPri, bodyLocation);
            // }
            Debug.Log("RemoveEquipment > " + bodyLocation);
            InvBody[(int)bodyLocation].clear();
        }

        // 每次攻击物品损坏，与耐久无关
        bool WeaponDecay(int ii)
        {
            if (!InvBody[ii].isEmpty() && InvBody[ii]._iClass == item_class.ICLASS_WEAPON && dfUtils.HasAnyOf(InvBody[ii]._iDamAcFlags, ItemSpecialEffectHf.Decay)) {
                InvBody[ii]._iPLDam -= 5;
                if (InvBody[ii]._iPLDam <= -100) {
                    RemoveEquipment((inv_body_loc)(ii), true);
                    CalcPlrInv(true);
                    return true;
                }
                CalcPlrInv(true);
            }
            return false;
        }

        bool DamageWeapon(int damageFrequency)
        {
            // Debug.Log("xx-- DamageWeapon");
            // if (&player != MyPlayer) {
            //     return false;
            // }

            if (WeaponDecay((int)inv_body_loc.INVLOC_HAND_LEFT))
                return true;
            if (WeaponDecay((int)inv_body_loc.INVLOC_HAND_RIGHT))
                return true;

            if (!dfUtils.FlipCoin(damageFrequency)) {
                return false;
            }

            if (!InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT].isEmpty() && InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._iClass == item_class.ICLASS_WEAPON) {
                if (InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._iDurability == dfItem.DUR_INDESTRUCTIBLE) {
                    return false;
                }

                InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._iDurability--;
                if (InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._iDurability <= 0) {
                    RemoveEquipment(inv_body_loc.INVLOC_HAND_LEFT, true);
                    CalcPlrInv(true);
                    return true;
                }
            }

            if (!InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT].isEmpty() && InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._iClass == item_class.ICLASS_WEAPON) {
                if (InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._iDurability == dfItem.DUR_INDESTRUCTIBLE) {
                    return false;
                }

                InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._iDurability--;
                if (InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._iDurability == 0) {
                    RemoveEquipment(inv_body_loc.INVLOC_HAND_RIGHT, true);
                    CalcPlrInv(true);
                    return true;
                }
            }

            if (InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT].isEmpty() && InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._itype == ItemType.Shield) {
                if (InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._iDurability == dfItem.DUR_INDESTRUCTIBLE) {
                    return false;
                }

                InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._iDurability--;
                if (InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT]._iDurability == 0) {
                    RemoveEquipment(inv_body_loc.INVLOC_HAND_RIGHT, true);
                    CalcPlrInv(true);
                    return true;
                }
            }

            if (InvBody[(int)inv_body_loc.INVLOC_HAND_RIGHT].isEmpty() && InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._itype == ItemType.Shield) {
                if (InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._iDurability == dfItem.DUR_INDESTRUCTIBLE) {
                    return false;
                }

                InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._iDurability--;
                if (InvBody[(int)inv_body_loc.INVLOC_HAND_LEFT]._iDurability == 0) {
                    RemoveEquipment(inv_body_loc.INVLOC_HAND_LEFT, true);
                    CalcPlrInv(true);
                    return true;
                }
            }

            return false;
        }

        // ---------------------------------------------------------------------
#region EditorTest
        [Button("LevelUp", EButtonEnableMode.Playmode)]
        private void TestLevelUp()
        {
            NextPlrLevel();
        }

        [Button("RefreshEquip", EButtonEnableMode.Playmode)]
        private void TestRefreshEquip()
        {
            RemoveEquipment(inv_body_loc.INVLOC_HEAD, true);
            RemoveEquipment(inv_body_loc.INVLOC_RING_LEFT, true);
            RemoveEquipment(inv_body_loc.INVLOC_RING_RIGHT, true);
            RemoveEquipment(inv_body_loc.INVLOC_AMULET, true);
            RemoveEquipment(inv_body_loc.INVLOC_HAND_LEFT, true);
            RemoveEquipment(inv_body_loc.INVLOC_HAND_RIGHT, true);
            RemoveEquipment(inv_body_loc.INVLOC_CHEST, true);

            if (testHeadItemId != (int)_item_indexes.IDI_NONE)
                ChangePlayerItems(inv_body_loc.INVLOC_HEAD, (_item_indexes)testHeadItemId);
            if (testLeftRingItemId != (int)_item_indexes.IDI_NONE)
                ChangePlayerItems(inv_body_loc.INVLOC_RING_LEFT, (_item_indexes)testLeftRingItemId);
            if (testRightRingItemId != (int)_item_indexes.IDI_NONE)
                ChangePlayerItems(inv_body_loc.INVLOC_RING_RIGHT, (_item_indexes)testRightRingItemId);
            if (testAmuletItemId != (int)_item_indexes.IDI_NONE)
                ChangePlayerItems(inv_body_loc.INVLOC_AMULET, (_item_indexes)testAmuletItemId);
            if (testLeftHandItemId != (int)_item_indexes.IDI_NONE)
                ChangePlayerItems(inv_body_loc.INVLOC_HAND_LEFT, (_item_indexes)testLeftHandItemId);
            if (testRightHandItemId != (int)_item_indexes.IDI_NONE)
            {
                var dat = dfData.AllItemsList[testRightHandItemId];
                if (dat.iLoc == item_equip_type.ILOC_TWOHAND)
                {
                    Debug.LogWarning("right hand is two-hand weapon, remove left hand equip");
                    testLeftHandItemId = (int)_item_indexes.IDI_NONE;
                    RemoveEquipment(inv_body_loc.INVLOC_HAND_LEFT, true);
                }
                ChangePlayerItems(inv_body_loc.INVLOC_HAND_RIGHT, (_item_indexes)testRightHandItemId);
            }
            if (testChestItemId != (int)_item_indexes.IDI_NONE)
                ChangePlayerItems(inv_body_loc.INVLOC_CHEST, (_item_indexes)testChestItemId);
        }

        [Dropdown("TestGetHeadItemIds")]
        public int testHeadItemId;
        public static DropdownList<int> testHeadItemIds = null;
        private DropdownList<int> TestGetHeadItemIds()
        {
            if (testHeadItemIds == null)
                testHeadItemIds = TestGetItemIds(item_equip_type.ILOC_HELM);
            return testHeadItemIds;
        }

        [Dropdown("TestGetRingItemIds")]
        public int testLeftRingItemId;
        [Dropdown("TestGetRingItemIds")]
        public int testRightRingItemId;
        public static DropdownList<int> testRingItemIds = null;
        private DropdownList<int> TestGetRingItemIds()
        {
            if (testRingItemIds == null)
                testRingItemIds = TestGetItemIds(item_equip_type.ILOC_RING);
            return testRingItemIds;
        }

        [Dropdown("TestGetHandItemIds")]
        public int testLeftHandItemId;
        [Dropdown("TestGetHandItemIds")]
        public int testRightHandItemId;
        public static DropdownList<int> testHandItemIds = null;
        private DropdownList<int> TestGetHandItemIds()
        {
            if (testHandItemIds == null)
                testHandItemIds = TestGetItemIds(item_equip_type.ILOC_ONEHAND, item_equip_type.ILOC_TWOHAND);
            return testHandItemIds;
        }

        [Dropdown("TestGetAmuletItemIds")]
        public int testAmuletItemId;
        public static DropdownList<int> testAmuletItemIds = null;
        private DropdownList<int> TestGetAmuletItemIds()
        {
            if (testAmuletItemIds == null)
                testAmuletItemIds = TestGetItemIds(item_equip_type.ILOC_AMULET);
            return testAmuletItemIds;
        }

        [Dropdown("TestGetChestItemIds")]
        public int testChestItemId;
        public static DropdownList<int> testChestItemIds = null;
        private DropdownList<int> TestGetChestItemIds()
        {
            if (testChestItemIds == null)
                testChestItemIds = TestGetItemIds(item_equip_type.ILOC_ARMOR);
            return testChestItemIds;
        }

        private DropdownList<int> TestGetItemIds(params item_equip_type[] locs)
        {
            var rt = new DropdownList<int>();
            rt.Add("None", (int)_item_indexes.IDI_NONE);

            for (int i = 0; i < dfData.AllItemsList.Length; ++i)
            {
                var d = dfData.AllItemsList[i];
                foreach (var loc in locs)
                {
                    if (d.iLoc == loc)
                    {
                        rt.Add(d.iName, i);
                        break;
                    }
                }
            }
            return rt;
        }

#endregion // EditorTest
    }
}

