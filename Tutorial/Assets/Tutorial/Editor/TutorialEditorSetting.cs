using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;

namespace Tutorial
{
    [CreateAssetMenu(fileName = "TutorialEditorSetting", menuName = "Tutorial/TutorialEditorSetting", order = 1)]
    public class TutorialEditorSetting : ScriptableObject
    {
        [Header("The path relative to Assets")]
        public string tutorialFolder;
        public string tutorialCfgPath;

        public string tutorialFolderWithAssets => "Assets/" + tutorialFolder;
        public string tutorialCfgPathWithAssets => "Assets/" + tutorialCfgPath;

        ///////////////////////////////////////////////////
        private static TutorialEditorSetting _Settings;
        public static TutorialEditorSetting Default
        {
            get
            {
                if (_Settings == null)
                {
                    string[] guids = AssetDatabase.FindAssets("TutorialEditorSetting");
                    if (guids == null || guids.Length == 0)
                    {
                        Debug.LogError("cant find any TutorialEditorSetting.asset");
                        return null;
                    }

                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _Settings = AssetDatabase.LoadAssetAtPath<TutorialEditorSetting>(path);
                    if (_Settings == null)
                        Debug.LogError("cant load TutorialEditorSetting at path > " + path);
                }
                return _Settings;
            }
        }
    }
}