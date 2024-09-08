using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    public static class AILogger
    {
        public const string PREFIX = "[UTILITY_AI]";

        public static void Log(string msg)
        {
            Debug.Log($"{PREFIX} {msg}");
        }

        public static void LogWarning(string msg)
        {
            Debug.LogWarning($"{PREFIX} {msg}");
        }

        public static void LogError(string msg)
        {
            Debug.LogError($"{PREFIX} {msg}");
        }
    }
}