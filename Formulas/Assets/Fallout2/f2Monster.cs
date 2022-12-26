using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public enum MonsterGoal
    {
        None,
        Normal,
        Retreat,
        Healing,
        Move,
        Attack,
        Inquiring,
        Talking,
    };

    /**
    * @brief Defines the relation of the monster to a monster pack.
    *        If value is different from Individual Monster, the leader must also be set
    */
    public enum LeaderRelation 
    {
        None,
        /**
        * @brief Minion that sticks to the leader
        */
        Leashed,
        /**
        * @brief Minion that was separated from the leader and acts individually until it reaches the leader again
        */
        Separated,
    };


    public class f2Monster : Unit
    {
        [Header("MONSTER")]
        public _monster_id type;

        [Header("RUNTIME")]
        public MonsterMode mode;
        public MonsterData data;
        public int armorClass;
        public int maxHitPoints;
        public int hitPoints;
        public int levelType;
        public int toHit;
        public _mai_id ai;
        /** Specifies current goal of the monster */
        public MonsterGoal goal;
        /** @brief Specifies monster's behaviour regarding moving and changing goals. */
        public int goalVar1;
        /**
        * @brief Specifies turning direction for @p RoundWalk in most cases.
        * Used in custom way by @p FallenAi, @p SnakeAi, @p M_FallenFear and @p FallenAi.
        */
        public int goalVar2;
        /**
        * @brief Controls monster's behaviour regarding special actions.
        * Used only by @p ScavengerAi and @p MegaAi.
        */
        public int goalVar3;
        public ActorPosition position = new ActorPosition();

        protected override void OnStart()
        {
            base.OnStart();

            InitMonster(((int)type));
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            // test
        }

        protected override void OnHit(Unit attacker)
        {
            base.OnHit(attacker);
            Debug.Log("xx-- d2Monster.OnHit");
        }

        public bool isPossibleToHit
        {
            // TODO
            get { return true; }
        }

        public bool tryLiftGargoyle()
        {
            // TODO
            return false;
        }

        public const int NoLeader = -1;
        public const int NightmareToHitBonus = 85;
        public const int HellToHitBonus = 120;
        public const int NightmareAcBonus = 50;
        public const int HellAcBonus = 80;

        public int rndItemSeed;
        public int aiSeed;
        public int whoHit;
        public int minDamage;
        public int maxDamage;
        public int minDamageSpecial;
        public int maxDamageSpecial;
        public int resistance;
        public int leader;
        public LeaderRelation leaderRelation;
        public int flags;
        public int intelligence;
        /** Direction faced by monster (direction enum) */
        public Direction direction;

        void InitMonster(int typeIndex)
        {
            data = dfData.MonstersData[typeIndex];

            // direction = rd;
            // position.tile = position;
            // position.future = position;
            // position.old = position;
            levelType = typeIndex;
            mode = MonsterMode.Stand;
            // animInfo = {};
            // changeAnimationData(MonsterGraphic::Stand);
            // animInfo.tickCounterOfCurrentFrame = GenerateRnd(animInfo.ticksPerFrame - 1);
            // animInfo.currentFrame = GenerateRnd(animInfo.numberOfFrames - 1);

            int maxhp = data.hitPointsMinimum + f2Utils.GenerateRnd(data.hitPointsMaximum - data.hitPointsMinimum + 1);
            if (type == _monster_id.MT_DIABLO && !dfDEF.gbIsHellfire) {
                maxhp /= 2;
            }
            maxHitPoints = maxhp << dfDEF.HPMANAOFFSET;

            if (!dfDEF.gbIsMultiplayer)
                maxHitPoints = Math.Max(maxHitPoints / 2, 64);

            hitPoints = maxHitPoints;
            ai = data.ai;
            intelligence = data.intelligence;
            goal = MonsterGoal.Normal;
            goalVar1 = 0;
            goalVar2 = 0;
            goalVar3 = 0;
            // pathCount = 0;
            // isInvalid = false;
            uniqueType = UniqueMonsterType.None;
            // activeForTicks = 0;
            // lightId = NO_LIGHT; // BUGFIX monsters initial light id should be -1 (fixed)
            rndItemSeed = f2Utils.AdvanceRndSeed();
            aiSeed = f2Utils.AdvanceRndSeed();
            whoHit = 0;
            toHit = data.toHit;
            minDamage = data.minDamage;
            maxDamage = data.maxDamage;
            minDamageSpecial = data.minDamageSpecial;
            maxDamageSpecial = data.maxDamageSpecial;
            armorClass = data.armorClass;
            resistance = data.resistance;
            leader = f2Monster.NoLeader;
            leaderRelation = LeaderRelation.None;
            flags = data.abilityFlags;
            // talkMsg = TEXT_NONE;

            if (ai == _mai_id.AI_GARG) 
            {
                // changeAnimationData(MonsterGraphic::Special);
                // animInfo.currentFrame = 0;
                flags |= monster_flag.MFLAG_ALLOW_SPECIAL;
                mode = MonsterMode.SpecialMeleeAttack;
            }

            if (dfDEF.gbDifficulty == _difficulty.DIFF_NIGHTMARE) 
            {
                maxHitPoints = 3 * maxHitPoints;
                if (dfDEF.gbIsHellfire)
                    maxHitPoints += (dfDEF.gbIsMultiplayer ? 100 : 50) << dfDEF.HPMANAOFFSET;
                else
                    maxHitPoints += 64;
                hitPoints = maxHitPoints;
                toHit += NightmareToHitBonus;
                minDamage = 2 * (minDamage + 2);
                maxDamage = 2 * (maxDamage + 2);
                minDamageSpecial = 2 * (minDamageSpecial + 2);
                maxDamageSpecial = 2 * (maxDamageSpecial + 2);
                armorClass += NightmareAcBonus;
            } 
            else if (dfDEF.gbDifficulty == _difficulty.DIFF_HELL) 
            {
                maxHitPoints = 4 * maxHitPoints;
                if (dfDEF.gbIsHellfire)
                    maxHitPoints += (dfDEF.gbIsMultiplayer ? 200 : 100) << dfDEF.HPMANAOFFSET;
                else
                    maxHitPoints += 192;
                hitPoints = maxHitPoints;
                toHit += HellToHitBonus;
                minDamage = 4 * minDamage + 6;
                maxDamage = 4 * maxDamage + 6;
                minDamageSpecial = 4 * minDamageSpecial + 6;
                maxDamageSpecial = 4 * maxDamageSpecial + 6;
                armorClass += HellAcBonus;
                resistance = data.resistanceHell;
            }
        }

        public UniqueMonsterType uniqueType;
        bool isUnique()
        {
            return uniqueType != UniqueMonsterType.None;
        }

        int level(_difficulty difficulty)
        {
            int baseLevel = data.level;
            if (isUnique()) 
            {
                baseLevel = dfData.UniqueMonstersData[(int)uniqueType].mlevel;
                if (baseLevel != 0) {
                    baseLevel *= 2;
                } else {
                    baseLevel = data.level + 5;
                }
            }

            if (type == _monster_id.MT_DIABLO && !dfDEF.gbIsHellfire) {
                baseLevel -= 15;
            }

            if (difficulty == _difficulty.DIFF_NIGHTMARE) {
                baseLevel += 15;
            } else if (difficulty == _difficulty.DIFF_HELL) {
                baseLevel += 30;
            }

            return baseLevel;
        }

        int GetMinHit()
        {
            switch (dfDEF.currlevel) {
            case 16:
                return 30;
            case 15:
                return 25;
            case 14:
                return 20;
            default:
                return 15;
            }
        }

        public void AttackPlayer(f2Player player)
        {
            MonsterAttackPlayer(player, toHit, minDamage, maxDamage);
        }

        void MonsterAttackPlayer(f2Player player, int hit, int minDam, int maxDam)
        {
            if (player._pHitPoints >> dfDEF.HPMANAOFFSET <= 0 || player._pInvincible || f2Utils.HasAnyOf(player._pSpellFlags, SpellFlag.Etherealize))
                return;
            // 判断距离
            // if (position.tile.WalkingDistance(player.position.tile) >= 2)
            //     return;

            int hper = f2Utils.GenerateRnd(100);
        #if _DEBUG
            if (DebugGodMode)
                hper = 1000;
        #endif
            int ac = player.GetArmor();
            // 针对
            if (f2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.ACAgainstDemons) && data.monsterClass == MonsterClass.Demon)
                ac += 40;
            if (f2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.ACAgainstUndead) && data.monsterClass == MonsterClass.Undead)
                ac += 20;
            // hit 越大 player 被击中的概率越高
            hit += 2 * (level(dfDEF.gbDifficulty) - player._pLevel)
                + 30
                - ac;
            int minhit = GetMinHit();
            hit = Math.Max(hit, minhit);
            if (hper >= hit)
            {
                f2Test.Inst.ShowDamageText(player, 0);
                return;
            }

            // blkper 越小越容易闪避
            int blkper = 100;
            if ((player._pmode == PLR_MODE.PM_STAND || player._pmode == PLR_MODE.PM_ATTACK) && player._pBlockFlag) {
                blkper = f2Utils.GenerateRnd(100);
            }
            // blk 越大越容易闪避
            int blk = player.GetBlockChance() - (level(dfDEF.gbDifficulty) * 2);
            blk = Mathf.Clamp(blk, 0, 100);
            if (blkper < blk) 
            {
                Direction dir = f2Utils.GetDirection(player.position.tile, position.tile);
                player.StartPlrBlock(dir);
                if (/*player == MyPlayer &&*/ player.wReflections > 0) 
                {
                    int kdam = f2Utils.GenerateRnd(((maxDam - minDam) << dfDEF.HPMANAOFFSET) + 1) + (minDam << dfDEF.HPMANAOFFSET);
                    kdam = Math.Max(kdam + (player._pIGetHit << dfDEF.HPMANAOFFSET), /*64*/1 << dfDEF.HPMANAOFFSET);
                    CheckReflect(player, ref kdam);
                }
                return;
            }
            // skip: 特殊处理
            if (type == _monster_id.MT_YZOMBIE /*&& &player == MyPlayer*/) 
            {
                if (player._pMaxHP > 64) {
                    if (player._pMaxHPBase > 64) {
                        player._pMaxHP -= 64;
                        if (player._pHitPoints > player._pMaxHP) {
                            player._pHitPoints = player._pMaxHP;
                        }
                        player._pMaxHPBase -= 64;
                        if (player._pHPBase > player._pMaxHPBase) {
                            player._pHPBase = player._pMaxHPBase;
                        }
                    }
                }
            }
            int dam = (minDam << dfDEF.HPMANAOFFSET) + f2Utils.GenerateRnd(((maxDam - minDam) << dfDEF.HPMANAOFFSET) + 1);
            // 最小 64 点伤害？
            // _pIGetHit 减伤
            dam = Math.Max(dam + (player._pIGetHit << dfDEF.HPMANAOFFSET), 1 << dfDEF.HPMANAOFFSET);
            // if (&player == MyPlayer) 
            {
                if (player.wReflections > 0)
                    CheckReflect(player, ref dam);
                player.ApplyPlrDamage(0, 0, dam);
            }

            // Reflect can also kill a monster, so make sure the monster is still alive
            if (f2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.Thorns) && mode != MonsterMode.Death) 
            {
                int mdam = (f2Utils.GenerateRnd(3) + 1) << dfDEF.HPMANAOFFSET;
                ApplyMonsterDamage(mdam);
                if (hitPoints >> dfDEF.HPMANAOFFSET <= 0)
                    M_StartKill(player, mdam);
                else
                    M_StartHit(player, mdam);
            }

            if ((flags & monster_flag.MFLAG_NOLIFESTEAL) == 0 && type == _monster_id.MT_SKING && dfDEF.gbIsMultiplayer)
                hitPoints += dam;
            if (player._pHitPoints >> dfDEF.HPMANAOFFSET <= 0) 
            {
                if (dfDEF.gbIsHellfire)
                    M_StartStand(direction);
                return;
            }

            player.StartPlrHit(dam, false);
            if ((flags & monster_flag.MFLAG_KNOCKBACK) != 0) {
                if (player._pmode != PLR_MODE.PM_GOTHIT)
                    player.StartPlrHit(0, true);

                // Point newPosition = player.position.tile + direction;
                // if (PosOkPlayer(player, newPosition)) {
                //     player.position.tile = newPosition;
                //     FixPlayerLocation(player, player._pdir);
                //     FixPlrWalkTags(player);
                //     dPlayer[newPosition.x][newPosition.y] = player.getId() + 1;
                //     SetPlayerOld(player);
                // }
            }
        }

        void CheckReflect(f2Player player, ref int dam)
        {
            player.wReflections--;
            // if (player.wReflections <= 0)
            //     NetSendCmdParam1(true, CMD_SETREFLECT, 0);
            // reflects 20-30% damage
            int mdam = dam * (f2Utils.GenerateRnd(10) + 20) / 100;
            ApplyMonsterDamage(mdam);
            dam = Math.Max(dam - mdam, 0);
            if (hitPoints >> dfDEF.HPMANAOFFSET <= 0)
                M_StartKill(player, mdam);
            else
                M_StartHit(player, mdam);
        }

        public void ApplyMonsterDamage(int damage)
        {
            hitPoints -= damage;

            // if (hitPoints >> d2DEF.HPMANAOFFSET <= 0) {
            //     delta_kill_monster(monster, position.tile, *MyPlayer);
            //     NetSendCmdLocParam1(false, CMD_MONSTDEATH, position.tile, getId());
            //     return;
            // }

            // delta_monster_hp(monster, *MyPlayer);
            // NetSendCmdMonDmg(false, getId(), damage);
        }

        void M_StartStand(Direction md)
        {
            // ClearMVars(monster);
            // if (type().type == MT_GOLEM)
            //     NewMonsterAnim(monster, MonsterGraphic::Walk, md);
            // else
            //     NewMonsterAnim(monster, MonsterGraphic::Stand, md);
            // var1 = static_cast<int>(mode);
            // var2 = 0;
            // mode = MonsterMode::Stand;
            // position.future = position.tile;
            // position.old = position.tile;
            // UpdateEnemy(monster);
        }

        public void M_StartKill(f2Player player, int dam)
        {
            // StartMonsterDeath(monster, player, true);
            f2Test.Inst.ShowDamageText(this, dam);
            m_Animator.Play("Hit");
        }

        public void M_StartHit(int dam)
        {
            f2Test.Inst.ShowDamageText(this, dam);
            m_Animator.Play("Hit");

            // PlayEffect(monster, MonsterSound::Hit);

            // if (IsHardHit(monster, dam)) {
            //     if (type().type == MT_BLINK) {
            //         Teleport(monster);
            //     } else if (IsAnyOf(type().type, MT_NSCAV, MT_BSCAV, MT_WSCAV, MT_YSCAV, MT_GRAVEDIG)) {
            //         goal = MonsterGoal::Normal;
            //         goalVar1 = 0;
            //         goalVar2 = 0;
            //     }
            //     if (mode != MonsterMode::Petrified) {
            //         StartMonsterGotHit(monster);
            //     }
            // }
        }

        public void M_StartHit(f2Player player, int dam)
        {
            // tag(player);
            // if (IsHardHit(monster, dam)) {
            //     enemy = player.getId();
            //     enemyPosition = player.position.future;
            //     flags &= ~MFLAG_TARGETS_MONSTER;
            //     direction = GetMonsterDirection(monster);
            // }

            M_StartHit(dam);
        }

    }
}
