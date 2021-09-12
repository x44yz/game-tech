using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AI.FSMTool
{
    // [CreateAssetMenu(fileName = "BTSettings", menuName = "BTSettings")]
    public class FTSettings : ScriptableObject
    {
        [MenuItem("GameTools/AI/BTSettings")]
        public static void OpenBTSettings()
        {
            var obj = AssetDatabase.LoadAssetAtPath<FTSettings>("Assets/Scripts/BehaviorTool/FTSettings.asset");
            Selection.activeObject = obj;
        }

        public string exportClientRelativePath;
        public string exportServerRelativePath;
        public string exportGameServerRelativePath;

        public string[] selfNodePaths;

        public string ExportClientPath
        {
            get { return Application.dataPath + exportClientRelativePath; }
        }

        public string ExportServerPath
        {
            get { return Application.dataPath + exportServerRelativePath; }
        }

        public string ExportGameServerRelativePath
        {
            get { return Application.dataPath + exportGameServerRelativePath; }
        }

        ////////////////////////////////////////////////
        private static FTSettings _btSettings;
        public static FTSettings Default
        {
            get
            {
                if (_btSettings == null)
                {
                    string[] guids = AssetDatabase.FindAssets("BTSettings");
                    if (guids == null || guids.Length == 0)
                    {
                        Debug.LogError("cant find any BTSettings.asset");
                        return null;
                    }

                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _btSettings = AssetDatabase.LoadAssetAtPath<FTSettings>(path);
                    if (_btSettings == null)
                        Debug.LogError("cant load BTSettings at path > " + path);
                }
                return _btSettings;
            }
        }
    }
}
