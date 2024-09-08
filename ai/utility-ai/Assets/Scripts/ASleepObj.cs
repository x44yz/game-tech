using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

public class ASleepObj : ActionObj
{
    public ASleep sleep => action as ASleep;
    public float energyRecoverSpd => sleep.energyRecoverSpd;
    public float hungerDrainSpd => sleep.hungerDrainSpd;

    public override void Enter(IContext ctx)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.IsAtPoint(PointType.HOME) == false)
            agent.moveToPoint = agent.GetPoint(PointType.HOME);
    }

    public override Status Execute(IContext ctx, float dt)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.curAtPointType != PointType.HOME)
            return Status.WAITING;

        agent.ModStat(Stat.ENERGY, energyRecoverSpd * actx.deltaMins);
        agent.ModStat(Stat.HUNGER, hungerDrainSpd * actx.deltaMins);
        return Status.EXECUTING;
    }
}
