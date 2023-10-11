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
        if (agent.IsAtPoint(PointType.HOME) == false)
            agent.moveToPoint = agent.GetPoint(PointType.HOME);
        agent.ModStat(Stat.MONEY, moneyCost);
    }

    public override void Execute(IContext ctx, float dt)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.curAtPointType != PointType.HOME)
            return;
        agent.ModStat(Stat.HUNGER, hungerRecoverSpd * actx.deltaSecs);
    }
}
