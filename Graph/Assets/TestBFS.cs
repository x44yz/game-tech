using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBFS : MonoBehaviour
{
    public Graph graph;

    public List<Vertex> vertexs;

    void Awake()
    {
        graph.AddVertexs(vertexs);
    }
}
