using System.Collections;
using System.Collections.Generic;

namespace AI.FSM
{
    public class StateMachine
    {
        public State curState;
        public Dictionary<State, Dictionary<Transition, State>> transitions = new Dictionary<State, Dictionary<Transition, State>>();

        public void Update(float dt)
        {
            if (curState == null)
                return;

            if (transitions.TryGetValue(curState, out Dictionary<Transition, State> ts))
            {
                foreach (var kv in ts)
                {
                    if (kv.Key.IsValid())
                    {
                        SetState(kv.Value);
                        break;
                    }
                }
            }

            curState.OnUpdate(dt);
        }

        public void SetState(State st)
        {
            curState = st;
        }

        public void AddTransition()
        {
            
        }
    }
}