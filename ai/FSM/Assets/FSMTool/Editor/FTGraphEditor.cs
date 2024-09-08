using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using XNode;
using XNodeEditor;
// using LitJson;
// using System.IO;

namespace AI.FSMTool
{
    [CustomNodeGraphEditor(typeof(FTGraph))]
    public class FTGraphEditor : NodeGraphEditor
    {
        private const float TOOLBAR_HEIGHT = 20f;
        private const float SEARCHFIELD_HEIGHT = 200f;

        private AutocompleteSearchField nodeSearchField;
        private List<string> nodeSearchPaths = new List<string>();

        private Rect toolbarRect;
        private string searchNodeName = "";
        private int lastFindIdx = 0;

        public override void OnOpen()
        {
            if (nodeSearchField == null)
            {
                nodeSearchField = new AutocompleteSearchField();
                nodeSearchField.onInputChanged = OnNodeSearchFieldInputChanged;
                nodeSearchField.onConfirm = OnNodeSearchFieldConfirm;
            }

            nodeSearchPaths.Clear();
            for (int i = 0; i < BTEditorDefine.NodeCfgs.Count; ++i)
            {
                FTNodeCfg cfgNode = BTEditorDefine.NodeCfgs[i];
                nodeSearchPaths.Add(cfgNode.path);
            }

            toolbarRect = new Rect(0, 0, Screen.width, TOOLBAR_HEIGHT);
            // NodeEditorWindow.current.blockMouseAreas.Add(toolbarRect);
        }

        public override void OnGUI()
        {
            FTGraph self = (FTGraph)target;
 
            DrawToolBar();
            DrawDebugInfo();
        }

        private void DrawToolBar()
        {
            // bool isRuntimeMode = NodeEditorWindow.mode == NodeEditorMode.Runtime;

            // GUILayout.BeginArea(new Rect(0, 0, Screen.width, SEARCHFIELD_HEIGHT), EditorStyles.toolbar);
            // GUILayout.BeginHorizontal();

            // GUILayout.Label(target.name, GUILayout.Width(150f));

            // // Export
            // EditorGUI.BeginDisabledGroup(isRuntimeMode);
            // if (GUILayout.Button("Export", EditorStyles.toolbarButton, GUILayout.Width(100)))
            // {
            //     BTSettings btSettings = BTSettings.Default;
            //     if (btSettings == null || string.IsNullOrEmpty(btSettings.ExportClientPath))
            //     {
            //         EditorUtility.DisplayDialog("Warning", "Failed to export, please check BTSettings.", "OK");
            //         return;
            //     }

            //     List<string> exportPaths = new List<string>();
            //     exportPaths.Add(btSettings.ExportClientPath);
            //     if (string.IsNullOrEmpty(btSettings.exportServerRelativePath) == false)
            //         exportPaths.Add(btSettings.ExportServerPath);
            //     if (string.IsNullOrEmpty(btSettings.exportGameServerRelativePath) == false)
            //         exportPaths.Add(btSettings.ExportGameServerRelativePath);

            //     ExportJson((FTGraph)target, exportPaths, target.name);

            //     EditorGUIUtility.ExitGUI();
            // }
            // EditorGUI.EndDisabledGroup();

            // // NOTE:
            // // if button added after nodeSearchField, button will be alignment-left
            // nodeSearchField.OnGUI(new Rect(260f, 1f, 210f, SEARCHFIELD_HEIGHT));

            // GUILayout.Space(210f);
            // if (GUILayout.Button("Find", EditorStyles.toolbarButton, GUILayout.Width(40)))
            // {
            //     FindFTNode(searchNodeName);
            // }

            // EditorGUI.BeginDisabledGroup(isRuntimeMode);
            // if (GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.Width(40)))
            // {
            //     if (EditorUtility.DisplayDialog("CONFIRM", "Are you sure to add " + searchNodeName, "YES", "NO"))
            //     {
            //         AddFTNode(searchNodeName, -NodeEditorWindow.current.panOffset);
            //     }
            //     EditorGUIUtility.ExitGUI();
            // }
            // EditorGUI.EndDisabledGroup();

            // GUILayout.EndHorizontal();
            // GUILayout.EndArea();
        }

