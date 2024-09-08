using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
	// private const int GRIDMAP_WIDTH = 8;
	// private const int GRIDMAP_HEIGHT = 8;
	private const int GOAL_X = 2;
	private const int GOAL_Y = 4;

	public Transform gridTileRoot;
	public GameObject gridTilePrefab; 

	AStarPathfinder pathfinder;

	private void Start() 
	{
		InitGridMap();
	

		StartCoroutine(KK());
	}

	GameObject[,] objs = new GameObject[Map.MAP_WIDTH, Map.MAP_HEIGHT]; 

	private void InitGridMap()
	{
		for (int i = 0; i < Map.MAP_WIDTH; ++i)
		{
			for (int j = 0; j < Map.MAP_HEIGHT; ++j)
			{
				GameObject obj = Instantiate(gridTilePrefab);
				obj.transform.SetParent(gridTileRoot, false);
				obj.transform.position = new Vector3(i, 0, j);
				obj.name = "cube" + i + "x" + j;
				objs[i, j] = obj;

				var meshRender = obj.GetComponent<MeshRenderer>();

				// if ((j + (i % 2)) % 2 == 0)
				// 	meshRender.material.color = Color.yellow;
				// else
				// 	meshRender.material.color = Color.gray;

				if (Map.GetMap(i, j) == 1)
					meshRender.material.color = Color.yellow;
				else
					meshRender.material.color = Color.gray;

				if (i == 0 && j == 0)
					meshRender.material.color = Color.green;
				if (i == GOAL_X && j == GOAL_Y)
					meshRender.material.color = Color.red;
			}
		}
	}

	IEnumerator KK()
	{
		pathfinder = new AStarPathfinder();
		// Pathfind(new NodePosition(0, 0), new NodePosition(2, 4), pathfinder);
				// Reset the allocated MapSearchNode pointer
		pathfinder.InitiatePathfind();

		// Create a start state
		MapSearchNode nodeStart = pathfinder.AllocateMapSearchNode(new NodePosition(0, 0));

		// Define the goal state
		MapSearchNode nodeEnd = pathfinder.AllocateMapSearchNode(new NodePosition(GOAL_X, GOAL_Y));

		// Set Start and goal states
		pathfinder.SetStartAndGoalStates(nodeStart, nodeEnd);

	// Set state to Searching and perform the search
		AStarPathfinder.SearchState searchState = AStarPathfinder.SearchState.Searching;
		uint searchSteps = 0;
		yield return null;

		do
		{
			// if (Input.GetKey(KeyCode.J))
			{
				Debug.Log("xx-- getkeydown");
				searchState = pathfinder.SearchStep();
				searchSteps++;

				RenderTiles(pathfinder);
			}

			if (searchState == AStarPathfinder.SearchState.Searching)
				yield return null;
		}
		while (searchState == AStarPathfinder.SearchState.Searching);

	// Search complete
		bool pathfindSucceeded = (searchState == AStarPathfinder.SearchState.Succeeded);

		if (pathfindSucceeded)
		{
			// Success
			Path newPath = new Path();
			int numSolutionNodes = 0;	// Don't count the starting cell in the path length

			// Get the start node
			MapSearchNode node = pathfinder.GetSolutionStart();
			newPath.Add(node.position);
			++numSolutionNodes;

			// Get all remaining solution nodes
			for( ;; )
			{
				node = pathfinder.GetSolutionNext();

				if( node == null )
				{
					break;
				}

				++numSolutionNodes;
				newPath.Add(node.position);

				//
				GameObject obj = objs[node.position.x, node.position.y];
				if (obj != null)
					obj.GetComponent<MeshRenderer>().material.color = Color.green;
			};

			// Once you're done with the solution we can free the nodes up
			pathfinder.FreeSolutionNodes();

			Debug.Log("xx-- Solution path length: " + numSolutionNodes);
			Debug.Log("xx-- Solution: " + newPath.ToString());
		}
		else if (searchState == AStarPathfinder.SearchState.Failed) 
		{
			// FAILED, no path to goal
			Debug.Log("xx-- Pathfind FAILED!");
		}
	}

	private void RenderTiles(AStarPathfinder pathfinder)
	{
			MapSearchNode node = pathfinder.GetSolutionStart();

			for( ;; )
			{
				node = pathfinder.GetSolutionNext();

				if( node == null )
				{
					break;
				}

				GameObject obj = objs[node.position.x, node.position.y];
				if (obj != null)
					obj.GetComponent<MeshRenderer>().material.color = Color.green;
			};
	}
}
