using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public float walkSpeed;

    [Header("DEBUG")]
    public Color fwColor;
    public float fwLength;

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
    
    private void Update()
    {
        float dt = Time.deltaTime;
        
        pos += walkSpeed * forward * dt;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = fwColor;
        Vector3 startPos = transform.position + Vector3.up * 1f;
        Gizmos.DrawLine(startPos, startPos + transform.forward * fwLength);
    }
}
