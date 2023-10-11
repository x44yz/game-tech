using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

[CreateAssetMenu(fileName = "CStat", menuName = "AI/C/CStat")]
public class CStat : Consideration
{
    public Stat stat;
    public AnimationCurve curve;

    public override float Score(IContext ctx)
    {
        AgentContext actx = ctx as AgentContext;
        float t = actx.GetStatNOR(stat);
        return curve.Evaluate(t);
    }
}
