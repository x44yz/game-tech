using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float mass = 1f; // 模拟重量
    public float rotateLerp = 0.7f;
    public float walkForce = 4f;

    [Header("RUNTIME")]
    public Vector3 velocity;
    public Vector3 acc;
    public Vector3 steerForce;
    public int forceNextId;

    [Header("DEBUG")]
    public Color forwardColor = Color.red;
    public float forwardLength = 1f;
    public Color steerForceColor = Color.green;
    [Range(1f, 100f)]
    public float steerForceDebugScale = 1f;

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector3 forward
    {
        get { return transform.forward; }
        set { transform.forward = value; }
    }

    public Vector3 right
    {
        get { return transform.right; }
    }

    private void Start()
    {
        velocity = walkSpeed * forward;
    }
    
    private void Update()
    {
        float dt = Time.deltaTime;

        var force = steerForce;
        if (force.magnitude < 0.01f && velocity.magnitude < walkSpeed)
            force = walkForce * forward;

        acc = force / mass;
        velocity += acc * dt;
        pos += velocity * dt;

        // forward
        forward = Vector3.Lerp(forward, velocity, rotateLerp);
    }

    Dictionary<int, Vector3> forceMap = new Dictionary<int, Vector3>();
    public int AddForce(int forceId, Vector3 force)
    {
        if (forceMap.ContainsKey(forceId) == false && Utils.IsZero(force))
            return forceId;

        RemoveForce(forceId);

        if (forceId < 0)
        {
            forceId = forceNextId;
            forceNextId += 1;
        }
        forceMap[forceId] = force;
        UpdateSteerForce();
        return forceId;
    }

    public bool RemoveForce(int forceId)
    {
        bool ret = forceMap.Remove(forceId);
        UpdateSteerForce();
        return ret;
    }

    private void UpdateSteerForce()
    {
        steerForce = Vector3.zero;
        foreach (var kv in forceMap)
        {
            steerForce += kv.Value;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = forwardColor;
        Vector3 startPos = transform.position + Vector3.up * 1f;
        Gizmos.DrawLine(startPos, startPos + transform.forward * forwardLength);

        Gizmos.color = steerForceColor;
        startPos = transform.position + Vector3.up * 0f;
        Gizmos.DrawLine(startPos, startPos + steerForce * steerForceDebugScale);
    }
}
