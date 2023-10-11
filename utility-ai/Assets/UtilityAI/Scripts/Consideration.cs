using UnityEngine;

namespace AI.Utility
{
    public abstract class Consideration : ScriptableObject
    {
        public abstract float Score(IContext ctx);
    }
}
