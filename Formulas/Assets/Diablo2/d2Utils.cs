using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace d2
{
    public static class d2Utils
    {
        public static int RndPL(int param1, int param2)
        {
            return param1 + GenerateRnd(param2 - param1 + 1);
        }

        public static bool FlipCoin(int frequency = 1)
        {
            // Casting here because GenerateRnd takes a signed argument when it should take and yield unsigned.
            return GenerateRnd(frequency) == 0;
        }

        public static T PickRandomlyAmong<T>(Array values)
        {
            var index = d2Utils.GenerateRnd(values.Length);

            return (T)values.GetValue(index);
        }

        public static T PickRandomlyAmong<T>(List<T> values)
        {
            var index = d2Utils.GenerateRnd(values.Count);

            return values[index];
        }

        public static void SetRndSeed(int seed)
        {
            // sglGameSeed = seed;
        }

        // public static int ToInt(this System.Enum e)
        // {
        //     return (int)(object)e;
        // }
        public static int AdvanceRndSeed()
        {
            // sglGameSeed = (RndMult * sglGameSeed) + RndInc;
            // return GetRndSeed();
            return (int)Time.timeSinceLevelLoad;
        }

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

        public static Direction GetDirection(Vector2Int start, Vector2Int destination)
        {
            Direction md;

            int mx = destination.x - start.x;
            int my = destination.y - start.y;
            if (mx >= 0) {
                if (my >= 0) {
                    if (5 * mx <= (my * 2)) // mx/my <= 0.4, approximation of tan(22.5)
                        return Direction.SouthWest;
                    md = Direction.South;
                } else {
                    my = -my;
                    if (5 * mx <= (my * 2))
                        return Direction.NorthEast;
                    md = Direction.East;
                }
                if (5 * my <= (mx * 2)) // my/mx <= 0.4
                    md = Direction.SouthEast;
            } else {
                mx = -mx;
                if (my >= 0) {
                    if (5 * mx <= (my * 2))
                        return Direction.SouthWest;
                    md = Direction.West;
                } else {
                    my = -my;
                    if (5 * mx <= (my * 2))
                        return Direction.NorthEast;
                    md = Direction.North;
                }
                if (5 * my <= (mx * 2))
                    md = Direction.NorthWest;
            }
            return md;
        }
    }
}

