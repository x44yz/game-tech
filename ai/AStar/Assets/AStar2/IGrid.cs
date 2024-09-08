using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
	public interface IGrid
	{
		// int Cost(Node a, Node b);
		List<Node> GetNeighbours(Node node);
		Node GetNode(Vector3 pos);
	}
}
