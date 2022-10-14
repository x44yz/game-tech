using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekComp : MonoBehaviour
{
    public Transform target;

    [Header("RUNTIME")]
    public AIAgent agent;
    public int forceId = -1;

    [Header("DEBUG")]
    public bool showSeekPath;
    public Color seekPathColor = Color.red;
    public float seekPointInterval;
    public int maxSeekPoint;

    public float seekPathTick = 0f;
    public int seekPathStartIdx = 0;
    public List<Vector3> seekPoints = new List<Vector3>();

    private void Awake() 
    {
        agent = GetComponent<AIAgent>();
    }

    void Update()
    {
        // Vector3 dist = Utils.Vector3ZeroY(target.position - agent.pos);
        // Vector3 desiredVelocity = dist.normalized * agent.maxMoveSpeed;
        // Vector3 steerForce = desiredVelocity - agent.velocity;
        // forceId = agent.AddForce(forceId, steerForce);

        // if (showSeekPath)
        // {
        //     seekPathTick += Time.deltaTime;
        //     if (seekPathTick >= seekPointInterval)
        //     {
        //         seekPathTick = 0f;
        //         if (seekPoints.Count < maxSeekPoint)
        //         {
        //             seekPoints.Add(agent.pos);
        //         }
        //         else
        //         {
        //             int idx = seekPathStartIdx;
        //             seekPoints[idx] = agent.pos;
        //             seekPathStartIdx = (seekPathStartIdx + 1) % seekPoints.Count;
        //         }
        //     }
        // }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = seekPathColor;

        for (int i = 1; i < seekPoints.Count; ++i)
        {
            var idx0 = (seekPathStartIdx + i - 1) % seekPoints.Count;
            var idx1 = (seekPathStartIdx + i) % seekPoints.Count;
            Gizmos.DrawLine(seekPoints[idx0], seekPoints[idx1]);
        }
    }
}
