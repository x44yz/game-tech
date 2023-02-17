using System;
using UnityEngine;

namespace Tutorial
{
    [Serializable]
    public class TutorialAction : ScriptableObject
    {
        public bool IsDone { get; set; }
#if UNITY_EDITOR
        public string desc;
#endif

        public virtual void Enter()
        {
            IsDone = false;
        }

        public virtual void Execute(float dt)
        {

        }

        public virtual void Exit()
        {

        }
    }
}