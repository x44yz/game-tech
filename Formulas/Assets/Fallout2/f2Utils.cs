using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace f2
{
    public static class f2Utils
    {
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

