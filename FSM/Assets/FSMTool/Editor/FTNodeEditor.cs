using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace AI.FSMTool
{
    [CustomNodeEditor(typeof(FTNode))]
    public class FTNodeEditor : NodeEditor
    {
        // 
        public static readonly string INPUT_PORT_PARENT = "From";
        public static readonly string OUTPUT_PORT_NEWCHILD = "To"; 

        private FTNode _FTNode;
        private NodePort _outputNewChild;

        private void CheckFTNodeValid()
        {
            if (_FTNode == null)
                _FTNode = target as FTNode;
        }

        public override Color GetTint()
        {
            CheckFTNodeValid();

            // if (NodeEditorWindow.mode == NodeEditorMode.Edit)
            // {
            //     return base.GetTint();
            // }
            // else
            // {
            //     if (_FTNode.CurAIStatus == Game.AIStatus.Running)
            //         return new Color(0x27 / 255.0f, 0xae / 255.0f, 0x60 / 255.0f);
            //     else
            //         return base.GetTint();
            // }

            return base.GetTint();
        }

        public override void AddContextMenuItems(GenericMenu menu) 
        {
            CheckFTNodeValid();

            // if (NodeEditorWindow.mode == NodeEditorMode.Edit)
            // {
            //     menu.AddItem(new GUIContent("Copy"), false, NodeEditorWindow.current.CopySelectedNodes);
            //     menu.AddItem(new GUIContent("Duplicate"), false, NodeEditorWindow.current.DuplicateSelectedNodes);
            //     menu.AddItem(new GUIContent("Remove"), false, NodeEditorWindow.current.RemoveSelectedNodes);

            //     FTGraph btGraph = _FTNode.graph as FTGraph;
            //     if (btGraph != null)
            //     {
            //         menu.AddSeparator("");
                    
            //         if (btGraph.root == null && _FTNode.IsRoot == false && 
            //             _FTNode.category.ToLower() == BTDef.B3_CATEGORY_COMPOSITE)
            //         {
            //             menu.AddItem(new GUIContent("Set Root"), false, _FTNode.SetRoot);
            //         }
            //         else
            //         {
            //             menu.AddDisabledItem(new GUIContent("Set Root"));
            //         }

            //         if (_FTNode.IsRoot)
            //         {
            //             menu.AddItem(new GUIContent("Unset Root"), false, _FTNode.UnsetRoot);
            //         }
            //     }
            // }
        }

        public override void OnHeaderGUI()
        {
            CheckFTNodeValid();

            GUILayout.Label(_FTNode.name, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));

            // Draw dot
            // if (NodeEditorWindow.mode == NodeEditorMode.Runtime)
            // {
            //     if (_FTNode.CurAIStatus != Game.AIStatus.None && _FTNode.CurAIStatus != Game.AIStatus.Running)
            //     {
            //         Rect dotRect = GUILayoutUtility.GetLastRect();
            //         dotRect.size = new Vector2(16, 16);
            //         dotRect.y += 6;
            //         if (_FTNode.CurAIStatus == Game.AIStatus.Succeed)
            //             GUI.color = Color.green;
            //         else if (_FTNode.CurAIStatus == Game.AIStatus.Failed)
            //             GUI.color = Color.red;
                        
            //         GUI.DrawTexture(dotRect, NodeEditorResources.dot);
            //         GUI.color = Color.white;
            //     }
            // }
        }

        public override void OnBodyGUI()
        {
            try
            {
                CheckFTNodeValid();

                // if (_FTNode.IsRoot == false)
                // {
                GUILayout.BeginHorizontal();
                NodePort port = _FTNode.GetInputPort(INPUT_PORT_PARENT);
                NodeEditorGUILayout.PortField(new GUIContent("From"), port, GUILayout.Width(60));
                GUILayout.EndHorizontal();
                // }

                // DrawStateNode(int.MaxValue);
            }
            catch (System.Exception ex)
            {
                // because will popup some unity ui event error, so ignore this.
                Debug.Log("BT-- ignore unity ui event error > " + Event.current.type + " - " + ex.ToString());
            }
        }

        private void DrawStateNode(int maxChildCount)
        {
            int childCount = _FTNode.ChildCount;
            for (int i = 0; i < childCount; i++) 
            {
                FTConnection connection = _FTNode.GetChildConnection(i);
                NodePort port = target.GetOutputPort(connection.portName);
                NodePort connected = port.Connection;

                if (connected == null)
                {
                    _FTNode.RemoveChildConnection(i);
                    i--;
                    childCount--;
                } 
                else
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.Space();
                        NodeEditorGUILayout.PortField(new GUIContent("To" + (i + 1)), port, GUILayout.Width(50));
                        GUILayout.EndHorizontal();
                    
                        GUILayout.Button("XXX");
                    }
                    GUILayout.EndVertical();
                }
            }

            // if (NodeEditorWindow.mode == NodeEditorMode.Edit && childCount < maxChildCount)
            if (childCount < maxChildCount)
            {
                _outputNewChild = target.GetOutputPort(OUTPUT_PORT_NEWCHILD);
                if (_outputNewChild == null)
                {
                    _outputNewChild = target.AddDynamicOutput(typeof(FTConnection), Node.ConnectionType.Override, Node.TypeConstraint.Inherited, OUTPUT_PORT_NEWCHILD);
                }

                if (_outputNewChild.Connection != null)
                {
                    _FTNode.AddChildConnection(_outputNewChild.Connection);
                    _outputNewChild.Disconnect(_outputNewChild.Connection);
                    EditorUtility.SetDirty(target);

                    if (BTEditorUtils.CheckExistDeadLoop(target as FTNode))
                    {
                        EditorUtility.DisplayDialog("Error", "Your father is still your father.", "OK");
                    }
                }

                GUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                NodeEditorGUILayout.PortField(_outputNewChild, GUILayout.Width(80));
                GUILayout.EndHorizontal();
            }
        }
    }
}
