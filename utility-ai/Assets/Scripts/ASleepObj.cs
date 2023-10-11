using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

public class ASleepObj : ActionObj
{
    public ASleep sleep => action as ASleep;
    public float energyRecoverSpd => sleep.energyRecoverSpd;

    public override void Enter(IContext ctx)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.IsAtPoint(PointType.HOME) == false)
            agent.moveToPoint = agent.GetPoint(PointType.HOME);
    }

    public override void Execute(IContext ctx, float dt)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.curAtPointType != PointType.HOME)
            return;
        agent.ModStat(Stat.ENERGY, energyRecoverSpd * actx.deltaSecs);
    }
}
