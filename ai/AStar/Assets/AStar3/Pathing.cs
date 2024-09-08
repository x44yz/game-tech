using System;
using System.Collections.Generic;
using UnityEngine;

namespace A3
{
    public class Pathing
    {
        private readonly static Vector2Int[] directions = {
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 1), new Vector2Int(-1, 1),
            new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
        };

        public struct Step
        {
            public Vector2 direction;
            public Vector2 pos;
        }

        private static List<Step> path = new List<Step>();
        private static Vector2Int target;
        private static BinaryHeap<Node> openNodes = new BinaryHeap<Node>(4096);
        private static HashSet<Node> closeNodes = new HashSet<Node>();

        private static int size;
        private static GameObject self;

        public static List<Step> BuildPath(Grid grid,
            Vector2 from_, Vector2 target_, float minRange = 0.1f, int size = 2, GameObject self = null, int depth = 100)
        {
            UnityEngine.Profiling.Profiler.BeginSample("BuildPath");
            Vector2Int from = grid.Snap(from_);
            target = grid.Snap(target_);
            path.Clear();
            if (from == target)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                return path;
            }
            openNodes.Clear();
            Node.Recycle(closeNodes);

            Pathing.size = size;
            Pathing.self = self;
            Node startNode = Node.Get();
            startNode.parent = null;
            startNode.pos = from;
            startNode.gScore = 0;
            startNode.hScore = Cost(from, target);
            startNode.score = int.MaxValue;
            startNode.directionIndex = -1;
            openNodes.Add(startNode);
            closeNodes.Add(startNode);
            int iterCount = 0;
            Node bestNode = startNode;
            while (openNodes.Count > 0)
            {
                Node node = openNodes.Take();

                if (node.hScore < bestNode.hScore)
                    bestNode = node;
                if (node.hScore <= minRange)
                {
                    TraverseBack(grid, node);
                    break;
                }

                StepTo(grid, node);
                iterCount += 1;
                if (iterCount > depth)
                {
                    TraverseBack(grid, bestNode);
                    break;
                }
            }
            //foreach (Node node in closeNodes)
            //{
            //    Iso.DebugDrawTile(node.pos, Color.magenta, 0.3f);
            //}
            //foreach (Node node in openNodes)
            //{
            //    Iso.DebugDrawTile(node.pos, Color.green, 0.3f);
            //}
            UnityEngine.Profiling.Profiler.EndSample();
            return path;
        }

        private static void StepTo(Grid grid, Node node)
        {
            Node newNode = null;

            int dirStart;
            int dirEnd;
            if (node.directionIndex == -1)
            {
                dirStart = 0;
                dirEnd = 8;
            }
            else if (node.directionIndex % 2 == 0)
            {
                dirStart = ((node.directionIndex - 1) + 8) % 8;
                dirEnd = dirStart + 3;
            }
            else
            {
                dirStart = ((node.directionIndex - 2) + 8) % 8;
                dirEnd = dirStart + 5;
            }

            for (int i = dirStart; i < dirEnd; ++i)
            {
                int dir = i % 8;
                Vector2Int pos = node.pos + directions[dir];
                bool passable = grid.Passable(pos, size, self);

                if (passable)
                {
                    if (newNode == null)
                        newNode = Node.Get();
                    newNode.pos = pos;

                    bool closed = closeNodes.Contains(newNode);
                    if (!closed)
                    {
                        newNode.parent = node;
                        newNode.gScore = node.gScore + 1;
                        newNode.hScore = Cost(target, newNode.pos);
                        newNode.score = newNode.gScore + newNode.hScore;
                        newNode.directionIndex = dir;
                        openNodes.Add(newNode);
                        closeNodes.Add(newNode);
                        newNode = null;
                    }
                }
            }

            if (newNode != null)
                newNode.Recycle();
        }

        private static void Collapse(Grid grid, Node node)
        {
            while (node.parent != null && node.parent.parent != null)
            {
                if (grid.Raycast(node.pos, node.parent.parent.pos, size: size, ignore: self))
                {
                    break;
                }

                node.parent = node.parent.parent;
            }
        }

        private static void TraverseBack(Grid grid, Node node)
        {
            UnityEngine.Profiling.Profiler.BeginSample("TraverseBack");
            while (node.parent != null)
            {
                Collapse(grid, node);
                Step step = new Step();
                step.direction = node.pos - node.parent.pos;
                step.pos = node.pos;
                path.Insert(0, step);
                node = node.parent;
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }

        private static int Cost(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        public static void DebugDrawPath(Grid grid, Vector2 from, List<Step> path)
        {
            if (path.Count > 0)
            {
                Debug.DrawLine(grid.Grid2WPos(from), grid.Grid2WPos(path[0].pos), Color.grey);
            }
            for (int i = 0; i < path.Count - 1; ++i)
            {
                Debug.DrawLine(grid.Grid2WPos(path[i].pos), grid.Grid2WPos(path[i + 1].pos));
            }
            if (path.Count > 0)
            {
                var center = grid.Grid2WPos(path[path.Count - 1].pos);
                Debug.DrawLine(center + grid.Grid2WPos(new Vector2(0, 0.15f)), center + grid.Grid2WPos(new Vector2(0, -0.15f)));
                Debug.DrawLine(center + grid.Grid2WPos(new Vector2(-0.15f, 0)), center + grid.Grid2WPos(new Vector2(0.15f, 0)));
            }
        }
    }
}
