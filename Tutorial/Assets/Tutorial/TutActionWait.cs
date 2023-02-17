using System;
using UnityEngine;

namespace Tutorial
{
    // [Serializable]
    public class TutActionWait : TutorialAction
    {
        // public float delay;
        public float duration = 2f; // seconds

        private float tick = 0f;

        public override void Enter()
        {
            base.Enter();
            tick = 0f;
        }

        public override void Execute(float dt)
        {
            // Debugger.Log("xx-- dt > " + dt + " - " + tick);

            tick += dt;
            if (tick >= duration)
                IsDone = true;
        }
    }
}