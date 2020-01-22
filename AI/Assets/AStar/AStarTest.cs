using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
	private const int GRIDMAP_WIDTH = 8;
	private const int GRIDMAP_HEIGHT = 8;

	public Transform gridTileRoot;
	public GameObject gridTilePrefab; 

	private void Start() 
	{
		InitGridMap();
	}

	private void InitGridMap()
	{
		for (int i = 0; i < GRIDMAP_WIDTH; ++i)
		{
			for (int j = 0; j < GRIDMAP_HEIGHT; ++j)
			{
				GameObject obj = Instantiate(gridTilePrefab);
				obj.transform.SetParent(gridTileRoot, false);
				RectTransform rt = obj.GetComponent<RectTransform>();
				rt.po
			}
		}
	}
}
