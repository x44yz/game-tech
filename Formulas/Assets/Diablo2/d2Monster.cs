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

        protected override void OnStart()
        {
            base.OnStart();

            InitMonster(this, ((int)type));
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

        void InitMonster(d2Monster monster, int typeIndex)
        {
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
    }
}
