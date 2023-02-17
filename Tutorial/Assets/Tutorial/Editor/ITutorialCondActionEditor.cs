using UnityEngine;

namespace Tutorial
{
    public interface ITutorialCondActionEditor 
    {
        UnityEngine.Object GetTarget();
        float GetInspectorGUIHeight();
        void DrawInspectorGUI(Rect rect);
    }
}