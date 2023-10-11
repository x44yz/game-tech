using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;

public enum PointType
{
    NONE = -1,
    HOME,
    OFFICE,
    DINER,
}

public class Point : MonoBehaviour
{
    public PointType ptype;

    public Vector2 pos
    {
        get 
        {
            var p = transform.position;
            return new Vector2(p.x, p.y);
        }
        set
        {
            float z = transform.position.z;
            transform.position = new Vector3(value.x, value.y, z);
        }
    }
}
