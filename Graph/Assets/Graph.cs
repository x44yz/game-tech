using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public int edgesCount;
    public Vertex firstVertex;
    public Dictionary<Vertex, List<Vertex>> adjacencyList = new Dictionary<Vertex, List<Vertex>>(10);

    public bool AddEdge(Vertex v1, Vertex v2)
    {
        if (adjacencyList.ContainsKey(v1) == false ||
            adjacencyList.ContainsKey(v2) == false)
        {
            Debug.LogWarning("cant add edge becase no vertex");
            return false;
        }
        if (IsEdgeExist(v1, v2))
        {
            Debug.LogWarning("cant add edge because had edge");
            return false;
        }

        adjacencyList[v1].Add(v2);
        adjacencyList[v2].Add(v1);

        edgesCount += 1;
        return true;
    }

    public bool AddVertexs(List<Vertex> vertexs)
    {
        foreach (var v in vertexs)
        {
            AddVertex(v);
        }
        return true;
    }

    public bool AddVertex(Vertex vertex)
    {
        if (adjacencyList.ContainsKey(vertex))
        {
            Debug.LogWarning("had contain vertex");
            return false;
        }

        if (adjacencyList.Count == 0)
            firstVertex = vertex;

        adjacencyList.Add(vertex, new List<Vertex>());
        return true;
    }

    public bool IsEdgeExist(Vertex v1, Vertex v2)
    {
        if (adjacencyList.ContainsKey(v1) && adjacencyList[v1].Contains(v2))
            return true;
        if (adjacencyList.ContainsKey(v2) && adjacencyList[v2].Contains(v1))
            return true;
        return false;
    }

    public int VertexsCount { get { return adjacencyList.Count; } }
    public int EdgesCount { get { return edgesCount; } }

    public bool HasVertex(Vertex v)
    {
        return adjacencyList.ContainsKey(v);
    }

    public List<Vertex> GetNeighbors(Vertex current)
    {
        return adjacencyList[current];
    }

    void OnDrawGizmos()
    {
        
    }
}
