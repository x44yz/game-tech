using UnityEngine;

namespace AI.Utility
{
    public class AgentAI : MonoBehaviour
    {
        public float selectInterval;
        public AIConfig config;

        [Header("RUNTIME")]
        public ActionObj[] actionObjs;
        public ActionObj curActionObj;
        [SerializeField] private float tick;
        [SerializeField] private bool bInited;

        public delegate void actionChangedDelegate(ActionObj act);
        public actionChangedDelegate onActionChanged;

        public ActionObj CurActionObj => curActionObj;

        public void Init()
        {
            if (bInited)
                return;
            bInited = true;

            actionObjs = new ActionObj[config.actions.Length];
            for (int i  = 0; i < actionObjs.Length; ++i)
            {
                var actionCfg = config.actions[i];
                actionObjs[i] = (ActionObj)gameObject.AddComponent(actionCfg.ActionObjType());
                actionObjs[i].Init(actionCfg);
            }
        }

        public void Tick(IContext ctx, float dt)
        {
            if (config == null)
                return;

            if (!bInited)
            {
                Init();
            }
                
            if (curActionObj != null)
                curActionObj.Execute(ctx, dt);

            tick += dt;
            if (tick < selectInterval)
                return;

            var bestAction = Select(ctx);
            if (bestAction == curActionObj)
                return;

            if (curActionObj != null)
            {
                curActionObj.Exit(ctx);
                Debug.Log($"[UTILITY_AI]{name} exit action > {curActionObj.name}");
            }
            curActionObj = bestAction;
            if (curActionObj != null)
            {
                curActionObj.Enter(ctx);
                Debug.Log($"[UTILITY_AI]{name} enter action > {curActionObj.name}");
            }

            if (onActionChanged != null)
                onActionChanged.Invoke(curActionObj);
        }

        private ActionObj Select(IContext ctx)
        {
            if (actionObjs == null || actionObjs.Length == 0)
                return null;

            float bestScore = float.MinValue;
            ActionObj bestAction = null;
            for (int i = 0; i < actionObjs.Length; ++i)
            {
                var act = actionObjs[i];
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
