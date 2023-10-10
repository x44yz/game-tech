using UnityEngine;

// namespace AI.Utility
// {
public abstract class Action : ScriptableObject
{
    public Precondition[] preconditions;
    public Consideration[] considerations;
    public float weight = 1f; // 权重

    public bool Evaluate(IContext ctx)
    {
        if (preconditions == null || preconditions.Length == 0)
            return true;

        for (int i = 0; i < preconditions.Length; ++i)
        {
            if (preconditions[i].IsTrue(ctx) == false)
                return false;
        }
        return true;
    }

    public float Score(IContext ctx)
    {
        if (considerations == null || considerations.Length == 0)
            return 1;

        float score = 0.0f;
        for (int i = 0; i < considerations.Length; i++)
        {
            score += considerations[i].Score(ctx);
        }

        // 平均
        score = score / considerations.Length * weight;
        return score;
    }

    public virtual void Enter(IContext ctx)
    {
    }
    public virtual void Execute(IContext ctx)
    {
    }
    public virtual void Exit(IContext ctx)
    {
    }
}
// }