using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;
using QuickDemo.FSM;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Actor : MonoBehaviour, IStateMachineOwner
{
    [Header("RUNTIME")]
    public ActorCfg cfg;
    public ActorData data;
    public StateMachine<Actor> fsm;
    public bool debugFSM;

    public bool IsFSMDebug => debugFSM;
    public string FSMDebugLogPrefix => name;

    public virtual float moveSpeed { get { return cfg.walkSpeed; } }

    public static Actor Create(int actorId)
    {
        var data = GameData.Inst.NewActor(actorId);
        return Actor.Create(data);
    }

    public static Actor Create(ActorData actorData)
    {
        var cfg = GameConfig.Inst.GetActor(actorData.id);
        var obj = AssetMgr.InstGameObject(cfg.asset);
        if (obj != null)
        {
            Actor at = obj.GetOrAddComponent<Actor>();
            at.cfg = cfg;
            at.data = actorData;
            at.name = "Actor_" + actorData.uid;
            return at;
        };
        Debug.LogError("[ACTOR]failed create actor > " + cfg.id + "-" + cfg.asset);
        return null;
    }

    protected virtual void Awake()
    {
        ASIdle idle = new ASIdle(this);

        fsm = new StateMachine<Actor>(this);

        fsm.Register(idle);

        // fsm.AddTransition(new Transition(idle, run, idle.IsTranslateToRun));

        fsm.Translate(typeof(ASIdle));
    }

    protected virtual void Update(float dt)
    {
        fsm.Update(dt);
    }
}

public class ActorState : State
{
    protected Actor owner;
    public GameConfig gCfgs { get { return GameConfig.Inst; } }

    public ActorState(Actor owner)
    {
        this.owner = owner;
    }

    public override void OnUpdate(float dt)
    {
    }
}

public class ASIdle : ActorState
{
    public ASIdle(Actor owner) : base(owner)
    {
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("----------------");
        
        Actor at = target as Actor;
        if (at != null && at.fsm != null && at.fsm.curState != null)
        {
            EditorGUILayout.LabelField("State", at.fsm.curState.GetType().ToString());
        }
        else
        {
            EditorGUILayout.LabelField("State", "None");
        }
    }
}
#endif