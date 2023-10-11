using UnityEngine;

namespace AI.Utility
{
    public abstract class ActionObj : MonoBehaviour
    {
        public delegate void scoreChangedDelegate(float v);
        public scoreChangedDelegate onScoreChanged;

        public Action action { get; protected set; }
        public Precondition[] preconditions => action.preconditions;
        public Consideration[] considerations => action.considerations;
        public float CurScore { get; protected set; }
        private float[] conScores = null;
        private float conTotalWeight;

        public virtual void Init(Action action)
        {
            this.action = action;

            conScores = new float[considerations.Length];
            conTotalWeight = 0f;
            foreach (var con in considerations)
            {
                conTotalWeight += con.weight;
            }
        }

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
                var con = considerations[i];
                float s = con.Score(ctx) * con.weight / conTotalWeight;
                // Debug.Log($"xx-- {name} - {i}/{considerations.Length}");
                conScores[i] = s;

                score += s;
            }

            // average
            score = score / considerations.Length;
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
        public virtual void Execute(IContext ctx, float dt)
        {
        }
        public virtual void Exit(IContext ctx)
        {
        }
    }
}