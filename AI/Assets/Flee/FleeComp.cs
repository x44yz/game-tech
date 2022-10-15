using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeComp : MonoBehaviour
{
    public Transform target;

    [Header("RUNTIME")]
    public AIAgent agent;

    private void Awake() 
    {
        agent = GetComponent<AIAgent>();
    }

    void Update()
    {
        Vector3 dist = Utils.Vector3ZeroY(agent.pos - target.position);
        Vector3 accel = dist.normalized * agent.maxAccel;
        
        agent.accel = accel;
    }

    void OnDrawGizmos()
    {
    }
}
