using UnityEngine;
using AI.FSM;

namespace Test
{
    public class ActorState : State
    {
        public Actor actor;
        public float beginTime;

        public ActorState(Actor actor)
        {
            this.actor = actor;
        }

        public override void OnEnter()
        {
            beginTime = Time.time;
        }

        public override void OnUpdate(float dt)
        {
        }
    }

    [FSMStateClass("ActorFSM")]
    public class EatState : ActorState
    {
        public EatState(Actor actor) : base(actor)
        {

        }

        public override void OnEnter()
        {
            Debug.Log("xx-- EatState.OnEnter");
            actor.targetPT = PointType.Eat;
            actor.hungryRate += actor.eatHungrySupply;
        }

        public override void OnExit()
        {
            Debug.Log("xx-- EatState.OnExit");
            actor.hungryRate -= actor.eatHungrySupply;
        }

        public override void OnUpdate(float dt)
        {
            // actor.hunger += actor.eatRate * dt;
        }
    }

    [FSMStateClass("ActorFSM")]
    public class SleepState : ActorState
    { 
        public SleepState(Actor actor) : base(actor)
        {
            
        }

        public override void OnEnter()
        {
            Debug.Log("xx-- SleepState.OnEnter");
            actor.targetPT = PointType.Sleep;
            actor.energyRate += actor.sleepEnergySupply;
            actor.hungryRate *= actor.sleepHungrySupplyMulti;
        }

        public override void OnExit()
        {
            Debug.Log("xx-- SleepState.OnExit");
            actor.energyRate -= actor.sleepEnergySupply;
            actor.hungryRate /= actor.sleepHungrySupplyMulti;
        }

    }

    [FSMStateClass("ActorFSM")]
    public class WorkState : ActorState
    {
        public WorkState(Actor actor) : base(actor)
        {
            
        }

        public override void OnEnter()
        {
            Debug.Log("xx-- WorkState.OnEnter");
            actor.targetPT = PointType.Work;
            actor.hungryRate += actor.workHungryDrain;
            actor.energyRate += actor.workEnergyDrain;
        }

        public override void OnExit()
        {
            Debug.Log("xx-- WorkState.OnExit");
            actor.hungryRate -= actor.workHungryDrain;
            actor.energyRate -= actor.workEnergyDrain;
        }
    }

    [FSMStateClass("ActorFSM")]
    public class IdleState : ActorState
    {
        public IdleState(Actor actor) : base(actor)
        {
            
        }

        public override void OnEnter()
        {
            Debug.Log("xx-- IdleState.OnEnter");
            actor.energyRate += actor.idleEnergySupply;
        }

        public override void OnExit()
        {
            Debug.Log("xx-- IdleState.OnExit");
            actor.energyRate -= actor.idleEnergySupply;
        }
    }

    [FSMStateClass("ActorFSM")]
    public class WalkState : ActorState
    {
        public Vector3 dir;

        public WalkState(Actor actor) : base(actor)
        {
            
        }

        public override void OnEnter()
        {
            Debug.Log("xx-- WalkState.OnEnter");
            Debug.Assert(actor.targetPT != PointType.None);
            Point pt = actor.GetPoint(actor.targetPT);
            Vector3 dist = pt.transform.position - actor.transform.position;
            dist.y = 0f;
            dir = dist.normalized;

            actor.energyRate += actor.walkEnergyDrain;
        }

        public override void OnExit()
        {
            Debug.Log("xx-- WalkState.OnExit");
            actor.energyRate -= actor.walkEnergyDrain;
        }

        public override void OnUpdate(float dt)
        {
            actor.transform.position += dir * actor.walkSpeed * dt;
        }
    }
}