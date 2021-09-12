using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AI.FSMTool
{
    [CustomEditor(typeof(FTSettings))]
    public class FTSettingsEditor : Editor
    {
        private FTSettings _btSetting;

        private void OnEnable()
        {
            _btSetting = (FTSettings)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Note: Path should be relative to Assets folder");

            SetExportPath("Export Client Path:", ref _btSetting.exportClientRelativePath);
            SetExportPath("Export Server Path:", ref _btSetting.exportServerRelativePath);
            SetExportPath("Export GameServer Path:", ref _btSetting.exportGameServerRelativePath);

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Define Self Node Paths:");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("selfNodePaths"));

            serializedObject.ApplyModifiedProperties();
        }

        private void SetExportPath(string title, ref string targetPath)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(title);
            GUILayout.BeginHorizontal();
            targetPath = EditorGUILayout.TextField(targetPath);
            if (GUILayout.Button("..."))
            {
                string path = EditorUtility.OpenFolderPanel("Choose Path", "", "");
                if (string.IsNullOrEmpty(path) == false)
                {
                    // replace with relative path
                    if (path.Contains(Application.dataPath))
                    {
                        targetPath = path.Replace(Application.dataPath, "");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Warning", "If you choose folder not in Assets/, " +
                            "please directly input the relative path", "OK");
                    }
                }
            }
            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(_btSetting);
        }
    }
}
