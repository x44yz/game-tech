// using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tutorial
{
    [CustomEditor(typeof(TutorialCondition), true)]
    public class TutorialConditionEditor : Editor, ITutorialCondActionEditor
    {
        public Tutorial tutorialCfg;
        private bool showCondition = true;
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
            serializedObject.Update ();

            Vector2 marginTL = new Vector2(rect.x, rect.y);
            float guiWidth = rect.width;

            showCondition = EditorGUI.BeginFoldoutHeaderGroup(new Rect(marginTL.x + 14, marginTL.y, guiWidth - 14, lineHeight), showCondition, GetConditionName());
            marginTL.y += lineHeight;
            EditorGUI.indentLevel++;
            {
                if (showCondition)
                {
                    Rect subRect = new Rect(marginTL.x, marginTL.y, guiWidth, lineHeight);


                    var tp = target.GetType();
                    if (tp == typeof(TutConditionOpenMenu))
                        DrawTutConditionOpenMenu(ref subRect);
                    else if (tp == typeof(TutConditionTutorial))
                        DrawTutConditionTutorial(ref subRect);

                    marginTL.y = subRect.y;
                }
            }
            EditorGUI.indentLevel--;
            EditorGUI.EndFoldoutHeaderGroup();

            guiHeight = marginTL.y - rect.y;
            serializedObject.ApplyModifiedProperties ();
        }

        private string GetConditionName()
        {
            string typeName = target.GetType().ToString();
            return ObjectNames.NicifyVariableName(typeName.Replace("Game.Tut", ""));
        }

        private void DrawTutConditionOpenMenu(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("menuName")); rect.y += lineHeight;
            
            TutConditionOpenMenu tut = target as TutConditionOpenMenu;
            if (tut != null && tut.menuName == "UIPageShop")
            {
                if (tut.parameters == null || tut.parameters.Length == 0)
                {
                    tut.parameters = new string[]{TutConditionOpenMenu.ShopTypes[0]};
                }

                int selectIdx = TutConditionOpenMenu.ShopTypes.IndexOf(tut.parameters[0]);
                selectIdx = Mathf.Clamp(selectIdx, 0, TutConditionOpenMenu.ShopTypes.Count - 1);

                // EditorGUI.BeginHorizontal();
                EditorGUI.LabelField(rect, "Shop Type");
                EditorGUI.BeginChangeCheck();
                selectIdx = EditorGUI.Popup(new Rect(rect.x + rect.width * 0.35f, rect.y, rect.width * 0.65f, rect.height), selectIdx, TutConditionOpenMenu.ShopTypes.ToArray());
                if (EditorGUI.EndChangeCheck())
                    tut.parameters[0] = TutConditionOpenMenu.ShopTypes[selectIdx];
                // EditorGUI.EndHorizontal();
                rect.y += lineHeight;
            }
        }

        private void DrawTutConditionTask(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("taskId")); rect.y += lineHeight;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("taskState")); rect.y += lineHeight;
        }

        private void DrawTutConditionLoadMap(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("mapId")); rect.y += lineHeight;
        }

        private void DrawTutConditionLoot(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("lootFromTargetId")); rect.y += lineHeight;
        }

        private void DrawTutConditionTutorial(ref Rect rect)
        {
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("tutorialId")); rect.y += lineHeight;
        }
    }
}