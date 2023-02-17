using System;
using UnityEngine;

namespace Tutorial
{
    public class TutActionAcceptClick : TutorialAction
    {
        public enum ClickType
        {
            AnyClick = 0,
            HighlightArea = 1,
            HudHighlight = 2,
        }

        public ClickType clickType;

        public override void Execute(float dt)
        {
            base.Execute(dt);

            var uiTutController = TutorialManager.Inst.GetTutUIController();
            if (uiTutController != null && !uiTutController.IsAcceptClickEnable)
                return;

            if (clickType == ClickType.AnyClick)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    IsDone = true;
                }
            }
            else if (clickType == ClickType.HighlightArea)
            {
                if (uiTutController == null)
                {
                    Debug.LogError("TutActionAcceptClick failed because cant find any UITutorialController");
                    IsDone = true;
                    return;
                }
                uiTutController.SetHighlightAreaAcceptClick(true, ()=>{
                    IsDone = true;
                });
            }
        }
    }
}