using System;
using UnityEngine;

namespace Tutorial
{
    // [Serializable]
    public class TutorialCondition : ScriptableObject
    {
        public virtual bool IsSatisfied(TutorialTriggerType triggerType,  params object[] datas)
        {
            return false;
        }
    }
}