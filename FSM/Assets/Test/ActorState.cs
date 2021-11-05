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

    [FSMAttrStateClass("ActorFSM")]
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

        [FSMAttrTransitionMethod("ActorFSM")]
        public bool IsTranslateToIdle()
        {
            return actor.hunger >= 20;
        }
    }

    [FSMAttrStateClass("ActorFSM")]
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

        [FSMAttrTransitionMethod("ActorFSM")]
        public bool IsTranslateToIdle()
        {
            return actor.energy > 10f;
        }
    }

    [FSMAttrStateClass("ActorFSM")]
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

        [FSMAttrTransitionMethod("ActorFSM")]
        public bool IsTranslateToIdle()
        {
            return actor.hunger < 0.1f || actor.energy < 0.5f;
        }
    }

    [FSMAttrStateClass("ActorFSM")]
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

        // Transition
        [FSMAttrTransitionMethod("ActorFSM")]
        public bool IsTranslateToEat()
        {
            return actor.hunger < 0.1f;
        }

        [FSMAttrTransitionMethod("ActorFSM")]
        public bool IsTranslateToSleep()
        {
            return actor.energy < 1f;
        }

        [FSMAttrTransitionMethod("ActorFSM")]
        public bool IsTranslateToWork()
        {
            return actor.hunger > 5f && actor.energy > 5f;
        }
    }

    [FSMAttrStateClass("ActorFSM")]
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

        [FSMAttrTransitionMethod("ActorFSM")]
        public bool IsTranslateToIdle()
        {
            var pt = actor.GetPoint(actor.targetPT);
            var dist = actor.transform.position - pt.transform.position;
            dist.y = 0f;
            return dist.magnitude <= Actor.POINT_STOP_DIST;
        }
    }
}