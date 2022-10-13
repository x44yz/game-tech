using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeComp : MonoBehaviour
{
    public Transform target;

    [Header("RUNTIME")]
    public AIAgent agent;
    public int forceId = -1;

    [Header("DEBUG")]
    public bool showFleePath;
    public Color fleePathColor = Color.red;
    public float fleePointInterval;
    public int maxFleePoint;

    public float fleePathTick = 0f;
    public int fleePathStartIdx = 0;
    public List<Vector3> fleePoints = new List<Vector3>();

    private void Awake() 
    {
        agent = GetComponent<AIAgent>();
    }

    void Update()
    {
        Vector3 dist = Utils.Vector3ZeroY(agent.pos - target.position);
        Vector3 desiredVelocity = dist.normalized * agent.maxMoveSpeed;
        Vector3 steerForce = desiredVelocity - agent.velocity;
        forceId = agent.AddForce(forceId, steerForce);

        if (showFleePath)
        {
            fleePathTick += Time.deltaTime;
            if (fleePathTick >= fleePointInterval)
            {
                fleePathTick = 0f;
                if (fleePoints.Count < maxFleePoint)
                {
                    fleePoints.Add(agent.pos);
                }
                else
                {
                    int idx = fleePathStartIdx;
                    fleePoints[idx] = agent.pos;
                    fleePathStartIdx = (fleePathStartIdx + 1) % fleePoints.Count;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = fleePathColor;

        for (int i = 1; i < fleePoints.Count; ++i)
        {
            var idx0 = (fleePathStartIdx + i - 1) % fleePoints.Count;
            var idx1 = (fleePathStartIdx + i) % fleePoints.Count;
            Gizmos.DrawLine(fleePoints[idx0], fleePoints[idx1]);
        }
    }
}
