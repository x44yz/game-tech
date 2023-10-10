using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    NONE = -1,
    ENERGY = 0,
    HUNGER = 1,
    MONEY = 2,
    COUNT,
}

public class Agent : MonoBehaviour, IAgentAIOwner
{
    public AgentAI ai;
    public AgentContext context;

    [Header("RUNTIME")]
    public float[] stats;
    public Action curAction;

    private void Start()
    {
        stats = new float[(int)Stat.COUNT];
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        ai.Tick(context, dt);
        curAction = ai.CurAction;
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
}
