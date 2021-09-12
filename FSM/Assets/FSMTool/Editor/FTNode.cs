using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using AI;

namespace AI.FSMTool
{
    // [CreateNodeMenu("FTNode")]
    // [Serializable]
    public class FTNode : Node
    {
        public string id = "";
        public string category = "";
        public string description = "";
        public List<FTNodeProperty> properties = new List<FTNodeProperty>();

        [Input] public FTConnection From;
        [SerializeField] protected List<FTConnection> _children = new List<FTConnection>();
        // [SerializeField] private bool _isRoot;
        private System.Type _nodeSystemType = null;
        private FSM.State _tFTNode = null; // for runtime mode

        public FSM.State StateNode
        {
            get { return _tFTNode; }
            set { _tFTNode = value; }
        }

        public override object GetValue(NodePort port)
        {
            // override for ignore warning
            return null;
        }

        // public override void OnCreateConnection(NodePort from, NodePort to) 
        // { 
        //     Debug.Log("xx-- OnCreateConnection > " + title + " - " + from.fieldName + " - " + to.fieldName);
        // }

        // public override void OnRemoveConnection(NodePort port) 
        // { 
        //     Debug.Log("xx-- OnRemoveConnection > " + title + " - " + port.fieldName);
        // }

//         public Game.AIStatus CurAIStatus
//         {
//             get
//              {
//                 if (_tFTNode == null)
//                 {
// #if UNITY_EDITOR
//                     // in editor, want to debug, so disable error log
// #else
//                     // Debug.LogError("TFTNode in FTNode is null");
// #endif
//                     return Game.AIStatus.None;
//                 }

//                 return _tFTNode.aiStatus;
//             }
//         }

        // public bool IsRoot
        // {
        //     get { return _isRoot; }
        //     set
        //     {
        //         FTGraph btGraph = graph as FTGraph;
        //         if (value)
        //         {
        //             btGraph.SetRoot(this);
        //             NodePort port = GetInputPort("parent");
        //             port.Disconnect(port.Connection);
        //         }
        //         else
        //         {
        //             btGraph.UnsetRoot(this);
        //         }
        //         _isRoot = value;
        //     }
        // }

        // public void SetRoot()
        // {
        //     IsRoot = true;
        // }

        // public void UnsetRoot()
        // {
        //     IsRoot = false;
        // }

        public int ChildCount
        {
            get { return _children.Count; }
        }

        public FTConnection GetChildConnection(int index)
        {
            return _children.Count > index ? _children[index] : null;
        }

        public FTNode GetChildNode(int index)
        {
            FTConnection bc = GetChildConnection(index);
            return bc != null ? bc.ConnectNode : null;
        }

        public void InitWhenAddFromGraph(FTNodeCfg cfg)
        {
            id = BTEditorUtils.NewGuid();

            InitWithFTNodeCfg(cfg);
        }

        public void InitWithFTNodeCfg(FTNodeCfg cfg)
        {
            if (cfg == null)
            {
                Debug.LogError("failed init FTNode because cfg is null");
                return;
            }

            string[] nodeInfo = cfg.path.Split('/');
            if (nodeInfo == null || nodeInfo.Length < 2)
            {
                Debug.LogError("failed init FTNode because path format is not right > " + cfg.path);
                return;
            }

            category = nodeInfo[0];
            name = nodeInfo[nodeInfo.Length - 1];

            properties.Clear();
            if (cfg.defaultProperties != null && cfg.defaultProperties.Length > 0)
            {
                for (int i = 0; i < cfg.defaultProperties.Length; ++i)
                {
                    FTNodeProperty np = cfg.defaultProperties[i].Clone();
                    properties.Add(np);
                } 
            }
        }

        public void AddChildConnection(NodePort connection)
        {
            if (connection.ValueType != typeof(FTConnection))
            {
                return;
            }

            NodePort newport = AddDynamicOutput(typeof(FTConnection), Node.ConnectionType.Override);
            _children.Add(new FTConnection(this, newport.fieldName));
            newport.Connect(connection);
        }

        public void RemoveChildConnection(int index)
        {
            if (_children.Count <= index)
            {
                return;
            }

            RemoveDynamicPort(_children[index].portName);
            _children.RemoveAt(index);
        }
    }
}
