using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using AI;

namespace AI.FSMTool
{
    [CreateAssetMenu(fileName = "FSM", menuName = "AI/FSM", order = 1)]
    public class FTGraph : NodeGraph
    {
        public class DebugParam
        {
            public uint uid = 0;
        }

        // public FTNode root;
        public string id = "";
        public string title = "FSM";
        public string description = "";

        [System.NonSerialized]
        public DebugParam debugParam = new DebugParam();

        public FTGraph()
        {
            id = BTEditorUtils.NewGuid();
        }

        // public void SetRoot(FTNode node)
        // {
        //     if (root != null)
        //     {
        //         root.IsRoot = false;
        //     }
        //     root = node;
        // }

        // public void UnsetRoot(FTNode node)
        // {
        //     // Only unset when the state passed is the same as the current one.
        //     if (node == root)
        //     {
        //         root = null;
        //     }
        // }

        // runtime mode
        public void InitWithBTTree(FSM.StateMachine tree)
        {
            Dictionary<string, FTNode> mapping = new Dictionary<string, FTNode>();
            for (int i = 0; i < nodes.Count; ++i)
            {
                if ((nodes[i] is FTNode) == false)
                    continue;

                FTNode FTNode = nodes[i] as FTNode;
                mapping[FTNode.id] = FTNode;
            }

            // InitWithFTNode(tree, mapping);
        }

        // private void InitWithFTNode(TBTTreeNode tFTNode, Dictionary<string, FTNode> mapping)
        // {
        //     FTNode FTNode = null;
        //     if (mapping.TryGetValue(tFTNode.FTNodeId, out FTNode))
        //         FTNode.TFTNode = tFTNode;
        //     else
        //         Debug.LogError("cant InitWithFTNode with id > " + tFTNode.FTNodeId);

        //     for (int i = 0; i < tFTNode.GetChildCount(); ++i)
        //     {
        //         TBTTreeNode subTFTNode = tFTNode.GetChild<TBTTreeNode>(i);
        //         InitWithFTNode(subTFTNode, mapping);
        //     }
        // }

        public bool IsExistFTNode(string FTNodeId)
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                if ((nodes[i] is FTNode) == false)
                    continue;

                FTNode FTNode = nodes[i] as FTNode;
                if (FTNode.id == FTNodeId)
                    return true;
            }
            return false;
        }
    }
}
