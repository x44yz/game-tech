using System;
using UnityEngine;

namespace Tutorial
{
    public class TutActionShowUIItem : TutorialAction
    {
        public bool visible;
        public string[] targetNames;

        public override void Enter()
        {
            base.Enter();
            IsDone = true;

            if (targetNames == null || targetNames.Length == 0)
                return;
            
            for (int i = 0; i < targetNames.Length; ++i)
            {
                var hud = TutorialUtils.GetTarget(targetNames[i]);
                if (hud == null)
                    continue;
                hud.gameObject.SetActive(visible);
            }
        }
    }
}