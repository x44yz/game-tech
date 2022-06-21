using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    // public int id;
    // public float x;
    // public float y;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.1f);
    }
}
