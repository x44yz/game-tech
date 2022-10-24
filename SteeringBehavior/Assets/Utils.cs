using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool IsZero(Vector3 v)
    {
        return Mathf.Approximately(v.sqrMagnitude, 0f);
    }

    public static Vector3 Vector3ZeroY(Vector3 v)
    {
        v.y = 0f;
        return v;
    }

    public static Vector3 Vector3Truncate(Vector3 v, float maxLength)
    {
        float maxLengthSquard = maxLength * maxLength;
        if (v.sqrMagnitude <= maxLengthSquard)
            return v;
        v = v.normalized * maxLength;
        return v;
    }
}
