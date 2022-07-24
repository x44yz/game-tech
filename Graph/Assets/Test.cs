using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Graph graph;

    public List<Vertex> vertexs;

    public Vertex v1;
    public Vertex v2;

    [Header("RUNTIME")]
    public List<Vertex> path;

    void Awake()
    {
        graph.AddVertexs(vertexs);
    
        graph.AddEdge(vertexs[0], vertexs[1]);
        graph.AddEdge(vertexs[0], vertexs[3]);

        graph.AddEdge(vertexs[1], vertexs[2]);
        graph.AddEdge(vertexs[1], vertexs[9]);

        graph.AddEdge(vertexs[2], vertexs[3]);
        graph.AddEdge(vertexs[2], vertexs[4]);
        graph.AddEdge(vertexs[2], vertexs[7]);

        graph.AddEdge(vertexs[4], vertexs[5]);

        graph.AddEdge(vertexs[5], vertexs[6]);

        graph.AddEdge(vertexs[6], vertexs[7]);
        graph.AddEdge(vertexs[7], vertexs[8]);
        graph.AddEdge(vertexs[8], vertexs[9]);

        // var  BFS.Find(graph, vertexs[0], vertexs[5]);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            path = BFS.Find(graph, v1, v2);
        }
    }

    void OnDrawGizmos()
    {
        if (path == null)
            return;

        Gizmos.color = Color.blue;
        var offset = Vector3.up * 0.1f;
        for (int i = 0; i < path.Count - 1; ++i)
        {
            Gizmos.DrawLine(path[i].transform.position + offset, path[i + 1].transform.position + offset);
        }
    }
}
