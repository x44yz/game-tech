using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
public abstract class Consideration : ScriptableObject
{
    // public string key;
    // public AnimationCurve curve;

	public abstract float Score(IContext ctx);
    // {
    //     float t = ctx.GetConsiderationVal(key);
    //     return curve.Evaluate(t);
	// }
}
