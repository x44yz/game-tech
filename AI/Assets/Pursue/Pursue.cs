using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 拦截
// 与 Seek 的区别，不是沿着目标的当前方向，而是预测到目标
// 想要移动的位置进行追逐，更加智能
public class Pursue : AIBehavoir
{
    public Transform target;

    [Header("RUNTIME")]
    public AIAgent agent;

    // [Header("DEBUG")]

    private void Awake() 
    {
        agent = GetComponent<AIAgent>();
    }

    void Update()
    {
        Vector3 dist = Utils.Vector3ZeroY(target.position - agent.pos);
        Vector3 accel = dist.normalized * agent.maxAccel;

        agent.accel = accel;
    }

    void OnDrawGizmos()
    {

    }
}
