using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using QuickDemo;
using QuickDemo.FSM;

// public class Monster : Actor
// {
    // [Header("RUNTIME")]
    // public int uid;
    // public MonsterCfg cfg;
    // public StateMachine<Monster> fsm;

    // public override float moveSpeed { get { return cfg.walkSpeed; } }

    // public static Monster Create(int monsterId)
    // {
    //     var cfg = GameConfig.Inst.GetMonster(monsterId);
    //     var obj = AssetMgr.InstGameObject(cfg.asset);
    //     if (obj != null)
    //     {
    //         Monster mt = obj.GetOrAddComponent<Monster>();
    //         mt.uid = GameData.NewUid();
    //         mt.cfg = cfg;
    //         mt.name = "Monster_" + mt.uid;
    //         return mt;
    //     };
    //     Debug.LogError("[MONSTER]failed create monster > " + cfg.id + "-" + cfg.asset);
    //     return null;
    // }

    // protected override void OnInit()
    // {
    //     MSIdle idle = new MSIdle(this);

    //     fsm = new StateMachine<Monster>(this);

    //     fsm.Register(idle);

    //     // fsm.AddTransition(new Transition(idle, run, idle.IsTranslateToRun));

    //     fsm.Translate(typeof(MSIdle));
    // }

    // protected override void OnUpdate(float dt)
    // {
    //     fsm.Update(dt);
    // }
// }

// public class MonsterState : State
// {
//     protected Monster owner;
//     public GameConfig gCfgs { get { return GameConfig.Inst; } }

//     public MonsterState(Monster owner)
//     {
//         this.owner = owner;
//     }

//     public override void OnUpdate(float dt)
//     {
//     }
// }

// public class MSIdle : MonsterState
// {
//     public MSIdle(Monster owner) : base(owner)
//     {
//     }

//     public override void OnUpdate(float dt)
//     {
//         base.OnUpdate(dt);
//     }
// }
