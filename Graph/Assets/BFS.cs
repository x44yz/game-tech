using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BreadthFirstSearcher
public static class BFS
{
    public static List<Vertex> Find(Graph graph, Vertex startVertex, Vertex matchVertex)
    {
        if (!graph.HasVertex(startVertex))
        {
            Debug.Log("doesn't contain startVertex");
            return null;
        }

        var visited = new HashSet<Vertex>();
        visited.Add(startVertex);

        var frontiers = new Queue<Vertex>();
        frontiers.Enqueue(startVertex);
        
        Dictionary<Vertex, Vertex> parents = new Dictionary<Vertex, Vertex>();

        while (frontiers.Count > 0)
        {
            Vertex current = frontiers.Dequeue();
            
            if (current == matchVertex)
            {
                break;
            }

            foreach (var neighbor in graph.GetNeighbors(current))
            {
                if (visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                frontiers.Enqueue(neighbor);
                parents[neighbor] = current;
            }
        }

        var path = new List<Vertex>();
        var parent = matchVertex;
        while (parent != null)
        {
            path.Add(parent);
            parent = parents[parent];
        }
        path.Reverse();
        return path;
    }
}
