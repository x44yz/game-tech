using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidanceComp : MonoBehaviour
{
    public Vector3 checkOffset = new Vector3(0f, 1f, 0f);
    public float headLength = 4f;
    public float sideLength = 2f;
    public LayerMask avoidanceLayer;

    [Header("RUNTIME")]
    public AIAgent agent;
    public int forceId;

    [Header("DEBUG")]
    public Color headColor = Color.blue;

    private void Awake() 
    {
        agent = GetComponent<AIAgent>();
    }

    private void Update()
    {
        var checkStartPos = agent.pos + checkOffset;

        Vector3 steerForce = Vector3.zero;
        // check forward
        RaycastHit forwardHit;
        if (Physics.Raycast(checkStartPos, agent.forward, out forwardHit, headLength, avoidanceLayer.value))
        {
            // 作用力与相撞的深度成正比
            steerForce += (headLength - (agent.pos - forwardHit.point).magnitude) * forwardHit.normal;
        }

        // 添加左右方向的检查是保证在角落是碰撞合理
        RaycastHit rightHit;
        if (Physics.Raycast(checkStartPos, agent.forward + agent.right, out rightHit, sideLength, avoidanceLayer.value))
        {
            steerForce += (sideLength - (agent.pos - rightHit.point).magnitude) * rightHit.normal;
        }

        RaycastHit leftHit;
        if (Physics.Raycast(checkStartPos, agent.forward - agent.right, out leftHit, sideLength, avoidanceLayer.value))
        {
            steerForce += (sideLength - (agent.pos - leftHit.point).magnitude) * leftHit.normal;
        }

        // Debug.Log("xx-- hit > " + steerForce);
        forceId = agent.AddForce(forceId, steerForce);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = headColor;
        Vector3 startPos = transform.position + checkOffset;
        Gizmos.DrawLine(startPos, startPos + transform.forward * headLength);
    }
}