        private void DrawDebugInfo()
        {
            // bool isRuntimeMode = NodeEditorWindow.mode == NodeEditorMode.Runtime;
            // if (!isRuntimeMode)
            //     return;

            // FTGraph self = (FTGraph)target;

            // GUILayout.BeginArea(new Rect(0, TOOLBAR_HEIGHT + 1, 150f, 20), EditorStyles.helpBox);
            // GUILayout.Label("UID: " + self.debugParam.uid);
            // GUILayout.EndArea();
        }

        private void ExportJson(FTGraph btGraph, List<string> paths, string filename)
        {
            // if (btGraph == null)
            // {
            //     Debug.Log("BT-- ExportJson failed because BTGraph is null");
            //     return;
            // }

            // // check root node
            // if (btGraph.root == null)
            // {
            //     EditorUtility.DisplayDialog("Warning", "Cant find any root node.", "OK");
            //     return;
            // }

            // // check dead loop
            // if (BTEditorUtils.CheckExistDeadLoop(btGraph.root))
            // {
            //     EditorUtility.DisplayDialog("Error", "Export failed, Your father is still your father.", "OK");
            //     return;
            // }

            // B3TreeData b3TreeData = BTEditorUtils.ToB3TreeData(btGraph);
            // if (b3TreeData == null)
            // {
            //     Debug.LogError("Failed export bt json, because ToB3TreeData return null");
            //     return;
            // }

            // System.Text.StringBuilder sb = new System.Text.StringBuilder();
            // JsonWriter jw = new JsonWriter(sb);
            // jw.PrettyPrint = true;
            // LitJson.JsonMapper.ToJson(b3TreeData, jw);

            // for (int i = 0; i < paths.Count; ++i)
            // {
            //     string fullpath = Path.GetFullPath(paths[i] + "/" + filename + ".json");

            //     if (File.Exists(fullpath))
            //     {
            //         Game.EditorUtils.Checkout(new List<string>() { fullpath });
            //         // NOTE:
            //         // because we cant get Checkout callback, so directly change file attribute
            //         File.SetAttributes(fullpath, File.GetAttributes(fullpath) & ~FileAttributes.ReadOnly );
            //         File.WriteAllText(fullpath, sb.ToString());
            //     }
            //     else
            //     {
            //         File.WriteAllText(fullpath, sb.ToString());
            //         Game.EditorUtils.Add(new List<string>() { fullpath });
            //     }
                
            //     Debugger.Log($"Success Export AI Json {fullpath}");
            // }

            // AssetDatabase.Refresh();
        }

        // public override void AddContextMenuItems(GenericMenu menu)
        // {
        //     if (NodeEditorWindow.mode == NodeEditorMode.Runtime)
        //         return;

        //     Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
        //     for (int i = 0; i < BTEditorDefine.NodeCfgs.Count; ++i)
        //     {
        //         FTNodeCfg cfgNode = BTEditorDefine.NodeCfgs[i];
        //         string path = cfgNode.path;

        //         menu.AddItem(new GUIContent(path), false, () =>
        //         {
        //             FTNode node = (FTNode)CreateNode(typeof(FTNode), pos);
        //             NodeEditorWindow.current.AutoConnect(node);
        //             node.InitWhenAddFromGraph(cfgNode);
        //         });
        //     }

        //     // NOTE:
        //     // because copy/paste will create same id Node, so temp disable them
        //     menu.AddSeparator("");
        //     menu.AddItem(new GUIContent("Copy"), false, () => {
        //         NodeEditorWindow.current.CopySelectedNodes();
        //     });
        //     if (NodeEditorWindow.copyBuffer != null && NodeEditorWindow.copyBuffer.Length > 0)
        //     {
        //         menu.AddItem(new GUIContent("Paste"), false, () => {
        //             NodeEditorWindow.current.PasteNodes(pos);
        //             var newNodes = Selection.objects;
        //             if (newNodes != null)
        //             {
        //                 for (int i = 0; i < newNodes.Length; ++i)
        //                 {
        //                     var FTNode = newNodes[i] as FTNode;
        //                     if (FTNode != null)
        //                         FTNode.id = BTEditorUtils.NewGuid();
        //                 }
        //             }
        //         });
        //     }
        //     else
        //         menu.AddDisabledItem(new GUIContent("Paste"));
        //     // menu.AddItem(new GUIContent("Preferences"), false, () => NodeEditorReflection.OpenPreferences());
        //     // menu.AddCustomContextMenuItems(target);

