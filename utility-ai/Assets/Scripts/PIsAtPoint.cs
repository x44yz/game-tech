using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/P/PIsAtPoint")]
public class PIsAtPoint : Precondition
{
    public float dist;

    // public override float Score(IContext ctx)
    // {
    //     var agentCtxt = ctx as AgentContext;
    //     if (agentCtxt.moveTarget == null)
    //         return 0f;
    //     var v = Vector3.Distance(agentCtxt.pos, agentCtxt.moveTarget.Value);
    //     if (v <= dist)
    //         return 1f;
    //     return 0f;
    // }

    public override bool IsTrue(IContext ctx)
    {
        return false;
        // var agentCtxt = ctx as AgentContext;
        // if (agentCtxt.moveTarget == null)
        //     return false;
        // var v = Vector3.Distance(agentCtxt.pos, agentCtxt.moveTarget.Value);
        // if (v <= dist)
        //     return true;
        // return false;
    }
}
