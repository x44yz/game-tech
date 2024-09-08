using UnityEngine;

namespace AI.Utility
{
    public class AgentAI : MonoBehaviour
    {
        // public float selectInterval;
        public AIConfig config;
        public bool showLog;

        [Header("RUNTIME")]
        public ActionObj[] actionObjs;
        public ActionObj curActionObj;
        [SerializeField] private float tick;
        [SerializeField] private bool inited;

        public delegate void ActionChangedDelegate(ActionObj act);
        public ActionChangedDelegate onActionChanged;

        public void Init()
        {
            if (inited)
                return;
            inited = true;

            actionObjs = new ActionObj[config.actions.Length];
            for (int i  = 0; i < actionObjs.Length; ++i)
            {
                var actionCfg = config.actions[i];
                var actionObj = (ActionObj)gameObject.AddComponent(actionCfg.ActionObjType());
                if (actionObj == null)
                {
                    AILogger.LogError($"{config.name} action {i} cant create obj");
                    continue;
                }
                actionObjs[i] = actionObj;
                actionObj.Init(actionCfg);
            }
        }

        public void Tick(IContext ctx, float dt)
        {
            if (config == null)
                return;

            if (!inited)
            {
                Init();
            }
                
            if (curActionObj != null)
            {
                var status = curActionObj.Execute(ctx, dt);
                if (status == ActionObj.Status.FINISHED)
                {
                    curActionObj.StartCooldown(ctx);
                    curActionObj.Exit(ctx);
                    AILogger.Log($"{name} exit action > {curActionObj.dbgName}");
                    curActionObj = null;
                }
            }

            // tick += dt;
            // if (tick < selectInterval)
            //     return;
            // tick -= selectInterval;

            var bestAction = Select(ctx);
            if (bestAction == curActionObj)
                return;

            if (curActionObj != null)
            {
                curActionObj.StartCooldown(ctx);
                curActionObj.Exit(ctx);
                AILogger.Log($"{name} exit action > {curActionObj.dbgName}");
            }
            curActionObj = bestAction;
            if (curActionObj != null)
            {
                curActionObj.Enter(ctx);
                AILogger.Log($"{name} enter action > {curActionObj.dbgName}");
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
                if (act == null)
                    continue;

                if (act.IsInCooldown(ctx))
                    continue;

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
