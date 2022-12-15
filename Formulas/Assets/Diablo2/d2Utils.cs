using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public static class d2Utils
    {
        // public static int ToInt(this System.Enum e)
        // {
        //     return (int)(object)e;
        // }

        public static int GenerateRnd(int maxExclusive) 
        {
            return UnityEngine.Random.Range(0, maxExclusive);
        }

        public static bool HasAllOf(Enum lhs, Enum test)
        {
            return HasAllOf((int)(object)lhs, (int)(object)test);
        }

        public static bool HasAllOf(int lhs, int test)
        {
            return (lhs & test) == test;
        }

        public static bool HasAnyOf(Enum lhs, Enum test)
        {
            return HasAnyOf((int)(object)lhs, (int)(object)test);
        }

        public static bool HasAnyOf(int lhs, int test)
        {
            return (lhs & test) != 0;
        }

        public static bool HasNoneOf(Enum lhs, Enum test)
        {
            return !HasAnyOf((int)(object)lhs, (int)(object)test);
        }
    }
}

