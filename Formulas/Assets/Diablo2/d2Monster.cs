using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
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


    public class d2Monster : Unit
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

            InitMonster(this, ((int)type));
        }

        protected override void OnUpdate(float dt)
        {
            // test
            if (Input.GetKeyDown(KeyCode.J))
            {
                var plr = GameObject.FindObjectOfType<d2Player>();
                MonsterAttackPlayer(this, plr, toHit, minDamage, maxDamage);
            }
        }

        protected override void OnHit(Unit attacker)
        {
            base.OnHit(attacker);
            Debug.Log("xx-- d2Monster.OnHit");
        }

        public override void TakeDamage(int damage)
        {
            hitPoints -= damage;

            d2Test.Inst.ShowDamageText(this, damage);

            if (hitPoints >> 6 <= 0) 
            {
                // delta_kill_monster(monster, monster.position.tile, *MyPlayer);
                // NetSendCmdLocParam1(false, CMD_MONSTDEATH, monster.position.tile, monster.getId());
                return;
            }

            // delta_monster_hp(monster, *MyPlayer);
            // NetSendCmdMonDmg(false, monster.getId(), damage);
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

        void InitMonster(d2Monster monster, int typeIndex)
        {
            monster.data = d2Data.MonstersData[typeIndex];

            // monster.direction = rd;
            // monster.position.tile = position;
            // monster.position.future = position;
            // monster.position.old = position;
            monster.levelType = typeIndex;
            monster.mode = MonsterMode.Stand;
            // monster.animInfo = {};
            // monster.changeAnimationData(MonsterGraphic::Stand);
            // monster.animInfo.tickCounterOfCurrentFrame = GenerateRnd(monster.animInfo.ticksPerFrame - 1);
            // monster.animInfo.currentFrame = GenerateRnd(monster.animInfo.numberOfFrames - 1);

            int maxhp = monster.data.hitPointsMinimum + d2Utils.GenerateRnd(monster.data.hitPointsMaximum - monster.data.hitPointsMinimum + 1);
            if (monster.type == _monster_id.MT_DIABLO && !d2DEF.gbIsHellfire) {
                maxhp /= 2;
            }
            monster.maxHitPoints = maxhp << 6;

            if (!d2DEF.gbIsMultiplayer)
                monster.maxHitPoints = Math.Max(monster.maxHitPoints / 2, 64);

            monster.hitPoints = monster.maxHitPoints;
            monster.ai = monster.data.ai;
            monster.intelligence = monster.data.intelligence;
            monster.goal = MonsterGoal.Normal;
            monster.goalVar1 = 0;
            monster.goalVar2 = 0;
            monster.goalVar3 = 0;
            // monster.pathCount = 0;
            // monster.isInvalid = false;
            // monster.uniqueType = UniqueMonsterType::None;
            // monster.activeForTicks = 0;
            // monster.lightId = NO_LIGHT; // BUGFIX monsters initial light id should be -1 (fixed)
            monster.rndItemSeed = d2Utils.AdvanceRndSeed();
            monster.aiSeed = d2Utils.AdvanceRndSeed();
            monster.whoHit = 0;
            monster.toHit = monster.data.toHit;
            monster.minDamage = monster.data.minDamage;
            monster.maxDamage = monster.data.maxDamage;
            monster.minDamageSpecial = monster.data.minDamageSpecial;
            monster.maxDamageSpecial = monster.data.maxDamageSpecial;
            monster.armorClass = monster.data.armorClass;
            monster.resistance = monster.data.resistance;
            monster.leader = d2Monster.NoLeader;
            monster.leaderRelation = LeaderRelation.None;
            monster.flags = monster.data.abilityFlags;
            // monster.talkMsg = TEXT_NONE;

            if (monster.ai == _mai_id.AI_GARG) 
            {
                // monster.changeAnimationData(MonsterGraphic::Special);
                // monster.animInfo.currentFrame = 0;
                monster.flags |= monster_flag.MFLAG_ALLOW_SPECIAL;
                monster.mode = MonsterMode.SpecialMeleeAttack;
            }

            if (d2DEF.gbDifficulty == _difficulty.DIFF_NIGHTMARE) 
            {
                monster.maxHitPoints = 3 * monster.maxHitPoints;
                if (d2DEF.gbIsHellfire)
                    monster.maxHitPoints += (d2DEF.gbIsMultiplayer ? 100 : 50) << 6;
                else
                    monster.maxHitPoints += 64;
                monster.hitPoints = monster.maxHitPoints;
                monster.toHit += NightmareToHitBonus;
                monster.minDamage = 2 * (monster.minDamage + 2);
                monster.maxDamage = 2 * (monster.maxDamage + 2);
                monster.minDamageSpecial = 2 * (monster.minDamageSpecial + 2);
                monster.maxDamageSpecial = 2 * (monster.maxDamageSpecial + 2);
                monster.armorClass += NightmareAcBonus;
            } 
            else if (d2DEF.gbDifficulty == _difficulty.DIFF_HELL) 
            {
                monster.maxHitPoints = 4 * monster.maxHitPoints;
                if (d2DEF.gbIsHellfire)
                    monster.maxHitPoints += (d2DEF.gbIsMultiplayer ? 200 : 100) << 6;
                else
                    monster.maxHitPoints += 192;
                monster.hitPoints = monster.maxHitPoints;
                monster.toHit += HellToHitBonus;
                monster.minDamage = 4 * monster.minDamage + 6;
                monster.maxDamage = 4 * monster.maxDamage + 6;
                monster.minDamageSpecial = 4 * monster.minDamageSpecial + 6;
                monster.maxDamageSpecial = 4 * monster.maxDamageSpecial + 6;
                monster.armorClass += HellAcBonus;
                monster.resistance = monster.data.resistanceHell;
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
                baseLevel = d2Data.UniqueMonstersData[(int)uniqueType].mlevel;
                if (baseLevel != 0) {
                    baseLevel *= 2;
                } else {
                    baseLevel = data.level + 5;
                }
            }

            if (type == _monster_id.MT_DIABLO && !d2DEF.gbIsHellfire) {
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
            switch (d2DEF.currlevel) {
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

        void MonsterAttackPlayer(d2Monster monster, d2Player player, int hit, int minDam, int maxDam)
        {
            if (player._pHitPoints >> 6 <= 0 || player._pInvincible || d2Utils.HasAnyOf(player._pSpellFlags, SpellFlag.Etherealize))
                return;
            // 判断距离
            // if (monster.position.tile.WalkingDistance(player.position.tile) >= 2)
            //     return;

            int hper = d2Utils.GenerateRnd(100);
        #if _DEBUG
            if (DebugGodMode)
                hper = 1000;
        #endif
            int ac = player.GetArmor();
            // 针对
            if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.ACAgainstDemons) && monster.data.monsterClass == MonsterClass.Demon)
                ac += 40;
            if (d2Utils.HasAnyOf(player.pDamAcFlags, ItemSpecialEffectHf.ACAgainstUndead) && monster.data.monsterClass == MonsterClass.Undead)
                ac += 20;
            // hit 越大 player 被击中的概率越高
            hit += 2 * (monster.level(d2DEF.gbDifficulty) - player._pLevel)
                + 30
                - ac;
            int minhit = GetMinHit();
            hit = Math.Max(hit, minhit);
            if (hper >= hit)
                return;

            // blkper 越小越容易闪避
            int blkper = 100;
            if ((player._pmode == PLR_MODE.PM_STAND || player._pmode == PLR_MODE.PM_ATTACK) && player._pBlockFlag) {
                blkper = d2Utils.GenerateRnd(100);
            }
            // blk 越大越容易闪避
            int blk = player.GetBlockChance() - (monster.level(d2DEF.gbDifficulty) * 2);
            blk = Mathf.Clamp(blk, 0, 100);
            if (blkper < blk) 
            {
                Direction dir = d2Utils.GetDirection(player.position.tile, monster.position.tile);
                player.StartPlrBlock(player, dir);
                if (/*player == MyPlayer &&*/ player.wReflections > 0) 
                {
                    int kdam = d2Utils.GenerateRnd(((maxDam - minDam) << 6) + 1) + (minDam << 6);
                    kdam = Math.Max(kdam + (player._pIGetHit << 6), 64);
                    CheckReflect(monster, player, ref kdam);
                }
                return;
            }
            // skip: 特殊处理
            if (monster.type == _monster_id.MT_YZOMBIE /*&& &player == MyPlayer*/) 
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
            int dam = (minDam << 6) + d2Utils.GenerateRnd(((maxDam - minDam) << 6) + 1);
            // 最小 64 点伤害？
            // _pIGetHit 减伤
            dam = Math.Max(dam + (player._pIGetHit << 6), 64);
            // if (&player == MyPlayer) 
            {
                if (player.wReflections > 0)
                    CheckReflect(monster, player, ref dam);
                player.ApplyPlrDamage(player, 0, 0, dam);
            }

            // Reflect can also kill a monster, so make sure the monster is still alive
            if (d2Utils.HasAnyOf(player._pIFlags, ItemSpecialEffect.Thorns) && monster.mode != MonsterMode.Death) 
            {
                int mdam = (d2Utils.GenerateRnd(3) + 1) << 6;
                ApplyMonsterDamage(monster, mdam);
                if (monster.hitPoints >> 6 <= 0)
                    M_StartKill(monster, player);
                else
                    M_StartHit(monster, player, mdam);
            }

            if ((monster.flags & monster_flag.MFLAG_NOLIFESTEAL) == 0 && monster.type == _monster_id.MT_SKING && d2DEF.gbIsMultiplayer)
                monster.hitPoints += dam;
            if (player._pHitPoints >> 6 <= 0) 
            {
                if (d2DEF.gbIsHellfire)
                    M_StartStand(monster, monster.direction);
                return;
            }

            player.StartPlrHit(player, dam, false);
            if ((monster.flags & monster_flag.MFLAG_KNOCKBACK) != 0) {
                if (player._pmode != PLR_MODE.PM_GOTHIT)
                    player.StartPlrHit(player, 0, true);

                // Point newPosition = player.position.tile + monster.direction;
                // if (PosOkPlayer(player, newPosition)) {
                //     player.position.tile = newPosition;
                //     FixPlayerLocation(player, player._pdir);
                //     FixPlrWalkTags(player);
                //     dPlayer[newPosition.x][newPosition.y] = player.getId() + 1;
                //     SetPlayerOld(player);
                // }
            }
        }

        void CheckReflect(d2Monster monster, d2Player player, ref int dam)
        {
            player.wReflections--;
            // if (player.wReflections <= 0)
            //     NetSendCmdParam1(true, CMD_SETREFLECT, 0);
            // reflects 20-30% damage
            int mdam = dam * (d2Utils.GenerateRnd(10) + 20) / 100;
            ApplyMonsterDamage(monster, mdam);
            dam = Math.Max(dam - mdam, 0);
            if (monster.hitPoints >> 6 <= 0)
                M_StartKill(monster, player);
            else
                M_StartHit(monster, player, mdam);
        }

        void ApplyMonsterDamage(d2Monster monster, int damage)
        {
            monster.hitPoints -= damage;

            // if (monster.hitPoints >> 6 <= 0) {
            //     delta_kill_monster(monster, monster.position.tile, *MyPlayer);
            //     NetSendCmdLocParam1(false, CMD_MONSTDEATH, monster.position.tile, monster.getId());
            //     return;
            // }

            // delta_monster_hp(monster, *MyPlayer);
            // NetSendCmdMonDmg(false, monster.getId(), damage);
        }

        void M_StartStand(d2Monster monster, Direction md)
        {
            // ClearMVars(monster);
            // if (monster.type().type == MT_GOLEM)
            //     NewMonsterAnim(monster, MonsterGraphic::Walk, md);
            // else
            //     NewMonsterAnim(monster, MonsterGraphic::Stand, md);
            // monster.var1 = static_cast<int>(monster.mode);
            // monster.var2 = 0;
            // monster.mode = MonsterMode::Stand;
            // monster.position.future = monster.position.tile;
            // monster.position.old = monster.position.tile;
            // UpdateEnemy(monster);
        }

        void M_StartKill(d2Monster monster, d2Player player)
        {
            // StartMonsterDeath(monster, player, true);
        }

        void M_StartHit(d2Monster monster, int dam)
        {
            // PlayEffect(monster, MonsterSound::Hit);

            // if (IsHardHit(monster, dam)) {
            //     if (monster.type().type == MT_BLINK) {
            //         Teleport(monster);
            //     } else if (IsAnyOf(monster.type().type, MT_NSCAV, MT_BSCAV, MT_WSCAV, MT_YSCAV, MT_GRAVEDIG)) {
            //         monster.goal = MonsterGoal::Normal;
            //         monster.goalVar1 = 0;
            //         monster.goalVar2 = 0;
            //     }
            //     if (monster.mode != MonsterMode::Petrified) {
            //         StartMonsterGotHit(monster);
            //     }
            // }
        }

        void M_StartHit(d2Monster monster, d2Player player, int dam)
        {
            // monster.tag(player);
            // if (IsHardHit(monster, dam)) {
            //     monster.enemy = player.getId();
            //     monster.enemyPosition = player.position.future;
            //     monster.flags &= ~MFLAG_TARGETS_MONSTER;
            //     monster.direction = GetMonsterDirection(monster);
            // }

            M_StartHit(monster, dam);
        }

    }
}
