using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
    public float z;
    public float x;
    public Vector3 targetDir;

    void Update()
    {
        float toRotation = (Mathf.Atan2(z, x) * Mathf.Rad2Deg);
        targetDir = Quaternion.Euler(0f, toRotation, 0f) * Vector3.forward;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, new Vector3(0f, 0f, z));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, new Vector3(x, 0f, 0f));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, targetDir.normalized);
    }
}
