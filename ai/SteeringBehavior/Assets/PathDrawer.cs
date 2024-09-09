using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    public bool showMovePath;
    public Color movePathColor = Color.red;
    public float movePointInterval = 0.1f;
    public int maxMovePoint = 100;

    [Header("RUNTIME")]
    public float movePathTick = 0f;
    public int movePathStartIdx = 0;
    public List<Vector3> movePoints = new List<Vector3>();

    void Update()
    {
        movePathTick += Time.deltaTime;
        if (movePathTick >= movePointInterval)
        {
            movePathTick = 0f;
            if (movePoints.Count < maxMovePoint)
            {
                movePoints.Add(transform.position);
            }
            else
            {
                int idx = movePathStartIdx;
                movePoints[idx] = transform.position;
                movePathStartIdx = (movePathStartIdx + 1) % movePoints.Count;
            }
        }
    }

    private void OnDrawGizmos() 
    {
        if (!showMovePath)
            return;

        Gizmos.color = movePathColor;
        for (int i = 1; i < movePoints.Count; ++i)
        {
            var idx0 = (movePathStartIdx + i - 1) % movePoints.Count;
            var idx1 = (movePathStartIdx + i) % movePoints.Count;
            Gizmos.DrawLine(movePoints[idx0], movePoints[idx1]);
        }
    }
}
