using UnityEngine;

// namespace AI.Utility
// {
public abstract class Action : ScriptableObject
{
    public Precondition[] preconditions;
    public Consideration[] considerations;
    public float weight = 1f; // 权重

    public delegate void FloatDelegate(float v);
    public FloatDelegate onScoreChanged;

    public float CurScore { get; protected set; }
    private float[] conScores = null;

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

        if (conScores == null || conScores.Length == 0)
            conScores = new float[considerations.Length];

        float score = 0.0f;
        for (int i = 0; i < considerations.Length; i++)
        {
            float s = considerations[i].Score(ctx);
            // Debug.Log($"xx-- {name} - {i}/{considerations.Length}");
            conScores[i] = s;

            score += s;
        }

        // 平均
        score = score / considerations.Length * weight;
        CurScore = score;

        if (onScoreChanged != null)
            onScoreChanged.Invoke(score);
        return score;
    }

    public float GetConsiderationScore(int idx)
    {
        if (conScores == null || idx < 0 || idx >= conScores.Length)
            return 0f;
        return conScores[idx];
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