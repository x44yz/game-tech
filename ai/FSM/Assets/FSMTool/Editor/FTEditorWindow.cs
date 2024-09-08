using UnityEditor;
using UnityEngine;
using System;
using XNodeEditor;

namespace AI.FSMTool
{
    public class FTEditorWindow : Editor
    {
        // public static TBTTreeNode OpenNodeEditor(uint uid, string fileName, Action closeCallback = null)
        // {
        //     if (EditorWindow.HasOpenInstances<NodeEditorWindow>())
        //         return null;

        //     // when runtime, open bt editor
        //     if (NodeEditorWindow.mode != NodeEditorMode.Runtime)
        //         return null;

        //     GameObject activeObject = Selection.activeGameObject;
        //     if (activeObject == null)
        //         return null;

        //     if (string.IsNullOrEmpty(fileName))
        //     {
        //         Debug.LogError("BT-- failed open BehaviorTree because config AI is null");
        //         return null;
        //     }

        //     string jsonFile = "Assets/" + BTSettings.Default.exportClientRelativePath + "/" + fileName + ".json";
        //     TextAsset text = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonFile);
        //     if (text == null)
        //     {
        //         Debugger.LogError("BT-- failed open BehaviorTree because load json file failed > " + jsonFile);
        //         return null;
        //     }

        //     TBTTreeNode btTree = BTUtils.ParseFromString(text.ToString());
        //     if (btTree == null)
        //     {
        //         Debugger.LogError("BT-- failed open BehaviorTree because parse json file failed > " + jsonFile);
        //         return null;
        //     }

        //     BTGraph btGraph = AssetDatabase.LoadAssetAtPath<BTGraph>("Assets/BehaviorTrees/" + fileName + ".asset");
        //     if (btGraph == null)
        //     {
        //         Debug.LogError("BT-- failed open BehaviorTree asset at path > " + fileName);
        //         return null;
        //     }

        //     NodeEditorWindow nodeWindow = NodeEditorWindow.Open(btGraph as XNode.NodeGraph, false);
        //     nodeWindow.OnWindowDestroy = closeCallback;
        //     btGraph.InitWithBTTree(btTree);
        //     btGraph.debugParam.uid = uid;
        //     return btTree;
        // }

        [InitializeOnLoadMethod]
        private static void OnLoad() 
        {
            EditorApplication.playModeStateChanged -= _OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += _OnPlayModeStateChanged;
        }

        private static void _OnPlayModeStateChanged(PlayModeStateChange state) 
        {
            // if (state == PlayModeStateChange.EnteredPlayMode)
            // {
            //     var window = EditorWindow.GetWindow(typeof(NodeEditorWindow));
            //     if (window != null)
            //         window.Close();

            //     NodeEditorWindow.mode = NodeEditorMode.Runtime;
            // }
            // else
            //     NodeEditorWindow.mode = NodeEditorMode.Edit;
        }
    }
}
