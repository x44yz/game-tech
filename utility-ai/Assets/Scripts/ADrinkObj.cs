using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

public class ADrinkObj : ActionObj
{
    public ADrink drink => action as ADrink;

    public float tick = 0f;

    public override void Enter(IContext ctx)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.IsAtPoint(PointType.PUB) == false)
            agent.moveToPoint = agent.GetPoint(PointType.PUB);

        tick = 0f;
    }

    public override Status Execute(IContext ctx, float dt)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.curAtPointType != PointType.PUB)
            return Status.WAITING;

        tick += actx.deltaMins;
        if (tick >= drink.minutes)
        {
            agent.ModStat(Stat.HUNGER, eat.hungerRecover);
            agent.ModStat(Stat.MONEY, eat.moneyCost);
            return Status.FINISHED;
        }

        return Status.EXECUTING;
    }
}
