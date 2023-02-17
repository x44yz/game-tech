using System;
using UnityEngine;

namespace Tutorial
{
    public class TutActionAreaHighlight : TutorialAction
    {
        public bool visible;
        public string targetName;
        public Rect rect;
        public Vector2 anchorMin = new Vector2(0.5f, 0.5f);
        public Vector2 anchorMax = new Vector2(0.5f, 0.5f);
        public bool acceptInput = true;

        public override void Enter()
        {
            base.Enter();
            IsDone = true;

            var uiTutController = TutorialManager.Inst.GetTutUIController();
            if (uiTutController == null)
            {
                Debug.LogError("TutActionAreaHighlight failed because cant find any UITutorialController");
                return;
            }
            if (visible)
                uiTutController.ShowHighlightArea(rect, anchorMin, anchorMax, acceptInput, targetName);
            else
                uiTutController.HideHighlightArea();
        }
    }
}