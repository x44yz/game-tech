using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/C/CTime")]
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
