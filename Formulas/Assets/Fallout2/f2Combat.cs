using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        // Provides metadata about critical hit effect.
        public struct CriticalHitDescription {
            public int damageMultiplier;

            // Damage flags that will be applied to defender.
            public int flags;

            // Stat to check to upgrade this critical hit to massive critical hit or
            // -1 if there is no massive critical hit.
            public int massiveCriticalStat;

            // Bonus/penalty to massive critical stat.
            public int massiveCriticalStatModifier;

            // Additional damage flags if this critical hit become massive critical.
            public int massiveCriticalFlags;

            public int messageId;
            public int massiveCriticalMessageId;

            public CriticalHitDescription(           
                int damageMultiplier,
                int flags,
                int massiveCriticalStat,
                int massiveCriticalStatModifier,
                int massiveCriticalFlags,
                int messageId,
                int massiveCriticalMessageId
            )
            {
                this.damageMultiplier = damageMultiplier;
                this.flags = flags;
                this.massiveCriticalStat = massiveCriticalStat;
                this.massiveCriticalStatModifier = massiveCriticalStatModifier;
                this.massiveCriticalFlags = massiveCriticalFlags;
                this.messageId = messageId;
                this.massiveCriticalMessageId = massiveCriticalMessageId;
            }
        };

        public const int CRTICIAL_EFFECT_COUNT = 6;

        // Player's criticals effects.
        public static CriticalHitDescription[,] pc_crit_succ_eff = new CriticalHitDescription[(int)HitLocation.HIT_LOCATION_COUNT,CRTICIAL_EFFECT_COUNT]{
            {
                new CriticalHitDescription( 3, 0, -1, 0, 0, 6500, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 3, (int)Dam.DAM_KNOCKED_DOWN, 6501, 6503 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 6501, 6503 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 2, (int)Dam.DAM_KNOCKED_OUT, 6503, 6502 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_BLIND, 6502, 6504 ),
                new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_DEAD, 6501, 6505 ),
            },
            {
                new CriticalHitDescription( 2, 0, -1, 0, 0, 6506, 5000 ),
                new CriticalHitDescription( 2, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6507, 5000 ),
                new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 6508, 6509 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6501, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6510, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6510, 5000 ),
            },
            {
                new CriticalHitDescription( 2, 0, -1, 0, 0, 6506, 5000 ),
                new CriticalHitDescription( 2, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6507, 5000 ),
                new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 6508, 6509 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6501, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6511, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6511, 5000 ),
            },
            {
                new CriticalHitDescription( 3, 0, -1, 0, 0, 6512, 5000 ),
                new CriticalHitDescription( 3, 0, -1, 0, 0, 6512, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6508, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6503, 5000 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6503, 5000 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_DEAD, 6503, 6513 ),
            },
            {
                new CriticalHitDescription( 3, 0, -1, 0, 0, 6512, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6514, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 6514, 6515 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6516, 5000 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6516, 5000 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6517, 5000 ),
            },
            {
                new CriticalHitDescription( 3, 0, -1, 0, 0, 6512, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6514, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 6514, 6515 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6516, 5000 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6516, 5000 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6517, 5000 ),
            },
            {
                new CriticalHitDescription( 3, 0, -1, 0, 0, 6518, 5000 ),
                new CriticalHitDescription( 3, 0, (int)Stat.STAT_LUCK, 3, (int)Dam.DAM_BLIND, 6518, 6519 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 3, (int)Dam.DAM_BLIND, 6501, 6519 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6520, 5000 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6521, 5000 ),
                new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6522, 5000 ),
            },
            {
                new CriticalHitDescription( 3, 0, -1, 0, 0, 6523, 5000 ),
                new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 6523, 6524 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6524, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 4, (int)Dam.DAM_KNOCKED_OUT, 6524, 6525 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 2, (int)Dam.DAM_KNOCKED_OUT, 6524, 6525 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT, -1, 0, 0, 6526, 5000 ),
            },
            {
                new CriticalHitDescription( 3, 0, -1, 0, 0, 6512, 5000 ),
                new CriticalHitDescription( 3, 0, -1, 0, 0, 6512, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6508, 5000 ),
                new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6503, 5000 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6503, 5000 ),
                new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_DEAD, 6503, 6513 ),
            },
        };

        // Critical hit tables for every kill type.
        //
        // 0x510978
        public static CriticalHitDescription[,,] crit_succ_eff = new CriticalHitDescription[(int)KillType.KILL_TYPE_COUNT,(int)HitLocation.HIT_LOCATION_COUNT,(int)CRTICIAL_EFFECT_COUNT]{
            // KILL_TYPE_MAN
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5002, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 5002, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 5004, 5003 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BLIND, 5005, 5006 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5007, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5008, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5009, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_LEFT, 5010, 5011 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5012, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5012, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5013, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5008, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5009, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_RIGHT, 5014, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5015, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5015, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5013, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5017, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5019, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5019, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5020, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5021, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5023, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5023, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_RIGHT, 5023, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5025, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5025, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5026, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5023, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5023, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_LEFT, 5023, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5025, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5025, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5026, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_LUCK, 4, (int)Dam.DAM_BLIND, 5027, 5028 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 3, (int)Dam.DAM_BLIND, 5029, 5028 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_BLIND, 5029, 5028 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5030, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 5031, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 5032, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5033, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 5034, 5035 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 5035, 5036 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_OUT, -1, 0, 0, 5036, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5035, 5036 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5037, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5017, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5018, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5019, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5020, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5021, 5000 ),
                },
            },
            // KILL_TYPE_WOMAN
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5101, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5102, 5103 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 5102, 5103 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 5104, 5103 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BLIND, 5105, 5106 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5107, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5108, 5100 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5109, 5100 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_ARM_LEFT, 5110, 5111 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_ARM_LEFT, 5110, 5111 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5112, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5113, 5100 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5108, 5100 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5109, 5100 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_ARM_RIGHT, 5114, 5100 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_ARM_RIGHT, 5114, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5115, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5113, 5100 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5116, 5100 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5117, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5119, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5119, 5100 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5120, 5100 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5121, 5100 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5123, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5123, 5124 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_RIGHT, 5123, 5124 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5125, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5125, 5126 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5126, 5100 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5123, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5123, 5124 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_LEFT, 5123, 5124 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5125, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5125, 5126 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5126, 5100 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_LUCK, 4, (int)Dam.DAM_BLIND, 5127, 5128 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 3, (int)Dam.DAM_BLIND, 5129, 5128 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_BLIND, 5129, 5128 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5130, 5100 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 5131, 5100 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 5132, 5100 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5133, 5100 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 5133, 5134 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5134, 5135 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_OUT, -1, 0, 0, 5135, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5134, 5135 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5135, 5100 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5116, 5100 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5117, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5119, 5100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5119, 5100 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5120, 5100 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5121, 5100 ),
                },
            },
            // KILL_TYPE_CHILD
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5200, 5201 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_KNOCKED_OUT, 5202, 5203 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_KNOCKED_OUT, 5202, 5203 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5203, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5203, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5204, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5205, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 5206, 5207 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_ARM_LEFT, 5206, 5207 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5208, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5208, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5208, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5209, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 5206, 5207 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_ARM_RIGHT, 5206, 5207 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5208, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5208, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5208, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5210, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5211, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5212, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5212, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5213, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5214, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5215, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BLIND | (int)Dam.DAM_ON_FIRE | (int)Dam.DAM_EXPLODE, 5000, 0 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BLIND | (int)Dam.DAM_ON_FIRE | (int)Dam.DAM_EXPLODE, 5000, 0 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5217, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5217, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5217, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5215, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BLIND | (int)Dam.DAM_ON_FIRE | (int)Dam.DAM_EXPLODE, 5000, 0 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BLIND | (int)Dam.DAM_ON_FIRE | (int)Dam.DAM_EXPLODE, 5000, 0 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5217, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5217, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5217, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_LUCK, 5, (int)Dam.DAM_BLIND, 5218, 5219 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_BLIND, 5220, 5221 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, -1, (int)Dam.DAM_BLIND, 5220, 5221 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5222, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 5223, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 5224, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5225, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5225, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5226, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5226, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5226, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5226, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5210, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5211, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5211, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5212, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5213, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5214, 5000 ),
                },
            },
            // KILL_TYPE_SUPER_MUTANT
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5300, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_KNOCKED_DOWN, 5301, 5302 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_KNOCKED_DOWN, 5301, 5302 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 5302, 5303 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5302, 5303 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5304, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5300, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_LOSE_TURN, 5300, 5306 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_CRIP_ARM_LEFT, 5307, 5308 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_LEFT, 5307, 5308 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5308, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5308, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5300, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_LOSE_TURN, 5300, 5006 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_CRIP_ARM_RIGHT, 5307, 5309 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_RIGHT, 5307, 5309 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5309, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5309, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5300, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5301, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5302, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5302, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5310, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5311, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5300, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5300, 5312 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_RIGHT, 5312, 5313 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5313, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5314, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5315, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5300, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5300, 5312 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_LEFT, 5312, 5313 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5313, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5314, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5315, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5300, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 5, (int)Dam.DAM_BLIND, 5316, 5317 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 3, (int)Dam.DAM_BLIND, 5316, 5317 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BLIND, 5318, 5319 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 5320, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 5321, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5300, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BYPASS, 5300, 5017 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_KNOCKED_DOWN, 5301, 5302 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5312, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5302, 5303 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5303, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5300, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5301, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5302, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5302, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5310, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5311, 5000 ),
                },
            },
            // KILL_TYPE_GHOUL
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5400, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_KNOCKED_OUT, 5400, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_KNOCKED_OUT, 5004, 5005 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_STRENGTH, 0, 0, 5005, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5401, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_DROP | (int)Dam.DAM_LOSE_TURN, 5001, 5402 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DROP | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 5402, 5012 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS | (int)Dam.DAM_DROP | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 5403, 5404 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS | (int)Dam.DAM_DROP, -1, 0, 0, 5404, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS | (int)Dam.DAM_DROP, -1, 0, 0, 5404, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_DROP | (int)Dam.DAM_LOSE_TURN, 5001, 5402 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DROP | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 5402, 5015 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS | (int)Dam.DAM_DROP | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 5403, 5404 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS | (int)Dam.DAM_DROP, -1, 0, 0, 5404, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS | (int)Dam.DAM_DROP, -1, 0, 0, 5404, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5017, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5018, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5004, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5003, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5007, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5023 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5023, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5024, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5024, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5026, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5023 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5023, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5024, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5024, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5026, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_LUCK, 3, (int)Dam.DAM_BLIND, 5001, 5405 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BLIND, 5406, 5407 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, -3, (int)Dam.DAM_BLIND, 5406, 5407 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5030, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 5031, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 5408, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BYPASS, 5001, 5033 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 5033, 5035 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5004, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5035, 5036 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5036, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5017, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5018, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5004, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5003, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5007, 5000 ),
                },
            },
            // KILL_TYPE_BRAHMIN
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_ENDURANCE, 2, (int)Dam.DAM_KNOCKED_DOWN, 5016, 5500 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_KNOCKED_DOWN, 5016, 5500 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT, (int)Stat.STAT_STRENGTH, 0, 0, 5501, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5502, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5016, 5503 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5016, 5503 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5503, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5503, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5016, 5503 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5016, 5503 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5503, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5503, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5504, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5504, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5504, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5505, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5505, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5506, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5016, 5503 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5016, 5503 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5503, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5503, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5016, 5503 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5016, 5503 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5503, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5503, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BLIND, 5029, 5507 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, -3, (int)Dam.DAM_BLIND, 5029, 5507 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5508, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 5509, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 5510, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5511, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5511, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5512, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5512, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, -1, 0, 0, 5513, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5504, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5504, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5504, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5505, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5505, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5506, 5000 ),
                },
            },
            // KILL_TYPE_RADSCORPION
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 3, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5600 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5600 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5600 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5600, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5601, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 5016, 5602 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 5602, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 5602, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 2, (int)Dam.DAM_CRIP_ARM_RIGHT, 5016, 5603 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 5016, 5603 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 5603, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5604, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5605, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5605, 5606 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5607, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 2, 0, 5001, 5600 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5600, 5608 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5609, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5608, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5608, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 2, 0, 5001, 5600 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5600, 5008 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5609, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5608, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5608, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_AGILITY, 3, (int)Dam.DAM_BLIND, 5001, 5610 ),
                    new CriticalHitDescription( 6, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_BLIND, 5016, 5610 ),
                    new CriticalHitDescription( 6, 0, (int)Stat.STAT_AGILITY, -3, (int)Dam.DAM_BLIND, 5016, 5610 ),
                    new CriticalHitDescription( 8, 0, (int)Stat.STAT_AGILITY, -3, (int)Dam.DAM_BLIND, 5611, 5612 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 5613, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5614, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5614, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5614, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT, -1, 0, 0, 5615, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT, -1, 0, 0, 5615, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5616, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 5604, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5605, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5605, 5606 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5607, 5000 ),
                },
            },
            // KILL_TYPE_RAT
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5700, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5700, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5701, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5701, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5701, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5701, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 5703, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 5703, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 5703, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 5703, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 5703, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 5703, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 5705, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 5705, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 5705, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 5705, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 5705, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 5705, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5706, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5707, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5707, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5707, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5707, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5708, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5709, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5709, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5709, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5709, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5709, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5709, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5710, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5710, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5710, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5710, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5710, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5710, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5711, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5712, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5712, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5712, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5712, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5712, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5711, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5711, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5712, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5712, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5706, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5707, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5707, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5707, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5707, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5708, 5000 ),
                },
            },
            // KILL_TYPE_FLOATER
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5800 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_AGILITY, -3, (int)Dam.DAM_KNOCKED_DOWN, 5016, 5800 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 5800, 5801 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 5800, 5801 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5802, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_LOSE_TURN, 5001, 5803 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_LOSE_TURN, 5001, 5803 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5804, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5804, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5805, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_LOSE_TURN, 5001, 5803 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_LOSE_TURN, 5001, 5803 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5804, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5804, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5805, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5800 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, -2, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5800 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5800, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5804, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5805, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 1, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5800 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_AGILITY, -1, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5800 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_CRIP_LEG_RIGHT, 5800, 5806 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_CRIP_LEG_RIGHT, 5804, 5806 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD | (int)Dam.DAM_ON_FIRE, -1, 0, 0, 5807, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5803, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5803, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5808, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5808, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5809, 5000 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_BLIND, 5016, 5810 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_BLIND, 5809, 5810 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 5810, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 5801, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5800 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5800 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5800, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5800, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5800 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, -2, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5800 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5800, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5804, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 5805, 5000 ),
                },
            },
            // KILL_TYPE_CENTAUR
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, 5016, 5900 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, 5016, 5900 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, 5901, 5900 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, 5901, 5900 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 5902, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_LOSE_TURN, 5016, 5903 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 5016, 5904 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 5904, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5905, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_LOSE_TURN, 5016, 5903 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 5016, 5904 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 5904, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 5905, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5901, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 2, 0, 5901, 5900 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5900, 5000 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_DEAD, -1, 0, 0, 5902, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5900, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5900, 5906 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5906, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 5906, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5907, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 5900, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5900, 5906 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5906, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 5906, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5907, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 1, (int)Dam.DAM_BLIND, 5001, 5908 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_BLIND, 5901, 5908 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5909, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 5910, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 5911, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 2, 0, -1, 0, 0, 5912, 5000 ),
                    new CriticalHitDescription( 2, 0, -1, 0, 0, 5912, 5000 ),
                    new CriticalHitDescription( 2, 0, -1, 0, 0, 5912, 5000 ),
                    new CriticalHitDescription( 2, 0, -1, 0, 0, 5912, 5000 ),
                    new CriticalHitDescription( 2, 0, -1, 0, 0, 5912, 5000 ),
                    new CriticalHitDescription( 2, 0, -1, 0, 0, 5912, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5901, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 2, 0, 5901, 5900 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5900, 5000 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_DEAD, -1, 0, 0, 5902, 5000 ),
                },
            },
            // KILL_TYPE_ROBOT
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 5, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6001, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6002, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6003, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 6000, 6004 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_LEFT, 6000, 6004 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 6004, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 6004, 6005 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 6000, 6004 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_RIGHT, 6000, 6004 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 6004, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 6004, 6005 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6006, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6007, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 6008, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, -1, 0, 0, 6009, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6010, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6007, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 6000, 6004 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_LEG_RIGHT, 6007, 6004 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 6004, 6011 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, 6004, 6012 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6007, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 6000, 6004 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_LEG_LEFT, 6007, 6004 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 6004, 6011 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, 6004, 6012 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_BLIND, 6000, 6013 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_BLIND, 6000, 6013 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_BLIND, 6000, 6013 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -6, (int)Dam.DAM_BLIND, 6000, 6013 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BLIND, -1, 0, 0, 6013, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, 6000, 6002 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, 6000, 6002 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, 0, 0, 6002, 6003 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, -4, 0, 6002, 6003 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6000, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6006, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6007, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 6008, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, -1, 0, 0, 6009, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6010, 5000 ),
                },
            },
            // KILL_TYPE_DOG
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 5016, 6100 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 5016, 6100 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -6, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_CRIP_ARM_RIGHT, 5016, 6101 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6100, 6102 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_DEAD, -1, 0, 0, 6103, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_CRIP_LEG_LEFT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_LEFT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, 5001, 6105 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_LEFT, (int)Stat.STAT_AGILITY, -1, (int)Dam.DAM_KNOCKED_DOWN, 6104, 6105 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 6105, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 6105 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_RIGHT, (int)Stat.STAT_AGILITY, -1, (int)Dam.DAM_KNOCKED_DOWN, 6104, 6105 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 6105, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, -1, (int)Dam.DAM_KNOCKED_DOWN, 5001, 6100 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_AGILITY, -3, (int)Dam.DAM_KNOCKED_DOWN, 5016, 6100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6100, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6103, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 1, (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 6105 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_RIGHT, (int)Stat.STAT_AGILITY, -1, (int)Dam.DAM_KNOCKED_DOWN, 6104, 6105 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 6105, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 1, (int)Dam.DAM_CRIP_LEG_LEFT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_LEG_LEFT, 5001, 6104 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, 5001, 6105 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_CRIP_LEG_LEFT, (int)Stat.STAT_AGILITY, -1, (int)Dam.DAM_KNOCKED_DOWN, 6104, 6105 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 6105, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 5018, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, -1, 0, 0, 5018, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 3, (int)Dam.DAM_BLIND, 5018, 6106 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_BLIND, 5018, 6106 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 6107, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, -2, (int)Dam.DAM_KNOCKED_DOWN, 5001, 6100 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_AGILITY, -5, (int)Dam.DAM_KNOCKED_DOWN, 5016, 6100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6100, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6103, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, -1, (int)Dam.DAM_KNOCKED_DOWN, 5001, 6100 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_AGILITY, -3, (int)Dam.DAM_KNOCKED_DOWN, 5016, 6100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6100, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6103, 5000 ),
                },
            },
            // KILL_TYPE_MANTIS
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 6200 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 5016, 6200 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_KNOCKED_OUT, 6200, 6201 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6200, 6201 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6202, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 5001, 6203 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 5001, 6203 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_ARM_LEFT, 5001, 6203 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_ARM_LEFT, 5016, 6203 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_ARM_LEFT, 5016, 6203 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6204, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 5001, 6203 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 5001, 6203 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_ARM_RIGHT, 5001, 6203 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_ARM_RIGHT, 5016, 6203 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_ARM_RIGHT, 5016, 6203 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6204, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 1000, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_BYPASS, 5001, 6205 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_BYPASS, 5001, 6205 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_BYPASS, 5016, 6205 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_BYPASS, 5016, 6205 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6206, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 6201 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, -2, (int)Dam.DAM_KNOCKED_DOWN, 5001, 6201 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_AGILITY, -4, (int)Dam.DAM_KNOCKED_DOWN, 5001, 6201 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 6201, 6203 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_RIGHT, 6201, 6203 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 6207, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 6201 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, -3, (int)Dam.DAM_KNOCKED_DOWN, 5001, 6201 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_LEG_LEFT, 6201, 6208 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_LEG_LEFT, 6201, 6208 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -5, (int)Dam.DAM_CRIP_LEG_LEFT, 6201, 6208 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 6208, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_LOSE_TURN, 6205, 6209 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_LOSE_TURN, 6205, 6209 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_BLIND, 6209, 6210 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_BLIND, 6209, 6210 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 6202, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6205, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6209, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 1000, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_BYPASS, 5001, 6205 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_BYPASS, 5001, 6205 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_BYPASS, 5016, 6205 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_BYPASS, 5016, 6205 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6206, 5000 ),
                },
            },
            // KILL_TYPE_DEATH_CLAW
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_DOWN, 5016, 5023 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 5016, 5023 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_ENDURANCE, -5, (int)Dam.DAM_KNOCKED_DOWN, 5016, 5023 ),
                    new CriticalHitDescription( 6, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, 5016, 5004 ),
                    new CriticalHitDescription( 6, 0, (int)Stat.STAT_ENDURANCE, -5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_LOSE_TURN, 5016, 5004 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_LEFT, 5001, 5011 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_ARM_LEFT, 5001, 5011 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_ARM_LEFT, 5001, 5011 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -6, (int)Dam.DAM_CRIP_ARM_LEFT, 5001, 5011 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -8, (int)Dam.DAM_CRIP_ARM_LEFT, 5001, 5011 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_ARM_RIGHT, 5001, 5014 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_ARM_RIGHT, 5001, 5014 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_ARM_RIGHT, 5001, 5014 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -6, (int)Dam.DAM_CRIP_ARM_RIGHT, 5001, 5014 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -8, (int)Dam.DAM_CRIP_ARM_RIGHT, 5001, 5014 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_BYPASS, 5001, 6300 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_BYPASS, 5016, 6300 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5004, 5000 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5005, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5004 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 5004 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 5004 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_LEG_RIGHT, 5016, 5022 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -5, (int)Dam.DAM_CRIP_LEG_RIGHT, 5023, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -6, (int)Dam.DAM_CRIP_LEG_RIGHT, 5023, 5024 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5001, 5004 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 5004 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_CRIP_LEG_RIGHT, 5001, 5004 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_CRIP_LEG_RIGHT, 5016, 5022 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -5, (int)Dam.DAM_CRIP_LEG_RIGHT, 5023, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -6, (int)Dam.DAM_CRIP_LEG_RIGHT, 5023, 5024 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_LOSE_TURN, 5001, 6301 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -6, (int)Dam.DAM_LOSE_TURN, 6300, 6301 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -2, (int)Dam.DAM_BLIND, 6301, 6302 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 6302, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 6302, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_AGILITY, 0, (int)Dam.DAM_KNOCKED_DOWN, 5016, 5004 ),
                    new CriticalHitDescription( 5, 0, (int)Stat.STAT_AGILITY, -3, (int)Dam.DAM_KNOCKED_DOWN, 5016, 5004 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 5001, 5000 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_BYPASS, 5001, 6300 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 5016, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -1, (int)Dam.DAM_BYPASS, 5016, 6300 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 5004, 5000 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 5005, 5000 ),
                },
            },
            // KILL_TYPE_PLANT
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 6400, 5000 ),
                    new CriticalHitDescription( 5, 0, -1, 0, 0, 6401, 5000 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, -1, 0, 0, 6402, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_LOSE_TURN, 6402, 6403 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -6, (int)Dam.DAM_LOSE_TURN, 6402, 6403 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6400, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6401, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6401, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 6402, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 6400, 5000 ),
                    new CriticalHitDescription( 5, 0, -1, 0, 0, 6401, 5000 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, -1, 0, 0, 6402, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -4, (int)Dam.DAM_BLIND, 6402, 6406 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 6406, 6404 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 6402, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 6402, 5000 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_LOSE_TURN, 6402, 6403 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -6, (int)Dam.DAM_LOSE_TURN, 6402, 6403 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6405, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6400, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6401, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6401, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, -1, 0, 0, 6402, 5000 ),
                },
            },
            // KILL_TYPE_GECKO
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6701, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6700, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6700, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6700, 5003 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BLIND, 6700, 5006 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6700, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6702, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6702, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_LEFT, 6702, 5011 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6702, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6702, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6702, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6702, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6702, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_RIGHT, 6702, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6702, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6702, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6702, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6701, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6701, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6704, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6704, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6704, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6704, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6705, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 6705, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_RIGHT, 6705, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6705, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6705, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6705, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6705, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 6705, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_LEFT, 6705, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6705, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6705, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6705, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_LUCK, 4, (int)Dam.DAM_BLIND, 6700, 5028 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 3, (int)Dam.DAM_BLIND, 6700, 5028 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_BLIND, 6700, 5028 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6700, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 6700, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 6700, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6703, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 6703, 5035 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6703, 5036 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_OUT, -1, 0, 0, 6703, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6703, 5036 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6703, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6700, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6700, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6700, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6700, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6700, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6700, 5000 ),
                },
            },
            // KILL_TYPE_ALIEN
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6801, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6800, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6800, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6803, 5003 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BLIND, 6804, 5006 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6804, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6806, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6806, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_LEFT, 6806, 5011 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6806, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6806, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6806, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6806, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6806, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_RIGHT, 6806, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6806, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6806, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6806, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6800, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6805, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 6805, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_RIGHT, 6805, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6805, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6805, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6805, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6805, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 6805, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_LEFT, 6805, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6805, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6805, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6805, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_LUCK, 4, (int)Dam.DAM_BLIND, 6803, 5028 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 3, (int)Dam.DAM_BLIND, 6803, 5028 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_BLIND, 6803, 5028 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6803, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 6803, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 6804, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6801, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 6801, 5035 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6801, 5036 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_OUT, -1, 0, 0, 6801, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6804, 5036 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6804, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6800, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6800, 5000 ),
                },
            },
            // KILL_TYPE_GIANT_ANT
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6901, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6901, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6902, 5003 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6902, 5003 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BLIND, 6902, 5006 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6902, 5000 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6906, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6906, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_LEFT, 6906, 5011 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6906, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6906, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6906, 5000 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6906, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6906, 5000 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_ARM_RIGHT, 6906, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6906, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6906, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6906, 5000 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6900, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6900, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6904, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6904, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6904, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6904, 5000 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6905, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 6905, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_RIGHT, 6905, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6905, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6905, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_RIGHT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6905, 5000 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 6905, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 6905, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_LEFT, 6905, 5024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6905, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6905, 5026 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_CRIP_LEG_LEFT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6905, 5000 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_LUCK, 4, (int)Dam.DAM_BLIND, 6900, 5028 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 3, (int)Dam.DAM_BLIND, 6906, 5028 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BYPASS, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_BLIND, 6901, 5028 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 6901, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BLIND | (int)Dam.DAM_BYPASS, -1, 0, 0, 6901, 5000 ),
                    new CriticalHitDescription( 8, (int)Dam.DAM_DEAD, -1, 0, 0, 6901, 5000 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6900, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 6900, 5035 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_OUT, 6900, 5036 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_OUT, -1, 0, 0, 6903, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_KNOCKED_OUT, 6903, 5036 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6903, 5000 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 6900, 5000 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_BYPASS, -1, 0, 0, 6900, 5000 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 6904, 5000 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_BYPASS, -1, 0, 0, 6904, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_OUT | (int)Dam.DAM_BYPASS, -1, 0, 0, 6904, 5000 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_DEAD, -1, 0, 0, 6904, 5000 ),
                },
            },
            // KILL_TYPE_BIG_BAD_BOSS
            {
                // HIT_LOCATION_HEAD
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7101, 7100 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7102, 7103 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 7102, 7103 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 7104, 7103 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_LUCK, 0, (int)Dam.DAM_BLIND, 7105, 7106 ),
                    new CriticalHitDescription( 6, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7105, 7100 ),
                },
                // HIT_LOCATION_LEFT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 7106, 7011 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_LEFT, -1, 0, 0, 7106, 7100 ),
                },
                // HIT_LOCATION_RIGHT_ARM
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_ARM_RIGHT, -1, 0, 0, 7106, 7100 ),
                },
                // HIT_LOCATION_TORSO
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7106, 7100 ),
                },
                // HIT_LOCATION_RIGHT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_RIGHT, 7106, 7106 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_RIGHT, 7060, 7106 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 7106, 7106 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_RIGHT, -1, 0, 0, 7106, 7100 ),
                },
                // HIT_LOCATION_LEFT_LEG
                {
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, 0, (int)Dam.DAM_CRIP_LEG_LEFT, 7106, 7024 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_CRIP_LEG_LEFT, 7106, 7024 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN | (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 7106, 7106 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_CRIP_LEG_LEFT, -1, 0, 0, 7106, 7100 ),
                },
                // HIT_LOCATION_EYES
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7106 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7106 ),
                    new CriticalHitDescription( 4, 0, (int)Stat.STAT_LUCK, 2, (int)Dam.DAM_BLIND, 7106, 7106 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BLIND | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_BLIND | (int)Dam.DAM_LOSE_TURN, -1, 0, 0, 7106, 7100 ),
                },
                // HIT_LOCATION_GROIN
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, 0, (int)Stat.STAT_ENDURANCE, -3, (int)Dam.DAM_KNOCKED_DOWN, 7106, 7106 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7106, 7106 ),
                    new CriticalHitDescription( 3, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 7106, 7106 ),
                    new CriticalHitDescription( 4, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7106, 7100 ),
                },
                // HIT_LOCATION_UNCALLED
                {
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 3, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 4, 0, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7106, 7100 ),
                    new CriticalHitDescription( 5, (int)Dam.DAM_KNOCKED_DOWN, -1, 0, 0, 7106, 7100 ),
                },
            },
        };

        public static int compute_attack(Attack attack)
        {
            int range = item_w_range(attack.attacker, attack.hitMode);
            int distance = obj_dist(attack.attacker, attack.defender);

            if (range < distance) {
                return -1;
            }

            int anim = item_w_anim(attack.attacker, attack.hitMode);
            // int accuracy = determine_to_hit_func(attack->attacker, attack->attacker->tile, attack->defender, attack->defenderHitLocation, attack->hitMode, 1);

            bool isGrenade = false;
            int damageType = item_w_damage_type(attack.attacker, attack.weapon);
            // if (anim == ANIM_THROW_ANIM && (damageType == DAMAGE_TYPE_EXPLOSION || damageType == DAMAGE_TYPE_PLASMA || damageType == DAMAGE_TYPE_EMP)) {
            //     isGrenade = true;
            // }

            if (attack.defenderHitLocation == (int)HitLocation.HIT_LOCATION_UNCALLED) {
                attack.defenderHitLocation = (int)HitLocation.HIT_LOCATION_TORSO;
            }

            int attackType = item_w_subtype(attack.weapon, attack.hitMode);
            int roundsHitMainTarget = 1;
            int damageMultiplier = 2;
            int roundsSpent = 1;

            // TODO
            int roll = (int)Roll.ROLL_SUCCESS;

            // if (anim == (int)AnimationType.ANIM_FIRE_BURST || anim == (int)AnimationType.ANIM_FIRE_CONTINUOUS) {
            //     roll = compute_spray(attack, accuracy, &roundsHitMainTarget, &roundsSpent, anim);
            // } else {
            //     int chance = critterGetStat(attack->attacker, (int)Stat.STAT_CRITICAL_CHANCE);
            //     roll = roll_check(accuracy, chance - hit_location_penalty[attack->defenderHitLocation], NULL);
            // }

            if (roll == (int)Roll.ROLL_FAILURE) {
                if (trait_level((int)Trait.TRAIT_JINXED) || perkHasRank(obj_dude, Perk.PERK_JINXED)) {
                    if (roll_random(0, 1) == 1) {
                        roll = (int)Roll.ROLL_CRITICAL_FAILURE;
                    }
                }
            }

            if (roll == (int)Roll.ROLL_SUCCESS) {
                if ((attackType == (int)AttackType.ATTACK_TYPE_MELEE || attackType == (int)AttackType.ATTACK_TYPE_UNARMED) && attack.attacker == obj_dude) {
                    if (perkHasRank(attack.attacker, Perk.PERK_SLAYER)) {
                        roll = (int)Roll.ROLL_CRITICAL_SUCCESS;
                    }

                    if (perkHasRank(obj_dude, Perk.PERK_SILENT_DEATH)
                        && !is_hit_from_front(obj_dude, attack.defender)
                        && is_pc_flag((int)DudeState.DUDE_STATE_SNEAKING)
                        && obj_dude != attack.defender.data.critter.combat.whoHitMe) {
                        damageMultiplier = 4;
                    }

                    if (((attack.hitMode == (int)HitMode.HIT_MODE_HAMMER_PUNCH || attack.hitMode == (int)HitMode.HIT_MODE_POWER_KICK) && roll_random(1, 100) <= 5)
                        || ((attack.hitMode == (int)HitMode.HIT_MODE_JAB || attack.hitMode == (int)HitMode.HIT_MODE_HOOK_KICK) && roll_random(1, 100) <= 10)
                        || (attack.hitMode == (int)HitMode.HIT_MODE_HAYMAKER && roll_random(1, 100) <= 15)
                        || (attack.hitMode == (int)HitMode.HIT_MODE_PALM_STRIKE && roll_random(1, 100) <= 20)
                        || (attack.hitMode == (int)HitMode.HIT_MODE_PIERCING_STRIKE && roll_random(1, 100) <= 40)
                        || (attack.hitMode == (int)HitMode.HIT_MODE_PIERCING_KICK && roll_random(1, 100) <= 50)) {
                        roll = (int)Roll.ROLL_CRITICAL_SUCCESS;
                    }
                }
            }

            if (attackType == (int)AttackType.ATTACK_TYPE_RANGED) {
                attack.ammoQuantity = roundsSpent;

                if (roll == (int)Roll.ROLL_SUCCESS && attack.attacker == obj_dude) {
                    if (perk_level(obj_dude, (int)Perk.PERK_SNIPER) != 0) {
                        int d10 = roll_random(1, 10);
                        int luck = critterGetStat(obj_dude, (int)Stat.STAT_LUCK);
                        if (d10 <= luck) {
                            roll = (int)Roll.ROLL_CRITICAL_SUCCESS;
                        }
                    }
                }
            } else {
                if (item_w_max_ammo(attack.weapon) > 0) {
                    attack.ammoQuantity = 1;
                }
            }

            if (item_w_compute_ammo_cost(attack.weapon, ref attack.ammoQuantity) == -1) {
                return -1;
            }

            switch ((Roll)roll) 
            {
            case Roll.ROLL_CRITICAL_SUCCESS:
                // FALLTHROUGH
            case Roll.ROLL_SUCCESS:
                if (roll == (int)Roll.ROLL_CRITICAL_SUCCESS)
                {
                    damageMultiplier = attack_crit_success(attack);
                }

                attack.attackerFlags |= (int)Dam.DAM_HIT;
                correctAttackForPerks(attack);
                compute_damage(attack, roundsHitMainTarget, damageMultiplier);
                break;
            case Roll.ROLL_FAILURE:
                if (attackType == (int)AttackType.ATTACK_TYPE_RANGED || attackType == (int)AttackType.ATTACK_TYPE_THROW) {
                    check_ranged_miss(attack);
                }
                break;
            case Roll.ROLL_CRITICAL_FAILURE:
                attack_crit_failure(attack);
                break;
            }

            if (attackType == (int)AttackType.ATTACK_TYPE_RANGED || attackType == (int)AttackType.ATTACK_TYPE_THROW)
            {
                if ((attack.attackerFlags & ((int)Dam.DAM_HIT | (int)Dam.DAM_CRITICAL)) == 0) {
                    int tile;
                    if (isGrenade) {
                        int throwDistance = roll_random(1, distance / 2);
                        if (throwDistance == 0) {
                            throwDistance = 1;
                        }

                        int rotation = roll_random(0, 5);
                        tile = tile_num_in_direction(attack.defender.tile, rotation, throwDistance);
                    } else {
                        tile = tile_num_beyond(attack.attacker.tile, attack.defender.tile, range);
                    }

                    attack.tile = tile;

                    f2Object v25 = attack.defender;
                    // make_straight_path_func(v25, attack.defender.tile, attack.tile, null, v25, 32, obj_shoot_blocking_at);
                    // if (v25 != null && v25 != attack->defender) 
                    // {
                    //     attack.tile = v25.tile;
                    // } 
                    // else 
                    // {
                    //     v25 = obj_blocking_at(NULL, attack->tile, attack->defender->elevation);
                    // }

                    if (v25 != null && (v25.flags & (uint)ObjectFlags.OBJECT_SHOOT_THRU) == 0) 
                    {
                        attack.attackerFlags |= (int)Dam.DAM_HIT;
                        attack.defender = v25;
                        compute_damage(attack, 1, 2);
                    }
                }
            }

            if ((damageType == (int)DamageType.DAMAGE_TYPE_EXPLOSION || isGrenade) && ((attack.attackerFlags & (int)Dam.DAM_HIT) != 0 || (attack.attackerFlags & (int)Dam.DAM_CRITICAL) == 0)) 
            {
                compute_explosion_on_extras(attack, 0, isGrenade, 0);
            } 
            else 
            {
                if ((attack.attackerFlags & (int)Dam.DAM_EXPLODE) != 0) {
                    compute_explosion_on_extras(attack, 1, isGrenade, 0);
                }
            }

            death_checks(attack);

            return 0;
        }

        static int attack_crit_failure(Attack attack)
        {
            // TODO
            return 0;
        }

        // compute_explosion_on_extras
        static void compute_explosion_on_extras(Attack attack, int a2, bool isGrenade, int a4)
        {
            // TODO
        }

        static void death_checks(Attack attack)
        {
            // TODO
            // check_for_death(attack->attacker, attack->attackerDamage, &(attack->attackerFlags));
            // check_for_death(attack->defender, attack->defenderDamage, &(attack->defenderFlags));

            // for (int index = 0; index < attack->extrasLength; index++) {
            //     check_for_death(attack->extras[index], attack->extrasDamage[index], &(attack->extrasFlags[index]));
            // }
        }

        public static int attack_crit_success(Attack attack)
        {
            f2Object defender = attack.defender;
            if (defender != null && critter_flag_check(defender.pid, (int)CritterFlags.CRITTER_INVULNERABLE)) {
                return 2;
            }

            if (defender != null && PID_TYPE(defender.pid) != (int)ObjType.OBJ_TYPE_CRITTER) {
                return 2;
            }

            attack.attackerFlags |= (int)Dam.DAM_CRITICAL;

            int chance = roll_random(1, 100);

            chance += critterGetStat(attack.attacker, Stat.STAT_BETTER_CRITICALS);

            int effect;
            if (chance <= 20)
                effect = 0;
            else if (chance <= 45)
                effect = 1;
            else if (chance <= 70)
                effect = 2;
            else if (chance <= 90)
                effect = 3;
            else if (chance <= 100)
                effect = 4;
            else
                effect = 5;

            CriticalHitDescription criticalHitDescription;
            if (defender == obj_dude) {
                criticalHitDescription = pc_crit_succ_eff[attack.defenderHitLocation, effect];
            } else {
                int killType = critterGetKillType(defender);
                criticalHitDescription = crit_succ_eff[killType, attack.defenderHitLocation, effect];
            }

            attack.defenderFlags |= criticalHitDescription.flags;

            // NOTE: Original code is slightly different, it does not set message in
            // advance, instead using "else" statement.
            attack.criticalMessageId = criticalHitDescription.messageId;

            if (criticalHitDescription.massiveCriticalStat != -1) 
            {
                int howMuch = 0;
                if (stat_result(defender, criticalHitDescription.massiveCriticalStat, 
                    criticalHitDescription.massiveCriticalStatModifier, ref howMuch) <= (int)Roll.ROLL_FAILURE) 
                {
                    attack.defenderFlags |= criticalHitDescription.massiveCriticalFlags;
                    attack.criticalMessageId = criticalHitDescription.massiveCriticalMessageId;
                }
            }

            if ((attack.defenderFlags & (int)Dam.DAM_CRIP_RANDOM) != 0) {
                // NOTE: Uninline.
                do_random_cripple(ref attack.defenderFlags);
            }

            if (item_w_perk(attack.weapon) == (int)Perk.PERK_WEAPON_ENHANCED_KNOCKOUT) {
                attack.defenderFlags |= (int)Dam.DAM_KNOCKED_OUT;
            }

            f2Object weapon = null;
            if (defender != obj_dude) {
                weapon = item_hit_with(defender, (int)HitMode.HIT_MODE_RIGHT_WEAPON_PRIMARY);
            }

            int flags = attackFindInvalidFlags(defender, weapon);
            attack.defenderFlags &= ~flags;

            return criticalHitDescription.damageMultiplier;
        }

        static void do_random_cripple(ref int flagsPtr)
        {
            flagsPtr &= ~(int)Dam.DAM_CRIP_RANDOM;

            switch (roll_random(0, 3)) {
            case 0:
                flagsPtr |= (int)Dam.DAM_CRIP_LEG_LEFT;
                break;
            case 1:
                flagsPtr |= (int)Dam.DAM_CRIP_LEG_RIGHT;
                break;
            case 2:
                flagsPtr |= (int)Dam.DAM_CRIP_ARM_LEFT;
                break;
            case 3:
                flagsPtr |= (int)Dam.DAM_CRIP_ARM_RIGHT;
                break;
            }
        }

        public static int correctAttackForPerks(Attack attack)
        {
            if (item_w_perk(attack.weapon) == (int)Perk.PERK_WEAPON_ENHANCED_KNOCKOUT) 
            {
                int difficulty = critterGetStat(attack.attacker, (int)Stat.STAT_STRENGTH) - 8;
                int chance = roll_random(1, 100);
                if (chance <= difficulty) 
                {
                    f2Object weapon = null;
                    if (attack.defender != obj_dude) 
                    {
                        weapon = item_hit_with(attack.defender, (int)HitMode.HIT_MODE_RIGHT_WEAPON_PRIMARY);
                    }

                    if ((attackFindInvalidFlags(attack.defender, weapon) & 1) == 0) {
                        attack.defenderFlags |= (int)Dam.DAM_KNOCKED_OUT;
                    }
                }
            }

            return 0;
        }

        static int attackFindInvalidFlags(f2Object critter, f2Object item)
        {
            int flags = 0;

            if (critter != null && PID_TYPE(critter.pid) == (int)ObjType.OBJ_TYPE_CRITTER && critter_flag_check(critter.pid, (int)CritterFlags.CRITTER_NO_DROP)) {
                flags |= (int)Dam.DAM_DROP;
            }

            if (item != null && item_is_hidden(item) != 0) {
                flags |= (int)Dam.DAM_DROP;
            }

            return flags;
        }

        // determine_to_hit
        static int determine_to_hit_func(f2Object attacker, int tile, f2Object defender, int hitLocation, int hitMode, int a6)
        {
            // TODO
            return 0;
        }

        static bool check_ranged_miss(Attack attack)
        {
            int range = item_w_range(attack.attacker, attack.hitMode);
            int to = tile_num_beyond(attack.attacker.tile, attack.defender.tile, range);

            int roll = (int)Roll.ROLL_FAILURE;
            f2Object critter = attack.attacker;
            if (critter != null) 
            {
                int curr = attack.attacker.tile;
                while (curr != to) 
                {
                    // make_straight_path_func(attack.attacker, curr, to, null, &critter, 32, obj_shoot_blocking_at);
                    if (critter != null) 
                    {
                        if ((critter.flags & (uint)ObjectFlags.OBJECT_SHOOT_THRU) == 0) 
                        {
                            if (FID_TYPE(critter.fid) != (int)ObjType.OBJ_TYPE_CRITTER) {
                                roll = (int)Roll.ROLL_SUCCESS;
                                break;
                            }

                            if (critter != attack.defender) 
                            {
                                int v6 = determine_to_hit_func(attack.attacker, attack.attacker.tile, critter, attack.defenderHitLocation, attack.hitMode, 1) / 3;
                                if (critter_is_dead(critter)) 
                                {
                                    v6 = 5;
                                }

                                if (roll_random(1, 100) <= v6) {
                                    roll = (int)Roll.ROLL_SUCCESS;
                                    break;
                                }
                            }

                            curr = critter.tile;
                        }
                    }

                    if (critter == null) 
                    {
                        break;
                    }
                }
            }

            attack.defenderHitLocation = (int)HitLocation.HIT_LOCATION_TORSO;

            if (roll < (int)Roll.ROLL_SUCCESS || critter == null || (critter.flags & (uint)ObjectFlags.OBJECT_SHOOT_THRU) == 0) {
                return false;
            }

            attack.defender = critter;
            attack.tile = critter.tile;
            attack.attackerFlags |= (int)Dam.DAM_HIT;
            attack.defenderHitLocation = (int)HitLocation.HIT_LOCATION_TORSO;
            compute_damage(attack, 1, 2);
            return true;
        }

        static void combat_ctd_init(Attack attack, f2Object attacker, f2Object defender, int hitMode, int hitLocation)
        {
            attack.attacker = attacker;
            attack.hitMode = hitMode;
            attack.weapon = item_hit_with(attacker, hitMode);
            attack.attackHitLocation = (int)HitLocation.HIT_LOCATION_TORSO;
            attack.attackerDamage = 0;
            attack.attackerFlags = 0;
            attack.ammoQuantity = 0;
            attack.criticalMessageId = -1;
            attack.defender = defender;
            attack.tile = defender != null ? defender.tile : -1;
            attack.defenderHitLocation = hitLocation;
            attack.defenderDamage = 0;
            attack.defenderFlags = 0;
            attack.defenderKnockback = 0;
            attack.extrasLength = 0;
            attack.oops = defender;
        }

        public class STRUCT_664980 {
            public f2Object attacker;
            public f2Object defender;
            public int actionPointsBonus;
            public int accuracyBonus;
            public int damageBonus;
            public int minDamage;
            public int maxDamage;
            public int field_1C; // probably bool, indicating field_20 and field_24 used
            public int field_20; // flags on attacker
            public int field_24; // flags on defender
        };

        static STRUCT_664980 gcsd = null;

        static Attack main_ctd = new Attack();
        // bonus action points from BONUS_MOVE perk.
        static int combat_free_move;
        static bool combat_call_display = false;
        static bool combat_cleanup_enabled = false;

        static int combat_attack(f2Object a1, f2Object a2, int hitMode, int hitLocation)
        {
            // if (a1 != obj_dude && hitMode == (int)HitMode.HIT_MODE_PUNCH && roll_random(1, 4) == 1) {
            //     int fid = art_id(OBJ_TYPE_CRITTER, a1->fid & 0xFFF, ANIM_KICK_LEG, (a1->fid & 0xF000) >> 12, (a1->fid & 0x70000000) >> 28);
            //     if (art_exists(fid)) {
            //         hitMode = HIT_MODE_KICK;
            //     }
            // }

            combat_ctd_init(main_ctd, a1, a2, hitMode, hitLocation);
            // debug_printf("computing attack...\n");

            if (compute_attack(main_ctd) == -1) {
                return -1;
            }

            if (gcsd != null)
            {
                main_ctd.defenderDamage += gcsd.damageBonus;

                if (main_ctd.defenderDamage < gcsd.minDamage) {
                    main_ctd.defenderDamage = gcsd.minDamage;
                }

                if (main_ctd.defenderDamage > gcsd.maxDamage) {
                    main_ctd.defenderDamage = gcsd.maxDamage;
                }

                if (gcsd.field_1C != 0) 
                {
                    // FIXME: looks like a bug, two different fields are used to set
                    // one field.
                    main_ctd.defenderFlags = gcsd.field_20;
                    main_ctd.defenderFlags = gcsd.field_24;
                }
            }

            bool aiming = false;
            if (main_ctd.defenderHitLocation == (int)HitLocation.HIT_LOCATION_TORSO || main_ctd.defenderHitLocation == (int)HitLocation.HIT_LOCATION_UNCALLED) {
                if (a1 == obj_dude) {
                    intface_get_attack(ref hitMode, ref aiming);
                } else {
                    aiming = false;
                }
            } else {
                aiming = true;
            }

            int actionPoints = item_w_mp_cost(a1, main_ctd.hitMode, aiming);
            // debug_printf("sequencing attack...\n");

            if (action_attack(main_ctd) == -1) {
                return -1;
            }

            if (actionPoints > a1.data.critter.combat.ap) {
                a1.data.critter.combat.ap = 0;
            } else {
                a1.data.critter.combat.ap -= actionPoints;
            }

            if (a1 == obj_dude) {
                intface_update_move_points(a1.data.critter.combat.ap, combat_free_move);
                critter_set_who_hit_me(a1, a2);
            }

            combat_call_display = true;
            combat_cleanup_enabled = true;
            combatAIInfoSetLastTarget(a1, a2);
            // debug_printf("running attack...\n");

            return 0;
        }

        static int combatAIInfoSetLastTarget(f2Object a1, f2Object a2)
        {
            // if (!isInCombat()) {
            //     return 0;
            // }

            // if (a1 == NULL) {
            //     return -1;
            // }

            // if (a1->cid == -1) {
            //     return -1;
            // }

            // if (a1 == a2) {
            //     return -1;
            // }

            // if (critter_is_dead(a2)) {
            //     a2 = NULL;
            // }

            // aiInfoList[a1->cid].lastTarget = a2;

            return 0;
        }
    }
}
