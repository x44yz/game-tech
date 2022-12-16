using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public class d2Monster : Unit
    {
        [Header("MONSTER")]
        public _monster_id type;
        public MonsterMode mode;
        public MonsterData data;
        public int armorClass;
        public int maxHitPoints;
        public int hitPoints;
        public int levelType;

        protected override void OnStart()
        {
            base.OnStart();

            InitMonster(this, )
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
                monster.maxHitPoints = std::max(monster.maxHitPoints / 2, 64);

            monster.hitPoints = monster.maxHitPoints;
            monster.ai = monster.data().ai;
            monster.intelligence = monster.data().intelligence;
            monster.goal = MonsterGoal::Normal;
            monster.goalVar1 = 0;
            monster.goalVar2 = 0;
            monster.goalVar3 = 0;
            monster.pathCount = 0;
            monster.isInvalid = false;
            monster.uniqueType = UniqueMonsterType::None;
            monster.activeForTicks = 0;
            monster.lightId = NO_LIGHT; // BUGFIX monsters initial light id should be -1 (fixed)
            monster.rndItemSeed = AdvanceRndSeed();
            monster.aiSeed = AdvanceRndSeed();
            monster.whoHit = 0;
            monster.toHit = monster.data().toHit;
            monster.minDamage = monster.data().minDamage;
            monster.maxDamage = monster.data().maxDamage;
            monster.minDamageSpecial = monster.data().minDamageSpecial;
            monster.maxDamageSpecial = monster.data().maxDamageSpecial;
            monster.armorClass = monster.data().armorClass;
            monster.resistance = monster.data().resistance;
            monster.leader = Monster::NoLeader;
            monster.leaderRelation = LeaderRelation::None;
            monster.flags = monster.data().abilityFlags;
            monster.talkMsg = TEXT_NONE;

            if (monster.ai == AI_GARG) {
                monster.changeAnimationData(MonsterGraphic::Special);
                monster.animInfo.currentFrame = 0;
                monster.flags |= MFLAG_ALLOW_SPECIAL;
                monster.mode = MonsterMode::SpecialMeleeAttack;
            }

            if (sgGameInitInfo.nDifficulty == DIFF_NIGHTMARE) {
                monster.maxHitPoints = 3 * monster.maxHitPoints;
                if (gbIsHellfire)
                    monster.maxHitPoints += (gbIsMultiplayer ? 100 : 50) << 6;
                else
                    monster.maxHitPoints += 64;
                monster.hitPoints = monster.maxHitPoints;
                monster.toHit += NightmareToHitBonus;
                monster.minDamage = 2 * (monster.minDamage + 2);
                monster.maxDamage = 2 * (monster.maxDamage + 2);
                monster.minDamageSpecial = 2 * (monster.minDamageSpecial + 2);
                monster.maxDamageSpecial = 2 * (monster.maxDamageSpecial + 2);
                monster.armorClass += NightmareAcBonus;
            } else if (sgGameInitInfo.nDifficulty == DIFF_HELL) {
                monster.maxHitPoints = 4 * monster.maxHitPoints;
                if (gbIsHellfire)
                    monster.maxHitPoints += (gbIsMultiplayer ? 200 : 100) << 6;
                else
                    monster.maxHitPoints += 192;
                monster.hitPoints = monster.maxHitPoints;
                monster.toHit += HellToHitBonus;
                monster.minDamage = 4 * monster.minDamage + 6;
                monster.maxDamage = 4 * monster.maxDamage + 6;
                monster.minDamageSpecial = 4 * monster.minDamageSpecial + 6;
                monster.maxDamageSpecial = 4 * monster.maxDamageSpecial + 6;
                monster.armorClass += HellAcBonus;
                monster.resistance = monster.data().resistanceHell;
            }
        }
    }
}
