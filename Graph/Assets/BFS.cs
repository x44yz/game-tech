using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BreadthFirstSearcher
public static class BFS
{
    public static Vertex FindFirstMatch(Graph graph, Vertex startVertex, Vertex matchVertex)
    {
        if (!graph.HasVertex(startVertex))
        {
            Debug.Log("doesn't contain startVertex");
            return null;
        }

        int level = 0;
        var frontiers = new List<Vertex>();
        var levels = new Dictionary<Vertex, int>(graph.VertexsCount);
        var parents = new Dictionary<Vertex, object>(graph.VertexsCount);

        frontiers.Add(startVertex);
        levels.Add(startVertex, 0);
        parents.Add(startVertex, null);

        if (startVertex == matchVertex)
            return startVertex;
    }
}
