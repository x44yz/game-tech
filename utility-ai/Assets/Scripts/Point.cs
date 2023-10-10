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
}
