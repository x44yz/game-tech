using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

namespace Tutorial
{
    [CustomEditor(typeof(Tutorial), true)]
    public class TutorialEditor : Editor
    {
        TutorialConditionEditor[] conditionEditors = null;
        TutorialActionEditor[] actionEditors = null;

        private Type[] conditionTypes;
        private ReorderableList conditionList;

        private Type[] actionTypes;
        private ReorderableList actionList;

        private bool refreshTutorialCfgs = false;

        [MenuItem("Tools/RefreshTutorialConfigs")]
        private static void RefreshTutorialConfigs()
        {
            if (TutorialEditorSetting.Default == null)
            {
                EditorUtility.DisplayDialog("Error", "Please create TutorialEditorSetting first", "OK");
                return;
            }

            if (string.IsNullOrEmpty(TutorialEditorSetting.Default.tutorialFolder))
            {
                EditorUtility.DisplayDialog("Error", "Please check TutorialEditorSetting tutorial folder setting", "OK");
                return;
            }

            if (string.IsNullOrEmpty(TutorialEditorSetting.Default.tutorialCfgPath))
            {
                EditorUtility.DisplayDialog("Error", "Please check TutorialEditorSetting tutorial cfg path setting", "OK");
                return;
            }

            var tutorials = LoadAllTutorialAssets(TutorialEditorSetting.Default.tutorialFolder).ToArray();

            // check same id
            for (int i = 0; i < tutorials.Length - 1; ++i)
            {
                for (int j = i + 1; j < tutorials.Length; ++j)
                {
                    if (tutorials[i].id == 0 || tutorials[j].id == 0)
                    {
                        EditorUtility.DisplayDialog("Error", "CHECK: exist tutorial id == 0 > " + (tutorials[i].id == 0 ? tutorials[i].name : tutorials[j].name), "OK");
                        return;
                    }

                    if (tutorials[i].id == tutorials[j].id)
                    {
                        EditorUtility.DisplayDialog("Error", "CHECK: exist same tutorial id > " + tutorials[i].name + " - " + tutorials[j].name, "OK");
                        return;
                    }
                }
            }

            var tcfgs = AssetDatabase.LoadAssetAtPath<TutorialConfigs>(TutorialEditorSetting.Default.tutorialCfgPathWithAssets);
            tcfgs.tutorials = tutorials;

            EditorUtility.SetDirty(tcfgs);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static List<Tutorial> LoadAllTutorialAssets(string tutorialFolder)
        {
            var objs = Directory.GetFiles(Application.dataPath + "/" + tutorialFolder);
            List<Tutorial> tuts = new List<Tutorial>();
            for (int i = 0; i < objs.Length; ++i)
            {

                string fileName = objs[i];
                if (fileName.Contains(".meta"))
                    continue;
                if (!fileName.Contains("TUT_"))
                    continue;

                string assetPath = fileName.Replace(Application.dataPath, "Assets");
                assetPath = assetPath.Replace("\\", "/");
                var asset = AssetDatabase.LoadAssetAtPath<Tutorial>(assetPath);
                tuts.Add(asset);
            }
            return tuts;
        }

        private bool CheckTutorialConfigsRefresh()
        {
            var tcfgs = AssetDatabase.LoadAssetAtPath<TutorialConfigs>(TutorialEditorSetting.Default.tutorialCfgPathWithAssets);
            if (tcfgs == null)
                return false;

            List<Tutorial> tuts = LoadAllTutorialAssets(TutorialEditorSetting.Default.tutorialFolder);
            if (tuts.Count != tcfgs.tutorials.Length)
                return true;

            for (int i = 0; i < tuts.Count; ++i)
            {
                bool find = false;
                for (int j = 0; j < tcfgs.tutorials.Length; ++j)
                {
                    if (tuts[i] == tcfgs.tutorials[j])
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                    return true;
            }
            return false;
        }

        private void OnEnable()
        {
            SetupTargetTypesArray<TutorialCondition>(ref conditionTypes);
            SetupTargetTypesArray<TutorialAction>(ref actionTypes);

            var tutorialCfg = (Tutorial)target;

            // Check tutorialcfg refresh
            refreshTutorialCfgs = CheckTutorialConfigsRefresh();

            conditionList = new ReorderableList(serializedObject, serializedObject.FindProperty("conditions"), true, true, true, true);
            conditionList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Conditions");
            };
            conditionList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                OnListDrawElement<TutorialCondition, TutorialConditionEditor>(tutorialCfg.conditions, conditionEditors, rect, index);
            };
            conditionList.elementHeightCallback = (index) =>
            {
                return OnListElementHeight<TutorialCondition, TutorialConditionEditor>(tutorialCfg.conditions, conditionEditors, index);
            };
            conditionList.onRemoveCallback = (ReorderableList list) =>
            {
                OnListRemoveElement<TutorialCondition, TutorialConditionEditor>(tutorialCfg.conditions, conditionEditors, list.index);
            };
            conditionList.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
            {
                OnListAddElement<TutorialCondition>(conditionTypes, tutorialCfg.conditions);
            };

            actionList = new ReorderableList(serializedObject, serializedObject.FindProperty("actions"), true, true, true, true);
            actionList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Actions");
            };
            actionList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                OnListDrawElement<TutorialAction, TutorialActionEditor>(tutorialCfg.actions, actionEditors, rect, index);
            };
            actionList.elementHeightCallback = (index) =>
            {
                return OnListElementHeight<TutorialAction, TutorialActionEditor>(tutorialCfg.actions, actionEditors, index);
            };
            actionList.onRemoveCallback = (ReorderableList list) =>
            {
                OnListRemoveElement<TutorialAction, TutorialActionEditor>(tutorialCfg.actions, actionEditors, list.index);
            };
            actionList.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
            {
                OnListAddElement<TutorialAction>(actionTypes, tutorialCfg.actions);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var tutorialCfg = (Tutorial)target;

            EditorGUILayout.BeginVertical();
            {
                if (EditorApplication.isPlaying)
                {
                    DrawPlayAndSave();
                }

                if (refreshTutorialCfgs)
                {
                    Color oldColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Refresh Tutorial Configs"))
                    {
                        RefreshTutorialConfigs();
                        refreshTutorialCfgs = CheckTutorialConfigsRefresh();
                        EditorGUIUtility.ExitGUI();
                    }
                    EditorGUILayout.Separator();
                    GUI.backgroundColor = oldColor;
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("id"));
                if (tutorialCfg.name.Contains("_" + tutorialCfg.id.ToString() + "_") == false)
                    EditorGUILayout.HelpBox("Id does not match tutorial name", MessageType.Error);

                // Conditions
                CheckAndCreateSubEditors<TutorialCondition, TutorialConditionEditor>(tutorialCfg.conditions.ToArray(), ref conditionEditors, (subEditor) =>
                {
                    subEditor.tutorialCfg = target as Tutorial;
                });
                conditionList.DoLayoutList();

                // Actions
                CheckAndCreateSubEditors<TutorialAction, TutorialActionEditor>(tutorialCfg.actions.ToArray(), ref actionEditors, (subEditor) =>
                {
                    subEditor.tutorialCfg = target as Tutorial;
                });

                actionList.DoLayoutList();
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private void AutoSaveAsset()
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
        }

        private void DrawPlayAndSave()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Play"))
            {
                ((Tutorial)target).PlayFromEditor();
            }
            if (GUILayout.Button("Refresh Current"))
            {
                
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }

        protected void CheckAndCreateSubEditors<TTarget, TEditor>(TTarget[] subEditorTargets, ref TEditor[] subEditors,
                Action<TEditor> subEditorSetupCallback)
            where TEditor : Editor
            where TTarget : UnityEngine.Object
        {
            if (subEditors != null && subEditors.Length == subEditorTargets.Length)
                return;

            CleanupEditors(ref subEditors);
            subEditors = new TEditor[subEditorTargets.Length];

            for (int i = 0; i < subEditors.Length; i++)
            {
                subEditors[i] = CreateEditor(subEditorTargets[i]) as TEditor;
                if (subEditorSetupCallback != null)
                    subEditorSetupCallback(subEditors[i]);
            }
        }

        protected void CleanupEditors<TEditor>(ref TEditor[] subEditors) where TEditor : Editor
        {
            if (subEditors == null)
                return;

            for (int i = 0; i < subEditors.Length; i++)
            {
                DestroyImmediate(subEditors[i]);
            }
            subEditors = null;
        }

        private void SetupTargetTypesArray<TTarget>(ref Type[] types)
        {
            Type reactionType = typeof(TTarget);
            Type[] allTypes = reactionType.Assembly.GetTypes();
            List<Type> reactionSubTypeList = new List<Type>();

            for (int i = 0; i < allTypes.Length; i++)
            {
                if (allTypes[i].IsSubclassOf(reactionType) && !allTypes[i].IsAbstract)
                {
                    reactionSubTypeList.Add(allTypes[i]);
                }
            }

            types = reactionSubTypeList.ToArray();
        }

        private float OnListElementHeight<TTarget, TEditor>(List<TTarget> subEditorTargets, TEditor[] subEditors, int index)
            where TEditor : ITutorialCondActionEditor
            where TTarget : UnityEngine.Object
        {
            if (index < 0 || index >= subEditorTargets.Count || subEditors == null)
                return EditorGUIUtility.singleLineHeight;

            // because maybe change list order, so use target to find editor 
            var t = subEditorTargets[index];
            for (int j = 0; j < subEditors.Length; j++)
            {
                if (subEditors[j].GetTarget() == null)
                    continue;

                var tt = subEditors[j].GetTarget() as TTarget;
                if (tt == t)
                {
                    return subEditors[j].GetInspectorGUIHeight();
                }
            }
            return EditorGUIUtility.singleLineHeight;
        }

        private void OnListRemoveElement<TTarget, TEditor>(List<TTarget> subEditorTargets, TEditor[] subEditors, int index)
            where TEditor : ITutorialCondActionEditor
            where TTarget : UnityEngine.Object
        {
            if (index < 0 || index >= subEditorTargets.Count || subEditors == null)
                return;

            var t = subEditorTargets[index];

            if (EditorUtility.DisplayDialog("WARNING", "Are you sure to delete this?", "OK", "CANCEL"))
            {
                subEditorTargets.Remove(t);
                UnityEngine.Object.DestroyImmediate(t, true);
                AutoSaveAsset();

            }
            EditorGUIUtility.ExitGUI();
        }

        private void OnListAddElement<TTarget>(Type[] types, List<TTarget> subEditorTargets)
            where TTarget : UnityEngine.Object
        {
            var menu = new GenericMenu();
            for (int i = 0; i < types.Length; ++i)
            {
                Type tp = types[i];

                menu.AddItem(new GUIContent(tp.Name), false, () =>
                {
                    TTarget t = CreateInstance(tp) as TTarget;
                    t.name = tp.ToString();
                    t.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;

                    subEditorTargets.Add(t);
                    AssetDatabase.AddObjectToAsset(t, target);

                    AutoSaveAsset();
                });
            }

            menu.ShowAsContext();
        }

        void OnListDrawElement<TTarget, TEditor>(List<TTarget> subEditorTargets, TEditor[] subEditors, Rect rect, int index)
                    where TEditor : ITutorialCondActionEditor
            where TTarget : UnityEngine.Object
        {
            if (index < 0 || index >= subEditorTargets.Count || subEditors == null)
                return;

            // because maybe change list order, so use target to find editor 
            var t = subEditorTargets[index];
            for (int j = 0; j < subEditors.Length; j++)
            {
                var tt = subEditors[j].GetTarget() as TTarget;
                if (tt == t)
                {
                    subEditors[j].DrawInspectorGUI(rect);
                    break;
                }
            }
        }
    }
}