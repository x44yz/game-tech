using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CWR
{
    public class Path : MonoBehaviour
    {
        public Transform[] nodes;

        [Header("DEBUG")]
        public bool showPath = true;
        public Color pathColor = Color.red;
        public Color nodeColor = Color.green;
        public float nodeRadius = 0.1f;

        private void OnDrawGizmos()
        {
            if (nodes == null || nodes.Length == 0)
                return;

            Gizmos.color = pathColor;
            for (int i = 1; i < nodes.Length; ++i)
            {
                var prev = nodes[i - 1];
                var curr = nodes[i];
                Gizmos.DrawLine(prev.position, curr.position);
            }

            Gizmos.color = nodeColor;
            for (int i = 0; i < nodes.Length; ++i)
            {
                Gizmos.DrawSphere(nodes[i].position, nodeRadius);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Path))]
    public class PathEditor : Editor
    {
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();

            var path = target as Path;
            if (GUILayout.Button("COLLECT NODES"))
            {
                path.nodes = null;
                path.nodes = new Transform[path.transform.childCount];
                for (int i = 0; i < path.transform.childCount; ++i)
                {
                    var n = path.transform.GetChild(i);
                    n.name = "Node_" + i;
                    path.nodes[i] = n;
                }
            }
        }
    }
#endif
}
