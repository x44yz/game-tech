using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

public class AEatObj : ActionObj
{
    public AEat eat => action as AEat;
    public float hungerRecoverSpd => eat.hungerRecoverSpd;
    public float moneyCost => eat.moneyCost;

    public override void Enter(IContext ctx)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.IsAtPoint(eat.pointType) == false)
            agent.moveToPoint = agent.GetPoint(eat.pointType);
        agent.ModStat(Stat.MONEY, moneyCost);
    }

    public override void Execute(IContext ctx, float dt)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.curAtPointType != eat.pointType)
            return;
        agent.ModStat(Stat.HUNGER, hungerRecoverSpd * actx.deltaSecs);
    }
}