        //     // menu.AddSeparator("");
        //     menu.AddItem(new GUIContent("Group"), false, () =>
        //     {
        //         var group = (XNode.NodeGroups.NodeGroup)CreateNode(typeof(XNode.NodeGroups.NodeGroup), pos);
        //         NodeEditorWindow.current.AutoConnect(group);
        //     });

        //     // self define
        //     var selfNodePaths = BTSettings.Default.selfNodePaths;
        //     if (selfNodePaths != null && selfNodePaths.Length > 0)
        //     {
        //         menu.AddSeparator("");
        //         for (int i = 0; i < selfNodePaths.Length; ++i)
        //         {
        //             string path = selfNodePaths[i];
        //             if (string.IsNullOrEmpty(path))
        //                 continue;

        //             menu.AddItem(new GUIContent(path), false, () =>
        //             {
        //                 var pinfos = path.Split('/');
        //                 if (pinfos == null)
        //                 {
        //                     Debug.LogError("BT-- parse self bt node path failed > " + path);
        //                     return;
        //                 }
        //                 string nodeName = pinfos[pinfos.Length - 1];
        //                 var nodeCfg = BTEditorDefine.GetFTNodeCfg(nodeName);
        //                 if (nodeCfg == null)
        //                 {
        //                     Debug.LogError("BT-- parse self bt node path failed > " + path);
        //                     return;
        //                 }

        //                 FTNode node = (FTNode)CreateNode(typeof(FTNode), pos);
        //                 NodeEditorWindow.current.AutoConnect(node);
        //                 node.InitWhenAddFromGraph(nodeCfg);
        //             });
        //         }
        //     }
        // }

        void OnNodeSearchFieldInputChanged(string searchString)
        {
            searchString = searchString.Trim();

            nodeSearchField.ClearResults();
            if(!string.IsNullOrEmpty(searchString) )
            {
                for (int i = 0; i < nodeSearchPaths.Count; ++i)
                {
                    string path = nodeSearchPaths[i].ToLower();
                    if (path.Contains(searchString.ToLower()))
                    {
                        nodeSearchField.AddResult(nodeSearchPaths[i]);
                    }
                }
            }
        }

        void OnNodeSearchFieldConfirm(string result)
        {
            int idx = nodeSearchPaths.IndexOf(result);
            if (idx < 0 || idx >= BTEditorDefine.NodeCfgs.Count)
            {
                EditorUtility.DisplayDialog("ERROR", "Cant find node > " + result, "OK");
                return;
            }

            var nodeInfos = result.Split('/');
            if (nodeInfos == null || nodeInfos.Length < 2)
                return;

            searchNodeName = nodeInfos[nodeInfos.Length - 1];
            nodeSearchField.searchString = searchNodeName;
            lastFindIdx = 0;
        }

        private void AddFTNode(string nodeName, Vector2 gpos)
        {
            var nodeCfg = BTEditorDefine.GetFTNodeCfg(nodeName);
            if (nodeCfg == null)
            {
                Debug.LogError("Failed to AddFTNode because cant find FTNodeCfg > " + nodeName);
                return;
            }

            FTNode node = (FTNode)CreateNode(typeof(FTNode), gpos);
            NodeEditorWindow.current.AutoConnect(node);
            node.InitWhenAddFromGraph(nodeCfg);

            FocusOnFTNode(node);
        }

        private void FindFTNode(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName))
                return;

            FTGraph gp = (FTGraph)target;
            if (gp == null || gp.nodes.Count == 0)
                return;
            
            int nodeCount = gp.nodes.Count;
            int startIdx = Mathf.Min(lastFindIdx + 1, nodeCount);
            
            string nodeNameLower = nodeName.ToLower();
            for (int i = 0; i < nodeCount; ++i)
            {
                int idx = (startIdx + i) % nodeCount;
                if (idx < 0 || idx >= nodeCount)
                    break;

                Node nd = gp.nodes[idx];
                if ((nd is FTNode) == false)
                    continue;

                if (nd.name.ToLower() == nodeNameLower)
                {
                    lastFindIdx = idx;
                    FocusOnFTNode(nd as FTNode);
                    break;
                }
            }
        }

        private void FocusOnFTNode(FTNode node)
        {
            Selection.activeObject = node;
            NodeEditorWindow.current.panOffset = -node.position;
            NodeEditorWindow.current.zoom = 1f;
        }
    }
}
