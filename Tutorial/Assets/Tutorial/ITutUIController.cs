using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public interface ITutUIController
    {
        void Show();
        void Hide();
        bool IsAcceptClickEnable {get;}
        void ShowTextArea(string title, string text, System.Action typeEndCallback);
        void HideTextArea();
        void ShowClickArrow(TutActionShowClickArrow.ArrowType arrowType, 
                            Vector2 pos, 
                            Vector2 anchorMin, 
                            Vector2 anchorMax, 
                            Vector2 scale,
                            string targetName);
        void HideClickArrow();
        void ShowHighlightArea(Rect rect, 
                            Vector2 anchorMin, 
                            Vector2 anchorMax, 
                            bool acceptInput, 
                            string targetName);
        void HideHighlightArea();
        void SetHighlightAreaAcceptClick(bool enable, System.Action clickCallback);
    }
}
