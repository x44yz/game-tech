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

    public class EatState : ActorState
    {
        public EatState(Actor actor) : base(actor)
        {

        }

        public override void OnEnter()
        {
            Debug.Log("xx-- EatState.OnEnter");
            actor.targetPT = PointType.Eat;
        }

        public override void OnUpdate(float dt)
        {
            actor.hunger += actor.eatRate * dt;
        }
    }

    public class SleepState : ActorState
    { 
        public SleepState(Actor actor) : base(actor)
        {
            
        }
    }

    public class WorkState : ActorState
    {
        public WorkState(Actor actor) : base(actor)
        {
            
        }

     
    }

    public class IdleState : ActorState
    {
        public IdleState(Actor actor) : base(actor)
        {
            
        }
    }

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
        }

        public override void OnUpdate(float dt)
        {
            actor.transform.position += dir * actor.walkSpeed * dt;
        }
    }
}