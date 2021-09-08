using UnityEngine;
using AI.FSM;

namespace Test
{
    public class EatState : State
    {
        public float beginTime;
        public float duration;

        public override void OnEnter()
        {
            beginTime = Time.time;
            duration = TestDef.RealHourToGameSeconds(0.5f);
        }

        public override void OnUpdate(float dt)
        {
        }

        public bool IsEatDone
        {
            get
            {
                return Time.time >= beginTime + duration;
            }
        }
    }

    public class SleepState : State
    { 
    }

    public class WorkState : State
    {

    }

    // public class EatToSleepTransition : Transition
    // {
    //     public Actor owner;

    //     public EatToSleepTransition(State from, State to)
    //         :base(from, to)
    //     {
    //     }

    //     public override bool IsValid() 
    //     {
    //         return owner.Hungry > 0 && owner.Fatigue > 1;
    //     }
    // }

    // public class EatToWorkTransition : Transition
    // {
    //     public EatToWorkTransition(SleepState from, EatState to)
    //         :base(from, to)
    //     {
    //     }
    // }

    // public class SleepToEatTransition : Transition
    // {
    //     public Actor owner;

    //     public SleepToEatTransition(State from, State to)
    //         :base(from, to)
    //     {
    //     }

    //     public override bool IsValid() 
    //     {
    //         return owner.Hungry > 0 && owner.Fatigue > 1;
    //     }
    // }

    // public class SleepToWorkTransition : Transition
    // {

    // }

    // public class WorkToEatTransition : Transition
    // {

    // }

    // public class WorkToSleepTransition : Transition
    // {
        
    // }
}