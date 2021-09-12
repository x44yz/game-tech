using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace AI.FSMTool
{
    [CustomEditor(typeof(FTGraph), true)]
    public class BTGraphInspectorEditor : Editor 
    {
        private FTGraph btGraph;

        private void OnEnable() 
        {
            btGraph = (FTGraph)target;
        }

        public override void OnInspectorGUI() 
        {
            serializedObject.Update();

            if (GUILayout.Button("Edit", GUILayout.Height(40)))
            {
                NodeEditorWindow.Open(serializedObject.targetObject as XNode.NodeGraph);
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField("Title", EditorStyles.boldLabel);
            btGraph.title = EditorGUILayout.TextField(btGraph.title);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Description", EditorStyles.boldLabel);
            btGraph.description = EditorGUILayout.TextArea(btGraph.description, GUILayout.Height(70f));

            serializedObject.ApplyModifiedProperties();
        }
    }
}