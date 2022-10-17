using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 拦截
// 与 Seek 的区别，不是沿着目标的当前方向，而是预测到目标
// 想要移动的位置进行追逐，更加智能
public class Pursue : AIBehavoir
{
    public Transform target;
    public float maxPrediction;

    [Header("RUNTIME")]
    public AIAgent agent;
    public AIAgent targetAgent;
    public float prediction;
    public Vector3 targetPos;

    // [Header("DEBUG")]

    private void Awake() 
    {
        agent = GetComponent<AIAgent>();
        targetAgent = target.GetComponent<AIAgent>();
    }

    void Update()
    {
        Vector3 dir = Utils.Vector3ZeroY(targetAgent.pos - agent.pos);
        float dist = dir.magnitude;
        float speed = agent.velocity.magnitude;
        // 预测值，其实是在 target 的方向上偏移
        // 当两者距离近的时候，预测值就小，反之越大
        if (speed <= dist / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = dist / speed;

        targetPos = targetAgent.pos + targetAgent.velocity * prediction;

        // 以下与 seek 相同
        dir = Utils.Vector3ZeroY(targetPos - agent.pos);
        agent.accel = dir.normalized * agent.maxAccel;
    }

    void OnDrawGizmos()
    {
        if (targetAgent == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(agent.pos, targetAgent.pos);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(agent.pos, targetPos);

        Handles.color = Color.blue;
        Handles.DrawWireDisc(agent.pos, Vector3.up, maxPrediction);
    }
}
