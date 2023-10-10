using UnityEngine;

namespace AI.Utility
{
    public abstract class Precondition : ScriptableObject
    {
        public abstract bool IsTrue(IContext ctx);
    }
}
