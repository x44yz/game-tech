using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool IsZero(Vector3 v)
    {
        return Mathf.Approximately(v.sqrMagnitude, 0f);
    }
}
