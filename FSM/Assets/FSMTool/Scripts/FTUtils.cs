using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AI.FSM;
using UnityEngine;

namespace AI.FSMTool
{
    public class FTUtils
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

        public static System.Type[] GetFSMStates(string fsmName)
        {
            var baseType = typeof(State);

            List<System.Type> types = new List<System.Type>();
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (System.Reflection.Assembly assembly in assemblies)
            {
                try
                {


                    // types.AddRange(assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray());
                    var tmpTypes = assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray();
                    foreach (var tp in tmpTypes)
                    {
                        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(tp);
                        foreach (System.Attribute attr in attrs)  
                        {  
                            if (attr is FSMAttrStateClass)
                                types.Add(tp);
                        }
                    }
                } 
                catch (System.Reflection.ReflectionTypeLoadException ex) 
                {
                    Debug.LogError("FSM: failed GetFSMStates > " + ex.ToString());
                }
            }

            return types.ToArray();
        }

        public static System.Type[] GetFSMTransitionClasses(string fsmName)
        {
            var baseType = typeof(State);

            List<System.Type> types = new List<System.Type>();
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (System.Reflection.Assembly assembly in assemblies)
            {
                try
                {
                    // types.AddRange(assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray());
                    var tmpTypes = assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray();
                    foreach (var tp in tmpTypes)
                    {
                        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(tp);
                        foreach (System.Attribute attr in attrs)  
                        {  
                            if (attr is FSMAttrTransitionClass)
                                types.Add(tp);
                        }
                    }
                } 
                catch (System.Reflection.ReflectionTypeLoadException ex) 
                {
                    Debug.LogError("FSM: failed GetFSMStates > " + ex.ToString());
                }
            }

            return types.ToArray();
        }

        public static System.Type[] GetFSMTransitionMethod(string fsmName)
        {
            // var baseType = typeof(State);

            List<System.Type> types = new List<System.Type>();
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (System.Reflection.Assembly assembly in assemblies)
            {
                try
                {
                    if (!assembly.GetName().ToString().Contains("Assembly-CSharp"))
                        continue;

                    // var tmpTypes = assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray();
                    var tmpTypes = assembly.GetTypes();
                    foreach (var tp in tmpTypes)
                    {
                        if (tp != typeof(Test.Actor))
                            continue;

                        var mds = tp.GetMethods();
                        foreach (var md in mds)
                        {
                            var attrs = md.GetCustomAttributes(typeof(FSMAttrTransitionMethod), true);
                            if (attrs != null && attrs.Length > 0)
                            {
                                types.Add(tp);
                            }
                        }

                        
                        // foreach (System.Attribute attr in attrs)  
                        // {  
                        //     if (attr is FSMAttrTransitionMethod)
                        //         types.Add(tp);
                        // }
                    }
                } 
                catch (System.Reflection.ReflectionTypeLoadException ex) 
                {
                    Debug.LogError("FSM: failed GetFSMStates > " + ex.ToString());
                }
            }

            return types.ToArray();
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
