using UnityEngine;

namespace AI.Utility
{
    public class AgentAI : MonoBehaviour
    {
        public float selectInterval;
        public AIConfig config;

        [Header("RUNTIME")]
        [SerializeField] private Action curAction;
        [SerializeField] private float tick;

        public delegate void actionChangedDelegate(Action act);
        public actionChangedDelegate onActionChanged;

        public Action CurAction => curAction;

        public void Tick(IContext ctx, float dt)
        {
            if (config == null)
                return;
                
            if (curAction != null)
                curAction.Execute(ctx, dt);

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

            if (onActionChanged != null)
                onActionChanged.Invoke(curAction);
        }

        private Action Select(IContext ctx)
        {
            if (config == null || config.actions == null || 
                config.actions.Length == 0)
                return null;

            float bestScore = float.MinValue;
            Action bestAction = null;
            for (int i = 0; i < config.actions.Length; ++i)
            {
                var act = config.actions[i];
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
}
