using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// https://www.redblobgames.com/pathfinding/a-star/implementation.html

namespace AStarTest2
{
// public class Graph<Location>
// {
//     // NameValueCollection would be a reasonable alternative here, if
//     // you're always using string location types
//     public Dictionary<Location, Location[]> edges
//         = new Dictionary<Location, Location[]>();

//     public Location[] Neighbors(Location id)
//     {
//         return edges[id];
//     }
// };


// class BreadthFirstSearch
// {
//     static void Search(Graph<string> graph, string start)
//     {
//         var frontier = new Queue<string>();
//         frontier.Enqueue(start);

//         var visited = new HashSet<string>();
//         visited.Add(start);

//         while (frontier.Count > 0)
//         {
//             var current = frontier.Dequeue();

//             Debug.Log("Visiting " + current);
//             foreach (var next in graph.Neighbors(current))
//             {
//                 if (!visited.Contains(next)) {
//                     frontier.Enqueue(next);
//                     visited.Add(next);
//                 }
//             }
//         }
//     }
    
//     static void Main()
//     {
//         Graph<string> g = new Graph<string>();
//         g.edges = new Dictionary<string, string[]>
//             {
//             { "A", new [] { "B" } },
//             { "B", new [] { "A", "C", "D" } },
//             { "C", new [] { "A" } },
//             { "D", new [] { "E", "A" } },
//             { "E", new [] { "B" } }
//         };

//         Search(g, "A");
//     }
// }

// A* needs only a WeightedGraph and a location type L, and does *not*
// have to be a grid. However, in the example code I am using a grid.
public interface WeightedGraph<L>
{
    double Cost(Location a, Location b);
    IEnumerable<Location> Neighbors(Location id);
}


public struct Location
{
    // Implementation notes: I am using the default Equals but it can
    // be slow. You'll probably want to override both Equals and
    // GetHashCode in a real project.
    
    public readonly int x, y;
    public Location(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

		public override string ToString()
		{
			return "(" + x + ", " + y + ")";
		}
}


public class SquareGrid : WeightedGraph<Location>
{
    // Implementation notes: I made the fields public for convenience,
    // but in a real project you'll probably want to follow standard
    // style and make them private.
    
    public static readonly Location[] DIRS = new []
        {
            new Location(1, 0),
            new Location(0, -1),
            new Location(-1, 0),
            new Location(0, 1)
        };

    public int width, height;
    public HashSet<Location> walls = new HashSet<Location>();
    public HashSet<Location> forests = new HashSet<Location>();

    public SquareGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public bool InBounds(Location id)
    {
        return 0 <= id.x && id.x < width
            && 0 <= id.y && id.y < height;
    }

    public bool Passable(Location id)
    {
        return !walls.Contains(id);
    }

    public double Cost(Location a, Location b)
    {
        return forests.Contains(b) ? 5 : 1;
    }
    
    public IEnumerable<Location> Neighbors(Location id)
    {
        foreach (var dir in DIRS) {
            Location next = new Location(id.x + dir.x, id.y + dir.y);
            if (InBounds(next) && Passable(next)) {
                yield return next;
            }
        }
    }
}

public class KNode
{
		public KNode parent; // used during the search to record the parent of successor nodes
		public KNode child; // used after the search for the application to view the search in reverse

		public float g; // cost of this node + it's predecessors
		public float h; // heuristic estimate of distance to goal
		public float f; // sum of cumulative cost of predecessors and self and heuristic

		public KNode()
		{
			Reinitialize();
		}

		public void Reinitialize()
		{
			parent = null;
			child = null;
			g = 0.0f;
			h = 0.0f;
			f = 0.0f;
		}
}


public class PriorityQueue<T>
{
    // I'm using an unsorted array for this example, but ideally this
    // would be a binary heap. There's an open issue for adding a binary
    // heap to the standard C# library: https://github.com/dotnet/corefx/issues/574
    //
    // Until then, find a binary heap class:
    // * https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
    // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
    // * http://xfleury.github.io/graphsearch.html
    // * http://stackoverflow.com/questions/102398/priority-queue-in-net
    
    private List<KeyValuePair<T, double>> elements = new List<KeyValuePair<T, double>>();

    public int Count
    {
        get { return elements.Count; }
    }
    
