using UnityEngine;

namespace AI.Utility
{
    public abstract class Consideration : ScriptableObject
    {
        public float weight = 1f;

        public abstract float Score(IContext ctx);
    }
}
