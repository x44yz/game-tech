using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

public class AEatObj : ActionObj
{
    public AEat eat => action as AEat;

    public float tick = 0f;

    public override void Enter(IContext ctx)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.IsAtPoint(eat.pointType) == false)
            agent.moveToPoint = agent.GetPoint(eat.pointType);

        tick = 0f;
    }

    public override Status Execute(IContext ctx, float dt)
    {
        var actx = ctx as AgentContext;
        var agent = actx.agent;
        if (agent.curAtPointType != eat.pointType)
            return Status.WAITING;

        tick += actx.deltaMins;
        if (tick >= eat.minutes)
        {
            agent.ModStat(Stat.HUNGER, eat.hungerRecover);
            agent.ModStat(Stat.MONEY, eat.moneyCost);
            return Status.FINISHED;
        }

        return Status.EXECUTING;
    }
}
