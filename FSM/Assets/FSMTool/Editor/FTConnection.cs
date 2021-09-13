using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace AI.FSMTool
{
    [System.Serializable]
    public class FTConnection
    {
        public FTNode node;
        public string portName;
        // dont serializable connect node, because we can get this from portName
        private FTNode _connectNode;

        // Transition
        // public 

        public FTConnection(FTNode node, string portName) 
        {
            this.node = node;
            this.portName = portName;
            RefreshConnectNode();
        }

        public FTNode ConnectNode
        {
            get 
            {
                RefreshConnectNode();
                return _connectNode;
            }
        }

        // because always change connection, refresh is one safe way before get
        private void RefreshConnectNode() 
        {
            NodePort port = node.GetOutputPort(portName);
            if (port == null || port.Connection == null)
                _connectNode = null;
            else
                _connectNode = (FTNode)port.Connection.node;
        }
    }
}
