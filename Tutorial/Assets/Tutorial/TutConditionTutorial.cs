using System;
using UnityEngine;

namespace Tutorial
{
    public class TutConditionTutorial : TutorialCondition
    {
        public int tutorialId;

        public override bool IsSatisfied(TutorialTriggerType triggerType,  params object[] datas)
        {
            if (triggerType != TutorialTriggerType.Tutorial)
                return false;

            int tid = (int)datas[0];
            if (tid != tutorialId)
                return false;

            return true;
        }
    }
}