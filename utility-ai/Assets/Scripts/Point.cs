using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
