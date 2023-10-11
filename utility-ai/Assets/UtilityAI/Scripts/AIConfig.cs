using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Utility
{
    [CreateAssetMenu(fileName = "AIConfig", menuName = "AI/AIConfig")]
    public class AIConfig : ScriptableObject
    {
        public Action[] actions;
    }
}
