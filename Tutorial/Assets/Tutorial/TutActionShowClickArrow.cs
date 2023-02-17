using System;
using UnityEngine;

namespace Tutorial
{
    // [Serializable]
    public class TutActionShowClickArrow : TutorialAction
    {
        public enum ArrowType
        {
            ArrowUp = 0,
            ArrowDown = 1,
            ArrowLeft = 2,
            ArrowRight = 3,
        }

        public bool visible;
        public string targetName;
        public ArrowType arrowType;
        public Vector2 position;
        public Vector2 anchorMin = new Vector2(0.5f, 0.5f);
        public Vector2 anchorMax = new Vector2(0.5f, 0.5f);
        public Vector2 scale = Vector2.one;

        public override void Enter()
        {
            base.Enter();
            IsDone = true;

            var uiTutController = TutorialManager.Inst.GetTutUIController();
            if (uiTutController == null)
            {
                Debug.LogError("TutActionShowClickArrow failed because cant find any UITutorialController");
                return;
            }
            if (visible)
                uiTutController.ShowClickArrow(arrowType, position, anchorMin, anchorMax, scale, targetName);
            else
                uiTutController.HideClickArrow();
        }
    }
}