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
        public WalkState(Actor actor) : base(actor)
        {
            
        }
    }
}