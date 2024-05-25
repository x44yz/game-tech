using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;

namespace A3
{
    public class Grid : MonoBehaviour
    {
        public LayerMask unwalkableMask;
        public Vector2 gridWorldSize;
        public float gridSize;
        
        Node[,] grid;
        int gridSizeX, gridSizeY;

        void Awake()
        {
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / gridSize);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / gridSize);
            // CreateGrid();
        }

        public Vector2Int Snap(Vector3 wpos)
        {
            return new Vector2Int(
                Mathf.RoundToInt(wpos.x),
                Mathf.RoundToInt(wpos.z));
        }

        public bool Passable(Vector2 gpos, int size, GameObject ignore = null)
        {
            return false;
        }

        public Vector3 Grid2WPos(Vector2 gpos)
        {
            return new Vector3(gpos.x * gridSize, 0f, gpos.y * gridSize);
        }

        public struct RaycastHit
        {
            public bool hit;
            public GameObject gameObject;
            public Vector2 pos;

            public static implicit operator bool(RaycastHit value)
            {
                return value.hit;
            }
        }

        // public struct Cell
        // {
        //     public CollisionLayers blocked;
        //     public GameObject gameObject;
        // }

        public RaycastHit Raycast(Vector2 from, Vector2 to, float rayLength = Mathf.Infinity, float maxRayLength = Mathf.Infinity, int size = 1, GameObject ignore = null)
        {
            // var hit = new RaycastHit();
            // var diff = to - from;
            // var stepLen = 0.2f;
            // if (rayLength == Mathf.Infinity)
            //     rayLength = Mathf.Min(diff.magnitude, maxRayLength);
            // int stepCount = Mathf.RoundToInt(rayLength / stepLen);
            // var step = diff.normalized * stepLen;
            // var pos = from;
            // for (int i = 0; i < stepCount; ++i)
            // {
            //     pos += step;
            //     Cell cell = GetCell(pos);
            //     bool passable = Passable(pos, size, ignore);
            //     if (!passable)
            //     {
            //         hit.hit = !passable;
            //         hit.gameObject = cell.gameObject;
            //         hit.pos = pos;
            //         break;
            //     }
            // }
            // return hit;
            throw new System.NotImplementedException();
        }

        // void CreateGrid()
        // {
        //     grid = new Node[gridSizeX,gridSizeY];
        //     Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        //     for (int x = 0; x < gridSizeX; x ++) {
        //         for (int y = 0; y < gridSizeY; y ++) {
        //             Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
        //             bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
        //             grid[x,y] = new Node(walkable,worldPoint, x,y);
        //         }
        //     }
        // }

        // public int Cost(Node nodeA, Node nodeB)
        // {
        //     int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        //     int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        //     if (dstX > dstY)
        //         return 14 * dstY + 10* (dstX-dstY);
        //     return 14 * dstX + 10 * (dstY-dstX);
        // }

        // public List<Node> GetNeighbours(Node node)
        // {
        //     List<Node> neighbours = new List<Node>();

        //     for (int x = -1; x <= 1; x++) {
        //         for (int y = -1; y <= 1; y++) {
        //             if (x == 0 && y == 0)
        //                 continue;

        //             int checkX = node.gridX + x;
        //             int checkY = node.gridY + y;

        //             if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
        //                 neighbours.Add(grid[checkX,checkY]);
        //             }
        //         }
        //     }

        //     return neighbours;
        // }
        

        // public Node NodeFromWorldPoint(Vector3 worldPosition)
        // {
        //     float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        //     float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
        //     percentX = Mathf.Clamp01(percentX);
        //     percentY = Mathf.Clamp01(percentY);

        //     int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        //     int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
        //     return grid[x,y];
        // }

        // public List<Node> path;
        // void OnDrawGizmos() 
        // {
        //     Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

        //     if (grid != null) {
        //         foreach (Node n in grid) {
        //             Gizmos.color = (n.walkable)?Color.white:Color.red;
        //             if (path != null)
        //                 if (path.Contains(n))
        //                     Gizmos.color = Color.black;
        //             Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
        //         }
        //     }
        // }
    }
}
