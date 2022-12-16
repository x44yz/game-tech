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

        public static bool IsAnyOf(Enum v, params Enum[] xs)
        {
            int[] ixs = new int[xs.Length];
            for (int i = 0; i < xs.Length; ++i)
            {
                ixs[i] = (int)(object)xs[i];
            }
            return IsAnyOf((int)(object)v, ixs);
        }

        public static bool IsAnyOf(int v, int x)
        {
            return v == x;
        }

        public static bool IsAnyOf(int v, params int[] xs)
        {
            if (xs != null)
            {
                bool ret = false;
                for (int i = 0; i < xs.Length; ++i)
                {
                    ret = ret || IsAnyOf(v, xs[i]);
                    if (ret)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsNoneOf(Enum v, params Enum[] xs)
        {
            int[] ixs = new int[xs.Length];
            for (int i = 0; i < xs.Length; ++i)
            {
                ixs[i] = (int)(object)xs[i];
            }
            return IsNoneOf((int)(object)v, ixs);
        }

        public static bool IsNoneOf(int v, int x)
        {
            return v != x;
        }

        public static bool IsNoneOf(int v, params int[] xs)
        {
            if (xs != null)
            {
                bool ret = true;
                for (int i = 0; i < xs.Length; ++i)
                {
                    ret = ret && IsNoneOf(v, xs[i]);
                    if (ret == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

