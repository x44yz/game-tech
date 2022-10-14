using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArriveComp : MonoBehaviour
{
    public Transform target;
    public float slowDownRadius;
    public float timeToTargetSpeed;

    [Header("RUNTIME")]
    public AIAgent agent;
    public int forceId = -1;

    [Header("DEBUG")]
    public Color slowDownColor = Color.blue;

    private void Awake() 
    {
        agent = GetComponent<AIAgent>();
    }

    void Update()
    {
        Vector3 distVec = Utils.Vector3ZeroY(target.position - agent.pos);
        float dist = distVec.magnitude;
        
        float targetSpeed;
        if (dist > slowDownRadius)
        {
            targetSpeed = agent.maxMoveSpeed;
        }
        else
        {
            targetSpeed = agent.maxMoveSpeed * (dist / slowDownRadius);
        }

        Vector3 targetVelocity = distVec.normalized * targetSpeed;
        Vector3 accel = targetVelocity - agent.velocity;
        accel *= 1f / timeToTargetSpeed;

        forceId = agent.AddForce(forceId, steerForce);
    }

    void OnDrawGizmos()
    {
        Handles.color = slowDownColor;
        Handles.DrawWireDisc(target.position, Vector3.up, slowDownRadius);
    }
}
