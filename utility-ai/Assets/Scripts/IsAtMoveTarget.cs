using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/IsAtMoveTarget")]
public class IsAtMoveTarget : Consideration
{
    public float dist;

    public override float Score(IContext ctx)
    {
        var agentCtxt = ctx as AgentContext;
        if (agentCtxt.moveTarget == null)
            return 0f;
        var v = Vector3.Distance(agentCtxt.pos, agentCtxt.moveTarget.Value);
        if (v <= dist)
            return 1f;
        return 0f;
    }
}
