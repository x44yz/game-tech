using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// namespace AI.Utility
// {
[CreateAssetMenu(menuName = "AI/UtilityAI")]
public class UtilityAI : ScriptableObject
{
    public float selectInterval;
    public Action[] actions;

    [SerializeField] private Action curAction;
    [SerializeField] private float tick;

    public Action CurAction => curAction;

    public void Tick(IContext ctx, float dt)
    {
        if (curAction != null)
            curAction.Execute(ctx);

        tick += dt;
        if (tick < selectInterval)
            return;

        var bestAction = Select(ctx);
        if (bestAction == curAction)
            return;

        if (curAction != null)
            curAction.Exit(ctx);
        curAction = bestAction;
        if (curAction != null)
            curAction.Enter(ctx);
    }

    private Action Select(IContext ctx)
    {
        if (actions == null || actions.Length == 0)
            return null;

        float bestScore = float.MinValue;
        Action bestAction = null;
        for (int i = 0; i < actions.Length; ++i)
        {
            var act = actions[i];
            if (act.Evaluate(ctx) == false)
                continue;

            float score = act.Score(ctx);
            if (score > bestScore)
            {
                bestScore = score;
                bestAction = act;
            }
        }

        return bestAction;
    }
}
// }
