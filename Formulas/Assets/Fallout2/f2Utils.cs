using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public static class f2Utils
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
    }
}

