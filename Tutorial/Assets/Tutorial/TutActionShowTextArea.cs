using System;
using UnityEngine;

namespace Tutorial
{
    // [Serializable]
    public class TutActionShowTextArea : TutorialAction
    {
        public bool visible;
        public string title;
        public string text;

        public override void Enter()
        {
            base.Enter();

            var uiTutController = TutorialManager.Inst.GetTutUIController();
            if (uiTutController == null)
            {
                Debug.LogError("TutActionShowTextArea failed because cant find any UITutorialController");
                return;
            }
            Debug.Log($"[TUT]TutActionShowTextArea > {visible}");
            if (visible)
            {
                uiTutController.ShowTextArea(title, text, ()=>{
                    IsDone = true;
                });
            }
            else {
                IsDone = true;
                uiTutController.HideTextArea();
            }
        }
    }
}