using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public enum TutorialTriggerType
    {
        OpenMenu,
        Task,
        Tutorial,
    }

    // [Serializable] 
    [CreateAssetMenu(fileName = "TUT_ID_NAME", menuName = "Tutorial/TUT_ID_NAME", order = 1)]
    public class Tutorial : ScriptableObject
    {
        // only need Serializable variables to be public
        public int id;
        public List<TutorialCondition> conditions = new List<TutorialCondition>();
        public List<TutorialAction> actions = new List<TutorialAction>();

        private int curActionIdx = 0;
        private TutorialAction curTutorialAction = null;
        public bool IsDone { get; set; }

        public bool CheckConditions(TutorialTriggerType triggerType, params object[] datas)
        {
            if (conditions == null || conditions.Count == 0)
                return false;

            for (int i = 0; i < conditions.Count; ++i)
            {
                var cond = conditions[i];

                if (!cond.IsSatisfied(triggerType, datas))
                {
                    return false;
                }
            }

            return true;
        }

        public void Start()
        {
            IsDone = false;
            curActionIdx = 0;
            DoTutorialAction(curActionIdx);
        }

        public void Stop()
        {
            IsDone = true;
        }

        private void DoTutorialAction(int actionIdx)
        {
            if (actionIdx < 0 || actionIdx >= actions.Count)
            {
                Stop();
                return;
            }

            curTutorialAction = actions[actionIdx];
#if UNITY_EDITOR
            Debug.Log($"[TUT]DoTutorialAction > {actionIdx} - {curTutorialAction} - {curTutorialAction.desc}");
#endif
            curTutorialAction.Enter();
            // make no-delay action translation dont wait one frame
            if (curTutorialAction.IsDone)
                MoveToNextTutorialAction();
        }

        public void MoveToNextTutorialAction()
        {
            if (curTutorialAction != null)
                curTutorialAction.Exit();

            curActionIdx += 1;
#if UNITY_EDITOR
            Debug.Log($"[TUT]MoveToNextTutorialAction > {curActionIdx}");
#endif
            DoTutorialAction(curActionIdx);
        }

        public void Tick(float dt)
        {
            if (curTutorialAction != null)
            {
                if (curTutorialAction.IsDone == false)
                    curTutorialAction.Execute(Time.deltaTime);
                else
                    MoveToNextTutorialAction();
            }
        }

#if UNITY_EDITOR
        public void PlayFromEditor()
        {
            TutorialManager.Inst.StartTutorial(this);
        }
#endif
    }
}
