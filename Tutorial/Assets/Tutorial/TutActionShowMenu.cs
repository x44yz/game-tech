using System;
using UnityEngine;

namespace Tutorial
{
    public enum TutMenuType
    {
        UIMenuGuide = 0,
    }

    // [Serializable]
    public class TutActionShowMenu : TutorialAction
    {
        public TutMenuType menuType;
        public bool visible;

        public override void Enter()
        {
            base.Enter();
            IsDone = true;

            if (visible)
                TutorialManager.Inst.ShowTutUIController(menuType);
            else
                TutorialManager.Inst.HideTutUIController(menuType);
        }
    }
}