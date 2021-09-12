using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.FSM;

namespace AI.FSMTool
{
    public class BTUtils
    {
        public static System.Type GetType(string typeName)
        {
            System.Type type = null;
            System.Reflection.Assembly[] assemblyArray = System.AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            for (int i = 0; (i < assemblyArrayLength); ++i)
            {
                System.Type[] typeArray = assemblyArray[i].GetTypes();
                int typeArrayLength = typeArray.Length;
                for (int j = 0; j < typeArrayLength; ++j)
                {
                    if (typeArray[j].Name.Equals(typeName))
                    {
                        return typeArray[j];
                    }
                }
            }
            return type;
        }

        // public static StateMachine LoadAIFromJson(string json)
        // {
        //     string resourcePath = ResourceManager.instance.GetPath(resId);

        //     TextAsset text = ResourceManager.instance.LoadResource(resourcePath) as TextAsset;
        //     if (text == null)
        //     {
        //         Debug.LogError("cant load ai config file > " + resourcePath);
        //         return null;
        //     }

        //     var ret = ParseFromString(text.ToString());
        //     return ret;
        // }

//         public static TBTTreeNode ParseFromString(string str)
//         {
//             if (string.IsNullOrEmpty(str))
//             {
//                 Debugger.LogError("cant deserialize ai config because str is null");
//                 return null;
//             }

//             B3TreeData nb3 = LitJson.JsonMapper.ToObject<B3TreeData>(str);
//             if (nb3 == null)
//             {
//                 Debugger.LogError("cant deserialize ai config to behavior3 format");
//                 return null;
//             }

//             TBTTreeNode aiTree = ToTsiUTree(nb3);
//             if (aiTree == null)
//             {
//                 Debugger.LogError("cant convert monster ai behavior3 format to tsiu");
//                 return null;
//             }

//             return aiTree;
//         }

//         public static TBTTreeNode ToTsiUTree(B3TreeData b3Tree)
//         {
//             if (b3Tree == null)
//             {
//                 Debug.LogError("cant ToTsiUTree because b3Tree is null");
//                 return null;
//             }

//             string rootId = b3Tree.root;
//             B3NodeInfo rootNode = b3Tree.nodes[rootId];
//             TBTTreeNode tbtTree = ToTsiUNode(b3Tree, rootNode);
//             return tbtTree;
//         }

//         private static TBTTreeNode ToTsiUNode(B3TreeData b3Tree, B3NodeInfo b3Node)
//         {
//             if (b3Node == null)
//             {
//                 Debug.LogError("cant ToTsiUNode because b3Node is null");
//                 return null;
//             }

//             TBTTreeNode node = new TBTTreeNode();
// #if UNITY_EDITOR
//             node.FTNodeId = b3Node.id;
// #endif
//             if (b3Node.properties != null)
//             {
//                 foreach (var kv in b3Node.properties)
//                 {
//                     node.AddProperty(kv.Key, kv.Value);
//                 }
//             }

//             for (int i = 0; i < b3Node.children.Count; ++i)
//             {
//                 string nodeId = b3Node.children[i];
//                 TBTTreeNode subTBTAction = ToTsiUNode(b3Tree, b3Tree.nodes[nodeId]);
//                 if (subTBTAction != null)
//                     node.AddChild(subTBTAction);
//             }
//             return node;
//         }
    }
}
