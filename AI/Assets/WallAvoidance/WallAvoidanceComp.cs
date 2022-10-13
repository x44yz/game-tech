using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidanceComp : MonoBehaviour
{
    public Vector3 checkOffset = new Vector3(0f, 1f, 0f);
    public float headLength = 4f;
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
        // Debug.Log("xx-- hit > " + avoidanceLayer.value + " - " + (1 << 6));
        if (Physics.Raycast(checkStartPos, agent.forward, out forwardHit, headLength, avoidanceLayer.value))
        {
            // 作用力与相撞的深度成正比
            steerForce += (headLength - (agent.pos - forwardHit.point).magnitude) * forwardHit.normal;
        }

        // Debug.Log("xx-- hit > " + steerForce);
        forceId = agent.AddForce(forceId, steerForce);

        // transform.position += volocity * Time.deltaTime;

        // if (volocity.magnitude > 0.01)
        // {
        //     Vector3 newForward = Vector3.Slerp(transform.forward, volocity, Time.deltaTime);
        //     newForward.y = 0;
        //     transform.forward = newForward;
        // }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = headColor;
        Vector3 startPos = transform.position + checkOffset;
        Gizmos.DrawLine(startPos, startPos + transform.forward * headLength);
    }
}
