using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSMTool
{
    public class BTEditorUtils
    {
        public static string NewGuid()
        {
            return System.Guid.NewGuid().GetHashCode().ToString();
        }

        public static string ToNodeDisplayName(System.Type type)
        {
            string displayName;
            // if (BTDef.TsiUNodeDisplayName.TryGetValue(type, out displayName))
            //     return displayName;

            displayName = type.ToString();
            // if (string.IsNullOrEmpty(type.Namespace) == false)
            //     displayName = displayName.Replace(type.Namespace + ".", "");
            // if (displayName.StartsWith("TBT"))
            //     displayName = displayName.Replace("TBT", "");

            return displayName;
        }

        public static string ToNodeDisplayCategory(System.Type type)
        {
            string category = "Action";
            // if (type.IsSubclassOf(typeof(TsiU.TBTCondition))) category = "Conditon";
            // else if (type.IsSubclassOf(typeof(TsiU.TBTComposite))) category = "Composite";
            // else if (type.IsSubclassOf(typeof(TsiU.TBTDecorator))) category = "Decorator";
            return category;
        }

        public static string TsiUTypeToB3Name(System.Type type)
        {
            // string name;
            // if (BTDef.TsiUToB3.TryGetValue(type, out name))
            //     return name;
            return type.ToString();
        }

        public static string TsiUTypeToB3Category(System.Type type)
        {
            try
            {
                // if (type.IsSubclassOf(typeof(TsiU.TBTCondition))) return BTDef.B3_CATEGORY_CONDITION;
                // else if (type.IsSubclassOf(typeof(TsiU.TBTAction))) return BTDef.B3_CATEGORY_ACTION;
                // else if (type.IsSubclassOf(typeof(TsiU.TBTComposite))) return BTDef.B3_CATEGORY_COMPOSITE;
                // else if (type.IsSubclassOf(typeof(TsiU.TBTDecorator))) return BTDef.B3_CATEGORY_DECORATOR;
                // else
                //     Debug.LogError("TsiUTypeToB3Category cant parse type > " + type);

                return string.Empty;
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("TsiUTypeToB3Category throw exception {0} for type {1}", ex.ToString(), type);
                return string.Empty;
            }
        }

        public static B3TreeData ToB3TreeData(FTGraph btGraph)
        {
            if (btGraph == null)
            {
                Debug.LogError("cant ToB3TreeData because btGraph is null");
                return null;
            }

            B3TreeData b3Tree = new B3TreeData();
            b3Tree.id = btGraph.id;
            // b3Tree.root = btGraph.root.id;
            b3Tree.title = btGraph.title;
            // b3Tree.description = btGraph.description;
            
            for (int i = 0; i < btGraph.nodes.Count; ++i)
            {
                if ((btGraph.nodes[i] is FTNode) == false)
                    continue;

                FTNode FTNode = (FTNode)btGraph.nodes[i];

                B3NodeInfo b3NodeInfo = new B3NodeInfo();

                if (b3Tree.nodes.ContainsKey(FTNode.id))
                {
                    UnityEditor.EditorUtility.DisplayDialog("Error", $"CHECK: exist same id AI node > {FTNode.id} - {FTNode.name}", "OK");
                    return null;
                }

                b3Tree.nodes[FTNode.id] = b3NodeInfo;

                Dictionary<string, object> dictNode = new Dictionary<string, object>();
                b3NodeInfo.id = FTNode.id;
                b3NodeInfo.name = FTNode.name;
                b3NodeInfo.category = FTNode.category;
                // b3NodeInfo.description = FTNode.description;
                // b3NodeInfo.display["x"] = (int)FTNode.position.x;
                // b3NodeInfo.display["y"] = (int)FTNode.position.y;

                if (FTNode.properties != null)
                {
                    for (int j = 0; j < FTNode.properties.Count; ++j)
                    {
                        FTNodeProperty prop = FTNode.properties[j];
                        b3NodeInfo.properties.Add(prop.Key, prop.Value);
                    }
                }

                for (int j = 0; j < FTNode.ChildCount; ++j)
                {
                    FTNode childFTNode = FTNode.GetChildNode(j);
                    if (childFTNode == null)
                    {
                        UnityEditor.EditorUtility.DisplayDialog("Error", $"CHECK: exist null child AI node > {FTNode.id} - {FTNode.name}", "OK");
                        return null;
                    }
                    if (btGraph.IsExistFTNode(childFTNode.id) == false)
                    {
                        UnityEditor.EditorUtility.DisplayDialog("Error", $"CHECK: cant find child AI node > {FTNode.id} - {FTNode.name} - child: {childFTNode.id}", "OK");
                        return null;
                    }
                    b3NodeInfo.children.Add(childFTNode.id);
                }
            }

            // custom value
            // b3Tree.display.camera_x = 960;
            // b3Tree.display.camera_y = 508;
            // b3Tree.display.camera_z = 1;

            return b3Tree;
        }

        public static bool IsLeafNode(FTNode node)
        {
            var lcategory = node.category.ToLower();
            if (lcategory == BTDef.B3_CATEGORY_ACTION || lcategory == BTDef.B3_CATEGORY_CONDITION)
                return true;
            return false;
        }

        public static bool CheckExistDeadLoop(FTNode fromNode)
        {
            if (fromNode == null)
                return false;

            List<FTNode> checkedNodes = new List<FTNode>();
            checkedNodes.Add(fromNode);
            return _CheckExistDeadLoop(fromNode, checkedNodes);
        }

        private static bool _CheckExistDeadLoop(FTNode fromNode, List<FTNode> checkedNodes)
        {
            if (IsLeafNode(fromNode))
                return false;

            for (int i = 0; i < fromNode.ChildCount; ++i)
            {
                var ch = fromNode.GetChildNode(i);
                if (ch == null)
                    continue;

                if (checkedNodes.Contains(ch))
                {
                    Debug.Log("BT-- exist dead loop at loop > " + ch.id + " - " + ch.name);
                    return true;
                }
                
                List<FTNode> ncheckedNodes = new List<FTNode>();
                ncheckedNodes.AddRange(checkedNodes);
                ncheckedNodes.Add(ch);
                if (_CheckExistDeadLoop(ch, ncheckedNodes))
                    return true;
            }
            return false;
        }
    }
}