    public void Enqueue(T item, double priority)
    {
        elements.Add(new KeyValuePair<T, double>(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++) {
            if (elements[i].Value < elements[bestIndex].Value) {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}


/* NOTE about types: in the main article, in the Python code I just
 * use numbers for costs, heuristics, and priorities. In the C++ code
 * I use a typedef for this, because you might want int or double or
 * another type. In this C# code I use double for costs, heuristics,
 * and priorities. You can use an int if you know your values are
 * always integers, and you can use a smaller size number if you know
 * the values are always small. */

public class AStarSearch
{
    public Dictionary<Location, Location> cameFrom
        = new Dictionary<Location, Location>();
    public Dictionary<Location, double> costSoFar
        = new Dictionary<Location, double>();
		// public PriorityQueue<Location> kk;
		// public Queue kk = new Queue();
		public Stack<Location> kk = new Stack<Location>();

    // Note: a generic version of A* would abstract over Location and
    // also Heuristic
    static public double Heuristic(Location a, Location b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public AStarSearch(WeightedGraph<Location> graph, Location start, Location goal)
    {
        var frontier = new PriorityQueue<Location>();
				// kk = frontier;
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Equals(goal))
            { 
                break;
            }

            foreach (var next in graph.Neighbors(current))
            {
                double newCost = costSoFar[current]
                    + graph.Cost(current, next);
                if (!costSoFar.ContainsKey(next)
                    || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    double priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;

										Debug.Log("xx-- from " + current + " to " + next);
                }
            }
        }
    }
}


	public class AStarTest2 : MonoBehaviour
	{
		public GameObject cubePrefab;


     void DrawGrid(SquareGrid grid, AStarSearch astar) {
        // Print out the cameFrom array
				StringBuilder sb = new StringBuilder();
        for (var y = 0; y < 10; y++)
        {
            for (var x = 0; x < 10; x++)
            {
                Location id = new Location(x, y);
                Location ptr = id;
                if (!astar.cameFrom.TryGetValue(id, out ptr))
                {
                    ptr = id;
                }

								GameObject obj = Instantiate(cubePrefab);
								obj.transform.position = new Vector3(x, 0, y);
								obj.name = "cube" + x + "x" + y;

								var meshRender = obj.GetComponent<MeshRenderer>();

                if (grid.walls.Contains(id)) { 
								// 	sb.Append("##"); 
								 	meshRender.material.color = Color.gray;
								}
								if (grid.forests.Contains(id)) {
									meshRender.material.color = Color.cyan;
								}

                // else if (ptr.x == x+1) { 
								// 	sb.Append("\u2192 "); 
								// 	meshRender.material.color = Color.green;
								// }
                // else if (ptr.x == x-1) { 
								// 	sb.Append("\u2190 "); 
								// 	meshRender.material.color = Color.green;
								// }
                // else if (ptr.y == y+1) { 
								// 	sb.Append("\u2193 "); 
								// 	meshRender.material.color = Color.green;
								// }
                // else if (ptr.y == y-1) { 
								// 	sb.Append("\u2191 "); 
								// 	meshRender.material.color = Color.green;
								// }
                // else { 
								// 	sb.Append("* "); 
								// 	meshRender.material.color = Color.red;
								// }

								// if (astar.cameFrom.TryGetValue(id, out ptr))
                // {
                //     ptr = id;
								// 		meshRender.material.color = Color.green;
                // }

								if (x == 1 && y == 4)
									meshRender.material.color = Color.yellow;
								if (x == 8 && y == 5)
									meshRender.material.color = Color.blue;
            }
            // sb.Append("\n");
        }

				// Debug.Log("-----------------------------");
				// Debug.Log(sb.ToString());
    }

		void Start()
		{
        // Make "diagram 4" from main article
        var grid = new SquareGrid(10, 10);
        for (var x = 1; x < 4; x++)
        {
            for (var y = 7; y < 9; y++)
            {
                grid.walls.Add(new Location(x, y));
            }
        }
        grid.forests = new HashSet<Location>
            {
                new Location(3, 4), new Location(3, 5),
                new Location(4, 1), new Location(4, 2),
                new Location(4, 3), new Location(4, 4),
                new Location(4, 5), new Location(4, 6),
                new Location(4, 7), new Location(4, 8),
                new Location(5, 1), new Location(5, 2),
                new Location(5, 3), new Location(5, 4),
                new Location(5, 5), new Location(5, 6),
                new Location(5, 7), new Location(5, 8),
                new Location(6, 2), new Location(6, 3),
                new Location(6, 4), new Location(6, 5),
                new Location(6, 6), new Location(6, 7),
                new Location(7, 3), new Location(7, 4),
                new Location(7, 5)
            };

        // Run A*
        var astar = new AStarSearch(grid, new Location(1, 4),
                                    new Location(1, 7));

        DrawGrid(grid, astar);
		}
	}
}
