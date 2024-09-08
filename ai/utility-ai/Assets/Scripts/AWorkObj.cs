using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

public class AWorkObj : ActionObj
{
    public AWork work => action as AWork;
    public float energyDrainSpd => work.energyDrainSpd;
    public float moneyCollectSpd => work.moneyCollectSpd;
    public float hungerDrainSpd => work.hungerDrainSpd;

    public override void Enter(IContext ctx)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.IsAtPoint(PointType.OFFICE) == false)
            agent.moveToPoint = agent.GetPoint(PointType.OFFICE);
    }

    public override Status Execute(IContext ctx, float dt)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.curAtPointType != PointType.OFFICE)
            return Status.WAITING;

        agent.ModStat(Stat.ENERGY, energyDrainSpd * actx.deltaMins);
        agent.ModStat(Stat.MONEY, moneyCollectSpd * actx.deltaMins);
        agent.ModStat(Stat.HUNGER, hungerDrainSpd * actx.deltaMins);
        return Status.EXECUTING;
    }
}
