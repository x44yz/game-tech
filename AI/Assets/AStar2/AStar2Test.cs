using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar2Test : MonoBehaviour
{
	public Transform start;
	public Transform target;
	public Grid grid;

	public AStar.Pathfinding pathfinding = new AStar.Pathfinding();

	void Start()
	{
		grid.path = pathfinding.FindPath(grid, start.position, target.position);
	}

	void Update()
	{
			
	}
}
