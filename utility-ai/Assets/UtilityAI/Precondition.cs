using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "Precondition", menuName = "utility-ai/Precondition", order = 0)]
public abstract class Precondition : ScriptableObject
{
    public abstract bool IsTrue(IContext ctx);
}
