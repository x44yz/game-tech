using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

[CreateAssetMenu(fileName = "CTime", menuName = "AI/C/CTime")]
public class CTime : Consideration
{
    public AnimationCurve curve;

    public override float Score(IContext ctx)
    {
        AgentContext actx = ctx as AgentContext;
        float t = actx.GetCurTimeNOR();
        return curve.Evaluate(t);
    }
}
