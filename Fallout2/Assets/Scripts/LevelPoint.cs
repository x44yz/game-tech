using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using QuickDemo;
using NaughtyAttributes;

public enum LevelPointId
{
    None = -1,
    ActorSpawn = 0,
    MonsterSapwn = 1,
}

public enum LevelPointType
{
    Point = 0,
    Rect = 1,
    Circle = 2,
    Arc = 3,
}

public class LevelPoint : MonoBehaviour
{
    public static List<LevelPoint> points;

    public LevelPointId id;
    public LevelPointType pointType;
    [ShowIf("pointTypeIsRect")]
    public Vector2 size;
    [ShowIf(EConditionOperator.Or, "pointTypeIsCircle", "pointTypeIsArc")]
    public float radius;
    [ShowIf("pointTypeIsArc")]
    [Range(0f, 360f)]
    public float angle;

    [Header("DEBUG")]
    public Color debugColor = Color.red;

    public bool pointTypeIsPoint { get { return pointType == LevelPointType.Point; } }
    public bool pointTypeIsRect { get { return pointType == LevelPointType.Rect; } }
    public bool pointTypeIsCircle { get { return pointType == LevelPointType.Circle; } }
    public bool pointTypeIsArc { get { return pointType == LevelPointType.Arc; } }

    public static LevelPoint FindPoint(LevelPointId id)
    {
        if (points == null)
            return null;

        return points.Find(x => x.id == id);
    }

    public static List<LevelPoint> FindPoints(LevelPointId id)
    {
        if (points == null)
            return null;

        return points.FindAll(x => x.id == id);
    }

    public Vector3 pos
    {
        get { return transform.position; }
    }

    public Vector3 forward
    {
        get { return transform.forward; }
    }

    public Vector3 back
    {
        get { return -forward; }
    }

    void Awake()
    {
        if (points == null)
        {
            points = new List<LevelPoint>();
        }
        points.Add(this);
    }

    public Vector3 GetRandomPos()
    {
        if (pointType != LevelPointType.Arc)
        {
            Debug.LogError("only point arc cant call this");
            return pos;
        }

        var r = Utils.Rand(radius);
        var a = Utils.Rand(-angle * 0.5f, angle * 0.5f);
        var dir = Quaternion.Euler(0f, a, 0f) * forward;
        return pos + dir.normalized * r;
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos() 
    {
        if (pointType == LevelPointType.Point)
        {
            Gizmos.color = debugColor;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
        else if (pointType == LevelPointType.Rect)
        {
            Gizmos.color = debugColor;
            Gizmos.DrawCube(transform.position, size);
        }
        else if (pointType == LevelPointType.Circle)
        {
            Handles.color = debugColor;
            Handles.DrawWireDisc(transform.position, Vector3.up, radius);
        }
        else if (pointType == LevelPointType.Arc)
        {
            Handles.color = debugColor;
            var from = Quaternion.Euler(0f, -angle * 0.5f, 0f) * forward;
            Handles.DrawSolidArc(transform.position, Vector3.up, from, angle, radius);
        } 
    }

    [Button(enabledMode: EButtonEnableMode.Editor)]
    private void AutoName()
    {
        name = $"PT_{id}_({pos.x.ToString("F2")},{pos.z.ToString("F2")})";
    }
#endif
}
