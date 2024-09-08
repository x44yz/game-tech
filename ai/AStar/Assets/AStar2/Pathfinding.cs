using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
	public class Pathfinding
	{
		public List<Node> FindPath(IGrid grid, Vector3 startPos, Vector3 targetPos)
		{
			Node startNode = grid.GetNode(startPos);
			Node targetNode = grid.GetNode(targetPos);

			return FindPath(grid, startNode, targetNode);
		}

		public List<Node> FindPath(IGrid grid, Node startNode, Node targetNode)
		{
			Debug.Log("startNode>" + startNode);
			Debug.Log("targetNode>" + targetNode);

			List<Node> openSet = new List<Node>();
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);
			Node lowHCostNode = null;

			while (openSet.Count > 0)
			{
				// 优先选择代价最小的点
				Node node = openSet[0];
				for (int i = 1; i < openSet.Count; i ++)
				{
					if (openSet[i].fCost <= node.fCost && openSet[i].hCost < node.hCost)
					{
						node = openSet[i];
					}
				}

				if (node == targetNode)
				{
					return RetracePath(startNode, targetNode);
				}

				// 记录到终点最近的点
				if (node != startNode && (lowHCostNode == null || node.hCost < lowHCostNode.hCost))
				{
					Debug.Log($"find low h cost node > {node}");
					lowHCostNode = node;
				}

				openSet.Remove(node);
				closedSet.Add(node);

				foreach (Node neighbour in grid.GetNeighbours(node))
				{
					if (!neighbour.walkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newCostToNeighbour = node.gCost + Cost(node, neighbour);
					if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = Cost(neighbour, targetNode);
						Debug.Log(neighbour);
						neighbour.parent = node;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}

			// 如果无法到达目标，找到离目标最近的点
			return FindPath(grid, startNode, lowHCostNode);;
		}

		List<Node> RetracePath(Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}
			path.Reverse();
			return path;
		}

		// int GetDistance(Node nodeA, Node nodeB)
		// {
		// 	int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		// 	int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		// 	if (dstX > dstY)
		// 		return 14*dstY + 10* (dstX-dstY);
		// 	return 14*dstX + 10 * (dstY-dstX);
		// }

		public int Cost(Node nodeA, Node nodeB)
		{
			int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
			int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

			if (dstX > dstY)
				return 14 * dstY + 10* (dstX-dstY);
			return 14 * dstX + 10 * (dstY-dstX);
		}
	}
}