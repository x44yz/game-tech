using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Utility;
using System.Linq;

public enum Stat
{
    NONE = -1,
    ENERGY = 0,
    HUNGER = 1,
    MONEY = 2,
    SOCIAL = 3,
    MOOD = 4,
    COUNT,
}

public class Agent : MonoBehaviour
{
    public AgentAI ai;
    public AgentContext context;
    public float walkSpd;
    [Range(0, 100)]
    public float initEnergy;
    [Range(0, 100)]
    public float initHunger;
    [Range(0, 100)]
    public float initMoney;
    [Range(0, 100)]
    public float initSocial;
    [Range(0, 100)]
    public float initMood;

    [Header("RUNTIME")]
    public float[] stats;
    public Point[] points;
    public Point moveToPoint;
    public Point curAtPoint;

    public PointType curAtPointType => curAtPoint ? curAtPoint.ptype : PointType.NONE;

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

    private void Start()
    {
        stats = new float[(int)Stat.COUNT];
        points = GameObject.FindObjectsOfType<Point>();

        ModStat(Stat.ENERGY, initEnergy);
        ModStat(Stat.HUNGER, initHunger);
        ModStat(Stat.MONEY, initMoney);
        ModStat(Stat.SOCIAL, initSocial);
        ModStat(Stat.MOOD, initMood);

        moveToPoint = null;
        curAtPoint = null;

        ai.onActionChanged += (ActionObj obj) =>{
            TimeSystem.Inst.paused = true;
        };
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (TimeSystem.Inst.paused)
            return;

        // update context
        context.deltaMins = TimeSystem.Inst.deltaMins;

        ai.Tick(context, dt);

        if (moveToPoint != null)
        {
            TickMove(dt);
        }
    }

    public IContext GetContext()
    {
        return context;
    }

    public AgentAI GetAgentAI()
    {
        return ai;
    }

    public float GetStat(Stat s)
    {
        return stats[(int)s];
    }

    public float GetStatMax(Stat s)
    {
        return 100f;
    }

    public void ModStat(Stat s, float v)
    {
        float cur = GetStat(s);
        float max = GetStatMax(s);
        stats[(int)s] = Mathf.Clamp(cur + v, 0, max);
    }

    public Point GetPoint(PointType pt)
    {
        if (points == null)
            return null;
        return points.First(x => x.ptype == pt);
    }

    public bool IsAtPoint(PointType pt, float dist = 0.1f)
    {
        var p = GetPoint(pt);
        if (p == null)
        {
            Debug.LogWarning($"[TEST]cant find point > {pt}");
            return false;
        }
        return IsAtPoint(p, dist);
    }

    public bool IsAtPoint(Point p, float dist = 0.1f)
    {
        if (p == null)
        {
            Debug.LogWarning($"[TEST]point is null");
            return false;
        }

        float d = Vector2.Distance(p.pos, pos);
        return d <= dist;
    }

    public void TickMove(float dt)
    {
        if (moveToPoint == null)
            return;

        if (curAtPoint != null && IsAtPoint(curAtPoint) == false)
        {
            Debug.Log($"[TEST]{name} leave point > {curAtPoint.ptype}");
            curAtPoint = null;
        }
    
        Vector2 dir = (moveToPoint.pos - pos).normalized;
        pos += dir * walkSpd * dt;
        if (IsAtPoint(moveToPoint))
        {
            Debug.Log($"[TEST]{name} reach to point > {moveToPoint.ptype}");
            curAtPoint = moveToPoint;
            moveToPoint = null;
        }
    }
}
