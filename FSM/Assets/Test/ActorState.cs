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
            if (Time.time >= beginTime + duration)
            {
                
            }
        }
    }

    public class SleepState : State
    { 
    }

    public class WorkState : State
    {

    }

    public class EatToSleepTransition : Transition
    {
        public EatToSleepTransition(State from, State to)
            :base(from, to)
        {
        }
    }

    public class EatToWorkTransition : Transition
    {
        public EatToWorkTransition(State from, State to)
            :base(from, to)
        {
        }
    }

    public class SleepToEatTransition : Transition
    {
        public SleepToEatTransition(State from, State to)
            :base(from, to)
        {
        }
    }

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