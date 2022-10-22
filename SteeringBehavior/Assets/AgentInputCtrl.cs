using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInputCtrl : MonoBehaviour
{
    [Header("RUNTIME")]
    public AIAgent agent;

    void Awake()
    {
        agent = GetComponent<AIAgent>();
    }

    void Update()
    {
        agent.velocity.x = Input.GetAxis("Horizontal");
        agent.velocity.z = Input.GetAxis("Vertical");
        agent.velocity *= agent.maxMoveSpeed;
        agent.accel = agent.velocity.normalized;
        Vector3 translation = agent.velocity * Time.deltaTime;
        transform.Translate(translation, Space.World);
        transform.LookAt(transform.position + agent.velocity);
        // orientation = transform.rotation.eulerAngles.y;
    }
}
