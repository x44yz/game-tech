using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A3
{
    class Node : IEquatable<Node>, IComparable<Node>
    {
        const int InitialCapacity = 10 * 1024;

        public int gScore;
        public int hScore;
        public int score;
        public Vector2Int pos;
        public Node parent;
        public int directionIndex;

        private Node()
        {
        }

        public int CompareTo(Node other)
        {
            return score.CompareTo(other.score);
        }

        public bool Equals(Node other)
        {
            return pos == other.pos;
        }

        override public int GetHashCode()
        {
            return (pos.x * 73856093) ^ (pos.y * 83492791);
        }

        private static List<Node> pool = new List<Node>(InitialCapacity);

        static Node()
        {
            for (int i = 0; i < InitialCapacity; ++i)
            {
                pool.Add(new Node());
            }
        }

        public static Node Get()
        {
            Node node;
            if (pool.Count > 0)
            {
                int last = pool.Count - 1;
                node = pool[last];
                pool.RemoveAt(last);
            }
            else
            {
                node = new Node();
            }
            return node;
        }

        public static void Recycle(ICollection<Node> nodes)
        {
            pool.AddRange(nodes);
            nodes.Clear();
        }

        public void Recycle()
        {
            pool.Add(this);
        }
    }
}
