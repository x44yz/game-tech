using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tutorial
{
    [CustomEditor(typeof(TutorialAction), true)]
    public class TutorialActionEditor : Editor, ITutorialCondActionEditor
    {
        public static readonly float DEFAULT_LINE_HEIGHT = 21;

        public Tutorial tutorialCfg;
        private bool showAction = false;
        private float lineHeight = 21;
        private float guiHeight = 41;

        private void OnEnable()
        {
            lineHeight = EditorGUIUtility.singleLineHeight;
        }

        public UnityEngine.Object GetTarget()
        {
            return target;
        }

        public float GetInspectorGUIHeight()
        {
            return guiHeight;
        }

        // dont use guilayout
        public void DrawInspectorGUI(Rect rect)
        {
            serializedObject.Update();

            var tutAction = target as TutorialAction;

            Vector2 marginTL = new Vector2(rect.x, rect.y);
            float guiWidth = rect.width;

            string title = GetActionName();
            if (string.IsNullOrEmpty(tutAction.desc) == false)
                title += " - " + tutAction.desc;

            showAction = EditorGUI.BeginFoldoutHeaderGroup(new Rect(marginTL.x + 14, marginTL.y, guiWidth - 14, lineHeight), showAction, title);
            marginTL.y += lineHeight;

            EditorGUI.indentLevel++;
            {
                if (showAction)
                {
                    EditorGUI.PropertyField(new Rect(marginTL.x, marginTL.y, guiWidth, lineHeight), serializedObject.FindProperty("desc"));
                    marginTL.y += lineHeight;

                    Rect subRect = new Rect(marginTL.x, marginTL.y, guiWidth, lineHeight);
                    var tp = target.GetType();
                    if (tp == typeof(TutActionWait))
                        DrawTutActionWait(ref subRect);
                    else if (tp == typeof(TutActionAreaHighlight))
                        DrawTutActionAreaHighlight(ref subRect);
                    else if (tp == typeof(TutActionAcceptClick))
                        DrawTutActionAcceptClick(ref subRect);
                    else if (tp == typeof(TutActionShowMenu))
                        DrawTutActionShowMenu(ref subRect);
                    else if (tp == typeof(TutActionShowTextArea))
                        DrawTutActionShowTextArea(ref subRect);
                    else if (tp == typeof(TutActionShowClickArrow))
                        DrawTutActionShowClickArrow(ref subRect);
                    else if (tp == typeof(TutActionShowUIItem))
                        DrawTutActionShowUIItem(ref subRect);

                    marginTL.y = subRect.y;
                }
            }
            EditorGUI.indentLevel--;
            EditorGUI.EndFoldoutHeaderGroup();

            guiHeight = marginTL.y - rect.y;
            serializedObject.ApplyModifiedProperties();
        }

        private string GetActionName()
        {
            string typeName = target.GetType().ToString();
            return ObjectNames.NicifyVariableName(typeName.Replace("Game.Tut", ""));
        }

        private void DrawTutActionWait(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("duration")); rect.y += lineHeight;
        }

        private void DrawTutActionAreaHighlight(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("visible")); rect.y += lineHeight;

            DragAndDropAreaGUI(ref rect, 0, 2, (obj) =>
            {
                var ctrl = FindUIBaseControllerInParent(obj);
                if (ctrl == null)
                {
                    Debug.LogError("failed drop gameobject because cant find UIBaseController in parent @Programmer");
                }
                else
                {
                    var rpath = obj.transform.FindParentRelativePath(ctrl.name, true);
                    if (string.IsNullOrEmpty(rpath))
                        Debug.LogError($"failed drop gameobject because cant find {ctrl.name} in parent");
                    else
                    {
                        serializedObject.FindProperty("targetName").stringValue = rpath;
                        var rt = obj.GetComponent<RectTransform>();
                        Rect trect = Rect.zero;
                        Vector2 tanchor = Vector2.zero;
                        QuickGetTargetRectAnchor(rt, ref trect, ref tanchor);
                        serializedObject.FindProperty("rect").rectValue = trect;
                        serializedObject.FindProperty("anchorMin").vector2Value = tanchor;
                        serializedObject.FindProperty("anchorMax").vector2Value = tanchor;
                    }
                }
            });

            DragAndDropAreaGUI(ref rect, 1, 2, (obj) =>
            {
                var ctrl = FindUIBaseControllerInParent(obj);
                if (ctrl == null)
                {
                    Debug.LogError("failed drop gameobject because cant find UIBaseController in parent @Programmer");
                }
                else
                {
                    var rpath = obj.transform.FindParentRelativePath(ctrl.name, true);
                    if (string.IsNullOrEmpty(rpath))
                        Debug.LogError($"failed drop gameobject because cant find {ctrl.name} in parent");
                    else
                    {
                        serializedObject.FindProperty("targetName").stringValue = "";
                        var rt = obj.GetComponent<RectTransform>();
                        var trect = new Rect(rt.anchoredPosition.x, rt.anchoredPosition.y, rt.sizeDelta.x, rt.sizeDelta.y);
                        var tanchorMin = rt.anchorMin;
                        var tanchorMax = rt.anchorMax;

                        serializedObject.FindProperty("rect").rectValue = trect;
                        serializedObject.FindProperty("anchorMin").vector2Value = tanchorMin;
                        serializedObject.FindProperty("anchorMax").vector2Value = tanchorMax;
                    }
                }
            });

            EditorGUI.PropertyField(rect, serializedObject.FindProperty("targetName")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("rect")); rect.y += lineHeight * 2;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("anchorMin")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("anchorMax")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("acceptInput")); rect.y += lineHeight;
        }

        public const int DefaultUIWidth = 1920;
        public const int DefaultUIHeight = 1080;
        private void QuickGetTargetRectAnchor(RectTransform rt, ref Rect tfRect, ref Vector2 anchor)
        {
            Vector3[] wVec = new Vector3[4];
            rt.GetWorldCorners(wVec);
            Vector2Int anchorMin = Vector2Int.zero;
            float w = wVec[2].x - wVec[0].x;
            float h = wVec[2].y - wVec[0].y;
            float px = (wVec[2].x + wVec[0].x) * 0.5f;
            float py = (wVec[2].y + wVec[0].y) * 0.5f;
            anchorMin.x = px < DefaultUIWidth * 0.5f ? 0 : 1;
            anchorMin.y = py < DefaultUIHeight * 0.5f ? 0 : 1;
            if (anchorMin.x == 1) px -= DefaultUIWidth;
            if (anchorMin.y == 1) py -= DefaultUIHeight;
            tfRect = new Rect(px, py, w, h);
            anchor = anchorMin;
        }

        // private void DrawTutActionAreaHighlightTarget(ref Rect rect)
        // {
        //     EditorGUI.PropertyField(rect, serializedObject.FindProperty("visible")); rect.y += lineHeight;
        //     EditorGUI.PropertyField(rect, serializedObject.FindProperty("targetName")); rect.y += lineHeight;
        //     EditorGUI.PropertyField(rect, serializedObject.FindProperty("rect")); rect.y += lineHeight * 2;
        //     EditorGUI.PropertyField(rect, serializedObject.FindProperty("anchorMin")); rect.y += lineHeight;
        //     EditorGUI.PropertyField(rect, serializedObject.FindProperty("anchorMax")); rect.y += lineHeight;
        //     EditorGUI.PropertyField(rect, serializedObject.FindProperty("visualType")); rect.y += lineHeight;
        //     EditorGUI.PropertyField(rect, serializedObject.FindProperty("acceptInput")); rect.y += lineHeight;
        // }

        private void DrawTutActionAcceptClick(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("clickType")); rect.y += lineHeight;
        }

        private void DrawTutActionShowMenu(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("menuType")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("visible")); rect.y += lineHeight;
        }

        private void DrawTutActionShowTextArea(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("visible")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("title")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("text")); rect.y += lineHeight;
        }

        private void DrawTutActionShowSlide(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("visible")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("title")); rect.y += lineHeight;

            var slidesSP = serializedObject.FindProperty("slides");
            EditorGUI.PropertyField(rect, slidesSP); rect.y += lineHeight;
            if (slidesSP.isExpanded)
            {
                EditorGUI.indentLevel++;
                slidesSP.arraySize = EditorGUI.IntField(rect, "Size", slidesSP.arraySize);
                rect.y += lineHeight;

                for (int i = 0; i < slidesSP.arraySize; ++i)
                {
                    var slideSP = slidesSP.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(rect, slideSP, true); rect.y += slideSP.isExpanded ? lineHeight * 7 : lineHeight;
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawTutActionShowClickArrow(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("visible")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("arrowType")); rect.y += lineHeight;

            DragAndDropAreaGUI(ref rect, 0, 2, (obj) =>
            {
                var ctrl = FindUIBaseControllerInParent(obj);
                if (ctrl == null)
                {
                    Debug.LogError("failed drop gameobject because cant find UIBaseController in parent @Programmer");
                }
                else
                {
                    var rpath = obj.transform.FindParentRelativePath(ctrl.name, true);
                    if (string.IsNullOrEmpty(rpath))
                        Debug.LogError($"failed drop gameobject because cant find {ctrl.name} in parent");
                    else
                    {
                        serializedObject.FindProperty("targetName").stringValue = rpath;
                        var rt = obj.GetComponent<RectTransform>();
                        Rect trect = Rect.zero;
                        Vector2 tanchor = Vector2.zero;
                        QuickGetTargetRectAnchor(rt, ref trect, ref tanchor);
                        serializedObject.FindProperty("position").vector2Value = trect.position;
                        serializedObject.FindProperty("anchorMin").vector2Value = tanchor;
                        serializedObject.FindProperty("anchorMax").vector2Value = tanchor;
                    }
                }
            });

            DragAndDropAreaGUI(ref rect, 1, 2, (obj) =>
            {
                var ctrl = FindUIBaseControllerInParent(obj);
                if (ctrl == null)
                {
                    Debug.LogError("failed drop gameobject because cant find UIBaseController in parent @Programmer");
                }
                else
                {
                    var rpath = obj.transform.FindParentRelativePath(ctrl.name, true);
                    if (string.IsNullOrEmpty(rpath))
                        Debug.LogError($"failed drop gameobject because cant find {ctrl.name} in parent");
                    else
                    {
                        serializedObject.FindProperty("targetName").stringValue = "";
                        var rt = obj.GetComponent<RectTransform>();
                        var trect = new Rect(rt.anchoredPosition.x, rt.anchoredPosition.y, rt.sizeDelta.x, rt.sizeDelta.y);
                        var tanchorMin = rt.anchorMin;
                        var tanchorMax = rt.anchorMax;
                        serializedObject.FindProperty("rect").rectValue = trect;
                        serializedObject.FindProperty("anchorMin").vector2Value = tanchorMin;
                        serializedObject.FindProperty("anchorMax").vector2Value = tanchorMax;
                    }
                }
            });

            EditorGUI.PropertyField(rect, serializedObject.FindProperty("targetName")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("position")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("anchorMin")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("anchorMax")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("scale")); rect.y += lineHeight;
        }

        private void DrawTutActionShowHint(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("visible")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("text")); rect.y += lineHeight;
        }

        private void DrawTutActionShowUIItem(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("visible")); rect.y += lineHeight;

            var hudNamesSP = serializedObject.FindProperty("targetNames");
            EditorGUI.PropertyField(rect, hudNamesSP, true); rect.y += lineHeight;
            if (hudNamesSP.isExpanded)
            {
                rect.y += lineHeight;
                rect.y += hudNamesSP.arraySize * lineHeight;
                rect.y += lineHeight * 0.5f;
            }
        }

        private void DrawTutActionCheckAnimation(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("ani")); rect.y += lineHeight;
        }

        private void DrawTutActionCheckTask(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("taskId")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("taskState")); rect.y += lineHeight;
        }

        private void DragAndDropAreaGUI(ref Rect rect, int idx, int max, System.Action<GameObject> cb)
        {
            float dropAreaHeight = 50f;
            float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

            // Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + verticalSpacing));
            Rect areaRect = rect;
            areaRect.y += verticalSpacing * 0.5f;
            areaRect.height = dropAreaHeight;
            areaRect.width = rect.width / max;
            areaRect.x += idx * areaRect.width;
            if (idx == max - 1)
            {
                rect.y += dropAreaHeight;
            }

            GUIStyle centredStyle = GUI.skin.box;
            centredStyle.alignment = TextAnchor.MiddleCenter;
            if (EditorApplication.isPlaying)
            {
                centredStyle.normal.textColor = GUI.skin.button.normal.textColor;
                GUI.Box(areaRect, "Drop GameObject Here", centredStyle);
                DraggingAndDropping(areaRect, cb);
            }
            else
            {
                centredStyle.normal.textColor = Color.gray;
                GUI.Box(areaRect, "Drop GameObject Here(only support play mode)", centredStyle);
            }
        }

        private void DraggingAndDropping(Rect dropArea, System.Action<GameObject> cb)
        {
            Event currentEvent = Event.current;
            if (!dropArea.Contains(currentEvent.mousePosition))
                return;

            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = IsDragValid() ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;
                    currentEvent.Use();
                    break;
                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
                    {
                        GameObject obj = DragAndDrop.objectReferences[0] as GameObject;
                        cb?.Invoke(obj);
                    }
                    currentEvent.Use();
                    break;
            }
        }

        private bool IsDragValid()
        {
            if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
                return true;
            return false;
        }

        private GameObject FindUIBaseControllerInParent(GameObject obj)
        {
            var ctrls = obj.GetComponentsInParent<Test.UIBaseController>();
            if (ctrls == null || ctrls.Length == 0)
                return null;

            return ctrls[0].gameObject;
            // for (int i = 0; i < ctrls.Length; ++i)
            // {
            //     var tt = TutorialUtils.GetTargetByPath(ctrls[i].name);
            //     if (tt != null)
            //         return tt;
            // }
            // return null;
        }
    }
}