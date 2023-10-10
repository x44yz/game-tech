using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentContext : IContext
{
    public Agent agent;
    public Vector3? moveTarget;
    public Vector3 pos => agent.transform.position;

    public float GetConsiderationVal(string key)
    {
        if (key == "IsAtHome")
        {

        }
        return 0f;
    }
}
