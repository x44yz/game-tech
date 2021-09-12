using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace AI.FSMTool
{
    [CustomEditor(typeof(FTNode), true)]
    public class FTNodeInspectorEditor : Editor 
    {
        private FTNode FTNode;
        private string[] categoryNodes = null;
        private int toReplaceNameIdx = -1;

        private void OnEnable() 
        {
            FTNode = (FTNode)target;
        }

        public override void OnInspectorGUI() 
        {
            serializedObject.Update();

            // if (NodeEditorWindow.mode == NodeEditorMode.Runtime)
            //     EditorGUI.BeginDisabledGroup(true);
            // else
            //     EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("ID", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(FTNode.id);

            DrawName();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Category", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(FTNode.category);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Description", EditorStyles.boldLabel);
            FTNode.description = EditorGUILayout.TextArea(FTNode.description, GUILayout.Height(70f));

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);

                if (GUILayout.Button("Add"))
                {
                    FTNodeProperty prop = new FTNodeProperty();
                    prop.Key = "key";
                    prop.StringValue = "value";
                    prop.ValueType = FTNodeProperty.StringType;
                    FTNode.properties.Add(prop);
                }
            }
            EditorGUILayout.EndHorizontal();

            DrawProperties();

            // if (NodeEditorWindow.mode == NodeEditorMode.Runtime)
            // {
            //     EditorGUI.EndDisabledGroup();
            //     DrawRuntimeInfo();
            // }
            // else
            // {
            //     if (EditorGUI.EndChangeCheck())
            //         AutoSaveAsset();
            // }

            serializedObject.ApplyModifiedProperties();
        }

        private void AutoSaveAsset()
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        private string[] GetValidReplaceNodes(string category)
        {
            if (categoryNodes != null)
                return categoryNodes;

            List<string> nodes = new List<string>();
            for (int i = 0; i < BTEditorDefine.NodeCfgs.Count; ++i)
            {
                string path = BTEditorDefine.NodeCfgs[i].path;
                string[] nodeInfo = path.Split('/');

                if (category == "Action" || category == "Condition" || 
                    nodeInfo[0] == category || FTNode.ChildCount == 0)
                {
                    nodes.Add(path);
                }
                else if (category == "Composite" && FTNode.ChildCount == 1 && 
                    nodeInfo[0] == "Decorator")
                {
                    nodes.Add(path);
                }
                else if (category == "Decorator" && nodeInfo[0] == "Composite")
                {
                    nodes.Add(path);
                }
            }
            categoryNodes = nodes.ToArray();
            return categoryNodes;
        }

        private void DrawName()
        {
            EditorGUILayout.LabelField("Name", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            {
                string[] nodes = GetValidReplaceNodes(FTNode.category);

                int curValIdx = Array.FindIndex(nodes, (x)=>{ return x.Contains(FTNode.name); });
                int selValIdx = EditorGUILayout.Popup(FTNode.name, toReplaceNameIdx != -1 ? toReplaceNameIdx : curValIdx, nodes);
                toReplaceNameIdx = selValIdx != curValIdx ? selValIdx : -1;

                EditorGUI.BeginDisabledGroup(toReplaceNameIdx == -1);
                if (GUILayout.Button("Replace"))
                {
                    if (EditorUtility.DisplayDialog("WARNING", "Are you sure to replace?", "YES", "NO"))
                    {
                        FTNode.name = nodes[toReplaceNameIdx];
                        var nodeCfg = BTEditorDefine.GetFTNodeCfg(FTNode.name);
                        FTNode.InitWithFTNodeCfg(nodeCfg);
                    }

                    toReplaceNameIdx = -1;
                    EditorGUIUtility.ExitGUI();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawProperties()
        {
            FTNodeCfg nodeCfg = BTEditorDefine.GetFTNodeCfg(FTNode.name);

            for (int i = 0; i < FTNode.properties.Count; ++i)
            {
                FTNodeProperty prop = FTNode.properties[i];
                bool isDefaultProperty = IsDefaultProperty(nodeCfg, prop.Key);
                FTNodePropertyCfg propCfg = BTEditorDefine.GetFTNodePropertyCfg(prop.Key, FTNode.category);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(isDefaultProperty);
                    {
                        prop.Key = EditorGUILayout.TextField(prop.Key);

                        int lastSelectTypeIndex = Array.IndexOf(FTNodeProperty.SupportTypes, prop.ValueType);
                        int selectTypeIndex = EditorGUILayout.Popup(lastSelectTypeIndex, FTNodeProperty.SupportTypes);
                        prop.ValueType = FTNodeProperty.SupportTypes[selectTypeIndex];
                    }
                    EditorGUI.EndDisabledGroup();

                    if (prop.ValueType == FTNodeProperty.IntType)
                    {
                        prop.IntValue = EditorGUILayout.IntField(prop.IntValue);
                    }
                    else if (prop.ValueType == FTNodeProperty.FloatType)
                    {
                        // special
                        bool isValueDisabled = false;
                        if (FTNode.name == "CheckTargetHatred" && prop.Key == "value2")
                        {
                            if (FTNode.properties[1].Key == "compOperator" && 
                                (FTNode.properties[1].StringValue == "lessEqual" || FTNode.properties[1].StringValue == "greaterEqual"))
                            {
                                isValueDisabled = true;
                            }
                        }

                        EditorGUI.BeginDisabledGroup(isValueDisabled);
                        prop.FloatValue = EditorGUILayout.FloatField(prop.FloatValue);
                        EditorGUI.EndDisabledGroup();
                    }
                    else if (prop.ValueType == FTNodeProperty.StringType)
                    {
                        if (propCfg != null && propCfg.valueType == FTNodePropertyCfg.ValueType.StringEnum)
                        {
                            string[] defVals = propCfg.GetStringEnumValues();
                            int curValIdx = Array.IndexOf(defVals, prop.StringValue);
                            if (curValIdx >= 0 && curValIdx < defVals.Length)
                            {
                                int selValIdx = EditorGUILayout.Popup(curValIdx, defVals);
                                prop.StringValue = defVals[selValIdx];
                            }
                            else
                            {
                                Debug.LogError("Failed to get value in prop cfg > " + FTNode.name + " - " + prop.StringValue);
                                prop.StringValue = EditorGUILayout.TextField(prop.StringValue);
                            }
                        }
                        else
                        {
                            prop.StringValue = EditorGUILayout.TextField(prop.StringValue);
                        }
                    }
                    else
                        Debug.LogError("not implement FTNodeProperty value type > " + prop.ValueType);
                    
                    EditorGUI.BeginDisabledGroup(isDefaultProperty);
                    if (GUILayout.Button("Remove"))
                    {
                        FTNode.properties.Remove(prop);
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private bool IsDefaultProperty(FTNodeCfg nodeCfg, string key)
        {
            if (nodeCfg == null) return false;
            if (nodeCfg.defaultProperties == null) return false;
            for (int i = 0; i < nodeCfg.defaultProperties.Length; ++i)
            {
                if (nodeCfg.defaultProperties[i].Key == key)
                    return true;
            }
            return false;
        }

        // private void DrawRuntimeInfo()
        // {
        //     if (FTNode.TFTNode == null)
        //         return;
            
        //     EditorGUILayout.Space();
        //     EditorGUILayout.LabelField("AIStatus: " + FTNode.TFTNode.aiStatus);
        //     if (FTNode.TFTNode.aiStatus == Game.AIStatus.Failed)
        //         EditorGUILayout.LabelField("FailedCode: " + FTNode.TFTNode.failedCode);
        // }
    }
}