using UnityEngine;

// namespace AI.Utility
// {
    public class Action : ScriptableObject
    {
        public Consideration[] considerations;
        public float weight = 1f; // 权重

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

        public virtual void Execute()
        {
        }
    }
// }