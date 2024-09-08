using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSMTool
{
    [System.Serializable]
    public class B3NodeInfo
    {
        public string id = "";
        public string name = "";
        public string category = "";
        // public string description = "";
        public Dictionary<string, object> properties = new Dictionary<string, object>();
        // public Dictionary<string, object> display = new Dictionary<string, object>();
        public List<string> children = new List<string>();
    }

    [System.Serializable]
    public class B3TreeDisplayInfo
    {
        public int camera_x = 0;
        public int camera_y = 0;
        public int camera_z = 0;
        public int x = 0;
        public int y = 0;
    }

    [System.Serializable]
    public class B3TreeData
    {
        public string version = "0.1.0";
        public string scope = "tree";
        public string id = "";
        public string title = "";
        // public string description = "";
        public string root = "";
        public Dictionary<string, object> properties = new Dictionary<string, object>();
        public Dictionary<string, B3NodeInfo> nodes = new Dictionary<string, B3NodeInfo>();
        // public B3TreeDisplayInfo display = new B3TreeDisplayInfo();
    }
}
