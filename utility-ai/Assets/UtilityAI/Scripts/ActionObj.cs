using UnityEngine;

namespace AI.Utility
{
    public abstract class ActionObj : MonoBehaviour
    {
        public enum Status
        {
            WAITING,
            EXECUTING,
            FINISHED,
        }

        public delegate void ScoreChangedDelegate(float v);
        public ScoreChangedDelegate onScoreChanged;

        public string dbgName => action ? action.name : name;
        public Action action { get; protected set; }
        public Precondition[] preconditions => action.preconditions;
        public Consideration[] considerations => action.considerations;
        public float curScore { get; protected set; }
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

        // public float Score(IContext ctx)
        // {
        //     if (considerations == null || considerations.Length == 0)
        //         return 1;

        //     var totalScore = 1f;

        //     var modificationFactor = 1f - 1f / considerations.Length;
        //     for (int i = 0; i < considerations.Length; i++)
        //     {
        //         var con = considerations[i];
        //         float score = con.Score(ctx);

        //         var makeUpValue = (1f - score) * modificationFactor;
        //         score += (makeUpValue * score);
        //         conScores[i] = score;

        //         totalScore *= score;
        //         if (totalScore < 0.01f)
        //             break;
        //     }

        //     CurScore = totalScore;

        //     if (onScoreChanged != null)
        //         onScoreChanged.Invoke(totalScore);
        //     return totalScore;
        // }

        public float Score(IContext ctx)
        {
            if (considerations == null || considerations.Length == 0)
                return 1;

            float score = 0.0f;
            for (int i = 0; i < considerations.Length; i++)
            {
                var con = considerations[i];
                float s = con.Score(ctx) * con.weight; // / conTotalWeight;
                // Debug.Log($"xx-- {name} - {i}/{considerations.Length}");
                conScores[i] = s;

                score += s;
            }

            // average
            score = score / considerations.Length;
            curScore = score;

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

        public float cooldownStartTS = float.MinValue;
        public bool IsInCooldown(IContext ctx)
        {
            if (action.cooldown <= 0f)
                return false;
            return ctx.GetActionCooldownTS() < cooldownStartTS + action.cooldown;
        }

        public void StartCooldown(IContext ctx)
        {
            cooldownStartTS = ctx.GetActionCooldownTS();
        }

        public virtual void Enter(IContext ctx)
        {
        }
        public virtual Status Execute(IContext ctx, float dt)
        {
            return Status.EXECUTING;
        }
        public virtual void Exit(IContext ctx)
        {
        }
    }
}