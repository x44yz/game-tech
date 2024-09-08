using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public partial class f2Game
    {
        public static int FID_TYPE(int value) => ((value) & 0xF000000) >> 24;
        public static int PID_TYPE(int value) => (value) >> 24;
        public static int SID_TYPE(int value) => (value) >> 24;

        public static int proto_ptr(int pid, ref Proto protoPtr)
        {
            // TODO
            protoPtr = null;
            return 0;
            // *protoPtr = NULL;

            // if (pid == -1) {
            //     return -1;
            // }

            // if (pid == 0x1000000) {
            //     *protoPtr = (Proto*)&gDudeProto;
            //     return 0;
            // }

            // ProtoList* protoList = &(_protoLists[PID_TYPE(pid)]);
            // ProtoListExtent* protoListExtent = protoList->head;
            // while (protoListExtent != NULL) {
            //     for (int index = 0; index < protoListExtent->length; index++) {
            //         Proto* proto = (Proto*)protoListExtent->proto[index];
            //         if (pid == proto->pid) {
            //             *protoPtr = proto;
            //             return 0;
            //         }
            //     }
            //     protoListExtent = protoListExtent->next;
            // }

            // if (protoList->head != NULL && protoList->tail != NULL) {
            //     if (PROTO_LIST_EXTENT_SIZE * protoList->length - (PROTO_LIST_EXTENT_SIZE - protoList->tail->length) > PROTO_LIST_MAX_ENTRIES) {
            //         _proto_remove_some_list(PID_TYPE(pid));
            //     }
            // }

            // return _proto_load_pid(pid, protoPtr);
        }

        public static int roll_random(int min, int max)
        {
            int result = UnityEngine.Random.Range(min, max);

            // if (min <= max) {
            //     result = min + ran1(max - min + 1);
            // } else {
            //     result = max + ran1(min - max + 1);
            // }

            // if (result < min || result > max) {
            //     debug_printf("Random number %d is not in range %d to %d", result, min, max);
            //     result = min;
            // }

            return result;
        }

        // Rolls d% against [difficulty].
        static int roll_check(int difficulty, int criticalSuccessModifier)
        {
            int howMuch = 0;
            return roll_check(difficulty, criticalSuccessModifier, ref howMuch);
        }

        static int roll_check(int difficulty, int criticalSuccessModifier, ref int howMuchPtr)
        {
            // difficulty = 20，那么就有 20% 的概率为 success，再加上 10% 的概率暴击
            int delta = difficulty - roll_random(1, 100);
            int result = roll_check_critical(delta, criticalSuccessModifier);

            // if (howMuchPtr != NULL) {
                howMuchPtr = delta;
            // }

            return result;
        }

        // Translates raw d% result into [Roll] constants, possibly upgrading to
        // criticals (starting from day 2).
        static int roll_check_critical(int delta, int criticalSuccessModifier)
        {
            int gameTime = game_time();

            int roll;
            if (delta < 0) {
                roll = (int)Roll.ROLL_FAILURE;

                if ((gameTime / GAME_TIME_TICKS_PER_DAY) >= 1) {
                    // 10% to become critical failure.
                    if (roll_random(1, 100) <= -delta / 10) {
                        roll = (int)Roll.ROLL_CRITICAL_FAILURE;
                    }
                }
            } else {
                roll = (int)Roll.ROLL_SUCCESS;

                if ((gameTime / GAME_TIME_TICKS_PER_DAY) >= 1) {
                    // 10% + modifier to become critical success.
                    if (roll_random(1, 100) <= delta / 10 + criticalSuccessModifier) {
                        roll = (int)Roll.ROLL_CRITICAL_SUCCESS;
                    }
                }
            }

            return roll;
        }

        // Returns game time in ticks (1/10 second).
        static int game_time()
        {
            // return fallout_game_time;
            return 0;
        }
    }
}

